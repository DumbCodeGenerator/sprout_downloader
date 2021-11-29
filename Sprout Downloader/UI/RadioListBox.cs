using System.ComponentModel;
using System.Data;
using System.Drawing.Text;
using System.Windows.Forms.VisualStyles;

namespace Sprout_Downloader.UI
{
    public sealed class RadioListBox : ListBox
    {
        private readonly StringFormat _align;
        private Brush _backBrush;
        private bool _isTransparent;

        // Public constructor
        public RadioListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectionMode = SelectionMode.One;
            ItemHeight = FontHeight;

            _align = new StringFormat(StringFormat.GenericDefault) { LineAlignment = StringAlignment.Center };

            // Force transparent analisys
            BackColor = BackColor;
        }

        // Allows the BackColor to be transparent
        public override Color BackColor
        {
            get => _isTransparent ? Color.Transparent : base.BackColor;
            set
            {
                if (value == Color.Transparent)
                {
                    _isTransparent = true;
                    base.BackColor = Parent?.BackColor ?? SystemColors.Window;
                }
                else
                {
                    _isTransparent = false;
                    base.BackColor = value;
                }

                _backBrush?.Dispose();
                _backBrush = new SolidBrush(base.BackColor);

                Invalidate();
            }
        }

        // Hides these properties in the designer
        [Browsable(false)]
        public override DrawMode DrawMode
        {
            get => base.DrawMode;
            set
            {
                if (value != DrawMode.OwnerDrawFixed)
                    throw new Exception("Invalid value for DrawMode property");
                base.DrawMode = value;
            }
        }

        [Browsable(false)]
        public override SelectionMode SelectionMode
        {
            get => base.SelectionMode;
            set
            {
                if (value != SelectionMode.One)
                    throw new Exception("Invalid value for SelectionMode property");
                base.SelectionMode = value;
            }
        }

        // Main paiting method
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int maxItem = Items.Count - 1;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            if (e.Index < 0 || e.Index > maxItem)
            {
                // Erase all background if control has no items
                e.Graphics.FillRectangle(_backBrush, ClientRectangle);
                return;
            }

            // Calculate bounds for background, if last item paint up to bottom of control
            Rectangle backRect = e.Bounds;
            if (e.Index == maxItem)
                backRect.Height = ClientRectangle.Top + ClientRectangle.Height - e.Bounds.Top;
            e.Graphics.FillRectangle(_backBrush, backRect);

            // Determines text color/brush
            Brush textBrush;
            bool isChecked = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            RadioButtonState state = isChecked ? RadioButtonState.CheckedNormal : RadioButtonState.UncheckedNormal;
            if ((e.State & DrawItemState.Disabled) == DrawItemState.Disabled)
            {
                textBrush = SystemBrushes.GrayText;
                state = isChecked ? RadioButtonState.CheckedDisabled : RadioButtonState.UncheckedDisabled;
            }
            else if ((e.State & DrawItemState.Grayed) == DrawItemState.Grayed)
            {
                textBrush = SystemBrushes.GrayText;
                state = isChecked ? RadioButtonState.CheckedDisabled : RadioButtonState.UncheckedDisabled;
            }
            else
            {
                textBrush = new SolidBrush(ForeColor);
            }

            // Determines bounds for text and radio button
            Size glyphSize = RadioButtonRenderer.GetGlyphSize(e.Graphics, state);
            Point glyphLocation = e.Bounds.Location;
            glyphLocation.Y += (e.Bounds.Height - glyphSize.Height) / 2;

            Rectangle bounds = new(e.Bounds.X + glyphSize.Width + 8, e.Bounds.Y,
                e.Bounds.Width - glyphSize.Width + 8, e.Bounds.Height);

            // Draws the radio button
            RadioButtonRenderer.DrawRadioButton(e.Graphics, glyphLocation, state);

            // Draws the text
            if (!string.IsNullOrEmpty(DisplayMember)) // Bound Datatable? Then show the column written in Displaymember
                e.Graphics.DrawString(((DataRowView)Items[e.Index])[DisplayMember].ToString(),
                    e.Font, textBrush, bounds, _align);
            else
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, textBrush, bounds, _align);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        // Prevent background erasing
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // WM_ERASEBKGND
            {
                m.Result = (IntPtr)1; // avoid default background erasing
                return;
            }

            base.DefWndProc(ref m);
        }

        // Other event handlers
        protected override void OnHandleCreated(EventArgs e)
        {
            if (FontHeight > ItemHeight)
                ItemHeight = FontHeight;

            base.OnHandleCreated(e);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (FontHeight > ItemHeight)
                ItemHeight = FontHeight;
            Update();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            // Force to change backcolor
            BackColor = BackColor;
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            // Force to change backcolor
            BackColor = BackColor;
        }
    }
}