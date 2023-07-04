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
            components = new System.ComponentModel.Container();
            gbMode = new GroupBox();
            pbConnectionModeHint = new PictureBox();
            cbConnectionMode = new ComboBox();
            lblConnectionMode = new Label();
            pbBehaviorHint = new PictureBox();
            cbBehavior = new ComboBox();
            lblBehavior = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            gbConnection = new GroupBox();
            pbDisconnectOnError = new PictureBox();
            chkDisconnectOnError = new CheckBox();
            lblDisconnectOnError = new Label();
            pbStayConnectedHint = new PictureBox();
            chkStayConnected = new CheckBox();
            lblStayConnected = new Label();
            pbReconnectAfterHint = new PictureBox();
            numReconnectAfter = new NumericUpDown();
            lblReconnectAfter = new Label();
            pbTcpPortHint = new PictureBox();
            numTcpPort = new NumericUpDown();
            lblTcpPort = new Label();
            pbHostHint = new PictureBox();
            txtHost = new TextBox();
            lblHost = new Label();
            toolTip = new ToolTip(components);
            gbMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbConnectionModeHint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbBehaviorHint).BeginInit();
            gbConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbDisconnectOnError).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbStayConnectedHint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbReconnectAfterHint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numReconnectAfter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTcpPortHint).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTcpPort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbHostHint).BeginInit();
            SuspendLayout();
            // 
            // gbMode
            // 
            gbMode.Controls.Add(pbConnectionModeHint);
            gbMode.Controls.Add(cbConnectionMode);
            gbMode.Controls.Add(lblConnectionMode);
            gbMode.Controls.Add(pbBehaviorHint);
            gbMode.Controls.Add(cbBehavior);
            gbMode.Controls.Add(lblBehavior);
            gbMode.Location = new Point(12, 12);
            gbMode.Name = "gbMode";
            gbMode.Padding = new Padding(10, 3, 10, 10);
            gbMode.Size = new Size(360, 87);
            gbMode.TabIndex = 0;
            gbMode.TabStop = false;
            gbMode.Text = "Operating Mode";
            // 
            // pbConnectionModeHint
            // 
            pbConnectionModeHint.Image = Properties.Resources.info;
            pbConnectionModeHint.Location = new Point(331, 54);
            pbConnectionModeHint.Name = "pbConnectionModeHint";
            pbConnectionModeHint.Size = new Size(16, 16);
            pbConnectionModeHint.SizeMode = PictureBoxSizeMode.AutoSize;
            pbConnectionModeHint.TabIndex = 4;
            pbConnectionModeHint.TabStop = false;
            // 
            // cbConnectionMode
            // 
            cbConnectionMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbConnectionMode.FormattingEnabled = true;
            cbConnectionMode.Items.AddRange(new object[] { "Individual", "Shared" });
            cbConnectionMode.Location = new Point(175, 51);
            cbConnectionMode.Name = "cbConnectionMode";
            cbConnectionMode.Size = new Size(150, 23);
            cbConnectionMode.TabIndex = 3;
            cbConnectionMode.SelectedIndexChanged += cbConnectionMode_SelectedIndexChanged;
            // 
            // lblConnectionMode
            // 
            lblConnectionMode.AutoSize = true;
            lblConnectionMode.Location = new Point(13, 55);
            lblConnectionMode.Name = "lblConnectionMode";
            lblConnectionMode.Size = new Size(103, 15);
            lblConnectionMode.TabIndex = 2;
            lblConnectionMode.Text = "Connection mode";
            // 
            // pbBehaviorHint
            // 
            pbBehaviorHint.Image = Properties.Resources.info;
            pbBehaviorHint.Location = new Point(331, 25);
            pbBehaviorHint.Name = "pbBehaviorHint";
            pbBehaviorHint.Size = new Size(16, 16);
            pbBehaviorHint.SizeMode = PictureBoxSizeMode.AutoSize;
            pbBehaviorHint.TabIndex = 5;
            pbBehaviorHint.TabStop = false;
            // 
            // cbBehavior
            // 
            cbBehavior.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBehavior.FormattingEnabled = true;
            cbBehavior.Items.AddRange(new object[] { "Master", "Slave" });
            cbBehavior.Location = new Point(175, 22);
            cbBehavior.Name = "cbBehavior";
            cbBehavior.Size = new Size(150, 23);
            cbBehavior.TabIndex = 1;
            // 
            // lblBehavior
            // 
            lblBehavior.AutoSize = true;
            lblBehavior.Location = new Point(13, 26);
            lblBehavior.Name = "lblBehavior";
            lblBehavior.Size = new Size(53, 15);
            lblBehavior.TabIndex = 0;
            lblBehavior.Text = "Behavior";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 295);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 295);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbConnection
            // 
            gbConnection.Controls.Add(pbDisconnectOnError);
            gbConnection.Controls.Add(chkDisconnectOnError);
            gbConnection.Controls.Add(lblDisconnectOnError);
            gbConnection.Controls.Add(pbStayConnectedHint);
            gbConnection.Controls.Add(chkStayConnected);
            gbConnection.Controls.Add(lblStayConnected);
            gbConnection.Controls.Add(pbReconnectAfterHint);
            gbConnection.Controls.Add(numReconnectAfter);
            gbConnection.Controls.Add(lblReconnectAfter);
            gbConnection.Controls.Add(pbTcpPortHint);
            gbConnection.Controls.Add(numTcpPort);
            gbConnection.Controls.Add(lblTcpPort);
            gbConnection.Controls.Add(pbHostHint);
            gbConnection.Controls.Add(txtHost);
            gbConnection.Controls.Add(lblHost);
            gbConnection.Location = new Point(12, 105);
            gbConnection.Name = "gbConnection";
            gbConnection.Padding = new Padding(10, 3, 10, 10);
            gbConnection.Size = new Size(360, 174);
            gbConnection.TabIndex = 1;
            gbConnection.TabStop = false;
            gbConnection.Text = "Connection";
            // 
            // pbDisconnectOnError
            // 
            pbDisconnectOnError.Image = Properties.Resources.info;
            pbDisconnectOnError.Location = new Point(331, 141);
            pbDisconnectOnError.Name = "pbDisconnectOnError";
            pbDisconnectOnError.Size = new Size(16, 16);
            pbDisconnectOnError.SizeMode = PictureBoxSizeMode.AutoSize;
            pbDisconnectOnError.TabIndex = 16;
            pbDisconnectOnError.TabStop = false;
            // 
            // chkDisconnectOnError
            // 
            chkDisconnectOnError.AutoSize = true;
            chkDisconnectOnError.Enabled = false;
            chkDisconnectOnError.Location = new Point(310, 142);
            chkDisconnectOnError.Name = "chkDisconnectOnError";
            chkDisconnectOnError.Size = new Size(15, 14);
            chkDisconnectOnError.TabIndex = 9;
            chkDisconnectOnError.UseVisualStyleBackColor = true;
            // 
            // lblDisconnectOnError
            // 
            lblDisconnectOnError.AutoSize = true;
            lblDisconnectOnError.Location = new Point(13, 142);
            lblDisconnectOnError.Name = "lblDisconnectOnError";
            lblDisconnectOnError.Size = new Size(111, 15);
            lblDisconnectOnError.TabIndex = 8;
            lblDisconnectOnError.Text = "Disconnect on error";
            // 
            // pbStayConnectedHint
            // 
            pbStayConnectedHint.Image = Properties.Resources.info;
            pbStayConnectedHint.Location = new Point(331, 112);
            pbStayConnectedHint.Name = "pbStayConnectedHint";
            pbStayConnectedHint.Size = new Size(16, 16);
            pbStayConnectedHint.SizeMode = PictureBoxSizeMode.AutoSize;
            pbStayConnectedHint.TabIndex = 11;
            pbStayConnectedHint.TabStop = false;
            // 
            // chkStayConnected
            // 
            chkStayConnected.AutoSize = true;
            chkStayConnected.Location = new Point(310, 113);
            chkStayConnected.Name = "chkStayConnected";
            chkStayConnected.Size = new Size(15, 14);
            chkStayConnected.TabIndex = 7;
            chkStayConnected.UseVisualStyleBackColor = true;
            chkStayConnected.CheckedChanged += chkStayConnected_CheckedChanged;
            // 
            // lblStayConnected
            // 
            lblStayConnected.AutoSize = true;
            lblStayConnected.Location = new Point(13, 113);
            lblStayConnected.Name = "lblStayConnected";
            lblStayConnected.Size = new Size(88, 15);
            lblStayConnected.TabIndex = 6;
            lblStayConnected.Text = "Stay connected";
            // 
            // pbReconnectAfterHint
            // 
            pbReconnectAfterHint.Image = Properties.Resources.info;
            pbReconnectAfterHint.Location = new Point(331, 83);
            pbReconnectAfterHint.Name = "pbReconnectAfterHint";
            pbReconnectAfterHint.Size = new Size(16, 16);
            pbReconnectAfterHint.SizeMode = PictureBoxSizeMode.AutoSize;
            pbReconnectAfterHint.TabIndex = 9;
            pbReconnectAfterHint.TabStop = false;
            // 
            // numReconnectAfter
            // 
            numReconnectAfter.Location = new Point(175, 80);
            numReconnectAfter.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numReconnectAfter.Name = "numReconnectAfter";
            numReconnectAfter.Size = new Size(150, 23);
            numReconnectAfter.TabIndex = 5;
            numReconnectAfter.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // lblReconnectAfter
            // 
            lblReconnectAfter.AutoSize = true;
            lblReconnectAfter.Location = new Point(13, 84);
            lblReconnectAfter.Name = "lblReconnectAfter";
            lblReconnectAfter.Size = new Size(86, 15);
            lblReconnectAfter.TabIndex = 4;
            lblReconnectAfter.Text = "Reconnect, sec";
            // 
            // pbTcpPortHint
            // 
            pbTcpPortHint.Image = Properties.Resources.info;
            pbTcpPortHint.Location = new Point(331, 54);
            pbTcpPortHint.Name = "pbTcpPortHint";
            pbTcpPortHint.Size = new Size(16, 16);
            pbTcpPortHint.SizeMode = PictureBoxSizeMode.AutoSize;
            pbTcpPortHint.TabIndex = 5;
            pbTcpPortHint.TabStop = false;
            // 
            // numTcpPort
            // 
            numTcpPort.Location = new Point(175, 51);
            numTcpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numTcpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTcpPort.Name = "numTcpPort";
            numTcpPort.Size = new Size(150, 23);
            numTcpPort.TabIndex = 3;
            numTcpPort.Value = new decimal(new int[] { 502, 0, 0, 0 });
            // 
            // lblTcpPort
            // 
            lblTcpPort.AutoSize = true;
            lblTcpPort.Location = new Point(13, 55);
            lblTcpPort.Name = "lblTcpPort";
            lblTcpPort.Size = new Size(96, 15);
            lblTcpPort.TabIndex = 2;
            lblTcpPort.Text = "Remote TCP port";
            // 
            // pbHostHint
            // 
            pbHostHint.Image = Properties.Resources.info;
            pbHostHint.Location = new Point(331, 25);
            pbHostHint.Name = "pbHostHint";
            pbHostHint.Size = new Size(16, 16);
            pbHostHint.SizeMode = PictureBoxSizeMode.AutoSize;
            pbHostHint.TabIndex = 6;
            pbHostHint.TabStop = false;
            // 
            // txtHost
            // 
            txtHost.Location = new Point(175, 22);
            txtHost.Name = "txtHost";
            txtHost.Size = new Size(150, 23);
            txtHost.TabIndex = 1;
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Location = new Point(13, 26);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(74, 15);
            lblHost.TabIndex = 0;
            lblHost.Text = "Remote host";
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 30000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            // 
            // FrmTcpClientChannelOptions
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 330);
            Controls.Add(gbConnection);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            Controls.Add(gbMode);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmTcpClientChannelOptions";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "TCP Client Options";
            Load += FrmCommTcpClientProps_Load;
            gbMode.ResumeLayout(false);
            gbMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbConnectionModeHint).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbBehaviorHint).EndInit();
            gbConnection.ResumeLayout(false);
            gbConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbDisconnectOnError).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbStayConnectedHint).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbReconnectAfterHint).EndInit();
            ((System.ComponentModel.ISupportInitialize)numReconnectAfter).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTcpPortHint).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTcpPort).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbHostHint).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbMode;
        private ComboBox cbBehavior;
        private Label lblBehavior;
        private Button btnOK;
        private Button btnCancel;
        private ComboBox cbConnectionMode;
        private Label lblConnectionMode;
        private GroupBox gbConnection;
        private Label lblTcpPort;
        private Label lblHost;
        private TextBox txtHost;
        private NumericUpDown numTcpPort;
        private PictureBox pbConnectionModeHint;
        private ToolTip toolTip;
        private PictureBox pbTcpPortHint;
        private PictureBox pbBehaviorHint;
        private PictureBox pbHostHint;
        private NumericUpDown numReconnectAfter;
        private PictureBox pbReconnectAfterHint;
        private Label lblReconnectAfter;
        private CheckBox chkStayConnected;
        private PictureBox pbStayConnectedHint;
        private Label lblStayConnected;
        private PictureBox pbDisconnectOnError;
        private CheckBox chkDisconnectOnError;
        private Label lblDisconnectOnError;
    }
}