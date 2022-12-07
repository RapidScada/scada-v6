namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    partial class FrmPasswordEncrypt
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
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.lblPasswordEncrypted = new System.Windows.Forms.Label();
            this.txtPasswordEncrypted = new System.Windows.Forms.TextBox();
            this.lblCopyMessage = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(9, 9);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 27);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(214, 23);
            this.txtPassword.TabIndex = 1;
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(232, 26);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(90, 23);
            this.btnEncrypt.TabIndex = 2;
            this.btnEncrypt.Text = "Encrypt";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            // 
            // lblPasswordEncrypted
            // 
            this.lblPasswordEncrypted.AutoSize = true;
            this.lblPasswordEncrypted.Location = new System.Drawing.Point(9, 53);
            this.lblPasswordEncrypted.Name = "lblPasswordEncrypted";
            this.lblPasswordEncrypted.Size = new System.Drawing.Size(113, 15);
            this.lblPasswordEncrypted.TabIndex = 3;
            this.lblPasswordEncrypted.Text = "Encrypted password";
            // 
            // txtPasswordEncrypted
            // 
            this.txtPasswordEncrypted.Location = new System.Drawing.Point(12, 71);
            this.txtPasswordEncrypted.Multiline = true;
            this.txtPasswordEncrypted.Name = "txtPasswordEncrypted";
            this.txtPasswordEncrypted.ReadOnly = true;
            this.txtPasswordEncrypted.Size = new System.Drawing.Size(310, 50);
            this.txtPasswordEncrypted.TabIndex = 4;
            // 
            // lblCopyMessage
            // 
            this.lblCopyMessage.AutoSize = true;
            this.lblCopyMessage.Location = new System.Drawing.Point(9, 141);
            this.lblCopyMessage.Name = "lblCopyMessage";
            this.lblCopyMessage.Size = new System.Drawing.Size(112, 15);
            this.lblCopyMessage.TabIndex = 5;
            this.lblCopyMessage.Text = "Copied to clipboard";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(247, 137);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmPasswordEncrypt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 172);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblCopyMessage);
            this.Controls.Add(this.txtPasswordEncrypted);
            this.Controls.Add(this.lblPasswordEncrypted);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPasswordEncrypt";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Encrypt Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Label lblPasswordEncrypted;
        private System.Windows.Forms.TextBox txtPasswordEncrypted;
        private System.Windows.Forms.Label lblCopyMessage;
        private System.Windows.Forms.Button btnClose;
    }
}