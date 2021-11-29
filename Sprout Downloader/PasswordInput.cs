using System.Diagnostics;

namespace Sprout_Downloader
{
    public partial class PasswordInput : Form
    {
        private readonly string _videoUrl;

        public PasswordInput(string videoUrl)
        {
            _videoUrl = videoUrl;
            InitializeComponent();
        }

        public string GetPassword()
        {
            return textBox1.Text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(_videoUrl);
        }
    }
}