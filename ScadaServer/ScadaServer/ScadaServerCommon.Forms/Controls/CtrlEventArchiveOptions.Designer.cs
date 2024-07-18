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
            txtRetentionUnit = new TextBox();
            numRetention = new NumericUpDown();
            lblRetention = new Label();
            chkLogEnabled = new CheckBox();
            lblLogEnabled = new Label();
            chkReadOnly = new CheckBox();
            lblReadOnly = new Label();
            ((System.ComponentModel.ISupportInitialize)numRetention).BeginInit();
            SuspendLayout();
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
            // CtrlEventArchiveOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtRetentionUnit);
            Controls.Add(numRetention);
            Controls.Add(lblRetention);
            Controls.Add(chkLogEnabled);
            Controls.Add(lblLogEnabled);
            Controls.Add(chkReadOnly);
            Controls.Add(lblReadOnly);
            Name = "CtrlEventArchiveOptions";
            Size = new Size(360, 80);
            ((System.ComponentModel.ISupportInitialize)numRetention).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtRetentionUnit;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private CheckBox chkReadOnly;
        private Label lblReadOnly;
    }
}
