namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    partial class CtrlCurDataExport
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
            gbOptions = new GroupBox();
            chkIncludeCalculated = new CheckBox();
            chkSkipUnchanged = new CheckBox();
            numAllDataPeriod = new NumericUpDown();
            lblAllDataPeriod = new Label();
            numTimePeriod = new NumericUpDown();
            lblTimePeriod = new Label();
            cbTrigger = new ComboBox();
            lblTrigger = new Label();
            gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAllDataPeriod).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTimePeriod).BeginInit();
            SuspendLayout();
            // 
            // gbOptions
            // 
            gbOptions.Controls.Add(chkIncludeCalculated);
            gbOptions.Controls.Add(chkSkipUnchanged);
            gbOptions.Controls.Add(numAllDataPeriod);
            gbOptions.Controls.Add(lblAllDataPeriod);
            gbOptions.Controls.Add(numTimePeriod);
            gbOptions.Controls.Add(lblTimePeriod);
            gbOptions.Controls.Add(cbTrigger);
            gbOptions.Controls.Add(lblTrigger);
            gbOptions.Dock = DockStyle.Fill;
            gbOptions.Location = new Point(0, 0);
            gbOptions.Margin = new Padding(3, 3, 3, 10);
            gbOptions.Name = "gbOptions";
            gbOptions.Padding = new Padding(10, 3, 10, 10);
            gbOptions.Size = new Size(404, 462);
            gbOptions.TabIndex = 0;
            gbOptions.TabStop = false;
            gbOptions.Text = "Current Data Export Options";
            // 
            // chkIncludeCalculated
            // 
            chkIncludeCalculated.AutoSize = true;
            chkIncludeCalculated.Location = new Point(13, 207);
            chkIncludeCalculated.Margin = new Padding(3, 3, 3, 10);
            chkIncludeCalculated.Name = "chkIncludeCalculated";
            chkIncludeCalculated.Size = new Size(172, 19);
            chkIncludeCalculated.TabIndex = 8;
            chkIncludeCalculated.Text = "Include calculated channels";
            chkIncludeCalculated.UseVisualStyleBackColor = true;
            chkIncludeCalculated.CheckedChanged += chkIncludeCalculated_CheckedChanged;
            // 
            // chkSkipUnchanged
            // 
            chkSkipUnchanged.AutoSize = true;
            chkSkipUnchanged.Location = new Point(13, 175);
            chkSkipUnchanged.Margin = new Padding(3, 3, 3, 10);
            chkSkipUnchanged.Name = "chkSkipUnchanged";
            chkSkipUnchanged.Size = new Size(137, 19);
            chkSkipUnchanged.TabIndex = 7;
            chkSkipUnchanged.Text = "Skip unchanged data";
            chkSkipUnchanged.UseVisualStyleBackColor = true;
            chkSkipUnchanged.CheckedChanged += chkSkipUnchanged_CheckedChanged;
            // 
            // numAllDataPeriod
            // 
            numAllDataPeriod.Location = new Point(13, 139);
            numAllDataPeriod.Margin = new Padding(3, 3, 3, 10);
            numAllDataPeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numAllDataPeriod.Name = "numAllDataPeriod";
            numAllDataPeriod.Size = new Size(120, 23);
            numAllDataPeriod.TabIndex = 6;
            numAllDataPeriod.Value = new decimal(new int[] { 60, 0, 0, 0 });
            numAllDataPeriod.ValueChanged += numAllDataPeriod_ValueChanged;
            // 
            // lblAllDataPeriod
            // 
            lblAllDataPeriod.AutoSize = true;
            lblAllDataPeriod.Location = new Point(10, 121);
            lblAllDataPeriod.Name = "lblAllDataPeriod";
            lblAllDataPeriod.Size = new Size(237, 15);
            lblAllDataPeriod.TabIndex = 5;
            lblAllDataPeriod.Text = "Period of exporting data of all channels, sec";
            // 
            // numTimePeriod
            // 
            numTimePeriod.Location = new Point(13, 88);
            numTimePeriod.Margin = new Padding(3, 3, 3, 10);
            numTimePeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numTimePeriod.Name = "numTimePeriod";
            numTimePeriod.Size = new Size(120, 23);
            numTimePeriod.TabIndex = 4;
            numTimePeriod.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numTimePeriod.ValueChanged += numTimePeriod_ValueChanged;
            // 
            // lblTimePeriod
            // 
            lblTimePeriod.AutoSize = true;
            lblTimePeriod.Location = new Point(10, 70);
            lblTimePeriod.Name = "lblTimePeriod";
            lblTimePeriod.Size = new Size(97, 15);
            lblTimePeriod.TabIndex = 3;
            lblTimePeriod.Text = "Timer period, sec";
            // 
            // cbTrigger
            // 
            cbTrigger.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTrigger.FormattingEnabled = true;
            cbTrigger.Items.AddRange(new object[] { "On Receive", "On Timer" });
            cbTrigger.Location = new Point(13, 37);
            cbTrigger.Margin = new Padding(3, 3, 3, 10);
            cbTrigger.Name = "cbTrigger";
            cbTrigger.Size = new Size(120, 23);
            cbTrigger.TabIndex = 2;
            cbTrigger.SelectedIndexChanged += cbTrigger_SelectedIndexChanged;
            // 
            // lblTrigger
            // 
            lblTrigger.AutoSize = true;
            lblTrigger.Location = new Point(10, 19);
            lblTrigger.Name = "lblTrigger";
            lblTrigger.Size = new Size(43, 15);
            lblTrigger.TabIndex = 1;
            lblTrigger.Text = "Trigger";
            // 
            // CtrlCurDataExport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbOptions);
            Margin = new Padding(3, 3, 3, 10);
            Name = "CtrlCurDataExport";
            Size = new Size(404, 462);
            gbOptions.ResumeLayout(false);
            gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAllDataPeriod).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTimePeriod).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbOptions;
        private CheckBox chkIncludeCalculated;
        private CheckBox chkSkipUnchanged;
        private NumericUpDown numAllDataPeriod;
        private Label lblAllDataPeriod;
        private NumericUpDown numTimePeriod;
        private Label lblTimePeriod;
        private ComboBox cbTrigger;
        private Label lblTrigger;
    }
}
