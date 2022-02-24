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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnCopyCompCode = new System.Windows.Forms.Button();
            this.pbInfo = new System.Windows.Forms.PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 81);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(510, 100);
            this.textBox1.TabIndex = 3;
            // 
            // btnCopyCompCode
            // 
            this.btnCopyCompCode.Location = new System.Drawing.Point(12, 187);
            this.btnCopyCompCode.Name = "btnCopyCompCode";
            this.btnCopyCompCode.Size = new System.Drawing.Size(75, 23);
            this.btnCopyCompCode.TabIndex = 4;
            this.btnCopyCompCode.Text = "Copy";
            this.btnCopyCompCode.UseVisualStyleBackColor = true;
            this.btnCopyCompCode.Click += new System.EventHandler(this.btnCopyCompCode_Click);
            // 
            // pbInfo
            // 
            this.pbInfo.Image = global::Scada.Admin.Properties.Resources.info;
            this.pbInfo.Location = new System.Drawing.Point(93, 190);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(16, 16);
            this.pbInfo.TabIndex = 3;
            this.pbInfo.TabStop = false;
            // 
            // lblCompCodeInfo
            // 
            this.lblCompCodeInfo.AutoSize = true;
            this.lblCompCodeInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCompCodeInfo.Location = new System.Drawing.Point(115, 191);
            this.lblCompCodeInfo.Name = "lblCompCodeInfo";
            this.lblCompCodeInfo.Size = new System.Drawing.Size(379, 15);
            this.lblCompCodeInfo.TabIndex = 5;
            this.lblCompCodeInfo.Text = "If the code is missing, upload the configuration and restart the services";
            // 
            // lblRegKey
            // 
            this.lblRegKey.AutoSize = true;
            this.lblRegKey.Location = new System.Drawing.Point(9, 223);
            this.lblRegKey.Name = "lblRegKey";
            this.lblRegKey.Size = new System.Drawing.Size(91, 15);
            this.lblRegKey.TabIndex = 6;
            this.lblRegKey.Text = "Registration key";
            // 
            // txtRegKey
            // 
            this.txtRegKey.Location = new System.Drawing.Point(12, 241);
            this.txtRegKey.Name = "txtRegKey";
            this.txtRegKey.Size = new System.Drawing.Size(510, 23);
            this.txtRegKey.TabIndex = 7;
            // 
            // btnPasteRegKey
            // 
            this.btnPasteRegKey.Location = new System.Drawing.Point(12, 270);
            this.btnPasteRegKey.Name = "btnPasteRegKey";
            this.btnPasteRegKey.Size = new System.Drawing.Size(75, 23);
            this.btnPasteRegKey.TabIndex = 8;
            this.btnPasteRegKey.Text = "Paste";
            this.btnPasteRegKey.UseVisualStyleBackColor = true;
            this.btnPasteRegKey.Click += new System.EventHandler(this.btnPasteRegKey_Click);
            // 
            // llblGetPermanentKey
            // 
            this.llblGetPermanentKey.AutoSize = true;
            this.llblGetPermanentKey.Location = new System.Drawing.Point(9, 306);
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
            this.llblGetTrialKey.Location = new System.Drawing.Point(9, 326);
            this.llblGetTrialKey.Name = "llblGetTrialKey";
            this.llblGetTrialKey.Size = new System.Drawing.Size(69, 15);
            this.llblGetTrialKey.TabIndex = 10;
            this.llblGetTrialKey.TabStop = true;
            this.llblGetTrialKey.Text = "Get trial key";
            this.llblGetTrialKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblGetTrialKey_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(366, 354);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(447, 354);
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
            // FrmRegistration
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 389);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.llblGetTrialKey);
            this.Controls.Add(this.llblGetPermanentKey);
            this.Controls.Add(this.btnPasteRegKey);
            this.Controls.Add(this.txtRegKey);
            this.Controls.Add(this.lblRegKey);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.lblCompCodeInfo);
            this.Controls.Add(this.btnCopyCompCode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblCompCode);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRegistration";
            this.ShowInTaskbar = false;
            this.Text = "Registration";
            this.Load += new System.EventHandler(this.FrmRegistration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCompCode;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnCopyCompCode;
        private System.Windows.Forms.PictureBox pbInfo;
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
    }
}