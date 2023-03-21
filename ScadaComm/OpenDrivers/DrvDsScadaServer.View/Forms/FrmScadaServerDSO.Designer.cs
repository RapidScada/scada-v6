
namespace Scada.Comm.Drivers.DrvDsScadaServer.View.Forms
{
    partial class FrmScadaServerDSO
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
            lblConnection = new Label();
            cbConnection = new ComboBox();
            lblClientLogEnabled = new Label();
            chkClientLogEnabled = new CheckBox();
            lblMaxQueueSize = new Label();
            numMaxQueueSize = new NumericUpDown();
            lblDataLifetime = new Label();
            numDataLifetime = new NumericUpDown();
            lblDeviceFilter = new Label();
            txtDeviceFilter = new TextBox();
            btnSelectDevices = new Button();
            btnManageConn = new Button();
            btnCancel = new Button();
            btnOK = new Button();
            lblUseDefaultConn = new Label();
            chkUseDefaultConn = new CheckBox();
            chkReadConfigDb = new CheckBox();
            lblReadConfigDb = new Label();
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDataLifetime).BeginInit();
            SuspendLayout();
            // 
            // lblConnection
            // 
            lblConnection.AutoSize = true;
            lblConnection.Location = new Point(9, 74);
            lblConnection.Name = "lblConnection";
            lblConnection.Size = new Size(69, 15);
            lblConnection.TabIndex = 4;
            lblConnection.Text = "Connection";
            // 
            // cbConnection
            // 
            cbConnection.FormattingEnabled = true;
            cbConnection.Location = new Point(222, 70);
            cbConnection.Name = "cbConnection";
            cbConnection.Size = new Size(150, 23);
            cbConnection.TabIndex = 5;
            // 
            // lblClientLogEnabled
            // 
            lblClientLogEnabled.AutoSize = true;
            lblClientLogEnabled.Location = new Point(9, 161);
            lblClientLogEnabled.Name = "lblClientLogEnabled";
            lblClientLogEnabled.Size = new Size(103, 15);
            lblClientLogEnabled.TabIndex = 10;
            lblClientLogEnabled.Text = "Client log enabled";
            // 
            // chkClientLogEnabled
            // 
            chkClientLogEnabled.AutoSize = true;
            chkClientLogEnabled.Location = new Point(357, 161);
            chkClientLogEnabled.Name = "chkClientLogEnabled";
            chkClientLogEnabled.Size = new Size(15, 14);
            chkClientLogEnabled.TabIndex = 11;
            chkClientLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblMaxQueueSize
            // 
            lblMaxQueueSize.AutoSize = true;
            lblMaxQueueSize.Location = new Point(9, 103);
            lblMaxQueueSize.Name = "lblMaxQueueSize";
            lblMaxQueueSize.Size = new Size(120, 15);
            lblMaxQueueSize.TabIndex = 6;
            lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // numMaxQueueSize
            // 
            numMaxQueueSize.Location = new Point(222, 99);
            numMaxQueueSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxQueueSize.Name = "numMaxQueueSize";
            numMaxQueueSize.Size = new Size(150, 23);
            numMaxQueueSize.TabIndex = 7;
            // 
            // lblDataLifetime
            // 
            lblDataLifetime.AutoSize = true;
            lblDataLifetime.Location = new Point(9, 132);
            lblDataLifetime.Name = "lblDataLifetime";
            lblDataLifetime.Size = new Size(146, 15);
            lblDataLifetime.TabIndex = 8;
            lblDataLifetime.Text = "Data lifetime in queue, sec";
            // 
            // numDataLifetime
            // 
            numDataLifetime.Location = new Point(222, 128);
            numDataLifetime.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numDataLifetime.Name = "numDataLifetime";
            numDataLifetime.Size = new Size(150, 23);
            numDataLifetime.TabIndex = 9;
            // 
            // lblDeviceFilter
            // 
            lblDeviceFilter.AutoSize = true;
            lblDeviceFilter.Location = new Point(9, 190);
            lblDeviceFilter.Name = "lblDeviceFilter";
            lblDeviceFilter.Size = new Size(69, 15);
            lblDeviceFilter.TabIndex = 12;
            lblDeviceFilter.Text = "Device filter";
            // 
            // txtDeviceFilter
            // 
            txtDeviceFilter.Location = new Point(12, 208);
            txtDeviceFilter.Name = "txtDeviceFilter";
            txtDeviceFilter.Size = new Size(279, 23);
            txtDeviceFilter.TabIndex = 13;
            // 
            // btnSelectDevices
            // 
            btnSelectDevices.Location = new Point(297, 208);
            btnSelectDevices.Name = "btnSelectDevices";
            btnSelectDevices.Size = new Size(75, 23);
            btnSelectDevices.TabIndex = 14;
            btnSelectDevices.Text = "Select...";
            btnSelectDevices.UseVisualStyleBackColor = true;
            btnSelectDevices.Click += btnSelectDevices_Click;
            // 
            // btnManageConn
            // 
            btnManageConn.Location = new Point(12, 247);
            btnManageConn.Name = "btnManageConn";
            btnManageConn.Size = new Size(140, 23);
            btnManageConn.TabIndex = 15;
            btnManageConn.Text = "Manage Connections";
            btnManageConn.UseVisualStyleBackColor = true;
            btnManageConn.Click += btnManageConn_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(297, 247);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(216, 247);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 16;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // lblUseDefaultConn
            // 
            lblUseDefaultConn.AutoSize = true;
            lblUseDefaultConn.Location = new Point(9, 45);
            lblUseDefaultConn.Name = "lblUseDefaultConn";
            lblUseDefaultConn.Size = new Size(129, 15);
            lblUseDefaultConn.TabIndex = 2;
            lblUseDefaultConn.Text = "Use default connection";
            // 
            // chkUseDefaultConn
            // 
            chkUseDefaultConn.AutoSize = true;
            chkUseDefaultConn.Location = new Point(357, 45);
            chkUseDefaultConn.Name = "chkUseDefaultConn";
            chkUseDefaultConn.Size = new Size(15, 14);
            chkUseDefaultConn.TabIndex = 3;
            chkUseDefaultConn.UseVisualStyleBackColor = true;
            chkUseDefaultConn.CheckedChanged += chkUseDefaultConn_CheckedChanged;
            // 
            // chkReadConfigDb
            // 
            chkReadConfigDb.AutoSize = true;
            chkReadConfigDb.Location = new Point(357, 16);
            chkReadConfigDb.Name = "chkReadConfigDb";
            chkReadConfigDb.Size = new Size(15, 14);
            chkReadConfigDb.TabIndex = 1;
            chkReadConfigDb.UseVisualStyleBackColor = true;
            // 
            // lblReadConfigDb
            // 
            lblReadConfigDb.AutoSize = true;
            lblReadConfigDb.Location = new Point(9, 16);
            lblReadConfigDb.Name = "lblReadConfigDb";
            lblReadConfigDb.Size = new Size(158, 15);
            lblReadConfigDb.TabIndex = 0;
            lblReadConfigDb.Text = "Read configuration database";
            // 
            // FrmScadaServerDSO
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 282);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(btnManageConn);
            Controls.Add(btnSelectDevices);
            Controls.Add(txtDeviceFilter);
            Controls.Add(lblDeviceFilter);
            Controls.Add(chkClientLogEnabled);
            Controls.Add(lblClientLogEnabled);
            Controls.Add(numDataLifetime);
            Controls.Add(lblDataLifetime);
            Controls.Add(numMaxQueueSize);
            Controls.Add(lblMaxQueueSize);
            Controls.Add(cbConnection);
            Controls.Add(lblConnection);
            Controls.Add(chkUseDefaultConn);
            Controls.Add(lblUseDefaultConn);
            Controls.Add(chkReadConfigDb);
            Controls.Add(lblReadConfigDb);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmScadaServerDSO";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Data Source Options";
            Load += FrmScadaServerDSO_Load;
            ((System.ComponentModel.ISupportInitialize)numMaxQueueSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDataLifetime).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.ComboBox cbConnection;
        private System.Windows.Forms.Label lblClientLogEnabled;
        private System.Windows.Forms.CheckBox chkClientLogEnabled;
        private System.Windows.Forms.Label lblMaxQueueSize;
        private System.Windows.Forms.NumericUpDown numMaxQueueSize;
        private System.Windows.Forms.Label lblDataLifetime;
        private System.Windows.Forms.NumericUpDown numDataLifetime;
        private System.Windows.Forms.Label lblDeviceFilter;
        private System.Windows.Forms.TextBox txtDeviceFilter;
        private System.Windows.Forms.Button btnSelectDevices;
        private System.Windows.Forms.Button btnManageConn;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblUseDefaultConn;
        private System.Windows.Forms.CheckBox chkUseDefaultConn;
        private CheckBox chkReadConfigDb;
        private Label lblReadConfigDb;
    }
}