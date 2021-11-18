
namespace Scada.Admin.App.Forms.Deployment
{
    partial class FrmTransfer
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnBreak = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pbDownload = new System.Windows.Forms.PictureBox();
            this.pbUpload = new System.Windows.Forms.PictureBox();
            this.pbSuccess = new System.Windows.Forms.PictureBox();
            this.pbError = new System.Windows.Forms.PictureBox();
            this.pnlLogHolder = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDownload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).BeginInit();
            this.pnlLogHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(50, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "lblStatus";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 50);
            this.progressBar.MarqueeAnimationSpeed = 0;
            this.progressBar.Maximum = 1000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(660, 23);
            this.progressBar.TabIndex = 1;
            // 
            // btnBreak
            // 
            this.btnBreak.Location = new System.Drawing.Point(516, 426);
            this.btnBreak.Name = "btnBreak";
            this.btnBreak.Size = new System.Drawing.Size(75, 23);
            this.btnBreak.TabIndex = 3;
            this.btnBreak.Text = "Break";
            this.btnBreak.UseVisualStyleBackColor = true;
            this.btnBreak.Click += new System.EventHandler(this.btnBreak_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(597, 426);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // pbDownload
            // 
            this.pbDownload.Image = global::Scada.Admin.App.Properties.Resources.download_32;
            this.pbDownload.Location = new System.Drawing.Point(12, 12);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(32, 32);
            this.pbDownload.TabIndex = 5;
            this.pbDownload.TabStop = false;
            this.pbDownload.Visible = false;
            // 
            // pbUpload
            // 
            this.pbUpload.Image = global::Scada.Admin.App.Properties.Resources.upload_32;
            this.pbUpload.Location = new System.Drawing.Point(108, 12);
            this.pbUpload.Name = "pbUpload";
            this.pbUpload.Size = new System.Drawing.Size(32, 32);
            this.pbUpload.TabIndex = 6;
            this.pbUpload.TabStop = false;
            this.pbUpload.Visible = false;
            // 
            // pbSuccess
            // 
            this.pbSuccess.Image = global::Scada.Admin.App.Properties.Resources.success_32;
            this.pbSuccess.Location = new System.Drawing.Point(146, 12);
            this.pbSuccess.Name = "pbSuccess";
            this.pbSuccess.Size = new System.Drawing.Size(32, 32);
            this.pbSuccess.TabIndex = 7;
            this.pbSuccess.TabStop = false;
            this.pbSuccess.Visible = false;
            // 
            // pbError
            // 
            this.pbError.Image = global::Scada.Admin.App.Properties.Resources.error_32;
            this.pbError.Location = new System.Drawing.Point(184, 12);
            this.pbError.Name = "pbError";
            this.pbError.Size = new System.Drawing.Size(32, 32);
            this.pbError.TabIndex = 8;
            this.pbError.TabStop = false;
            this.pbError.Visible = false;
            // 
            // pnlLogHolder
            // 
            this.pnlLogHolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLogHolder.Controls.Add(this.txtLog);
            this.pnlLogHolder.Location = new System.Drawing.Point(12, 79);
            this.pnlLogHolder.Name = "pnlLogHolder";
            this.pnlLogHolder.Size = new System.Drawing.Size(660, 331);
            this.pnlLogHolder.TabIndex = 2;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(658, 329);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // FrmTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBreak);
            this.Controls.Add(this.pnlLogHolder);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pbError);
            this.Controls.Add(this.pbSuccess);
            this.Controls.Add(this.pbUpload);
            this.Controls.Add(this.pbDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTransfer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transfer Configuration";
            this.Load += new System.EventHandler(this.FrmTransfer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDownload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbError)).EndInit();
            this.pnlLogHolder.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnBreak;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox pbDownload;
        private System.Windows.Forms.PictureBox pbUpload;
        private System.Windows.Forms.PictureBox pbSuccess;
        private System.Windows.Forms.PictureBox pbError;
        private System.Windows.Forms.Panel pnlLogHolder;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}