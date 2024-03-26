namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    partial class FrmTableOptions
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
            chkUseDefault = new CheckBox();
            txtArchiveCode = new TextBox();
            numPeriod = new NumericUpDown();
            txtChartArgs = new TextBox();
            lblArchiveCode = new Label();
            lblPeriod = new Label();
            lblChartArgs = new Label();
            btnSelectArchiveCode = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            pnlOptions = new Panel();
            ((System.ComponentModel.ISupportInitialize)numPeriod).BeginInit();
            pnlOptions.SuspendLayout();
            SuspendLayout();
            // 
            // chkUseDefault
            // 
            chkUseDefault.AutoSize = true;
            chkUseDefault.Location = new Point(12, 12);
            chkUseDefault.Margin = new Padding(3, 3, 3, 10);
            chkUseDefault.Name = "chkUseDefault";
            chkUseDefault.Size = new Size(128, 19);
            chkUseDefault.TabIndex = 0;
            chkUseDefault.Text = "Use default options";
            chkUseDefault.UseVisualStyleBackColor = true;
            chkUseDefault.CheckedChanged += chkUseDefault_CheckedChanged;
            // 
            // txtArchiveCode
            // 
            txtArchiveCode.Location = new Point(0, 18);
            txtArchiveCode.Margin = new Padding(3, 3, 3, 10);
            txtArchiveCode.Name = "txtArchiveCode";
            txtArchiveCode.Size = new Size(100, 23);
            txtArchiveCode.TabIndex = 1;
            // 
            // numPeriod
            // 
            numPeriod.Location = new Point(0, 69);
            numPeriod.Margin = new Padding(3, 3, 3, 10);
            numPeriod.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numPeriod.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPeriod.Name = "numPeriod";
            numPeriod.Size = new Size(100, 23);
            numPeriod.TabIndex = 4;
            numPeriod.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtChartArgs
            // 
            txtChartArgs.Location = new Point(0, 120);
            txtChartArgs.Margin = new Padding(3, 3, 3, 10);
            txtChartArgs.Name = "txtChartArgs";
            txtChartArgs.Size = new Size(360, 23);
            txtChartArgs.TabIndex = 6;
            // 
            // lblArchiveCode
            // 
            lblArchiveCode.AutoSize = true;
            lblArchiveCode.Location = new Point(-3, 0);
            lblArchiveCode.Name = "lblArchiveCode";
            lblArchiveCode.Size = new Size(76, 15);
            lblArchiveCode.TabIndex = 0;
            lblArchiveCode.Text = "Archive code";
            // 
            // lblPeriod
            // 
            lblPeriod.AutoSize = true;
            lblPeriod.Location = new Point(-3, 51);
            lblPeriod.Name = "lblPeriod";
            lblPeriod.Size = new Size(98, 15);
            lblPeriod.TabIndex = 3;
            lblPeriod.Text = "Table period, min";
            // 
            // lblChartArgs
            // 
            lblChartArgs.AutoSize = true;
            lblChartArgs.Location = new Point(-3, 102);
            lblChartArgs.Name = "lblChartArgs";
            lblChartArgs.Size = new Size(96, 15);
            lblChartArgs.TabIndex = 5;
            lblChartArgs.Text = "Chart arguments";
            // 
            // btnSelectArchiveCode
            // 
            btnSelectArchiveCode.Location = new Point(106, 18);
            btnSelectArchiveCode.Name = "btnSelectArchiveCode";
            btnSelectArchiveCode.Size = new Size(75, 23);
            btnSelectArchiveCode.TabIndex = 2;
            btnSelectArchiveCode.Text = "Select...";
            btnSelectArchiveCode.UseVisualStyleBackColor = true;
            btnSelectArchiveCode.Click += btnSelectArchiveCode_Click;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 197);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 2;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 197);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlOptions
            // 
            pnlOptions.Controls.Add(txtChartArgs);
            pnlOptions.Controls.Add(lblChartArgs);
            pnlOptions.Controls.Add(numPeriod);
            pnlOptions.Controls.Add(lblPeriod);
            pnlOptions.Controls.Add(btnSelectArchiveCode);
            pnlOptions.Controls.Add(txtArchiveCode);
            pnlOptions.Controls.Add(lblArchiveCode);
            pnlOptions.Location = new Point(12, 41);
            pnlOptions.Margin = new Padding(3, 0, 3, 3);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new Size(360, 150);
            pnlOptions.TabIndex = 1;
            // 
            // FrmTableOptions
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 232);
            Controls.Add(pnlOptions);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(chkUseDefault);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmTableOptions";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Table View Options";
            Load += FrmTableOptions_Load;
            ((System.ComponentModel.ISupportInitialize)numPeriod).EndInit();
            pnlOptions.ResumeLayout(false);
            pnlOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkUseDefault;
        private TextBox txtArchiveCode;
        private NumericUpDown numPeriod;
        private TextBox txtChartArgs;
        private Label lblArchiveCode;
        private Label lblPeriod;
        private Label lblChartArgs;
        private Button btnSelectArchiveCode;
        private Button btnOK;
        private Button btnCancel;
        private Panel pnlOptions;
    }
}