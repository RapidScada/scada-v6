namespace Scada.Server.Modules.ModArcPostgreSql.View.Controls
{
    partial class CtrlDatabaseOptions
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
            gbDbOptions = new GroupBox();
            numBatchSize = new NumericUpDown();
            lblBatchSize = new Label();
            numMaxQueueSize = new NumericUpDown();
            lblMaxQueueSize = new Label();
            cbPartitionSize = new ComboBox();
            lblPartitionSize = new Label();
            cbConnection = new ComboBox();
            lblConnection = new Label();
            chkUseDefaultConn = new CheckBox();
            lblUseDefaultConn = new Label();
            gbDbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).BeginInit();
            SuspendLayout();
            // 
            // gbDbOptions
            // 
            gbDbOptions.Controls.Add(numBatchSize);
            gbDbOptions.Controls.Add(lblBatchSize);
            gbDbOptions.Controls.Add(numMaxQueueSize);
            gbDbOptions.Controls.Add(lblMaxQueueSize);
            gbDbOptions.Controls.Add(cbPartitionSize);
            gbDbOptions.Controls.Add(lblPartitionSize);
            gbDbOptions.Controls.Add(cbConnection);
            gbDbOptions.Controls.Add(lblConnection);
            gbDbOptions.Controls.Add(chkUseDefaultConn);
            gbDbOptions.Controls.Add(lblUseDefaultConn);
            gbDbOptions.Location = new Point(0, 0);
            gbDbOptions.Name = "gbDbOptions";
            gbDbOptions.Padding = new Padding(10, 3, 10, 10);
            gbDbOptions.Size = new Size(360, 175);
            gbDbOptions.TabIndex = 0;
            gbDbOptions.TabStop = false;
            gbDbOptions.Text = "Database Options";
            // 
            // numBatchSize
            // 
            numBatchSize.Location = new Point(196, 139);
            numBatchSize.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numBatchSize.Name = "numBatchSize";
            numBatchSize.Size = new Size(151, 23);
            numBatchSize.TabIndex = 9;
            // 
            // lblBatchSize
            // 
            lblBatchSize.AutoSize = true;
            lblBatchSize.Location = new Point(13, 143);
            lblBatchSize.Name = "lblBatchSize";
            lblBatchSize.Size = new Size(118, 15);
            lblBatchSize.TabIndex = 8;
            lblBatchSize.Text = "Items per transaction";
            // 
            // numMaxQueueSize
            // 
            numMaxQueueSize.Location = new Point(196, 110);
            numMaxQueueSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxQueueSize.Name = "numMaxQueueSize";
            numMaxQueueSize.Size = new Size(151, 23);
            numMaxQueueSize.TabIndex = 7;
            // 
            // lblMaxQueueSize
            // 
            lblMaxQueueSize.AutoSize = true;
            lblMaxQueueSize.Location = new Point(13, 114);
            lblMaxQueueSize.Name = "lblMaxQueueSize";
            lblMaxQueueSize.Size = new Size(120, 15);
            lblMaxQueueSize.TabIndex = 6;
            lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // cbPartitionSize
            // 
            cbPartitionSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPartitionSize.FormattingEnabled = true;
            cbPartitionSize.Items.AddRange(new object[] { "One day", "One month", "One year" });
            cbPartitionSize.Location = new Point(196, 81);
            cbPartitionSize.Name = "cbPartitionSize";
            cbPartitionSize.Size = new Size(151, 23);
            cbPartitionSize.TabIndex = 5;
            // 
            // lblPartitionSize
            // 
            lblPartitionSize.AutoSize = true;
            lblPartitionSize.Location = new Point(13, 85);
            lblPartitionSize.Name = "lblPartitionSize";
            lblPartitionSize.Size = new Size(74, 15);
            lblPartitionSize.TabIndex = 4;
            lblPartitionSize.Text = "Partition size";
            // 
            // cbConnection
            // 
            cbConnection.FormattingEnabled = true;
            cbConnection.Location = new Point(196, 52);
            cbConnection.Name = "cbConnection";
            cbConnection.Size = new Size(151, 23);
            cbConnection.TabIndex = 3;
            // 
            // lblConnection
            // 
            lblConnection.AutoSize = true;
            lblConnection.Location = new Point(13, 56);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(69, 15);
            lblConnection.TabIndex = 2;
            lblConnection.Text = "Connection";
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
            // lblUseDefaultConn
            // 
            lblUseDefaultConn.AutoSize = true;
            lblUseDefaultConn.Location = new Point(13, 26);
            lblUseDefaultConn.Name = "lblUseDefaultConn";
            lblUseDefaultConn.Size = new Size(129, 15);
            lblUseDefaultConn.TabIndex = 0;
            lblUseDefaultConn.Text = "Use default connection";
            // 
            // CtrlDatabaseOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbDbOptions);
            Name = "CtrlDatabaseOptions";
            Size = new Size(360, 175);
            gbDbOptions.ResumeLayout(false);
            gbDbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBatchSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbDbOptions;
        private NumericUpDown numBatchSize;
        private Label lblBatchSize;
        private ComboBox cbPartitionSize;
        private Label lblPartitionSize;
        private NumericUpDown numMaxQueueSize;
        private Label lblMaxQueueSize;
        private ComboBox cbConnection;
        private Label lblConnection;
        private CheckBox chkUseDefaultConn;
        private Label lblUseDefaultConn;
    }
}
