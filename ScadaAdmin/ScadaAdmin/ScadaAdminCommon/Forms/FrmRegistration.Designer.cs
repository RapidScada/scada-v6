namespace Scada.Admin.Forms
{
    partial class FrmRegistration
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
            this.lblCompCode = new System.Windows.Forms.Label();
            this.txtCompCode = new System.Windows.Forms.TextBox();
            this.btnCopyCompCode = new System.Windows.Forms.Button();
            this.pbInfo1 = new System.Windows.Forms.PictureBox();
            this.lblCompCodeInfo = new System.Windows.Forms.Label();
            this.lblRegKey = new System.Windows.Forms.Label();
            this.txtRegKey = new System.Windows.Forms.TextBox();
            this.btnPasteRegKey = new System.Windows.Forms.Button();
            this.llblGetPermanentKey = new System.Windows.Forms.LinkLabel();
            this.llblGetTrialKey = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.pbInfo2 = new System.Windows.Forms.PictureBox();
            this.lblRegKeyInfo = new System.Windows.Forms.Label();
            this.btnRefreshCompCode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCompCode
            // 
            this.lblCompCode.AutoSize = true;
            this.lblCompCode.Location = new System.Drawing.Point(9, 63);
            this.lblCompCode.Name = "lblCompCode";
            this.lblCompCode.Size = new System.Drawing.Size(90, 15);
            this.lblCompCode.TabIndex = 2;
            this.lblCompCode.Text = "Computer code";
            // 
            // txtCompCode
            // 
            this.txtCompCode.Location = new System.Drawing.Point(12, 81);
            this.txtCompCode.Multiline = true;
            this.txtCompCode.Name = "txtCompCode";
            this.txtCompCode.ReadOnly = true;
            this.txtCompCode.Size = new System.Drawing.Size(510, 100);
            this.txtCompCode.TabIndex = 3;
            // 
            // btnCopyCompCode
            // 
            this.btnCopyCompCode.Location = new System.Drawing.Point(12, 187);
            this.btnCopyCompCode.Name = "btnCopyCompCode";
            this.btnCopyCompCode.Size = new System.Drawing.Size(80, 23);
            this.btnCopyCompCode.TabIndex = 4;
            this.btnCopyCompCode.Text = "Copy";
            this.btnCopyCompCode.UseVisualStyleBackColor = true;
            this.btnCopyCompCode.Click += new System.EventHandler(this.btnCopyCompCode_Click);
            // 
            // pbInfo1
            // 
            this.pbInfo1.Image = global::Scada.Admin.Properties.Resources.info;
            this.pbInfo1.Location = new System.Drawing.Point(12, 216);
            this.pbInfo1.Name = "pbInfo1";
            this.pbInfo1.Size = new System.Drawing.Size(16, 16);
            this.pbInfo1.TabIndex = 3;
            this.pbInfo1.TabStop = false;
            // 
            // lblCompCodeInfo
            // 
            this.lblCompCodeInfo.AutoSize = true;
            this.lblCompCodeInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCompCodeInfo.Location = new System.Drawing.Point(34, 217);
            this.lblCompCodeInfo.Name = "lblCompCodeInfo";
            this.lblCompCodeInfo.Size = new System.Drawing.Size(379, 15);
            this.lblCompCodeInfo.TabIndex = 5;
            this.lblCompCodeInfo.Text = "If the code is missing, upload the configuration and restart the services";
            // 
            // lblRegKey
            // 
            this.lblRegKey.AutoSize = true;
            this.lblRegKey.Location = new System.Drawing.Point(9, 245);
            this.lblRegKey.Name = "lblRegKey";
            this.lblRegKey.Size = new System.Drawing.Size(91, 15);
            this.lblRegKey.TabIndex = 6;
            this.lblRegKey.Text = "Registration key";
            // 
            // txtRegKey
            // 
            this.txtRegKey.Location = new System.Drawing.Point(12, 263);
            this.txtRegKey.Name = "txtRegKey";
            this.txtRegKey.Size = new System.Drawing.Size(510, 23);
            this.txtRegKey.TabIndex = 7;
            // 
            // btnPasteRegKey
            // 
            this.btnPasteRegKey.Location = new System.Drawing.Point(12, 292);
            this.btnPasteRegKey.Name = "btnPasteRegKey";
            this.btnPasteRegKey.Size = new System.Drawing.Size(80, 23);
            this.btnPasteRegKey.TabIndex = 8;
            this.btnPasteRegKey.Text = "Paste";
            this.btnPasteRegKey.UseVisualStyleBackColor = true;
            this.btnPasteRegKey.Click += new System.EventHandler(this.btnPasteRegKey_Click);
            // 
            // llblGetPermanentKey
            // 
            this.llblGetPermanentKey.AutoSize = true;
            this.llblGetPermanentKey.Location = new System.Drawing.Point(9, 350);
            this.llblGetPermanentKey.Name = "llblGetPermanentKey";
            this.llblGetPermanentKey.Size = new System.Drawing.Size(107, 15);
            this.llblGetPermanentKey.TabIndex = 9;
            this.llblGetPermanentKey.TabStop = true;
            this.llblGetPermanentKey.Text = "Get permanent key";
            this.llblGetPermanentKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblGetPermanentKey_LinkClicked);
            // 
            // llblGetTrialKey
            // 
            this.llblGetTrialKey.AutoSize = true;
            this.llblGetTrialKey.Location = new System.Drawing.Point(9, 370);
            this.llblGetTrialKey.Name = "llblGetTrialKey";
            this.llblGetTrialKey.Size = new System.Drawing.Size(69, 15);
            this.llblGetTrialKey.TabIndex = 10;
            this.llblGetTrialKey.TabStop = true;
            this.llblGetTrialKey.Text = "Get trial key";
            this.llblGetTrialKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblGetTrialKey_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(366, 398);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(447, 398);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(9, 9);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(82, 15);
            this.lblProductName.TabIndex = 0;
            this.lblProductName.Text = "Product name";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(12, 27);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.ReadOnly = true;
            this.txtProductName.Size = new System.Drawing.Size(510, 23);
            this.txtProductName.TabIndex = 1;
            // 
            // pbInfo2
            // 
            this.pbInfo2.Image = global::Scada.Admin.Properties.Resources.info;
            this.pbInfo2.Location = new System.Drawing.Point(12, 321);
            this.pbInfo2.Name = "pbInfo2";
            this.pbInfo2.Size = new System.Drawing.Size(16, 16);
            this.pbInfo2.TabIndex = 13;
            this.pbInfo2.TabStop = false;
            // 
            // lblRegKeyInfo
            // 
            this.lblRegKeyInfo.AutoSize = true;
            this.lblRegKeyInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRegKeyInfo.Location = new System.Drawing.Point(34, 322);
            this.lblRegKeyInfo.Name = "lblRegKeyInfo";
            this.lblRegKeyInfo.Size = new System.Drawing.Size(347, 15);
            this.lblRegKeyInfo.TabIndex = 14;
            this.lblRegKeyInfo.Text = "Key verification result is written in the application or module log.";
            // 
            // btnRefreshCompCode
            // 
            this.btnRefreshCompCode.Location = new System.Drawing.Point(98, 187);
            this.btnRefreshCompCode.Name = "btnRefreshCompCode";
            this.btnRefreshCompCode.Size = new System.Drawing.Size(80, 23);
            this.btnRefreshCompCode.TabIndex = 15;
            this.btnRefreshCompCode.Text = "Refresh";
            this.btnRefreshCompCode.UseVisualStyleBackColor = true;
            this.btnRefreshCompCode.Click += new System.EventHandler(this.btnRefreshCompCode_Click);
            // 
            // FrmRegistration
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 433);
            this.Controls.Add(this.pbInfo2);
            this.Controls.Add(this.lblRegKeyInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.llblGetTrialKey);
            this.Controls.Add(this.llblGetPermanentKey);
            this.Controls.Add(this.btnPasteRegKey);
            this.Controls.Add(this.txtRegKey);
            this.Controls.Add(this.lblRegKey);
            this.Controls.Add(this.pbInfo1);
            this.Controls.Add(this.lblCompCodeInfo);
            this.Controls.Add(this.btnRefreshCompCode);
            this.Controls.Add(this.btnCopyCompCode);
            this.Controls.Add(this.txtCompCode);
            this.Controls.Add(this.lblCompCode);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRegistration";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registration";
            this.Load += new System.EventHandler(this.FrmRegistration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCompCode;
        private System.Windows.Forms.TextBox txtCompCode;
        private System.Windows.Forms.Button btnCopyCompCode;
        private System.Windows.Forms.PictureBox pbInfo1;
        private System.Windows.Forms.Label lblCompCodeInfo;
        private System.Windows.Forms.Label lblRegKey;
        private System.Windows.Forms.TextBox txtRegKey;
        private System.Windows.Forms.Button btnPasteRegKey;
        private System.Windows.Forms.LinkLabel llblGetPermanentKey;
        private System.Windows.Forms.LinkLabel llblGetTrialKey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.PictureBox pbInfo2;
        private System.Windows.Forms.Label lblRegKeyInfo;
        private System.Windows.Forms.Button btnRefreshCompCode;
    }
}