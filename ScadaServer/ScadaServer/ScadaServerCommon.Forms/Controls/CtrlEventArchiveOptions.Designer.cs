namespace Scada.Server.Forms.Controls
{
    partial class CtrlEventArchiveOptions
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
            this.txtRetentionUnit = new System.Windows.Forms.TextBox();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.lblReadOnly = new System.Windows.Forms.Label();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
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
            this.gbOptions.Size = new System.Drawing.Size(360, 116);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "General Options";
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
            this.chkLogEnabled.Location = new System.Drawing.Point(332, 55);
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
            this.chkReadOnly.Location = new System.Drawing.Point(332, 26);
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
            // CtrlEventArchiveOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Name = "CtrlEventArchiveOptions";
            this.Size = new System.Drawing.Size(360, 116);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbOptions;
        private TextBox txtRetentionUnit;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private CheckBox chkReadOnly;
        private Label lblReadOnly;
    }
}
