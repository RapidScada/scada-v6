
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmEAO
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
            this.lblRetention = new System.Windows.Forms.Label();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblUseCopyDir = new System.Windows.Forms.Label();
            this.chkUseCopyDir = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRetention
            // 
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(12, 16);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(125, 15);
            this.lblRetention.TabIndex = 0;
            this.lblRetention.Text = "Retention period, days";
            // 
            // numRetention
            // 
            this.numRetention.Location = new System.Drawing.Point(272, 12);
            this.numRetention.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numRetention.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRetention.Name = "numRetention";
            this.numRetention.Size = new System.Drawing.Size(100, 23);
            this.numRetention.TabIndex = 1;
            this.numRetention.Value = new decimal(new int[] {
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
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmEAO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 139);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkUseCopyDir);
            this.Controls.Add(this.lblUseCopyDir);
            this.Controls.Add(this.chkLogEnabled);
            this.Controls.Add(this.lblLogEnabled);
            this.Controls.Add(this.numRetention);
            this.Controls.Add(this.lblRetention);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEAO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Archive Options";
            this.Load += new System.EventHandler(this.FrmHAO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblRetention;
        private System.Windows.Forms.NumericUpDown numRetention;
        private System.Windows.Forms.Label lblLogEnabled;
        private System.Windows.Forms.CheckBox chkLogEnabled;
        private System.Windows.Forms.Label lblUseCopyDir;
        private System.Windows.Forms.CheckBox chkUseCopyDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}