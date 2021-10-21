namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    partial class FrmUdpChannelOptions
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
            this.components = new System.ComponentModel.Container();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.pbDeviceMappingHint = new System.Windows.Forms.PictureBox();
            this.cbDeviceMapping = new System.Windows.Forms.ComboBox();
            this.lblDeviceMapping = new System.Windows.Forms.Label();
            this.pbBehaviorHint = new System.Windows.Forms.PictureBox();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.pbRemoteIpAddressHint = new System.Windows.Forms.PictureBox();
            this.txtRemoteIpAddress = new System.Windows.Forms.TextBox();
            this.lblRemoteIpAddress = new System.Windows.Forms.Label();
            this.pbRemoteUdpPortHint = new System.Windows.Forms.PictureBox();
            this.numRemoteUdpPort = new System.Windows.Forms.NumericUpDown();
            this.lblRemoteUdpPort = new System.Windows.Forms.Label();
            this.pbLocalUdpPortHint = new System.Windows.Forms.PictureBox();
            this.numLocalUdpPort = new System.Windows.Forms.NumericUpDown();
            this.lblLocalUdpPort = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeviceMappingHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteIpAddressHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteUdpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteUdpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalUdpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalUdpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.pbDeviceMappingHint);
            this.gbMode.Controls.Add(this.cbDeviceMapping);
            this.gbMode.Controls.Add(this.lblDeviceMapping);
            this.gbMode.Controls.Add(this.pbBehaviorHint);
            this.gbMode.Controls.Add(this.cbBehavior);
            this.gbMode.Controls.Add(this.lblBehavior);
            this.gbMode.Location = new System.Drawing.Point(12, 12);
            this.gbMode.Name = "gbMode";
            this.gbMode.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbMode.Size = new System.Drawing.Size(360, 87);
            this.gbMode.TabIndex = 0;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Operating Mode";
            // 
            // pbDeviceMappingHint
            // 
            this.pbDeviceMappingHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbDeviceMappingHint.Location = new System.Drawing.Point(331, 54);
            this.pbDeviceMappingHint.Name = "pbDeviceMappingHint";
            this.pbDeviceMappingHint.Size = new System.Drawing.Size(16, 16);
            this.pbDeviceMappingHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDeviceMappingHint.TabIndex = 4;
            this.pbDeviceMappingHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbDeviceMappingHint, "\r\n");
            // 
            // cbDeviceMapping
            // 
            this.cbDeviceMapping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeviceMapping.FormattingEnabled = true;
            this.cbDeviceMapping.Items.AddRange(new object[] {
            "By IP address",
            "By hello packet",
            "Driver determined"});
            this.cbDeviceMapping.Location = new System.Drawing.Point(175, 51);
            this.cbDeviceMapping.Name = "cbDeviceMapping";
            this.cbDeviceMapping.Size = new System.Drawing.Size(150, 23);
            this.cbDeviceMapping.TabIndex = 3;
            // 
            // lblDeviceMapping
            // 
            this.lblDeviceMapping.AutoSize = true;
            this.lblDeviceMapping.Location = new System.Drawing.Point(13, 55);
            this.lblDeviceMapping.Name = "lblDeviceMapping";
            this.lblDeviceMapping.Size = new System.Drawing.Size(93, 15);
            this.lblDeviceMapping.TabIndex = 2;
            this.lblDeviceMapping.Text = "Device mapping";
            // 
            // pbBehaviorHint
            // 
            this.pbBehaviorHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbBehaviorHint.Location = new System.Drawing.Point(331, 25);
            this.pbBehaviorHint.Name = "pbBehaviorHint";
            this.pbBehaviorHint.Size = new System.Drawing.Size(16, 16);
            this.pbBehaviorHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBehaviorHint.TabIndex = 5;
            this.pbBehaviorHint.TabStop = false;
            this.toolTip.SetToolTip(this.pbBehaviorHint, "\r\n");
            // 
            // cbBehavior
            // 
            this.cbBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBehavior.FormattingEnabled = true;
            this.cbBehavior.Items.AddRange(new object[] {
            "Master",
            "Slave"});
            this.cbBehavior.Location = new System.Drawing.Point(175, 22);
            this.cbBehavior.Name = "cbBehavior";
            this.cbBehavior.Size = new System.Drawing.Size(150, 23);
            this.cbBehavior.TabIndex = 1;
            this.cbBehavior.SelectedIndexChanged += new System.EventHandler(this.cbBehavior_SelectedIndexChanged);
            // 
            // lblBehavior
            // 
            this.lblBehavior.AutoSize = true;
            this.lblBehavior.Location = new System.Drawing.Point(13, 26);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(53, 15);
            this.lblBehavior.TabIndex = 0;
            this.lblBehavior.Text = "Behavior";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.pbRemoteIpAddressHint);
            this.gbConnection.Controls.Add(this.txtRemoteIpAddress);
            this.gbConnection.Controls.Add(this.lblRemoteIpAddress);
            this.gbConnection.Controls.Add(this.pbRemoteUdpPortHint);
            this.gbConnection.Controls.Add(this.numRemoteUdpPort);
            this.gbConnection.Controls.Add(this.lblRemoteUdpPort);
            this.gbConnection.Controls.Add(this.pbLocalUdpPortHint);
            this.gbConnection.Controls.Add(this.numLocalUdpPort);
            this.gbConnection.Controls.Add(this.lblLocalUdpPort);
            this.gbConnection.Location = new System.Drawing.Point(12, 105);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(360, 116);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // pbRemoteIpAddressHint
            // 
            this.pbRemoteIpAddressHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbRemoteIpAddressHint.Location = new System.Drawing.Point(331, 83);
            this.pbRemoteIpAddressHint.Name = "pbRemoteIpAddressHint";
            this.pbRemoteIpAddressHint.Size = new System.Drawing.Size(16, 16);
            this.pbRemoteIpAddressHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRemoteIpAddressHint.TabIndex = 9;
            this.pbRemoteIpAddressHint.TabStop = false;
            // 
            // txtRemoteIpAddress
            // 
            this.txtRemoteIpAddress.Location = new System.Drawing.Point(175, 80);
            this.txtRemoteIpAddress.Name = "txtRemoteIpAddress";
            this.txtRemoteIpAddress.Size = new System.Drawing.Size(150, 23);
            this.txtRemoteIpAddress.TabIndex = 5;
            // 
            // lblRemoteIpAddress
            // 
            this.lblRemoteIpAddress.AutoSize = true;
            this.lblRemoteIpAddress.Location = new System.Drawing.Point(13, 84);
            this.lblRemoteIpAddress.Name = "lblRemoteIpAddress";
            this.lblRemoteIpAddress.Size = new System.Drawing.Size(104, 15);
            this.lblRemoteIpAddress.TabIndex = 4;
            this.lblRemoteIpAddress.Text = "Remote IP address";
            // 
            // pbRemoteUdpPortHint
            // 
            this.pbRemoteUdpPortHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbRemoteUdpPortHint.Location = new System.Drawing.Point(331, 54);
            this.pbRemoteUdpPortHint.Name = "pbRemoteUdpPortHint";
            this.pbRemoteUdpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbRemoteUdpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRemoteUdpPortHint.TabIndex = 5;
            this.pbRemoteUdpPortHint.TabStop = false;
            // 
            // numRemoteUdpPort
            // 
            this.numRemoteUdpPort.Location = new System.Drawing.Point(175, 51);
            this.numRemoteUdpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numRemoteUdpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRemoteUdpPort.Name = "numRemoteUdpPort";
            this.numRemoteUdpPort.Size = new System.Drawing.Size(150, 23);
            this.numRemoteUdpPort.TabIndex = 3;
            this.numRemoteUdpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRemoteUdpPort
            // 
            this.lblRemoteUdpPort.AutoSize = true;
            this.lblRemoteUdpPort.Location = new System.Drawing.Point(13, 55);
            this.lblRemoteUdpPort.Name = "lblRemoteUdpPort";
            this.lblRemoteUdpPort.Size = new System.Drawing.Size(99, 15);
            this.lblRemoteUdpPort.TabIndex = 2;
            this.lblRemoteUdpPort.Text = "Remote UDP port";
            // 
            // pbLocalUdpPortHint
            // 
            this.pbLocalUdpPortHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbLocalUdpPortHint.Location = new System.Drawing.Point(331, 25);
            this.pbLocalUdpPortHint.Name = "pbLocalUdpPortHint";
            this.pbLocalUdpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbLocalUdpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLocalUdpPortHint.TabIndex = 8;
            this.pbLocalUdpPortHint.TabStop = false;
            // 
            // numLocalUdpPort
            // 
            this.numLocalUdpPort.Location = new System.Drawing.Point(175, 22);
            this.numLocalUdpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numLocalUdpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLocalUdpPort.Name = "numLocalUdpPort";
            this.numLocalUdpPort.Size = new System.Drawing.Size(150, 23);
            this.numLocalUdpPort.TabIndex = 1;
            this.numLocalUdpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLocalUdpPort
            // 
            this.lblLocalUdpPort.AutoSize = true;
            this.lblLocalUdpPort.Location = new System.Drawing.Point(13, 26);
            this.lblLocalUdpPort.Name = "lblLocalUdpPort";
            this.lblLocalUdpPort.Size = new System.Drawing.Size(86, 15);
            this.lblLocalUdpPort.TabIndex = 0;
            this.lblLocalUdpPort.Text = "Local UDP port";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FrmUdpChannelOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 272);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.gbMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUdpChannelOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UDP Options";
            this.Load += new System.EventHandler(this.FrmCommUdpProps_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeviceMappingHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteIpAddressHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemoteUdpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRemoteUdpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalUdpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLocalUdpPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.ComboBox cbBehavior;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbDeviceMapping;
        private System.Windows.Forms.Label lblDeviceMapping;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label lblRemoteUdpPort;
        private System.Windows.Forms.Label lblLocalUdpPort;
        private System.Windows.Forms.TextBox txtRemoteIpAddress;
        private System.Windows.Forms.NumericUpDown numRemoteUdpPort;
        private System.Windows.Forms.PictureBox pbDeviceMappingHint;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbRemoteUdpPortHint;
        private System.Windows.Forms.Label lblRemoteIpAddress;
        private System.Windows.Forms.NumericUpDown numLocalUdpPort;
        private System.Windows.Forms.PictureBox pbLocalUdpPortHint;
        private System.Windows.Forms.PictureBox pbRemoteIpAddressHint;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
    }
}