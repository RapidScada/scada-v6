namespace Scada.Admin.App.Forms.Tables
{
    partial class FrmOutCnl
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
            this.lblOutCnlNum = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.cbCmdType = new System.Windows.Forms.ComboBox();
            this.lblCmdType = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblObj = new System.Windows.Forms.Label();
            this.txtObjNum = new System.Windows.Forms.TextBox();
            this.cbObj = new System.Windows.Forms.ComboBox();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.txtDeviceNum = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.lblFormula = new System.Windows.Forms.Label();
            this.chkFormulaEnabled = new System.Windows.Forms.CheckBox();
            this.txtFormula = new System.Windows.Forms.TextBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.chkEventEnabled = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtOutCnlNum = new System.Windows.Forms.TextBox();
            this.txtCmdNum = new System.Windows.Forms.TextBox();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblOutCnlNum
            // 
            this.lblOutCnlNum.AutoSize = true;
            this.lblOutCnlNum.Location = new System.Drawing.Point(9, 34);
            this.lblOutCnlNum.Name = "lblOutCnlNum";
            this.lblOutCnlNum.Size = new System.Drawing.Size(51, 15);
            this.lblOutCnlNum.TabIndex = 1;
            this.lblOutCnlNum.Text = "Number";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(12, 12);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(59, 19);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // cbCmdType
            // 
            this.cbCmdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdType.FormattingEnabled = true;
            this.cbCmdType.Location = new System.Drawing.Point(12, 96);
            this.cbCmdType.Name = "cbCmdType";
            this.cbCmdType.Size = new System.Drawing.Size(460, 23);
            this.cbCmdType.TabIndex = 6;
            // 
            // lblCmdType
            // 
            this.lblCmdType.AutoSize = true;
            this.lblCmdType.Location = new System.Drawing.Point(9, 78);
            this.lblCmdType.Name = "lblCmdType";
            this.lblCmdType.Size = new System.Drawing.Size(90, 15);
            this.lblCmdType.TabIndex = 5;
            this.lblCmdType.Text = "Command type";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(118, 52);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(354, 23);
            this.txtName.TabIndex = 4;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(115, 34);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name";
            // 
            // lblObj
            // 
            this.lblObj.AutoSize = true;
            this.lblObj.Location = new System.Drawing.Point(9, 122);
            this.lblObj.Name = "lblObj";
            this.lblObj.Size = new System.Drawing.Size(42, 15);
            this.lblObj.TabIndex = 7;
            this.lblObj.Text = "Object";
            // 
            // txtObjNum
            // 
            this.txtObjNum.Location = new System.Drawing.Point(12, 140);
            this.txtObjNum.Name = "txtObjNum";
            this.txtObjNum.ReadOnly = true;
            this.txtObjNum.Size = new System.Drawing.Size(100, 23);
            this.txtObjNum.TabIndex = 8;
            // 
            // cbObj
            // 
            this.cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(118, 140);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(354, 23);
            this.cbObj.TabIndex = 9;
            this.cbObj.SelectedIndexChanged += new System.EventHandler(this.cbObj_SelectedIndexChanged);
            // 
            // cbDevice
            // 
            this.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Location = new System.Drawing.Point(118, 184);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(354, 23);
            this.cbDevice.TabIndex = 12;
            this.cbDevice.SelectedIndexChanged += new System.EventHandler(this.cbDevice_SelectedIndexChanged);
            // 
            // txtDeviceNum
            // 
            this.txtDeviceNum.Location = new System.Drawing.Point(12, 184);
            this.txtDeviceNum.Name = "txtDeviceNum";
            this.txtDeviceNum.ReadOnly = true;
            this.txtDeviceNum.Size = new System.Drawing.Size(100, 23);
            this.txtDeviceNum.TabIndex = 11;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(9, 166);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 15);
            this.lblDevice.TabIndex = 10;
            this.lblDevice.Text = "Device";
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(9, 254);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(51, 15);
            this.lblFormula.TabIndex = 17;
            this.lblFormula.Text = "Formula";
            // 
            // chkFormulaEnabled
            // 
            this.chkFormulaEnabled.AutoSize = true;
            this.chkFormulaEnabled.Location = new System.Drawing.Point(12, 276);
            this.chkFormulaEnabled.Name = "chkFormulaEnabled";
            this.chkFormulaEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkFormulaEnabled.TabIndex = 18;
            this.chkFormulaEnabled.UseVisualStyleBackColor = true;
            // 
            // txtFormula
            // 
            this.txtFormula.Location = new System.Drawing.Point(33, 272);
            this.txtFormula.MaxLength = 100;
            this.txtFormula.Name = "txtFormula";
            this.txtFormula.Size = new System.Drawing.Size(439, 23);
            this.txtFormula.TabIndex = 19;
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(9, 298);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(45, 15);
            this.lblFormat.TabIndex = 20;
            this.lblFormat.Text = "Format";
            // 
            // cbFormat
            // 
            this.cbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Location = new System.Drawing.Point(12, 316);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(460, 23);
            this.cbFormat.TabIndex = 21;
            // 
            // chkEventEnabled
            // 
            this.chkEventEnabled.AutoSize = true;
            this.chkEventEnabled.Location = new System.Drawing.Point(12, 345);
            this.chkEventEnabled.Name = "chkEventEnabled";
            this.chkEventEnabled.Size = new System.Drawing.Size(100, 19);
            this.chkEventEnabled.TabIndex = 22;
            this.chkEventEnabled.Text = "Event enabled";
            this.chkEventEnabled.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(316, 370);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 23;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(397, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtOutCnlNum
            // 
            this.txtOutCnlNum.Location = new System.Drawing.Point(12, 52);
            this.txtOutCnlNum.Name = "txtOutCnlNum";
            this.txtOutCnlNum.Size = new System.Drawing.Size(100, 23);
            this.txtOutCnlNum.TabIndex = 2;
            // 
            // txtCmdNum
            // 
            this.txtCmdNum.Location = new System.Drawing.Point(12, 228);
            this.txtCmdNum.Name = "txtCmdNum";
            this.txtCmdNum.Size = new System.Drawing.Size(100, 23);
            this.txtCmdNum.TabIndex = 14;
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(9, 210);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(109, 15);
            this.lblCmdNum.TabIndex = 13;
            this.lblCmdNum.Text = "Command number";
            // 
            // lblCmdCode
            // 
            this.lblCmdCode.AutoSize = true;
            this.lblCmdCode.Location = new System.Drawing.Point(115, 210);
            this.lblCmdCode.Name = "lblCmdCode";
            this.lblCmdCode.Size = new System.Drawing.Size(93, 15);
            this.lblCmdCode.TabIndex = 15;
            this.lblCmdCode.Text = "Command code";
            // 
            // txtCmdCode
            // 
            this.txtCmdCode.Location = new System.Drawing.Point(118, 228);
            this.txtCmdCode.Name = "txtCmdCode";
            this.txtCmdCode.Size = new System.Drawing.Size(354, 23);
            this.txtCmdCode.TabIndex = 16;
            // 
            // FrmOutCnl
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 405);
            this.Controls.Add(this.txtCmdCode);
            this.Controls.Add(this.lblCmdCode);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.lblCmdNum);
            this.Controls.Add(this.chkEventEnabled);
            this.Controls.Add(this.txtCmdNum);
            this.Controls.Add(this.txtOutCnlNum);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbFormat);
            this.Controls.Add(this.txtFormula);
            this.Controls.Add(this.chkFormulaEnabled);
            this.Controls.Add(this.lblFormula);
            this.Controls.Add(this.cbDevice);
            this.Controls.Add(this.txtDeviceNum);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.cbObj);
            this.Controls.Add(this.txtObjNum);
            this.Controls.Add(this.lblObj);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCmdType);
            this.Controls.Add(this.cbCmdType);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.lblOutCnlNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOutCnl";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Output Channel Properties";
            this.Load += new System.EventHandler(this.FrmInCnlProps_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOutCnlNum;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.ComboBox cbCmdType;
        private System.Windows.Forms.Label lblCmdType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblObj;
        private System.Windows.Forms.TextBox txtObjNum;
        private System.Windows.Forms.ComboBox cbObj;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.TextBox txtDeviceNum;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label lblFormula;
        private System.Windows.Forms.CheckBox chkFormulaEnabled;
        private System.Windows.Forms.TextBox txtFormula;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.ComboBox cbFormat;
        private System.Windows.Forms.CheckBox chkEventEnabled;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtOutCnlNum;
        private System.Windows.Forms.TextBox txtCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.Label lblCmdCode;
        private System.Windows.Forms.TextBox txtCmdCode;
    }
}