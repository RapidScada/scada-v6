namespace Scada.Server.Modules.ModDeviceAlarm.View.Forms
{
    partial class FrmSmtpConfig
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
            chkEnableSsl = new CheckBox();
            txtSenderDisplayName = new TextBox();
            lblSenderDisplayName = new Label();
            txtUsername = new TextBox();
            lblUsername = new Label();
            numPort = new NumericUpDown();
            lblPort = new Label();
            txtHost = new TextBox();
            lblHost = new Label();
            txtPassword = new TextBox();
            lblPassword = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            gbServer = new GroupBox();
            gbSender = new GroupBox();
            lblSenderAddress = new Label();
            txtSenderAddress = new TextBox();
            btnSmtpText = new Button();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            gbServer.SuspendLayout();
            gbSender.SuspendLayout();
            SuspendLayout();
            // 
            // chkEnableSsl
            // 
            chkEnableSsl.AutoSize = true;
            chkEnableSsl.Location = new Point(13, 125);
            chkEnableSsl.Name = "chkEnableSsl";
            chkEnableSsl.Size = new Size(73, 21);
            chkEnableSsl.TabIndex = 8;
            chkEnableSsl.Text = "Use SSL";
            chkEnableSsl.UseVisualStyleBackColor = true;
            // 
            // txtSenderDisplayName
            // 
            txtSenderDisplayName.Location = new Point(13, 92);
            txtSenderDisplayName.Name = "txtSenderDisplayName";
            txtSenderDisplayName.Size = new Size(334, 23);
            txtSenderDisplayName.TabIndex = 3;
            // 
            // lblSenderDisplayName
            // 
            lblSenderDisplayName.AutoSize = true;
            lblSenderDisplayName.Location = new Point(10, 71);
            lblSenderDisplayName.Name = "lblSenderDisplayName";
            lblSenderDisplayName.Size = new Size(86, 17);
            lblSenderDisplayName.TabIndex = 2;
            lblSenderDisplayName.Text = "Display name";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(13, 92);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(200, 23);
            txtUsername.TabIndex = 5;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(10, 71);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(67, 17);
            lblUsername.TabIndex = 4;
            lblUsername.Text = "Username";
            // 
            // numPort
            // 
            numPort.Location = new Point(219, 42);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(128, 23);
            numPort.TabIndex = 3;
            numPort.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(216, 22);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(32, 17);
            lblPort.TabIndex = 2;
            lblPort.Text = "Port";
            // 
            // txtHost
            // 
            txtHost.Location = new Point(13, 42);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(200, 23);
            txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(10, 22);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(74, 17);
            lblHost.TabIndex = 0;
            lblHost.Text = "Server host";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(219, 92);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(128, 23);
            txtPassword.TabIndex = 7;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(216, 71);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(64, 17);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Password";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 332);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 26);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(297, 332);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 26);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbServer
            // 
            gbServer.Controls.Add(lblHost);
            gbServer.Controls.Add(txtHost);
            gbServer.Controls.Add(lblPort);
            gbServer.Controls.Add(txtPassword);
            gbServer.Controls.Add(chkEnableSsl);
            gbServer.Controls.Add(numPort);
            gbServer.Controls.Add(lblPassword);
            gbServer.Controls.Add(lblUsername);
            gbServer.Controls.Add(txtUsername);
            gbServer.Location = new Point(12, 14);
            gbServer.Name = "gbServer";
            gbServer.Padding = new Padding(10, 3, 10, 11);
            gbServer.Size = new Size(360, 161);
            gbServer.TabIndex = 0;
            gbServer.TabStop = false;
            gbServer.Text = "SMTP Server";
            // 
            // gbSender
            // 
            gbSender.Controls.Add(lblSenderAddress);
            gbSender.Controls.Add(txtSenderAddress);
            gbSender.Controls.Add(lblSenderDisplayName);
            gbSender.Controls.Add(txtSenderDisplayName);
            gbSender.Location = new Point(12, 181);
            gbSender.Name = "gbSender";
            gbSender.Padding = new Padding(10, 3, 10, 11);
            gbSender.Size = new Size(360, 133);
            gbSender.TabIndex = 1;
            gbSender.TabStop = false;
            gbSender.Text = "From";
            // 
            // lblSenderAddress
            // 
            lblSenderAddress.AutoSize = true;
            lblSenderAddress.Location = new Point(10, 22);
            lblSenderAddress.Name = "lblSenderAddress";
            lblSenderAddress.Size = new Size(100, 17);
            lblSenderAddress.TabIndex = 0;
            lblSenderAddress.Text = "Sender address";
            // 
            // txtSenderAddress
            // 
            txtSenderAddress.Location = new Point(13, 42);
            txtSenderAddress.Name = "txtSenderAddress";
            txtSenderAddress.Size = new Size(334, 23);
            txtSenderAddress.TabIndex = 1;
            // 
            // btnSmtpText
            // 
            btnSmtpText.Location = new Point(12, 332);
            btnSmtpText.Name = "btnSmtpText";
            btnSmtpText.Size = new Size(75, 26);
            btnSmtpText.TabIndex = 5;
            btnSmtpText.Text = "Test";
            btnSmtpText.UseVisualStyleBackColor = true;
            btnSmtpText.Click += btnSmtpText_Click;
            // 
            // FrmSmtpConfig
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 372);
            Controls.Add(btnSmtpText);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(gbSender);
            Controls.Add(gbServer);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSmtpConfig";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Smtp Properties - Email";
            FormClosing += FrmSmtpConfig_FormClosing;
            Load += FrmConfig_Load;
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            gbServer.ResumeLayout(false);
            gbServer.PerformLayout();
            gbSender.ResumeLayout(false);
            gbSender.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.CheckBox chkEnableSsl;
        private System.Windows.Forms.TextBox txtSenderDisplayName;
        private System.Windows.Forms.Label lblSenderDisplayName;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbServer;
        private System.Windows.Forms.GroupBox gbSender;
        private System.Windows.Forms.Label lblSenderAddress;
        private System.Windows.Forms.TextBox txtSenderAddress;
        private Button btnSmtpText;
    }
}