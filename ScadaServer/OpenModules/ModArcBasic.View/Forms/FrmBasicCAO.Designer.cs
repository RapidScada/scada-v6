
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmBasicCAO
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
            this.lblFlushPeriod = new System.Windows.Forms.Label();
            this.numFlushPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblUseCopyDir = new System.Windows.Forms.Label();
            this.chkUseCopyDir = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnShowDir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numFlushPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWritingPeriod
            // 
            this.lblFlushPeriod.AutoSize = true;
            this.lblFlushPeriod.Location = new System.Drawing.Point(12, 16);
            this.lblFlushPeriod.Name = "lblWritingPeriod";
            this.lblFlushPeriod.Size = new System.Drawing.Size(153, 15);
            this.lblFlushPeriod.TabIndex = 0;
            this.lblFlushPeriod.Text = "Period of saving to disk, sec";
            // 
            // numWritingPeriod
            // 
            this.numFlushPeriod.Location = new System.Drawing.Point(272, 12);
            this.numFlushPeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numFlushPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFlushPeriod.Name = "numWritingPeriod";
            this.numFlushPeriod.Size = new System.Drawing.Size(100, 23);
            this.numFlushPeriod.TabIndex = 1;
            this.numFlushPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLogEnabled
            // 
            this.lblLogEnabled.AutoSize = true;
            this.lblLogEnabled.Location = new System.Drawing.Point(12, 45);
            this.lblLogEnabled.Name = "lblLogEnabled";
            this.lblLogEnabled.Size = new System.Drawing.Size(72, 15);
            this.lblLogEnabled.TabIndex = 2;
            this.lblLogEnabled.Text = "Log enabled";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(315, 45);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkLogEnabled.TabIndex = 3;
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblUseCopyDir
            // 
            this.lblUseCopyDir.AutoSize = true;
            this.lblUseCopyDir.Location = new System.Drawing.Point(12, 74);
            this.lblUseCopyDir.Name = "lblUseCopyDir";
            this.lblUseCopyDir.Size = new System.Drawing.Size(105, 15);
            this.lblUseCopyDir.TabIndex = 4;
            this.lblUseCopyDir.Text = "Use copy directory";
            // 
            // chkUseCopyDir
            // 
            this.chkUseCopyDir.AutoSize = true;
            this.chkUseCopyDir.Location = new System.Drawing.Point(315, 74);
            this.chkUseCopyDir.Name = "chkUseCopyDir";
            this.chkUseCopyDir.Size = new System.Drawing.Size(15, 14);
            this.chkUseCopyDir.TabIndex = 5;
            this.chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 104);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowDir
            // 
            this.btnShowDir.Location = new System.Drawing.Point(12, 104);
            this.btnShowDir.Name = "btnShowDir";
            this.btnShowDir.Size = new System.Drawing.Size(100, 23);
            this.btnShowDir.TabIndex = 6;
            this.btnShowDir.Text = "Directories";
            this.btnShowDir.UseVisualStyleBackColor = true;
            this.btnShowDir.Click += new System.EventHandler(this.btnShowDir_Click);
            // 
            // FrmCAO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 139);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnShowDir);
            this.Controls.Add(this.chkUseCopyDir);
            this.Controls.Add(this.lblUseCopyDir);
            this.Controls.Add(this.chkLogEnabled);
            this.Controls.Add(this.lblLogEnabled);
            this.Controls.Add(this.numFlushPeriod);
            this.Controls.Add(this.lblFlushPeriod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCAO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Current Archive Options";
            this.Load += new System.EventHandler(this.FrmHAO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numFlushPeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblFlushPeriod;
        private System.Windows.Forms.NumericUpDown numFlushPeriod;
        private System.Windows.Forms.Label lblLogEnabled;
        private System.Windows.Forms.CheckBox chkLogEnabled;
        private System.Windows.Forms.Label lblUseCopyDir;
        private System.Windows.Forms.CheckBox chkUseCopyDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnShowDir;
    }
}