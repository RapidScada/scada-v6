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
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.cbDeadband = new System.Windows.Forms.ComboBox();
            this.numDeadband = new System.Windows.Forms.NumericUpDown();
            this.lblDeadband = new System.Windows.Forms.Label();
            this.chkWriteOnChange = new System.Windows.Forms.CheckBox();
            this.lblWriteOnChange = new System.Windows.Forms.Label();
            this.chkWriteWithPeriod = new System.Windows.Forms.CheckBox();
            this.lblWriteWithPeriod = new System.Windows.Forms.Label();
            this.lblIsPeriodic = new System.Windows.Forms.Label();
            this.chkIsPeriodic = new System.Windows.Forms.CheckBox();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.lblReadOnly = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.numPullToPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblPullToPeriod = new System.Windows.Forms.Label();
            this.cbWritingUnit = new System.Windows.Forms.ComboBox();
            this.numWritingPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblWritingPeriod = new System.Windows.Forms.Label();
            this.txtRetentionUnit = new System.Windows.Forms.TextBox();
            this.txtPullToPeriodUnit = new System.Windows.Forms.TextBox();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeadband)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbDeadband);
            this.gbOptions.Controls.Add(this.numDeadband);
            this.gbOptions.Controls.Add(this.lblDeadband);
            this.gbOptions.Controls.Add(this.chkWriteOnChange);
            this.gbOptions.Controls.Add(this.lblWriteOnChange);
            this.gbOptions.Controls.Add(this.txtPullToPeriodUnit);
            this.gbOptions.Controls.Add(this.numPullToPeriod);
            this.gbOptions.Controls.Add(this.lblPullToPeriod);
            this.gbOptions.Controls.Add(this.cbWritingUnit);
            this.gbOptions.Controls.Add(this.numWritingPeriod);
            this.gbOptions.Controls.Add(this.lblWritingPeriod);
            this.gbOptions.Controls.Add(this.chkWriteWithPeriod);
            this.gbOptions.Controls.Add(this.lblWriteWithPeriod);
            this.gbOptions.Controls.Add(this.chkIsPeriodic);
            this.gbOptions.Controls.Add(this.lblIsPeriodic);
            this.gbOptions.Controls.Add(this.txtRetentionUnit);
            this.gbOptions.Controls.Add(this.numRetention);
            this.gbOptions.Controls.Add(this.lblRetention);
            this.gbOptions.Controls.Add(this.chkLogEnabled);
            this.gbOptions.Controls.Add(this.lblLogEnabled);
            this.gbOptions.Controls.Add(this.chkReadOnly);
            this.gbOptions.Controls.Add(this.lblReadOnly);
            this.gbOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbOptions.Location = new System.Drawing.Point(0, 0);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(360, 290);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "General Options";
            // 
            // cbDeadband
            // 
            this.cbDeadband.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeadband.FormattingEnabled = true;
            this.cbDeadband.Items.AddRange(new object[] {
            "Abs.",
            "%"});
            this.cbDeadband.Location = new System.Drawing.Point(277, 254);
            this.cbDeadband.Name = "cbDeadband";
            this.cbDeadband.Size = new System.Drawing.Size(70, 23);
            this.cbDeadband.TabIndex = 21;
            // 
            // numDeadband
            // 
            this.numDeadband.DecimalPlaces = 6;
            this.numDeadband.Location = new System.Drawing.Point(196, 254);
            this.numDeadband.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDeadband.Name = "numDeadband";
            this.numDeadband.Size = new System.Drawing.Size(75, 23);
            this.numDeadband.TabIndex = 20;
            // 
            // lblDeadband
            // 
            this.lblDeadband.AutoSize = true;
            this.lblDeadband.Location = new System.Drawing.Point(13, 258);
            this.lblDeadband.Name = "lblDeadband";
            this.lblDeadband.Size = new System.Drawing.Size(61, 15);
            this.lblDeadband.TabIndex = 19;
            this.lblDeadband.Text = "Deadband";
            // 
            // chkWriteOnChange
            // 
            this.chkWriteOnChange.AutoSize = true;
            this.chkWriteOnChange.Location = new System.Drawing.Point(226, 229);
            this.chkWriteOnChange.Name = "chkWriteOnChange";
            this.chkWriteOnChange.Size = new System.Drawing.Size(15, 14);
            this.chkWriteOnChange.TabIndex = 18;
            this.chkWriteOnChange.UseVisualStyleBackColor = true;
            // 
            // lblWriteOnChange
            // 
            this.lblWriteOnChange.AutoSize = true;
            this.lblWriteOnChange.Location = new System.Drawing.Point(13, 229);
            this.lblWriteOnChange.Name = "lblWriteOnChange";
            this.lblWriteOnChange.Size = new System.Drawing.Size(94, 15);
            this.lblWriteOnChange.TabIndex = 17;
            this.lblWriteOnChange.Text = "Write on change";
            // 
            // chkWriteWithPeriod
            // 
            this.chkWriteWithPeriod.AutoSize = true;
            this.chkWriteWithPeriod.Location = new System.Drawing.Point(226, 142);
            this.chkWriteWithPeriod.Name = "chkWriteWithPeriod";
            this.chkWriteWithPeriod.Size = new System.Drawing.Size(15, 14);
            this.chkWriteWithPeriod.TabIndex = 10;
            this.chkWriteWithPeriod.UseVisualStyleBackColor = true;
            // 
            // lblWriteWithPeriod
            // 
            this.lblWriteWithPeriod.AutoSize = true;
            this.lblWriteWithPeriod.Location = new System.Drawing.Point(13, 142);
            this.lblWriteWithPeriod.Name = "lblWriteWithPeriod";
            this.lblWriteWithPeriod.Size = new System.Drawing.Size(98, 15);
            this.lblWriteWithPeriod.TabIndex = 9;
            this.lblWriteWithPeriod.Text = "Write with period";
            // 
            // lblIsPeriodic
            // 
            this.lblIsPeriodic.AutoSize = true;
            this.lblIsPeriodic.Location = new System.Drawing.Point(13, 113);
            this.lblIsPeriodic.Name = "lblIsPeriodic";
            this.lblIsPeriodic.Size = new System.Drawing.Size(61, 15);
            this.lblIsPeriodic.TabIndex = 7;
            this.lblIsPeriodic.Text = "Is periodic";
            // 
            // chkIsPeriodic
            // 
            this.chkIsPeriodic.AutoSize = true;
            this.chkIsPeriodic.Location = new System.Drawing.Point(226, 113);
            this.chkIsPeriodic.Name = "chkIsPeriodic";
            this.chkIsPeriodic.Size = new System.Drawing.Size(15, 14);
            this.chkIsPeriodic.TabIndex = 8;
            this.chkIsPeriodic.UseVisualStyleBackColor = true;
            // 
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.Location = new System.Drawing.Point(226, 26);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(15, 14);
            this.chkReadOnly.TabIndex = 1;
            this.chkReadOnly.UseVisualStyleBackColor = true;
            // 
            // lblReadOnly
            // 
            this.lblReadOnly.AutoSize = true;
            this.lblReadOnly.Location = new System.Drawing.Point(13, 26);
            this.lblReadOnly.Name = "lblReadOnly";
            this.lblReadOnly.Size = new System.Drawing.Size(59, 15);
            this.lblReadOnly.TabIndex = 0;
            this.lblReadOnly.Text = "Read only";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(226, 55);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkLogEnabled.TabIndex = 3;
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblLogEnabled
            // 
            this.lblLogEnabled.AutoSize = true;
            this.lblLogEnabled.Location = new System.Drawing.Point(13, 55);
            this.lblLogEnabled.Name = "lblLogEnabled";
            this.lblLogEnabled.Size = new System.Drawing.Size(72, 15);
            this.lblLogEnabled.TabIndex = 2;
            this.lblLogEnabled.Text = "Log enabled";
            // 
            // numRetention
            // 
            this.numRetention.Location = new System.Drawing.Point(196, 80);
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
            this.numRetention.TabIndex = 5;
            this.numRetention.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRetention
            // 
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(13, 84);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(95, 15);
            this.lblRetention.TabIndex = 4;
            this.lblRetention.Text = "Retention period";
            // 
            // numPullToPeriod
            // 
            this.numPullToPeriod.Location = new System.Drawing.Point(196, 196);
            this.numPullToPeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numPullToPeriod.Name = "numPullToPeriod";
            this.numPullToPeriod.Size = new System.Drawing.Size(75, 23);
            this.numPullToPeriod.TabIndex = 15;
            // 
            // lblPullToPeriod
            // 
            this.lblPullToPeriod.AutoSize = true;
            this.lblPullToPeriod.Location = new System.Drawing.Point(13, 200);
            this.lblPullToPeriod.Name = "lblPullToPeriod";
            this.lblPullToPeriod.Size = new System.Drawing.Size(78, 15);
            this.lblPullToPeriod.TabIndex = 14;
            this.lblPullToPeriod.Text = "Pull to period";
            // 
            // cbWritingUnit
            // 
            this.cbWritingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWritingUnit.FormattingEnabled = true;
            this.cbWritingUnit.Items.AddRange(new object[] {
            "Sec",
            "Min",
            "Hour"});
            this.cbWritingUnit.Location = new System.Drawing.Point(277, 167);
            this.cbWritingUnit.Name = "cbWritingUnit";
            this.cbWritingUnit.Size = new System.Drawing.Size(70, 23);
            this.cbWritingUnit.TabIndex = 13;
            // 
            // numWritingPeriod
            // 
            this.numWritingPeriod.Location = new System.Drawing.Point(196, 167);
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
            this.numWritingPeriod.TabIndex = 12;
            this.numWritingPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblWritingPeriod
            // 
            this.lblWritingPeriod.AutoSize = true;
            this.lblWritingPeriod.Location = new System.Drawing.Point(13, 171);
            this.lblWritingPeriod.Name = "lblWritingPeriod";
            this.lblWritingPeriod.Size = new System.Drawing.Size(83, 15);
            this.lblWritingPeriod.TabIndex = 11;
            this.lblWritingPeriod.Text = "Writing period";
            // 
            // txtRetentionUnit
            // 
            this.txtRetentionUnit.Location = new System.Drawing.Point(277, 80);
            this.txtRetentionUnit.Name = "txtRetentionUnit";
            this.txtRetentionUnit.ReadOnly = true;
            this.txtRetentionUnit.Size = new System.Drawing.Size(70, 23);
            this.txtRetentionUnit.TabIndex = 6;
            this.txtRetentionUnit.Text = "Days";
            // 
            // txtPullToPeriodUnit
            // 
            this.txtPullToPeriodUnit.Location = new System.Drawing.Point(277, 196);
            this.txtPullToPeriodUnit.Name = "txtPullToPeriodUnit";
            this.txtPullToPeriodUnit.ReadOnly = true;
            this.txtPullToPeriodUnit.Size = new System.Drawing.Size(70, 23);
            this.txtPullToPeriodUnit.TabIndex = 16;
            this.txtPullToPeriodUnit.Text = "Sec";
            // 
            // CtrlHistoricalArchiveOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Name = "CtrlHistoricalArchiveOptions";
            this.Size = new System.Drawing.Size(360, 290);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeadband)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbOptions;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private NumericUpDown numPullToPeriod;
        private Label lblPullToPeriod;
        private ComboBox cbWritingUnit;
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
        private ComboBox cbDeadband;
        private TextBox txtRetentionUnit;
        private TextBox txtPullToPeriodUnit;
    }
}
