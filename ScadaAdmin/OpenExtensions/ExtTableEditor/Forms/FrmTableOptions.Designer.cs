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
            this.chkUseDefault = new System.Windows.Forms.CheckBox();
            this.txtArchiveCode = new System.Windows.Forms.TextBox();
            this.numPeriod = new System.Windows.Forms.NumericUpDown();
            this.txtChartArgs = new System.Windows.Forms.TextBox();
            this.lblArchiveCode = new System.Windows.Forms.Label();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.lblChartArgs = new System.Windows.Forms.Label();
            this.btnSelectArchiveCode = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlOptions = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriod)).BeginInit();
            this.pnlOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUseDefault
            // 
            this.chkUseDefault.AutoSize = true;
            this.chkUseDefault.Location = new System.Drawing.Point(12, 12);
            this.chkUseDefault.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkUseDefault.Name = "chkUseDefault";
            this.chkUseDefault.Size = new System.Drawing.Size(128, 19);
            this.chkUseDefault.TabIndex = 0;
            this.chkUseDefault.Text = "Use default options";
            this.chkUseDefault.UseVisualStyleBackColor = true;
            this.chkUseDefault.CheckedChanged += new System.EventHandler(this.chkUseDefault_CheckedChanged);
            // 
            // txtArchiveCode
            // 
            this.txtArchiveCode.Location = new System.Drawing.Point(0, 18);
            this.txtArchiveCode.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtArchiveCode.Name = "txtArchiveCode";
            this.txtArchiveCode.Size = new System.Drawing.Size(100, 23);
            this.txtArchiveCode.TabIndex = 1;
            // 
            // numPeriod
            // 
            this.numPeriod.Location = new System.Drawing.Point(0, 69);
            this.numPeriod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numPeriod.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPeriod.Name = "numPeriod";
            this.numPeriod.Size = new System.Drawing.Size(100, 23);
            this.numPeriod.TabIndex = 4;
            this.numPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtChartArgs
            // 
            this.txtChartArgs.Location = new System.Drawing.Point(0, 120);
            this.txtChartArgs.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtChartArgs.Name = "txtChartArgs";
            this.txtChartArgs.Size = new System.Drawing.Size(360, 23);
            this.txtChartArgs.TabIndex = 6;
            // 
            // lblArchiveCode
            // 
            this.lblArchiveCode.AutoSize = true;
            this.lblArchiveCode.Location = new System.Drawing.Point(-3, 0);
            this.lblArchiveCode.Name = "lblArchiveCode";
            this.lblArchiveCode.Size = new System.Drawing.Size(76, 15);
            this.lblArchiveCode.TabIndex = 0;
            this.lblArchiveCode.Text = "Archive code";
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(-3, 51);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(98, 15);
            this.lblPeriod.TabIndex = 3;
            this.lblPeriod.Text = "Table period, min";
            // 
            // lblChartArgs
            // 
            this.lblChartArgs.AutoSize = true;
            this.lblChartArgs.Location = new System.Drawing.Point(-3, 102);
            this.lblChartArgs.Name = "lblChartArgs";
            this.lblChartArgs.Size = new System.Drawing.Size(96, 15);
            this.lblChartArgs.TabIndex = 5;
            this.lblChartArgs.Text = "Chart arguments";
            // 
            // btnSelectArchiveCode
            // 
            this.btnSelectArchiveCode.Location = new System.Drawing.Point(106, 18);
            this.btnSelectArchiveCode.Name = "btnSelectArchiveCode";
            this.btnSelectArchiveCode.Size = new System.Drawing.Size(75, 23);
            this.btnSelectArchiveCode.TabIndex = 2;
            this.btnSelectArchiveCode.Text = "Select...";
            this.btnSelectArchiveCode.UseVisualStyleBackColor = true;
            this.btnSelectArchiveCode.Click += new System.EventHandler(this.btnSelectArchiveCode_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 197);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 197);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlOptions
            // 
            this.pnlOptions.Controls.Add(this.txtChartArgs);
            this.pnlOptions.Controls.Add(this.lblChartArgs);
            this.pnlOptions.Controls.Add(this.numPeriod);
            this.pnlOptions.Controls.Add(this.lblPeriod);
            this.pnlOptions.Controls.Add(this.btnSelectArchiveCode);
            this.pnlOptions.Controls.Add(this.txtArchiveCode);
            this.pnlOptions.Controls.Add(this.lblArchiveCode);
            this.pnlOptions.Location = new System.Drawing.Point(12, 41);
            this.pnlOptions.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(360, 150);
            this.pnlOptions.TabIndex = 1;
            // 
            // FrmTableOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 232);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkUseDefault);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTableOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Table View Options";
            this.Load += new System.EventHandler(this.FrmTableOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPeriod)).EndInit();
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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