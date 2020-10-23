using System.Windows.Forms;

namespace Sprout_Downloader
{
    public partial class PasswordInput : Form
    {
        public PasswordInput()
        {
            InitializeComponent();
        }

        public string GetPassword()
        {
            return textBox1.Text;
        }
    }
}