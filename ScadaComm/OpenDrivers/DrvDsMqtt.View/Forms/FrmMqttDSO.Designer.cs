namespace Scada.Comm.Drivers.DrvDsMqtt.View.Forms
{
    partial class FrmMqttDSO
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.pageConnectionOptions = new System.Windows.Forms.TabPage();
            this.cbProtocolVersion = new System.Windows.Forms.ComboBox();
            this.lblProtocolVersion = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.lblClientID = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.pagePublishOptions = new System.Windows.Forms.TabPage();
            this.btnSelectDevices = new System.Windows.Forms.Button();
            this.txtDeviceFilter = new System.Windows.Forms.TextBox();
            this.lblDeviceFilter = new System.Windows.Forms.Label();
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.lblDetailedLog = new System.Windows.Forms.Label();
            this.numDataLifetime = new System.Windows.Forms.NumericUpDown();
            this.lblDataLifetime = new System.Windows.Forms.Label();
            this.numMaxQueueSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxQueueSize = new System.Windows.Forms.Label();
            this.chkRetain = new System.Windows.Forms.CheckBox();
            this.lblRetain = new System.Windows.Forms.Label();
            this.cbQosLevel = new System.Windows.Forms.ComboBox();
            this.lblQosLevel = new System.Windows.Forms.Label();
            this.lblFormatExample = new System.Windows.Forms.Label();
            this.txtPublishFormat = new System.Windows.Forms.TextBox();
            this.lblPublishFormat = new System.Windows.Forms.Label();
            this.txtUndefinedValue = new System.Windows.Forms.TextBox();
            this.lblUndefinedValue = new System.Windows.Forms.Label();
            this.txtRootTopic = new System.Windows.Forms.TextBox();
            this.lblRootTopic = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.pageConnectionOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.pagePublishOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.pageConnectionOptions);
            this.tabControl.Controls.Add(this.pagePublishOptions);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 324);
            this.tabControl.TabIndex = 0;
            // 
            // pageConnectionOptions
            // 
            this.pageConnectionOptions.Controls.Add(this.cbProtocolVersion);
            this.pageConnectionOptions.Controls.Add(this.lblProtocolVersion);
            this.pageConnectionOptions.Controls.Add(this.numTimeout);
            this.pageConnectionOptions.Controls.Add(this.lblTimeout);
            this.pageConnectionOptions.Controls.Add(this.txtPassword);
            this.pageConnectionOptions.Controls.Add(this.lblPassword);
            this.pageConnectionOptions.Controls.Add(this.txtUsername);
            this.pageConnectionOptions.Controls.Add(this.lblUsername);
            this.pageConnectionOptions.Controls.Add(this.txtClientID);
            this.pageConnectionOptions.Controls.Add(this.lblClientID);
            this.pageConnectionOptions.Controls.Add(this.numPort);
            this.pageConnectionOptions.Controls.Add(this.lblPort);
            this.pageConnectionOptions.Controls.Add(this.txtServer);
            this.pageConnectionOptions.Controls.Add(this.lblServer);
            this.pageConnectionOptions.Location = new System.Drawing.Point(4, 24);
            this.pageConnectionOptions.Name = "pageConnectionOptions";
            this.pageConnectionOptions.Padding = new System.Windows.Forms.Padding(5);
            this.pageConnectionOptions.Size = new System.Drawing.Size(376, 296);
            this.pageConnectionOptions.TabIndex = 0;
            this.pageConnectionOptions.Text = "Connection";
            this.pageConnectionOptions.UseVisualStyleBackColor = true;
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
            this.cbProtocolVersion.Location = new System.Drawing.Point(168, 182);
            this.cbProtocolVersion.Name = "cbProtocolVersion";
            this.cbProtocolVersion.Size = new System.Drawing.Size(200, 23);
            this.cbProtocolVersion.TabIndex = 13;
            // 
            // lblProtocolVersion
            // 
            this.lblProtocolVersion.AutoSize = true;
            this.lblProtocolVersion.Location = new System.Drawing.Point(8, 186);
            this.lblProtocolVersion.Name = "lblProtocolVersion";
            this.lblProtocolVersion.Size = new System.Drawing.Size(93, 15);
            this.lblProtocolVersion.TabIndex = 12;
            this.lblProtocolVersion.Text = "Protocol version";
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(168, 153);
            this.numTimeout.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(100, 23);
            this.numTimeout.TabIndex = 11;
            this.numTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(8, 157);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(73, 15);
            this.lblTimeout.TabIndex = 10;
            this.lblTimeout.Text = "Timeout, ms";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(168, 124);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(8, 128);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(168, 95);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 7;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(8, 99);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username";
            // 
            // txtClientID
            // 
            this.txtClientID.Location = new System.Drawing.Point(168, 66);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.Size = new System.Drawing.Size(200, 23);
            this.txtClientID.TabIndex = 5;
            // 
            // lblClientID
            // 
            this.lblClientID.AutoSize = true;
            this.lblClientID.Location = new System.Drawing.Point(8, 70);
            this.lblClientID.Name = "lblClientID";
            this.lblClientID.Size = new System.Drawing.Size(52, 15);
            this.lblClientID.TabIndex = 4;
            this.lblClientID.Text = "Client ID";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(168, 37);
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
            this.lblPort.Location = new System.Drawing.Point(8, 41);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(52, 15);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "TCP port";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(168, 8);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(200, 23);
            this.txtServer.TabIndex = 1;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(8, 12);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(39, 15);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            // 
            // pagePublishOptions
            // 
            this.pagePublishOptions.Controls.Add(this.btnSelectDevices);
            this.pagePublishOptions.Controls.Add(this.txtDeviceFilter);
            this.pagePublishOptions.Controls.Add(this.lblDeviceFilter);
            this.pagePublishOptions.Controls.Add(this.chkDetailedLog);
            this.pagePublishOptions.Controls.Add(this.lblDetailedLog);
            this.pagePublishOptions.Controls.Add(this.numDataLifetime);
            this.pagePublishOptions.Controls.Add(this.lblDataLifetime);
            this.pagePublishOptions.Controls.Add(this.numMaxQueueSize);
            this.pagePublishOptions.Controls.Add(this.lblMaxQueueSize);
            this.pagePublishOptions.Controls.Add(this.chkRetain);
            this.pagePublishOptions.Controls.Add(this.lblRetain);
            this.pagePublishOptions.Controls.Add(this.cbQosLevel);
            this.pagePublishOptions.Controls.Add(this.lblQosLevel);
            this.pagePublishOptions.Controls.Add(this.lblFormatExample);
            this.pagePublishOptions.Controls.Add(this.txtPublishFormat);
            this.pagePublishOptions.Controls.Add(this.lblPublishFormat);
            this.pagePublishOptions.Controls.Add(this.txtUndefinedValue);
            this.pagePublishOptions.Controls.Add(this.lblUndefinedValue);
            this.pagePublishOptions.Controls.Add(this.txtRootTopic);
            this.pagePublishOptions.Controls.Add(this.lblRootTopic);
            this.pagePublishOptions.Location = new System.Drawing.Point(4, 24);
            this.pagePublishOptions.Name = "pagePublishOptions";
            this.pagePublishOptions.Padding = new System.Windows.Forms.Padding(5);
            this.pagePublishOptions.Size = new System.Drawing.Size(376, 296);
            this.pagePublishOptions.TabIndex = 1;
            this.pagePublishOptions.Text = "Publishing";
            this.pagePublishOptions.UseVisualStyleBackColor = true;
            // 
            // btnSelectDevices
            // 
            this.btnSelectDevices.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectDevices.Image = global::Scada.Comm.Drivers.DrvDsMqtt.View.Properties.Resources.find;
            this.btnSelectDevices.Location = new System.Drawing.Point(345, 265);
            this.btnSelectDevices.Name = "btnSelectDevices";
            this.btnSelectDevices.Size = new System.Drawing.Size(23, 23);
            this.btnSelectDevices.TabIndex = 19;
            this.btnSelectDevices.UseVisualStyleBackColor = true;
            this.btnSelectDevices.Click += new System.EventHandler(this.btnSelectDevices_Click);
            // 
            // txtDeviceFilter
            // 
            this.txtDeviceFilter.Location = new System.Drawing.Point(168, 265);
            this.txtDeviceFilter.Name = "txtDeviceFilter";
            this.txtDeviceFilter.Size = new System.Drawing.Size(171, 23);
            this.txtDeviceFilter.TabIndex = 18;
            // 
            // lblDeviceFilter
            // 
            this.lblDeviceFilter.AutoSize = true;
            this.lblDeviceFilter.Location = new System.Drawing.Point(8, 269);
            this.lblDeviceFilter.Name = "lblDeviceFilter";
            this.lblDeviceFilter.Size = new System.Drawing.Size(69, 15);
            this.lblDeviceFilter.TabIndex = 17;
            this.lblDeviceFilter.Text = "Device filter";
            // 
            // chkDetailedLog
            // 
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(261, 240);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.Size = new System.Drawing.Size(15, 14);
            this.chkDetailedLog.TabIndex = 16;
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            // 
            // lblDetailedLog
            // 
            this.lblDetailedLog.AutoSize = true;
            this.lblDetailedLog.Location = new System.Drawing.Point(8, 240);
            this.lblDetailedLog.Name = "lblDetailedLog";
            this.lblDetailedLog.Size = new System.Drawing.Size(70, 15);
            this.lblDetailedLog.TabIndex = 15;
            this.lblDetailedLog.Text = "Detailed log";
            // 
            // numDataLifetime
            // 
            this.numDataLifetime.Location = new System.Drawing.Point(168, 207);
            this.numDataLifetime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDataLifetime.Name = "numDataLifetime";
            this.numDataLifetime.Size = new System.Drawing.Size(200, 23);
            this.numDataLifetime.TabIndex = 14;
            // 
            // lblDataLifetime
            // 
            this.lblDataLifetime.AutoSize = true;
            this.lblDataLifetime.Location = new System.Drawing.Point(8, 211);
            this.lblDataLifetime.Name = "lblDataLifetime";
            this.lblDataLifetime.Size = new System.Drawing.Size(146, 15);
            this.lblDataLifetime.TabIndex = 13;
            this.lblDataLifetime.Text = "Data lifetime in queue, sec";
            // 
            // numMaxQueueSize
            // 
            this.numMaxQueueSize.Location = new System.Drawing.Point(168, 178);
            this.numMaxQueueSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMaxQueueSize.Name = "numMaxQueueSize";
            this.numMaxQueueSize.Size = new System.Drawing.Size(200, 23);
            this.numMaxQueueSize.TabIndex = 12;
            // 
            // lblMaxQueueSize
            // 
            this.lblMaxQueueSize.AutoSize = true;
            this.lblMaxQueueSize.Location = new System.Drawing.Point(8, 182);
            this.lblMaxQueueSize.Name = "lblMaxQueueSize";
            this.lblMaxQueueSize.Size = new System.Drawing.Size(120, 15);
            this.lblMaxQueueSize.TabIndex = 11;
            this.lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // chkRetain
            // 
            this.chkRetain.AutoSize = true;
            this.chkRetain.Location = new System.Drawing.Point(261, 153);
            this.chkRetain.Name = "chkRetain";
            this.chkRetain.Size = new System.Drawing.Size(15, 14);
            this.chkRetain.TabIndex = 10;
            this.chkRetain.UseVisualStyleBackColor = true;
            // 
            // lblRetain
            // 
            this.lblRetain.AutoSize = true;
            this.lblRetain.Location = new System.Drawing.Point(8, 153);
            this.lblRetain.Name = "lblRetain";
            this.lblRetain.Size = new System.Drawing.Size(40, 15);
            this.lblRetain.TabIndex = 9;
            this.lblRetain.Text = "Retain";
            // 
            // cbQosLevel
            // 
            this.cbQosLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQosLevel.FormattingEnabled = true;
            this.cbQosLevel.Items.AddRange(new object[] {
            "At most once (0)",
            "At least once (1)",
            "Exactly once (2)"});
            this.cbQosLevel.Location = new System.Drawing.Point(168, 120);
            this.cbQosLevel.Name = "cbQosLevel";
            this.cbQosLevel.Size = new System.Drawing.Size(200, 23);
            this.cbQosLevel.TabIndex = 8;
            // 
            // lblQosLevel
            // 
            this.lblQosLevel.AutoSize = true;
            this.lblQosLevel.Location = new System.Drawing.Point(8, 124);
            this.lblQosLevel.Name = "lblQosLevel";
            this.lblQosLevel.Size = new System.Drawing.Size(125, 15);
            this.lblQosLevel.TabIndex = 7;
            this.lblQosLevel.Text = "Quality of service level";
            // 
            // lblFormatExample
            // 
            this.lblFormatExample.AutoSize = true;
            this.lblFormatExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFormatExample.Location = new System.Drawing.Point(168, 92);
            this.lblFormatExample.Name = "lblFormatExample";
            this.lblFormatExample.Size = new System.Drawing.Size(150, 15);
            this.lblFormatExample.TabIndex = 6;
            this.lblFormatExample.Text = "{ \"Val\": @val, \"Stat\": @stat }";
            // 
            // txtPublishFormat
            // 
            this.txtPublishFormat.Location = new System.Drawing.Point(168, 66);
            this.txtPublishFormat.Name = "txtPublishFormat";
            this.txtPublishFormat.Size = new System.Drawing.Size(200, 23);
            this.txtPublishFormat.TabIndex = 5;
            // 
            // lblPublishFormat
            // 
            this.lblPublishFormat.AutoSize = true;
            this.lblPublishFormat.Location = new System.Drawing.Point(8, 70);
            this.lblPublishFormat.Name = "lblPublishFormat";
            this.lblPublishFormat.Size = new System.Drawing.Size(85, 15);
            this.lblPublishFormat.TabIndex = 4;
            this.lblPublishFormat.Text = "Publish format";
            // 
            // txtUndefinedValue
            // 
            this.txtUndefinedValue.Location = new System.Drawing.Point(168, 37);
            this.txtUndefinedValue.Name = "txtUndefinedValue";
            this.txtUndefinedValue.Size = new System.Drawing.Size(200, 23);
            this.txtUndefinedValue.TabIndex = 3;
            // 
            // lblUndefinedValue
            // 
            this.lblUndefinedValue.AutoSize = true;
            this.lblUndefinedValue.Location = new System.Drawing.Point(8, 41);
            this.lblUndefinedValue.Name = "lblUndefinedValue";
            this.lblUndefinedValue.Size = new System.Drawing.Size(93, 15);
            this.lblUndefinedValue.TabIndex = 2;
            this.lblUndefinedValue.Text = "Undefined value";
            // 
            // txtRootTopic
            // 
            this.txtRootTopic.Location = new System.Drawing.Point(168, 8);
            this.txtRootTopic.Name = "txtRootTopic";
            this.txtRootTopic.Size = new System.Drawing.Size(200, 23);
            this.txtRootTopic.TabIndex = 1;
            // 
            // lblRootTopic
            // 
            this.lblRootTopic.AutoSize = true;
            this.lblRootTopic.Location = new System.Drawing.Point(8, 12);
            this.lblRootTopic.Name = "lblRootTopic";
            this.lblRootTopic.Size = new System.Drawing.Size(62, 15);
            this.lblRootTopic.TabIndex = 0;
            this.lblRootTopic.Text = "Root topic";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 324);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(384, 41);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(297, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(216, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmMqttDSO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 365);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMqttDSO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Source Options";
            this.Load += new System.EventHandler(this.FrmMqttDSO_Load);
            this.tabControl.ResumeLayout(false);
            this.pageConnectionOptions.ResumeLayout(false);
            this.pageConnectionOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.pagePublishOptions.ResumeLayout(false);
            this.pagePublishOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl;
        private TabPage pageConnectionOptions;
        private TabPage pagePublishOptions;
        private Panel pnlBottom;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cbProtocolVersion;
        private Label lblProtocolVersion;
        private NumericUpDown numTimeout;
        private Label lblTimeout;
        private TextBox txtPassword;
        private Label lblPassword;
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtClientID;
        private Label lblClientID;
        private NumericUpDown numPort;
        private Label lblPort;
        private TextBox txtServer;
        private Label lblServer;
        private TextBox txtRootTopic;
        private Label lblRootTopic;
        private TextBox txtUndefinedValue;
        private Label lblUndefinedValue;
        private TextBox txtPublishFormat;
        private Label lblPublishFormat;
        private Label lblFormatExample;
        private ComboBox cbQosLevel;
        private Label lblQosLevel;
        private CheckBox chkRetain;
        private Label lblRetain;
        private Label lblMaxQueueSize;
        private NumericUpDown numMaxQueueSize;
        private NumericUpDown numDataLifetime;
        private Label lblDataLifetime;
        private CheckBox chkDetailedLog;
        private Label lblDetailedLog;
        private TextBox txtDeviceFilter;
        private Label lblDeviceFilter;
        private Button btnSelectDevices;
    }
}