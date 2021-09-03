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
            this.gbDirs = new System.Windows.Forms.GroupBox();
            this.btnSetToDefaultLinux = new System.Windows.Forms.Button();
            this.btnSetToDefaultWin = new System.Windows.Forms.Button();
            this.btnBrowseArcCopyDir = new System.Windows.Forms.Button();
            this.btnBrowseArcDir = new System.Windows.Forms.Button();
            this.btnBrowseItfDir = new System.Windows.Forms.Button();
            this.btnBrowseBaseDATDir = new System.Windows.Forms.Button();
            this.txtArcCopyDir = new System.Windows.Forms.TextBox();
            this.lblArcCopyDir = new System.Windows.Forms.Label();
            this.txtArcDir = new System.Windows.Forms.TextBox();
            this.lblArcDir = new System.Windows.Forms.Label();
            this.txtViewDir = new System.Windows.Forms.TextBox();
            this.lblViewDir = new System.Windows.Forms.Label();
            this.txtBaseDir = new System.Windows.Forms.TextBox();
            this.lblBaseDir = new System.Windows.Forms.Label();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.lblTcpPort = new System.Windows.Forms.Label();
            this.numTcpPort = new System.Windows.Forms.NumericUpDown();
            this.lblLdapPath = new System.Windows.Forms.Label();
            this.chkUseAD = new System.Windows.Forms.CheckBox();
            this.txtLdapPath = new System.Windows.Forms.TextBox();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.fbdDir = new System.Windows.Forms.FolderBrowserDialog();
            this.gbDirs.SuspendLayout();
            this.gbConn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).BeginInit();
            this.gbLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDirs
            // 
            this.gbDirs.Controls.Add(this.btnSetToDefaultLinux);
            this.gbDirs.Controls.Add(this.btnSetToDefaultWin);
            this.gbDirs.Controls.Add(this.btnBrowseArcCopyDir);
            this.gbDirs.Controls.Add(this.btnBrowseArcDir);
            this.gbDirs.Controls.Add(this.btnBrowseItfDir);
            this.gbDirs.Controls.Add(this.btnBrowseBaseDATDir);
            this.gbDirs.Controls.Add(this.txtArcCopyDir);
            this.gbDirs.Controls.Add(this.lblArcCopyDir);
            this.gbDirs.Controls.Add(this.txtArcDir);
            this.gbDirs.Controls.Add(this.lblArcDir);
            this.gbDirs.Controls.Add(this.txtViewDir);
            this.gbDirs.Controls.Add(this.lblViewDir);
            this.gbDirs.Controls.Add(this.txtBaseDir);
            this.gbDirs.Controls.Add(this.lblBaseDir);
            this.gbDirs.Location = new System.Drawing.Point(12, 138);
            this.gbDirs.Name = "gbDirs";
            this.gbDirs.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDirs.Size = new System.Drawing.Size(500, 211);
            this.gbDirs.TabIndex = 2;
            this.gbDirs.TabStop = false;
            this.gbDirs.Text = "Directories";
            // 
            // btnSetToDefaultLinux
            // 
            this.btnSetToDefaultLinux.Location = new System.Drawing.Point(139, 175);
            this.btnSetToDefaultLinux.Name = "btnSetToDefaultLinux";
            this.btnSetToDefaultLinux.Size = new System.Drawing.Size(120, 23);
            this.btnSetToDefaultLinux.TabIndex = 13;
            this.btnSetToDefaultLinux.Text = "Default for Linux";
            this.btnSetToDefaultLinux.UseVisualStyleBackColor = true;
            this.btnSetToDefaultLinux.Click += new System.EventHandler(this.btnSetToDefault_Click);
            // 
            // btnSetToDefaultWin
            // 
            this.btnSetToDefaultWin.Location = new System.Drawing.Point(13, 175);
            this.btnSetToDefaultWin.Name = "btnSetToDefaultWin";
            this.btnSetToDefaultWin.Size = new System.Drawing.Size(120, 23);
            this.btnSetToDefaultWin.TabIndex = 12;
            this.btnSetToDefaultWin.Text = "Default for Windows";
            this.btnSetToDefaultWin.UseVisualStyleBackColor = true;
            this.btnSetToDefaultWin.Click += new System.EventHandler(this.btnSetToDefault_Click);
            // 
            // btnBrowseArcCopyDir
            // 
            this.btnBrowseArcCopyDir.Location = new System.Drawing.Point(412, 148);
            this.btnBrowseArcCopyDir.Name = "btnBrowseArcCopyDir";
            this.btnBrowseArcCopyDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseArcCopyDir.TabIndex = 11;
            this.btnBrowseArcCopyDir.Text = "Browse...";
            this.btnBrowseArcCopyDir.UseVisualStyleBackColor = true;
            this.btnBrowseArcCopyDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseArcDir
            // 
            this.btnBrowseArcDir.Location = new System.Drawing.Point(412, 109);
            this.btnBrowseArcDir.Name = "btnBrowseArcDir";
            this.btnBrowseArcDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseArcDir.TabIndex = 8;
            this.btnBrowseArcDir.Text = "Browse...";
            this.btnBrowseArcDir.UseVisualStyleBackColor = true;
            this.btnBrowseArcDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseItfDir
            // 
            this.btnBrowseItfDir.Location = new System.Drawing.Point(412, 70);
            this.btnBrowseItfDir.Name = "btnBrowseItfDir";
            this.btnBrowseItfDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseItfDir.TabIndex = 5;
            this.btnBrowseItfDir.Text = "Browse...";
            this.btnBrowseItfDir.UseVisualStyleBackColor = true;
            this.btnBrowseItfDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseBaseDATDir
            // 
            this.btnBrowseBaseDATDir.Location = new System.Drawing.Point(412, 31);
            this.btnBrowseBaseDATDir.Name = "btnBrowseBaseDATDir";
            this.btnBrowseBaseDATDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseBaseDATDir.TabIndex = 2;
            this.btnBrowseBaseDATDir.Text = "Browse...";
            this.btnBrowseBaseDATDir.UseVisualStyleBackColor = true;
            this.btnBrowseBaseDATDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtArcCopyDir
            // 
            this.txtArcCopyDir.Location = new System.Drawing.Point(13, 149);
            this.txtArcCopyDir.Name = "txtArcCopyDir";
            this.txtArcCopyDir.Size = new System.Drawing.Size(393, 20);
            this.txtArcCopyDir.TabIndex = 10;
            this.txtArcCopyDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblArcCopyDir
            // 
            this.lblArcCopyDir.AutoSize = true;
            this.lblArcCopyDir.Location = new System.Drawing.Point(10, 133);
            this.lblArcCopyDir.Name = "lblArcCopyDir";
            this.lblArcCopyDir.Size = new System.Drawing.Size(137, 13);
            this.lblArcCopyDir.TabIndex = 9;
            this.lblArcCopyDir.Text = "Archive copy in DAT format";
            // 
            // txtArcDir
            // 
            this.txtArcDir.Location = new System.Drawing.Point(13, 110);
            this.txtArcDir.Name = "txtArcDir";
            this.txtArcDir.Size = new System.Drawing.Size(393, 20);
            this.txtArcDir.TabIndex = 7;
            this.txtArcDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblArcDir
            // 
            this.lblArcDir.AutoSize = true;
            this.lblArcDir.Location = new System.Drawing.Point(10, 94);
            this.lblArcDir.Name = "lblArcDir";
            this.lblArcDir.Size = new System.Drawing.Size(111, 13);
            this.lblArcDir.TabIndex = 6;
            this.lblArcDir.Text = "Archive in DAT format";
            // 
            // txtItfDir
            // 
            this.txtViewDir.Location = new System.Drawing.Point(13, 71);
            this.txtViewDir.Name = "txtItfDir";
            this.txtViewDir.Size = new System.Drawing.Size(393, 20);
            this.txtViewDir.TabIndex = 4;
            this.txtViewDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblItfDir
            // 
            this.lblViewDir.AutoSize = true;
            this.lblViewDir.Location = new System.Drawing.Point(10, 55);
            this.lblViewDir.Name = "lblItfDir";
            this.lblViewDir.Size = new System.Drawing.Size(49, 13);
            this.lblViewDir.TabIndex = 3;
            this.lblViewDir.Text = "Interface";
            // 
            // txtBaseDATDir
            // 
            this.txtBaseDir.Location = new System.Drawing.Point(13, 32);
            this.txtBaseDir.Name = "txtBaseDATDir";
            this.txtBaseDir.Size = new System.Drawing.Size(393, 20);
            this.txtBaseDir.TabIndex = 1;
            this.txtBaseDir.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblBaseDATDir
            // 
            this.lblBaseDir.AutoSize = true;
            this.lblBaseDir.Location = new System.Drawing.Point(10, 16);
            this.lblBaseDir.Name = "lblBaseDATDir";
            this.lblBaseDir.Size = new System.Drawing.Size(184, 13);
            this.lblBaseDir.TabIndex = 0;
            this.lblBaseDir.Text = "Configuration database in DAT format";
            // 
            // gbConn
            // 
            this.gbConn.Controls.Add(this.lblTcpPort);
            this.gbConn.Controls.Add(this.numTcpPort);
            this.gbConn.Controls.Add(this.lblLdapPath);
            this.gbConn.Controls.Add(this.chkUseAD);
            this.gbConn.Controls.Add(this.txtLdapPath);
            this.gbConn.Location = new System.Drawing.Point(12, 12);
            this.gbConn.Name = "gbConn";
            this.gbConn.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbConn.Size = new System.Drawing.Size(500, 65);
            this.gbConn.TabIndex = 0;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Connection";
            // 
            // lblTcpPort
            // 
            this.lblTcpPort.AutoSize = true;
            this.lblTcpPort.Location = new System.Drawing.Point(10, 16);
            this.lblTcpPort.Name = "lblTcpPort";
            this.lblTcpPort.Size = new System.Drawing.Size(49, 13);
            this.lblTcpPort.TabIndex = 0;
            this.lblTcpPort.Text = "TCP port";
            // 
            // numTcpPort
            // 
            this.numTcpPort.Location = new System.Drawing.Point(13, 32);
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
            this.numTcpPort.Size = new System.Drawing.Size(100, 20);
            this.numTcpPort.TabIndex = 1;
            this.numTcpPort.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTcpPort.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblLdapPath
            // 
            this.lblLdapPath.AutoSize = true;
            this.lblLdapPath.Location = new System.Drawing.Point(137, 16);
            this.lblLdapPath.Name = "lblLdapPath";
            this.lblLdapPath.Size = new System.Drawing.Size(89, 13);
            this.lblLdapPath.TabIndex = 3;
            this.lblLdapPath.Text = "Domain controller";
            // 
            // chkUseAD
            // 
            this.chkUseAD.AutoSize = true;
            this.chkUseAD.Location = new System.Drawing.Point(120, 35);
            this.chkUseAD.Name = "chkUseAD";
            this.chkUseAD.Size = new System.Drawing.Size(15, 14);
            this.chkUseAD.TabIndex = 2;
            this.chkUseAD.UseVisualStyleBackColor = true;
            this.chkUseAD.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // txtLdapPath
            // 
            this.txtLdapPath.Location = new System.Drawing.Point(140, 32);
            this.txtLdapPath.Name = "txtLdapPath";
            this.txtLdapPath.Size = new System.Drawing.Size(347, 20);
            this.txtLdapPath.TabIndex = 4;
            this.txtLdapPath.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // gbLog
            // 
            this.gbLog.Controls.Add(this.chkDetailedLog);
            this.gbLog.Location = new System.Drawing.Point(12, 83);
            this.gbLog.Name = "gbLog";
            this.gbLog.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLog.Size = new System.Drawing.Size(500, 49);
            this.gbLog.TabIndex = 1;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "Logging";
            // 
            // chkDetailedLog
            // 
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(13, 19);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.Size = new System.Drawing.Size(82, 17);
            this.chkDetailedLog.TabIndex = 0;
            this.chkDetailedLog.Text = "Detailed log";
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            this.chkDetailedLog.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // fbdDir
            // 
            this.fbdDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // FrmCommonParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Controls.Add(this.gbLog);
            this.Controls.Add(this.gbDirs);
            this.Controls.Add(this.gbConn);
            this.Name = "FrmCommonParams";
            this.Text = "Common Parameters";
            this.Load += new System.EventHandler(this.FrmCommonParams_Load);
            this.gbDirs.ResumeLayout(false);
            this.gbDirs.PerformLayout();
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTcpPort)).EndInit();
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDirs;
        private System.Windows.Forms.TextBox txtArcCopyDir;
        private System.Windows.Forms.Label lblArcCopyDir;
        private System.Windows.Forms.TextBox txtArcDir;
        private System.Windows.Forms.Label lblArcDir;
        private System.Windows.Forms.TextBox txtViewDir;
        private System.Windows.Forms.Label lblViewDir;
        private System.Windows.Forms.TextBox txtBaseDir;
        private System.Windows.Forms.Label lblBaseDir;
        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.Label lblTcpPort;
        private System.Windows.Forms.NumericUpDown numTcpPort;
        private System.Windows.Forms.Label lblLdapPath;
        private System.Windows.Forms.CheckBox chkUseAD;
        private System.Windows.Forms.TextBox txtLdapPath;
        private System.Windows.Forms.Button btnBrowseBaseDATDir;
        private System.Windows.Forms.Button btnBrowseItfDir;
        private System.Windows.Forms.Button btnBrowseArcDir;
        private System.Windows.Forms.Button btnBrowseArcCopyDir;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.CheckBox chkDetailedLog;
        private System.Windows.Forms.FolderBrowserDialog fbdDir;
        private System.Windows.Forms.Button btnSetToDefaultLinux;
        private System.Windows.Forms.Button btnSetToDefaultWin;
    }
}