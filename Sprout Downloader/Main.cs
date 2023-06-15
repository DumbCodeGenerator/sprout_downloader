using Sprout_Downloader.Json;
using Sprout_Downloader.Util;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sprout_Downloader
{
    public partial class Main : Form
    {
        private const int OrigHeight = 492;
        private const int InfoHeight = 565;
        private const int FullHeight = 665;

        private static readonly string[] SizeSuffixes =
            {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

        private static readonly string _totalProgTemp = "Downloaded videos: %current/%total";

        private int _currentProg = 0;

        private static readonly CookieContainer Cookies = new();

        private static readonly HttpClient
            WebClient = new(new HttpClientHandler { CookieContainer = Cookies });

        private List<ParsedURL> _videoUrls;

        private bool _stopButton = false;


        public Main()
        {
            InitializeComponent();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            if (_stopButton)
            {
                Program.Cancel();
                ResetState();
                return;
            }

            Program.RefreshCTS();

            SetStop(true);

            downloadProgress.CustomText = string.Empty;

            _videoUrls = urlTextBox.ParseURL();

            Height = OrigHeight;
            if (!_videoUrls.Any())
            {
                ShowError("Couldn't parse not a single URL");
                return;
            }

            Height = FullHeight;
            UpdateTotalProg();

            foreach (ParsedURL videoUrl in _videoUrls)
            {
                if (Program.IsCancellationRequested())
                    break;

                SetAction($"Getting link #{_currentProg + 1}...");
                downloadProgress.Value = 0;
                await ProcessLink(videoUrl);
            }
        }

        private async Task ProcessLink(ParsedURL item)
        {
            if (Program.IsCancellationRequested())
                return;

            using HttpResponseMessage response = await WebClient.GetAsync(item.URL, Program.GetCancellationToken());
            if (response == null)
            {
                ShowError("Empty response!");
                return;
            }

            string responseString = await response.Content.ReadAsStringAsync(Program.GetCancellationToken());

            if (response.StatusCode != HttpStatusCode.OK && responseString.Contains("Password Protected Video",
                StringComparison.OrdinalIgnoreCase))
            {
                string authToken = Regex.Match(responseString, "name='authenticity_token' value='(.*?)'").Groups[1]
                    .Value;

                if (string.IsNullOrWhiteSpace(authToken))
                    ShowError("Can't get auth token from the page");
                else
                    await CheckIfNeedInput(authToken, item);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                ShowError($"Can't get the link. Status Code: {response.StatusCode}");
            }
            else
            {
                await DownloadVideo(responseString, item);
            }
        }

        private async Task CheckIfNeedInput(string authToken, ParsedURL item)
        {
            if (Program.IsCancellationRequested())
                return;

            if (item.Password == null)
            {
                using PasswordInput passDialog = new(item.URL);
                if (passDialog.ShowDialog(this) == DialogResult.OK)
                    await PutPassword(authToken, item.SetPassword(passDialog.GetPassword()));
                else
                    ShowError("Password input cancelled");
            }
            else
            {
                await PutPassword(authToken, item);
            }
        }

        private async Task PutPassword(string authToken, ParsedURL item)
        {
            if (Program.IsCancellationRequested())
                return;

            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                        {"password", item.Password},
                        {"authenticity_token", authToken},
                        {"_method", "put"}
                    };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            using HttpResponseMessage response = await WebClient.PostAsync(item.URL, content, Program.GetCancellationToken());

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync(Program.GetCancellationToken());
                string embeddedUrl = Regex.Match(responseString, "<iframe src='(.*?)'").Groups[1].Value;
                if (!string.IsNullOrWhiteSpace(embeddedUrl))
                    await DownloadVideo(await WebClient.GetStringAsync(embeddedUrl, Program.GetCancellationToken()), item);
                else
                    ShowError("Didn't find embedded video url");
            }
            else
            {
                ShowError($"Can't log in. Probably wrong password. Status Code: {response.StatusCode}");
            }
        }

        private async Task DownloadVideo(string response, ParsedURL item)
        {
            SetAction("Getting info from index...");
            string data = Regex.Match(response, "var dat = '(.*?)'").Groups[1].Value;

            if (string.IsNullOrWhiteSpace(data))
            {
                string embeddedUrl = Regex.Match(response, "<iframe src='(.*?)'").Groups[1].Value;

                if (!string.IsNullOrWhiteSpace(embeddedUrl))
                    await ProcessLink(item.SetURL(embeddedUrl));

                return;
            }

            data = Encoding.UTF8.GetString(Convert.FromBase64String(data));

            SproutData dataObj = JsonSerializer.Deserialize(data, JsonContext.Default.SproutData);

            if (dataObj != null)
            {
                string m3U8String = await WebClient.GetStringAsync(dataObj.GetIndexUrl(), Program.GetCancellationToken());
                M3UParser m3U8Obj = new(m3U8String, dataObj);

                if (Properties.Settings.Default.bestQuality)
                {
                    await StartDownload(m3U8Obj);
                }
                else
                {
                    await StartQualitySelector(m3U8Obj);
                }
            }
            else
            {
                ShowError("Couldn't find any data for video");
            }
        }

        private async Task StartQualitySelector(M3UParser m3U8Obj)
        {
            using QualitySelector selector = new(m3U8Obj.GetQualityList());
            if (selector.ShowDialog(this) == DialogResult.OK)
            {
                await StartDownload(m3U8Obj, selector.GetSelectedIndex());
            }
            else
            {
                ShowError("Quality Selector cancelled");
            }
        }

        private async Task StartDownload(M3UParser m3U8Obj, int qualityIndex = 0)
        {
            SetAction("Getting segments...");
            m3U8Obj.Index = qualityIndex;
            string playlistUrl = m3U8Obj.GetPlaylistUrl();
            string m3U8String = await WebClient.GetStringAsync(playlistUrl, Program.GetCancellationToken());

            m3U8Obj.SetPlaylistString(m3U8String);

            SetAction($"Downloading segments: 0/{m3U8Obj.GetPlaylistParser().GetSegmentsCount()}");

            await new Downloader().Start(m3U8Obj,
                ProgressCallback,
                FinishCallback);
        }

        private void ProgressCallback(ICollection<Downloader.Status> statuses, int currentSegment, int totalSegments,
            long fullSize)
        {
            if (Program.IsCancellationRequested())
                return;

            long total = statuses.Sum(x => x.TotalBytes);
            long received = statuses.Sum(x => x.BytesReceived);

            bool notAccurate = fullSize > total;

            total = notAccurate ? fullSize : total;

            int percentage = (int)Math.Round((double)(100 * received) / total);

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
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

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

        private async void FinishCallback(M3UParser parser)
        {
            if (Program.IsCancellationRequested())
                return;

            SetAction("Decrypt and concat segments into one file...");
            string title = parser.GetVideoTitle().Replace("\"", "");;
            Key key = parser.GetPlaylistParser().GetKey();

            await Task.Run(() =>
            {
                using FileStream mainFile = File.OpenWrite(title + ".ts");
                parser.GetPlaylistParser().GetSegments().ForEach(segment =>
                {
                    using FileStream fileSeg = File.OpenRead(segment.GetFullPath());
                    Utils.Decrypt(mainFile, fileSeg, key);
                });
            });
            Directory.Delete(parser.GetSegmentsFolder(), true);
            SetAction("Convert to MP4...");
            Utils.ConvertToMp4(title, () =>
            {
                _currentProg++;
                if (_currentProg == _videoUrls.Count)
                {
                    SetAction("All videos downloaded!", Color.Green);
                    this.SetPropertyThreadSafe(() => Height, InfoHeight);
                    ResetState(false);
                }
                else
                    UpdateTotalProg();
            });
        }

        private void UpdateTotalProg()
        {
            totalProgressLabel.SetPropertyThreadSafe(() => totalProgressLabel.Text, _totalProgTemp.Replace("%current", _currentProg.ToString()).Replace("%total", _videoUrls.Count.ToString()));
        }

        private void ResetState(bool resetHeight = true)
        {
            if (resetHeight)
                this.SetPropertyThreadSafe(() => Height, OrigHeight);

            downloadProgress.SetPropertyThreadSafe(() => downloadProgress.Value, 0);
            downloadProgress.SetPropertyThreadSafe(() => downloadProgress.CustomText, string.Empty);
            _currentProg = 0;
            UpdateTotalProg();
            SetStop(false);
            ToggleButton(true);
        }

        private void SetAction(string action, Color? color = null)
        {
            actionLabel.SetPropertyThreadSafe(() => actionLabel.ForeColor, color ?? Color.Black);
            actionLabel.SetPropertyThreadSafe(() => actionLabel.Text, action);
        }

        private void SetStop(bool state)
        {
            _stopButton = state;
            button1.SetPropertyThreadSafe(() => button1.Text, _stopButton ? "Stop" : "Download");
        }

        private void ToggleButton(bool enable)
        {
            button1.SetPropertyThreadSafe(() => button1.Enabled, enable);
        }

        public void ShowError(string text)
        {
            Height = InfoHeight;
            actionLabel.ForeColor = Color.Red;
            actionLabel.Text = text;
            ToggleButton(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Height = OrigHeight;
            InitSettings();
        }

        private void InitSettings()
        {
            toolStripTextBox1.Text = Properties.Settings.Default.threadCount.ToString();
            ToggleQuality();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(toolStripTextBox1.Text, out int count))
            {
                Properties.Settings.Default.threadCount = count;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void alwaysAskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.bestQuality = false;
            ToggleQuality();
        }

        private void alwaysTheBestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.bestQuality = true;
            ToggleQuality();
        }

        private void ToggleQuality()
        {
            if (Properties.Settings.Default.bestQuality)
            {
                alwaysTheBestToolStripMenuItem.Checked = true;
                if (alwaysAskToolStripMenuItem.Checked)
                {
                    alwaysAskToolStripMenuItem.Checked = false;
                }
            }
            else
            {
                alwaysAskToolStripMenuItem.Checked = true;
                if (alwaysTheBestToolStripMenuItem.Checked)
                {
                    alwaysTheBestToolStripMenuItem.Checked = false;
                }
            }
        }
    }
}
