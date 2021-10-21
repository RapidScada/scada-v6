namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    partial class FrmTcpClientChannelOptions
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
            this.pbConnectionModeHint = new System.Windows.Forms.PictureBox();
            this.cbConnectionMode = new System.Windows.Forms.ComboBox();
            this.lblConnectionMode = new System.Windows.Forms.Label();
            this.pbBehaviorHint = new System.Windows.Forms.PictureBox();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.pbStayConnectedHint = new System.Windows.Forms.PictureBox();
            this.chkStayConnected = new System.Windows.Forms.CheckBox();
            this.lblStayConnected = new System.Windows.Forms.Label();
            this.pbReconnectAfterHint = new System.Windows.Forms.PictureBox();
            this.numReconnectAfter = new System.Windows.Forms.NumericUpDown();
            this.lblReconnectAfter = new System.Windows.Forms.Label();
            this.pbTcpPortHint = new System.Windows.Forms.PictureBox();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.pbHostHint = new System.Windows.Forms.PictureBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectionModeHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).BeginInit();
            this.gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStayConnectedHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReconnectAfterHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReconnectAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHostHint)).BeginInit();
            this.SuspendLayout();
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.pbConnectionModeHint);
            this.gbMode.Controls.Add(this.cbConnectionMode);
            this.gbMode.Controls.Add(this.lblConnectionMode);
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
            "Individual",
            "Shared"});
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
            this.pbBehaviorHint.TabIndex = 5;
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
            this.btnOK.Location = new System.Drawing.Point(216, 266);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.pbStayConnectedHint);
            this.gbConnection.Controls.Add(this.chkStayConnected);
            this.gbConnection.Controls.Add(this.lblStayConnected);
            this.gbConnection.Controls.Add(this.pbReconnectAfterHint);
            this.gbConnection.Controls.Add(this.numReconnectAfter);
            this.gbConnection.Controls.Add(this.lblReconnectAfter);
            this.gbConnection.Controls.Add(this.pbTcpPortHint);
            this.gbConnection.Controls.Add(this.numTcpPort);
            this.gbConnection.Controls.Add(this.lblTcpPort);
            this.gbConnection.Controls.Add(this.pbHostHint);
            this.gbConnection.Controls.Add(this.txtHost);
            this.gbConnection.Controls.Add(this.lblHost);
            this.gbConnection.Location = new System.Drawing.Point(12, 105);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConnection.Size = new System.Drawing.Size(360, 145);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // pbStayConnectedHint
            // 
            this.pbStayConnectedHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbStayConnectedHint.Location = new System.Drawing.Point(331, 112);
            this.pbStayConnectedHint.Name = "pbStayConnectedHint";
            this.pbStayConnectedHint.Size = new System.Drawing.Size(16, 16);
            this.pbStayConnectedHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbStayConnectedHint.TabIndex = 11;
            this.pbStayConnectedHint.TabStop = false;
            // 
            // chkStayConnected
            // 
            this.chkStayConnected.AutoSize = true;
            this.chkStayConnected.Location = new System.Drawing.Point(243, 113);
            this.chkStayConnected.Name = "chkStayConnected";
            this.chkStayConnected.Size = new System.Drawing.Size(15, 14);
            this.chkStayConnected.TabIndex = 7;
            this.chkStayConnected.UseVisualStyleBackColor = true;
            // 
            // lblStayConnected
            // 
            this.lblStayConnected.AutoSize = true;
            this.lblStayConnected.Location = new System.Drawing.Point(13, 113);
            this.lblStayConnected.Name = "lblStayConnected";
            this.lblStayConnected.Size = new System.Drawing.Size(88, 15);
            this.lblStayConnected.TabIndex = 6;
            this.lblStayConnected.Text = "Stay connected";
            // 
            // pbReconnectAfterHint
            // 
            this.pbReconnectAfterHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbReconnectAfterHint.Location = new System.Drawing.Point(331, 83);
            this.pbReconnectAfterHint.Name = "pbReconnectAfterHint";
            this.pbReconnectAfterHint.Size = new System.Drawing.Size(16, 16);
            this.pbReconnectAfterHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbReconnectAfterHint.TabIndex = 9;
            this.pbReconnectAfterHint.TabStop = false;
            // 
            // numReconnectAfter
            // 
            this.numReconnectAfter.Location = new System.Drawing.Point(175, 80);
            this.numReconnectAfter.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numReconnectAfter.Name = "numReconnectAfter";
            this.numReconnectAfter.Size = new System.Drawing.Size(150, 23);
            this.numReconnectAfter.TabIndex = 5;
            this.numReconnectAfter.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblReconnectAfter
            // 
            this.lblReconnectAfter.AutoSize = true;
            this.lblReconnectAfter.Location = new System.Drawing.Point(13, 84);
            this.lblReconnectAfter.Name = "lblReconnectAfter";
            this.lblReconnectAfter.Size = new System.Drawing.Size(86, 15);
            this.lblReconnectAfter.TabIndex = 4;
            this.lblReconnectAfter.Text = "Reconnect, sec";
            // 
            // pbTcpPortHint
            // 
            this.pbTcpPortHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbTcpPortHint.Location = new System.Drawing.Point(331, 54);
            this.pbTcpPortHint.Name = "pbTcpPortHint";
            this.pbTcpPortHint.Size = new System.Drawing.Size(16, 16);
            this.pbTcpPortHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbTcpPortHint.TabIndex = 5;
            this.pbTcpPortHint.TabStop = false;
            // 
            // numTcpPort
            // 
            this.numTcpPort.Location = new System.Drawing.Point(175, 51);
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
            this.numTcpPort.TabIndex = 3;
            this.numTcpPort.Value = new decimal(new int[] {
            502,
            0,
            0,
            0});
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.AutoSize = true;
            this.lblTcpPort.Location = new System.Drawing.Point(13, 55);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(96, 15);
            this.lblTcpPort.TabIndex = 2;
            this.lblTcpPort.Text = "Remote TCP port";
            // 
            // pbHostHint
            // 
            this.pbHostHint.Image = global::Scada.Comm.Drivers.DrvCnlBasic.View.Properties.Resources.info;
            this.pbHostHint.Location = new System.Drawing.Point(331, 25);
            this.pbHostHint.Name = "pbHostHint";
            this.pbHostHint.Size = new System.Drawing.Size(16, 16);
            this.pbHostHint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHostHint.TabIndex = 6;
            this.pbHostHint.TabStop = false;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(175, 22);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(150, 23);
            this.txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(13, 26);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(74, 15);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Remote host";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // FrmTcpClientChannelOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 301);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTcpClientChannelOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TCP Client Options";
            this.Load += new System.EventHandler(this.FrmCommTcpClientProps_Load);
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectionModeHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBehaviorHint)).EndInit();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStayConnectedHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReconnectAfterHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReconnectAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTcpPortHint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHostHint)).EndInit();
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
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.PictureBox pbConnectionModeHint;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pbTcpPortHint;
        private System.Windows.Forms.PictureBox pbBehaviorHint;
        private System.Windows.Forms.PictureBox pbHostHint;
        private System.Windows.Forms.NumericUpDown numReconnectAfter;
        private System.Windows.Forms.PictureBox pbReconnectAfterHint;
        private System.Windows.Forms.Label lblReconnectAfter;
        private System.Windows.Forms.CheckBox chkStayConnected;
        private System.Windows.Forms.PictureBox pbStayConnectedHint;
        private System.Windows.Forms.Label lblStayConnected;
    }
}