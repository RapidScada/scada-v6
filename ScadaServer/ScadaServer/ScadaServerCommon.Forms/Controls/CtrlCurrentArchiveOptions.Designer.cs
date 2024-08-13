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
            txtFlushPeriodUnit = new TextBox();
            numFlushPeriod = new NumericUpDown();
            lblFlushPeriod = new Label();
            chkLogEnabled = new CheckBox();
            lblLogEnabled = new Label();
            chkReadOnly = new CheckBox();
            lblReadOnly = new Label();
            ((System.ComponentModel.ISupportInitialize)numFlushPeriod).BeginInit();
            SuspendLayout();
            // 
            // txtFlushPeriodUnit
            // 
            txtFlushPeriodUnit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtFlushPeriodUnit.Location = new Point(290, 54);
            txtFlushPeriodUnit.Name = "txtFlushPeriodUnit";
            txtFlushPeriodUnit.ReadOnly = true;
            txtFlushPeriodUnit.Size = new Size(70, 23);
            txtFlushPeriodUnit.TabIndex = 6;
            txtFlushPeriodUnit.Text = "Sec";
            // 
            // numFlushPeriod
            // 
            numFlushPeriod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numFlushPeriod.Location = new Point(209, 54);
            numFlushPeriod.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numFlushPeriod.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFlushPeriod.Name = "numFlushPeriod";
            numFlushPeriod.Size = new Size(75, 23);
            numFlushPeriod.TabIndex = 5;
            numFlushPeriod.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFlushPeriod
            // 
            lblFlushPeriod.AutoSize = true;
            lblFlushPeriod.Location = new Point(-3, 58);
            lblFlushPeriod.Name = "lblFlushPeriod";
            lblFlushPeriod.Size = new Size(72, 15);
            lblFlushPeriod.TabIndex = 4;
            lblFlushPeriod.Text = "Flush period";
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
            // CtrlCurrentArchiveOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtFlushPeriodUnit);
            Controls.Add(numFlushPeriod);
            Controls.Add(lblFlushPeriod);
            Controls.Add(chkLogEnabled);
            Controls.Add(lblLogEnabled);
            Controls.Add(chkReadOnly);
            Controls.Add(lblReadOnly);
            Name = "CtrlCurrentArchiveOptions";
            Size = new Size(360, 80);
            ((System.ComponentModel.ISupportInitialize)numFlushPeriod).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtFlushPeriodUnit;
        private NumericUpDown numFlushPeriod;
        private Label lblFlushPeriod;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private CheckBox chkReadOnly;
        private Label lblReadOnly;
    }
}
