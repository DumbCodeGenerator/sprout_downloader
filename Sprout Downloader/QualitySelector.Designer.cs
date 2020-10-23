namespace Sprout_Downloader
{
    partial class QualitySelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new Sprout_Downloader.AntiAliasingLabel();
            this.radioListBox1 = new Sprout_Downloader.RadioListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(100, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 36);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select quality for download:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            // 
            // radioListBox1
            // 
            this.radioListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.radioListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.radioListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.radioListBox1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold);
            this.radioListBox1.ForeColor = System.Drawing.Color.Green;
            this.radioListBox1.FormattingEnabled = true;
            this.radioListBox1.ItemHeight = 20;
            this.radioListBox1.Location = new System.Drawing.Point(12, 52);
            this.radioListBox1.Name = "radioListBox1";
            this.radioListBox1.Size = new System.Drawing.Size(264, 140);
            this.radioListBox1.TabIndex = 0;
            // 
            // QualitySelector
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 240);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioListBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "QualitySelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QualitySelector";
            this.Load += new System.EventHandler(this.QualitySelector_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private RadioListBox radioListBox1;
        private AntiAliasingLabel label1;
    }
}