
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
            this.lblConnection = new System.Windows.Forms.Label();
            this.cbConnection = new System.Windows.Forms.ComboBox();
            this.lblClientLogEnabled = new System.Windows.Forms.Label();
            this.chkClientLogEnabled = new System.Windows.Forms.CheckBox();
            this.lblMaxQueueSize = new System.Windows.Forms.Label();
            this.numMaxQueueSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxCurDataAge = new System.Windows.Forms.Label();
            this.numMaxCurDataAge = new System.Windows.Forms.NumericUpDown();
            this.lblDataLifetime = new System.Windows.Forms.Label();
            this.numDataLifetime = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceFilter = new System.Windows.Forms.Label();
            this.txtDeviceFilter = new System.Windows.Forms.TextBox();
            this.btnSelectDevices = new System.Windows.Forms.Button();
            this.btnManageConn = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblUseDefaultConn = new System.Windows.Forms.Label();
            this.chkUseDefaultConn = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCurDataAge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).BeginInit();
            this.SuspendLayout();
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(9, 45);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(69, 15);
            this.lblConnection.TabIndex = 2;
            this.lblConnection.Text = "Connection";
            // 
            // cbConnection
            // 
            this.cbConnection.FormattingEnabled = true;
            this.cbConnection.Location = new System.Drawing.Point(222, 41);
            this.cbConnection.Name = "cbConnection";
            this.cbConnection.Size = new System.Drawing.Size(150, 23);
            this.cbConnection.TabIndex = 3;
            // 
            // lblClientLogEnabled
            // 
            this.lblClientLogEnabled.AutoSize = true;
            this.lblClientLogEnabled.Location = new System.Drawing.Point(9, 161);
            this.lblClientLogEnabled.Name = "lblClientLogEnabled";
            this.lblClientLogEnabled.Size = new System.Drawing.Size(103, 15);
            this.lblClientLogEnabled.TabIndex = 10;
            this.lblClientLogEnabled.Text = "Client log enabled";
            // 
            // chkClientLogEnabled
            // 
            this.chkClientLogEnabled.AutoSize = true;
            this.chkClientLogEnabled.Location = new System.Drawing.Point(357, 161);
            this.chkClientLogEnabled.Name = "chkClientLogEnabled";
            this.chkClientLogEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkClientLogEnabled.TabIndex = 11;
            this.chkClientLogEnabled.UseVisualStyleBackColor = true;
            // 
            // lblMaxQueueSize
            // 
            this.lblMaxQueueSize.AutoSize = true;
            this.lblMaxQueueSize.Location = new System.Drawing.Point(9, 74);
            this.lblMaxQueueSize.Name = "lblMaxQueueSize";
            this.lblMaxQueueSize.Size = new System.Drawing.Size(120, 15);
            this.lblMaxQueueSize.TabIndex = 4;
            this.lblMaxQueueSize.Text = "Maximum queue size";
            // 
            // numMaxQueueSize
            // 
            this.numMaxQueueSize.Location = new System.Drawing.Point(222, 70);
            this.numMaxQueueSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMaxQueueSize.Name = "numMaxQueueSize";
            this.numMaxQueueSize.Size = new System.Drawing.Size(150, 23);
            this.numMaxQueueSize.TabIndex = 5;
            // 
            // lblMaxCurDataAge
            // 
            this.lblMaxCurDataAge.AutoSize = true;
            this.lblMaxCurDataAge.Location = new System.Drawing.Point(9, 103);
            this.lblMaxCurDataAge.Name = "lblMaxCurDataAge";
            this.lblMaxCurDataAge.Size = new System.Drawing.Size(198, 15);
            this.lblMaxCurDataAge.TabIndex = 6;
            this.lblMaxCurDataAge.Text = "Current data becomes historical, sec";
            // 
            // numMaxCurDataAge
            // 
            this.numMaxCurDataAge.Location = new System.Drawing.Point(222, 99);
            this.numMaxCurDataAge.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numMaxCurDataAge.Name = "numMaxCurDataAge";
            this.numMaxCurDataAge.Size = new System.Drawing.Size(150, 23);
            this.numMaxCurDataAge.TabIndex = 7;
            // 
            // lblDataLifetime
            // 
            this.lblDataLifetime.AutoSize = true;
            this.lblDataLifetime.Location = new System.Drawing.Point(9, 132);
            this.lblDataLifetime.Name = "lblDataLifetime";
            this.lblDataLifetime.Size = new System.Drawing.Size(146, 15);
            this.lblDataLifetime.TabIndex = 8;
            this.lblDataLifetime.Text = "Data lifetime in queue, sec";
            // 
            // numDataLifetime
            // 
            this.numDataLifetime.Location = new System.Drawing.Point(222, 128);
            this.numDataLifetime.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDataLifetime.Name = "numDataLifetime";
            this.numDataLifetime.Size = new System.Drawing.Size(150, 23);
            this.numDataLifetime.TabIndex = 9;
            // 
            // lblDeviceFilter
            // 
            this.lblDeviceFilter.AutoSize = true;
            this.lblDeviceFilter.Location = new System.Drawing.Point(9, 190);
            this.lblDeviceFilter.Name = "lblDeviceFilter";
            this.lblDeviceFilter.Size = new System.Drawing.Size(69, 15);
            this.lblDeviceFilter.TabIndex = 12;
            this.lblDeviceFilter.Text = "Device filter";
            // 
            // txtDeviceFilter
            // 
            this.txtDeviceFilter.Location = new System.Drawing.Point(12, 208);
            this.txtDeviceFilter.Name = "txtDeviceFilter";
            this.txtDeviceFilter.Size = new System.Drawing.Size(279, 23);
            this.txtDeviceFilter.TabIndex = 13;
            // 
            // btnSelectDevices
            // 
            this.btnSelectDevices.Location = new System.Drawing.Point(297, 208);
            this.btnSelectDevices.Name = "btnSelectDevices";
            this.btnSelectDevices.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDevices.TabIndex = 14;
            this.btnSelectDevices.Text = "Select...";
            this.btnSelectDevices.UseVisualStyleBackColor = true;
            this.btnSelectDevices.Click += new System.EventHandler(this.btnSelectDevices_Click);
            // 
            // btnManageConn
            // 
            this.btnManageConn.Location = new System.Drawing.Point(12, 247);
            this.btnManageConn.Name = "btnManageConn";
            this.btnManageConn.Size = new System.Drawing.Size(140, 23);
            this.btnManageConn.TabIndex = 15;
            this.btnManageConn.Text = "Manage Connections";
            this.btnManageConn.UseVisualStyleBackColor = true;
            this.btnManageConn.Click += new System.EventHandler(this.btnManageConn_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 247);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblUseDefaultConn
            // 
            this.lblUseDefaultConn.AutoSize = true;
            this.lblUseDefaultConn.Location = new System.Drawing.Point(9, 16);
            this.lblUseDefaultConn.Name = "lblUseDefaultConn";
            this.lblUseDefaultConn.Size = new System.Drawing.Size(129, 15);
            this.lblUseDefaultConn.TabIndex = 0;
            this.lblUseDefaultConn.Text = "Use default connection";
            // 
            // chkUseDefaultConn
            // 
            this.chkUseDefaultConn.AutoSize = true;
            this.chkUseDefaultConn.Location = new System.Drawing.Point(357, 16);
            this.chkUseDefaultConn.Name = "chkUseDefaultConn";
            this.chkUseDefaultConn.Size = new System.Drawing.Size(15, 14);
            this.chkUseDefaultConn.TabIndex = 1;
            this.chkUseDefaultConn.UseVisualStyleBackColor = true;
            this.chkUseDefaultConn.CheckedChanged += new System.EventHandler(this.chkUseDefaultConn_CheckedChanged);
            // 
            // FrmScadaServerDSO
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 282);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnManageConn);
            this.Controls.Add(this.btnSelectDevices);
            this.Controls.Add(this.txtDeviceFilter);
            this.Controls.Add(this.lblDeviceFilter);
            this.Controls.Add(this.chkClientLogEnabled);
            this.Controls.Add(this.lblClientLogEnabled);
            this.Controls.Add(this.numDataLifetime);
            this.Controls.Add(this.lblDataLifetime);
            this.Controls.Add(this.numMaxCurDataAge);
            this.Controls.Add(this.lblMaxCurDataAge);
            this.Controls.Add(this.numMaxQueueSize);
            this.Controls.Add(this.lblMaxQueueSize);
            this.Controls.Add(this.cbConnection);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.chkUseDefaultConn);
            this.Controls.Add(this.lblUseDefaultConn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmScadaServerDSO";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Source Options";
            this.Load += new System.EventHandler(this.FrmScadaServerDSO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxQueueSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCurDataAge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLifetime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.ComboBox cbConnection;
        private System.Windows.Forms.Label lblClientLogEnabled;
        private System.Windows.Forms.CheckBox chkClientLogEnabled;
        private System.Windows.Forms.Label lblMaxQueueSize;
        private System.Windows.Forms.NumericUpDown numMaxQueueSize;
        private System.Windows.Forms.Label lblMaxCurDataAge;
        private System.Windows.Forms.NumericUpDown numMaxCurDataAge;
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
    }
}