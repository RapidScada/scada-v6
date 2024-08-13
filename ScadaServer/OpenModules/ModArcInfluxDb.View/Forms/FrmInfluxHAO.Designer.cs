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
            tabControl = new TabControl();
            pageGeneral = new TabPage();
            pageDatabase = new TabPage();
            pnlBottom = new Panel();
            ((System.ComponentModel.ISupportInitialize)numFlushInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).BeginInit();
            tabControl.SuspendLayout();
            pageGeneral.SuspendLayout();
            pageDatabase.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // ctrlHistoricalArchiveOptions
            // 
            ctrlHistoricalArchiveOptions.ArchiveOptions = null;
            ctrlHistoricalArchiveOptions.Location = new Point(8, 8);
            ctrlHistoricalArchiveOptions.Name = "ctrlHistoricalArchiveOptions";
            ctrlHistoricalArchiveOptions.Size = new Size(360, 309);
            ctrlHistoricalArchiveOptions.TabIndex = 0;
            // 
            // txtFlushIntervalUnit
            // 
            txtFlushIntervalUnit.Location = new Point(298, 66);
            txtFlushIntervalUnit.Name = "txtFlushIntervalUnit";
            txtFlushIntervalUnit.ReadOnly = true;
            txtFlushIntervalUnit.Size = new Size(70, 23);
            txtFlushIntervalUnit.TabIndex = 6;
            txtFlushIntervalUnit.Text = "ms";
            // 
            // numFlushInterval
            // 
            numFlushInterval.Location = new Point(217, 66);
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
            lblFlushInterval.Location = new Point(8, 70);
            lblFlushInterval.Name = "lblFlushInterval";
            lblFlushInterval.Size = new Size(77, 15);
            lblFlushInterval.TabIndex = 4;
            lblFlushInterval.Text = "Flush interval";
            // 
            // numBatchSize
            // 
            numBatchSize.Location = new Point(217, 37);
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
            lblBatchSize.Location = new Point(8, 41);
            lblBatchSize.Name = "lblBatchSize";
            lblBatchSize.Size = new Size(59, 15);
            lblBatchSize.TabIndex = 2;
            lblBatchSize.Text = "Batch size";
            // 
            // cbConnection
            // 
            cbConnection.FormattingEnabled = true;
            cbConnection.Location = new Point(217, 8);
            cbConnection.Name = "cbConnection";
            cbConnection.Size = new Size(151, 23);
            cbConnection.TabIndex = 1;
            // 
            // lblConnection
            // 
            lblConnection.AutoSize = true;
            lblConnection.Location = new Point(8, 12);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(69, 15);
            lblConnection.TabIndex = 0;
            lblConnection.Text = "Connection";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 6);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 6);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnManageConn
            // 
            btnManageConn.Location = new Point(12, 6);
            btnManageConn.Name = "btnManageConn";
            btnManageConn.Size = new Size(140, 23);
            btnManageConn.TabIndex = 0;
            btnManageConn.Text = "Manage Connections";
            btnManageConn.UseVisualStyleBackColor = true;
            btnManageConn.Click += btnManageConn_Click;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(pageGeneral);
            tabControl.Controls.Add(pageDatabase);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(384, 353);
            tabControl.TabIndex = 0;
            // 
            // pageGeneral
            // 
            pageGeneral.Controls.Add(ctrlHistoricalArchiveOptions);
            pageGeneral.Location = new Point(4, 24);
            pageGeneral.Name = "pageGeneral";
            pageGeneral.Padding = new Padding(5);
            pageGeneral.Size = new Size(376, 325);
            pageGeneral.TabIndex = 0;
            pageGeneral.Text = "General";
            pageGeneral.UseVisualStyleBackColor = true;
            // 
            // pageDatabase
            // 
            pageDatabase.Controls.Add(txtFlushIntervalUnit);
            pageDatabase.Controls.Add(numFlushInterval);
            pageDatabase.Controls.Add(lblFlushInterval);
            pageDatabase.Controls.Add(numBatchSize);
            pageDatabase.Controls.Add(lblBatchSize);
            pageDatabase.Controls.Add(cbConnection);
            pageDatabase.Controls.Add(lblConnection);
            pageDatabase.Location = new Point(4, 24);
            pageDatabase.Name = "pageDatabase";
            pageDatabase.Padding = new Padding(5);
            pageDatabase.Size = new Size(376, 296);
            pageDatabase.TabIndex = 1;
            pageDatabase.Text = "Database";
            pageDatabase.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnManageConn);
            pnlBottom.Controls.Add(btnOK);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 353);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(384, 41);
            pnlBottom.TabIndex = 1;
            // 
            // FrmInfluxHAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 394);
            Controls.Add(tabControl);
            Controls.Add(pnlBottom);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmInfluxHAO";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Historical Archive Options";
            Load += FrmInfluxHAO_Load;
            ((System.ComponentModel.ISupportInitialize)numFlushInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).EndInit();
            tabControl.ResumeLayout(false);
            pageGeneral.ResumeLayout(false);
            pageDatabase.ResumeLayout(false);
            pageDatabase.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Server.Forms.Controls.CtrlHistoricalArchiveOptions ctrlHistoricalArchiveOptions;
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
        private TabControl tabControl;
        private TabPage pageGeneral;
        private TabPage pageDatabase;
        private Panel pnlBottom;
    }
}