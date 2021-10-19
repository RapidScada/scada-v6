namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    partial class CtrlLinePolling
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbSelectedDevice = new System.Windows.Forms.GroupBox();
            this.btnResetPollingOptions = new System.Windows.Forms.Button();
            this.btnDeviceProperties = new System.Windows.Forms.Button();
            this.txtCustomOptions = new System.Windows.Forms.TextBox();
            this.lblCustomOptions = new System.Windows.Forms.Label();
            this.txtCmdLine = new System.Windows.Forms.TextBox();
            this.lblCmdLine = new System.Windows.Forms.Label();
            this.dtpPeriod = new System.Windows.Forms.DateTimePicker();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.lblTime = new System.Windows.Forms.Label();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.txtStrAddress = new System.Windows.Forms.TextBox();
            this.lblStrAddress = new System.Windows.Forms.Label();
            this.numNumAddress = new System.Windows.Forms.NumericUpDown();
            this.lblNumAddress = new System.Windows.Forms.Label();
            this.cbDriver = new System.Windows.Forms.ComboBox();
            this.lblDriver = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numDeviceNum = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceNum = new System.Windows.Forms.Label();
            this.chkIsBound = new System.Windows.Forms.CheckBox();
            this.chkPollOnCmd = new System.Windows.Forms.CheckBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lvDevicePolling = new System.Windows.Forms.ListView();
            this.colOrder = new System.Windows.Forms.ColumnHeader();
            this.colActive = new System.Windows.Forms.ColumnHeader();
            this.colPollOnCmd = new System.Windows.Forms.ColumnHeader();
            this.colIsBound = new System.Windows.Forms.ColumnHeader();
            this.colNumber = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colDriver = new System.Windows.Forms.ColumnHeader();
            this.colNumAddress = new System.Windows.Forms.ColumnHeader();
            this.colStrAddress = new System.Windows.Forms.ColumnHeader();
            this.colTimeout = new System.Windows.Forms.ColumnHeader();
            this.colDelay = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colPeriod = new System.Windows.Forms.ColumnHeader();
            this.colCmdLine = new System.Windows.Forms.ColumnHeader();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.btnMoveUpDevice = new System.Windows.Forms.Button();
            this.btnMoveDownDevice = new System.Windows.Forms.Button();
            this.btnDeleteDevice = new System.Windows.Forms.Button();
            this.btnPasteDevice = new System.Windows.Forms.Button();
            this.btnCopyDevice = new System.Windows.Forms.Button();
            this.btnCutDevice = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbSelectedDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceNum)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSelectedDevice
            // 
            this.gbSelectedDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbSelectedDevice.Controls.Add(this.btnResetPollingOptions);
            this.gbSelectedDevice.Controls.Add(this.btnDeviceProperties);
            this.gbSelectedDevice.Controls.Add(this.txtCustomOptions);
            this.gbSelectedDevice.Controls.Add(this.lblCustomOptions);
            this.gbSelectedDevice.Controls.Add(this.txtCmdLine);
            this.gbSelectedDevice.Controls.Add(this.lblCmdLine);
            this.gbSelectedDevice.Controls.Add(this.dtpPeriod);
            this.gbSelectedDevice.Controls.Add(this.lblPeriod);
            this.gbSelectedDevice.Controls.Add(this.dtpTime);
            this.gbSelectedDevice.Controls.Add(this.lblTime);
            this.gbSelectedDevice.Controls.Add(this.numDelay);
            this.gbSelectedDevice.Controls.Add(this.lblDelay);
            this.gbSelectedDevice.Controls.Add(this.numTimeout);
            this.gbSelectedDevice.Controls.Add(this.lblTimeout);
            this.gbSelectedDevice.Controls.Add(this.txtStrAddress);
            this.gbSelectedDevice.Controls.Add(this.lblStrAddress);
            this.gbSelectedDevice.Controls.Add(this.numNumAddress);
            this.gbSelectedDevice.Controls.Add(this.lblNumAddress);
            this.gbSelectedDevice.Controls.Add(this.cbDriver);
            this.gbSelectedDevice.Controls.Add(this.lblDriver);
            this.gbSelectedDevice.Controls.Add(this.txtName);
            this.gbSelectedDevice.Controls.Add(this.lblName);
            this.gbSelectedDevice.Controls.Add(this.numDeviceNum);
            this.gbSelectedDevice.Controls.Add(this.lblDeviceNum);
            this.gbSelectedDevice.Controls.Add(this.chkIsBound);
            this.gbSelectedDevice.Controls.Add(this.chkPollOnCmd);
            this.gbSelectedDevice.Controls.Add(this.chkActive);
            this.gbSelectedDevice.Location = new System.Drawing.Point(9, 279);
            this.gbSelectedDevice.Margin = new System.Windows.Forms.Padding(9, 3, 3, 12);
            this.gbSelectedDevice.Name = "gbSelectedDevice";
            this.gbSelectedDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSelectedDevice.Size = new System.Drawing.Size(650, 259);
            this.gbSelectedDevice.TabIndex = 8;
            this.gbSelectedDevice.TabStop = false;
            this.gbSelectedDevice.Text = "Selected Device";
            // 
            // btnResetPollingOptions
            // 
            this.btnResetPollingOptions.Location = new System.Drawing.Point(119, 223);
            this.btnResetPollingOptions.Name = "btnResetPollingOptions";
            this.btnResetPollingOptions.Size = new System.Drawing.Size(100, 23);
            this.btnResetPollingOptions.TabIndex = 26;
            this.btnResetPollingOptions.Text = "Reset";
            this.toolTip.SetToolTip(this.btnResetPollingOptions, "Set the device polling options to default");
            this.btnResetPollingOptions.UseVisualStyleBackColor = true;
            this.btnResetPollingOptions.Click += new System.EventHandler(this.btnResetPollingOptions_Click);
            // 
            // btnDeviceProperties
            // 
            this.btnDeviceProperties.Location = new System.Drawing.Point(13, 223);
            this.btnDeviceProperties.Name = "btnDeviceProperties";
            this.btnDeviceProperties.Size = new System.Drawing.Size(100, 23);
            this.btnDeviceProperties.TabIndex = 25;
            this.btnDeviceProperties.Text = "Properies";
            this.btnDeviceProperties.UseVisualStyleBackColor = true;
            this.btnDeviceProperties.Click += new System.EventHandler(this.btnDeviceProperties_Click);
            // 
            // txtCustomOptions
            // 
            this.txtCustomOptions.Location = new System.Drawing.Point(437, 62);
            this.txtCustomOptions.Multiline = true;
            this.txtCustomOptions.Name = "txtCustomOptions";
            this.txtCustomOptions.ReadOnly = true;
            this.txtCustomOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCustomOptions.Size = new System.Drawing.Size(200, 155);
            this.txtCustomOptions.TabIndex = 24;
            this.txtCustomOptions.WordWrap = false;
            // 
            // lblCustomOptions
            // 
            this.lblCustomOptions.AutoSize = true;
            this.lblCustomOptions.Location = new System.Drawing.Point(434, 44);
            this.lblCustomOptions.Name = "lblCustomOptions";
            this.lblCustomOptions.Size = new System.Drawing.Size(49, 15);
            this.lblCustomOptions.TabIndex = 23;
            this.lblCustomOptions.Text = "Options";
            // 
            // txtCmdLine
            // 
            this.txtCmdLine.Location = new System.Drawing.Point(13, 194);
            this.txtCmdLine.Name = "txtCmdLine";
            this.txtCmdLine.Size = new System.Drawing.Size(418, 23);
            this.txtCmdLine.TabIndex = 22;
            this.txtCmdLine.TextChanged += new System.EventHandler(this.txtCmdLine_TextChanged);
            // 
            // lblCmdLine
            // 
            this.lblCmdLine.AutoSize = true;
            this.lblCmdLine.Location = new System.Drawing.Point(10, 176);
            this.lblCmdLine.Name = "lblCmdLine";
            this.lblCmdLine.Size = new System.Drawing.Size(86, 15);
            this.lblCmdLine.TabIndex = 21;
            this.lblCmdLine.Text = "Command line";
            // 
            // dtpPeriod
            // 
            this.dtpPeriod.CustomFormat = "HH:mm:ss";
            this.dtpPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPeriod.Location = new System.Drawing.Point(331, 150);
            this.dtpPeriod.Name = "dtpPeriod";
            this.dtpPeriod.ShowUpDown = true;
            this.dtpPeriod.Size = new System.Drawing.Size(100, 23);
            this.dtpPeriod.TabIndex = 20;
            this.dtpPeriod.Value = new System.DateTime(2018, 1, 1, 0, 1, 0, 0);
            this.dtpPeriod.ValueChanged += new System.EventHandler(this.dtpPeriod_ValueChanged);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(328, 132);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(41, 15);
            this.lblPeriod.TabIndex = 19;
            this.lblPeriod.Text = "Period";
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "HH:mm:ss";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(225, 150);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(100, 23);
            this.dtpTime.TabIndex = 18;
            this.dtpTime.Value = new System.DateTime(2018, 1, 1, 10, 0, 0, 0);
            this.dtpTime.ValueChanged += new System.EventHandler(this.dtpTime_ValueChanged);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(222, 132);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(33, 15);
            this.lblTime.TabIndex = 17;
            this.lblTime.Text = "Time";
            // 
            // numDelay
            // 
            this.numDelay.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numDelay.Location = new System.Drawing.Point(119, 150);
            this.numDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(100, 23);
            this.numDelay.TabIndex = 16;
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(116, 132);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(36, 15);
            this.lblDelay.TabIndex = 15;
            this.lblDelay.Text = "Delay";
            // 
            // numTimeout
            // 
            this.numTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numTimeout.Location = new System.Drawing.Point(13, 150);
            this.numTimeout.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(100, 23);
            this.numTimeout.TabIndex = 14;
            this.numTimeout.ValueChanged += new System.EventHandler(this.numTimeout_ValueChanged);
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(10, 132);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(51, 15);
            this.lblTimeout.TabIndex = 13;
            this.lblTimeout.Text = "Timeout";
            // 
            // txtStrAddress
            // 
            this.txtStrAddress.Location = new System.Drawing.Point(119, 106);
            this.txtStrAddress.Name = "txtStrAddress";
            this.txtStrAddress.Size = new System.Drawing.Size(312, 23);
            this.txtStrAddress.TabIndex = 12;
            this.txtStrAddress.TextChanged += new System.EventHandler(this.txtStrAddress_TextChanged);
            // 
            // lblStrAddress
            // 
            this.lblStrAddress.AutoSize = true;
            this.lblStrAddress.Location = new System.Drawing.Point(116, 88);
            this.lblStrAddress.Name = "lblStrAddress";
            this.lblStrAddress.Size = new System.Drawing.Size(154, 15);
            this.lblStrAddress.TabIndex = 11;
            this.lblStrAddress.Text = "String address or host name";
            // 
            // numNumAddress
            // 
            this.numNumAddress.Location = new System.Drawing.Point(13, 106);
            this.numNumAddress.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numNumAddress.Name = "numNumAddress";
            this.numNumAddress.Size = new System.Drawing.Size(100, 23);
            this.numNumAddress.TabIndex = 10;
            this.numNumAddress.ValueChanged += new System.EventHandler(this.numNumAddress_ValueChanged);
            // 
            // lblNumAddress
            // 
            this.lblNumAddress.AutoSize = true;
            this.lblNumAddress.Location = new System.Drawing.Point(10, 88);
            this.lblNumAddress.Name = "lblNumAddress";
            this.lblNumAddress.Size = new System.Drawing.Size(96, 15);
            this.lblNumAddress.TabIndex = 9;
            this.lblNumAddress.Text = "Numeric address";
            // 
            // cbDriver
            // 
            this.cbDriver.FormattingEnabled = true;
            this.cbDriver.Location = new System.Drawing.Point(331, 62);
            this.cbDriver.Name = "cbDriver";
            this.cbDriver.Size = new System.Drawing.Size(100, 23);
            this.cbDriver.TabIndex = 8;
            this.cbDriver.TextChanged += new System.EventHandler(this.cbDriver_TextChanged);
            // 
            // lblDriver
            // 
            this.lblDriver.AutoSize = true;
            this.lblDriver.Location = new System.Drawing.Point(328, 44);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(38, 15);
            this.lblDriver.TabIndex = 7;
            this.lblDriver.Text = "Driver";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(119, 62);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(206, 23);
            this.txtName.TabIndex = 6;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(116, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name";
            // 
            // numDeviceNum
            // 
            this.numDeviceNum.Location = new System.Drawing.Point(13, 62);
            this.numDeviceNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDeviceNum.Name = "numDeviceNum";
            this.numDeviceNum.Size = new System.Drawing.Size(100, 23);
            this.numDeviceNum.TabIndex = 4;
            this.numDeviceNum.ValueChanged += new System.EventHandler(this.numDeviceNum_ValueChanged);
            // 
            // lblDeviceNum
            // 
            this.lblDeviceNum.AutoSize = true;
            this.lblDeviceNum.Location = new System.Drawing.Point(10, 44);
            this.lblDeviceNum.Name = "lblDeviceNum";
            this.lblDeviceNum.Size = new System.Drawing.Size(51, 15);
            this.lblDeviceNum.TabIndex = 3;
            this.lblDeviceNum.Text = "Number";
            // 
            // chkIsBound
            // 
            this.chkIsBound.AutoSize = true;
            this.chkIsBound.Location = new System.Drawing.Point(331, 22);
            this.chkIsBound.Name = "chkIsBound";
            this.chkIsBound.Size = new System.Drawing.Size(220, 19);
            this.chkIsBound.TabIndex = 2;
            this.chkIsBound.Text = "Bound to the configuration database";
            this.chkIsBound.UseVisualStyleBackColor = true;
            this.chkIsBound.CheckedChanged += new System.EventHandler(this.chkIsBound_CheckedChanged);
            // 
            // chkPollOnCmd
            // 
            this.chkPollOnCmd.AutoSize = true;
            this.chkPollOnCmd.Location = new System.Drawing.Point(119, 22);
            this.chkPollOnCmd.Name = "chkPollOnCmd";
            this.chkPollOnCmd.Size = new System.Drawing.Size(147, 19);
            this.chkPollOnCmd.TabIndex = 1;
            this.chkPollOnCmd.Text = "Poll only on command";
            this.chkPollOnCmd.UseVisualStyleBackColor = true;
            this.chkPollOnCmd.CheckedChanged += new System.EventHandler(this.chkPollOnCmd_CheckedChanged);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 22);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(59, 19);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            this.chkActive.CheckedChanged += new System.EventHandler(this.chkActive_CheckedChanged);
            // 
            // lvDevicePolling
            // 
            this.lvDevicePolling.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevicePolling.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colOrder,
            this.colActive,
            this.colPollOnCmd,
            this.colIsBound,
            this.colNumber,
            this.colName,
            this.colDriver,
            this.colNumAddress,
            this.colStrAddress,
            this.colTimeout,
            this.colDelay,
            this.colTime,
            this.colPeriod,
            this.colCmdLine});
            this.lvDevicePolling.FullRowSelect = true;
            this.lvDevicePolling.GridLines = true;
            this.lvDevicePolling.HideSelection = false;
            this.lvDevicePolling.Location = new System.Drawing.Point(9, 41);
            this.lvDevicePolling.Margin = new System.Windows.Forms.Padding(9, 3, 12, 3);
            this.lvDevicePolling.MultiSelect = false;
            this.lvDevicePolling.Name = "lvDevicePolling";
            this.lvDevicePolling.ShowItemToolTips = true;
            this.lvDevicePolling.Size = new System.Drawing.Size(679, 232);
            this.lvDevicePolling.TabIndex = 7;
            this.lvDevicePolling.UseCompatibleStateImageBehavior = false;
            this.lvDevicePolling.View = System.Windows.Forms.View.Details;
            this.lvDevicePolling.SelectedIndexChanged += new System.EventHandler(this.lvDevicePolling_SelectedIndexChanged);
            this.lvDevicePolling.DoubleClick += new System.EventHandler(this.lvDevicePolling_DoubleClick);
            // 
            // colOrder
            // 
            this.colOrder.Text = "#";
            this.colOrder.Width = 50;
            // 
            // colActive
            // 
            this.colActive.Text = "Active";
            this.colActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colPollOnCmd
            // 
            this.colPollOnCmd.Text = "Poll on Command";
            // 
            // colIsBound
            // 
            this.colIsBound.Text = "Bound";
            this.colIsBound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colNumber
            // 
            this.colNumber.Text = "Number";
            this.colNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 150;
            // 
            // colDriver
            // 
            this.colDriver.Text = "Driver";
            this.colDriver.Width = 100;
            // 
            // colNumAddress
            // 
            this.colNumAddress.Text = "Numerc Address";
            this.colNumAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colStrAddress
            // 
            this.colStrAddress.Text = "String Address";
            this.colStrAddress.Width = 100;
            // 
            // colTimeout
            // 
            this.colTimeout.Text = "Timeout";
            this.colTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colDelay
            // 
            this.colDelay.Text = "Delay";
            this.colDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colPeriod
            // 
            this.colPeriod.Text = "Period";
            this.colPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colCmdLine
            // 
            this.colCmdLine.Text = "Command Line";
            this.colCmdLine.Width = 150;
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.Location = new System.Drawing.Point(9, 12);
            this.btnAddDevice.Margin = new System.Windows.Forms.Padding(9, 12, 3, 3);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(80, 23);
            this.btnAddDevice.TabIndex = 0;
            this.btnAddDevice.Text = "Add";
            this.btnAddDevice.UseVisualStyleBackColor = true;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // btnMoveUpDevice
            // 
            this.btnMoveUpDevice.Location = new System.Drawing.Point(95, 12);
            this.btnMoveUpDevice.Name = "btnMoveUpDevice";
            this.btnMoveUpDevice.Size = new System.Drawing.Size(80, 23);
            this.btnMoveUpDevice.TabIndex = 1;
            this.btnMoveUpDevice.Text = "Move Up";
            this.btnMoveUpDevice.UseVisualStyleBackColor = true;
            this.btnMoveUpDevice.Click += new System.EventHandler(this.btnMoveUpDevice_Click);
            // 
            // btnMoveDownDevice
            // 
            this.btnMoveDownDevice.Location = new System.Drawing.Point(181, 12);
            this.btnMoveDownDevice.Name = "btnMoveDownDevice";
            this.btnMoveDownDevice.Size = new System.Drawing.Size(80, 23);
            this.btnMoveDownDevice.TabIndex = 2;
            this.btnMoveDownDevice.Text = "Move Down";
            this.btnMoveDownDevice.UseVisualStyleBackColor = true;
            this.btnMoveDownDevice.Click += new System.EventHandler(this.btnMoveDownDevice_Click);
            // 
            // btnDeleteDevice
            // 
            this.btnDeleteDevice.Location = new System.Drawing.Point(267, 12);
            this.btnDeleteDevice.Name = "btnDeleteDevice";
            this.btnDeleteDevice.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteDevice.TabIndex = 3;
            this.btnDeleteDevice.Text = "Delete";
            this.btnDeleteDevice.UseVisualStyleBackColor = true;
            this.btnDeleteDevice.Click += new System.EventHandler(this.btnDeleteDevice_Click);
            // 
            // btnPasteDevice
            // 
            this.btnPasteDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPasteDevice.Location = new System.Drawing.Point(608, 12);
            this.btnPasteDevice.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.btnPasteDevice.Name = "btnPasteDevice";
            this.btnPasteDevice.Size = new System.Drawing.Size(80, 23);
            this.btnPasteDevice.TabIndex = 6;
            this.btnPasteDevice.Text = "Paste";
            this.btnPasteDevice.UseVisualStyleBackColor = true;
            this.btnPasteDevice.Click += new System.EventHandler(this.btnPasteDevice_Click);
            // 
            // btnCopyDevice
            // 
            this.btnCopyDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyDevice.Location = new System.Drawing.Point(522, 12);
            this.btnCopyDevice.Name = "btnCopyDevice";
            this.btnCopyDevice.Size = new System.Drawing.Size(80, 23);
            this.btnCopyDevice.TabIndex = 5;
            this.btnCopyDevice.Text = "Copy";
            this.btnCopyDevice.UseVisualStyleBackColor = true;
            this.btnCopyDevice.Click += new System.EventHandler(this.btnCopyDevice_Click);
            // 
            // btnCutDevice
            // 
            this.btnCutDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCutDevice.Location = new System.Drawing.Point(436, 12);
            this.btnCutDevice.Name = "btnCutDevice";
            this.btnCutDevice.Size = new System.Drawing.Size(80, 23);
            this.btnCutDevice.TabIndex = 4;
            this.btnCutDevice.Text = "Cut";
            this.btnCutDevice.UseVisualStyleBackColor = true;
            this.btnCutDevice.Click += new System.EventHandler(this.btnCutDevice_Click);
            // 
            // CtrlLinePolling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbSelectedDevice);
            this.Controls.Add(this.lvDevicePolling);
            this.Controls.Add(this.btnPasteDevice);
            this.Controls.Add(this.btnCopyDevice);
            this.Controls.Add(this.btnCutDevice);
            this.Controls.Add(this.btnDeleteDevice);
            this.Controls.Add(this.btnMoveDownDevice);
            this.Controls.Add(this.btnMoveUpDevice);
            this.Controls.Add(this.btnAddDevice);
            this.Name = "CtrlLinePolling";
            this.Size = new System.Drawing.Size(700, 550);
            this.Load += new System.EventHandler(this.CtrlLineReqSequence_Load);
            this.gbSelectedDevice.ResumeLayout(false);
            this.gbSelectedDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelectedDevice;
        private System.Windows.Forms.ComboBox cbDriver;
        private System.Windows.Forms.TextBox txtCmdLine;
        private System.Windows.Forms.DateTimePicker dtpPeriod;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label lblCmdLine;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label lblStrAddress;
        private System.Windows.Forms.TextBox txtStrAddress;
        private System.Windows.Forms.Label lblNumAddress;
        private System.Windows.Forms.NumericUpDown numNumAddress;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numDeviceNum;
        private System.Windows.Forms.Label lblDeviceNum;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkIsBound;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.ListView lvDevicePolling;
        private System.Windows.Forms.ColumnHeader colOrder;
        private System.Windows.Forms.ColumnHeader colActive;
        private System.Windows.Forms.ColumnHeader colIsBound;
        private System.Windows.Forms.ColumnHeader colNumber;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colDriver;
        private System.Windows.Forms.ColumnHeader colNumAddress;
        private System.Windows.Forms.ColumnHeader colStrAddress;
        private System.Windows.Forms.ColumnHeader colTimeout;
        private System.Windows.Forms.ColumnHeader colDelay;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colPeriod;
        private System.Windows.Forms.ColumnHeader colCmdLine;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.Button btnMoveUpDevice;
        private System.Windows.Forms.Button btnMoveDownDevice;
        private System.Windows.Forms.Button btnDeleteDevice;
        private System.Windows.Forms.Button btnPasteDevice;
        private System.Windows.Forms.Button btnCopyDevice;
        private System.Windows.Forms.Button btnCutDevice;
        private System.Windows.Forms.Button btnDeviceProperties;
        private System.Windows.Forms.Button btnResetPollingOptions;
        internal System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox txtCustomOptions;
        private System.Windows.Forms.Label lblCustomOptions;
        private System.Windows.Forms.CheckBox chkPollOnCmd;
        private System.Windows.Forms.ColumnHeader colPollOnCmd;
    }
}
