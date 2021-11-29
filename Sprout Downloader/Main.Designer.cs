using Sprout_Downloader.UI;

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
            this.totalProgressLabel = new Sprout_Downloader.UI.AntiAliasingLabel();
            this.downloadProgress = new Sprout_Downloader.UI.TextProgressBar();
            this.actionLabel = new Sprout_Downloader.UI.AntiAliasingLabel();
            this.label1 = new Sprout_Downloader.UI.AntiAliasingLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threadsCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.qualityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysAskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysTheBestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // urlTextBox
            // 
            this.urlTextBox.AcceptsReturn = true;
            this.urlTextBox.Location = new System.Drawing.Point(14, 73);
            this.urlTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.urlTextBox.MaxLength = 99999;
            this.urlTextBox.Multiline = true;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.PlaceholderText = "URL Syntax: <URL>||<Password, if needed, else ONLY url without additional charact" +
    "ers>";
            this.urlTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.urlTextBox.Size = new System.Drawing.Size(542, 305);
            this.urlTextBox.TabIndex = 0;
            this.urlTextBox.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(14, 384);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(543, 52);
            this.button1.TabIndex = 2;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.totalProgressLabel);
            this.panel1.Controls.Add(this.downloadProgress);
            this.panel1.Controls.Add(this.actionLabel);
            this.panel1.Location = new System.Drawing.Point(14, 455);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(542, 164);
            this.panel1.TabIndex = 3;
            // 
            // totalProgressLabel
            // 
            this.totalProgressLabel.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.totalProgressLabel.Location = new System.Drawing.Point(3, 130);
            this.totalProgressLabel.Name = "totalProgressLabel";
            this.totalProgressLabel.Size = new System.Drawing.Size(536, 32);
            this.totalProgressLabel.TabIndex = 6;
            this.totalProgressLabel.Text = "Downloaded videos: 0/0";
            this.totalProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.totalProgressLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            // 
            // downloadProgress
            // 
            this.downloadProgress.CustomText = "";
            this.downloadProgress.Location = new System.Drawing.Point(0, 75);
            this.downloadProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.downloadProgress.Name = "downloadProgress";
            this.downloadProgress.ProgressColor = System.Drawing.Color.LightGreen;
            this.downloadProgress.Size = new System.Drawing.Size(542, 48);
            this.downloadProgress.TabIndex = 5;
            this.downloadProgress.TextColor = System.Drawing.Color.Black;
            this.downloadProgress.TextFont = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.downloadProgress.VisualMode = Sprout_Downloader.UI.ProgressBarDisplayMode.CustomText;
            // 
            // actionLabel
            // 
            this.actionLabel.Font = new System.Drawing.Font("Samsung Sans", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.actionLabel.Location = new System.Drawing.Point(0, 0);
            this.actionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actionLabel.Name = "actionLabel";
            this.actionLabel.Padding = new System.Windows.Forms.Padding(12);
            this.actionLabel.Size = new System.Drawing.Size(542, 72);
            this.actionLabel.TabIndex = 4;
            this.actionLabel.Text = "Action...";
            this.actionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.actionLabel.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Samsung Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(14, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(542, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Insert Video URLs:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(570, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.threadsCountToolStripMenuItem,
            this.qualityToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // threadsCountToolStripMenuItem
            // 
            this.threadsCountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.threadsCountToolStripMenuItem.Name = "threadsCountToolStripMenuItem";
            this.threadsCountToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.threadsCountToolStripMenuItem.Text = "Threads Count";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Text = "4";
            this.toolStripTextBox1.TextChanged += new System.EventHandler(this.toolStripTextBox1_TextChanged);
            // 
            // qualityToolStripMenuItem
            // 
            this.qualityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysAskToolStripMenuItem,
            this.alwaysTheBestToolStripMenuItem});
            this.qualityToolStripMenuItem.Name = "qualityToolStripMenuItem";
            this.qualityToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.qualityToolStripMenuItem.Text = "Quality";
            // 
            // alwaysAskToolStripMenuItem
            // 
            this.alwaysAskToolStripMenuItem.Name = "alwaysAskToolStripMenuItem";
            this.alwaysAskToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.alwaysAskToolStripMenuItem.Text = "Always ask";
            this.alwaysAskToolStripMenuItem.Click += new System.EventHandler(this.alwaysAskToolStripMenuItem_Click);
            // 
            // alwaysTheBestToolStripMenuItem
            // 
            this.alwaysTheBestToolStripMenuItem.Name = "alwaysTheBestToolStripMenuItem";
            this.alwaysTheBestToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.alwaysTheBestToolStripMenuItem.Text = "Always the best";
            this.alwaysTheBestToolStripMenuItem.Click += new System.EventHandler(this.alwaysTheBestToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 626);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Sprout Downloader (by JCTrich)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem threadsCountToolStripMenuItem;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripMenuItem qualityToolStripMenuItem;
        private ToolStripMenuItem alwaysAskToolStripMenuItem;
        private ToolStripMenuItem alwaysTheBestToolStripMenuItem;
        private AntiAliasingLabel totalProgressLabel;
    }
}

