namespace Scada.Comm.Drivers.DrvModbus.View.Controls
{
    partial class CtrlCmd
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
            this.gbCmd = new System.Windows.Forms.GroupBox();
            this.pnlCmdElem = new System.Windows.Forms.Panel();
            this.lblCmdByteOrderExample = new System.Windows.Forms.Label();
            this.txtCmdByteOrder = new System.Windows.Forms.TextBox();
            this.lblCmdByteOrder = new System.Windows.Forms.Label();
            this.numCmdElemCnt = new System.Windows.Forms.NumericUpDown();
            this.lblCmdElemCnt = new System.Windows.Forms.Label();
            this.cbCmdElemType = new System.Windows.Forms.ComboBox();
            this.lblCmdElemType = new System.Windows.Forms.Label();
            this.lblCmdAddressHint = new System.Windows.Forms.Label();
            this.numCmdAddress = new System.Windows.Forms.NumericUpDown();
            this.lblCmdAddress = new System.Windows.Forms.Label();
            this.txtCmdFuncCodeHex = new System.Windows.Forms.TextBox();
            this.lblCmdFuncCodeHex = new System.Windows.Forms.Label();
            this.numCmdFuncCode = new System.Windows.Forms.NumericUpDown();
            this.lblCmdFuncCode = new System.Windows.Forms.Label();
            this.chkCmdMultiple = new System.Windows.Forms.CheckBox();
            this.cbCmdDataBlock = new System.Windows.Forms.ComboBox();
            this.lblCmdDataBlock = new System.Windows.Forms.Label();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.pnlCmdCodeWarn = new System.Windows.Forms.Panel();
            this.lblCmdCodeWarn = new System.Windows.Forms.Label();
            this.pbCmdCodeWarn = new System.Windows.Forms.PictureBox();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtCmdName = new System.Windows.Forms.TextBox();
            this.lblCmdName = new System.Windows.Forms.Label();
            this.gbCmd.SuspendLayout();
            this.pnlCmdElem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdElemCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdFuncCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            this.pnlCmdCodeWarn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCmdCodeWarn)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCmd
            // 
            this.gbCmd.Controls.Add(this.pnlCmdElem);
            this.gbCmd.Controls.Add(this.txtCmdFuncCodeHex);
            this.gbCmd.Controls.Add(this.lblCmdFuncCodeHex);
            this.gbCmd.Controls.Add(this.numCmdFuncCode);
            this.gbCmd.Controls.Add(this.lblCmdFuncCode);
            this.gbCmd.Controls.Add(this.chkCmdMultiple);
            this.gbCmd.Controls.Add(this.cbCmdDataBlock);
            this.gbCmd.Controls.Add(this.lblCmdDataBlock);
            this.gbCmd.Controls.Add(this.numCmdNum);
            this.gbCmd.Controls.Add(this.lblCmdNum);
            this.gbCmd.Controls.Add(this.pnlCmdCodeWarn);
            this.gbCmd.Controls.Add(this.txtCmdCode);
            this.gbCmd.Controls.Add(this.lblCmdCode);
            this.gbCmd.Controls.Add(this.txtCmdName);
            this.gbCmd.Controls.Add(this.lblCmdName);
            this.gbCmd.Location = new System.Drawing.Point(0, 0);
            this.gbCmd.Name = "gbCmd";
            this.gbCmd.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCmd.Size = new System.Drawing.Size(300, 409);
            this.gbCmd.TabIndex = 0;
            this.gbCmd.TabStop = false;
            this.gbCmd.Text = "Command Parameters";
            // 
            // pnlCmdElem
            // 
            this.pnlCmdElem.Controls.Add(this.lblCmdByteOrderExample);
            this.pnlCmdElem.Controls.Add(this.txtCmdByteOrder);
            this.pnlCmdElem.Controls.Add(this.lblCmdByteOrder);
            this.pnlCmdElem.Controls.Add(this.numCmdElemCnt);
            this.pnlCmdElem.Controls.Add(this.lblCmdElemCnt);
            this.pnlCmdElem.Controls.Add(this.cbCmdElemType);
            this.pnlCmdElem.Controls.Add(this.lblCmdElemType);
            this.pnlCmdElem.Controls.Add(this.lblCmdAddressHint);
            this.pnlCmdElem.Controls.Add(this.numCmdAddress);
            this.pnlCmdElem.Controls.Add(this.lblCmdAddress);
            this.pnlCmdElem.Location = new System.Drawing.Point(13, 267);
            this.pnlCmdElem.Name = "pnlCmdElem";
            this.pnlCmdElem.Size = new System.Drawing.Size(274, 129);
            this.pnlCmdElem.TabIndex = 14;
            // 
            // lblCmdByteOrderExample
            // 
            this.lblCmdByteOrderExample.AutoSize = true;
            this.lblCmdByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCmdByteOrderExample.Location = new System.Drawing.Point(137, 110);
            this.lblCmdByteOrderExample.Name = "lblCmdByteOrderExample";
            this.lblCmdByteOrderExample.Size = new System.Drawing.Size(126, 15);
            this.lblCmdByteOrderExample.TabIndex = 9;
            this.lblCmdByteOrderExample.Text = "For example, 01234567";
            // 
            // txtCmdByteOrder
            // 
            this.txtCmdByteOrder.Location = new System.Drawing.Point(0, 106);
            this.txtCmdByteOrder.Name = "txtCmdByteOrder";
            this.txtCmdByteOrder.Size = new System.Drawing.Size(134, 23);
            this.txtCmdByteOrder.TabIndex = 8;
            this.txtCmdByteOrder.TextChanged += new System.EventHandler(this.txtCmdByteOrder_TextChanged);
            // 
            // lblCmdByteOrder
            // 
            this.lblCmdByteOrder.AutoSize = true;
            this.lblCmdByteOrder.Location = new System.Drawing.Point(-3, 88);
            this.lblCmdByteOrder.Name = "lblCmdByteOrder";
            this.lblCmdByteOrder.Size = new System.Drawing.Size(61, 15);
            this.lblCmdByteOrder.TabIndex = 7;
            this.lblCmdByteOrder.Text = "Byte order";
            // 
            // numCmdElemCnt
            // 
            this.numCmdElemCnt.Location = new System.Drawing.Point(140, 62);
            this.numCmdElemCnt.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numCmdElemCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdElemCnt.Name = "numCmdElemCnt";
            this.numCmdElemCnt.Size = new System.Drawing.Size(134, 23);
            this.numCmdElemCnt.TabIndex = 6;
            this.numCmdElemCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdElemCnt.ValueChanged += new System.EventHandler(this.numCmdElemCnt_ValueChanged);
            // 
            // lblCmdElemCnt
            // 
            this.lblCmdElemCnt.AutoSize = true;
            this.lblCmdElemCnt.Location = new System.Drawing.Point(137, 44);
            this.lblCmdElemCnt.Name = "lblCmdElemCnt";
            this.lblCmdElemCnt.Size = new System.Drawing.Size(84, 15);
            this.lblCmdElemCnt.TabIndex = 5;
            this.lblCmdElemCnt.Text = "Element count";
            // 
            // cbCmdElemType
            // 
            this.cbCmdElemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdElemType.FormattingEnabled = true;
            this.cbCmdElemType.Items.AddRange(new object[] {
            "Undefined",
            "ushort (2 bytes)",
            "short (2 bytes)",
            "uint (4 bytes)",
            "int (4 bytes)",
            "ulong (8 bytes)",
            "long (8 bytes)",
            "float (4 bytes)",
            "double (8 bytes)",
            "bool (1 bit)"});
            this.cbCmdElemType.Location = new System.Drawing.Point(0, 62);
            this.cbCmdElemType.Name = "cbCmdElemType";
            this.cbCmdElemType.Size = new System.Drawing.Size(134, 23);
            this.cbCmdElemType.TabIndex = 4;
            this.cbCmdElemType.SelectedIndexChanged += new System.EventHandler(this.cbCmdElemType_SelectedIndexChanged);
            // 
            // lblCmdElemType
            // 
            this.lblCmdElemType.AutoSize = true;
            this.lblCmdElemType.Location = new System.Drawing.Point(-3, 44);
            this.lblCmdElemType.Name = "lblCmdElemType";
            this.lblCmdElemType.Size = new System.Drawing.Size(76, 15);
            this.lblCmdElemType.TabIndex = 3;
            this.lblCmdElemType.Text = "Element type";
            // 
            // lblCmdAddressHint
            // 
            this.lblCmdAddressHint.AutoSize = true;
            this.lblCmdAddressHint.Location = new System.Drawing.Point(137, 22);
            this.lblCmdAddressHint.Name = "lblCmdAddressHint";
            this.lblCmdAddressHint.Size = new System.Drawing.Size(29, 15);
            this.lblCmdAddressHint.TabIndex = 2;
            this.lblCmdAddressHint.Text = "DEC";
            // 
            // numCmdAddress
            // 
            this.numCmdAddress.Location = new System.Drawing.Point(0, 18);
            this.numCmdAddress.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numCmdAddress.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdAddress.Name = "numCmdAddress";
            this.numCmdAddress.Size = new System.Drawing.Size(134, 23);
            this.numCmdAddress.TabIndex = 1;
            this.numCmdAddress.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdAddress.ValueChanged += new System.EventHandler(this.numCmdAddress_ValueChanged);
            // 
            // lblCmdAddress
            // 
            this.lblCmdAddress.AutoSize = true;
            this.lblCmdAddress.Location = new System.Drawing.Point(-3, 0);
            this.lblCmdAddress.Name = "lblCmdAddress";
            this.lblCmdAddress.Size = new System.Drawing.Size(93, 15);
            this.lblCmdAddress.TabIndex = 0;
            this.lblCmdAddress.Text = "Element address";
            // 
            // txtCmdFuncCodeHex
            // 
            this.txtCmdFuncCodeHex.Location = new System.Drawing.Point(153, 238);
            this.txtCmdFuncCodeHex.Name = "txtCmdFuncCodeHex";
            this.txtCmdFuncCodeHex.ReadOnly = true;
            this.txtCmdFuncCodeHex.Size = new System.Drawing.Size(134, 23);
            this.txtCmdFuncCodeHex.TabIndex = 13;
            // 
            // lblCmdFuncCodeHex
            // 
            this.lblCmdFuncCodeHex.AutoSize = true;
            this.lblCmdFuncCodeHex.Location = new System.Drawing.Point(150, 220);
            this.lblCmdFuncCodeHex.Name = "lblCmdFuncCodeHex";
            this.lblCmdFuncCodeHex.Size = new System.Drawing.Size(76, 15);
            this.lblCmdFuncCodeHex.TabIndex = 12;
            this.lblCmdFuncCodeHex.Text = "Hexadecimal";
            // 
            // numCmdFuncCode
            // 
            this.numCmdFuncCode.Location = new System.Drawing.Point(13, 238);
            this.numCmdFuncCode.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numCmdFuncCode.Name = "numCmdFuncCode";
            this.numCmdFuncCode.Size = new System.Drawing.Size(134, 23);
            this.numCmdFuncCode.TabIndex = 11;
            this.numCmdFuncCode.ValueChanged += new System.EventHandler(this.numCmdFuncCode_ValueChanged);
            // 
            // lblCmdFuncCode
            // 
            this.lblCmdFuncCode.AutoSize = true;
            this.lblCmdFuncCode.Location = new System.Drawing.Point(10, 220);
            this.lblCmdFuncCode.Name = "lblCmdFuncCode";
            this.lblCmdFuncCode.Size = new System.Drawing.Size(83, 15);
            this.lblCmdFuncCode.TabIndex = 10;
            this.lblCmdFuncCode.Text = "Function code";
            // 
            // chkCmdMultiple
            // 
            this.chkCmdMultiple.AutoSize = true;
            this.chkCmdMultiple.Location = new System.Drawing.Point(13, 198);
            this.chkCmdMultiple.Name = "chkCmdMultiple";
            this.chkCmdMultiple.Size = new System.Drawing.Size(70, 19);
            this.chkCmdMultiple.TabIndex = 9;
            this.chkCmdMultiple.Text = "Multiple";
            this.chkCmdMultiple.UseVisualStyleBackColor = true;
            this.chkCmdMultiple.CheckedChanged += new System.EventHandler(this.chkCmdMultiple_CheckedChanged);
            // 
            // cbCmdDataBlock
            // 
            this.cbCmdDataBlock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCmdDataBlock.FormattingEnabled = true;
            this.cbCmdDataBlock.Items.AddRange(new object[] {
            "Coils (0X)",
            "Holding Registers (4X)",
            "Custom"});
            this.cbCmdDataBlock.Location = new System.Drawing.Point(13, 169);
            this.cbCmdDataBlock.Name = "cbCmdDataBlock";
            this.cbCmdDataBlock.Size = new System.Drawing.Size(274, 23);
            this.cbCmdDataBlock.TabIndex = 8;
            this.cbCmdDataBlock.SelectedIndexChanged += new System.EventHandler(this.cbCmdDataBlock_SelectedIndexChanged);
            // 
            // lblCmdDataBlock
            // 
            this.lblCmdDataBlock.AutoSize = true;
            this.lblCmdDataBlock.Location = new System.Drawing.Point(10, 151);
            this.lblCmdDataBlock.Name = "lblCmdDataBlock";
            this.lblCmdDataBlock.Size = new System.Drawing.Size(63, 15);
            this.lblCmdDataBlock.TabIndex = 7;
            this.lblCmdDataBlock.Text = "Data block";
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(13, 125);
            this.numCmdNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numCmdNum.Name = "numCmdNum";
            this.numCmdNum.Size = new System.Drawing.Size(134, 23);
            this.numCmdNum.TabIndex = 6;
            this.numCmdNum.ValueChanged += new System.EventHandler(this.numCmdNum_ValueChanged);
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(10, 107);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(109, 15);
            this.lblCmdNum.TabIndex = 5;
            this.lblCmdNum.Text = "Command number";
            // 
            // pnlCmdCodeWarn
            // 
            this.pnlCmdCodeWarn.Controls.Add(this.lblCmdCodeWarn);
            this.pnlCmdCodeWarn.Controls.Add(this.pbCmdCodeWarn);
            this.pnlCmdCodeWarn.Location = new System.Drawing.Point(153, 81);
            this.pnlCmdCodeWarn.Name = "pnlCmdCodeWarn";
            this.pnlCmdCodeWarn.Size = new System.Drawing.Size(134, 23);
            this.pnlCmdCodeWarn.TabIndex = 4;
            // 
            // lblCmdCodeWarn
            // 
            this.lblCmdCodeWarn.AutoSize = true;
            this.lblCmdCodeWarn.ForeColor = System.Drawing.Color.Red;
            this.lblCmdCodeWarn.Location = new System.Drawing.Point(19, 4);
            this.lblCmdCodeWarn.Name = "lblCmdCodeWarn";
            this.lblCmdCodeWarn.Size = new System.Drawing.Size(72, 15);
            this.lblCmdCodeWarn.TabIndex = 0;
            this.lblCmdCodeWarn.Text = "Fill out code";
            // 
            // pbCmdCodeWarn
            // 
            this.pbCmdCodeWarn.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.warning;
            this.pbCmdCodeWarn.Location = new System.Drawing.Point(0, 3);
            this.pbCmdCodeWarn.Name = "pbCmdCodeWarn";
            this.pbCmdCodeWarn.Size = new System.Drawing.Size(16, 16);
            this.pbCmdCodeWarn.TabIndex = 0;
            this.pbCmdCodeWarn.TabStop = false;
            // 
            // txtCmdCode
            // 
            this.txtCmdCode.Location = new System.Drawing.Point(13, 81);
            this.txtCmdCode.Name = "txtCmdCode";
            this.txtCmdCode.Size = new System.Drawing.Size(134, 23);
            this.txtCmdCode.TabIndex = 3;
            this.txtCmdCode.TextChanged += new System.EventHandler(this.txtCmdCode_TextChanged);
            // 
            // lblCmdCode
            // 
            this.lblCmdCode.AutoSize = true;
            this.lblCmdCode.Location = new System.Drawing.Point(10, 63);
            this.lblCmdCode.Name = "lblCmdCode";
            this.lblCmdCode.Size = new System.Drawing.Size(93, 15);
            this.lblCmdCode.TabIndex = 2;
            this.lblCmdCode.Text = "Command code";
            // 
            // txtCmdName
            // 
            this.txtCmdName.Location = new System.Drawing.Point(13, 37);
            this.txtCmdName.Name = "txtCmdName";
            this.txtCmdName.Size = new System.Drawing.Size(274, 23);
            this.txtCmdName.TabIndex = 1;
            this.txtCmdName.TextChanged += new System.EventHandler(this.txtCmdName_TextChanged);
            // 
            // lblCmdName
            // 
            this.lblCmdName.AutoSize = true;
            this.lblCmdName.Location = new System.Drawing.Point(10, 19);
            this.lblCmdName.Name = "lblCmdName";
            this.lblCmdName.Size = new System.Drawing.Size(39, 15);
            this.lblCmdName.TabIndex = 0;
            this.lblCmdName.Text = "Name";
            // 
            // CtrlCmd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCmd);
            this.Name = "CtrlCmd";
            this.Size = new System.Drawing.Size(300, 409);
            this.gbCmd.ResumeLayout(false);
            this.gbCmd.PerformLayout();
            this.pnlCmdElem.ResumeLayout(false);
            this.pnlCmdElem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdElemCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdFuncCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            this.pnlCmdCodeWarn.ResumeLayout(false);
            this.pnlCmdCodeWarn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCmdCodeWarn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCmd;
        private System.Windows.Forms.CheckBox chkCmdMultiple;
        private System.Windows.Forms.Label lblCmdElemCnt;
        private System.Windows.Forms.NumericUpDown numCmdElemCnt;
        private System.Windows.Forms.TextBox txtCmdName;
        private System.Windows.Forms.Label lblCmdName;
        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.NumericUpDown numCmdAddress;
        private System.Windows.Forms.Label lblCmdAddress;
        private System.Windows.Forms.Label lblCmdDataBlock;
        private System.Windows.Forms.ComboBox cbCmdDataBlock;
        private System.Windows.Forms.TextBox txtCmdFuncCodeHex;
        private System.Windows.Forms.Label lblCmdFuncCode;
        private System.Windows.Forms.Label lblCmdByteOrderExample;
        private System.Windows.Forms.TextBox txtCmdByteOrder;
        private System.Windows.Forms.Label lblCmdByteOrder;
        private System.Windows.Forms.Label lblCmdAddressHint;
        private System.Windows.Forms.Label lblCmdElemType;
        private System.Windows.Forms.ComboBox cbCmdElemType;
        private System.Windows.Forms.TextBox txtCmdCode;
        private System.Windows.Forms.Label lblCmdCode;
        private System.Windows.Forms.Panel pnlCmdCodeWarn;
        private System.Windows.Forms.Label lblCmdCodeWarn;
        private System.Windows.Forms.PictureBox pbCmdCodeWarn;
        private System.Windows.Forms.NumericUpDown numCmdFuncCode;
        private System.Windows.Forms.Label lblCmdFuncCodeHex;
        private System.Windows.Forms.Panel pnlCmdElem;
    }
}
