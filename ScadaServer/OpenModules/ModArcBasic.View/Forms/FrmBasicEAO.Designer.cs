
namespace Scada.Server.Modules.ModArcBasic.View.Forms
{
    partial class FrmBasicEAO
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
            btnOK = new Button();
            btnCancel = new Button();
            btnShowDir = new Button();
            gbGeneralOptions = new GroupBox();
            txtRetentionUnit = new TextBox();
            numRetention = new NumericUpDown();
            lblRetention = new Label();
            chkLogEnabled = new CheckBox();
            lblLogEnabled = new Label();
            gbWritingOptions = new GroupBox();
            numMaxQueueSize = new NumericUpDown();
            lblMaxQueueSize = new Label();
            lblUseCopyDir = new Label();
            chkUseCopyDir = new CheckBox();
            gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRetention).BeginInit();
            gbWritingOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 209);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 209);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowDir
            // 
            btnShowDir.Location = new Point(12, 209);
            btnShowDir.Name = "btnShowDir";
            btnShowDir.Size = new Size(100, 23);
            btnShowDir.TabIndex = 2;
            btnShowDir.Text = "Directories";
            btnShowDir.UseVisualStyleBackColor = true;
            btnShowDir.Click += btnShowDir_Click;
            // 
            // gbGeneralOptions
            // 
            gbGeneralOptions.Controls.Add(txtRetentionUnit);
            gbGeneralOptions.Controls.Add(numRetention);
            gbGeneralOptions.Controls.Add(lblRetention);
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
            // txtRetentionUnit
            // 
            txtRetentionUnit.Location = new Point(277, 51);
            txtRetentionUnit.Name = "txtRetentionUnit";
            txtRetentionUnit.ReadOnly = true;
            txtRetentionUnit.Size = new Size(70, 23);
            txtRetentionUnit.TabIndex = 4;
            txtRetentionUnit.Text = "Sec";
            // 
            // numRetention
            // 
            numRetention.Location = new Point(196, 51);
            numRetention.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numRetention.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRetention.Name = "numRetention";
            numRetention.Size = new Size(75, 23);
            numRetention.TabIndex = 3;
            numRetention.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblRetention
            // 
            lblRetention.AutoSize = true;
            lblRetention.Location = new Point(13, 55);
            lblRetention.Name = "lblRetention";
            lblRetention.Size = new Size(95, 15);
            lblRetention.TabIndex = 2;
            lblRetention.Text = "Retention period";
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
            gbWritingOptions.Controls.Add(numMaxQueueSize);
            gbWritingOptions.Controls.Add(lblMaxQueueSize);
            gbWritingOptions.Controls.Add(lblUseCopyDir);
            gbWritingOptions.Controls.Add(chkUseCopyDir);
            gbWritingOptions.Location = new Point(12, 105);
            gbWritingOptions.Name = "gbWritingOptions";
            gbWritingOptions.Padding = new Padding(10, 3, 10, 10);
            gbWritingOptions.Size = new Size(360, 88);
            gbWritingOptions.TabIndex = 1;
            gbWritingOptions.TabStop = false;
            gbWritingOptions.Text = "Writing Options";
            // 
            // numMaxQueueSize
            // 
            numMaxQueueSize.Location = new Point(196, 52);
            numMaxQueueSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxQueueSize.Name = "numMaxQueueSize";
            numMaxQueueSize.Size = new Size(151, 23);
            numMaxQueueSize.TabIndex = 3;
            // 
            // lblMaxQueueSize
            // 
            lblMaxQueueSize.AutoSize = true;
            lblMaxQueueSize.Location = new Point(20, 56);
            lblMaxQueueSize.Name = "lblMaxQueueSize";
            lblMaxQueueSize.Size = new Size(120, 15);
            lblMaxQueueSize.TabIndex = 2;
            lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // lblUseCopyDir
            // 
            lblUseCopyDir.AutoSize = true;
            lblUseCopyDir.Location = new Point(20, 26);
            lblUseCopyDir.Name = "lblUseCopyDir";
            lblUseCopyDir.Size = new Size(128, 15);
            lblUseCopyDir.TabIndex = 0;
            lblUseCopyDir.Text = "Write to copy directory";
            // 
            // chkUseCopyDir
            // 
            chkUseCopyDir.AutoSize = true;
            chkUseCopyDir.Location = new Point(339, 26);
            chkUseCopyDir.Name = "chkUseCopyDir";
            chkUseCopyDir.Size = new Size(15, 14);
            chkUseCopyDir.TabIndex = 1;
            chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // FrmBasicEAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 244);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnShowDir);
            Controls.Add(gbWritingOptions);
            Controls.Add(gbGeneralOptions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmBasicEAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Event Archive Options";
            Load += FrmHAO_Load;
            gbGeneralOptions.ResumeLayout(false);
            gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRetention).EndInit();
            gbWritingOptions.ResumeLayout(false);
            gbWritingOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowDir;
        private GroupBox gbGeneralOptions;
        private TextBox txtRetentionUnit;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
        private GroupBox gbWritingOptions;
        private NumericUpDown numMaxQueueSize;
        private Label lblMaxQueueSize;
        private Label lblUseCopyDir;
        private CheckBox chkUseCopyDir;
    }
}