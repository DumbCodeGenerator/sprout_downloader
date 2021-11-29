using Sprout_Downloader.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;

namespace Sprout_Downloader
{
    public class Downloader
    {
        private readonly ConcurrentDictionary<string, Status> _progresses = new();
        private int _current;
        private long _fullSize;
        private Action<ICollection<Status>, int, int, long> _progressCallback;
        private int _total;

        public async Task Start(M3UParser parser, Action<ICollection<Status>, int, int, long> progressCallback,
            Action<M3UParser> finishCallback)
        {
            PlaylistParser playlist = parser.GetPlaylistParser();
            _fullSize = parser.GetInaccurateVideoSize();
            _total = playlist.GetSegmentsCount();
            _progressCallback = progressCallback;

            ActionBlock<Segment> workers = new ActionBlock<Segment>(segment =>
            {
                try
                {
                    Download(segment).Wait(Program.GetCancellationToken());
                }
                catch (Exception)
                {
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = Properties.Settings.Default.threadCount });

            playlist.GetSegments().ForEach(x => workers.Post(x));
            workers.Complete();
            await workers.Completion;

            finishCallback(parser);
        }

        private void DownloadProgressCallback(string filename, Status status)
        {
            _progresses.AddOrUpdate(filename, status, (key, oldValue) => status);

            _progressCallback(_progresses.Values, _current, _total, _fullSize);
        }

        private void DownloadCompleteCallback()
        {
            Interlocked.Increment(ref _current);
        }

        private async Task Download(Segment segment)
        {
            using HttpClient hc = new();
            using HttpResponseMessage response = await hc.GetAsync(segment.Url, Program.GetCancellationToken());
            response.EnsureSuccessStatusCode();
            long fileSize = response.Content.Headers.ContentLength ?? -1L;
            int bufferSize = 4 * 1024 * 1024;

            Directory.CreateDirectory(segment.Folder);

            using Stream content = await response.Content.ReadAsStreamAsync(Program.GetCancellationToken());
            using FileStream fileStream = new FileStream(segment.GetFullPath(), FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true);

            long totalRead = 0L;
            byte[] buffer = new byte[bufferSize];
            bool isMoreToRead = true;

            do
            {
                int read = await content.ReadAsync(buffer.AsMemory(0, buffer.Length), Program.GetCancellationToken());
                if (read == 0)
                {
                    isMoreToRead = false;
                    DownloadCompleteCallback();
                }
                else
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, read), Program.GetCancellationToken());

                    totalRead += read;
                    DownloadProgressCallback(segment.GetFullPath(), new Status { BytesReceived = totalRead, TotalBytes = fileSize });
                }
            }
            while (isMoreToRead);
        }

        public class Status
        {
            public long BytesReceived { get; set; }
            public long TotalBytes { get; set; }
        }
    }
}