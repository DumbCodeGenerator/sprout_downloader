using System.Drawing.Text;

namespace Sprout_Downloader.UI
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