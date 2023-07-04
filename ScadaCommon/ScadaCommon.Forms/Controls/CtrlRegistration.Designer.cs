namespace Scada.Forms.Controls
{
    partial class CtrlRegistration
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbInfo2 = new PictureBox();
            lblRegKeyInfo = new Label();
            llblGetTrialKey = new LinkLabel();
            llblGetPermanentKey = new LinkLabel();
            btnPasteRegKey = new Button();
            txtRegKey = new TextBox();
            lblRegKey = new Label();
            pbInfo1 = new PictureBox();
            lblCompCodeInfo = new Label();
            btnRefreshCompCode = new Button();
            btnCopyCompCode = new Button();
            txtCompCode = new TextBox();
            lblCompCode = new Label();
            txtProductName = new TextBox();
            lblProductName = new Label();
            ((System.ComponentModel.ISupportInitialize)pbInfo2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbInfo1).BeginInit();
            SuspendLayout();
            // 
            // pbInfo2
            // 
            pbInfo2.Image = Properties.Resources.info;
            pbInfo2.Location = new Point(0, 309);
            pbInfo2.Name = "pbInfo2";
            pbInfo2.Size = new Size(16, 16);
            pbInfo2.TabIndex = 28;
            pbInfo2.TabStop = false;
            // 
            // lblRegKeyInfo
            // 
            lblRegKeyInfo.AutoSize = true;
            lblRegKeyInfo.ForeColor = SystemColors.GrayText;
            lblRegKeyInfo.Location = new Point(22, 310);
            lblRegKeyInfo.Name = "lblRegKeyInfo";
            lblRegKeyInfo.Size = new Size(347, 15);
            lblRegKeyInfo.TabIndex = 10;
            lblRegKeyInfo.Text = "Key verification result is written in the application or module log.";
            // 
            // llblGetTrialKey
            // 
            llblGetTrialKey.AutoSize = true;
            llblGetTrialKey.Location = new Point(-3, 358);
            llblGetTrialKey.Name = "llblGetTrialKey";
            llblGetTrialKey.Size = new Size(69, 15);
            llblGetTrialKey.TabIndex = 12;
            llblGetTrialKey.TabStop = true;
            llblGetTrialKey.Text = "Get trial key";
            llblGetTrialKey.LinkClicked += llblGetTrialKey_LinkClicked;
            // 
            // llblGetPermanentKey
            // 
            llblGetPermanentKey.AutoSize = true;
            llblGetPermanentKey.Location = new Point(-3, 338);
            llblGetPermanentKey.Name = "llblGetPermanentKey";
            llblGetPermanentKey.Size = new Size(107, 15);
            llblGetPermanentKey.TabIndex = 11;
            llblGetPermanentKey.TabStop = true;
            llblGetPermanentKey.Text = "Get permanent key";
            llblGetPermanentKey.LinkClicked += llblGetPermanentKey_LinkClicked;
            // 
            // btnPasteRegKey
            // 
            btnPasteRegKey.Location = new Point(0, 280);
            btnPasteRegKey.Name = "btnPasteRegKey";
            btnPasteRegKey.Size = new Size(90, 23);
            btnPasteRegKey.TabIndex = 9;
            btnPasteRegKey.Text = "Paste";
            btnPasteRegKey.UseVisualStyleBackColor = true;
            btnPasteRegKey.Click += btnPasteRegKey_Click;
            // 
            // txtRegKey
            // 
            txtRegKey.Location = new Point(0, 251);
            txtRegKey.Name = "txtRegKey";
            txtRegKey.Size = new Size(510, 23);
            txtRegKey.TabIndex = 8;
            // 
            // lblRegKey
            // 
            lblRegKey.AutoSize = true;
            lblRegKey.Location = new Point(-3, 233);
            lblRegKey.Name = "lblRegKey";
            lblRegKey.Size = new Size(91, 15);
            lblRegKey.TabIndex = 7;
            lblRegKey.Text = "Registration key";
            // 
            // pbInfo1
            // 
            pbInfo1.Image = Properties.Resources.info;
            pbInfo1.Location = new Point(0, 204);
            pbInfo1.Name = "pbInfo1";
            pbInfo1.Size = new Size(16, 16);
            pbInfo1.TabIndex = 19;
            pbInfo1.TabStop = false;
            // 
            // lblCompCodeInfo
            // 
            lblCompCodeInfo.AutoSize = true;
            lblCompCodeInfo.ForeColor = SystemColors.GrayText;
            lblCompCodeInfo.Location = new Point(22, 205);
            lblCompCodeInfo.Name = "lblCompCodeInfo";
            lblCompCodeInfo.Size = new Size(377, 15);
            lblCompCodeInfo.TabIndex = 6;
            lblCompCodeInfo.Text = "If the code is missing, upload the configuration and restart the service.";
            // 
            // btnRefreshCompCode
            // 
            btnRefreshCompCode.Location = new Point(96, 175);
            btnRefreshCompCode.Name = "btnRefreshCompCode";
            btnRefreshCompCode.Size = new Size(90, 23);
            btnRefreshCompCode.TabIndex = 5;
            btnRefreshCompCode.Text = "Refresh";
            btnRefreshCompCode.UseVisualStyleBackColor = true;
            btnRefreshCompCode.Click += btnRefreshCompCode_Click;
            // 
            // btnCopyCompCode
            // 
            btnCopyCompCode.Location = new Point(0, 175);
            btnCopyCompCode.Name = "btnCopyCompCode";
            btnCopyCompCode.Size = new Size(90, 23);
            btnCopyCompCode.TabIndex = 4;
            btnCopyCompCode.Text = "Copy";
            btnCopyCompCode.UseVisualStyleBackColor = true;
            btnCopyCompCode.Click += btnCopyCompCode_Click;
            // 
            // txtCompCode
            // 
            txtCompCode.Location = new Point(0, 69);
            txtCompCode.Multiline = true;
            txtCompCode.Name = "txtCompCode";
            txtCompCode.ReadOnly = true;
            txtCompCode.Size = new Size(510, 100);
            txtCompCode.TabIndex = 3;
            // 
            // lblCompCode
            // 
            lblCompCode.AutoSize = true;
            lblCompCode.Location = new Point(-3, 51);
            lblCompCode.Name = "lblCompCode";
            lblCompCode.Size = new Size(90, 15);
            lblCompCode.TabIndex = 2;
            lblCompCode.Text = "Computer code";
            // 
            // txtProductName
            // 
            txtProductName.Location = new Point(0, 15);
            txtProductName.Name = "txtProductName";
            txtProductName.ReadOnly = true;
            txtProductName.Size = new Size(510, 23);
            txtProductName.TabIndex = 1;
            // 
            // lblProductName
            // 
            lblProductName.AutoSize = true;
            lblProductName.Location = new Point(-3, -3);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(82, 15);
            lblProductName.TabIndex = 0;
            lblProductName.Text = "Product name";
            // 
            // CtrlRegistration
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(llblGetTrialKey);
            Controls.Add(llblGetPermanentKey);
            Controls.Add(lblRegKeyInfo);
            Controls.Add(pbInfo2);
            Controls.Add(btnPasteRegKey);
            Controls.Add(txtRegKey);
            Controls.Add(lblRegKey);
            Controls.Add(lblCompCodeInfo);
            Controls.Add(pbInfo1);
            Controls.Add(btnRefreshCompCode);
            Controls.Add(btnCopyCompCode);
            Controls.Add(txtCompCode);
            Controls.Add(lblCompCode);
            Controls.Add(txtProductName);
            Controls.Add(lblProductName);
            Name = "CtrlRegistration";
            Size = new Size(510, 375);
            ((System.ComponentModel.ISupportInitialize)pbInfo2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbInfo1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbInfo2;
        private Label lblRegKeyInfo;
        private LinkLabel llblGetTrialKey;
        private LinkLabel llblGetPermanentKey;
        private Button btnPasteRegKey;
        private TextBox txtRegKey;
        private Label lblRegKey;
        private PictureBox pbInfo1;
        private Label lblCompCodeInfo;
        private Button btnRefreshCompCode;
        private Button btnCopyCompCode;
        private TextBox txtCompCode;
        private Label lblCompCode;
        private TextBox txtProductName;
        private Label lblProductName;
    }
}
