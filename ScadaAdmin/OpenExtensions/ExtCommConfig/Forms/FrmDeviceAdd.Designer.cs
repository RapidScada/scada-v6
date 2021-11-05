namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmDeviceAdd
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
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.cbCommLine = new System.Windows.Forms.ComboBox();
            this.lblCommLine = new System.Windows.Forms.Label();
            this.txtStrAddress = new System.Windows.Forms.TextBox();
            this.lblStrAddress = new System.Windows.Forms.Label();
            this.txtNumAddress = new System.Windows.Forms.TextBox();
            this.lblNumAddress = new System.Windows.Forms.Label();
            this.cbDevType = new System.Windows.Forms.ComboBox();
            this.lblDevType = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.lblDescr = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numDeviceNum = new System.Windows.Forms.NumericUpDown();
            this.lblDeviceNum = new System.Windows.Forms.Label();
            this.gbComm = new System.Windows.Forms.GroupBox();
            this.cbInstance = new System.Windows.Forms.ComboBox();
            this.lblInstance = new System.Windows.Forms.Label();
            this.chkAddToComm = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.gbDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceNum)).BeginInit();
            this.gbComm.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDevice
            // 
            this.gbDevice.Controls.Add(this.txtDescr);
            this.gbDevice.Controls.Add(this.lblDescr);
            this.gbDevice.Controls.Add(this.cbCommLine);
            this.gbDevice.Controls.Add(this.lblCommLine);
            this.gbDevice.Controls.Add(this.txtStrAddress);
            this.gbDevice.Controls.Add(this.lblStrAddress);
            this.gbDevice.Controls.Add(this.txtNumAddress);
            this.gbDevice.Controls.Add(this.lblNumAddress);
            this.gbDevice.Controls.Add(this.cbDevType);
            this.gbDevice.Controls.Add(this.lblDevType);
            this.gbDevice.Controls.Add(this.txtCode);
            this.gbDevice.Controls.Add(this.lblCode);
            this.gbDevice.Controls.Add(this.txtName);
            this.gbDevice.Controls.Add(this.lblName);
            this.gbDevice.Controls.Add(this.numDeviceNum);
            this.gbDevice.Controls.Add(this.lblDeviceNum);
            this.gbDevice.Location = new System.Drawing.Point(12, 12);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(360, 293);
            this.gbDevice.TabIndex = 0;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device";
            // 
            // cbCommLine
            // 
            this.cbCommLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCommLine.FormattingEnabled = true;
            this.cbCommLine.Location = new System.Drawing.Point(13, 213);
            this.cbCommLine.Name = "cbCommLine";
            this.cbCommLine.Size = new System.Drawing.Size(334, 23);
            this.cbCommLine.TabIndex = 13;
            // 
            // lblCommLine
            // 
            this.lblCommLine.AutoSize = true;
            this.lblCommLine.Location = new System.Drawing.Point(10, 195);
            this.lblCommLine.Name = "lblCommLine";
            this.lblCommLine.Size = new System.Drawing.Size(116, 15);
            this.lblCommLine.TabIndex = 12;
            this.lblCommLine.Text = "Communication line";
            // 
            // txtStrAddress
            // 
            this.txtStrAddress.Location = new System.Drawing.Point(119, 169);
            this.txtStrAddress.Name = "txtStrAddress";
            this.txtStrAddress.Size = new System.Drawing.Size(228, 23);
            this.txtStrAddress.TabIndex = 11;
            // 
            // lblStrAddress
            // 
            this.lblStrAddress.AutoSize = true;
            this.lblStrAddress.Location = new System.Drawing.Point(116, 151);
            this.lblStrAddress.Name = "lblStrAddress";
            this.lblStrAddress.Size = new System.Drawing.Size(83, 15);
            this.lblStrAddress.TabIndex = 10;
            this.lblStrAddress.Text = "String Address";
            // 
            // txtNumAddress
            // 
            this.txtNumAddress.Location = new System.Drawing.Point(13, 169);
            this.txtNumAddress.Name = "txtNumAddress";
            this.txtNumAddress.Size = new System.Drawing.Size(100, 23);
            this.txtNumAddress.TabIndex = 9;
            // 
            // lblNumAddress
            // 
            this.lblNumAddress.AutoSize = true;
            this.lblNumAddress.Location = new System.Drawing.Point(10, 151);
            this.lblNumAddress.Name = "lblNumAddress";
            this.lblNumAddress.Size = new System.Drawing.Size(96, 15);
            this.lblNumAddress.TabIndex = 8;
            this.lblNumAddress.Text = "Numeric address";
            // 
            // cbDevType
            // 
            this.cbDevType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevType.FormattingEnabled = true;
            this.cbDevType.Location = new System.Drawing.Point(13, 125);
            this.cbDevType.Name = "cbDevType";
            this.cbDevType.Size = new System.Drawing.Size(334, 23);
            this.cbDevType.TabIndex = 7;
            // 
            // lblDevType
            // 
            this.lblDevType.AutoSize = true;
            this.lblDevType.Location = new System.Drawing.Point(10, 107);
            this.lblDevType.Name = "lblDevType";
            this.lblDevType.Size = new System.Drawing.Size(68, 15);
            this.lblDevType.TabIndex = 6;
            this.lblDevType.Text = "Device type";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(13, 257);
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(334, 23);
            this.txtDescr.TabIndex = 15;
            // 
            // lblDescr
            // 
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(10, 239);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(67, 15);
            this.lblDescr.TabIndex = 14;
            this.lblDescr.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(119, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(228, 23);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(116, 19);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name";
            // 
            // numDeviceNum
            // 
            this.numDeviceNum.Location = new System.Drawing.Point(13, 37);
            this.numDeviceNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDeviceNum.Name = "numDeviceNum";
            this.numDeviceNum.Size = new System.Drawing.Size(100, 23);
            this.numDeviceNum.TabIndex = 1;
            this.numDeviceNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDeviceNum
            // 
            this.lblDeviceNum.AutoSize = true;
            this.lblDeviceNum.Location = new System.Drawing.Point(10, 19);
            this.lblDeviceNum.Name = "lblDeviceNum";
            this.lblDeviceNum.Size = new System.Drawing.Size(51, 15);
            this.lblDeviceNum.TabIndex = 0;
            this.lblDeviceNum.Text = "Number";
            // 
            // gbComm
            // 
            this.gbComm.Controls.Add(this.cbInstance);
            this.gbComm.Controls.Add(this.lblInstance);
            this.gbComm.Controls.Add(this.chkAddToComm);
            this.gbComm.Location = new System.Drawing.Point(12, 311);
            this.gbComm.Name = "gbComm";
            this.gbComm.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbComm.Size = new System.Drawing.Size(360, 98);
            this.gbComm.TabIndex = 1;
            this.gbComm.TabStop = false;
            this.gbComm.Text = "Communicator";
            // 
            // cbInstance
            // 
            this.cbInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInstance.FormattingEnabled = true;
            this.cbInstance.Location = new System.Drawing.Point(13, 62);
            this.cbInstance.Name = "cbInstance";
            this.cbInstance.Size = new System.Drawing.Size(334, 23);
            this.cbInstance.TabIndex = 2;
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(10, 44);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(51, 15);
            this.lblInstance.TabIndex = 1;
            this.lblInstance.Text = "Instance";
            // 
            // chkAddToComm
            // 
            this.chkAddToComm.AutoSize = true;
            this.chkAddToComm.Checked = true;
            this.chkAddToComm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddToComm.Location = new System.Drawing.Point(13, 22);
            this.chkAddToComm.Name = "chkAddToComm";
            this.chkAddToComm.Size = new System.Drawing.Size(183, 19);
            this.chkAddToComm.TabIndex = 0;
            this.chkAddToComm.Text = "Add device to Communicator";
            this.chkAddToComm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 425);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(10, 63);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(35, 15);
            this.lblCode.TabIndex = 4;
            this.lblCode.Text = "Code";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(13, 81);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(334, 23);
            this.txtCode.TabIndex = 5;
            // 
            // FrmDeviceAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 460);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbComm);
            this.Controls.Add(this.gbDevice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeviceAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Device";
            this.Load += new System.EventHandler(this.FrmDeviceAdd_Load);
            this.gbDevice.ResumeLayout(false);
            this.gbDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDeviceNum)).EndInit();
            this.gbComm.ResumeLayout(false);
            this.gbComm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDevice;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numDeviceNum;
        private System.Windows.Forms.Label lblDeviceNum;
        private System.Windows.Forms.ComboBox cbDevType;
        private System.Windows.Forms.Label lblDevType;
        private System.Windows.Forms.Label lblNumAddress;
        private System.Windows.Forms.ComboBox cbCommLine;
        private System.Windows.Forms.Label lblCommLine;
        private System.Windows.Forms.TextBox txtStrAddress;
        private System.Windows.Forms.Label lblStrAddress;
        private System.Windows.Forms.GroupBox gbComm;
        private System.Windows.Forms.ComboBox cbInstance;
        private System.Windows.Forms.Label lblInstance;
        private System.Windows.Forms.CheckBox chkAddToComm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtNumAddress;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
    }
}