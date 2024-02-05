namespace Scada.Server.Modules.ModArcInfluxDb.View.Forms
{
    partial class FrmInfluxHAO
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
            ctrlHistoricalArchiveOptions = new Server.Forms.Controls.CtrlHistoricalArchiveOptions();
            gbDbOptions = new GroupBox();
            txtFlushIntervalUnit = new TextBox();
            numFlushInterval = new NumericUpDown();
            lblFlushInterval = new Label();
            numBatchSize = new NumericUpDown();
            lblBatchSize = new Label();
            cbConnection = new ComboBox();
            lblConnection = new Label();
            btnCancel = new Button();
            btnOK = new Button();
            btnManageConn = new Button();
            gbDbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFlushInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).BeginInit();
            SuspendLayout();
            // 
            // ctrlHistoricalArchiveOptions
            // 
            ctrlHistoricalArchiveOptions.ArchiveOptions = null;
            ctrlHistoricalArchiveOptions.Location = new Point(12, 12);
            ctrlHistoricalArchiveOptions.Name = "ctrlHistoricalArchiveOptions";
            ctrlHistoricalArchiveOptions.Size = new Size(360, 290);
            ctrlHistoricalArchiveOptions.TabIndex = 0;
            // 
            // gbDbOptions
            // 
            gbDbOptions.Controls.Add(txtFlushIntervalUnit);
            gbDbOptions.Controls.Add(numFlushInterval);
            gbDbOptions.Controls.Add(lblFlushInterval);
            gbDbOptions.Controls.Add(numBatchSize);
            gbDbOptions.Controls.Add(lblBatchSize);
            gbDbOptions.Controls.Add(cbConnection);
            gbDbOptions.Controls.Add(lblConnection);
            gbDbOptions.Location = new Point(12, 308);
            gbDbOptions.Name = "gbDbOptions";
            gbDbOptions.Padding = new Padding(10, 3, 10, 10);
            gbDbOptions.Size = new Size(360, 116);
            gbDbOptions.TabIndex = 1;
            gbDbOptions.TabStop = false;
            gbDbOptions.Text = "Database Options";
            // 
            // txtFlushIntervalUnit
            // 
            txtFlushIntervalUnit.Location = new Point(277, 80);
            txtFlushIntervalUnit.Name = "txtFlushIntervalUnit";
            txtFlushIntervalUnit.ReadOnly = true;
            txtFlushIntervalUnit.Size = new Size(70, 23);
            txtFlushIntervalUnit.TabIndex = 6;
            txtFlushIntervalUnit.Text = "ms";
            // 
            // numFlushInterval
            // 
            numFlushInterval.Location = new Point(196, 80);
            numFlushInterval.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
            numFlushInterval.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numFlushInterval.Name = "numFlushInterval";
            numFlushInterval.Size = new Size(75, 23);
            numFlushInterval.TabIndex = 5;
            numFlushInterval.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // lblFlushInterval
            // 
            lblFlushInterval.AutoSize = true;
            lblFlushInterval.Location = new Point(13, 84);
            lblFlushInterval.Name = "lblFlushInterval";
            lblFlushInterval.Size = new Size(77, 15);
            lblFlushInterval.TabIndex = 4;
            lblFlushInterval.Text = "Flush interval";
            // 
            // numBatchSize
            // 
            numBatchSize.Location = new Point(196, 51);
            numBatchSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numBatchSize.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numBatchSize.Name = "numBatchSize";
            numBatchSize.Size = new Size(151, 23);
            numBatchSize.TabIndex = 3;
            numBatchSize.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // lblBatchSize
            // 
            lblBatchSize.AutoSize = true;
            lblBatchSize.Location = new Point(13, 55);
            lblBatchSize.Name = "lblBatchSize";
            lblBatchSize.Size = new Size(59, 15);
            lblBatchSize.TabIndex = 2;
            lblBatchSize.Text = "Batch size";
            // 
            // cbConnection
            // 
            cbConnection.FormattingEnabled = true;
            cbConnection.Location = new Point(196, 22);
            cbConnection.Name = "cbConnection";
            cbConnection.Size = new Size(151, 23);
            cbConnection.TabIndex = 1;
            // 
            // lblConnection
            // 
            lblConnection.AutoSize = true;
            lblConnection.Location = new Point(13, 26);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(69, 15);
            lblConnection.TabIndex = 0;
            lblConnection.Text = "Connection";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 440);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 440);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnManageConn
            // 
            btnManageConn.Location = new Point(12, 440);
            btnManageConn.Name = "btnManageConn";
            btnManageConn.Size = new Size(140, 23);
            btnManageConn.TabIndex = 2;
            btnManageConn.Text = "Manage Connections";
            btnManageConn.UseVisualStyleBackColor = true;
            btnManageConn.Click += btnManageConn_Click;
            // 
            // FrmInfluxHAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 475);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnManageConn);
            Controls.Add(gbDbOptions);
            Controls.Add(ctrlHistoricalArchiveOptions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmInfluxHAO";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historical Archive Options";
            Load += FrmInfluxHAO_Load;
            gbDbOptions.ResumeLayout(false);
            gbDbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numFlushInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Server.Forms.Controls.CtrlHistoricalArchiveOptions ctrlHistoricalArchiveOptions;
        private GroupBox gbDbOptions;
        private ComboBox cbConnection;
        private Label lblConnection;
        private Button btnCancel;
        private Button btnOK;
        private Button btnManageConn;
        private TextBox txtFlushIntervalUnit;
        private NumericUpDown numFlushInterval;
        private Label lblFlushInterval;
        private NumericUpDown numBatchSize;
        private Label lblBatchSize;
    }
}