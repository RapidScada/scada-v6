namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
{
    partial class FrmSecurityOptions
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
            lblSecurityMode = new Label();
            cbSecurityMode = new ComboBox();
            cbSecurityPolicy = new ComboBox();
            lblSecurityPolicy = new Label();
            cbAuthenticationMode = new ComboBox();
            lblAuthenticationMode = new Label();
            pnlUsername = new Panel();
            txtPassword = new TextBox();
            lblPassword = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            pnlUsername.SuspendLayout();
            SuspendLayout();
            // 
            // lblSecurityMode
            // 
            lblSecurityMode.AutoSize = true;
            lblSecurityMode.Location = new Point(9, 9);
            lblSecurityMode.Name = "lblSecurityMode";
            lblSecurityMode.Size = new Size(83, 15);
            lblSecurityMode.TabIndex = 0;
            lblSecurityMode.Text = "Security mode";
            // 
            // cbSecurityMode
            // 
            cbSecurityMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSecurityMode.FormattingEnabled = true;
            cbSecurityMode.Location = new Point(12, 25);
            cbSecurityMode.Name = "cbSecurityMode";
            cbSecurityMode.Size = new Size(260, 23);
            cbSecurityMode.TabIndex = 1;
            cbSecurityMode.SelectedIndexChanged += cbSecurityMode_SelectedIndexChanged;
            // 
            // cbSecurityPolicy
            // 
            cbSecurityPolicy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSecurityPolicy.FormattingEnabled = true;
            cbSecurityPolicy.Location = new Point(12, 69);
            cbSecurityPolicy.Name = "cbSecurityPolicy";
            cbSecurityPolicy.Size = new Size(260, 23);
            cbSecurityPolicy.TabIndex = 3;
            // 
            // lblSecurityPolicy
            // 
            lblSecurityPolicy.AutoSize = true;
            lblSecurityPolicy.Location = new Point(9, 51);
            lblSecurityPolicy.Name = "lblSecurityPolicy";
            lblSecurityPolicy.Size = new Size(84, 15);
            lblSecurityPolicy.TabIndex = 2;
            lblSecurityPolicy.Text = "Security policy";
            // 
            // cbAuthenticationMode
            // 
            cbAuthenticationMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbAuthenticationMode.FormattingEnabled = true;
            cbAuthenticationMode.Location = new Point(12, 113);
            cbAuthenticationMode.Name = "cbAuthenticationMode";
            cbAuthenticationMode.Size = new Size(260, 23);
            cbAuthenticationMode.TabIndex = 5;
            cbAuthenticationMode.SelectedIndexChanged += cbAuthenticationMode_SelectedIndexChanged;
            // 
            // lblAuthenticationMode
            // 
            lblAuthenticationMode.AutoSize = true;
            lblAuthenticationMode.Location = new Point(9, 95);
            lblAuthenticationMode.Name = "lblAuthenticationMode";
            lblAuthenticationMode.Size = new Size(120, 15);
            lblAuthenticationMode.TabIndex = 4;
            lblAuthenticationMode.Text = "Authentication mode";
            // 
            // pnlUsername
            // 
            pnlUsername.Controls.Add(txtPassword);
            pnlUsername.Controls.Add(lblPassword);
            pnlUsername.Controls.Add(txtUsername);
            pnlUsername.Controls.Add(lblUsername);
            pnlUsername.Location = new Point(12, 142);
            pnlUsername.Name = "pnlUsername";
            pnlUsername.Size = new Size(260, 90);
            pnlUsername.TabIndex = 6;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(0, 62);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(260, 23);
            txtPassword.TabIndex = 3;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(-3, 44);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(0, 18);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(260, 23);
            txtUsername.TabIndex = 1;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(-3, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(60, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Username";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(116, 243);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 7;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(197, 243);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmSecurityOptions
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(284, 278);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(pnlUsername);
            Controls.Add(cbAuthenticationMode);
            Controls.Add(lblAuthenticationMode);
            Controls.Add(cbSecurityPolicy);
            Controls.Add(lblSecurityPolicy);
            Controls.Add(cbSecurityMode);
            Controls.Add(lblSecurityMode);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSecurityOptions";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Security Options";
            Load += FrmSecurityOptions_Load;
            pnlUsername.ResumeLayout(false);
            pnlUsername.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSecurityMode;
        private System.Windows.Forms.ComboBox cbSecurityMode;
        private System.Windows.Forms.ComboBox cbSecurityPolicy;
        private System.Windows.Forms.Label lblSecurityPolicy;
        private System.Windows.Forms.ComboBox cbAuthenticationMode;
        private System.Windows.Forms.Label lblAuthenticationMode;
        private System.Windows.Forms.Panel pnlUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}