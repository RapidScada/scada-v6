
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmHAO
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
            this.lblWritingMode = new System.Windows.Forms.Label();
            this.cbWritingMode = new System.Windows.Forms.ComboBox();
            this.lblWritingPeriod = new System.Windows.Forms.Label();
            this.numWritingPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblWritingUnit = new System.Windows.Forms.Label();
            this.cbWritingUnit = new System.Windows.Forms.ComboBox();
            this.lblPullToPeriod = new System.Windows.Forms.Label();
            this.numPullToPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblUseCopyDir = new System.Windows.Forms.Label();
            this.chkUseCopyDir = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWritingMode
            // 
            this.lblWritingMode.AutoSize = true;
            this.lblWritingMode.Location = new System.Drawing.Point(12, 16);
            this.lblWritingMode.Name = "lblWritingMode";
            this.lblWritingMode.Size = new System.Drawing.Size(80, 15);
            this.lblWritingMode.TabIndex = 0;
            this.lblWritingMode.Text = "Writing mode";
            // 
            // cbWritingMode
            // 
            this.cbWritingMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWritingMode.FormattingEnabled = true;
            this.cbWritingMode.Location = new System.Drawing.Point(222, 12);
            this.cbWritingMode.Name = "cbWritingMode";
            this.cbWritingMode.Size = new System.Drawing.Size(150, 23);
            this.cbWritingMode.TabIndex = 1;
            // 
            // lblWritingPeriod
            // 
            this.lblWritingPeriod.AutoSize = true;
            this.lblWritingPeriod.Location = new System.Drawing.Point(12, 45);
            this.lblWritingPeriod.Name = "lblWritingPeriod";
            this.lblWritingPeriod.Size = new System.Drawing.Size(83, 15);
            this.lblWritingPeriod.TabIndex = 2;
            this.lblWritingPeriod.Text = "Writing period";
            // 
            // numWritingPeriod
            // 
            this.numWritingPeriod.Location = new System.Drawing.Point(222, 41);
            this.numWritingPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWritingPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWritingPeriod.Name = "numWritingPeriod";
            this.numWritingPeriod.Size = new System.Drawing.Size(150, 23);
            this.numWritingPeriod.TabIndex = 3;
            this.numWritingPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblWritingUnit
            // 
            this.lblWritingUnit.AutoSize = true;
            this.lblWritingUnit.Location = new System.Drawing.Point(12, 74);
            this.lblWritingUnit.Name = "lblWritingUnit";
            this.lblWritingUnit.Size = new System.Drawing.Size(70, 15);
            this.lblWritingUnit.TabIndex = 4;
            this.lblWritingUnit.Text = "Writing unit";
            // 
            // cbWritingUnit
            // 
            this.cbWritingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWritingUnit.FormattingEnabled = true;
            this.cbWritingUnit.Location = new System.Drawing.Point(222, 70);
            this.cbWritingUnit.Name = "cbWritingUnit";
            this.cbWritingUnit.Size = new System.Drawing.Size(150, 23);
            this.cbWritingUnit.TabIndex = 5;
            // 
            // lblPullToPeriod
            // 
            this.lblPullToPeriod.AutoSize = true;
            this.lblPullToPeriod.Location = new System.Drawing.Point(12, 103);
            this.lblPullToPeriod.Name = "lblPullToPeriod";
            this.lblPullToPeriod.Size = new System.Drawing.Size(101, 15);
            this.lblPullToPeriod.TabIndex = 6;
            this.lblPullToPeriod.Text = "Pull to period, sec";
            // 
            // numPullToPeriod
            // 
            this.numPullToPeriod.Location = new System.Drawing.Point(222, 99);
            this.numPullToPeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numPullToPeriod.Name = "numPullToPeriod";
            this.numPullToPeriod.Size = new System.Drawing.Size(150, 23);
            this.numPullToPeriod.TabIndex = 7;
            // 
            // lblRetention
            // 
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(12, 132);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(125, 15);
            this.lblRetention.TabIndex = 8;
            this.lblRetention.Text = "Retention period, days";
            // 
            // numRetention
            // 
            this.numRetention.Location = new System.Drawing.Point(222, 128);
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
            this.numRetention.Size = new System.Drawing.Size(150, 23);
            this.numRetention.TabIndex = 9;
            this.numRetention.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLogEnabled
            // 
            this.lblLogEnabled.AutoSize = true;
            this.lblLogEnabled.Location = new System.Drawing.Point(12, 161);
            this.lblLogEnabled.Name = "lblLogEnabled";
            this.lblLogEnabled.Size = new System.Drawing.Size(72, 15);
            this.lblLogEnabled.TabIndex = 10;
            this.lblLogEnabled.Text = "Log enabled";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(290, 161);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkLogEnabled.TabIndex = 11;
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblUseCopyDir
            // 
            this.lblUseCopyDir.AutoSize = true;
            this.lblUseCopyDir.Location = new System.Drawing.Point(12, 190);
            this.lblUseCopyDir.Name = "lblUseCopyDir";
            this.lblUseCopyDir.Size = new System.Drawing.Size(105, 15);
            this.lblUseCopyDir.TabIndex = 12;
            this.lblUseCopyDir.Text = "Use copy directory";
            // 
            // chkUseCopyDir
            // 
            this.chkUseCopyDir.AutoSize = true;
            this.chkUseCopyDir.Location = new System.Drawing.Point(290, 190);
            this.chkUseCopyDir.Name = "chkUseCopyDir";
            this.chkUseCopyDir.Size = new System.Drawing.Size(15, 14);
            this.chkUseCopyDir.TabIndex = 13;
            this.chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 220);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmHAO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 255);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkUseCopyDir);
            this.Controls.Add(this.lblUseCopyDir);
            this.Controls.Add(this.chkLogEnabled);
            this.Controls.Add(this.lblLogEnabled);
            this.Controls.Add(this.numRetention);
            this.Controls.Add(this.lblRetention);
            this.Controls.Add(this.numPullToPeriod);
            this.Controls.Add(this.lblPullToPeriod);
            this.Controls.Add(this.cbWritingUnit);
            this.Controls.Add(this.lblWritingUnit);
            this.Controls.Add(this.numWritingPeriod);
            this.Controls.Add(this.lblWritingPeriod);
            this.Controls.Add(this.cbWritingMode);
            this.Controls.Add(this.lblWritingMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHAO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historical Archive Options";
            this.Load += new System.EventHandler(this.FrmHAO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWritingMode;
        private System.Windows.Forms.ComboBox cbWritingMode;
        private System.Windows.Forms.Label lblWritingPeriod;
        private System.Windows.Forms.NumericUpDown numWritingPeriod;
        private System.Windows.Forms.Label lblWritingUnit;
        private System.Windows.Forms.ComboBox cbWritingUnit;
        private System.Windows.Forms.Label lblPullToPeriod;
        private System.Windows.Forms.NumericUpDown numPullToPeriod;
        private System.Windows.Forms.Label lblRetention;
        private System.Windows.Forms.NumericUpDown numRetention;
        private System.Windows.Forms.Label lblLogEnabled;
        private System.Windows.Forms.CheckBox chkLogEnabled;
        private System.Windows.Forms.Label lblUseCopyDir;
        private System.Windows.Forms.CheckBox chkUseCopyDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}