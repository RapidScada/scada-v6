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
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.chkIncludeCalculated = new System.Windows.Forms.CheckBox();
            this.chkSkipUnchanged = new System.Windows.Forms.CheckBox();
            this.numAllDataPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblAllDataPeriod = new System.Windows.Forms.Label();
            this.numTimePeriod = new System.Windows.Forms.NumericUpDown();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.cbTrigger = new System.Windows.Forms.ComboBox();
            this.lblTrigger = new System.Windows.Forms.Label();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAllDataPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimePeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.chkIncludeCalculated);
            this.gbOptions.Controls.Add(this.chkSkipUnchanged);
            this.gbOptions.Controls.Add(this.numAllDataPeriod);
            this.gbOptions.Controls.Add(this.lblAllDataPeriod);
            this.gbOptions.Controls.Add(this.numTimePeriod);
            this.gbOptions.Controls.Add(this.lblTimePeriod);
            this.gbOptions.Controls.Add(this.cbTrigger);
            this.gbOptions.Controls.Add(this.lblTrigger);
            this.gbOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOptions.Location = new System.Drawing.Point(0, 0);
            this.gbOptions.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(404, 462);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Current Data Transfer Options";
            // 
            // chkIncludeCalculated
            // 
            this.chkIncludeCalculated.AutoSize = true;
            this.chkIncludeCalculated.Location = new System.Drawing.Point(13, 210);
            this.chkIncludeCalculated.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkIncludeCalculated.Name = "chkIncludeCalculated";
            this.chkIncludeCalculated.Size = new System.Drawing.Size(172, 19);
            this.chkIncludeCalculated.TabIndex = 8;
            this.chkIncludeCalculated.Text = "Include calculated channels";
            this.chkIncludeCalculated.UseVisualStyleBackColor = true;
            this.chkIncludeCalculated.CheckedChanged += new System.EventHandler(this.chkIncludeCalculated_CheckedChanged);
            // 
            // chkSkipUnchanged
            // 
            this.chkSkipUnchanged.AutoSize = true;
            this.chkSkipUnchanged.Location = new System.Drawing.Point(13, 178);
            this.chkSkipUnchanged.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkSkipUnchanged.Name = "chkSkipUnchanged";
            this.chkSkipUnchanged.Size = new System.Drawing.Size(137, 19);
            this.chkSkipUnchanged.TabIndex = 7;
            this.chkSkipUnchanged.Text = "Skip unchanged data";
            this.chkSkipUnchanged.UseVisualStyleBackColor = true;
            this.chkSkipUnchanged.CheckedChanged += new System.EventHandler(this.chkSkipUnchanged_CheckedChanged);
            // 
            // numAllDataPeriod
            // 
            this.numAllDataPeriod.Location = new System.Drawing.Point(13, 142);
            this.numAllDataPeriod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numAllDataPeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numAllDataPeriod.Name = "numAllDataPeriod";
            this.numAllDataPeriod.Size = new System.Drawing.Size(120, 23);
            this.numAllDataPeriod.TabIndex = 6;
            this.numAllDataPeriod.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numAllDataPeriod.ValueChanged += new System.EventHandler(this.numAllDataPeriod_ValueChanged);
            // 
            // lblAllDataPeriod
            // 
            this.lblAllDataPeriod.AutoSize = true;
            this.lblAllDataPeriod.Location = new System.Drawing.Point(10, 124);
            this.lblAllDataPeriod.Name = "lblAllDataPeriod";
            this.lblAllDataPeriod.Size = new System.Drawing.Size(237, 15);
            this.lblAllDataPeriod.TabIndex = 5;
            this.lblAllDataPeriod.Text = "Period of exporting data of all channels, sec";
            // 
            // numTimePeriod
            // 
            this.numTimePeriod.Location = new System.Drawing.Point(13, 91);
            this.numTimePeriod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numTimePeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numTimePeriod.Name = "numTimePeriod";
            this.numTimePeriod.Size = new System.Drawing.Size(120, 23);
            this.numTimePeriod.TabIndex = 4;
            this.numTimePeriod.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTimePeriod.ValueChanged += new System.EventHandler(this.numTimePeriod_ValueChanged);
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.AutoSize = true;
            this.lblTimePeriod.Location = new System.Drawing.Point(10, 73);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(97, 15);
            this.lblTimePeriod.TabIndex = 3;
            this.lblTimePeriod.Text = "Timer period, sec";
            // 
            // cbTrigger
            // 
            this.cbTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTrigger.FormattingEnabled = true;
            this.cbTrigger.Items.AddRange(new object[] {
            "On Receive",
            "On Timer"});
            this.cbTrigger.Location = new System.Drawing.Point(13, 40);
            this.cbTrigger.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbTrigger.Name = "cbTrigger";
            this.cbTrigger.Size = new System.Drawing.Size(120, 23);
            this.cbTrigger.TabIndex = 2;
            this.cbTrigger.SelectedIndexChanged += new System.EventHandler(this.cbTrigger_SelectedIndexChanged);
            // 
            // lblTrigger
            // 
            this.lblTrigger.AutoSize = true;
            this.lblTrigger.Location = new System.Drawing.Point(10, 22);
            this.lblTrigger.Name = "lblTrigger";
            this.lblTrigger.Size = new System.Drawing.Size(43, 15);
            this.lblTrigger.TabIndex = 1;
            this.lblTrigger.Text = "Trigger";
            // 
            // CtrlCurDataExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlCurDataExport";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAllDataPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimePeriod)).EndInit();
            this.ResumeLayout(false);

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
