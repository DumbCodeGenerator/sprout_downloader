using System.Drawing.Text;
using System.Windows.Forms;

namespace Sprout_Downloader
{
    public class AntiAliasingLabel : Label
    {
        public TextRenderingHint TextRenderingHint { get; set; } = TextRenderingHint.SystemDefault;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint;
            base.OnPaint(e);
        }
    }
}