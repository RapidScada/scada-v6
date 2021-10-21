namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    partial class FrmTcpServerChannelOptions
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
            this.pbConnectionModeHint = new System.Windows.Forms.PictureBox();
            this.cbConnectionMode = new System.Windows.Forms.ComboBox();
            this.lblConnectionMode = new System.Windows.Forms.Label();
            this.pbBehaviorHint = new System.Windows.Forms.PictureBox();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.pbClientLifetimeHint = new System.Windows.Forms.PictureBox();
            this.numClientLifetime = new System.Windows.Forms.NumericUpDown();
            this.lblClientLifetime = new System.Windows.Forms.Label();
            this.pbTcpPortHint = new System.Windows.Forms.PictureBox();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeviceMappingHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectionModeHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLifetimeHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClientLifetime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.pbDeviceMappingHint);
            this.gbMode.Controls.Add(this.cbDeviceMapping);
            this.gbMode.Controls.Add(this.lblDeviceMapping);
            this.gbMode.Controls.Add(this.pbConnectionModeHint);
            this.gbMode.Controls.Add(this.cbConnectionMode);
            this.gbMode.Controls.Add(this.lblConnectionMode);
            this.gbMode.Controls.Add(this.pbBehaviorHint);
            this.gbMode.Controls.Add(this.cbBehavior);
            this.gbMode.Controls.Add(this.lblBehavior);
            this.gbMode.Location = new System.Drawing.Point(12, 12);
            this.gbMode.Name = "gbMode";
            this.gbMode.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbMode.Size = new System.Drawing.Size(360, 116);
            this.gbMode.TabIndex = 0;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Operating Mode";
            // 
            // pbDeviceMappingHint
            // 
            this.pbDeviceMappingHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbDeviceMappingHint.Location = new System.Drawing.Point(331, 83);
            this.pbDeviceMappingHint.Name = "pbDeviceMappingHint";
            this.pbDeviceMappingHint.Size = new System.Drawing.Size(16, 16);
            this.pbDeviceMappingHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbDeviceMappingHint.TabIndex = 7;
            this.pbDeviceMappingHint.TabStop = false;
            // 
            // cbDeviceMapping
            // 
            this.cbDeviceMapping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeviceMapping.FormattingEnabled = true;
            this.cbDeviceMapping.Items.AddRange(new object[] {
            "By IP address",
            "By \"Hello\" packet",
            "Driver determined"});
            this.cbDeviceMapping.Location = new System.Drawing.Point(175, 80);
            this.cbDeviceMapping.Name = "cbDeviceMapping";
            this.cbDeviceMapping.Size = new System.Drawing.Size(150, 23);
            this.cbDeviceMapping.TabIndex = 6;
            // 
            // lblDeviceMapping
            // 
            this.lblDeviceMapping.AutoSize = true;
            this.lblDeviceMapping.Location = new System.Drawing.Point(13, 84);
            this.lblDeviceMapping.Name = "lblDeviceMapping";
            this.lblDeviceMapping.Size = new System.Drawing.Size(93, 15);
            this.lblDeviceMapping.TabIndex = 5;
            this.lblDeviceMapping.Text = "Device mapping";
            // 
            // pbConnectionModeHint
            // 
            this.pbConnectionModeHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbConnectionModeHint.Location = new System.Drawing.Point(331, 54);
            this.pbConnectionModeHint.Name = "pbConnectionModeHint";
            this.pbConnectionModeHint.Size = new System.Drawing.Size(16, 16);
            this.pbConnectionModeHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnectionModeHint.TabIndex = 4;
            this.pbConnectionModeHint.TabStop = false;
            // 
            // cbConnectionMode
            // 
            this.cbConnectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnectionMode.FormattingEnabled = true;
            this.cbConnectionMode.Items.AddRange(new object[] {
            "Индивидуальное",
            "Общее"});
            this.cbConnectionMode.Location = new System.Drawing.Point(175, 51);
            this.cbConnectionMode.Name = "cbConnectionMode";
            this.cbConnectionMode.Size = new System.Drawing.Size(150, 23);
            this.cbConnectionMode.TabIndex = 3;
            this.cbConnectionMode.SelectedIndexChanged += new System.EventHandler(this.cbConnectionMode_SelectedIndexChanged);
            // 
            // lblConnectionMode
            // 
            this.lblConnectionMode.AutoSize = true;
            this.lblConnectionMode.Location = new System.Drawing.Point(13, 55);
            this.lblConnectionMode.Name = "lblConnectionMode";
            this.lblConnectionMode.Size = new System.Drawing.Size(103, 15);
            this.lblConnectionMode.TabIndex = 2;
            this.lblConnectionMode.Text = "Connection mode";
            // 
            // pbBehaviorHint
            // 
            this.pbBehaviorHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbBehaviorHint.Location = new System.Drawing.Point(331, 25);
            this.pbBehaviorHint.Name = "pbBehaviorHint";
            this.pbBehaviorHint.Size = new System.Drawing.Size(16, 16);
            this.pbBehaviorHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbBehaviorHint.TabIndex = 8;
            this.pbBehaviorHint.TabStop = false;
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
            this.gbConnection.Controls.Add(this.pbClientLifetimeHint);
            this.gbConnection.Controls.Add(this.numClientLifetime);
            this.gbConnection.Controls.Add(this.lblClientLifetime);
            this.gbConnection.Controls.Add(this.pbTcpPortHint);
            this.gbConnection.Controls.Add(this.numTcpPort);
            this.gbConnection.Controls.Add(this.lblTcpPort);
            this.gbConnection.Location = new System.Drawing.Point(12, 134);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(360, 87);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // pbClientLifetimeHint
            // 
            this.pbClientLifetimeHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbClientLifetimeHint.Location = new System.Drawing.Point(331, 54);
            this.pbClientLifetimeHint.Name = "pbClientLifetimeHint";
            this.pbClientLifetimeHint.Size = new System.Drawing.Size(16, 16);
            this.pbClientLifetimeHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbClientLifetimeHint.TabIndex = 9;
            this.pbClientLifetimeHint.TabStop = false;
            // 
            // numClientLifetime
            // 
            this.numClientLifetime.Location = new System.Drawing.Point(175, 51);
            this.numClientLifetime.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numClientLifetime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClientLifetime.Name = "numClientLifetime";
            this.numClientLifetime.Size = new System.Drawing.Size(150, 23);
            this.numClientLifetime.TabIndex = 3;
            this.numClientLifetime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblClientLifetime
            // 
            this.lblClientLifetime.AutoSize = true;
            this.lblClientLifetime.Location = new System.Drawing.Point(13, 55);
            this.lblClientLifetime.Name = "lblClientLifetime";
            this.lblClientLifetime.Size = new System.Drawing.Size(114, 15);
            this.lblClientLifetime.TabIndex = 2;
            this.lblClientLifetime.Text = "Inactive lifetime, sec";
            // 
            // pbTcpPortHint
            // 
            this.pbTcpPortHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbTcpPortHint.Location = new System.Drawing.Point(331, 25);
            this.pbTcpPortHint.Name = "pbTcpPortHint";
            this.pbTcpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbTcpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTcpPortHint.TabIndex = 8;
            this.pbTcpPortHint.TabStop = false;
            // 
            // numTcpPort
            // 
            this.numTcpPort.Location = new System.Drawing.Point(175, 22);
            this.numTcpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTcpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTcpPort.Name = "numTcpPort";
            this.numTcpPort.Size = new System.Drawing.Size(150, 23);
            this.numTcpPort.TabIndex = 1;
            this.numTcpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.AutoSize = true;
            this.lblTcpPort.Location = new System.Drawing.Point(13, 26);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(83, 15);
            this.lblTcpPort.TabIndex = 0;
            this.lblTcpPort.Text = "Local TCP port";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FrmTcpServerChannelOptions
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
            this.Name = "FrmTcpServerChannelOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TCP Server Options";
            this.Load += new System.EventHandler(this.FrmCommTcpServerProps_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeviceMappingHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectionModeHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClientLifetimeHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numClientLifetime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.ComboBox cbBehavior;
        private System.Windows.Forms.Label lblBehavior;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbConnectionMode;
        private System.Windows.Forms.Label lblConnectionMode;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label lblTcpPort;
        private System.Windows.Forms.Label lblClientLifetime;
        private System.Windows.Forms.NumericUpDown numClientLifetime;
        private System.Windows.Forms.PictureBox pbConnectionModeHint;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbDeviceMappingHint;
        private System.Windows.Forms.ComboBox cbDeviceMapping;
        private System.Windows.Forms.Label lblDeviceMapping;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
        private System.Windows.Forms.PictureBox pbClientLifetimeHint;
        private System.Windows.Forms.PictureBox pbTcpPortHint;
    }
}