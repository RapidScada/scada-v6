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
            components = new System.ComponentModel.Container();
            gbSelectedDevice = new System.Windows.Forms.GroupBox();
            btnResetPollingOptions = new System.Windows.Forms.Button();
            btnDeviceProperties = new System.Windows.Forms.Button();
            txtCustomOptions = new System.Windows.Forms.TextBox();
            lblCustomOptions = new System.Windows.Forms.Label();
            txtCmdLine = new System.Windows.Forms.TextBox();
            lblCmdLine = new System.Windows.Forms.Label();
            dtpPeriod = new System.Windows.Forms.DateTimePicker();
            lblPeriod = new System.Windows.Forms.Label();
            dtpTime = new System.Windows.Forms.DateTimePicker();
            lblTime = new System.Windows.Forms.Label();
            numDelay = new System.Windows.Forms.NumericUpDown();
            lblDelay = new System.Windows.Forms.Label();
            numTimeout = new System.Windows.Forms.NumericUpDown();
            lblTimeout = new System.Windows.Forms.Label();
            txtStrAddress = new System.Windows.Forms.TextBox();
            lblStrAddress = new System.Windows.Forms.Label();
            numNumAddress = new System.Windows.Forms.NumericUpDown();
            lblNumAddress = new System.Windows.Forms.Label();
            cbDriver = new System.Windows.Forms.ComboBox();
            lblDriver = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            lblName = new System.Windows.Forms.Label();
            numDeviceNum = new System.Windows.Forms.NumericUpDown();
            lblDeviceNum = new System.Windows.Forms.Label();
            chkIsBound = new System.Windows.Forms.CheckBox();
            chkPollOnCmd = new System.Windows.Forms.CheckBox();
            chkActive = new System.Windows.Forms.CheckBox();
            lvDevicePolling = new System.Windows.Forms.ListView();
            colOrder = new System.Windows.Forms.ColumnHeader();
            colActive = new System.Windows.Forms.ColumnHeader();
            colPollOnCmd = new System.Windows.Forms.ColumnHeader();
            colIsBound = new System.Windows.Forms.ColumnHeader();
            colNumber = new System.Windows.Forms.ColumnHeader();
            colName = new System.Windows.Forms.ColumnHeader();
            colDriver = new System.Windows.Forms.ColumnHeader();
            colNumAddress = new System.Windows.Forms.ColumnHeader();
            colStrAddress = new System.Windows.Forms.ColumnHeader();
            colTimeout = new System.Windows.Forms.ColumnHeader();
            colDelay = new System.Windows.Forms.ColumnHeader();
            colTime = new System.Windows.Forms.ColumnHeader();
            colPeriod = new System.Windows.Forms.ColumnHeader();
            colCmdLine = new System.Windows.Forms.ColumnHeader();
            btnAddDevice = new System.Windows.Forms.Button();
            btnMoveUpDevice = new System.Windows.Forms.Button();
            btnMoveDownDevice = new System.Windows.Forms.Button();
            btnDeleteDevice = new System.Windows.Forms.Button();
            btnPasteDevice = new System.Windows.Forms.Button();
            btnCopyDevice = new System.Windows.Forms.Button();
            btnCutDevice = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            pnlTop = new System.Windows.Forms.Panel();
            pnlBottom = new System.Windows.Forms.Panel();
            gbSelectedDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numNumAddress).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDeviceNum).BeginInit();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // gbSelectedDevice
            // 
            gbSelectedDevice.Controls.Add(btnResetPollingOptions);
            gbSelectedDevice.Controls.Add(btnDeviceProperties);
            gbSelectedDevice.Controls.Add(txtCustomOptions);
            gbSelectedDevice.Controls.Add(lblCustomOptions);
            gbSelectedDevice.Controls.Add(txtCmdLine);
            gbSelectedDevice.Controls.Add(lblCmdLine);
            gbSelectedDevice.Controls.Add(dtpPeriod);
            gbSelectedDevice.Controls.Add(lblPeriod);
            gbSelectedDevice.Controls.Add(dtpTime);
            gbSelectedDevice.Controls.Add(lblTime);
            gbSelectedDevice.Controls.Add(numDelay);
            gbSelectedDevice.Controls.Add(lblDelay);
            gbSelectedDevice.Controls.Add(numTimeout);
            gbSelectedDevice.Controls.Add(lblTimeout);
            gbSelectedDevice.Controls.Add(txtStrAddress);
            gbSelectedDevice.Controls.Add(lblStrAddress);
            gbSelectedDevice.Controls.Add(numNumAddress);
            gbSelectedDevice.Controls.Add(lblNumAddress);
            gbSelectedDevice.Controls.Add(cbDriver);
            gbSelectedDevice.Controls.Add(lblDriver);
            gbSelectedDevice.Controls.Add(txtName);
            gbSelectedDevice.Controls.Add(lblName);
            gbSelectedDevice.Controls.Add(numDeviceNum);
            gbSelectedDevice.Controls.Add(lblDeviceNum);
            gbSelectedDevice.Controls.Add(chkIsBound);
            gbSelectedDevice.Controls.Add(chkPollOnCmd);
            gbSelectedDevice.Controls.Add(chkActive);
            gbSelectedDevice.Location = new System.Drawing.Point(0, 3);
            gbSelectedDevice.Name = "gbSelectedDevice";
            gbSelectedDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            gbSelectedDevice.Size = new System.Drawing.Size(650, 259);
            gbSelectedDevice.TabIndex = 0;
            gbSelectedDevice.TabStop = false;
            gbSelectedDevice.Text = "Selected Device";
            // 
            // btnResetPollingOptions
            // 
            btnResetPollingOptions.Location = new System.Drawing.Point(119, 223);
            btnResetPollingOptions.Name = "btnResetPollingOptions";
            btnResetPollingOptions.Size = new System.Drawing.Size(100, 23);
            btnResetPollingOptions.TabIndex = 26;
            btnResetPollingOptions.Text = "Reset";
            toolTip.SetToolTip(btnResetPollingOptions, "Set the device polling options to default");
            btnResetPollingOptions.UseVisualStyleBackColor = true;
            btnResetPollingOptions.Click += btnResetPollingOptions_Click;
            // 
            // btnDeviceProperties
            // 
            btnDeviceProperties.Location = new System.Drawing.Point(13, 223);
            btnDeviceProperties.Name = "btnDeviceProperties";
            btnDeviceProperties.Size = new System.Drawing.Size(100, 23);
            btnDeviceProperties.TabIndex = 25;
            btnDeviceProperties.Text = "Properies";
            btnDeviceProperties.UseVisualStyleBackColor = true;
            btnDeviceProperties.Click += btnDeviceProperties_Click;
            // 
            // txtCustomOptions
            // 
            txtCustomOptions.Location = new System.Drawing.Point(437, 62);
            txtCustomOptions.Multiline = true;
            txtCustomOptions.Name = "txtCustomOptions";
            txtCustomOptions.ReadOnly = true;
            txtCustomOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtCustomOptions.Size = new System.Drawing.Size(200, 155);
            txtCustomOptions.TabIndex = 24;
            txtCustomOptions.WordWrap = false;
            // 
            // lblCustomOptions
            // 
            lblCustomOptions.AutoSize = true;
            lblCustomOptions.Location = new System.Drawing.Point(434, 44);
            lblCustomOptions.Name = "lblCustomOptions";
            lblCustomOptions.Size = new System.Drawing.Size(49, 15);
            lblCustomOptions.TabIndex = 23;
            lblCustomOptions.Text = "Options";
            // 
            // txtCmdLine
            // 
            txtCmdLine.Location = new System.Drawing.Point(13, 194);
            txtCmdLine.Name = "txtCmdLine";
            txtCmdLine.Size = new System.Drawing.Size(418, 23);
            txtCmdLine.TabIndex = 22;
            txtCmdLine.TextChanged += txtCmdLine_TextChanged;
            // 
            // lblCmdLine
            // 
            lblCmdLine.AutoSize = true;
            lblCmdLine.Location = new System.Drawing.Point(10, 176);
            lblCmdLine.Name = "lblCmdLine";
            lblCmdLine.Size = new System.Drawing.Size(86, 15);
            lblCmdLine.TabIndex = 21;
            lblCmdLine.Text = "Command line";
            // 
            // dtpPeriod
            // 
            dtpPeriod.CustomFormat = "HH:mm:ss";
            dtpPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpPeriod.Location = new System.Drawing.Point(331, 150);
            dtpPeriod.Name = "dtpPeriod";
            dtpPeriod.ShowUpDown = true;
            dtpPeriod.Size = new System.Drawing.Size(100, 23);
            dtpPeriod.TabIndex = 20;
            dtpPeriod.Value = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            dtpPeriod.ValueChanged += dtpPeriod_ValueChanged;
            // 
            // lblPeriod
            // 
            lblPeriod.AutoSize = true;
            lblPeriod.Location = new System.Drawing.Point(328, 132);
            lblPeriod.Name = "lblPeriod";
            lblPeriod.Size = new System.Drawing.Size(41, 15);
            lblPeriod.TabIndex = 19;
            lblPeriod.Text = "Period";
            // 
            // dtpTime
            // 
            dtpTime.CustomFormat = "HH:mm:ss";
            dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtpTime.Location = new System.Drawing.Point(225, 150);
            dtpTime.Name = "dtpTime";
            dtpTime.ShowUpDown = true;
            dtpTime.Size = new System.Drawing.Size(100, 23);
            dtpTime.TabIndex = 18;
            dtpTime.Value = new System.DateTime(2021, 1, 1, 0, 0, 0, 0);
            dtpTime.ValueChanged += dtpTime_ValueChanged;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new System.Drawing.Point(222, 132);
            lblTime.Name = "lblTime";
            lblTime.Size = new System.Drawing.Size(33, 15);
            lblTime.TabIndex = 17;
            lblTime.Text = "Time";
            // 
            // numDelay
            // 
            numDelay.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numDelay.Location = new System.Drawing.Point(119, 150);
            numDelay.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
            numDelay.Name = "numDelay";
            numDelay.Size = new System.Drawing.Size(100, 23);
            numDelay.TabIndex = 16;
            numDelay.ValueChanged += numDelay_ValueChanged;
            // 
            // lblDelay
            // 
            lblDelay.AutoSize = true;
            lblDelay.Location = new System.Drawing.Point(116, 132);
            lblDelay.Name = "lblDelay";
            lblDelay.Size = new System.Drawing.Size(36, 15);
            lblDelay.TabIndex = 15;
            lblDelay.Text = "Delay";
            // 
            // numTimeout
            // 
            numTimeout.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numTimeout.Location = new System.Drawing.Point(13, 150);
            numTimeout.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new System.Drawing.Size(100, 23);
            numTimeout.TabIndex = 14;
            numTimeout.ValueChanged += numTimeout_ValueChanged;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new System.Drawing.Point(10, 132);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new System.Drawing.Size(51, 15);
            lblTimeout.TabIndex = 13;
            lblTimeout.Text = "Timeout";
            // 
            // txtStrAddress
            // 
            txtStrAddress.Location = new System.Drawing.Point(119, 106);
            txtStrAddress.Name = "txtStrAddress";
            txtStrAddress.Size = new System.Drawing.Size(312, 23);
            txtStrAddress.TabIndex = 12;
            txtStrAddress.TextChanged += txtStrAddress_TextChanged;
            // 
            // lblStrAddress
            // 
            lblStrAddress.AutoSize = true;
            lblStrAddress.Location = new System.Drawing.Point(116, 88);
            lblStrAddress.Name = "lblStrAddress";
            lblStrAddress.Size = new System.Drawing.Size(154, 15);
            lblStrAddress.TabIndex = 11;
            lblStrAddress.Text = "String address or host name";
            // 
            // numNumAddress
            // 
            numNumAddress.Location = new System.Drawing.Point(13, 106);
            numNumAddress.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numNumAddress.Name = "numNumAddress";
            numNumAddress.Size = new System.Drawing.Size(100, 23);
            numNumAddress.TabIndex = 10;
            numNumAddress.ValueChanged += numNumAddress_ValueChanged;
            // 
            // lblNumAddress
            // 
            lblNumAddress.AutoSize = true;
            lblNumAddress.Location = new System.Drawing.Point(10, 88);
            lblNumAddress.Name = "lblNumAddress";
            lblNumAddress.Size = new System.Drawing.Size(96, 15);
            lblNumAddress.TabIndex = 9;
            lblNumAddress.Text = "Numeric address";
            // 
            // cbDriver
            // 
            cbDriver.FormattingEnabled = true;
            cbDriver.Location = new System.Drawing.Point(331, 62);
            cbDriver.Name = "cbDriver";
            cbDriver.Size = new System.Drawing.Size(100, 23);
            cbDriver.TabIndex = 8;
            cbDriver.TextChanged += cbDriver_TextChanged;
            // 
            // lblDriver
            // 
            lblDriver.AutoSize = true;
            lblDriver.Location = new System.Drawing.Point(328, 44);
            lblDriver.Name = "lblDriver";
            lblDriver.Size = new System.Drawing.Size(38, 15);
            lblDriver.TabIndex = 7;
            lblDriver.Text = "Driver";
            // 
            // txtName
            // 
            txtName.Location = new System.Drawing.Point(119, 62);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(206, 23);
            txtName.TabIndex = 6;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(116, 44);
            lblName.Name = "lblName";
            lblName.Size = new System.Drawing.Size(39, 15);
            lblName.TabIndex = 5;
            lblName.Text = "Name";
            // 
            // numDeviceNum
            // 
            numDeviceNum.Location = new System.Drawing.Point(13, 62);
            numDeviceNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numDeviceNum.Name = "numDeviceNum";
            numDeviceNum.Size = new System.Drawing.Size(100, 23);
            numDeviceNum.TabIndex = 4;
            numDeviceNum.ValueChanged += numDeviceNum_ValueChanged;
            // 
            // lblDeviceNum
            // 
            lblDeviceNum.AutoSize = true;
            lblDeviceNum.Location = new System.Drawing.Point(10, 44);
            lblDeviceNum.Name = "lblDeviceNum";
            lblDeviceNum.Size = new System.Drawing.Size(51, 15);
            lblDeviceNum.TabIndex = 3;
            lblDeviceNum.Text = "Number";
            // 
            // chkIsBound
            // 
            chkIsBound.AutoSize = true;
            chkIsBound.Location = new System.Drawing.Point(331, 22);
            chkIsBound.Name = "chkIsBound";
            chkIsBound.Size = new System.Drawing.Size(220, 19);
            chkIsBound.TabIndex = 2;
            chkIsBound.Text = "Bound to the configuration database";
            chkIsBound.UseVisualStyleBackColor = true;
            chkIsBound.CheckedChanged += chkIsBound_CheckedChanged;
            // 
            // chkPollOnCmd
            // 
            chkPollOnCmd.AutoSize = true;
            chkPollOnCmd.Location = new System.Drawing.Point(119, 22);
            chkPollOnCmd.Name = "chkPollOnCmd";
            chkPollOnCmd.Size = new System.Drawing.Size(147, 19);
            chkPollOnCmd.TabIndex = 1;
            chkPollOnCmd.Text = "Poll only on command";
            chkPollOnCmd.UseVisualStyleBackColor = true;
            chkPollOnCmd.CheckedChanged += chkPollOnCmd_CheckedChanged;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Location = new System.Drawing.Point(13, 22);
            chkActive.Name = "chkActive";
            chkActive.Size = new System.Drawing.Size(59, 19);
            chkActive.TabIndex = 0;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            chkActive.CheckedChanged += chkActive_CheckedChanged;
            // 
            // lvDevicePolling
            // 
            lvDevicePolling.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colOrder, colActive, colPollOnCmd, colIsBound, colNumber, colName, colDriver, colNumAddress, colStrAddress, colTimeout, colDelay, colTime, colPeriod, colCmdLine });
            lvDevicePolling.Dock = System.Windows.Forms.DockStyle.Fill;
            lvDevicePolling.FullRowSelect = true;
            lvDevicePolling.GridLines = true;
            lvDevicePolling.Location = new System.Drawing.Point(0, 29);
            lvDevicePolling.Margin = new System.Windows.Forms.Padding(9, 3, 12, 3);
            lvDevicePolling.MultiSelect = false;
            lvDevicePolling.Name = "lvDevicePolling";
            lvDevicePolling.ShowItemToolTips = true;
            lvDevicePolling.Size = new System.Drawing.Size(700, 259);
            lvDevicePolling.TabIndex = 1;
            lvDevicePolling.UseCompatibleStateImageBehavior = false;
            lvDevicePolling.View = System.Windows.Forms.View.Details;
            lvDevicePolling.SelectedIndexChanged += lvDevicePolling_SelectedIndexChanged;
            lvDevicePolling.DoubleClick += lvDevicePolling_DoubleClick;
            // 
            // colOrder
            // 
            colOrder.Text = "#";
            colOrder.Width = 50;
            // 
            // colActive
            // 
            colActive.Text = "Active";
            colActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colPollOnCmd
            // 
            colPollOnCmd.Text = "Poll on Command";
            colPollOnCmd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colIsBound
            // 
            colIsBound.Text = "Bound";
            colIsBound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colNumber
            // 
            colNumber.Text = "Number";
            colNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 150;
            // 
            // colDriver
            // 
            colDriver.Text = "Driver";
            colDriver.Width = 100;
            // 
            // colNumAddress
            // 
            colNumAddress.Text = "Numerc Address";
            colNumAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colStrAddress
            // 
            colStrAddress.Text = "String Address";
            colStrAddress.Width = 150;
            // 
            // colTimeout
            // 
            colTimeout.Text = "Timeout";
            colTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colDelay
            // 
            colDelay.Text = "Delay";
            colDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colTime
            // 
            colTime.Text = "Time";
            colTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colPeriod
            // 
            colPeriod.Text = "Period";
            colPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colCmdLine
            // 
            colCmdLine.Text = "Command Line";
            colCmdLine.Width = 150;
            // 
            // btnAddDevice
            // 
            btnAddDevice.Location = new System.Drawing.Point(0, 0);
            btnAddDevice.Name = "btnAddDevice";
            btnAddDevice.Size = new System.Drawing.Size(80, 23);
            btnAddDevice.TabIndex = 0;
            btnAddDevice.Text = "Add";
            btnAddDevice.UseVisualStyleBackColor = true;
            btnAddDevice.Click += btnAddDevice_Click;
            // 
            // btnMoveUpDevice
            // 
            btnMoveUpDevice.Location = new System.Drawing.Point(86, 0);
            btnMoveUpDevice.Name = "btnMoveUpDevice";
            btnMoveUpDevice.Size = new System.Drawing.Size(80, 23);
            btnMoveUpDevice.TabIndex = 1;
            btnMoveUpDevice.Text = "Move Up";
            btnMoveUpDevice.UseVisualStyleBackColor = true;
            btnMoveUpDevice.Click += btnMoveUpDevice_Click;
            // 
            // btnMoveDownDevice
            // 
            btnMoveDownDevice.Location = new System.Drawing.Point(172, 0);
            btnMoveDownDevice.Name = "btnMoveDownDevice";
            btnMoveDownDevice.Size = new System.Drawing.Size(80, 23);
            btnMoveDownDevice.TabIndex = 2;
            btnMoveDownDevice.Text = "Move Down";
            btnMoveDownDevice.UseVisualStyleBackColor = true;
            btnMoveDownDevice.Click += btnMoveDownDevice_Click;
            // 
            // btnDeleteDevice
            // 
            btnDeleteDevice.Location = new System.Drawing.Point(258, 0);
            btnDeleteDevice.Name = "btnDeleteDevice";
            btnDeleteDevice.Size = new System.Drawing.Size(80, 23);
            btnDeleteDevice.TabIndex = 3;
            btnDeleteDevice.Text = "Delete";
            btnDeleteDevice.UseVisualStyleBackColor = true;
            btnDeleteDevice.Click += btnDeleteDevice_Click;
            // 
            // btnPasteDevice
            // 
            btnPasteDevice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPasteDevice.Location = new System.Drawing.Point(620, 0);
            btnPasteDevice.Name = "btnPasteDevice";
            btnPasteDevice.Size = new System.Drawing.Size(80, 23);
            btnPasteDevice.TabIndex = 6;
            btnPasteDevice.Text = "Paste";
            btnPasteDevice.UseVisualStyleBackColor = true;
            btnPasteDevice.Click += btnPasteDevice_Click;
            // 
            // btnCopyDevice
            // 
            btnCopyDevice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCopyDevice.Location = new System.Drawing.Point(534, 0);
            btnCopyDevice.Name = "btnCopyDevice";
            btnCopyDevice.Size = new System.Drawing.Size(80, 23);
            btnCopyDevice.TabIndex = 5;
            btnCopyDevice.Text = "Copy";
            btnCopyDevice.UseVisualStyleBackColor = true;
            btnCopyDevice.Click += btnCopyDevice_Click;
            // 
            // btnCutDevice
            // 
            btnCutDevice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCutDevice.Location = new System.Drawing.Point(448, 0);
            btnCutDevice.Name = "btnCutDevice";
            btnCutDevice.Size = new System.Drawing.Size(80, 23);
            btnCutDevice.TabIndex = 4;
            btnCutDevice.Text = "Cut";
            btnCutDevice.UseVisualStyleBackColor = true;
            btnCutDevice.Click += btnCutDevice_Click;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(btnPasteDevice);
            pnlTop.Controls.Add(btnCopyDevice);
            pnlTop.Controls.Add(btnCutDevice);
            pnlTop.Controls.Add(btnDeleteDevice);
            pnlTop.Controls.Add(btnMoveDownDevice);
            pnlTop.Controls.Add(btnMoveUpDevice);
            pnlTop.Controls.Add(btnAddDevice);
            pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            pnlTop.Location = new System.Drawing.Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new System.Drawing.Size(700, 29);
            pnlTop.TabIndex = 0;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(gbSelectedDevice);
            pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlBottom.Location = new System.Drawing.Point(0, 288);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new System.Drawing.Size(700, 262);
            pnlBottom.TabIndex = 2;
            // 
            // CtrlLinePolling
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(lvDevicePolling);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Name = "CtrlLinePolling";
            Size = new System.Drawing.Size(700, 550);
            Load += CtrlLineReqSequence_Load;
            gbSelectedDevice.ResumeLayout(false);
            gbSelectedDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numNumAddress).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDeviceNum).EndInit();
            pnlTop.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
    }
}
