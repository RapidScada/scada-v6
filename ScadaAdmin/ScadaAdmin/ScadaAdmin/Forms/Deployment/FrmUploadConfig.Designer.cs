﻿namespace Scada.Admin.App.Forms.Deployment
{
    partial class FrmUploadConfig
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
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlTransferOptions = new Scada.Admin.App.Controls.Deployment.CtrlTransferOptions();
            this.ctrlProfileSelector = new Scada.Admin.App.Controls.Deployment.CtrlProfileSelector();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(325, 420);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(406, 420);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ctrlTransferOptions
            // 
            this.ctrlTransferOptions.Location = new System.Drawing.Point(12, 131);
            this.ctrlTransferOptions.Name = "ctrlTransferOptions";
            this.ctrlTransferOptions.Size = new System.Drawing.Size(469, 273);
            this.ctrlTransferOptions.TabIndex = 1;
            this.ctrlTransferOptions.OptionsChanged += new System.EventHandler(this.ctrlTransferOptions_OptionsChanged);
            // 
            // ctrlProfileSelector
            // 
            this.ctrlProfileSelector.Location = new System.Drawing.Point(12, 12);
            this.ctrlProfileSelector.Name = "ctrlProfileSelector";
            this.ctrlProfileSelector.Size = new System.Drawing.Size(469, 113);
            this.ctrlProfileSelector.TabIndex = 0;
            this.ctrlProfileSelector.SelectedProfileChanged += new System.EventHandler(this.ctrlProfileSelector_SelectedProfileChanged);
            // 
            // FrmUploadConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(493, 455);
            this.Controls.Add(this.ctrlTransferOptions);
            this.Controls.Add(this.ctrlProfileSelector);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUploadConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upload Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmUploadConfig_FormClosed);
            this.Load += new System.EventHandler(this.FrmUploadConfig_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnClose;
        private Controls.Deployment.CtrlProfileSelector ctrlProfileSelector;
        private Controls.Deployment.CtrlTransferOptions ctrlTransferOptions;
    }
}