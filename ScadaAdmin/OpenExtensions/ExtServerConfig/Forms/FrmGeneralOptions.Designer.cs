namespace Scada.Admin.Extensions.ExtServerConfig.Forms
{
    partial class FrmGeneralOptions
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
            gbListenerOptions = new GroupBox();
            btnCopyKey = new Button();
            btnGenerateKey = new Button();
            txtSecretKey = new TextBox();
            lblSecretKey = new Label();
            numTimeout = new NumericUpDown();
            lblTimeout = new Label();
            numPort = new NumericUpDown();
            lblPort = new Label();
            gbGeneralOptions = new GroupBox();
            btnSelectObjects = new Button();
            txtEnableFormulasObjNums = new TextBox();
            chkDisableFormulas = new CheckBox();
            lblDisableFormulas = new Label();
            numMaxLogSize = new NumericUpDown();
            lblMaxLogSize = new Label();
            numStopWait = new NumericUpDown();
            lblStopWait = new Label();
            chkGenerateAckCmd = new CheckBox();
            lblGenerateAckCmd = new Label();
            chkUseArchivalStatus = new CheckBox();
            lblUseArchivalStatus = new Label();
            numMaxCurDataAge = new NumericUpDown();
            lblMaxCurDataAge = new Label();
            numUnrelIfInactive = new NumericUpDown();
            lblUnrelIfInactive = new Label();
            folderBrowserDialog = new FolderBrowserDialog();
            gbListenerOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxLogSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStopWait).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxCurDataAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numUnrelIfInactive).BeginInit();
            SuspendLayout();
            // 
            // gbListenerOptions
            // 
            gbListenerOptions.Controls.Add(btnCopyKey);
            gbListenerOptions.Controls.Add(btnGenerateKey);
            gbListenerOptions.Controls.Add(txtSecretKey);
            gbListenerOptions.Controls.Add(lblSecretKey);
            gbListenerOptions.Controls.Add(numTimeout);
            gbListenerOptions.Controls.Add(lblTimeout);
            gbListenerOptions.Controls.Add(numPort);
            gbListenerOptions.Controls.Add(lblPort);
            gbListenerOptions.Location = new Point(12, 272);
            gbListenerOptions.Name = "gbListenerOptions";
            gbListenerOptions.Padding = new Padding(10, 3, 10, 10);
            gbListenerOptions.Size = new Size(500, 167);
            gbListenerOptions.TabIndex = 1;
            gbListenerOptions.TabStop = false;
            gbListenerOptions.Text = "Listener Options";
            // 
            // btnCopyKey
            // 
            btnCopyKey.Location = new Point(119, 131);
            btnCopyKey.Name = "btnCopyKey";
            btnCopyKey.Size = new Size(100, 23);
            btnCopyKey.TabIndex = 7;
            btnCopyKey.Text = "Copy";
            btnCopyKey.UseVisualStyleBackColor = true;
            btnCopyKey.Click += btnCopyKey_Click;
            // 
            // btnGenerateKey
            // 
            btnGenerateKey.Location = new Point(13, 131);
            btnGenerateKey.Name = "btnGenerateKey";
            btnGenerateKey.Size = new Size(100, 23);
            btnGenerateKey.TabIndex = 6;
            btnGenerateKey.Text = "Generate";
            btnGenerateKey.UseVisualStyleBackColor = true;
            btnGenerateKey.Click += btnGenerateKey_Click;
            // 
            // txtSecretKey
            // 
            txtSecretKey.ForeColor = SystemColors.WindowText;
            txtSecretKey.Location = new Point(13, 102);
            txtSecretKey.Name = "txtSecretKey";
            txtSecretKey.Size = new Size(474, 23);
            txtSecretKey.TabIndex = 5;
            txtSecretKey.UseSystemPasswordChar = true;
            txtSecretKey.TextChanged += control_Changed;
            txtSecretKey.Enter += txtSecretKey_Enter;
            txtSecretKey.Leave += txtSecretKey_Leave;
            // 
            // lblSecretKey
            // 
            lblSecretKey.AutoSize = true;
            lblSecretKey.Location = new Point(10, 84);
            lblSecretKey.Name = "lblSecretKey";
            lblSecretKey.Size = new Size(60, 15);
            lblSecretKey.TabIndex = 4;
            lblSecretKey.Text = "Secret key";
            // 
            // numTimeout
            // 
            numTimeout.Location = new Point(387, 51);
            numTimeout.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numTimeout.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new Size(100, 23);
            numTimeout.TabIndex = 3;
            numTimeout.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            numTimeout.ValueChanged += control_Changed;
            // 
            // lblTimeout
            // 
            lblTimeout.AutoSize = true;
            lblTimeout.Location = new Point(10, 55);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new Size(73, 15);
            lblTimeout.TabIndex = 2;
            lblTimeout.Text = "Timeout, ms";
            // 
            // numPort
            // 
            numPort.Location = new Point(387, 22);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(100, 23);
            numPort.TabIndex = 1;
            numPort.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            numPort.ValueChanged += control_Changed;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(10, 26);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(52, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "TCP port";
            // 
            // gbGeneralOptions
            // 
            gbGeneralOptions.Controls.Add(btnSelectObjects);
            gbGeneralOptions.Controls.Add(txtEnableFormulasObjNums);
            gbGeneralOptions.Controls.Add(chkDisableFormulas);
            gbGeneralOptions.Controls.Add(lblDisableFormulas);
            gbGeneralOptions.Controls.Add(numMaxLogSize);
            gbGeneralOptions.Controls.Add(lblMaxLogSize);
            gbGeneralOptions.Controls.Add(numStopWait);
            gbGeneralOptions.Controls.Add(lblStopWait);
            gbGeneralOptions.Controls.Add(chkGenerateAckCmd);
            gbGeneralOptions.Controls.Add(lblGenerateAckCmd);
            gbGeneralOptions.Controls.Add(chkUseArchivalStatus);
            gbGeneralOptions.Controls.Add(lblUseArchivalStatus);
            gbGeneralOptions.Controls.Add(numMaxCurDataAge);
            gbGeneralOptions.Controls.Add(lblMaxCurDataAge);
            gbGeneralOptions.Controls.Add(numUnrelIfInactive);
            gbGeneralOptions.Controls.Add(lblUnrelIfInactive);
            gbGeneralOptions.Location = new Point(12, 12);
            gbGeneralOptions.Name = "gbGeneralOptions";
            gbGeneralOptions.Padding = new Padding(10, 3, 10, 10);
            gbGeneralOptions.Size = new Size(500, 254);
            gbGeneralOptions.TabIndex = 0;
            gbGeneralOptions.TabStop = false;
            gbGeneralOptions.Text = "General Options";
            // 
            // btnSelectObjects
            // 
            btnSelectObjects.Location = new Point(387, 218);
            btnSelectObjects.Name = "btnSelectObjects";
            btnSelectObjects.Size = new Size(100, 23);
            btnSelectObjects.TabIndex = 15;
            btnSelectObjects.Text = "Select...";
            btnSelectObjects.UseVisualStyleBackColor = true;
            btnSelectObjects.Click += btnSelectObjects_Click;
            // 
            // txtEnableFormulasObjNums
            // 
            txtEnableFormulasObjNums.Enabled = false;
            txtEnableFormulasObjNums.Location = new Point(34, 218);
            txtEnableFormulasObjNums.Name = "txtEnableFormulasObjNums";
            txtEnableFormulasObjNums.Size = new Size(347, 23);
            txtEnableFormulasObjNums.TabIndex = 14;
            txtEnableFormulasObjNums.TextChanged += control_Changed;
            // 
            // chkDisableFormulas
            // 
            chkDisableFormulas.AutoSize = true;
            chkDisableFormulas.Location = new Point(13, 222);
            chkDisableFormulas.Name = "chkDisableFormulas";
            chkDisableFormulas.Size = new Size(15, 14);
            chkDisableFormulas.TabIndex = 13;
            chkDisableFormulas.UseVisualStyleBackColor = true;
            chkDisableFormulas.CheckedChanged += chkDisableFormulas_CheckedChanged;
            // 
            // lblDisableFormulas
            // 
            lblDisableFormulas.AutoSize = true;
            lblDisableFormulas.Location = new Point(10, 200);
            lblDisableFormulas.Name = "lblDisableFormulas";
            lblDisableFormulas.Size = new Size(219, 15);
            lblDisableFormulas.TabIndex = 12;
            lblDisableFormulas.Text = "Disable channel formulas except objects";
            // 
            // numMaxLogSize
            // 
            numMaxLogSize.Location = new Point(387, 167);
            numMaxLogSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxLogSize.Name = "numMaxLogSize";
            numMaxLogSize.Size = new Size(100, 23);
            numMaxLogSize.TabIndex = 11;
            numMaxLogSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxLogSize.ValueChanged += control_Changed;
            // 
            // lblMaxLogSize
            // 
            lblMaxLogSize.AutoSize = true;
            lblMaxLogSize.Location = new Point(10, 171);
            lblMaxLogSize.Name = "lblMaxLogSize";
            lblMaxLogSize.Size = new Size(147, 15);
            lblMaxLogSize.TabIndex = 10;
            lblMaxLogSize.Text = "Maximum log file size, MB";
            // 
            // numStopWait
            // 
            numStopWait.Location = new Point(387, 138);
            numStopWait.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numStopWait.Name = "numStopWait";
            numStopWait.Size = new Size(100, 23);
            numStopWait.TabIndex = 9;
            numStopWait.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numStopWait.ValueChanged += control_Changed;
            // 
            // lblStopWait
            // 
            lblStopWait.AutoSize = true;
            lblStopWait.Location = new Point(10, 142);
            lblStopWait.Name = "lblStopWait";
            lblStopWait.Size = new Size(137, 15);
            lblStopWait.TabIndex = 8;
            lblStopWait.Text = "Wait for service stop, sec";
            // 
            // chkGenerateAckCmd
            // 
            chkGenerateAckCmd.AutoSize = true;
            chkGenerateAckCmd.Location = new Point(472, 113);
            chkGenerateAckCmd.Name = "chkGenerateAckCmd";
            chkGenerateAckCmd.Size = new Size(15, 14);
            chkGenerateAckCmd.TabIndex = 7;
            chkGenerateAckCmd.UseVisualStyleBackColor = true;
            chkGenerateAckCmd.CheckedChanged += control_Changed;
            // 
            // lblGenerateAckCmd
            // 
            lblGenerateAckCmd.AutoSize = true;
            lblGenerateAckCmd.Location = new Point(10, 113);
            lblGenerateAckCmd.Name = "lblGenerateAckCmd";
            lblGenerateAckCmd.Size = new Size(267, 15);
            lblGenerateAckCmd.TabIndex = 6;
            lblGenerateAckCmd.Text = "Generate command when event is acknowledged";
            // 
            // chkUseArchivalStatus
            // 
            chkUseArchivalStatus.AutoSize = true;
            chkUseArchivalStatus.Location = new Point(472, 84);
            chkUseArchivalStatus.Name = "chkUseArchivalStatus";
            chkUseArchivalStatus.Size = new Size(15, 14);
            chkUseArchivalStatus.TabIndex = 5;
            chkUseArchivalStatus.UseVisualStyleBackColor = true;
            chkUseArchivalStatus.CheckedChanged += control_Changed;
            // 
            // lblUseArchivalStatus
            // 
            lblUseArchivalStatus.AutoSize = true;
            lblUseArchivalStatus.Location = new Point(10, 84);
            lblUseArchivalStatus.Name = "lblUseArchivalStatus";
            lblUseArchivalStatus.Size = new Size(223, 15);
            lblUseArchivalStatus.TabIndex = 4;
            lblUseArchivalStatus.Text = "Mark incoming historical data as archival";
            // 
            // numMaxCurDataAge
            // 
            numMaxCurDataAge.Location = new Point(387, 51);
            numMaxCurDataAge.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numMaxCurDataAge.Name = "numMaxCurDataAge";
            numMaxCurDataAge.Size = new Size(100, 23);
            numMaxCurDataAge.TabIndex = 3;
            numMaxCurDataAge.ValueChanged += control_Changed;
            // 
            // lblMaxCurDataAge
            // 
            lblMaxCurDataAge.AutoSize = true;
            lblMaxCurDataAge.Location = new Point(10, 55);
            lblMaxCurDataAge.Name = "lblMaxCurDataAge";
            lblMaxCurDataAge.Size = new Size(230, 15);
            lblMaxCurDataAge.TabIndex = 2;
            lblMaxCurDataAge.Text = "Write current data as historical if older, sec";
            // 
            // numUnrelIfInactive
            // 
            numUnrelIfInactive.Location = new Point(387, 22);
            numUnrelIfInactive.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numUnrelIfInactive.Name = "numUnrelIfInactive";
            numUnrelIfInactive.Size = new Size(100, 23);
            numUnrelIfInactive.TabIndex = 1;
            numUnrelIfInactive.Value = new decimal(new int[] { 300, 0, 0, 0 });
            numUnrelIfInactive.ValueChanged += control_Changed;
            // 
            // lblUnrelIfInactive
            // 
            lblUnrelIfInactive.AutoSize = true;
            lblUnrelIfInactive.Location = new Point(10, 26);
            lblUnrelIfInactive.Name = "lblUnrelIfInactive";
            lblUnrelIfInactive.Size = new Size(225, 15);
            lblUnrelIfInactive.TabIndex = 0;
            lblUnrelIfInactive.Text = "Mark channel as unreliable if inactive, sec";
            // 
            // folderBrowserDialog
            // 
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            // 
            // FrmGeneralOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 461);
            Controls.Add(gbGeneralOptions);
            Controls.Add(gbListenerOptions);
            Name = "FrmGeneralOptions";
            Text = "General Options";
            Load += FrmCommonParams_Load;
            gbListenerOptions.ResumeLayout(false);
            gbListenerOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            gbGeneralOptions.ResumeLayout(false);
            gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxLogSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStopWait).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxCurDataAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numUnrelIfInactive).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.GroupBox gbListenerOptions;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.GroupBox gbGeneralOptions;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.NumericUpDown numMaxLogSize;
        private System.Windows.Forms.Label lblMaxLogSize;
        private System.Windows.Forms.NumericUpDown numUnrelIfInactive;
        private System.Windows.Forms.Label lblUnrelIfInactive;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.Button btnGenerateKey;
        private System.Windows.Forms.Button btnCopyKey;
        private System.Windows.Forms.CheckBox chkGenerateAckCmd;
        private System.Windows.Forms.Label lblGenerateAckCmd;
        private System.Windows.Forms.Button btnSelectObjects;
        private System.Windows.Forms.TextBox txtEnableFormulasObjNums;
        private System.Windows.Forms.CheckBox chkDisableFormulas;
        private System.Windows.Forms.Label lblDisableFormulas;
        private System.Windows.Forms.CheckBox chkUseArchivalStatus;
        private System.Windows.Forms.Label lblUseArchivalStatus;
        private NumericUpDown numMaxCurDataAge;
        private Label lblMaxCurDataAge;
        private NumericUpDown numStopWait;
        private Label lblStopWait;
    }
}