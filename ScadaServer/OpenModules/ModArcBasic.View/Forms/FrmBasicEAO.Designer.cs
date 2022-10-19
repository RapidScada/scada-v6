
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShowDir = new System.Windows.Forms.Button();
            this.gbPathOptions = new System.Windows.Forms.GroupBox();
            this.lblUseCopyDir = new System.Windows.Forms.Label();
            this.chkUseCopyDir = new System.Windows.Forms.CheckBox();
            this.gbGeneralOptions = new System.Windows.Forms.GroupBox();
            this.txtRetentionUnit = new System.Windows.Forms.TextBox();
            this.numRetention = new System.Windows.Forms.NumericUpDown();
            this.lblRetention = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.gbPathOptions.SuspendLayout();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 179);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnShowDir
            // 
            this.btnShowDir.Location = new System.Drawing.Point(12, 179);
            this.btnShowDir.Name = "btnShowDir";
            this.btnShowDir.Size = new System.Drawing.Size(100, 23);
            this.btnShowDir.TabIndex = 2;
            this.btnShowDir.Text = "Directories";
            this.btnShowDir.UseVisualStyleBackColor = true;
            this.btnShowDir.Click += new System.EventHandler(this.btnShowDir_Click);
            // 
            // gbPathOptions
            // 
            this.gbPathOptions.Controls.Add(this.lblUseCopyDir);
            this.gbPathOptions.Controls.Add(this.chkUseCopyDir);
            this.gbPathOptions.Location = new System.Drawing.Point(12, 105);
            this.gbPathOptions.Name = "gbPathOptions";
            this.gbPathOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPathOptions.Size = new System.Drawing.Size(360, 58);
            this.gbPathOptions.TabIndex = 1;
            this.gbPathOptions.TabStop = false;
            this.gbPathOptions.Text = "Path Options";
            // 
            // lblUseCopyDir
            // 
            this.lblUseCopyDir.AutoSize = true;
            this.lblUseCopyDir.Location = new System.Drawing.Point(13, 26);
            this.lblUseCopyDir.Name = "lblUseCopyDir";
            this.lblUseCopyDir.Size = new System.Drawing.Size(128, 15);
            this.lblUseCopyDir.TabIndex = 0;
            this.lblUseCopyDir.Text = "Write to copy directory";
            // 
            // chkUseCopyDir
            // 
            this.chkUseCopyDir.AutoSize = true;
            this.chkUseCopyDir.Location = new System.Drawing.Point(332, 26);
            this.chkUseCopyDir.Name = "chkUseCopyDir";
            this.chkUseCopyDir.Size = new System.Drawing.Size(15, 14);
            this.chkUseCopyDir.TabIndex = 1;
            this.chkUseCopyDir.UseVisualStyleBackColor = true;
            // 
            // gbGeneralOptions
            // 
            this.gbGeneralOptions.Controls.Add(this.txtRetentionUnit);
            this.gbGeneralOptions.Controls.Add(this.numRetention);
            this.gbGeneralOptions.Controls.Add(this.lblRetention);
            this.gbGeneralOptions.Controls.Add(this.chkLogEnabled);
            this.gbGeneralOptions.Controls.Add(this.lblLogEnabled);
            this.gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(360, 87);
            this.gbGeneralOptions.TabIndex = 0;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // txtRetentionUnit
            // 
            this.txtRetentionUnit.Location = new System.Drawing.Point(277, 51);
            this.txtRetentionUnit.Name = "txtRetentionUnit";
            this.txtRetentionUnit.ReadOnly = true;
            this.txtRetentionUnit.Size = new System.Drawing.Size(70, 23);
            this.txtRetentionUnit.TabIndex = 4;
            this.txtRetentionUnit.Text = "Sec";
            // 
            // numRetention
            // 
            this.numRetention.Location = new System.Drawing.Point(196, 51);
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
            this.numRetention.TabIndex = 3;
            this.numRetention.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRetention
            // 
            this.lblRetention.AutoSize = true;
            this.lblRetention.Location = new System.Drawing.Point(13, 55);
            this.lblRetention.Name = "lblRetention";
            this.lblRetention.Size = new System.Drawing.Size(95, 15);
            this.lblRetention.TabIndex = 2;
            this.lblRetention.Text = "Retention period";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(332, 26);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkLogEnabled.TabIndex = 1;
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblLogEnabled
            // 
            this.lblLogEnabled.AutoSize = true;
            this.lblLogEnabled.Location = new System.Drawing.Point(13, 26);
            this.lblLogEnabled.Name = "lblLogEnabled";
            this.lblLogEnabled.Size = new System.Drawing.Size(72, 15);
            this.lblLogEnabled.TabIndex = 0;
            this.lblLogEnabled.Text = "Log enabled";
            // 
            // FrmBasicEAO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 214);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnShowDir);
            this.Controls.Add(this.gbPathOptions);
            this.Controls.Add(this.gbGeneralOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBasicEAO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Event Archive Options";
            this.Load += new System.EventHandler(this.FrmHAO_Load);
            this.gbPathOptions.ResumeLayout(false);
            this.gbPathOptions.PerformLayout();
            this.gbGeneralOptions.ResumeLayout(false);
            this.gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRetention)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShowDir;
        private GroupBox gbPathOptions;
        private Label lblUseCopyDir;
        private CheckBox chkUseCopyDir;
        private GroupBox gbGeneralOptions;
        private TextBox txtRetentionUnit;
        private NumericUpDown numRetention;
        private Label lblRetention;
        private CheckBox chkLogEnabled;
        private Label lblLogEnabled;
    }
}