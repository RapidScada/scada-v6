namespace Scada.Server.Forms.Controls
{
    partial class CtrlHistoricalArchiveOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cbDeadbandUnit = new ComboBox();
            numDeadband = new NumericUpDown();
            lblDeadband = new Label();
            chkWriteOnChange = new CheckBox();
            lblWriteOnChange = new Label();
            txtPullToPeriodUnit = new TextBox();
            numPullToPeriod = new NumericUpDown();
            lblPullToPeriod = new Label();
            cbWritingPeriodUnit = new ComboBox();
            numWritingPeriod = new NumericUpDown();
            lblWritingPeriod = new Label();
            chkWriteWithPeriod = new CheckBox();
            lblWriteWithPeriod = new Label();
            chkIsPeriodic = new CheckBox();
            lblIsPeriodic = new Label();
            txtRetentionUnit = new TextBox();
            numRetention = new NumericUpDown();
            lblRetention = new Label();
            chkLogEnabled = new CheckBox();
            lblLogEnabled = new Label();
            chkReadOnly = new CheckBox();
            lblReadOnly = new Label();
            cbWritingOffsetUnit = new ComboBox();
            numWritingOffset = new NumericUpDown();
            lblWritingOffset = new Label();
            chkUsePeriodStartTime = new CheckBox();
            lblUsePeriodStartTime = new Label();
            ((System.ComponentModel.ISupportInitialize)numDeadband).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPullToPeriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWritingPeriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRetention).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWritingOffset).BeginInit();
            SuspendLayout();
            // 
            // cbDeadbandUnit
            // 
            cbDeadbandUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbDeadbandUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDeadbandUnit.FormattingEnabled = true;
            cbDeadbandUnit.Items.AddRange(new object[] { "Abs.", "%" });
            cbDeadbandUnit.Location = new Point(290, 286);
            cbDeadbandUnit.Name = "cbDeadbandUnit";
            cbDeadbandUnit.Size = new Size(70, 23);
            cbDeadbandUnit.TabIndex = 26;
            // 
            // numDeadband
            // 
            numDeadband.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numDeadband.DecimalPlaces = 6;
            numDeadband.Location = new Point(209, 286);
            numDeadband.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numDeadband.Name = "numDeadband";
            numDeadband.Size = new Size(75, 23);
            numDeadband.TabIndex = 25;
            // 
            // lblDeadband
            // 
            lblDeadband.AutoSize = true;
            lblDeadband.Location = new Point(-3, 290);
            lblDeadband.Name = "lblDeadband";
            lblDeadband.Size = new Size(61, 15);
            lblDeadband.TabIndex = 24;
            lblDeadband.Text = "Deadband";
            // 
            // chkWriteOnChange
            // 
            chkWriteOnChange.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkWriteOnChange.AutoSize = true;
            chkWriteOnChange.Location = new Point(345, 261);
            chkWriteOnChange.Name = "chkWriteOnChange";
            chkWriteOnChange.Size = new Size(15, 14);
            chkWriteOnChange.TabIndex = 23;
            chkWriteOnChange.UseVisualStyleBackColor = true;
            chkWriteOnChange.CheckedChanged += chkWriteOnChange_CheckedChanged;
            // 
            // lblWriteOnChange
            // 
            lblWriteOnChange.AutoSize = true;
            lblWriteOnChange.Location = new Point(-3, 261);
            lblWriteOnChange.Name = "lblWriteOnChange";
            lblWriteOnChange.Size = new Size(94, 15);
            lblWriteOnChange.TabIndex = 22;
            lblWriteOnChange.Text = "Write on change";
            // 
            // txtPullToPeriodUnit
            // 
            txtPullToPeriodUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtPullToPeriodUnit.Location = new Point(290, 228);
            txtPullToPeriodUnit.Name = "txtPullToPeriodUnit";
            txtPullToPeriodUnit.ReadOnly = true;
            txtPullToPeriodUnit.Size = new Size(70, 23);
            txtPullToPeriodUnit.TabIndex = 21;
            txtPullToPeriodUnit.Text = "Sec";
            // 
            // numPullToPeriod
            // 
            numPullToPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numPullToPeriod.Location = new Point(209, 228);
            numPullToPeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numPullToPeriod.Name = "numPullToPeriod";
            numPullToPeriod.Size = new Size(75, 23);
            numPullToPeriod.TabIndex = 20;
            // 
            // lblPullToPeriod
            // 
            lblPullToPeriod.AutoSize = true;
            lblPullToPeriod.Location = new Point(-3, 232);
            lblPullToPeriod.Name = "lblPullToPeriod";
            lblPullToPeriod.Size = new Size(78, 15);
            lblPullToPeriod.TabIndex = 19;
            lblPullToPeriod.Text = "Pull to period";
            // 
            // cbWritingPeriodUnit
            // 
            cbWritingPeriodUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbWritingPeriodUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWritingPeriodUnit.FormattingEnabled = true;
            cbWritingPeriodUnit.Items.AddRange(new object[] { "Sec", "Min", "Hour", "Day" });
            cbWritingPeriodUnit.Location = new Point(290, 170);
            cbWritingPeriodUnit.Name = "cbWritingPeriodUnit";
            cbWritingPeriodUnit.Size = new Size(70, 23);
            cbWritingPeriodUnit.TabIndex = 15;
            // 
            // numWritingPeriod
            // 
            numWritingPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numWritingPeriod.Location = new Point(209, 170);
            numWritingPeriod.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numWritingPeriod.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numWritingPeriod.Name = "numWritingPeriod";
            numWritingPeriod.Size = new Size(75, 23);
            numWritingPeriod.TabIndex = 14;
            numWritingPeriod.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblWritingPeriod
            // 
            lblWritingPeriod.AutoSize = true;
            lblWritingPeriod.Location = new Point(-3, 174);
            lblWritingPeriod.Name = "lblWritingPeriod";
            lblWritingPeriod.Size = new Size(83, 15);
            lblWritingPeriod.TabIndex = 13;
            lblWritingPeriod.Text = "Writing period";
            // 
            // chkWriteWithPeriod
            // 
            chkWriteWithPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkWriteWithPeriod.AutoSize = true;
            chkWriteWithPeriod.Location = new Point(345, 116);
            chkWriteWithPeriod.Name = "chkWriteWithPeriod";
            chkWriteWithPeriod.Size = new Size(15, 14);
            chkWriteWithPeriod.TabIndex = 10;
            chkWriteWithPeriod.UseVisualStyleBackColor = true;
            chkWriteWithPeriod.CheckedChanged += chkWriteWithPeriod_CheckedChanged;
            // 
            // lblWriteWithPeriod
            // 
            lblWriteWithPeriod.AutoSize = true;
            lblWriteWithPeriod.Location = new Point(-3, 116);
            lblWriteWithPeriod.Name = "lblWriteWithPeriod";
            lblWriteWithPeriod.Size = new Size(98, 15);
            lblWriteWithPeriod.TabIndex = 9;
            lblWriteWithPeriod.Text = "Write with period";
            // 
            // chkIsPeriodic
            // 
            chkIsPeriodic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkIsPeriodic.AutoSize = true;
            chkIsPeriodic.Location = new Point(345, 87);
            chkIsPeriodic.Name = "chkIsPeriodic";
            chkIsPeriodic.Size = new Size(15, 14);
            chkIsPeriodic.TabIndex = 8;
            chkIsPeriodic.UseVisualStyleBackColor = true;
            chkIsPeriodic.CheckedChanged += chkIsPeriodic_CheckedChanged;
            // 
            // lblIsPeriodic
            // 
            lblIsPeriodic.AutoSize = true;
            lblIsPeriodic.Location = new Point(-3, 87);
            lblIsPeriodic.Name = "lblIsPeriodic";
            lblIsPeriodic.Size = new Size(104, 15);
            lblIsPeriodic.TabIndex = 7;
            lblIsPeriodic.Text = "Only periodic data";
            // 
            // txtRetentionUnit
            // 
            txtRetentionUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtRetentionUnit.Location = new Point(290, 54);
            txtRetentionUnit.Name = "txtRetentionUnit";
            txtRetentionUnit.ReadOnly = true;
            txtRetentionUnit.Size = new Size(70, 23);
            txtRetentionUnit.TabIndex = 6;
            txtRetentionUnit.Text = "Day";
            // 
            // numRetention
            // 
            numRetention.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numRetention.Location = new Point(209, 54);
            numRetention.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numRetention.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRetention.Name = "numRetention";
            numRetention.Size = new Size(75, 23);
            numRetention.TabIndex = 5;
            numRetention.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblRetention
            // 
            lblRetention.AutoSize = true;
            lblRetention.Location = new Point(-3, 58);
            lblRetention.Name = "lblRetention";
            lblRetention.Size = new Size(95, 15);
            lblRetention.TabIndex = 4;
            lblRetention.Text = "Retention period";
            // 
            // chkLogEnabled
            // 
            chkLogEnabled.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkLogEnabled.AutoSize = true;
            chkLogEnabled.Location = new Point(345, 29);
            chkLogEnabled.Name = "chkLogEnabled";
            chkLogEnabled.Size = new Size(15, 14);
            chkLogEnabled.TabIndex = 3;
            chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblLogEnabled
            // 
            lblLogEnabled.AutoSize = true;
            lblLogEnabled.Location = new Point(-3, 29);
            lblLogEnabled.Name = "lblLogEnabled";
            lblLogEnabled.Size = new Size(72, 15);
            lblLogEnabled.TabIndex = 2;
            lblLogEnabled.Text = "Log enabled";
            // 
            // chkReadOnly
            // 
            chkReadOnly.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkReadOnly.AutoSize = true;
            chkReadOnly.Location = new Point(345, 0);
            chkReadOnly.Name = "chkReadOnly";
            chkReadOnly.Size = new Size(15, 14);
            chkReadOnly.TabIndex = 1;
            chkReadOnly.UseVisualStyleBackColor = true;
            chkReadOnly.CheckedChanged += chkReadOnly_CheckedChanged;
            // 
            // lblReadOnly
            // 
            lblReadOnly.AutoSize = true;
            lblReadOnly.Location = new Point(-3, 0);
            lblReadOnly.Name = "lblReadOnly";
            lblReadOnly.Size = new Size(59, 15);
            lblReadOnly.TabIndex = 0;
            lblReadOnly.Text = "Read only";
            // 
            // cbWritingOffsetUnit
            // 
            cbWritingOffsetUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbWritingOffsetUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWritingOffsetUnit.FormattingEnabled = true;
            cbWritingOffsetUnit.Items.AddRange(new object[] { "Sec", "Min", "Hour", "Day" });
            cbWritingOffsetUnit.Location = new Point(290, 199);
            cbWritingOffsetUnit.Name = "cbWritingOffsetUnit";
            cbWritingOffsetUnit.Size = new Size(70, 23);
            cbWritingOffsetUnit.TabIndex = 18;
            // 
            // numWritingOffset
            // 
            numWritingOffset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numWritingOffset.Location = new Point(209, 199);
            numWritingOffset.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numWritingOffset.Name = "numWritingOffset";
            numWritingOffset.Size = new Size(75, 23);
            numWritingOffset.TabIndex = 17;
            numWritingOffset.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblWritingOffset
            // 
            lblWritingOffset.AutoSize = true;
            lblWritingOffset.Location = new Point(-3, 203);
            lblWritingOffset.Name = "lblWritingOffset";
            lblWritingOffset.Size = new Size(79, 15);
            lblWritingOffset.TabIndex = 16;
            lblWritingOffset.Text = "Writing offset";
            // 
            // chkUsePeriodStartTime
            // 
            chkUsePeriodStartTime.AutoSize = true;
            chkUsePeriodStartTime.Location = new Point(345, 145);
            chkUsePeriodStartTime.Name = "chkUsePeriodStartTime";
            chkUsePeriodStartTime.Size = new Size(15, 14);
            chkUsePeriodStartTime.TabIndex = 12;
            chkUsePeriodStartTime.UseVisualStyleBackColor = true;
            // 
            // lblUsePeriodStartTime
            // 
            lblUsePeriodStartTime.AutoSize = true;
            lblUsePeriodStartTime.Location = new Point(-3, 145);
            lblUsePeriodStartTime.Name = "lblUsePeriodStartTime";
            lblUsePeriodStartTime.Size = new Size(142, 15);
            lblUsePeriodStartTime.TabIndex = 11;
            lblUsePeriodStartTime.Text = "Timestamp at period start";
            // 
            // CtrlHistoricalArchiveOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cbDeadbandUnit);
            Controls.Add(numDeadband);
            Controls.Add(lblDeadband);
            Controls.Add(chkWriteOnChange);
            Controls.Add(lblWriteOnChange);
            Controls.Add(txtPullToPeriodUnit);
            Controls.Add(numPullToPeriod);
            Controls.Add(lblPullToPeriod);
            Controls.Add(cbWritingOffsetUnit);
            Controls.Add(numWritingOffset);
            Controls.Add(lblWritingOffset);
            Controls.Add(cbWritingPeriodUnit);
            Controls.Add(numWritingPeriod);
            Controls.Add(lblWritingPeriod);
            Controls.Add(chkUsePeriodStartTime);
            Controls.Add(lblUsePeriodStartTime);
            Controls.Add(chkWriteWithPeriod);
            Controls.Add(lblWriteWithPeriod);
            Controls.Add(chkIsPeriodic);
            Controls.Add(lblIsPeriodic);
            Controls.Add(txtRetentionUnit);
            Controls.Add(numRetention);
            Controls.Add(lblRetention);
            Controls.Add(chkLogEnabled);
            Controls.Add(lblLogEnabled);
            Controls.Add(chkReadOnly);
            Controls.Add(lblReadOnly);
            Name = "CtrlHistoricalArchiveOptions";
            Size = new Size(360, 309);
            ((System.ComponentModel.ISupportInitialize)numDeadband).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPullToPeriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWritingPeriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRetention).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWritingOffset).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private NumericUpDown numPullToPeriod;
        private Label lblPullToPeriod;
        private ComboBox cbWritingPeriodUnit;
        private NumericUpDown numWritingPeriod;
        private Label lblWritingPeriod;
        private Label lblReadOnly;
        private CheckBox chkReadOnly;
        private Label lblIsPeriodic;
        private CheckBox chkIsPeriodic;
        private CheckBox chkWriteWithPeriod;
        private Label lblWriteWithPeriod;
        private CheckBox chkWriteOnChange;
        private Label lblWriteOnChange;
        private NumericUpDown numDeadband;
        private Label lblDeadband;
        private ComboBox cbDeadbandUnit;
        private TextBox txtRetentionUnit;
        private TextBox txtPullToPeriodUnit;
        private ComboBox cbWritingOffsetUnit;
        private NumericUpDown numWritingOffset;
        private Label lblWritingOffset;
        private CheckBox chkUsePeriodStartTime;
        private Label lblUsePeriodStartTime;
    }
}
