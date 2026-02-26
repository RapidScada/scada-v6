namespace Scada.Admin.App.Forms
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            pbAboutRu = new PictureBox();
            lblWebsite = new Label();
            lblVersionRu = new Label();
            pbAboutEn = new PictureBox();
            lblVersionEn = new Label();
            ((System.ComponentModel.ISupportInitialize)pbAboutRu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbAboutEn).BeginInit();
            SuspendLayout();
            // 
            // pbAboutRu
            // 
            pbAboutRu.Enabled = false;
            pbAboutRu.Image = (Image)resources.GetObject("pbAboutRu.Image");
            pbAboutRu.Location = new Point(0, 0);
            pbAboutRu.Name = "pbAboutRu";
            pbAboutRu.Size = new Size(424, 222);
            pbAboutRu.TabIndex = 0;
            pbAboutRu.TabStop = false;
            // 
            // lblWebsite
            // 
            lblWebsite.Cursor = Cursors.Hand;
            lblWebsite.Location = new Point(220, 174);
            lblWebsite.Name = "lblWebsite";
            lblWebsite.Size = new Size(95, 23);
            lblWebsite.TabIndex = 2;
            lblWebsite.Click += lblLink_Click;
            // 
            // lblVersionRu
            // 
            lblVersionRu.BackColor = Color.White;
            lblVersionRu.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblVersionRu.ForeColor = Color.Black;
            lblVersionRu.Location = new Point(227, 77);
            lblVersionRu.Margin = new Padding(0);
            lblVersionRu.Name = "lblVersionRu";
            lblVersionRu.Size = new Size(80, 12);
            lblVersionRu.TabIndex = 0;
            lblVersionRu.Text = "Версия 6.0.0.0";
            lblVersionRu.TextAlign = ContentAlignment.TopRight;
            lblVersionRu.Click += FrmAbout_Click;
            // 
            // pbAboutEn
            // 
            pbAboutEn.Enabled = false;
            pbAboutEn.Image = (Image)resources.GetObject("pbAboutEn.Image");
            pbAboutEn.Location = new Point(0, 0);
            pbAboutEn.Name = "pbAboutEn";
            pbAboutEn.Size = new Size(424, 222);
            pbAboutEn.TabIndex = 3;
            pbAboutEn.TabStop = false;
            // 
            // lblVersionEn
            // 
            lblVersionEn.BackColor = Color.White;
            lblVersionEn.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblVersionEn.ForeColor = Color.Black;
            lblVersionEn.Location = new Point(266, 77);
            lblVersionEn.Margin = new Padding(0);
            lblVersionEn.Name = "lblVersionEn";
            lblVersionEn.Size = new Size(80, 12);
            lblVersionEn.TabIndex = 1;
            lblVersionEn.Text = "Version 6.0.0.0";
            lblVersionEn.Click += FrmAbout_Click;
            // 
            // FrmAbout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(424, 222);
            Controls.Add(lblVersionEn);
            Controls.Add(pbAboutEn);
            Controls.Add(lblVersionRu);
            Controls.Add(pbAboutRu);
            Controls.Add(lblWebsite);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmAbout";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About";
            Click += FrmAbout_Click;
            KeyPress += FrmAbout_KeyPress;
            ((System.ComponentModel.ISupportInitialize)pbAboutRu).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbAboutEn).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pbAboutRu;
        private System.Windows.Forms.Label lblWebsite;
        private System.Windows.Forms.Label lblVersionRu;
        private System.Windows.Forms.PictureBox pbAboutEn;
        private System.Windows.Forms.Label lblVersionEn;
    }
}