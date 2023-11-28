namespace Scada.Comm.Drivers.DrvCnlGoogle.View.Forms
{
    partial class FrmGoogleCloudChannelOptions
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
            txtServer = new TextBox();
            lblServer = new Label();
            lblClientID = new Label();
            txtClientID = new TextBox();
            lblTimeout = new Label();
            numTimeout = new NumericUpDown();
            lblCredentialType = new Label();
            cbCredentialType = new ComboBox();
            btnOK = new Button();
            btnCancel = new Button();
            lblUseAdcFile = new Label();
            chkUseAdcFile = new CheckBox();
            gbSelfSignAccessToken = new GroupBox();
            txtServerKey = new TextBox();
            lblServerKey = new Label();
            txtClientSecret = new TextBox();
            lblClientSecret = new Label();
            gbAdcOptions = new GroupBox();
            txtAdcFilePath = new TextBox();
            lblAdcFilePath = new Label();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            gbSelfSignAccessToken.SuspendLayout();
            gbAdcOptions.SuspendLayout();
            SuspendLayout();
            // 
            // txtServer
            // 
            txtServer.Location = new Point(173, 22);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(200, 23);
            txtServer.TabIndex = 1;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(13, 26);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(45, 17);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server";
            // 
            // lblClientID
            // 
            lblClientID.AutoSize = true;
            lblClientID.Location = new Point(13, 116);
            lblClientID.Name = "lblClientID";
            lblClientID.Size = new Size(57, 17);
            lblClientID.TabIndex = 8;
            lblClientID.Text = "Client ID";
            // 
            // txtClientID
            // 
            txtClientID.Location = new Point(173, 111);
            txtClientID.Name = "txtClientID";
            txtClientID.Size = new Size(200, 23);
            txtClientID.TabIndex = 9;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new Point(13, 56);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new Size(79, 17);
            lblTimeout.TabIndex = 4;
            lblTimeout.Text = "Timeout, ms";
            // 
            // numTimeout
            // 
            numTimeout.Location = new Point(273, 51);
            numTimeout.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new Size(100, 23);
            numTimeout.TabIndex = 5;
            numTimeout.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            // 
            // lblCredentialType
            // 
            lblCredentialType.AutoSize = true;
            lblCredentialType.Location = new Point(12, 16);
            lblCredentialType.Name = "lblCredentialType";
            lblCredentialType.Size = new Size(96, 17);
            lblCredentialType.TabIndex = 14;
            lblCredentialType.Text = "Credential type";
            // 
            // cbCredentialType
            // 
            cbCredentialType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCredentialType.FormattingEnabled = true;
            cbCredentialType.Items.AddRange(new object[] { "ApplicationDefaultCredential(ADC)", "SelfSignAccessToken" });
            cbCredentialType.Location = new Point(179, 12);
            cbCredentialType.Name = "cbCredentialType";
            cbCredentialType.Size = new Size(200, 25);
            cbCredentialType.TabIndex = 15;
            cbCredentialType.SelectedIndexChanged += cbCredentialType_SelectedIndexChanged;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 305);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 26);
            btnOK.TabIndex = 16;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 305);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 26);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblUseAdcFile
            // 
            lblUseAdcFile.AutoSize = true;
            lblUseAdcFile.Location = new Point(13, 19);
            lblUseAdcFile.Name = "lblUseAdcFile";
            lblUseAdcFile.Size = new Size(76, 17);
            lblUseAdcFile.TabIndex = 6;
            lblUseAdcFile.Text = "Use adc file";
            // 
            // chkUseAdcFile
            // 
            chkUseAdcFile.AutoSize = true;
            chkUseAdcFile.Location = new Point(358, 19);
            chkUseAdcFile.Name = "chkUseAdcFile";
            chkUseAdcFile.Size = new Size(15, 14);
            chkUseAdcFile.TabIndex = 7;
            chkUseAdcFile.UseVisualStyleBackColor = true;
            // 
            // gbSelfSignAccessToken
            // 
            gbSelfSignAccessToken.Controls.Add(txtServerKey);
            gbSelfSignAccessToken.Controls.Add(lblServerKey);
            gbSelfSignAccessToken.Controls.Add(txtClientSecret);
            gbSelfSignAccessToken.Controls.Add(lblClientSecret);
            gbSelfSignAccessToken.Controls.Add(txtServer);
            gbSelfSignAccessToken.Controls.Add(lblServer);
            gbSelfSignAccessToken.Controls.Add(numTimeout);
            gbSelfSignAccessToken.Controls.Add(lblTimeout);
            gbSelfSignAccessToken.Controls.Add(txtClientID);
            gbSelfSignAccessToken.Controls.Add(lblClientID);
            gbSelfSignAccessToken.Location = new Point(6, 43);
            gbSelfSignAccessToken.Name = "gbSelfSignAccessToken";
            gbSelfSignAccessToken.Size = new Size(389, 176);
            gbSelfSignAccessToken.TabIndex = 18;
            gbSelfSignAccessToken.TabStop = false;
            gbSelfSignAccessToken.Text = "Access token options";
            // 
            // txtServerKey
            // 
            txtServerKey.Location = new Point(173, 80);
            txtServerKey.Name = "txtServerKey";
            txtServerKey.Size = new Size(200, 23);
            txtServerKey.TabIndex = 13;
            txtServerKey.UseSystemPasswordChar = true;
            // 
            // lblServerKey
            // 
            lblServerKey.AutoSize = true;
            lblServerKey.Location = new Point(13, 85);
            lblServerKey.Name = "lblServerKey";
            lblServerKey.Size = new Size(69, 17);
            lblServerKey.TabIndex = 12;
            lblServerKey.Text = "Server key";
            // 
            // txtClientSecret
            // 
            txtClientSecret.Location = new Point(173, 142);
            txtClientSecret.Name = "txtClientSecret";
            txtClientSecret.Size = new Size(200, 23);
            txtClientSecret.TabIndex = 11;
            // 
            // lblClientSecret
            // 
            lblClientSecret.AutoSize = true;
            lblClientSecret.Location = new Point(13, 147);
            lblClientSecret.Name = "lblClientSecret";
            lblClientSecret.Size = new Size(79, 17);
            lblClientSecret.TabIndex = 10;
            lblClientSecret.Text = "Client secret";
            // 
            // gbAdcOptions
            // 
            gbAdcOptions.Controls.Add(txtAdcFilePath);
            gbAdcOptions.Controls.Add(lblUseAdcFile);
            gbAdcOptions.Controls.Add(lblAdcFilePath);
            gbAdcOptions.Controls.Add(chkUseAdcFile);
            gbAdcOptions.Location = new Point(6, 225);
            gbAdcOptions.Name = "gbAdcOptions";
            gbAdcOptions.Size = new Size(389, 75);
            gbAdcOptions.TabIndex = 19;
            gbAdcOptions.TabStop = false;
            gbAdcOptions.Text = "ADC options";
            // 
            // txtAdcFilePath
            // 
            txtAdcFilePath.Location = new Point(173, 41);
            txtAdcFilePath.Name = "txtAdcFilePath";
            txtAdcFilePath.Size = new Size(200, 23);
            txtAdcFilePath.TabIndex = 15;
            // 
            // lblAdcFilePath
            // 
            lblAdcFilePath.AutoSize = true;
            lblAdcFilePath.Location = new Point(13, 46);
            lblAdcFilePath.Name = "lblAdcFilePath";
            lblAdcFilePath.Size = new Size(81, 17);
            lblAdcFilePath.TabIndex = 14;
            lblAdcFilePath.Text = "Adc file path";
            // 
            // FrmGoogleCloudChannelOptions
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(402, 339);
            Controls.Add(gbAdcOptions);
            Controls.Add(gbSelfSignAccessToken);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(cbCredentialType);
            Controls.Add(lblCredentialType);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmGoogleCloudChannelOptions";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Google Cloud Options";
            Load += FrmMqttClientChannelOptions_Load;
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            gbSelfSignAccessToken.ResumeLayout(false);
            gbSelfSignAccessToken.PerformLayout();
            gbAdcOptions.ResumeLayout(false);
            gbAdcOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtServer;
        private Label lblServer;
        private Label lblClientID;
        private TextBox txtClientID;
        private Label lblTimeout;
        private NumericUpDown numTimeout;
        private Label lblCredentialType;
        private ComboBox cbCredentialType;
        private Button btnOK;
        private Button btnCancel;
        private Label lblUseAdcFile;
        private CheckBox chkUseAdcFile;
        private GroupBox gbSelfSignAccessToken;
        private TextBox txtServerKey;
        private Label lblServerKey;
        private TextBox txtClientSecret;
        private Label lblClientSecret;
        private GroupBox gbAdcOptions;
        private TextBox txtAdcFilePath;
        private Label lblAdcFilePath;
    }
}