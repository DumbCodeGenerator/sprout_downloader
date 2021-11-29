using System.ComponentModel;
using System.Drawing.Text;

namespace Sprout_Downloader.UI
{
    public enum ProgressBarDisplayMode
    {
        NoText,
        Percentage,
        CurrProgress,
        CustomText,
        TextAndPercentage,
        TextAndCurrProgress
    }

    public class TextProgressBar : ProgressBar
    {
        private SolidBrush _progressColourBrush = (SolidBrush)Brushes.LightGreen;

        private string _text = string.Empty;

        private SolidBrush _textColourBrush = (SolidBrush)Brushes.Black;

        private ProgressBarDisplayMode _visualMode = ProgressBarDisplayMode.CurrProgress;

        public TextProgressBar()
        {
            Value = Minimum;
            FixComponentBlinking();
        }

        [Description("Font of the text on ProgressBar")]
        [Category("Additional Options")]
        public Font TextFont { get; set; } = new Font(FontFamily.GenericSerif, 11, FontStyle.Bold | FontStyle.Italic);

        [Category("Additional Options")]
        public Color TextColor
        {
            get => _textColourBrush.Color;
            set
            {
                _textColourBrush.Dispose();
                _textColourBrush = new SolidBrush(value);
            }
        }

        [Category("Additional Options")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color ProgressColor
        {
            get => _progressColourBrush.Color;
            set
            {
                _progressColourBrush.Dispose();
                _progressColourBrush = new SolidBrush(value);
            }
        }

        [Category("Additional Options")]
        [Browsable(true)]
        public ProgressBarDisplayMode VisualMode
        {
            get => _visualMode;
            set
            {
                _visualMode = value;
                Invalidate(); //redraw component after change value from VS Properties section
            }
        }

        [Description("If it's empty, % will be shown")]
        [Category("Additional Options")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string CustomText
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate(); //redraw component after change value from VS Properties section
            }
        }

        private string _textToDraw
        {
            get
            {
                string text = CustomText;

                switch (VisualMode)
                {
                    case ProgressBarDisplayMode.Percentage:
                        text = _percentageStr;
                        break;
                    case ProgressBarDisplayMode.CurrProgress:
                        text = _currProgressStr;
                        break;
                    case ProgressBarDisplayMode.TextAndCurrProgress:
                        text = $"{CustomText}: {_currProgressStr}";
                        break;
                    case ProgressBarDisplayMode.TextAndPercentage:
                        text = $"{CustomText}: {_percentageStr}";
                        break;
                }

                return text;
            }
            set { }
        }

        private string _percentageStr => $"{(int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100} %";

        private string _currProgressStr => $"{Value}/{Maximum}";

        private void FixComponentBlinking()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawProgressBar(g);

            DrawStringIfNeeded(g);
        }

        private void DrawProgressBar(Graphics g)
        {
            Rectangle rect = ClientRectangle;

            ProgressBarRenderer.DrawHorizontalBar(g, rect);

            rect.Inflate(-3, -3);

            if (Value > 0)
            {
                Rectangle clip = new(rect.X, rect.Y, (int)Math.Round((float)Value / Maximum * rect.Width),
                    rect.Height);

                g.FillRectangle(_progressColourBrush, clip);
            }
        }

        private void DrawStringIfNeeded(Graphics g)
        {
            if (VisualMode != ProgressBarDisplayMode.NoText)
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                string text = _textToDraw;

                SizeF len = g.MeasureString(text, TextFont);

                Point location = new(Width / 2 - (int)len.Width / 2, Height / 2 - (int)len.Height / 2);

                g.DrawString(text, TextFont, _textColourBrush, location);
            }
        }

        public new void Dispose()
        {
            _textColourBrush.Dispose();
            _progressColourBrush.Dispose();
            base.Dispose();
        }
    }
}