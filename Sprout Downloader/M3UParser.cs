using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Sprout_Downloader
{
    public class M3UParser
    {
        private readonly SproutData _dataObj;

        private readonly Lazy<IEnumerable<Playlist>> _lazyPlaylists;
        private PlaylistParser _playlistParser;

        public M3UParser(string input, SproutData dataObj)
        {
            _dataObj = dataObj;

            _lazyPlaylists = new Lazy<IEnumerable<Playlist>>(() =>
            {
                var matchList = Regex.Matches(input,
                    @"^#EXT-X-STREAM-INF:.*?BANDWIDTH=(.*?),RESOLUTION.*$\n^(.*?\.m3u8)$", RegexOptions.Multiline);
                return matchList.Cast<Match>().Select(match => new Playlist
                {
                    Quality = match.Groups[2].Value.Replace(".m3u8", "p"),
                    Url = _dataObj.SignUrl(_dataObj.GetBaseUrl() + match.Groups[2].Value),
                    InaccurateSize = long.Parse(match.Groups[1].Value) / 8 * (long) _dataObj.Duration
                });
            });
        }

        public int Index { get; set; }

        private IEnumerable<Playlist> GetPlaylists()
        {
            return _lazyPlaylists.Value;
        }

        public IEnumerable<string> GetQualityList()
        {
            return GetPlaylists().Select(x => x.Quality);
        }

        public string GetPlaylistUrl()
        {
            return GetPlaylists().ElementAt(Index).Url;
        }

        public long GetInaccurateVideoSize()
        {
            return GetPlaylists().ElementAt(Index).InaccurateSize;
        }

        public void SetPlaylistString(string input)
        {
            _playlistParser = new PlaylistParser(input, _dataObj);
        }

        public PlaylistParser GetPlaylistParser()
        {
            return _playlistParser;
        }

        public string GetVideoTitle()
        {
            return _dataObj.Title;
        }
    }

    public class PlaylistParser
    {
        private readonly Lazy<Key> _lazyKey;
        private readonly Lazy<IEnumerable<Segment>> _lazySegments;

        public PlaylistParser(string m3UString, SproutData dataObj)
        {
            _lazyKey = new Lazy<Key>(() =>
            {
                var match = Regex.Match(m3UString, "#EXT-X-KEY:.*?URI=\"(.*?)\".*?IV=0x(.*?)$", RegexOptions.Multiline);
                using (var wc = new WebClient())
                {
                    var bytes = wc.DownloadData(dataObj.SignUrl(dataObj.GetBaseUrl() + match.Groups[1].Value));
                    return new Key
                    {
                        Iv = Utils.StringToByteArrayFastest(match.Groups[2].Value),
                        Bytes = bytes
                    };
                }
            });

            _lazySegments = new Lazy<IEnumerable<Segment>>(() =>
            {
                var matchList = Regex.Matches(m3UString, "^.*?\\.ts$", RegexOptions.Multiline);
                return matchList.Cast<Match>().Select(match => new Segment
                {
                    Filename = dataObj.Title + "/" + match.Value,
                    Url = dataObj.SignUrl(dataObj.GetBaseUrl() + match.Value)
                });
            });
        }

        public Key GetKey()
        {
            return _lazyKey.Value;
        }

        public IEnumerable<Segment> GetSegments()
        {
            return _lazySegments.Value;
        }

        public int GetSegmentsCount()
        {
            return GetSegments().Count();
        }
    }

    public class Playlist
    {
        public string Quality { get; set; }
        public long InaccurateSize { get; set; }
        public string Url { get; set; }
    }

    public class Key
    {
        public byte[] Bytes { get; set; }
        public byte[] Iv { get; set; }
    }

    public class Segment
    {
        public string Filename { get; set; }
        public string Url { get; set; }
    }
}