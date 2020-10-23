using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Sprout_Downloader
{
    public class Downloader
    {
        private readonly BlockingCollection<string> _files = new BlockingCollection<string>();
        private readonly ConcurrentDictionary<object, Status> _progresses = new ConcurrentDictionary<object, Status>();
        private readonly BlockingCollection<Segment> _queue = new BlockingCollection<Segment>();
        private readonly int _threadCount = 4;
        private int _current;
        private long _fullSize;
        private Action<IEnumerable<Status>, int, int, long> _progressCallback;
        private int _total;

        public async void Start(M3UParser parser, Action<IEnumerable<Status>, int, int, long> progressCallback,
            Action<string[], string, Key> finishCallback)
        {
            var playlist = parser.GetPlaylistParser();
            _fullSize = parser.GetInaccurateVideoSize();
            _total = playlist.GetSegmentsCount();
            _progressCallback = progressCallback;

            var workers = new Task[_threadCount];

            for (var i = 0; i < _threadCount; ++i)
            {
                var task = new Task(Download);
                workers[i] = task;
                task.Start();
            }

            foreach (var segment in playlist.GetSegments()) _queue.Add(segment);
            _queue.CompleteAdding();

            await Task.WhenAll(workers);

            _files.CompleteAdding();
            var files = _files.ToArray();
            Array.Sort(files);
            finishCallback(files, parser.GetVideoTitle(), playlist.GetKey());
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!_progresses.ContainsKey(e.UserState))
                _progresses.TryAdd(e.UserState,
                    new Status {BytesReceived = e.BytesReceived, TotalBytes = e.TotalBytesToReceive});
            else
                _progresses[e.UserState].BytesReceived = e.BytesReceived;

            _progressCallback(_progresses.Select(x => x.Value), _current, _total, _fullSize);
        }

        private void DownloadCompleteCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled) return;
            lock (e.UserState)
            {
                Interlocked.Increment(ref _current);
                Monitor.Pulse(e.UserState);
            }
        }

        private void Download()
        {
            using (var wc = new WebClient())
            {
                wc.DownloadProgressChanged += DownloadProgressCallback;
                wc.DownloadFileCompleted += DownloadCompleteCallback;

                foreach (var segment in _queue.GetConsumingEnumerable())
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(segment.Filename) ??
                                                  throw new InvalidOperationException());
                        _files.Add(segment.Filename);
                        lock (segment.Filename)
                        {
                            wc.DownloadFileAsync(new Uri(segment.Url), segment.Filename, segment.Filename);
                            Monitor.Wait(segment.Filename);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
            }
        }

        public class Status
        {
            public long BytesReceived { get; set; }
            public long TotalBytes { get; set; }
        }
    }
}