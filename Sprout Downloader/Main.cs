using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Sprout_Downloader
{
    public partial class Main : Form
    {
        private const int OrigHeight = 205;
        private const int InfoHeight = 260;
        private const int FullHeight = 363;

        private static readonly string[] SizeSuffixes =
            {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

        private static readonly CookieContainer Cookies = new CookieContainer();

        private static readonly HttpClient
            WebClient = new HttpClient(new HttpClientHandler {CookieContainer = Cookies});

        private string _videoUrl;


        public Main()
        {
            InitializeComponent();
        }

        private static bool CheckUrlValid(string source)
        {
            return Uri.TryCreate(source, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ToggleButton(false);

            downloadProgress.CustomText = string.Empty;

            _videoUrl = urlTextBox.Text;
            if (!_videoUrl.StartsWith("http://") && !_videoUrl.StartsWith("https://"))
                _videoUrl = "https://" + _videoUrl;

            Height = OrigHeight;
            if (!CheckUrlValid(_videoUrl))
            {
                ShowError("Invalid URL!");
                return;
            }

            Height = FullHeight;
            SetAction("Getting link...");
            downloadProgress.Value = 0;

            using (var response = await WebClient.GetAsync(_videoUrl))
            {
                if (response == null)
                {
                    ShowError("Empty response!");
                    return;
                }

                var responseString = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK && responseString.Contains("Password Protected Video",
                    StringComparison.OrdinalIgnoreCase))
                {
                    var authToken = Regex.Match(responseString, "name='authenticity_token' value='(.*?)'").Groups[1]
                        .Value;

                    if (string.IsNullOrWhiteSpace(authToken))
                        ShowError("Can't get auth token from the page");
                    else
                        PasswordProcessing(authToken);
                }
                else if (response.StatusCode != HttpStatusCode.OK)
                {
                    ShowError($"Can't get the link. Status Code: {response.StatusCode}");
                }
                else
                {
                    DownloadVideo(responseString);
                }
            }
        }

        private async void PasswordProcessing(string authToken)
        {
            using (var passDialog = new PasswordInput())
            {
                if (passDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var values = new Dictionary<string, string>
                    {
                        {"password", passDialog.GetPassword()},
                        {"authenticity_token", authToken},
                        {"_method", "put"}
                    };

                    var content = new FormUrlEncodedContent(values);

                    using (var response = await WebClient.PostAsync(_videoUrl, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var embeddedUrl = Regex.Match(responseString, "<iframe src='(.*?)'").Groups[1].Value;
                            if (!string.IsNullOrWhiteSpace(embeddedUrl))
                                DownloadVideo(await WebClient.GetStringAsync(embeddedUrl));
                            else
                                ShowError("Didn't find embedded video url");
                        }
                        else
                        {
                            ShowError($"Can't log in. Probably wrong password. Status Code: {response.StatusCode}");
                        }
                    }
                }
                else
                {
                    ShowError("Password input cancelled");
                }
            }
        }

        private async void DownloadVideo(string response)
        {
            SetAction("Getting info from index...");
            var data = Regex.Match(response, "var dat = '(.*?)'").Groups[1].Value;
            data = Encoding.UTF8.GetString(Convert.FromBase64String(data));

            var dataObj = JsonConvert.DeserializeObject<SproutData>(data);

            if (dataObj != null)
            {
                var m3U8String = await WebClient.GetStringAsync(dataObj.GetIndexUrl());
                var m3U8Obj = new M3UParser(m3U8String, dataObj);
                using (var selector = new QualitySelector(m3U8Obj.GetQualityList()))
                {
                    if (selector.ShowDialog(this) == DialogResult.OK)
                    {
                        SetAction("Getting segments...");
                        m3U8Obj.Index = selector.GetSelectedIndex();
                        var playlistUrl = m3U8Obj.GetPlaylistUrl();
                        m3U8String = await WebClient.GetStringAsync(playlistUrl);

                        m3U8Obj.SetPlaylistString(m3U8String);

                        SetAction($"Downloading segments: 0/{m3U8Obj.GetPlaylistParser().GetSegmentsCount()}");

                        new Downloader().Start(m3U8Obj,
                            ProgressCallback,
                            FinishCallback);
                    }
                    else
                    {
                        ShowError("Quality Selector cancelled");
                    }
                }
            }
            else
            {
                ShowError("Couldn't find any data for video");
            }
        }

        private void ProgressCallback(IEnumerable<Downloader.Status> statuses, int currentSegment, int totalSegments,
            long fullSize)
        {
            var enumerable = statuses as Downloader.Status[] ?? statuses.ToArray();

            var total = enumerable.Sum(x => x.TotalBytes);
            var received = enumerable.Sum(x => x.BytesReceived);

            var notAccurate = fullSize > total;

            total = notAccurate ? fullSize : total;

            var percentage = (int) Math.Round((double) (100 * received) / total);

            SetAction($"Downloading segments: {currentSegment}/{totalSegments}");

            downloadProgress.SetPropertyThreadSafe(() => downloadProgress.CustomText,
                $"{SizeSuffix(received)}/{(notAccurate ? "~" : string.Empty) + SizeSuffix(total)}");
            downloadProgress.SetPropertyThreadSafe(() => downloadProgress.Value, percentage);
        }

        private static string SizeSuffix(long value, int decimalPlaces = 0)
        {
            if (decimalPlaces < 0) throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            if (value < 0) return "-" + SizeSuffix(-value);
            if (value == 0) return string.Format("{0:n" + decimalPlaces + "} bytes", 0);

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            var mag = (int) Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            var adjustedSize = (decimal) value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        private async void FinishCallback(string[] files, string title, Key key)
        {
            SetAction("Decrypt and concat segments into one file...");
            await Task.Run(() =>
            {
                using (var mainFile = File.OpenWrite(title + ".ts"))
                {
                    foreach (var file in files)
                    {
                        using (var segment = File.OpenRead(file))
                        {
                            Utils.Decrypt(mainFile, segment, key);
                        }

                        File.Delete(file);
                    }
                }
            });
            Directory.Delete(title);
            SetAction("Convert to MP4...");
            Utils.ConvertToMp4(title, () =>
            {
                SetAction("Video downloaded!", Color.Green);
                this.SetPropertyThreadSafe(() => Height, InfoHeight);
                ToggleButton(true);
            });
        }

        private void SetAction(string action, Color? color = null)
        {
            actionLabel.SetPropertyThreadSafe(() => actionLabel.ForeColor, color ?? Color.Black);
            actionLabel.SetPropertyThreadSafe(() => actionLabel.Text, action);
        }

        private void ToggleButton(bool enable)
        {
            button1.SetPropertyThreadSafe(() => button1.Enabled, enable);
        }

        private void ShowError(string text)
        {
            Height = InfoHeight;
            actionLabel.ForeColor = Color.Red;
            actionLabel.Text = text;
            ToggleButton(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Height = OrigHeight;
        }
    }
}