﻿namespace Scada.Comm.Drivers.DrvModbus.View.Controls
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
            this.lblElemScalingExample = new System.Windows.Forms.Label();
            this.txtElemScaling = new System.Windows.Forms.TextBox();
            this.lblElemScaling = new System.Windows.Forms.Label();
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
            this.pnlElemTagCodeWarn = new System.Windows.Forms.Panel();
            this.lblElemTagCodeWarn = new System.Windows.Forms.Label();
            this.pbElemTagCodeWarn = new System.Windows.Forms.PictureBox();
            this.txtElemTagCode = new System.Windows.Forms.TextBox();
            this.lblElemTagCode = new System.Windows.Forms.Label();
            this.txtElemName = new System.Windows.Forms.TextBox();
            this.lblElemName = new System.Windows.Forms.Label();
            this.gbElem.SuspendLayout();
            this.pnlElemTagCodeWarn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbElemTagCodeWarn)).BeginInit();
            this.SuspendLayout();
            // 
            // gbElem
            // 
            this.gbElem.Controls.Add(this.lblElemScalingExample);
            this.gbElem.Controls.Add(this.txtElemScaling);
            this.gbElem.Controls.Add(this.lblElemScaling);
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
            this.gbElem.Controls.Add(this.pnlElemTagCodeWarn);
            this.gbElem.Controls.Add(this.txtElemTagCode);
            this.gbElem.Controls.Add(this.lblElemTagCode);
            this.gbElem.Controls.Add(this.txtElemName);
            this.gbElem.Controls.Add(this.lblElemName);
            this.gbElem.Location = new System.Drawing.Point(0, 0);
            this.gbElem.Name = "gbElem";
            this.gbElem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbElem.Size = new System.Drawing.Size(300, 448);
            this.gbElem.TabIndex = 0;
            this.gbElem.TabStop = false;
            this.gbElem.Text = "Element Parameters";
            // 
            // lblElemScalingExample
            // 
            this.lblElemScalingExample.AutoSize = true;
            this.lblElemScalingExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblElemScalingExample.Location = new System.Drawing.Point(150, 407);
            this.lblElemScalingExample.Name = "lblElemScalingExample";
            this.lblElemScalingExample.Size = new System.Drawing.Size(126, 15);
            this.lblElemScalingExample.TabIndex = 26;
            this.lblElemScalingExample.Text = "For example, 0;65535;0;35";
            // 
            // txtElemScaling
            // 
            this.txtElemScaling.Location = new System.Drawing.Point(13, 403);
            this.txtElemScaling.Name = "txtElemScaling";
            this.txtElemScaling.Size = new System.Drawing.Size(134, 23);
            this.txtElemScaling.TabIndex = 25;
            this.txtElemScaling.TextChanged += new System.EventHandler(this.txtScaling_TextChanged);
            // 
            // lblElemScaling
            // 
            this.lblElemScaling.AutoSize = true;
            this.lblElemScaling.Location = new System.Drawing.Point(10, 385);
            this.lblElemScaling.Name = "lblElemScaling";
            this.lblElemScaling.Size = new System.Drawing.Size(61, 15);
            this.lblElemScaling.TabIndex = 24;
            this.lblElemScaling.Text = "Scaling";
            // 
            // chkElemIsBitMask
            // 
            this.chkElemIsBitMask.AutoSize = true;
            this.chkElemIsBitMask.Location = new System.Drawing.Point(13, 363);
            this.chkElemIsBitMask.Name = "chkElemIsBitMask";
            this.chkElemIsBitMask.Size = new System.Drawing.Size(71, 19);
            this.chkElemIsBitMask.TabIndex = 23;
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
            this.chkElemReadOnly.TabIndex = 22;
            this.chkElemReadOnly.Text = "Read only";
            this.chkElemReadOnly.UseVisualStyleBackColor = true;
            this.chkElemReadOnly.CheckedChanged += new System.EventHandler(this.chkElemReadOnly_CheckedChanged);
            // 
            // lblElemByteOrderExample
            // 
            this.lblElemByteOrderExample.AutoSize = true;
            this.lblElemByteOrderExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblElemByteOrderExample.Location = new System.Drawing.Point(150, 313);
            this.lblElemByteOrderExample.Name = "lblElemByteOrderExample";
            this.lblElemByteOrderExample.Size = new System.Drawing.Size(126, 15);
            this.lblElemByteOrderExample.TabIndex = 21;
            this.lblElemByteOrderExample.Text = "For example, 01234567";
            // 
            // txtElemByteOrder
            // 
            this.txtElemByteOrder.Location = new System.Drawing.Point(13, 309);
            this.txtElemByteOrder.Name = "txtElemByteOrder";
            this.txtElemByteOrder.Size = new System.Drawing.Size(134, 23);
            this.txtElemByteOrder.TabIndex = 20;
            this.txtElemByteOrder.TextChanged += new System.EventHandler(this.txtByteOrder_TextChanged);
            // 
            // lblElemByteOrder
            // 
            this.lblElemByteOrder.AutoSize = true;
            this.lblElemByteOrder.Location = new System.Drawing.Point(10, 291);
            this.lblElemByteOrder.Name = "lblElemByteOrder";
            this.lblElemByteOrder.Size = new System.Drawing.Size(61, 15);
            this.lblElemByteOrder.TabIndex = 19;
            this.lblElemByteOrder.Text = "Byte order";
            // 
            // rbBool
            // 
            this.rbBool.AutoSize = true;
            this.rbBool.Location = new System.Drawing.Point(13, 269);
            this.rbBool.Name = "rbBool";
            this.rbBool.Size = new System.Drawing.Size(83, 19);
            this.rbBool.TabIndex = 18;
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
            this.rbDouble.TabIndex = 17;
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
            this.rbFloat.TabIndex = 16;
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
            this.rbLong.TabIndex = 15;
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
            this.rbULong.TabIndex = 14;
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
            this.rbInt.TabIndex = 13;
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
            this.rbUInt.TabIndex = 12;
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
            this.rbShort.TabIndex = 11;
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
            this.rbUShort.TabIndex = 10;
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
            this.lblElemType.TabIndex = 9;
            this.lblElemType.Text = "Type:";
            // 
            // txtElemAddress
            // 
            this.txtElemAddress.Location = new System.Drawing.Point(153, 125);
            this.txtElemAddress.Name = "txtElemAddress";
            this.txtElemAddress.ReadOnly = true;
            this.txtElemAddress.Size = new System.Drawing.Size(134, 23);
            this.txtElemAddress.TabIndex = 8;
            // 
            // lblElemAddress
            // 
            this.lblElemAddress.AutoSize = true;
            this.lblElemAddress.Location = new System.Drawing.Point(150, 107);
            this.lblElemAddress.Name = "lblElemAddress";
            this.lblElemAddress.Size = new System.Drawing.Size(49, 15);
            this.lblElemAddress.TabIndex = 7;
            this.lblElemAddress.Text = "Address";
            // 
            // txtElemTagNum
            // 
            this.txtElemTagNum.Location = new System.Drawing.Point(13, 125);
            this.txtElemTagNum.Name = "txtElemTagNum";
            this.txtElemTagNum.ReadOnly = true;
            this.txtElemTagNum.Size = new System.Drawing.Size(134, 23);
            this.txtElemTagNum.TabIndex = 6;
            // 
            // lblElemTagNum
            // 
            this.lblElemTagNum.AutoSize = true;
            this.lblElemTagNum.Location = new System.Drawing.Point(10, 107);
            this.lblElemTagNum.Name = "lblElemTagNum";
            this.lblElemTagNum.Size = new System.Drawing.Size(70, 15);
            this.lblElemTagNum.TabIndex = 5;
            this.lblElemTagNum.Text = "Tag number";
            // 
            // pnlElemTagCodeWarn
            // 
            this.pnlElemTagCodeWarn.Controls.Add(this.lblElemTagCodeWarn);
            this.pnlElemTagCodeWarn.Controls.Add(this.pbElemTagCodeWarn);
            this.pnlElemTagCodeWarn.Location = new System.Drawing.Point(153, 81);
            this.pnlElemTagCodeWarn.Name = "pnlElemTagCodeWarn";
            this.pnlElemTagCodeWarn.Size = new System.Drawing.Size(134, 23);
            this.pnlElemTagCodeWarn.TabIndex = 4;
            // 
            // lblElemTagCodeWarn
            // 
            this.lblElemTagCodeWarn.AutoSize = true;
            this.lblElemTagCodeWarn.ForeColor = System.Drawing.Color.Red;
            this.lblElemTagCodeWarn.Location = new System.Drawing.Point(19, 4);
            this.lblElemTagCodeWarn.Name = "lblElemTagCodeWarn";
            this.lblElemTagCodeWarn.Size = new System.Drawing.Size(72, 15);
            this.lblElemTagCodeWarn.TabIndex = 0;
            this.lblElemTagCodeWarn.Text = "Fill out code";
            // 
            // pbElemTagCodeWarn
            // 
            this.pbElemTagCodeWarn.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.warning;
            this.pbElemTagCodeWarn.Location = new System.Drawing.Point(0, 3);
            this.pbElemTagCodeWarn.Name = "pbElemTagCodeWarn";
            this.pbElemTagCodeWarn.Size = new System.Drawing.Size(16, 16);
            this.pbElemTagCodeWarn.TabIndex = 0;
            this.pbElemTagCodeWarn.TabStop = false;
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
            this.Size = new System.Drawing.Size(300, 448);
            this.gbElem.ResumeLayout(false);
            this.gbElem.PerformLayout();
            this.pnlElemTagCodeWarn.ResumeLayout(false);
            this.pnlElemTagCodeWarn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbElemTagCodeWarn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbElem;
        private System.Windows.Forms.Label lblElemByteOrderExample;
        private System.Windows.Forms.TextBox txtElemByteOrder;
        private System.Windows.Forms.Label lblElemByteOrder;
        private System.Windows.Forms.Label lblElemScalingExample;
        private System.Windows.Forms.TextBox txtElemScaling;
        private System.Windows.Forms.Label lblElemScaling;
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
        private System.Windows.Forms.Panel pnlElemTagCodeWarn;
        private System.Windows.Forms.PictureBox pbElemTagCodeWarn;
        private System.Windows.Forms.Label lblElemTagCodeWarn;
    }
}
