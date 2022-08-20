namespace Scada.Server.Forms.Controls
{
    partial class CtrlCurrentArchiveOptions
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
            this.txtFlushPeriodUnit = new System.Windows.Forms.TextBox();
            this.numFlushPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblFlushPeriod = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.lblReadOnly = new System.Windows.Forms.Label();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFlushPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.txtFlushPeriodUnit);
            this.gbOptions.Controls.Add(this.numFlushPeriod);
            this.gbOptions.Controls.Add(this.lblFlushPeriod);
            this.gbOptions.Controls.Add(this.chkLogEnabled);
            this.gbOptions.Controls.Add(this.lblLogEnabled);
            this.gbOptions.Controls.Add(this.chkReadOnly);
            this.gbOptions.Controls.Add(this.lblReadOnly);
            this.gbOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbOptions.Location = new System.Drawing.Point(0, 0);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(360, 116);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "General Options";
            // 
            // txtFlushPeriodUnit
            // 
            this.txtFlushPeriodUnit.Location = new System.Drawing.Point(277, 80);
            this.txtFlushPeriodUnit.Name = "txtFlushPeriodUnit";
            this.txtFlushPeriodUnit.ReadOnly = true;
            this.txtFlushPeriodUnit.Size = new System.Drawing.Size(70, 23);
            this.txtFlushPeriodUnit.TabIndex = 6;
            this.txtFlushPeriodUnit.Text = "Sec";
            // 
            // numFlushPeriod
            // 
            this.numFlushPeriod.Location = new System.Drawing.Point(196, 80);
            this.numFlushPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFlushPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFlushPeriod.Name = "numFlushPeriod";
            this.numFlushPeriod.Size = new System.Drawing.Size(75, 23);
            this.numFlushPeriod.TabIndex = 5;
            this.numFlushPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblFlushPeriod
            // 
            this.lblFlushPeriod.AutoSize = true;
            this.lblFlushPeriod.Location = new System.Drawing.Point(13, 84);
            this.lblFlushPeriod.Name = "lblFlushPeriod";
            this.lblFlushPeriod.Size = new System.Drawing.Size(72, 15);
            this.lblFlushPeriod.TabIndex = 4;
            this.lblFlushPeriod.Text = "Flush period";
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
            // CtrlCurrentArchiveOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Name = "CtrlCurrentArchiveOptions";
            this.Size = new System.Drawing.Size(360, 116);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFlushPeriod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbOptions;
        private TextBox txtFlushPeriodUnit;
        private NumericUpDown numFlushPeriod;
        private Label lblFlushPeriod;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private CheckBox chkReadOnly;
        private Label lblReadOnly;
    }
}
