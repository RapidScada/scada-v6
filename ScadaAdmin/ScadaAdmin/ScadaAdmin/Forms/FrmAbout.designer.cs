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
            pbAboutRu = new System.Windows.Forms.PictureBox();
            lblWebsite = new System.Windows.Forms.Label();
            lblVersionRu = new System.Windows.Forms.Label();
            pbAboutEn = new System.Windows.Forms.PictureBox();
            lblVersionEn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pbAboutRu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbAboutEn).BeginInit();
            SuspendLayout();
            // 
            // pbAboutRu
            // 
            pbAboutRu.Enabled = false;
            pbAboutRu.Image = (System.Drawing.Image)resources.GetObject("pbAboutRu.Image");
            pbAboutRu.Location = new System.Drawing.Point(0, 0);
            pbAboutRu.Name = "pbAboutRu";
            pbAboutRu.Size = new System.Drawing.Size(424, 222);
            pbAboutRu.TabIndex = 0;
            pbAboutRu.TabStop = false;
            // 
            // lblWebsite
            // 
            lblWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            lblWebsite.Location = new System.Drawing.Point(220, 174);
            lblWebsite.Name = "lblWebsite";
            lblWebsite.Size = new System.Drawing.Size(95, 23);
            lblWebsite.TabIndex = 2;
            lblWebsite.Click += lblLink_Click;
            // 
            // lblVersionRu
            // 
            lblVersionRu.BackColor = System.Drawing.Color.White;
            lblVersionRu.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            lblVersionRu.ForeColor = System.Drawing.Color.Black;
            lblVersionRu.Location = new System.Drawing.Point(227, 77);
            lblVersionRu.Margin = new System.Windows.Forms.Padding(0);
            lblVersionRu.Name = "lblVersionRu";
            lblVersionRu.Size = new System.Drawing.Size(80, 12);
            lblVersionRu.TabIndex = 0;
            lblVersionRu.Text = "Версия 6.0.0.0";
            lblVersionRu.TextAlign = System.Drawing.ContentAlignment.TopRight;
            lblVersionRu.Click += FrmAbout_Click;
            // 
            // pbAboutEn
            // 
            pbAboutEn.Enabled = false;
            pbAboutEn.Image = (System.Drawing.Image)resources.GetObject("pbAboutEn.Image");
            pbAboutEn.Location = new System.Drawing.Point(0, 0);
            pbAboutEn.Name = "pbAboutEn";
            pbAboutEn.Size = new System.Drawing.Size(424, 222);
            pbAboutEn.TabIndex = 3;
            pbAboutEn.TabStop = false;
            // 
            // lblVersionEn
            // 
            lblVersionEn.BackColor = System.Drawing.Color.White;
            lblVersionEn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            lblVersionEn.ForeColor = System.Drawing.Color.Black;
            lblVersionEn.Location = new System.Drawing.Point(266, 77);
            lblVersionEn.Margin = new System.Windows.Forms.Padding(0);
            lblVersionEn.Name = "lblVersionEn";
            lblVersionEn.Size = new System.Drawing.Size(80, 12);
            lblVersionEn.TabIndex = 1;
            lblVersionEn.Text = "Version 6.0.0.0";
            lblVersionEn.Click += FrmAbout_Click;
            // 
            // FrmAbout
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(424, 222);
            Controls.Add(lblVersionEn);
            Controls.Add(pbAboutEn);
            Controls.Add(lblVersionRu);
            Controls.Add(pbAboutRu);
            Controls.Add(lblWebsite);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "FrmAbout";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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