namespace Scada.Comm.Drivers.DrvCnlBasic.View.Forms
{
    partial class FrmSerialPortChannelOptions
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbPort = new System.Windows.Forms.GroupBox();
            this.chkRtsEnable = new System.Windows.Forms.CheckBox();
            this.lblRtsEnable = new System.Windows.Forms.Label();
            this.chkDtrEnable = new System.Windows.Forms.CheckBox();
            this.lblDtrEnable = new System.Windows.Forms.Label();
            this.cbStopBits = new System.Windows.Forms.ComboBox();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cbPortName = new System.Windows.Forms.ComboBox();
            this.lblPortName = new System.Windows.Forms.Label();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.cbBehavior = new System.Windows.Forms.ComboBox();
            this.lblBehavior = new System.Windows.Forms.Label();
            this.gbPort.SuspendLayout();
            this.gbMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 324);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 324);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbPort
            // 
            this.gbPort.Controls.Add(this.chkRtsEnable);
            this.gbPort.Controls.Add(this.lblRtsEnable);
            this.gbPort.Controls.Add(this.chkDtrEnable);
            this.gbPort.Controls.Add(this.lblDtrEnable);
            this.gbPort.Controls.Add(this.cbStopBits);
            this.gbPort.Controls.Add(this.lblStopBits);
            this.gbPort.Controls.Add(this.cbParity);
            this.gbPort.Controls.Add(this.lblParity);
            this.gbPort.Controls.Add(this.cbDataBits);
            this.gbPort.Controls.Add(this.lblDataBits);
            this.gbPort.Controls.Add(this.cbBaudRate);
            this.gbPort.Controls.Add(this.lblBaudRate);
            this.gbPort.Controls.Add(this.cbPortName);
            this.gbPort.Controls.Add(this.lblPortName);
            this.gbPort.Location = new System.Drawing.Point(12, 12);
            this.gbPort.Name = "gbPort";
            this.gbPort.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbPort.Size = new System.Drawing.Size(360, 232);
            this.gbPort.TabIndex = 0;
            this.gbPort.TabStop = false;
            this.gbPort.Text = "Serial Port";
            // 
            // chkRtsEnable
            // 
            this.chkRtsEnable.AutoSize = true;
            this.chkRtsEnable.Location = new System.Drawing.Point(265, 200);
            this.chkRtsEnable.Name = "chkRtsEnable";
            this.chkRtsEnable.Size = new System.Drawing.Size(15, 14);
            this.chkRtsEnable.TabIndex = 13;
            this.chkRtsEnable.UseVisualStyleBackColor = true;
            // 
            // lblRtsEnable
            // 
            this.lblRtsEnable.AutoSize = true;
            this.lblRtsEnable.Location = new System.Drawing.Point(13, 200);
            this.lblRtsEnable.Name = "lblRtsEnable";
            this.lblRtsEnable.Size = new System.Drawing.Size(25, 15);
            this.lblRtsEnable.TabIndex = 12;
            this.lblRtsEnable.Text = "RTS";
            // 
            // chkDtrEnable
            // 
            this.chkDtrEnable.AutoSize = true;
            this.chkDtrEnable.Location = new System.Drawing.Point(265, 171);
            this.chkDtrEnable.Name = "chkDtrEnable";
            this.chkDtrEnable.Size = new System.Drawing.Size(15, 14);
            this.chkDtrEnable.TabIndex = 11;
            this.chkDtrEnable.UseVisualStyleBackColor = true;
            // 
            // lblDtrEnable
            // 
            this.lblDtrEnable.AutoSize = true;
            this.lblDtrEnable.Location = new System.Drawing.Point(13, 171);
            this.lblDtrEnable.Name = "lblDtrEnable";
            this.lblDtrEnable.Size = new System.Drawing.Size(27, 15);
            this.lblDtrEnable.TabIndex = 10;
            this.lblDtrEnable.Text = "DTR";
            // 
            // cbStopBits
            // 
            this.cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "1.5"});
            this.cbStopBits.Location = new System.Drawing.Point(197, 138);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(150, 23);
            this.cbStopBits.TabIndex = 9;
            // 
            // lblStopBits
            // 
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(13, 142);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(53, 15);
            this.lblStopBits.TabIndex = 8;
            this.lblStopBits.Text = "Stop bits";
            // 
            // cbParity
            // 
            this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.cbParity.Location = new System.Drawing.Point(197, 109);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(150, 23);
            this.cbParity.TabIndex = 7;
            // 
            // lblParity
            // 
            this.lblParity.AutoSize = true;
            this.lblParity.Location = new System.Drawing.Point(13, 113);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(37, 15);
            this.lblParity.TabIndex = 6;
            this.lblParity.Text = "Parity";
            // 
            // cbDataBits
            // 
            this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cbDataBits.Location = new System.Drawing.Point(197, 80);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(150, 23);
            this.cbDataBits.TabIndex = 5;
            // 
            // lblDataBits
            // 
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(13, 84);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(53, 15);
            this.lblDataBits.TabIndex = 4;
            this.lblDataBits.Text = "Data bits";
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "110",
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.cbBaudRate.Location = new System.Drawing.Point(197, 51);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(150, 23);
            this.cbBaudRate.TabIndex = 3;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(13, 55);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(57, 15);
            this.lblBaudRate.TabIndex = 2;
            this.lblBaudRate.Text = "Baud rate";
            // 
            // cbPortName
            // 
            this.cbPortName.FormattingEnabled = true;
            this.cbPortName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10"});
            this.cbPortName.Location = new System.Drawing.Point(197, 22);
            this.cbPortName.Name = "cbPortName";
            this.cbPortName.Size = new System.Drawing.Size(150, 23);
            this.cbPortName.TabIndex = 1;
            this.cbPortName.Text = "COM1";
            // 
            // lblPortName
            // 
            this.lblPortName.AutoSize = true;
            this.lblPortName.Location = new System.Drawing.Point(13, 26);
            this.lblPortName.Name = "lblPortName";
            this.lblPortName.Size = new System.Drawing.Size(62, 15);
            this.lblPortName.TabIndex = 0;
            this.lblPortName.Text = "Port name";
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.cbBehavior);
            this.gbMode.Controls.Add(this.lblBehavior);
            this.gbMode.Location = new System.Drawing.Point(12, 250);
            this.gbMode.Name = "gbMode";
            this.gbMode.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbMode.Size = new System.Drawing.Size(360, 58);
            this.gbMode.TabIndex = 1;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Operating Mode";
            // 
            // cbBehavior
            // 
            this.cbBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBehavior.FormattingEnabled = true;
            this.cbBehavior.Items.AddRange(new object[] {
            "Master",
            "Slave"});
            this.cbBehavior.Location = new System.Drawing.Point(197, 22);
            this.cbBehavior.Name = "cbBehavior";
            this.cbBehavior.Size = new System.Drawing.Size(150, 23);
            this.cbBehavior.TabIndex = 1;
            // 
            // lblBehavior
            // 
            this.lblBehavior.AutoSize = true;
            this.lblBehavior.Location = new System.Drawing.Point(13, 26);
            this.lblBehavior.Name = "lblBehavior";
            this.lblBehavior.Size = new System.Drawing.Size(53, 15);
            this.lblBehavior.TabIndex = 0;
            this.lblBehavior.Text = "Behavior";
            // 
            // FrmSerialChannelOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 359);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbMode);
            this.Controls.Add(this.gbPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSerialChannelOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Serial Port Options";
            this.Load += new System.EventHandler(this.FrmCommSerialProps_Load);
            this.gbPort.ResumeLayout(false);
            this.gbPort.PerformLayout();
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbPort;
        private System.Windows.Forms.Label lblPortName;
        private System.Windows.Forms.CheckBox chkRtsEnable;
        private System.Windows.Forms.CheckBox chkDtrEnable;
        private System.Windows.Forms.ComboBox cbStopBits;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cbPortName;
        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.ComboBox cbBehavior;
        private System.Windows.Forms.Label lblRtsEnable;
        private System.Windows.Forms.Label lblDtrEnable;
        private System.Windows.Forms.Label lblBehavior;
    }
}