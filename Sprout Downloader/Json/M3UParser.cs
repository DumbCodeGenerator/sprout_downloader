using Sprout_Downloader.Util;
using System.Text.RegularExpressions;

namespace Sprout_Downloader.Json
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
                MatchCollection matchList = Regex.Matches(input,
                    @"^#EXT-X-STREAM-INF:.*?BANDWIDTH=(.*?),RESOLUTION.*$\n^(.*?\.m3u8)$", RegexOptions.Multiline);
                return matchList.Cast<Match>().Select(match => new Playlist
                {
                    Quality = match.Groups[2].Value.Replace(".m3u8", "p"),
                    Url = _dataObj.SignUrl(_dataObj.GetBaseUrl() + match.Groups[2].Value),
                    InaccurateSize = long.Parse(match.Groups[1].Value) / 8 * (long)_dataObj.Duration
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

        private int GetBestQuality()
        {
            return GetPlaylists().Count() - 1;
        }

        public string GetPlaylistUrl()
        {
            return GetPlaylists().ElementAt(Properties.Settings.Default.bestQuality ? GetBestQuality() : Index).Url;
        }

        public long GetInaccurateVideoSize()
        {
            return GetPlaylists().ElementAt(Properties.Settings.Default.bestQuality ? GetBestQuality() : Index).InaccurateSize;
        }

        public void SetPlaylistString(string input)
        {
            _playlistParser = new PlaylistParser(input, _dataObj, this);
        }

        public PlaylistParser GetPlaylistParser()
        {
            return _playlistParser;
        }

        public string GetVideoTitle()
        {
            return Path.GetFileNameWithoutExtension(_dataObj.Title);
        }

        public string GetSegmentsFolder()
        {
            return "segments_" + GetVideoTitle();
        }
    }

    public class PlaylistParser
    {
        private readonly Lazy<Key> _lazyKey;
        private readonly Lazy<List<Segment>> _lazySegments;

        public PlaylistParser(string m3UString, SproutData dataObj, M3UParser parser)
        {
            _lazyKey = new Lazy<Key>(() =>
            {
                Match match = Regex.Match(m3UString, "#EXT-X-KEY:.*?URI=\"(.*?)\".*?IV=0x(.*?)$", RegexOptions.Multiline);
                using HttpClient hc = new();
                byte[] bytes = hc.GetByteArrayAsync(dataObj.SignUrl(dataObj.GetBaseUrl() + match.Groups[1].Value)).Result;
                return new Key
                {
                    Iv = Utils.StringToByteArrayFastest(match.Groups[2].Value),
                    Bytes = bytes
                };
            });

            _lazySegments = new Lazy<List<Segment>>(() =>
            {
                MatchCollection matchList = Regex.Matches(m3UString, "^.*?\\.ts$", RegexOptions.Multiline);
                return matchList.Cast<Match>().Select(match => new Segment
                {
                    Folder = parser.GetSegmentsFolder(),
                    Filename = match.Value,
                    Url = dataObj.SignUrl(dataObj.GetBaseUrl() + match.Value)
                }).ToList();
            });
        }

        public Key GetKey()
        {
            return _lazyKey.Value;
        }

        public List<Segment> GetSegments()
        {
            return _lazySegments.Value;
        }

        public int GetSegmentsCount()
        {
            return GetSegments().Count;
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
        public string Folder { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }

        public string GetFullPath()
        {
            return Folder + "/" + Filename;
        }
    }
}