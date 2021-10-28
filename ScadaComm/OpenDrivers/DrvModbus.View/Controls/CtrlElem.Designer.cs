namespace Scada.Comm.Drivers.DrvModbus.View.Controls
{
    partial class CtrlElem
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
            this.gbElem = new System.Windows.Forms.GroupBox();
            this.chkElemIsBitMask = new System.Windows.Forms.CheckBox();
            this.chkElemReadOnly = new System.Windows.Forms.CheckBox();
            this.lblElemByteOrderExample = new System.Windows.Forms.Label();
            this.txtElemByteOrder = new System.Windows.Forms.TextBox();
            this.lblElemByteOrder = new System.Windows.Forms.Label();
            this.rbBool = new System.Windows.Forms.RadioButton();
            this.rbDouble = new System.Windows.Forms.RadioButton();
            this.rbFloat = new System.Windows.Forms.RadioButton();
            this.rbLong = new System.Windows.Forms.RadioButton();
            this.rbULong = new System.Windows.Forms.RadioButton();
            this.rbInt = new System.Windows.Forms.RadioButton();
            this.rbUInt = new System.Windows.Forms.RadioButton();
            this.rbShort = new System.Windows.Forms.RadioButton();
            this.rbUShort = new System.Windows.Forms.RadioButton();
            this.lblElemType = new System.Windows.Forms.Label();
            this.txtElemAddress = new System.Windows.Forms.TextBox();
            this.lblElemAddress = new System.Windows.Forms.Label();
            this.txtElemTagNum = new System.Windows.Forms.TextBox();
            this.lblElemTagNum = new System.Windows.Forms.Label();
            this.txtElemTagCode = new System.Windows.Forms.TextBox();
            this.lblElemTagCode = new System.Windows.Forms.Label();
            this.txtElemName = new System.Windows.Forms.TextBox();
            this.lblElemName = new System.Windows.Forms.Label();
            this.gbElem.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbElem
            // 
            this.gbElem.Controls.Add(this.chkElemIsBitMask);
            this.gbElem.Controls.Add(this.chkElemReadOnly);
            this.gbElem.Controls.Add(this.lblElemByteOrderExample);
            this.gbElem.Controls.Add(this.txtElemByteOrder);
            this.gbElem.Controls.Add(this.lblElemByteOrder);
            this.gbElem.Controls.Add(this.rbBool);
            this.gbElem.Controls.Add(this.rbDouble);
            this.gbElem.Controls.Add(this.rbFloat);
            this.gbElem.Controls.Add(this.rbLong);
            this.gbElem.Controls.Add(this.rbULong);
            this.gbElem.Controls.Add(this.rbInt);
            this.gbElem.Controls.Add(this.rbUInt);
            this.gbElem.Controls.Add(this.rbShort);
            this.gbElem.Controls.Add(this.rbUShort);
            this.gbElem.Controls.Add(this.lblElemType);
            this.gbElem.Controls.Add(this.txtElemAddress);
            this.gbElem.Controls.Add(this.lblElemAddress);
            this.gbElem.Controls.Add(this.txtElemTagNum);
            this.gbElem.Controls.Add(this.lblElemTagNum);
            this.gbElem.Controls.Add(this.txtElemTagCode);
            this.gbElem.Controls.Add(this.lblElemTagCode);
            this.gbElem.Controls.Add(this.txtElemName);
            this.gbElem.Controls.Add(this.lblElemName);
            this.gbElem.Location = new System.Drawing.Point(0, 0);
            this.gbElem.Name = "gbElem";
            this.gbElem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbElem.Size = new System.Drawing.Size(300, 395);
            this.gbElem.TabIndex = 0;
            this.gbElem.TabStop = false;
            this.gbElem.Text = "Element Parameters";
            // 
            // chkElemIsBitMask
            // 
            this.chkElemIsBitMask.AutoSize = true;
            this.chkElemIsBitMask.Location = new System.Drawing.Point(13, 363);
            this.chkElemIsBitMask.Name = "chkElemIsBitMask";
            this.chkElemIsBitMask.Size = new System.Drawing.Size(71, 19);
            this.chkElemIsBitMask.TabIndex = 22;
            this.chkElemIsBitMask.Text = "Bit mask";
            this.chkElemIsBitMask.UseVisualStyleBackColor = true;
            this.chkElemIsBitMask.CheckedChanged += new System.EventHandler(this.chkElemIsBitMask_CheckedChanged);
            // 
            // chkElemReadOnly
            // 
            this.chkElemReadOnly.AutoSize = true;
            this.chkElemReadOnly.Location = new System.Drawing.Point(13, 338);
            this.chkElemReadOnly.Name = "chkElemReadOnly";
            this.chkElemReadOnly.Size = new System.Drawing.Size(78, 19);
            this.chkElemReadOnly.TabIndex = 21;
            this.chkElemReadOnly.Text = "Read only";
            this.chkElemReadOnly.UseVisualStyleBackColor = true;
            this.chkElemReadOnly.CheckedChanged += new System.EventHandler(this.chkElemReadOnly_CheckedChanged);
            // 
            // lblElemByteOrderExample
            // 
            this.lblElemByteOrderExample.AutoSize = true;
            this.lblElemByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblElemByteOrderExample.Location = new System.Drawing.Point(153, 313);
            this.lblElemByteOrderExample.Name = "lblElemByteOrderExample";
            this.lblElemByteOrderExample.Size = new System.Drawing.Size(126, 15);
            this.lblElemByteOrderExample.TabIndex = 20;
            this.lblElemByteOrderExample.Text = "For example, 01234567";
            // 
            // txtElemByteOrder
            // 
            this.txtElemByteOrder.Location = new System.Drawing.Point(13, 309);
            this.txtElemByteOrder.Name = "txtElemByteOrder";
            this.txtElemByteOrder.Size = new System.Drawing.Size(134, 23);
            this.txtElemByteOrder.TabIndex = 19;
            this.txtElemByteOrder.TextChanged += new System.EventHandler(this.txtByteOrder_TextChanged);
            // 
            // lblElemByteOrder
            // 
            this.lblElemByteOrder.AutoSize = true;
            this.lblElemByteOrder.Location = new System.Drawing.Point(10, 291);
            this.lblElemByteOrder.Name = "lblElemByteOrder";
            this.lblElemByteOrder.Size = new System.Drawing.Size(61, 15);
            this.lblElemByteOrder.TabIndex = 18;
            this.lblElemByteOrder.Text = "Byte order";
            // 
            // rbBool
            // 
            this.rbBool.AutoSize = true;
            this.rbBool.Location = new System.Drawing.Point(13, 269);
            this.rbBool.Name = "rbBool";
            this.rbBool.Size = new System.Drawing.Size(83, 19);
            this.rbBool.TabIndex = 17;
            this.rbBool.TabStop = true;
            this.rbBool.Text = "bool (1 bit)";
            this.rbBool.UseVisualStyleBackColor = true;
            this.rbBool.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbDouble
            // 
            this.rbDouble.AutoSize = true;
            this.rbDouble.Location = new System.Drawing.Point(153, 244);
            this.rbDouble.Name = "rbDouble";
            this.rbDouble.Size = new System.Drawing.Size(110, 19);
            this.rbDouble.TabIndex = 16;
            this.rbDouble.TabStop = true;
            this.rbDouble.Text = "double (8 bytes)";
            this.rbDouble.UseVisualStyleBackColor = true;
            this.rbDouble.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbFloat
            // 
            this.rbFloat.AutoSize = true;
            this.rbFloat.Location = new System.Drawing.Point(13, 244);
            this.rbFloat.Name = "rbFloat";
            this.rbFloat.Size = new System.Drawing.Size(97, 19);
            this.rbFloat.TabIndex = 15;
            this.rbFloat.TabStop = true;
            this.rbFloat.Text = "float (4 bytes)";
            this.rbFloat.UseVisualStyleBackColor = true;
            this.rbFloat.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbLong
            // 
            this.rbLong.AutoSize = true;
            this.rbLong.Location = new System.Drawing.Point(153, 219);
            this.rbLong.Name = "rbLong";
            this.rbLong.Size = new System.Drawing.Size(97, 19);
            this.rbLong.TabIndex = 14;
            this.rbLong.TabStop = true;
            this.rbLong.Text = "long (8 bytes)";
            this.rbLong.UseVisualStyleBackColor = true;
            this.rbLong.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbULong
            // 
            this.rbULong.AutoSize = true;
            this.rbULong.Location = new System.Drawing.Point(13, 219);
            this.rbULong.Name = "rbULong";
            this.rbULong.Size = new System.Drawing.Size(104, 19);
            this.rbULong.TabIndex = 13;
            this.rbULong.TabStop = true;
            this.rbULong.Text = "ulong (8 bytes)";
            this.rbULong.UseVisualStyleBackColor = true;
            this.rbULong.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbInt
            // 
            this.rbInt.AutoSize = true;
            this.rbInt.Location = new System.Drawing.Point(153, 194);
            this.rbInt.Name = "rbInt";
            this.rbInt.Size = new System.Drawing.Size(87, 19);
            this.rbInt.TabIndex = 12;
            this.rbInt.TabStop = true;
            this.rbInt.Text = "int (4 bytes)";
            this.rbInt.UseVisualStyleBackColor = true;
            this.rbInt.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbUInt
            // 
            this.rbUInt.AutoSize = true;
            this.rbUInt.Location = new System.Drawing.Point(13, 194);
            this.rbUInt.Name = "rbUInt";
            this.rbUInt.Size = new System.Drawing.Size(94, 19);
            this.rbUInt.TabIndex = 11;
            this.rbUInt.TabStop = true;
            this.rbUInt.Text = "uint (4 bytes)";
            this.rbUInt.UseVisualStyleBackColor = true;
            this.rbUInt.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbShort
            // 
            this.rbShort.AutoSize = true;
            this.rbShort.Location = new System.Drawing.Point(153, 169);
            this.rbShort.Name = "rbShort";
            this.rbShort.Size = new System.Drawing.Size(100, 19);
            this.rbShort.TabIndex = 10;
            this.rbShort.TabStop = true;
            this.rbShort.Text = "short (2 bytes)";
            this.rbShort.UseVisualStyleBackColor = true;
            this.rbShort.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // rbUShort
            // 
            this.rbUShort.AutoSize = true;
            this.rbUShort.Location = new System.Drawing.Point(13, 169);
            this.rbUShort.Name = "rbUShort";
            this.rbUShort.Size = new System.Drawing.Size(107, 19);
            this.rbUShort.TabIndex = 9;
            this.rbUShort.TabStop = true;
            this.rbUShort.Text = "ushort (2 bytes)";
            this.rbUShort.UseVisualStyleBackColor = true;
            this.rbUShort.CheckedChanged += new System.EventHandler(this.rbType_CheckedChanged);
            // 
            // lblElemType
            // 
            this.lblElemType.AutoSize = true;
            this.lblElemType.Location = new System.Drawing.Point(10, 151);
            this.lblElemType.Name = "lblElemType";
            this.lblElemType.Size = new System.Drawing.Size(34, 15);
            this.lblElemType.TabIndex = 8;
            this.lblElemType.Text = "Type:";
            // 
            // txtElemAddress
            // 
            this.txtElemAddress.Location = new System.Drawing.Point(153, 125);
            this.txtElemAddress.Name = "txtElemAddress";
            this.txtElemAddress.ReadOnly = true;
            this.txtElemAddress.Size = new System.Drawing.Size(134, 23);
            this.txtElemAddress.TabIndex = 7;
            // 
            // lblElemAddress
            // 
            this.lblElemAddress.AutoSize = true;
            this.lblElemAddress.Location = new System.Drawing.Point(150, 107);
            this.lblElemAddress.Name = "lblElemAddress";
            this.lblElemAddress.Size = new System.Drawing.Size(49, 15);
            this.lblElemAddress.TabIndex = 6;
            this.lblElemAddress.Text = "Address";
            // 
            // txtElemTagNum
            // 
            this.txtElemTagNum.Location = new System.Drawing.Point(13, 125);
            this.txtElemTagNum.Name = "txtElemTagNum";
            this.txtElemTagNum.ReadOnly = true;
            this.txtElemTagNum.Size = new System.Drawing.Size(134, 23);
            this.txtElemTagNum.TabIndex = 5;
            // 
            // lblElemTagNum
            // 
            this.lblElemTagNum.AutoSize = true;
            this.lblElemTagNum.Location = new System.Drawing.Point(10, 107);
            this.lblElemTagNum.Name = "lblElemTagNum";
            this.lblElemTagNum.Size = new System.Drawing.Size(70, 15);
            this.lblElemTagNum.TabIndex = 4;
            this.lblElemTagNum.Text = "Tag number";
            // 
            // txtElemTagCode
            // 
            this.txtElemTagCode.Location = new System.Drawing.Point(13, 81);
            this.txtElemTagCode.Name = "txtElemTagCode";
            this.txtElemTagCode.Size = new System.Drawing.Size(134, 23);
            this.txtElemTagCode.TabIndex = 3;
            this.txtElemTagCode.TextChanged += new System.EventHandler(this.txtElemTagCode_TextChanged);
            // 
            // lblElemTagCode
            // 
            this.lblElemTagCode.AutoSize = true;
            this.lblElemTagCode.Location = new System.Drawing.Point(10, 63);
            this.lblElemTagCode.Name = "lblElemTagCode";
            this.lblElemTagCode.Size = new System.Drawing.Size(54, 15);
            this.lblElemTagCode.TabIndex = 2;
            this.lblElemTagCode.Text = "Tag code";
            // 
            // txtElemName
            // 
            this.txtElemName.Location = new System.Drawing.Point(13, 37);
            this.txtElemName.Name = "txtElemName";
            this.txtElemName.Size = new System.Drawing.Size(274, 23);
            this.txtElemName.TabIndex = 1;
            this.txtElemName.TextChanged += new System.EventHandler(this.txtElemName_TextChanged);
            // 
            // lblElemName
            // 
            this.lblElemName.AutoSize = true;
            this.lblElemName.Location = new System.Drawing.Point(10, 19);
            this.lblElemName.Name = "lblElemName";
            this.lblElemName.Size = new System.Drawing.Size(39, 15);
            this.lblElemName.TabIndex = 0;
            this.lblElemName.Text = "Name";
            // 
            // CtrlElem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbElem);
            this.Name = "CtrlElem";
            this.Size = new System.Drawing.Size(300, 395);
            this.gbElem.ResumeLayout(false);
            this.gbElem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbElem;
        private System.Windows.Forms.Label lblElemByteOrderExample;
        private System.Windows.Forms.TextBox txtElemByteOrder;
        private System.Windows.Forms.Label lblElemByteOrder;
        private System.Windows.Forms.RadioButton rbDouble;
        private System.Windows.Forms.RadioButton rbLong;
        private System.Windows.Forms.RadioButton rbULong;
        private System.Windows.Forms.RadioButton rbBool;
        private System.Windows.Forms.RadioButton rbFloat;
        private System.Windows.Forms.RadioButton rbInt;
        private System.Windows.Forms.RadioButton rbUInt;
        private System.Windows.Forms.RadioButton rbShort;
        private System.Windows.Forms.Label lblElemType;
        private System.Windows.Forms.RadioButton rbUShort;
        private System.Windows.Forms.TextBox txtElemAddress;
        private System.Windows.Forms.Label lblElemAddress;
        private System.Windows.Forms.TextBox txtElemTagNum;
        private System.Windows.Forms.Label lblElemTagNum;
        private System.Windows.Forms.TextBox txtElemName;
        private System.Windows.Forms.Label lblElemName;
        private System.Windows.Forms.Label lblElemTagCode;
        private System.Windows.Forms.TextBox txtElemTagCode;
        private System.Windows.Forms.CheckBox chkElemIsBitMask;
        private System.Windows.Forms.CheckBox chkElemReadOnly;
    }
}
