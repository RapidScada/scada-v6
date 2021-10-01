
namespace Scada.Server.Modules.ModArcPostgreSql.View.Forms
{
    partial class FrmPostgreCAO
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
            this.chkUseStorageConn = new System.Windows.Forms.CheckBox();
            this.cbConnection = new System.Windows.Forms.ComboBox();
            this.btnManageConn = new System.Windows.Forms.Button();
            this.lblMaxQueueSize = new System.Windows.Forms.Label();
            this.numMaxQueueSize = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblUseStorageConn = new System.Windows.Forms.Label();
            this.lblConnection = new System.Windows.Forms.Label();
            this.gbGeneralOptions = new System.Windows.Forms.GroupBox();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblLogEnabled = new System.Windows.Forms.Label();
            this.numWritingPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblWritingPeriod = new System.Windows.Forms.Label();
            this.gbDbOptions = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).BeginInit();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).BeginInit();
            this.gbDbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUseStorageConn
            // 
            this.chkUseStorageConn.AutoSize = true;
            this.chkUseStorageConn.Location = new System.Drawing.Point(265, 26);
            this.chkUseStorageConn.Name = "chkUseStorageConn";
            this.chkUseStorageConn.Size = new System.Drawing.Size(15, 14);
            this.chkUseStorageConn.TabIndex = 1;
            this.chkUseStorageConn.UseVisualStyleBackColor = true;
            this.chkUseStorageConn.CheckedChanged += new System.EventHandler(this.chkUseStorageConn_CheckedChanged);
            // 
            // cbConnection
            // 
            this.cbConnection.FormattingEnabled = true;
            this.cbConnection.Location = new System.Drawing.Point(197, 51);
            this.cbConnection.Name = "cbConnection";
            this.cbConnection.Size = new System.Drawing.Size(150, 23);
            this.cbConnection.TabIndex = 3;
            // 
            // btnManageConn
            // 
            this.btnManageConn.Location = new System.Drawing.Point(12, 237);
            this.btnManageConn.Name = "btnManageConn";
            this.btnManageConn.Size = new System.Drawing.Size(140, 23);
            this.btnManageConn.TabIndex = 2;
            this.btnManageConn.Text = "Manage Connections";
            this.btnManageConn.UseVisualStyleBackColor = true;
            this.btnManageConn.Click += new System.EventHandler(this.btnManageConn_Click);
            // 
            // lblMaxQueueSize
            // 
            this.lblMaxQueueSize.AutoSize = true;
            this.lblMaxQueueSize.Location = new System.Drawing.Point(13, 87);
            this.lblMaxQueueSize.Name = "lblMaxQueueSize";
            this.lblMaxQueueSize.Size = new System.Drawing.Size(120, 15);
            this.lblMaxQueueSize.TabIndex = 4;
            this.lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // numMaxQueueSize
            // 
            this.numMaxQueueSize.Location = new System.Drawing.Point(197, 80);
            this.numMaxQueueSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMaxQueueSize.Name = "numMaxQueueSize";
            this.numMaxQueueSize.Size = new System.Drawing.Size(150, 23);
            this.numMaxQueueSize.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblUseStorageConn
            // 
            this.lblUseStorageConn.AutoSize = true;
            this.lblUseStorageConn.Location = new System.Drawing.Point(13, 26);
            this.lblUseStorageConn.Name = "lblUseStorageConn";
            this.lblUseStorageConn.Size = new System.Drawing.Size(131, 15);
            this.lblUseStorageConn.TabIndex = 0;
            this.lblUseStorageConn.Text = "Use storage connection";
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(13, 55);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(69, 15);
            this.lblConnection.TabIndex = 2;
            this.lblConnection.Text = "Connection";
            // 
            // gbGeneralOptions
            // 
            this.gbGeneralOptions.Controls.Add(this.chkLogEnabled);
            this.gbGeneralOptions.Controls.Add(this.lblLogEnabled);
            this.gbGeneralOptions.Controls.Add(this.numWritingPeriod);
            this.gbGeneralOptions.Controls.Add(this.lblWritingPeriod);
            this.gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(360, 87);
            this.gbGeneralOptions.TabIndex = 0;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(265, 55);
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
            // numWritingPeriod
            // 
            this.numWritingPeriod.Location = new System.Drawing.Point(197, 22);
            this.numWritingPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numWritingPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWritingPeriod.Name = "numWritingPeriod";
            this.numWritingPeriod.Size = new System.Drawing.Size(150, 23);
            this.numWritingPeriod.TabIndex = 1;
            this.numWritingPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblWritingPeriod
            // 
            this.lblWritingPeriod.AutoSize = true;
            this.lblWritingPeriod.Location = new System.Drawing.Point(13, 26);
            this.lblWritingPeriod.Name = "lblWritingPeriod";
            this.lblWritingPeriod.Size = new System.Drawing.Size(179, 15);
            this.lblWritingPeriod.TabIndex = 0;
            this.lblWritingPeriod.Text = "Period of saving to database, sec";
            // 
            // gbDbOptions
            // 
            this.gbDbOptions.Controls.Add(this.numMaxQueueSize);
            this.gbDbOptions.Controls.Add(this.lblMaxQueueSize);
            this.gbDbOptions.Controls.Add(this.cbConnection);
            this.gbDbOptions.Controls.Add(this.lblConnection);
            this.gbDbOptions.Controls.Add(this.chkUseStorageConn);
            this.gbDbOptions.Controls.Add(this.lblUseStorageConn);
            this.gbDbOptions.Location = new System.Drawing.Point(12, 105);
            this.gbDbOptions.Name = "gbDbOptions";
            this.gbDbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDbOptions.Size = new System.Drawing.Size(360, 116);
            this.gbDbOptions.TabIndex = 1;
            this.gbDbOptions.TabStop = false;
            this.gbDbOptions.Text = "Database Options";
            // 
            // FrmPostgreCAO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 272);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnManageConn);
            this.Controls.Add(this.gbDbOptions);
            this.Controls.Add(this.gbGeneralOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPostgreCAO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Current Archive Options";
            this.Load += new System.EventHandler(this.FrmPostgreHAO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).EndInit();
            this.gbGeneralOptions.ResumeLayout(false);
            this.gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWritingPeriod)).EndInit();
            this.gbDbOptions.ResumeLayout(false);
            this.gbDbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseStorageConn;
        private System.Windows.Forms.ComboBox cbConnection;
        private System.Windows.Forms.Button btnManageConn;
        private System.Windows.Forms.Label lblMaxQueueSize;
        private System.Windows.Forms.NumericUpDown numMaxQueueSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblUseStorageConn;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.GroupBox gbGeneralOptions;
        private System.Windows.Forms.GroupBox gbDbOptions;
        private System.Windows.Forms.CheckBox chkLogEnabled;
        private System.Windows.Forms.Label lblLogEnabled;
        private System.Windows.Forms.NumericUpDown numWritingPeriod;
        private System.Windows.Forms.Label lblWritingPeriod;
    }
}