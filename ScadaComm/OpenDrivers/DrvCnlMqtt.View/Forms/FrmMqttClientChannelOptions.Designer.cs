namespace Scada.Comm.Drivers.DrvCnlMqtt.View.Forms
{
    partial class FrmMqttClientChannelOptions
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
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblClientID = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblProtocolVersion = new System.Windows.Forms.Label();
            this.cbProtocolVersion = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUseTls = new System.Windows.Forms.Label();
            this.chkUseTls = new System.Windows.Forms.CheckBox();
            this.lblCaCertFile = new System.Windows.Forms.Label();
            this.txtCaCertFile = new System.Windows.Forms.TextBox();
            this.btnCaCertFileBrowse = new System.Windows.Forms.Button();
            this.lblClientCertFile = new System.Windows.Forms.Label();
            this.txtClientCertFile = new System.Windows.Forms.TextBox();
            this.btnClientCertFileBrowse = new System.Windows.Forms.Button();
            this.lblClientCertPassword = new System.Windows.Forms.Label();
            this.txtClientCertPassword = new System.Windows.Forms.TextBox();
            this.chkAllowUntrustedCertificates = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCertificateRevocationErrors = new System.Windows.Forms.CheckBox();
            this.openCertFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(272, 41);
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
            this.numPort.Size = new System.Drawing.Size(100, 23);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] {
            1883,
            0,
            0,
            0});
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(12, 45);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(52, 15);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "TCP port";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(172, 12);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(200, 23);
            this.txtServer.TabIndex = 1;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(12, 16);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(39, 15);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            // 
            // lblClientID
            //
            this.lblClientID.AutoSize = true;
            this.lblClientID.Location = new System.Drawing.Point(12, 281);
            this.lblClientID.Name = "lblClientID";
            this.lblClientID.Size = new System.Drawing.Size(52, 15);
            this.lblClientID.TabIndex = 8;
            this.lblClientID.Text = "Client ID";
            //
            // txtClientID
            //
            this.txtClientID.Location = new System.Drawing.Point(172, 277);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.Size = new System.Drawing.Size(200, 23);
            this.txtClientID.TabIndex = 9;
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 310);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 10;
            this.lblUsername.Text = "Username";
            //
            // txtUsername
            //
            this.txtUsername.Location = new System.Drawing.Point(172, 306);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 11;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 339);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 12;
            this.lblPassword.Text = "Password";
            //
            // txtPassword
            //
            this.txtPassword.Location = new System.Drawing.Point(172, 335);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.TabIndex = 13;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(12, 74);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(73, 15);
            this.lblTimeout.TabIndex = 4;
            this.lblTimeout.Text = "Timeout, ms";
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(272, 70);
            this.numTimeout.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(100, 23);
            this.numTimeout.TabIndex = 5;
            this.numTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lblProtocolVersion
            //
            this.lblProtocolVersion.AutoSize = true;
            this.lblProtocolVersion.Location = new System.Drawing.Point(12, 368);
            this.lblProtocolVersion.Name = "lblProtocolVersion";
            this.lblProtocolVersion.Size = new System.Drawing.Size(93, 15);
            this.lblProtocolVersion.TabIndex = 14;
            this.lblProtocolVersion.Text = "Protocol version";
            //
            // cbProtocolVersion
            //
            this.cbProtocolVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtocolVersion.FormattingEnabled = true;
            this.cbProtocolVersion.Items.AddRange(new object[] {
            "Default",
            "3.1",
            "3.1.1",
            "5.0"});
            this.cbProtocolVersion.Location = new System.Drawing.Point(172, 364);
            this.cbProtocolVersion.Name = "cbProtocolVersion";
            this.cbProtocolVersion.Size = new System.Drawing.Size(200, 23);
            this.cbProtocolVersion.TabIndex = 15;
            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(252, 403);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(333, 403);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // lblUseTls
            //
            this.lblUseTls.AutoSize = true;
            this.lblUseTls.Location = new System.Drawing.Point(12, 103);
            this.lblUseTls.Name = "lblUseTls";
            this.lblUseTls.Size = new System.Drawing.Size(47, 15);
            this.lblUseTls.TabIndex = 6;
            this.lblUseTls.Text = "Use TLS";
            //
            // chkUseTls
            //
            this.chkUseTls.AutoSize = true;
            this.chkUseTls.Location = new System.Drawing.Point(357, 103);
            this.chkUseTls.Name = "chkUseTls";
            this.chkUseTls.Size = new System.Drawing.Size(15, 14);
            this.chkUseTls.TabIndex = 7;
            this.chkUseTls.UseVisualStyleBackColor = true;
            //
            // lblCaCertFile
            //
            this.lblCaCertFile.AutoSize = true;
            this.lblCaCertFile.Location = new System.Drawing.Point(12, 136);
            this.lblCaCertFile.Name = "lblCaCertFile";
            this.lblCaCertFile.Size = new System.Drawing.Size(103, 15);
            this.lblCaCertFile.TabIndex = 18;
            this.lblCaCertFile.Text = "CA certificate file";
            //
            // txtCaCertFile
            //
            this.txtCaCertFile.Location = new System.Drawing.Point(172, 132);
            this.txtCaCertFile.Name = "txtCaCertFile";
            this.txtCaCertFile.Size = new System.Drawing.Size(190, 23);
            this.txtCaCertFile.TabIndex = 19;
            //
            // btnCaCertFileBrowse
            //
            this.btnCaCertFileBrowse.Location = new System.Drawing.Point(368, 131);
            this.btnCaCertFileBrowse.Name = "btnCaCertFileBrowse";
            this.btnCaCertFileBrowse.Size = new System.Drawing.Size(28, 25);
            this.btnCaCertFileBrowse.TabIndex = 20;
            this.btnCaCertFileBrowse.Text = "...";
            this.btnCaCertFileBrowse.UseVisualStyleBackColor = true;
            this.btnCaCertFileBrowse.Click += new System.EventHandler(this.btnCaCertFileBrowse_Click);
            //
            // lblClientCertFile
            //
            this.lblClientCertFile.AutoSize = true;
            this.lblClientCertFile.Location = new System.Drawing.Point(12, 165);
            this.lblClientCertFile.Name = "lblClientCertFile";
            this.lblClientCertFile.Size = new System.Drawing.Size(94, 15);
            this.lblClientCertFile.TabIndex = 21;
            this.lblClientCertFile.Text = "Client cert file";
            //
            // txtClientCertFile
            //
            this.txtClientCertFile.Location = new System.Drawing.Point(172, 161);
            this.txtClientCertFile.Name = "txtClientCertFile";
            this.txtClientCertFile.Size = new System.Drawing.Size(190, 23);
            this.txtClientCertFile.TabIndex = 22;
            //
            // btnClientCertFileBrowse
            //
            this.btnClientCertFileBrowse.Location = new System.Drawing.Point(368, 160);
            this.btnClientCertFileBrowse.Name = "btnClientCertFileBrowse";
            this.btnClientCertFileBrowse.Size = new System.Drawing.Size(28, 25);
            this.btnClientCertFileBrowse.TabIndex = 23;
            this.btnClientCertFileBrowse.Text = "...";
            this.btnClientCertFileBrowse.UseVisualStyleBackColor = true;
            this.btnClientCertFileBrowse.Click += new System.EventHandler(this.btnClientCertFileBrowse_Click);
            //
            // lblClientCertPassword
            //
            this.lblClientCertPassword.AutoSize = true;
            this.lblClientCertPassword.Location = new System.Drawing.Point(12, 194);
            this.lblClientCertPassword.Name = "lblClientCertPassword";
            this.lblClientCertPassword.Size = new System.Drawing.Size(122, 15);
            this.lblClientCertPassword.TabIndex = 24;
            this.lblClientCertPassword.Text = "Client cert password";
            //
            // txtClientCertPassword
            //
            this.txtClientCertPassword.Location = new System.Drawing.Point(172, 190);
            this.txtClientCertPassword.Name = "txtClientCertPassword";
            this.txtClientCertPassword.Size = new System.Drawing.Size(200, 23);
            this.txtClientCertPassword.TabIndex = 25;
            this.txtClientCertPassword.UseSystemPasswordChar = true;
            //
            // chkAllowUntrustedCertificates
            //
            this.chkAllowUntrustedCertificates.AutoSize = true;
            this.chkAllowUntrustedCertificates.Location = new System.Drawing.Point(172, 219);
            this.chkAllowUntrustedCertificates.Name = "chkAllowUntrustedCertificates";
            this.chkAllowUntrustedCertificates.Size = new System.Drawing.Size(216, 19);
            this.chkAllowUntrustedCertificates.TabIndex = 17;
            this.chkAllowUntrustedCertificates.Text = "Allow untrusted certificates";
            this.chkAllowUntrustedCertificates.UseVisualStyleBackColor = true;
            //
            // chkIgnoreCertificateRevocationErrors
            //
            this.chkIgnoreCertificateRevocationErrors.AutoSize = true;
            this.chkIgnoreCertificateRevocationErrors.Location = new System.Drawing.Point(172, 248);
            this.chkIgnoreCertificateRevocationErrors.Name = "chkIgnoreCertificateRevocationErrors";
            this.chkIgnoreCertificateRevocationErrors.Size = new System.Drawing.Size(240, 19);
            this.chkIgnoreCertificateRevocationErrors.TabIndex = 18;
            this.chkIgnoreCertificateRevocationErrors.Text = "Ignore certificate revocation errors";
            this.chkIgnoreCertificateRevocationErrors.UseVisualStyleBackColor = true;
            //
            // openCertFileDialog
            //
            this.openCertFileDialog.Filter = "Certificate files|*.pem;*.crt;*.cer;*.pfx;*.p12|All files|*.*";
            //
            // FrmMqttClientChannelOptions
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(420, 438);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbProtocolVersion);
            this.Controls.Add(this.lblProtocolVersion);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtClientID);
            this.Controls.Add(this.lblClientID);
            this.Controls.Add(this.chkIgnoreCertificateRevocationErrors);
            this.Controls.Add(this.chkAllowUntrustedCertificates);
            this.Controls.Add(this.txtClientCertPassword);
            this.Controls.Add(this.lblClientCertPassword);
            this.Controls.Add(this.btnClientCertFileBrowse);
            this.Controls.Add(this.txtClientCertFile);
            this.Controls.Add(this.lblClientCertFile);
            this.Controls.Add(this.btnCaCertFileBrowse);
            this.Controls.Add(this.txtCaCertFile);
            this.Controls.Add(this.lblCaCertFile);
            this.Controls.Add(this.chkUseTls);
            this.Controls.Add(this.lblUseTls);
            this.Controls.Add(this.numTimeout);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMqttClientChannelOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MQTT Client Options";
            this.Load += new System.EventHandler(this.FrmMqttClientChannelOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericUpDown numPort;
        private Label lblPort;
        private TextBox txtServer;
        private Label lblServer;
        private Label lblClientID;
        private TextBox txtClientID;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblTimeout;
        private NumericUpDown numTimeout;
        private Label lblProtocolVersion;
        private ComboBox cbProtocolVersion;
        private Button btnOK;
        private Button btnCancel;
        private Label lblUseTls;
        private CheckBox chkUseTls;
        private Label lblCaCertFile;
        private TextBox txtCaCertFile;
        private Button btnCaCertFileBrowse;
        private Label lblClientCertFile;
        private TextBox txtClientCertFile;
        private Button btnClientCertFileBrowse;
        private Label lblClientCertPassword;
        private TextBox txtClientCertPassword;
        private CheckBox chkAllowUntrustedCertificates;
        private CheckBox chkIgnoreCertificateRevocationErrors;
        private OpenFileDialog openCertFileDialog;
    }
}