
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
            btnOK = new Button();
            btnCancel = new Button();
            btnShowDir = new Button();
            gbGeneralOptions = new GroupBox();
            txtPullToPeriodUnit = new TextBox();
            numPullToPeriod = new NumericUpDown();
            lblPullToPeriod = new Label();
            cbWritingPeriodUnit = new ComboBox();
            numWritingPeriod = new NumericUpDown();
            lblWritingPeriod = new Label();
            chkWriteWithPeriod = new CheckBox();
            lblWriteWithPeriod = new Label();
            txtRetentionUnit = new TextBox();
            numRetention = new NumericUpDown();
            lblRetention = new Label();
            chkLogEnabled = new CheckBox();
            lblLogEnabled = new Label();
            gbWritingOptions = new GroupBox();
            lblUseCopyDir = new Label();
            chkUseCopyDir = new CheckBox();
            numMaxQueueSize = new NumericUpDown();
            lblMaxQueueSize = new Label();
            gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPullToPeriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWritingPeriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRetention).BeginInit();
            gbWritingOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 296);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 296);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowDir
            // 
            btnShowDir.Location = new Point(12, 296);
            btnShowDir.Name = "btnShowDir";
            btnShowDir.Size = new Size(100, 23);
            btnShowDir.TabIndex = 2;
            btnShowDir.Text = "Directories";
            btnShowDir.UseVisualStyleBackColor = true;
            btnShowDir.Click += btnShowDir_Click;
            // 
            // gbGeneralOptions
            // 
            gbGeneralOptions.Controls.Add(txtPullToPeriodUnit);
            gbGeneralOptions.Controls.Add(numPullToPeriod);
            gbGeneralOptions.Controls.Add(lblPullToPeriod);
            gbGeneralOptions.Controls.Add(cbWritingPeriodUnit);
            gbGeneralOptions.Controls.Add(numWritingPeriod);
            gbGeneralOptions.Controls.Add(lblWritingPeriod);
            gbGeneralOptions.Controls.Add(chkWriteWithPeriod);
            gbGeneralOptions.Controls.Add(lblWriteWithPeriod);
            gbGeneralOptions.Controls.Add(txtRetentionUnit);
            gbGeneralOptions.Controls.Add(numRetention);
            gbGeneralOptions.Controls.Add(lblRetention);
            gbGeneralOptions.Controls.Add(chkLogEnabled);
            gbGeneralOptions.Controls.Add(lblLogEnabled);
            gbGeneralOptions.Location = new Point(12, 12);
            gbGeneralOptions.Name = "gbGeneralOptions";
            gbGeneralOptions.Padding = new Padding(10, 3, 10, 10);
            gbGeneralOptions.Size = new Size(360, 174);
            gbGeneralOptions.TabIndex = 0;
            gbGeneralOptions.TabStop = false;
            gbGeneralOptions.Text = "General Options";
            // 
            // txtPullToPeriodUnit
            // 
            txtPullToPeriodUnit.Location = new Point(277, 138);
            txtPullToPeriodUnit.Name = "txtPullToPeriodUnit";
            txtPullToPeriodUnit.ReadOnly = true;
            txtPullToPeriodUnit.Size = new Size(70, 23);
            txtPullToPeriodUnit.TabIndex = 12;
            txtPullToPeriodUnit.Text = "Sec";
            // 
            // numPullToPeriod
            // 
            numPullToPeriod.Location = new Point(196, 138);
            numPullToPeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numPullToPeriod.Name = "numPullToPeriod";
            numPullToPeriod.Size = new Size(75, 23);
            numPullToPeriod.TabIndex = 11;
            // 
            // lblPullToPeriod
            // 
            lblPullToPeriod.AutoSize = true;
            lblPullToPeriod.Location = new Point(13, 142);
            lblPullToPeriod.Name = "lblPullToPeriod";
            lblPullToPeriod.Size = new Size(78, 15);
            lblPullToPeriod.TabIndex = 10;
            lblPullToPeriod.Text = "Pull to period";
            // 
            // cbWritingPeriodUnit
            // 
            cbWritingPeriodUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWritingPeriodUnit.FormattingEnabled = true;
            cbWritingPeriodUnit.Items.AddRange(new object[] { "Sec", "Min", "Hour" });
            cbWritingPeriodUnit.Location = new Point(277, 109);
            cbWritingPeriodUnit.Name = "cbWritingPeriodUnit";
            cbWritingPeriodUnit.Size = new Size(70, 23);
            cbWritingPeriodUnit.TabIndex = 9;
            // 
            // numWritingPeriod
            // 
            numWritingPeriod.Location = new Point(196, 109);
            numWritingPeriod.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numWritingPeriod.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWritingPeriod.Name = "numWritingPeriod";
            numWritingPeriod.Size = new Size(75, 23);
            numWritingPeriod.TabIndex = 8;
            numWritingPeriod.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblWritingPeriod
            // 
            lblWritingPeriod.AutoSize = true;
            lblWritingPeriod.Location = new Point(13, 113);
            lblWritingPeriod.Name = "lblWritingPeriod";
            lblWritingPeriod.Size = new Size(83, 15);
            lblWritingPeriod.TabIndex = 7;
            lblWritingPeriod.Text = "Writing period";
            // 
            // chkWriteWithPeriod
            // 
            chkWriteWithPeriod.AutoSize = true;
            chkWriteWithPeriod.Location = new Point(332, 84);
            chkWriteWithPeriod.Name = "chkWriteWithPeriod";
            chkWriteWithPeriod.Size = new Size(15, 14);
            chkWriteWithPeriod.TabIndex = 6;
            chkWriteWithPeriod.UseVisualStyleBackColor = true;
            // 
            // lblWriteWithPeriod
            // 
            lblWriteWithPeriod.AutoSize = true;
            lblWriteWithPeriod.Location = new Point(13, 84);
            lblWriteWithPeriod.Name = "lblWriteWithPeriod";
            lblWriteWithPeriod.Size = new Size(98, 15);
            lblWriteWithPeriod.TabIndex = 5;
            lblWriteWithPeriod.Text = "Write with period";
            // 
            // txtRetentionUnit
            // 
            txtRetentionUnit.Location = new Point(277, 51);
            txtRetentionUnit.Name = "txtRetentionUnit";
            txtRetentionUnit.ReadOnly = true;
            txtRetentionUnit.Size = new Size(70, 23);
            txtRetentionUnit.TabIndex = 4;
            txtRetentionUnit.Text = "Day";
            // 
            // numRetention
            // 
            numRetention.Location = new Point(196, 51);
            numRetention.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numRetention.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRetention.Name = "numRetention";
            numRetention.Size = new Size(75, 23);
            numRetention.TabIndex = 3;
            numRetention.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblRetention
            // 
            lblRetention.AutoSize = true;
            lblRetention.Location = new Point(13, 55);
            lblRetention.Name = "lblRetention";
            lblRetention.Size = new Size(95, 15);
            lblRetention.TabIndex = 2;
            lblRetention.Text = "Retention period";
            // 
            // chkLogEnabled
            // 
            chkLogEnabled.AutoSize = true;
            chkLogEnabled.Location = new Point(332, 26);
            chkLogEnabled.Name = "chkLogEnabled";
            chkLogEnabled.Size = new Size(15, 14);
            chkLogEnabled.TabIndex = 1;
            chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblLogEnabled
            // 
            lblLogEnabled.AutoSize = true;
            lblLogEnabled.Location = new Point(13, 26);
            lblLogEnabled.Name = "lblLogEnabled";
            lblLogEnabled.Size = new Size(72, 15);
            lblLogEnabled.TabIndex = 0;
            lblLogEnabled.Text = "Log enabled";
            // 
            // gbWritingOptions
            // 
            gbWritingOptions.Controls.Add(numMaxQueueSize);
            gbWritingOptions.Controls.Add(lblMaxQueueSize);
            gbWritingOptions.Controls.Add(lblUseCopyDir);
            gbWritingOptions.Controls.Add(chkUseCopyDir);
            gbWritingOptions.Location = new Point(12, 192);
            gbWritingOptions.Name = "gbWritingOptions";
            gbWritingOptions.Padding = new Padding(10, 3, 10, 10);
            gbWritingOptions.Size = new Size(360, 88);
            gbWritingOptions.TabIndex = 1;
            gbWritingOptions.TabStop = false;
            gbWritingOptions.Text = "Writing Options";
            // 
            // lblUseCopyDir
            // 
            lblUseCopyDir.AutoSize = true;
            lblUseCopyDir.Location = new Point(13, 26);
            lblUseCopyDir.Name = "lblUseCopyDir";
            lblUseCopyDir.Size = new Size(128, 15);
            lblUseCopyDir.TabIndex = 0;
            lblUseCopyDir.Text = "Write to copy directory";
            // 
            // chkUseCopyDir
            // 
            chkUseCopyDir.AutoSize = true;
            chkUseCopyDir.Location = new Point(332, 26);
            chkUseCopyDir.Name = "chkUseCopyDir";
            chkUseCopyDir.Size = new Size(15, 14);
            chkUseCopyDir.TabIndex = 1;
            chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // numMaxQueueSize
            // 
            numMaxQueueSize.Location = new Point(196, 52);
            numMaxQueueSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxQueueSize.Name = "numMaxQueueSize";
            numMaxQueueSize.Size = new Size(151, 23);
            numMaxQueueSize.TabIndex = 3;
            // 
            // lblMaxQueueSize
            // 
            lblMaxQueueSize.AutoSize = true;
            lblMaxQueueSize.Location = new Point(13, 56);
            lblMaxQueueSize.Name = "lblMaxQueueSize";
            lblMaxQueueSize.Size = new Size(120, 15);
            lblMaxQueueSize.TabIndex = 2;
            lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // FrmBasicHAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 331);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnShowDir);
            Controls.Add(gbWritingOptions);
            Controls.Add(gbGeneralOptions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBasicHAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historical Archive Options";
            Load += FrmHAO_Load;
            gbGeneralOptions.ResumeLayout(false);
            gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPullToPeriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWritingPeriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRetention).EndInit();
            gbWritingOptions.ResumeLayout(false);
            gbWritingOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).EndInit();
            ResumeLayout(false);
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
        private GroupBox gbWritingOptions;
        private Label lblUseCopyDir;
        private CheckBox chkUseCopyDir;
        private NumericUpDown numMaxQueueSize;
        private Label lblMaxQueueSize;
    }
}