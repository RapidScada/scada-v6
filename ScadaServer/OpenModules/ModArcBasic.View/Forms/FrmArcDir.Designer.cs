
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmArcDir
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
            this.chkUseDefaultDir = new System.Windows.Forms.CheckBox();
            this.btnSetToDefaultLinux = new System.Windows.Forms.Button();
            this.btnSetToDefaultWin = new System.Windows.Forms.Button();
            this.btnBrowseArcCopyDir = new System.Windows.Forms.Button();
            this.btnBrowseArcDir = new System.Windows.Forms.Button();
            this.txtArcCopyDir = new System.Windows.Forms.TextBox();
            this.lblArcCopyDir = new System.Windows.Forms.Label();
            this.txtArcDir = new System.Windows.Forms.TextBox();
            this.lblArcDir = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // chkUseDefaultDir
            // 
            this.chkUseDefaultDir.AutoSize = true;
            this.chkUseDefaultDir.Location = new System.Drawing.Point(12, 12);
            this.chkUseDefaultDir.Name = "chkUseDefaultDir";
            this.chkUseDefaultDir.Size = new System.Drawing.Size(143, 19);
            this.chkUseDefaultDir.TabIndex = 0;
            this.chkUseDefaultDir.Text = "Use default directories";
            this.chkUseDefaultDir.UseVisualStyleBackColor = true;
            this.chkUseDefaultDir.CheckedChanged += new System.EventHandler(this.chkUseDefaultDir_CheckedChanged);
            // 
            // btnSetToDefaultLinux
            // 
            this.btnSetToDefaultLinux.Location = new System.Drawing.Point(123, 135);
            this.btnSetToDefaultLinux.Name = "btnSetToDefaultLinux";
            this.btnSetToDefaultLinux.Size = new System.Drawing.Size(105, 23);
            this.btnSetToDefaultLinux.TabIndex = 8;
            this.btnSetToDefaultLinux.Text = "Set for Linux";
            this.btnSetToDefaultLinux.UseVisualStyleBackColor = true;
            this.btnSetToDefaultLinux.Click += new System.EventHandler(this.btnSetToDefault_Click);
            // 
            // btnSetToDefaultWin
            // 
            this.btnSetToDefaultWin.Location = new System.Drawing.Point(12, 135);
            this.btnSetToDefaultWin.Name = "btnSetToDefaultWin";
            this.btnSetToDefaultWin.Size = new System.Drawing.Size(105, 23);
            this.btnSetToDefaultWin.TabIndex = 7;
            this.btnSetToDefaultWin.Text = "Set for Windows";
            this.btnSetToDefaultWin.UseVisualStyleBackColor = true;
            this.btnSetToDefaultWin.Click += new System.EventHandler(this.btnSetToDefault_Click);
            // 
            // btnBrowseArcCopyDir
            // 
            this.btnBrowseArcCopyDir.Location = new System.Drawing.Point(347, 96);
            this.btnBrowseArcCopyDir.Name = "btnBrowseArcCopyDir";
            this.btnBrowseArcCopyDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseArcCopyDir.TabIndex = 6;
            this.btnBrowseArcCopyDir.Text = "Browse...";
            this.btnBrowseArcCopyDir.UseVisualStyleBackColor = true;
            this.btnBrowseArcCopyDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnBrowseArcDir
            // 
            this.btnBrowseArcDir.Location = new System.Drawing.Point(347, 52);
            this.btnBrowseArcDir.Name = "btnBrowseArcDir";
            this.btnBrowseArcDir.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseArcDir.TabIndex = 3;
            this.btnBrowseArcDir.Text = "Browse...";
            this.btnBrowseArcDir.UseVisualStyleBackColor = true;
            this.btnBrowseArcDir.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtArcCopyDir
            // 
            this.txtArcCopyDir.Location = new System.Drawing.Point(12, 96);
            this.txtArcCopyDir.Name = "txtArcCopyDir";
            this.txtArcCopyDir.Size = new System.Drawing.Size(329, 23);
            this.txtArcCopyDir.TabIndex = 5;
            // 
            // lblArcCopyDir
            // 
            this.lblArcCopyDir.AutoSize = true;
            this.lblArcCopyDir.Location = new System.Drawing.Point(9, 78);
            this.lblArcCopyDir.Name = "lblArcCopyDir";
            this.lblArcCopyDir.Size = new System.Drawing.Size(152, 15);
            this.lblArcCopyDir.TabIndex = 4;
            this.lblArcCopyDir.Text = "Archive copy in DAT format";
            // 
            // txtArcDir
            // 
            this.txtArcDir.Location = new System.Drawing.Point(12, 52);
            this.txtArcDir.Name = "txtArcDir";
            this.txtArcDir.Size = new System.Drawing.Size(329, 23);
            this.txtArcDir.TabIndex = 2;
            // 
            // lblArcDir
            // 
            this.lblArcDir.AutoSize = true;
            this.lblArcDir.Location = new System.Drawing.Point(9, 34);
            this.lblArcDir.Name = "lblArcDir";
            this.lblArcDir.Size = new System.Drawing.Size(123, 15);
            this.lblArcDir.TabIndex = 1;
            this.lblArcDir.Text = "Archive in DAT format";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(266, 135);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(347, 135);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // FrmDir
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 170);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSetToDefaultLinux);
            this.Controls.Add(this.btnSetToDefaultWin);
            this.Controls.Add(this.btnBrowseArcCopyDir);
            this.Controls.Add(this.txtArcCopyDir);
            this.Controls.Add(this.lblArcCopyDir);
            this.Controls.Add(this.btnBrowseArcDir);
            this.Controls.Add(this.txtArcDir);
            this.Controls.Add(this.lblArcDir);
            this.Controls.Add(this.chkUseDefaultDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDir";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Archive Directories";
            this.Load += new System.EventHandler(this.FrmDir_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseDefaultDir;
        private System.Windows.Forms.Button btnSetToDefaultLinux;
        private System.Windows.Forms.Button btnSetToDefaultWin;
        private System.Windows.Forms.Button btnBrowseArcCopyDir;
        private System.Windows.Forms.Button btnBrowseArcDir;
        private System.Windows.Forms.TextBox txtArcCopyDir;
        private System.Windows.Forms.Label lblArcCopyDir;
        private System.Windows.Forms.TextBox txtArcDir;
        private System.Windows.Forms.Label lblArcDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}