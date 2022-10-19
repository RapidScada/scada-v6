
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmBasicHAO
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShowDir = new System.Windows.Forms.Button();
            this.gbGeneralOptions = new System.Windows.Forms.GroupBox();
            this.txtPullToPeriodUnit = new System.Windows.Forms.TextBox();
            this.numPullToPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblPullToPeriod = new System.Windows.Forms.Label();
            this.cbWritingPeriodUnit = new System.Windows.Forms.ComboBox();
            this.numWritingPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblWritingPeriod = new System.Windows.Forms.Label();
            this.chkWriteWithPeriod = new System.Windows.Forms.CheckBox();
            this.lblWriteWithPeriod = new System.Windows.Forms.Label();
            this.txtRetentionUnit = new System.Windows.Forms.TextBox();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.gbPathOptions = new System.Windows.Forms.GroupBox();
            this.lblUseCopyDir = new System.Windows.Forms.Label();
            this.chkUseCopyDir = new System.Windows.Forms.CheckBox();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.gbPathOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 266);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowDir
            // 
            this.btnShowDir.Location = new System.Drawing.Point(12, 266);
            this.btnShowDir.Name = "btnShowDir";
            this.btnShowDir.Size = new System.Drawing.Size(100, 23);
            this.btnShowDir.TabIndex = 2;
            this.btnShowDir.Text = "Directories";
            this.btnShowDir.UseVisualStyleBackColor = true;
            this.btnShowDir.Click += new System.EventHandler(this.btnShowDir_Click);
            // 
            // gbGeneralOptions
            // 
            this.gbGeneralOptions.Controls.Add(this.txtPullToPeriodUnit);
            this.gbGeneralOptions.Controls.Add(this.numPullToPeriod);
            this.gbGeneralOptions.Controls.Add(this.lblPullToPeriod);
            this.gbGeneralOptions.Controls.Add(this.cbWritingPeriodUnit);
            this.gbGeneralOptions.Controls.Add(this.numWritingPeriod);
            this.gbGeneralOptions.Controls.Add(this.lblWritingPeriod);
            this.gbGeneralOptions.Controls.Add(this.chkWriteWithPeriod);
            this.gbGeneralOptions.Controls.Add(this.lblWriteWithPeriod);
            this.gbGeneralOptions.Controls.Add(this.txtRetentionUnit);
            this.gbGeneralOptions.Controls.Add(this.numRetention);
            this.gbGeneralOptions.Controls.Add(this.lblRetention);
            this.gbGeneralOptions.Controls.Add(this.chkLogEnabled);
            this.gbGeneralOptions.Controls.Add(this.lblLogEnabled);
            this.gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(360, 174);
            this.gbGeneralOptions.TabIndex = 0;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // txtPullToPeriodUnit
            // 
            this.txtPullToPeriodUnit.Location = new System.Drawing.Point(277, 138);
            this.txtPullToPeriodUnit.Name = "txtPullToPeriodUnit";
            this.txtPullToPeriodUnit.ReadOnly = true;
            this.txtPullToPeriodUnit.Size = new System.Drawing.Size(70, 23);
            this.txtPullToPeriodUnit.TabIndex = 12;
            this.txtPullToPeriodUnit.Text = "Sec";
            // 
            // numPullToPeriod
            // 
            this.numPullToPeriod.Location = new System.Drawing.Point(196, 138);
            this.numPullToPeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numPullToPeriod.Name = "numPullToPeriod";
            this.numPullToPeriod.Size = new System.Drawing.Size(75, 23);
            this.numPullToPeriod.TabIndex = 11;
            // 
            // lblPullToPeriod
            // 
            this.lblPullToPeriod.AutoSize = true;
            this.lblPullToPeriod.Location = new System.Drawing.Point(13, 142);
            this.lblPullToPeriod.Name = "lblPullToPeriod";
            this.lblPullToPeriod.Size = new System.Drawing.Size(78, 15);
            this.lblPullToPeriod.TabIndex = 10;
            this.lblPullToPeriod.Text = "Pull to period";
            // 
            // cbWritingPeriodUnit
            // 
            this.cbWritingPeriodUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWritingPeriodUnit.FormattingEnabled = true;
            this.cbWritingPeriodUnit.Items.AddRange(new object[] {
            "Sec",
            "Min",
            "Hour"});
            this.cbWritingPeriodUnit.Location = new System.Drawing.Point(277, 109);
            this.cbWritingPeriodUnit.Name = "cbWritingPeriodUnit";
            this.cbWritingPeriodUnit.Size = new System.Drawing.Size(70, 23);
            this.cbWritingPeriodUnit.TabIndex = 9;
            // 
            // numWritingPeriod
            // 
            this.numWritingPeriod.Location = new System.Drawing.Point(196, 109);
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
            this.numWritingPeriod.Size = new System.Drawing.Size(75, 23);
            this.numWritingPeriod.TabIndex = 8;
            this.numWritingPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblWritingPeriod
            // 
            this.lblWritingPeriod.AutoSize = true;
            this.lblWritingPeriod.Location = new System.Drawing.Point(13, 113);
            this.lblWritingPeriod.Name = "lblWritingPeriod";
            this.lblWritingPeriod.Size = new System.Drawing.Size(83, 15);
            this.lblWritingPeriod.TabIndex = 7;
            this.lblWritingPeriod.Text = "Writing period";
            // 
            // chkWriteWithPeriod
            // 
            this.chkWriteWithPeriod.AutoSize = true;
            this.chkWriteWithPeriod.Location = new System.Drawing.Point(332, 84);
            this.chkWriteWithPeriod.Name = "chkWriteWithPeriod";
            this.chkWriteWithPeriod.Size = new System.Drawing.Size(15, 14);
            this.chkWriteWithPeriod.TabIndex = 6;
            this.chkWriteWithPeriod.UseVisualStyleBackColor = true;
            // 
            // lblWriteWithPeriod
            // 
            this.lblWriteWithPeriod.AutoSize = true;
            this.lblWriteWithPeriod.Location = new System.Drawing.Point(13, 84);
            this.lblWriteWithPeriod.Name = "lblWriteWithPeriod";
            this.lblWriteWithPeriod.Size = new System.Drawing.Size(98, 15);
            this.lblWriteWithPeriod.TabIndex = 5;
            this.lblWriteWithPeriod.Text = "Write with period";
            // 
            // txtRetentionUnit
            // 
            this.txtRetentionUnit.Location = new System.Drawing.Point(277, 51);
            this.txtRetentionUnit.Name = "txtRetentionUnit";
            this.txtRetentionUnit.ReadOnly = true;
            this.txtRetentionUnit.Size = new System.Drawing.Size(70, 23);
            this.txtRetentionUnit.TabIndex = 4;
            this.txtRetentionUnit.Text = "Day";
            // 
            // numRetention
            // 
            this.numRetention.Location = new System.Drawing.Point(196, 51);
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
            this.numRetention.Size = new System.Drawing.Size(75, 23);
            this.numRetention.TabIndex = 3;
            this.numRetention.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRetention
            // 
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(13, 55);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(95, 15);
            this.lblRetention.TabIndex = 2;
            this.lblRetention.Text = "Retention period";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(332, 26);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkLogEnabled.TabIndex = 1;
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblLogEnabled
            // 
            this.lblLogEnabled.AutoSize = true;
            this.lblLogEnabled.Location = new System.Drawing.Point(13, 26);
            this.lblLogEnabled.Name = "lblLogEnabled";
            this.lblLogEnabled.Size = new System.Drawing.Size(72, 15);
            this.lblLogEnabled.TabIndex = 0;
            this.lblLogEnabled.Text = "Log enabled";
            // 
            // gbPathOptions
            // 
            this.gbPathOptions.Controls.Add(this.lblUseCopyDir);
            this.gbPathOptions.Controls.Add(this.chkUseCopyDir);
            this.gbPathOptions.Location = new System.Drawing.Point(12, 192);
            this.gbPathOptions.Name = "gbPathOptions";
            this.gbPathOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPathOptions.Size = new System.Drawing.Size(360, 58);
            this.gbPathOptions.TabIndex = 1;
            this.gbPathOptions.TabStop = false;
            this.gbPathOptions.Text = "Path Options";
            // 
            // lblUseCopyDir
            // 
            this.lblUseCopyDir.AutoSize = true;
            this.lblUseCopyDir.Location = new System.Drawing.Point(13, 26);
            this.lblUseCopyDir.Name = "lblUseCopyDir";
            this.lblUseCopyDir.Size = new System.Drawing.Size(128, 15);
            this.lblUseCopyDir.TabIndex = 0;
            this.lblUseCopyDir.Text = "Write to copy directory";
            // 
            // chkUseCopyDir
            // 
            this.chkUseCopyDir.AutoSize = true;
            this.chkUseCopyDir.Location = new System.Drawing.Point(332, 26);
            this.chkUseCopyDir.Name = "chkUseCopyDir";
            this.chkUseCopyDir.Size = new System.Drawing.Size(15, 14);
            this.chkUseCopyDir.TabIndex = 1;
            this.chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // FrmBasicHAO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 301);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnShowDir);
            this.Controls.Add(this.gbPathOptions);
            this.Controls.Add(this.gbGeneralOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBasicHAO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Historical Archive Options";
            this.Load += new System.EventHandler(this.FrmHAO_Load);
            this.gbGeneralOptions.ResumeLayout(false);
            this.gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.gbPathOptions.ResumeLayout(false);
            this.gbPathOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowDir;
        private GroupBox gbGeneralOptions;
        private TextBox txtPullToPeriodUnit;
        private NumericUpDown numPullToPeriod;
        private Label lblPullToPeriod;
        private ComboBox cbWritingPeriodUnit;
        private NumericUpDown numWritingPeriod;
        private Label lblWritingPeriod;
        private CheckBox chkWriteWithPeriod;
        private Label lblWriteWithPeriod;
        private TextBox txtRetentionUnit;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private GroupBox gbPathOptions;
        private Label lblUseCopyDir;
        private CheckBox chkUseCopyDir;
    }
}