namespace Sprout_Downloader
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.downloadProgress = new Sprout_Downloader.TextProgressBar();
            this.actionLabel = new Sprout_Downloader.AntiAliasingLabel();
            this.label1 = new Sprout_Downloader.AntiAliasingLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(12, 63);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(465, 20);
            this.urlTextBox.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(188, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 45);
            this.button1.TabIndex = 2;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.downloadProgress);
            this.panel1.Controls.Add(this.actionLabel);
            this.panel1.Location = new System.Drawing.Point(12, 168);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(465, 142);
            this.panel1.TabIndex = 3;
            // 
            // downloadProgress
            // 
            this.downloadProgress.CustomText = "";
            this.downloadProgress.Location = new System.Drawing.Point(0, 65);
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.ProgressColor = System.Drawing.Color.LightGreen;
            this.downloadProgress.Size = new System.Drawing.Size(465, 42);
            this.downloadProgress.TabIndex = 5;
            this.downloadProgress.TextColor = System.Drawing.Color.Black;
            this.downloadProgress.TextFont = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloadProgress.VisualMode = Sprout_Downloader.ProgressBarDisplayMode.CustomText;
            // 
            // actionLabel
            // 
            this.actionLabel.Font = new System.Drawing.Font("Samsung Sans", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.actionLabel.Location = new System.Drawing.Point(0, 0);
            this.actionLabel.Name = "actionLabel";
            this.actionLabel.Padding = new System.Windows.Forms.Padding(10);
            this.actionLabel.Size = new System.Drawing.Size(465, 62);
            this.actionLabel.TabIndex = 4;
            this.actionLabel.Text = "Action...";
            this.actionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.actionLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(465, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Insert Video URL:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            // 
            // Main
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 322);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urlTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Sprout Downloader (by JCTrich)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private TextProgressBar downloadProgress;
        private AntiAliasingLabel label1;
        private AntiAliasingLabel actionLabel;
    }
}

