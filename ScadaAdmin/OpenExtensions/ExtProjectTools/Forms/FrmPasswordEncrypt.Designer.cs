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
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblPasswordEncrypted = new Label();
            txtPasswordEncrypted = new TextBox();
            btnClose = new Button();
            btnCopy = new Button();
            SuspendLayout();
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(9, 9);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 0;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(12, 27);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(310, 23);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lblPasswordEncrypted
            // 
            lblPasswordEncrypted.AutoSize = true;
            lblPasswordEncrypted.Location = new Point(9, 53);
            lblPasswordEncrypted.Name = "lblPasswordEncrypted";
            lblPasswordEncrypted.Size = new Size(113, 15);
            lblPasswordEncrypted.TabIndex = 2;
            lblPasswordEncrypted.Text = "Encrypted password";
            // 
            // txtPasswordEncrypted
            // 
            txtPasswordEncrypted.Location = new Point(12, 71);
            txtPasswordEncrypted.Multiline = true;
            txtPasswordEncrypted.Name = "txtPasswordEncrypted";
            txtPasswordEncrypted.ReadOnly = true;
            txtPasswordEncrypted.Size = new Size(310, 50);
            txtPasswordEncrypted.TabIndex = 3;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(247, 137);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 5;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(151, 137);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(90, 23);
            btnCopy.TabIndex = 4;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // FrmPasswordEncrypt
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(334, 172);
            Controls.Add(btnClose);
            Controls.Add(btnCopy);
            Controls.Add(txtPasswordEncrypted);
            Controls.Add(lblPasswordEncrypted);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPasswordEncrypt";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Encrypt Password";
            Load += FrmPasswordEncrypt_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblPasswordEncrypted;
        private TextBox txtPasswordEncrypted;
        private Button btnClose;
        private Button btnCopy;
    }
}