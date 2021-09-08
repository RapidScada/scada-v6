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
            this.gbPathOptions = new System.Windows.Forms.GroupBox();
            this.btnSetToDefaultLinux = new System.Windows.Forms.Button();
            this.btnSetToDefaultWin = new System.Windows.Forms.Button();
            this.btnBrowseViewDir = new System.Windows.Forms.Button();
            this.txtViewDir = new System.Windows.Forms.TextBox();
            this.lblViewDir = new System.Windows.Forms.Label();
            this.btnBrowseBaseDir = new System.Windows.Forms.Button();
            this.txtBaseDir = new System.Windows.Forms.TextBox();
            this.lblBaseDir = new System.Windows.Forms.Label();
            this.btnBrowseArcCopyDir = new System.Windows.Forms.Button();
            this.txtArcCopyDir = new System.Windows.Forms.TextBox();
            this.lblArcCopyDir = new System.Windows.Forms.Label();
            this.btnBrowseArcDir = new System.Windows.Forms.Button();
            this.txtArcDir = new System.Windows.Forms.TextBox();
            this.lblArcDir = new System.Windows.Forms.Label();
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
            this.numMaxLogSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxLogSize = new System.Windows.Forms.Label();
            this.numUnrelIfInactive = new System.Windows.Forms.NumericUpDown();
            this.lblUnrelIfInactive = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gbPathOptions.SuspendLayout();
            this.gbListenerOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnrelIfInactive)).BeginInit();
            this.SuspendLayout();
            // 
            // gbPathOptions
            // 
            this.gbPathOptions.Controls.Add(this.btnSetToDefaultLinux);
            this.gbPathOptions.Controls.Add(this.btnSetToDefaultWin);
            this.gbPathOptions.Controls.Add(this.btnBrowseViewDir);
            this.gbPathOptions.Controls.Add(this.txtViewDir);
            this.gbPathOptions.Controls.Add(this.lblViewDir);
            this.gbPathOptions.Controls.Add(this.btnBrowseBaseDir);
            this.gbPathOptions.Controls.Add(this.txtBaseDir);
            this.gbPathOptions.Controls.Add(this.lblBaseDir);
            this.gbPathOptions.Controls.Add(this.btnBrowseArcCopyDir);
            this.gbPathOptions.Controls.Add(this.txtArcCopyDir);
            this.gbPathOptions.Controls.Add(this.lblArcCopyDir);
            this.gbPathOptions.Controls.Add(this.btnBrowseArcDir);
            this.gbPathOptions.Controls.Add(this.txtArcDir);
            this.gbPathOptions.Controls.Add(this.lblArcDir);
            this.gbPathOptions.Location = new System.Drawing.Point(12, 278);
            this.gbPathOptions.Name = "gbPathOptions";
            this.gbPathOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPathOptions.Size = new System.Drawing.Size(500, 234);
            this.gbPathOptions.TabIndex = 2;
            this.gbPathOptions.TabStop = false;
            this.gbPathOptions.Text = "Directories";
            // 
            // btnSetToDefaultLinux
            // 
            this.btnSetToDefaultLinux.Location = new System.Drawing.Point(169, 198);
            this.btnSetToDefaultLinux.Name = "btnSetToDefaultLinux";
            this.btnSetToDefaultLinux.Size = new System.Drawing.Size(150, 23);
            this.btnSetToDefaultLinux.TabIndex = 13;
            this.btnSetToDefaultLinux.Text = "Default for Linux";
            this.btnSetToDefaultLinux.UseVisualStyleBackColor = true;
            this.btnSetToDefaultLinux.Click += new System.EventHandler(this.btnSetToDefault_Click);
            // 
            // btnSetToDefaultWin
            // 
            this.btnSetToDefaultWin.Location = new System.Drawing.Point(13, 198);
            this.btnSetToDefaultWin.Name = "btnSetToDefaultWin";
            this.btnSetToDefaultWin.Size = new System.Drawing.Size(150, 23);
            this.btnSetToDefaultWin.TabIndex = 12;
            this.btnSetToDefaultWin.Text = "Default for Windows";
            this.btnSetToDefaultWin.UseVisualStyleBackColor = true;
            this.btnSetToDefaultWin.Click += new System.EventHandler(this.btnSetToDefault_Click);
            // 
            // btnBrowseViewDir
            // 
            this.btnBrowseViewDir.Location = new System.Drawing.Point(412, 169);
            this.btnBrowseViewDir.Name = "btnBrowseViewDir";
            this.btnBrowseViewDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseViewDir.TabIndex = 11;
            this.btnBrowseViewDir.Text = "Browse...";
            this.btnBrowseViewDir.UseVisualStyleBackColor = true;
            this.btnBrowseViewDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtViewDir
            // 
            this.txtViewDir.Location = new System.Drawing.Point(13, 169);
            this.txtViewDir.Name = "txtViewDir";
            this.txtViewDir.Size = new System.Drawing.Size(393, 23);
            this.txtViewDir.TabIndex = 10;
            this.txtViewDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblViewDir
            // 
            this.lblViewDir.AutoSize = true;
            this.lblViewDir.Location = new System.Drawing.Point(10, 151);
            this.lblViewDir.Name = "lblViewDir";
            this.lblViewDir.Size = new System.Drawing.Size(53, 15);
            this.lblViewDir.TabIndex = 9;
            this.lblViewDir.Text = "Interface";
            // 
            // btnBrowseBaseDir
            // 
            this.btnBrowseBaseDir.Location = new System.Drawing.Point(412, 125);
            this.btnBrowseBaseDir.Name = "btnBrowseBaseDir";
            this.btnBrowseBaseDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseBaseDir.TabIndex = 8;
            this.btnBrowseBaseDir.Text = "Browse...";
            this.btnBrowseBaseDir.UseVisualStyleBackColor = true;
            this.btnBrowseBaseDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtBaseDir
            // 
            this.txtBaseDir.Location = new System.Drawing.Point(13, 125);
            this.txtBaseDir.Name = "txtBaseDir";
            this.txtBaseDir.Size = new System.Drawing.Size(393, 23);
            this.txtBaseDir.TabIndex = 7;
            this.txtBaseDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblBaseDir
            // 
            this.lblBaseDir.AutoSize = true;
            this.lblBaseDir.Location = new System.Drawing.Point(10, 107);
            this.lblBaseDir.Name = "lblBaseDir";
            this.lblBaseDir.Size = new System.Drawing.Size(207, 15);
            this.lblBaseDir.TabIndex = 6;
            this.lblBaseDir.Text = "Configuration database in DAT format";
            // 
            // btnBrowseArcCopyDir
            // 
            this.btnBrowseArcCopyDir.Location = new System.Drawing.Point(412, 81);
            this.btnBrowseArcCopyDir.Name = "btnBrowseArcCopyDir";
            this.btnBrowseArcCopyDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseArcCopyDir.TabIndex = 5;
            this.btnBrowseArcCopyDir.Text = "Browse...";
            this.btnBrowseArcCopyDir.UseVisualStyleBackColor = true;
            this.btnBrowseArcCopyDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtArcCopyDir
            // 
            this.txtArcCopyDir.Location = new System.Drawing.Point(13, 81);
            this.txtArcCopyDir.Name = "txtArcCopyDir";
            this.txtArcCopyDir.Size = new System.Drawing.Size(393, 23);
            this.txtArcCopyDir.TabIndex = 4;
            this.txtArcCopyDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblArcCopyDir
            // 
            this.lblArcCopyDir.AutoSize = true;
            this.lblArcCopyDir.Location = new System.Drawing.Point(10, 63);
            this.lblArcCopyDir.Name = "lblArcCopyDir";
            this.lblArcCopyDir.Size = new System.Drawing.Size(152, 15);
            this.lblArcCopyDir.TabIndex = 3;
            this.lblArcCopyDir.Text = "Archive copy in DAT format";
            // 
            // btnBrowseArcDir
            // 
            this.btnBrowseArcDir.Location = new System.Drawing.Point(412, 37);
            this.btnBrowseArcDir.Name = "btnBrowseArcDir";
            this.btnBrowseArcDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseArcDir.TabIndex = 2;
            this.btnBrowseArcDir.Text = "Browse...";
            this.btnBrowseArcDir.UseVisualStyleBackColor = true;
            this.btnBrowseArcDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtArcDir
            // 
            this.txtArcDir.Location = new System.Drawing.Point(13, 37);
            this.txtArcDir.Name = "txtArcDir";
            this.txtArcDir.Size = new System.Drawing.Size(393, 23);
            this.txtArcDir.TabIndex = 1;
            this.txtArcDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblArcDir
            // 
            this.lblArcDir.AutoSize = true;
            this.lblArcDir.Location = new System.Drawing.Point(10, 19);
            this.lblArcDir.Name = "lblArcDir";
            this.lblArcDir.Size = new System.Drawing.Size(123, 15);
            this.lblArcDir.TabIndex = 0;
            this.lblArcDir.Text = "Archive in DAT format";
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
            this.gbListenerOptions.Location = new System.Drawing.Point(12, 105);
            this.gbListenerOptions.Name = "gbListenerOptions";
            this.gbListenerOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbListenerOptions.Size = new System.Drawing.Size(500, 167);
            this.gbListenerOptions.TabIndex = 1;
            this.gbListenerOptions.TabStop = false;
            this.gbListenerOptions.Text = "Listener Options";
            // 
            // btnCopyKey
            // 
            this.btnCopyKey.Location = new System.Drawing.Point(169, 131);
            this.btnCopyKey.Name = "btnCopyKey";
            this.btnCopyKey.Size = new System.Drawing.Size(150, 23);
            this.btnCopyKey.TabIndex = 7;
            this.btnCopyKey.Text = "Copy";
            this.btnCopyKey.UseVisualStyleBackColor = true;
            this.btnCopyKey.Click += new System.EventHandler(this.btnCopyKey_Click);
            // 
            // btnGenerateKey
            // 
            this.btnGenerateKey.Location = new System.Drawing.Point(13, 131);
            this.btnGenerateKey.Name = "btnGenerateKey";
            this.btnGenerateKey.Size = new System.Drawing.Size(150, 23);
            this.btnGenerateKey.TabIndex = 6;
            this.btnGenerateKey.Text = "Generate Key";
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
            this.gbGeneralOptions.Controls.Add(this.numMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.lblMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.numUnrelIfInactive);
            this.gbGeneralOptions.Controls.Add(this.lblUnrelIfInactive);
            this.gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(500, 87);
            this.gbGeneralOptions.TabIndex = 0;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // numMaxLogSize
            // 
            this.numMaxLogSize.Location = new System.Drawing.Point(387, 51);
            this.numMaxLogSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxLogSize.Name = "numMaxLogSize";
            this.numMaxLogSize.Size = new System.Drawing.Size(100, 23);
            this.numMaxLogSize.TabIndex = 3;
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
            this.lblMaxLogSize.Location = new System.Drawing.Point(10, 55);
            this.lblMaxLogSize.Name = "lblMaxLogSize";
            this.lblMaxLogSize.Size = new System.Drawing.Size(147, 15);
            this.lblMaxLogSize.TabIndex = 2;
            this.lblMaxLogSize.Text = "Maximum log file size, MB";
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
            this.ClientSize = new System.Drawing.Size(684, 531);
            this.Controls.Add(this.gbGeneralOptions);
            this.Controls.Add(this.gbListenerOptions);
            this.Controls.Add(this.gbPathOptions);
            this.Name = "FrmGeneralOptions";
            this.Text = "General Options";
            this.Load += new System.EventHandler(this.FrmCommonParams_Load);
            this.gbPathOptions.ResumeLayout(false);
            this.gbPathOptions.PerformLayout();
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

        private System.Windows.Forms.GroupBox gbPathOptions;
        private System.Windows.Forms.TextBox txtArcCopyDir;
        private System.Windows.Forms.Label lblArcCopyDir;
        private System.Windows.Forms.TextBox txtArcDir;
        private System.Windows.Forms.Label lblArcDir;
        private System.Windows.Forms.TextBox txtViewDir;
        private System.Windows.Forms.Label lblViewDir;
        private System.Windows.Forms.TextBox txtBaseDir;
        private System.Windows.Forms.Label lblBaseDir;
        private System.Windows.Forms.GroupBox gbListenerOptions;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Button btnBrowseBaseDir;
        private System.Windows.Forms.Button btnBrowseViewDir;
        private System.Windows.Forms.Button btnBrowseArcDir;
        private System.Windows.Forms.Button btnBrowseArcCopyDir;
        private System.Windows.Forms.GroupBox gbGeneralOptions;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnSetToDefaultLinux;
        private System.Windows.Forms.Button btnSetToDefaultWin;
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
    }
}