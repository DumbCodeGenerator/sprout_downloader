﻿using System.Text.Json.Serialization;

namespace Sprout_Downloader.Json
{
    public class SproutData
    {
        [JsonPropertyName("title")] public string Title { get; set; }

        [JsonPropertyName("duration")] public double Duration { get; set; }

        [JsonPropertyName("sessionID")] public string SessionId { get; set; }

        [JsonPropertyName("s3_user_hash")] public string UserHash { get; set; }

        [JsonPropertyName("s3_video_hash")] public string VideoHash { get; set; }

        [JsonPropertyName("signatures")] public Dictionary<string, Signature> Signatures { get; set; }

        public string SignUrl(string url)
        {
            if (url.EndsWith(".m3u8"))
                return url + Signatures["m"] + SessionId;
            if (url.EndsWith(".key"))
                return url + Signatures["k"] + SessionId;
            if (url.EndsWith(".ts"))
                return url + Signatures["t"] + SessionId;
            return "Wrong url type!";
        }

        public string GetBaseUrl()
        {
            return "https://hls2.videos.sproutvideo.com/" + UserHash + '/' + VideoHash + "/video/";
        }

        public string GetIndexUrl()
        {
            return SignUrl(GetBaseUrl() + "index.m3u8");
        }
    }

    public class Signature
    {
        [JsonPropertyName("CloudFront-Policy")] public string Policy { get; set; }

        [JsonPropertyName("CloudFront-Signature")] public string SignatureValue { get; set; }

        [JsonPropertyName("CloudFront-Key-Pair-Id")]
        public string KeyIdPair { get; set; }

        public override string ToString()
        {
            return "?Policy=" + Policy + "&Signature=" + SignatureValue + "&Key-Pair-Id=" + KeyIdPair +
                   "&sessionID=";
        }
    }
}