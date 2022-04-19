namespace Scada.Comm.Drivers.DrvEmail.View.Forms
{
    partial class FrmConfig
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
            this.btnAddressBook = new System.Windows.Forms.Button();
            this.chkEnableSsl = new System.Windows.Forms.CheckBox();
            this.txtSenderDisplayName = new System.Windows.Forms.TextBox();
            this.lblSenderDisplayName = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbServer = new System.Windows.Forms.GroupBox();
            this.gbSender = new System.Windows.Forms.GroupBox();
            this.lblSenderAddress = new System.Windows.Forms.Label();
            this.txtSenderAddress = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.gbServer.SuspendLayout();
            this.gbSender.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddressBook
            // 
            this.btnAddressBook.Location = new System.Drawing.Point(12, 293);
            this.btnAddressBook.Name = "btnAddressBook";
            this.btnAddressBook.Size = new System.Drawing.Size(130, 23);
            this.btnAddressBook.TabIndex = 2;
            this.btnAddressBook.Text = "Address Book";
            this.btnAddressBook.UseVisualStyleBackColor = true;
            this.btnAddressBook.Click += new System.EventHandler(this.btnAddressBook_Click);
            // 
            // chkEnableSsl
            // 
            this.chkEnableSsl.AutoSize = true;
            this.chkEnableSsl.Location = new System.Drawing.Point(13, 110);
            this.chkEnableSsl.Name = "chkEnableSsl";
            this.chkEnableSsl.Size = new System.Drawing.Size(66, 19);
            this.chkEnableSsl.TabIndex = 8;
            this.chkEnableSsl.Text = "Use SSL";
            this.chkEnableSsl.UseVisualStyleBackColor = true;
            // 
            // txtSenderDisplayName
            // 
            this.txtSenderDisplayName.Location = new System.Drawing.Point(13, 81);
            this.txtSenderDisplayName.Name = "txtSenderDisplayName";
            this.txtSenderDisplayName.Size = new System.Drawing.Size(334, 23);
            this.txtSenderDisplayName.TabIndex = 3;
            // 
            // lblSenderDisplayName
            // 
            this.lblSenderDisplayName.AutoSize = true;
            this.lblSenderDisplayName.Location = new System.Drawing.Point(10, 63);
            this.lblSenderDisplayName.Name = "lblSenderDisplayName";
            this.lblSenderDisplayName.Size = new System.Drawing.Size(78, 15);
            this.lblSenderDisplayName.TabIndex = 2;
            this.lblSenderDisplayName.Text = "Display name";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(13, 81);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 5;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(10, 63);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 4;
            this.lblUsername.Text = "Username";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(219, 37);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(128, 23);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(216, 19);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 15);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(13, 37);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(200, 23);
            this.txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(10, 19);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(65, 15);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Server host";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(219, 81);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(128, 23);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(216, 63);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 293);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbServer
            // 
            this.gbServer.Controls.Add(this.lblHost);
            this.gbServer.Controls.Add(this.txtHost);
            this.gbServer.Controls.Add(this.lblPort);
            this.gbServer.Controls.Add(this.txtPassword);
            this.gbServer.Controls.Add(this.chkEnableSsl);
            this.gbServer.Controls.Add(this.numPort);
            this.gbServer.Controls.Add(this.lblPassword);
            this.gbServer.Controls.Add(this.lblUsername);
            this.gbServer.Controls.Add(this.txtUsername);
            this.gbServer.Location = new System.Drawing.Point(12, 12);
            this.gbServer.Name = "gbServer";
            this.gbServer.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbServer.Size = new System.Drawing.Size(360, 142);
            this.gbServer.TabIndex = 0;
            this.gbServer.TabStop = false;
            this.gbServer.Text = "SMTP Server";
            // 
            // gbSender
            // 
            this.gbSender.Controls.Add(this.lblSenderAddress);
            this.gbSender.Controls.Add(this.txtSenderAddress);
            this.gbSender.Controls.Add(this.lblSenderDisplayName);
            this.gbSender.Controls.Add(this.txtSenderDisplayName);
            this.gbSender.Location = new System.Drawing.Point(12, 160);
            this.gbSender.Name = "gbSender";
            this.gbSender.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSender.Size = new System.Drawing.Size(360, 117);
            this.gbSender.TabIndex = 1;
            this.gbSender.TabStop = false;
            this.gbSender.Text = "From";
            // 
            // lblSenderAddress
            // 
            this.lblSenderAddress.AutoSize = true;
            this.lblSenderAddress.Location = new System.Drawing.Point(10, 19);
            this.lblSenderAddress.Name = "lblSenderAddress";
            this.lblSenderAddress.Size = new System.Drawing.Size(86, 15);
            this.lblSenderAddress.TabIndex = 0;
            this.lblSenderAddress.Text = "Sender address";
            // 
            // txtSenderAddress
            // 
            this.txtSenderAddress.Location = new System.Drawing.Point(13, 37);
            this.txtSenderAddress.Name = "txtSenderAddress";
            this.txtSenderAddress.Size = new System.Drawing.Size(334, 23);
            this.txtSenderAddress.TabIndex = 1;
            // 
            // FrmConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 328);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAddressBook);
            this.Controls.Add(this.gbSender);
            this.Controls.Add(this.gbServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device {0} Properties - Email";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.gbServer.ResumeLayout(false);
            this.gbServer.PerformLayout();
            this.gbSender.ResumeLayout(false);
            this.gbSender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAddressBook;
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
    }
}