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
            this.gbListenerOptions = new System.Windows.Forms.GroupBox();
            this.btnCopyKey = new System.Windows.Forms.Button();
            this.btnGenerateKey = new System.Windows.Forms.Button();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPort = new System.Windows.Forms.Label();
            this.gbGeneralOptions = new System.Windows.Forms.GroupBox();
            this.btnSelectObjects = new System.Windows.Forms.Button();
            this.txtEnableFormulasObjNums = new System.Windows.Forms.TextBox();
            this.chkDisableFormulas = new System.Windows.Forms.CheckBox();
            this.lblDisableFormulas = new System.Windows.Forms.Label();
            this.numMaxLogSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxLogSize = new System.Windows.Forms.Label();
            this.chkGenerateAckCmd = new System.Windows.Forms.CheckBox();
            this.lblGenerateAckCmd = new System.Windows.Forms.Label();
            this.numUnrelIfInactive = new System.Windows.Forms.NumericUpDown();
            this.lblUnrelIfInactive = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gbListenerOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnrelIfInactive)).BeginInit();
            this.SuspendLayout();
            // 
            // gbListenerOptions
            // 
            this.gbListenerOptions.Controls.Add(this.btnCopyKey);
            this.gbListenerOptions.Controls.Add(this.btnGenerateKey);
            this.gbListenerOptions.Controls.Add(this.txtSecretKey);
            this.gbListenerOptions.Controls.Add(this.lblSecretKey);
            this.gbListenerOptions.Controls.Add(this.numTimeout);
            this.gbListenerOptions.Controls.Add(this.lblTimeout);
            this.gbListenerOptions.Controls.Add(this.numPort);
            this.gbListenerOptions.Controls.Add(this.lblPort);
            this.gbListenerOptions.Location = new System.Drawing.Point(12, 185);
            this.gbListenerOptions.Name = "gbListenerOptions";
            this.gbListenerOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbListenerOptions.Size = new System.Drawing.Size(500, 167);
            this.gbListenerOptions.TabIndex = 1;
            this.gbListenerOptions.TabStop = false;
            this.gbListenerOptions.Text = "Listener Options";
            // 
            // btnCopyKey
            // 
            this.btnCopyKey.Location = new System.Drawing.Point(119, 131);
            this.btnCopyKey.Name = "btnCopyKey";
            this.btnCopyKey.Size = new System.Drawing.Size(100, 23);
            this.btnCopyKey.TabIndex = 7;
            this.btnCopyKey.Text = "Copy";
            this.btnCopyKey.UseVisualStyleBackColor = true;
            this.btnCopyKey.Click += new System.EventHandler(this.btnCopyKey_Click);
            // 
            // btnGenerateKey
            // 
            this.btnGenerateKey.Location = new System.Drawing.Point(13, 131);
            this.btnGenerateKey.Name = "btnGenerateKey";
            this.btnGenerateKey.Size = new System.Drawing.Size(100, 23);
            this.btnGenerateKey.TabIndex = 6;
            this.btnGenerateKey.Text = "Generate";
            this.btnGenerateKey.UseVisualStyleBackColor = true;
            this.btnGenerateKey.Click += new System.EventHandler(this.btnGenerateKey_Click);
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSecretKey.Location = new System.Drawing.Point(13, 102);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(474, 23);
            this.txtSecretKey.TabIndex = 5;
            this.txtSecretKey.UseSystemPasswordChar = true;
            this.txtSecretKey.TextChanged += new System.EventHandler(this.control_Changed);
            this.txtSecretKey.Enter += new System.EventHandler(this.txtSecretKey_Enter);
            this.txtSecretKey.Leave += new System.EventHandler(this.txtSecretKey_Leave);
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(10, 84);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(60, 15);
            this.lblSecretKey.TabIndex = 4;
            this.lblSecretKey.Text = "Secret key";
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(387, 51);
            this.numTimeout.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(100, 23);
            this.numTimeout.TabIndex = 3;
            this.numTimeout.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTimeout.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(10, 55);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(73, 15);
            this.lblTimeout.TabIndex = 2;
            this.lblTimeout.Text = "Timeout, ms";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(387, 22);
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
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(10, 26);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(52, 15);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "TCP port";
            // 
            // gbGeneralOptions
            // 
            this.gbGeneralOptions.Controls.Add(this.btnSelectObjects);
            this.gbGeneralOptions.Controls.Add(this.txtEnableFormulasObjNums);
            this.gbGeneralOptions.Controls.Add(this.chkDisableFormulas);
            this.gbGeneralOptions.Controls.Add(this.lblDisableFormulas);
            this.gbGeneralOptions.Controls.Add(this.numMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.lblMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.chkGenerateAckCmd);
            this.gbGeneralOptions.Controls.Add(this.lblGenerateAckCmd);
            this.gbGeneralOptions.Controls.Add(this.numUnrelIfInactive);
            this.gbGeneralOptions.Controls.Add(this.lblUnrelIfInactive);
            this.gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(500, 167);
            this.gbGeneralOptions.TabIndex = 0;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // btnSelectObjects
            // 
            this.btnSelectObjects.Location = new System.Drawing.Point(387, 131);
            this.btnSelectObjects.Name = "btnSelectObjects";
            this.btnSelectObjects.Size = new System.Drawing.Size(100, 23);
            this.btnSelectObjects.TabIndex = 9;
            this.btnSelectObjects.Text = "Select...";
            this.btnSelectObjects.UseVisualStyleBackColor = true;
            this.btnSelectObjects.Click += new System.EventHandler(this.btnSelectObjects_Click);
            // 
            // txtEnableFormulasObjNums
            // 
            this.txtEnableFormulasObjNums.Enabled = false;
            this.txtEnableFormulasObjNums.Location = new System.Drawing.Point(34, 131);
            this.txtEnableFormulasObjNums.Name = "txtEnableFormulasObjNums";
            this.txtEnableFormulasObjNums.Size = new System.Drawing.Size(347, 23);
            this.txtEnableFormulasObjNums.TabIndex = 8;
            this.txtEnableFormulasObjNums.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // chkDisableFormulas
            // 
            this.chkDisableFormulas.AutoSize = true;
            this.chkDisableFormulas.Location = new System.Drawing.Point(13, 135);
            this.chkDisableFormulas.Name = "chkDisableFormulas";
            this.chkDisableFormulas.Size = new System.Drawing.Size(15, 14);
            this.chkDisableFormulas.TabIndex = 7;
            this.chkDisableFormulas.UseVisualStyleBackColor = true;
            this.chkDisableFormulas.CheckedChanged += new System.EventHandler(this.chkDisableFormulas_CheckedChanged);
            // 
            // lblDisableFormulas
            // 
            this.lblDisableFormulas.AutoSize = true;
            this.lblDisableFormulas.Location = new System.Drawing.Point(10, 113);
            this.lblDisableFormulas.Name = "lblDisableFormulas";
            this.lblDisableFormulas.Size = new System.Drawing.Size(361, 15);
            this.lblDisableFormulas.TabIndex = 6;
            this.lblDisableFormulas.Text = "Disable channel formulas, except for channels belonging to objects";
            // 
            // numMaxLogSize
            // 
            this.numMaxLogSize.Location = new System.Drawing.Point(387, 80);
            this.numMaxLogSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxLogSize.Name = "numMaxLogSize";
            this.numMaxLogSize.Size = new System.Drawing.Size(100, 23);
            this.numMaxLogSize.TabIndex = 5;
            this.numMaxLogSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxLogSize.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblMaxLogSize
            // 
            this.lblMaxLogSize.AutoSize = true;
            this.lblMaxLogSize.Location = new System.Drawing.Point(10, 84);
            this.lblMaxLogSize.Name = "lblMaxLogSize";
            this.lblMaxLogSize.Size = new System.Drawing.Size(147, 15);
            this.lblMaxLogSize.TabIndex = 4;
            this.lblMaxLogSize.Text = "Maximum log file size, MB";
            // 
            // chkGenerateAckCmd
            // 
            this.chkGenerateAckCmd.AutoSize = true;
            this.chkGenerateAckCmd.Location = new System.Drawing.Point(430, 55);
            this.chkGenerateAckCmd.Name = "chkGenerateAckCmd";
            this.chkGenerateAckCmd.Size = new System.Drawing.Size(15, 14);
            this.chkGenerateAckCmd.TabIndex = 3;
            this.chkGenerateAckCmd.UseVisualStyleBackColor = true;
            this.chkGenerateAckCmd.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblGenerateAckCmd
            // 
            this.lblGenerateAckCmd.AutoSize = true;
            this.lblGenerateAckCmd.Location = new System.Drawing.Point(10, 55);
            this.lblGenerateAckCmd.Name = "lblGenerateAckCmd";
            this.lblGenerateAckCmd.Size = new System.Drawing.Size(267, 15);
            this.lblGenerateAckCmd.TabIndex = 2;
            this.lblGenerateAckCmd.Text = "Generate command when event is acknowledged";
            // 
            // numUnrelIfInactive
            // 
            this.numUnrelIfInactive.Location = new System.Drawing.Point(387, 22);
            this.numUnrelIfInactive.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numUnrelIfInactive.Name = "numUnrelIfInactive";
            this.numUnrelIfInactive.Size = new System.Drawing.Size(100, 23);
            this.numUnrelIfInactive.TabIndex = 1;
            this.numUnrelIfInactive.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numUnrelIfInactive.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblUnrelIfInactive
            // 
            this.lblUnrelIfInactive.AutoSize = true;
            this.lblUnrelIfInactive.Location = new System.Drawing.Point(10, 26);
            this.lblUnrelIfInactive.Name = "lblUnrelIfInactive";
            this.lblUnrelIfInactive.Size = new System.Drawing.Size(251, 15);
            this.lblUnrelIfInactive.TabIndex = 0;
            this.lblUnrelIfInactive.Text = "Channel is marked as unreliable if inactive, sec";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // FrmGeneralOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.gbGeneralOptions);
            this.Controls.Add(this.gbListenerOptions);
            this.Name = "FrmGeneralOptions";
            this.Text = "General Options";
            this.Load += new System.EventHandler(this.FrmCommonParams_Load);
            this.gbListenerOptions.ResumeLayout(false);
            this.gbListenerOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.gbGeneralOptions.ResumeLayout(false);
            this.gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnrelIfInactive)).EndInit();
            this.ResumeLayout(false);

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
    }
}