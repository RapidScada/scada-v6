namespace Scada.Scheme.Editor
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
            this.lblVersionEn = new System.Windows.Forms.Label();
            this.pbAboutEn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutEn)).BeginInit();
            this.SuspendLayout();
            // 
            // lblVersionEn
            // 
            this.lblVersionEn.BackColor = System.Drawing.Color.White;
            this.lblVersionEn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.lblVersionEn.ForeColor = System.Drawing.Color.Black;
            this.lblVersionEn.Location = new System.Drawing.Point(255, 65);
            this.lblVersionEn.Margin = new System.Windows.Forms.Padding(0);
            this.lblVersionEn.Name = "lblVersionEn";
            this.lblVersionEn.Size = new System.Drawing.Size(80, 11);
            this.lblVersionEn.TabIndex = 4;
            this.lblVersionEn.Text = "Version 5.0.0.0";
            // 
            // pbAboutEn
            // 
            this.pbAboutEn.Enabled = false;
            this.pbAboutEn.Image = ((System.Drawing.Image)(resources.GetObject("pbAboutEn.Image")));
            this.pbAboutEn.Location = new System.Drawing.Point(0, 0);
            this.pbAboutEn.Name = "pbAboutEn";
            this.pbAboutEn.Size = new System.Drawing.Size(429, 225);
            this.pbAboutEn.TabIndex = 5;
            this.pbAboutEn.TabStop = false;
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 226);
            this.Controls.Add(this.lblVersionEn);
            this.Controls.Add(this.pbAboutEn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе";
            this.Click += new System.EventHandler(this.FrmAbout_Click);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmAbout_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutEn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblVersionEn;
        private System.Windows.Forms.PictureBox pbAboutEn;
    }
}