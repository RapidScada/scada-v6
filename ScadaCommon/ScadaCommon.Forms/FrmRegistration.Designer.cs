namespace Scada.Forms
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
            this.lblComCodeInfo = new System.Windows.Forms.Label();
            this.lblRegKey = new System.Windows.Forms.Label();
            this.txtRegKey = new System.Windows.Forms.TextBox();
            this.btnPasteRegKey = new System.Windows.Forms.Button();
            this.llblGetPermanentKey = new System.Windows.Forms.LinkLabel();
            this.llblGetTrialKey = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCompCode
            // 
            this.lblCompCode.AutoSize = true;
            this.lblCompCode.Location = new System.Drawing.Point(9, 9);
            this.lblCompCode.Name = "lblCompCode";
            this.lblCompCode.Size = new System.Drawing.Size(90, 15);
            this.lblCompCode.TabIndex = 0;
            this.lblCompCode.Text = "Computer code";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 27);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(510, 100);
            this.textBox1.TabIndex = 1;
            // 
            // btnCopyCompCode
            // 
            this.btnCopyCompCode.Location = new System.Drawing.Point(12, 133);
            this.btnCopyCompCode.Name = "btnCopyCompCode";
            this.btnCopyCompCode.Size = new System.Drawing.Size(75, 23);
            this.btnCopyCompCode.TabIndex = 2;
            this.btnCopyCompCode.Text = "Copy";
            this.btnCopyCompCode.UseVisualStyleBackColor = true;
            // 
            // pbInfo
            // 
            this.pbInfo.Image = global::Scada.Forms.Properties.Resources.info;
            this.pbInfo.Location = new System.Drawing.Point(93, 136);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(16, 16);
            this.pbInfo.TabIndex = 3;
            this.pbInfo.TabStop = false;
            // 
            // lblComCodeInfo
            // 
            this.lblComCodeInfo.AutoSize = true;
            this.lblComCodeInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblComCodeInfo.Location = new System.Drawing.Point(115, 137);
            this.lblComCodeInfo.Name = "lblComCodeInfo";
            this.lblComCodeInfo.Size = new System.Drawing.Size(379, 15);
            this.lblComCodeInfo.TabIndex = 4;
            this.lblComCodeInfo.Text = "If the code is missing, upload the configuration and restart the services";
            // 
            // lblRegKey
            // 
            this.lblRegKey.AutoSize = true;
            this.lblRegKey.Location = new System.Drawing.Point(9, 169);
            this.lblRegKey.Name = "lblRegKey";
            this.lblRegKey.Size = new System.Drawing.Size(91, 15);
            this.lblRegKey.TabIndex = 5;
            this.lblRegKey.Text = "Registration key";
            // 
            // txtRegKey
            // 
            this.txtRegKey.Location = new System.Drawing.Point(12, 187);
            this.txtRegKey.Name = "txtRegKey";
            this.txtRegKey.Size = new System.Drawing.Size(510, 23);
            this.txtRegKey.TabIndex = 6;
            // 
            // btnPasteRegKey
            // 
            this.btnPasteRegKey.Location = new System.Drawing.Point(12, 216);
            this.btnPasteRegKey.Name = "btnPasteRegKey";
            this.btnPasteRegKey.Size = new System.Drawing.Size(75, 23);
            this.btnPasteRegKey.TabIndex = 7;
            this.btnPasteRegKey.Text = "Paste";
            this.btnPasteRegKey.UseVisualStyleBackColor = true;
            // 
            // llblGetPermanentKey
            // 
            this.llblGetPermanentKey.AutoSize = true;
            this.llblGetPermanentKey.Location = new System.Drawing.Point(9, 252);
            this.llblGetPermanentKey.Name = "llblGetPermanentKey";
            this.llblGetPermanentKey.Size = new System.Drawing.Size(107, 15);
            this.llblGetPermanentKey.TabIndex = 8;
            this.llblGetPermanentKey.TabStop = true;
            this.llblGetPermanentKey.Text = "Get permanent key";
            // 
            // llblGetTrialKey
            // 
            this.llblGetTrialKey.AutoSize = true;
            this.llblGetTrialKey.Location = new System.Drawing.Point(9, 272);
            this.llblGetTrialKey.Name = "llblGetTrialKey";
            this.llblGetTrialKey.Size = new System.Drawing.Size(69, 15);
            this.llblGetTrialKey.TabIndex = 9;
            this.llblGetTrialKey.TabStop = true;
            this.llblGetTrialKey.Text = "Get trial key";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(366, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(447, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmRegistration
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(534, 335);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.llblGetTrialKey);
            this.Controls.Add(this.llblGetPermanentKey);
            this.Controls.Add(this.btnPasteRegKey);
            this.Controls.Add(this.txtRegKey);
            this.Controls.Add(this.lblRegKey);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.lblComCodeInfo);
            this.Controls.Add(this.btnCopyCompCode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblCompCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRegistration";
            this.ShowInTaskbar = false;
            this.Text = "Registration";
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCompCode;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnCopyCompCode;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Label lblComCodeInfo;
        private System.Windows.Forms.Label lblRegKey;
        private System.Windows.Forms.TextBox txtRegKey;
        private System.Windows.Forms.Button btnPasteRegKey;
        private System.Windows.Forms.LinkLabel llblGetPermanentKey;
        private System.Windows.Forms.LinkLabel llblGetTrialKey;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}