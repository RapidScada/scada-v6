
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmBasicCAO
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
            lblUseCopyDir = new Label();
            chkUseCopyDir = new CheckBox();
            btnOK = new Button();
            btnCancel = new Button();
            btnShowDir = new Button();
            gbGeneralOptions = new GroupBox();
            txtFlushPeriodUnit = new TextBox();
            numFlushPeriod = new NumericUpDown();
            lblFlushPeriod = new Label();
            chkLogEnabled = new CheckBox();
            lblLogEnabled = new Label();
            gbWritingOptions = new GroupBox();
            gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFlushPeriod).BeginInit();
            gbWritingOptions.SuspendLayout();
            SuspendLayout();
            // 
            // lblUseCopyDir
            // 
            lblUseCopyDir.AutoSize = true;
            lblUseCopyDir.Location = new Point(13, 26);
            lblUseCopyDir.Name = "lblUseCopyDir";
            lblUseCopyDir.Size = new Size(128, 15);
            lblUseCopyDir.TabIndex = 0;
            lblUseCopyDir.Text = "Write to copy directory";
            // 
            // chkUseCopyDir
            // 
            chkUseCopyDir.AutoSize = true;
            chkUseCopyDir.Location = new Point(332, 26);
            chkUseCopyDir.Name = "chkUseCopyDir";
            chkUseCopyDir.Size = new Size(15, 14);
            chkUseCopyDir.TabIndex = 1;
            chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 179);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 179);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowDir
            // 
            btnShowDir.Location = new Point(12, 179);
            btnShowDir.Name = "btnShowDir";
            btnShowDir.Size = new Size(100, 23);
            btnShowDir.TabIndex = 2;
            btnShowDir.Text = "Directories";
            btnShowDir.UseVisualStyleBackColor = true;
            btnShowDir.Click += btnShowDir_Click;
            // 
            // gbGeneralOptions
            // 
            gbGeneralOptions.Controls.Add(txtFlushPeriodUnit);
            gbGeneralOptions.Controls.Add(numFlushPeriod);
            gbGeneralOptions.Controls.Add(lblFlushPeriod);
            gbGeneralOptions.Controls.Add(chkLogEnabled);
            gbGeneralOptions.Controls.Add(lblLogEnabled);
            gbGeneralOptions.Location = new Point(12, 12);
            gbGeneralOptions.Name = "gbGeneralOptions";
            gbGeneralOptions.Padding = new Padding(10, 3, 10, 10);
            gbGeneralOptions.Size = new Size(360, 87);
            gbGeneralOptions.TabIndex = 0;
            gbGeneralOptions.TabStop = false;
            gbGeneralOptions.Text = "General Options";
            // 
            // txtFlushPeriodUnit
            // 
            txtFlushPeriodUnit.Location = new Point(277, 51);
            txtFlushPeriodUnit.Name = "txtFlushPeriodUnit";
            txtFlushPeriodUnit.ReadOnly = true;
            txtFlushPeriodUnit.Size = new Size(70, 23);
            txtFlushPeriodUnit.TabIndex = 4;
            txtFlushPeriodUnit.Text = "Sec";
            // 
            // numFlushPeriod
            // 
            numFlushPeriod.Location = new Point(196, 51);
            numFlushPeriod.Maximum = new decimal(new int[] { 86400, 0, 0, 0 });
            numFlushPeriod.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numFlushPeriod.Name = "numFlushPeriod";
            numFlushPeriod.Size = new Size(75, 23);
            numFlushPeriod.TabIndex = 3;
            numFlushPeriod.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblFlushPeriod
            // 
            lblFlushPeriod.AutoSize = true;
            lblFlushPeriod.Location = new Point(13, 55);
            lblFlushPeriod.Name = "lblFlushPeriod";
            lblFlushPeriod.Size = new Size(72, 15);
            lblFlushPeriod.TabIndex = 2;
            lblFlushPeriod.Text = "Flush period";
            // 
            // chkLogEnabled
            // 
            chkLogEnabled.AutoSize = true;
            chkLogEnabled.Location = new Point(332, 26);
            chkLogEnabled.Name = "chkLogEnabled";
            chkLogEnabled.Size = new Size(15, 14);
            chkLogEnabled.TabIndex = 1;
            chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblLogEnabled
            // 
            lblLogEnabled.AutoSize = true;
            lblLogEnabled.Location = new Point(13, 26);
            lblLogEnabled.Name = "lblLogEnabled";
            lblLogEnabled.Size = new Size(72, 15);
            lblLogEnabled.TabIndex = 0;
            lblLogEnabled.Text = "Log enabled";
            // 
            // gbWritingOptions
            // 
            gbWritingOptions.Controls.Add(lblUseCopyDir);
            gbWritingOptions.Controls.Add(chkUseCopyDir);
            gbWritingOptions.Location = new Point(12, 105);
            gbWritingOptions.Name = "gbWritingOptions";
            gbWritingOptions.Padding = new Padding(10, 3, 10, 10);
            gbWritingOptions.Size = new Size(360, 58);
            gbWritingOptions.TabIndex = 1;
            gbWritingOptions.TabStop = false;
            gbWritingOptions.Text = "Writing Options";
            // 
            // FrmBasicCAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 214);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnShowDir);
            Controls.Add(gbWritingOptions);
            Controls.Add(gbGeneralOptions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBasicCAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Current Archive Options";
            Load += FrmHAO_Load;
            gbGeneralOptions.ResumeLayout(false);
            gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFlushPeriod).EndInit();
            gbWritingOptions.ResumeLayout(false);
            gbWritingOptions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label lblUseCopyDir;
        private System.Windows.Forms.CheckBox chkUseCopyDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowDir;
        private GroupBox gbGeneralOptions;
        private TextBox txtFlushPeriodUnit;
        private NumericUpDown numFlushPeriod;
        private Label lblFlushPeriod;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private GroupBox gbWritingOptions;
    }
}