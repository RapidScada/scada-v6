
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
            ctrlCurrentArchiveOptions = new Server.Forms.Controls.CtrlCurrentArchiveOptions();
            chkUseDefaultConn = new CheckBox();
            cbConnection = new ComboBox();
            btnManageConn = new Button();
            lblMaxQueueSize = new Label();
            numMaxQueueSize = new NumericUpDown();
            btnCancel = new Button();
            btnOK = new Button();
            lblUseDefaultConn = new Label();
            lblConnection = new Label();
            gbDbOptions = new GroupBox();
            numBatchSize = new NumericUpDown();
            lblBatchSize = new Label();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).BeginInit();
            gbDbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).BeginInit();
            SuspendLayout();
            // 
            // ctrlCurrentArchiveOptions
            // 
            ctrlCurrentArchiveOptions.ArchiveOptions = null;
            ctrlCurrentArchiveOptions.Location = new Point(12, 12);
            ctrlCurrentArchiveOptions.Name = "ctrlCurrentArchiveOptions";
            ctrlCurrentArchiveOptions.Size = new Size(360, 116);
            ctrlCurrentArchiveOptions.TabIndex = 0;
            // 
            // chkUseDefaultConn
            // 
            chkUseDefaultConn.AutoSize = true;
            chkUseDefaultConn.Location = new Point(332, 26);
            chkUseDefaultConn.Name = "chkUseDefaultConn";
            chkUseDefaultConn.Size = new Size(15, 14);
            chkUseDefaultConn.TabIndex = 1;
            chkUseDefaultConn.UseVisualStyleBackColor = true;
            chkUseDefaultConn.CheckedChanged += chkUseDefaultConn_CheckedChanged;
            // 
            // cbConnection
            // 
            cbConnection.FormattingEnabled = true;
            cbConnection.Location = new Point(196, 51);
            cbConnection.Name = "cbConnection";
            cbConnection.Size = new Size(151, 23);
            cbConnection.TabIndex = 3;
            // 
            // btnManageConn
            // 
            btnManageConn.Location = new Point(12, 295);
            btnManageConn.Name = "btnManageConn";
            btnManageConn.Size = new Size(140, 23);
            btnManageConn.TabIndex = 2;
            btnManageConn.Text = "Manage Connections";
            btnManageConn.UseVisualStyleBackColor = true;
            btnManageConn.Click += btnManageConn_Click;
            // 
            // lblMaxQueueSize
            // 
            lblMaxQueueSize.AutoSize = true;
            lblMaxQueueSize.Location = new Point(13, 87);
            lblMaxQueueSize.Name = "lblMaxQueueSize";
            lblMaxQueueSize.Size = new Size(120, 15);
            lblMaxQueueSize.TabIndex = 4;
            lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // numMaxQueueSize
            // 
            numMaxQueueSize.Location = new Point(196, 80);
            numMaxQueueSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxQueueSize.Name = "numMaxQueueSize";
            numMaxQueueSize.Size = new Size(151, 23);
            numMaxQueueSize.TabIndex = 5;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 295);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 295);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 3;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // lblUseDefaultConn
            // 
            lblUseDefaultConn.AutoSize = true;
            lblUseDefaultConn.Location = new Point(13, 26);
            lblUseDefaultConn.Name = "lblUseDefaultConn";
            lblUseDefaultConn.Size = new Size(131, 15);
            lblUseDefaultConn.TabIndex = 0;
            lblUseDefaultConn.Text = "Use default connection";
            // 
            // lblConnection
            // 
            lblConnection.AutoSize = true;
            lblConnection.Location = new Point(13, 55);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(69, 15);
            lblConnection.TabIndex = 2;
            lblConnection.Text = "Connection";
            // 
            // gbDbOptions
            // 
            gbDbOptions.Controls.Add(numBatchSize);
            gbDbOptions.Controls.Add(lblBatchSize);
            gbDbOptions.Controls.Add(numMaxQueueSize);
            gbDbOptions.Controls.Add(lblMaxQueueSize);
            gbDbOptions.Controls.Add(cbConnection);
            gbDbOptions.Controls.Add(lblConnection);
            gbDbOptions.Controls.Add(chkUseDefaultConn);
            gbDbOptions.Controls.Add(lblUseDefaultConn);
            gbDbOptions.Location = new Point(12, 134);
            gbDbOptions.Name = "gbDbOptions";
            gbDbOptions.Padding = new Padding(10, 3, 10, 10);
            gbDbOptions.Size = new Size(360, 145);
            gbDbOptions.TabIndex = 1;
            gbDbOptions.TabStop = false;
            gbDbOptions.Text = "Database Options";
            // 
            // numBatchSize
            // 
            numBatchSize.Location = new Point(196, 109);
            numBatchSize.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numBatchSize.Name = "numBatchSize";
            numBatchSize.Size = new Size(151, 23);
            numBatchSize.TabIndex = 7;
            // 
            // lblBatchSize
            // 
            lblBatchSize.AutoSize = true;
            lblBatchSize.Location = new Point(13, 113);
            lblBatchSize.Name = "lblBatchSize";
            lblBatchSize.Size = new Size(118, 15);
            lblBatchSize.TabIndex = 6;
            lblBatchSize.Text = "Items per transaction";
            // 
            // FrmPostgreCAO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 330);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnManageConn);
            Controls.Add(gbDbOptions);
            Controls.Add(ctrlCurrentArchiveOptions);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmPostgreCAO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Current Archive Options";
            Load += FrmPostgreHAO_Load;
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).EndInit();
            gbDbOptions.ResumeLayout(false);
            gbDbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseDefaultConn;
        private System.Windows.Forms.ComboBox cbConnection;
        private System.Windows.Forms.Button btnManageConn;
        private System.Windows.Forms.Label lblMaxQueueSize;
        private System.Windows.Forms.NumericUpDown numMaxQueueSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblUseDefaultConn;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.GroupBox gbDbOptions;
        private Server.Forms.Controls.CtrlCurrentArchiveOptions ctrlCurrentArchiveOptions;
        private NumericUpDown numBatchSize;
        private Label lblBatchSize;
    }
}