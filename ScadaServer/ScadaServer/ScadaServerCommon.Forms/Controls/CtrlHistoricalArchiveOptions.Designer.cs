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
            this.cbDeadbandUnit = new System.Windows.Forms.ComboBox();
            this.numDeadband = new System.Windows.Forms.NumericUpDown();
            this.lblDeadband = new System.Windows.Forms.Label();
            this.chkWriteOnChange = new System.Windows.Forms.CheckBox();
            this.lblWriteOnChange = new System.Windows.Forms.Label();
            this.txtPullToPeriodUnit = new System.Windows.Forms.TextBox();
            this.numPullToPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblPullToPeriod = new System.Windows.Forms.Label();
            this.cbWritingPeriodUnit = new System.Windows.Forms.ComboBox();
            this.numWritingPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblWritingPeriod = new System.Windows.Forms.Label();
            this.chkWriteWithPeriod = new System.Windows.Forms.CheckBox();
            this.lblWriteWithPeriod = new System.Windows.Forms.Label();
            this.chkIsPeriodic = new System.Windows.Forms.CheckBox();
            this.lblIsPeriodic = new System.Windows.Forms.Label();
            this.txtRetentionUnit = new System.Windows.Forms.TextBox();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.lblReadOnly = new System.Windows.Forms.Label();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeadband)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbDeadbandUnit);
            this.gbOptions.Controls.Add(this.numDeadband);
            this.gbOptions.Controls.Add(this.lblDeadband);
            this.gbOptions.Controls.Add(this.chkWriteOnChange);
            this.gbOptions.Controls.Add(this.lblWriteOnChange);
            this.gbOptions.Controls.Add(this.txtPullToPeriodUnit);
            this.gbOptions.Controls.Add(this.numPullToPeriod);
            this.gbOptions.Controls.Add(this.lblPullToPeriod);
            this.gbOptions.Controls.Add(this.cbWritingPeriodUnit);
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
            // cbDeadbandUnit
            // 
            this.cbDeadbandUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeadbandUnit.FormattingEnabled = true;
            this.cbDeadbandUnit.Items.AddRange(new object[] {
            "Abs.",
            "%"});
            this.cbDeadbandUnit.Location = new System.Drawing.Point(277, 254);
            this.cbDeadbandUnit.Name = "cbDeadbandUnit";
            this.cbDeadbandUnit.Size = new System.Drawing.Size(70, 23);
            this.cbDeadbandUnit.TabIndex = 21;
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
            this.chkWriteOnChange.Location = new System.Drawing.Point(264, 229);
            this.chkWriteOnChange.Name = "chkWriteOnChange";
            this.chkWriteOnChange.Size = new System.Drawing.Size(15, 14);
            this.chkWriteOnChange.TabIndex = 18;
            this.chkWriteOnChange.UseVisualStyleBackColor = true;
            this.chkWriteOnChange.CheckedChanged += new System.EventHandler(this.chkWriteOnChange_CheckedChanged);
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
            // txtPullToPeriodUnit
            // 
            this.txtPullToPeriodUnit.Location = new System.Drawing.Point(277, 196);
            this.txtPullToPeriodUnit.Name = "txtPullToPeriodUnit";
            this.txtPullToPeriodUnit.ReadOnly = true;
            this.txtPullToPeriodUnit.Size = new System.Drawing.Size(70, 23);
            this.txtPullToPeriodUnit.TabIndex = 16;
            this.txtPullToPeriodUnit.Text = "Sec";
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
            // cbWritingPeriodUnit
            // 
            this.cbWritingPeriodUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWritingPeriodUnit.FormattingEnabled = true;
            this.cbWritingPeriodUnit.Items.AddRange(new object[] {
            "Sec",
            "Min",
            "Hour"});
            this.cbWritingPeriodUnit.Location = new System.Drawing.Point(277, 167);
            this.cbWritingPeriodUnit.Name = "cbWritingPeriodUnit";
            this.cbWritingPeriodUnit.Size = new System.Drawing.Size(70, 23);
            this.cbWritingPeriodUnit.TabIndex = 13;
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
            // chkWriteWithPeriod
            // 
            this.chkWriteWithPeriod.AutoSize = true;
            this.chkWriteWithPeriod.Location = new System.Drawing.Point(264, 142);
            this.chkWriteWithPeriod.Name = "chkWriteWithPeriod";
            this.chkWriteWithPeriod.Size = new System.Drawing.Size(15, 14);
            this.chkWriteWithPeriod.TabIndex = 10;
            this.chkWriteWithPeriod.UseVisualStyleBackColor = true;
            this.chkWriteWithPeriod.CheckedChanged += new System.EventHandler(this.chkWriteWithPeriod_CheckedChanged);
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
            // chkIsPeriodic
            // 
            this.chkIsPeriodic.AutoSize = true;
            this.chkIsPeriodic.Location = new System.Drawing.Point(264, 113);
            this.chkIsPeriodic.Name = "chkIsPeriodic";
            this.chkIsPeriodic.Size = new System.Drawing.Size(15, 14);
            this.chkIsPeriodic.TabIndex = 8;
            this.chkIsPeriodic.UseVisualStyleBackColor = true;
            this.chkIsPeriodic.CheckedChanged += new System.EventHandler(this.chkIsPeriodic_CheckedChanged);
            // 
            // lblIsPeriodic
            // 
            this.lblIsPeriodic.AutoSize = true;
            this.lblIsPeriodic.Location = new System.Drawing.Point(13, 113);
            this.lblIsPeriodic.Name = "lblIsPeriodic";
            this.lblIsPeriodic.Size = new System.Drawing.Size(104, 15);
            this.lblIsPeriodic.TabIndex = 7;
            this.lblIsPeriodic.Text = "Only periodic data";
            // 
            // txtRetentionUnit
            // 
            this.txtRetentionUnit.Location = new System.Drawing.Point(277, 80);
            this.txtRetentionUnit.Name = "txtRetentionUnit";
            this.txtRetentionUnit.ReadOnly = true;
            this.txtRetentionUnit.Size = new System.Drawing.Size(70, 23);
            this.txtRetentionUnit.TabIndex = 6;
            this.txtRetentionUnit.Text = "Day";
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
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(264, 55);
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
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.Location = new System.Drawing.Point(264, 26);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(15, 14);
            this.chkReadOnly.TabIndex = 1;
            this.chkReadOnly.UseVisualStyleBackColor = true;
            this.chkReadOnly.CheckedChanged += new System.EventHandler(this.chkReadOnly_CheckedChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.numPullToPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
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
    }
}
