namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View.Controls
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
            gbElem = new GroupBox();
            rbBool = new RadioButton();
            rbDouble = new RadioButton();
            rbFloat = new RadioButton();
            rbLong = new RadioButton();
            rbULong = new RadioButton();
            rbInt = new RadioButton();
            rbUInt = new RadioButton();
            rbShort = new RadioButton();
            rbUShort = new RadioButton();
            lblElemType = new Label();
            txtElemTagNum = new TextBox();
            lblElemTagNum = new Label();
            pnlElemTagCodeWarn = new Panel();
            lblElemTagCodeWarn = new Label();
            pbElemTagCodeWarn = new PictureBox();
            txtElemTagCode = new TextBox();
            lblElemTagCode = new Label();
            txtElemName = new TextBox();
            lblElemName = new Label();
            gbElem.SuspendLayout();
            pnlElemTagCodeWarn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbElemTagCodeWarn).BeginInit();
            SuspendLayout();
            // 
            // gbElem
            // 
            gbElem.Controls.Add(rbBool);
            gbElem.Controls.Add(rbDouble);
            gbElem.Controls.Add(rbFloat);
            gbElem.Controls.Add(rbLong);
            gbElem.Controls.Add(rbULong);
            gbElem.Controls.Add(rbInt);
            gbElem.Controls.Add(rbUInt);
            gbElem.Controls.Add(rbShort);
            gbElem.Controls.Add(rbUShort);
            gbElem.Controls.Add(lblElemType);
            gbElem.Controls.Add(txtElemTagNum);
            gbElem.Controls.Add(lblElemTagNum);
            gbElem.Controls.Add(pnlElemTagCodeWarn);
            gbElem.Controls.Add(txtElemTagCode);
            gbElem.Controls.Add(lblElemTagCode);
            gbElem.Controls.Add(txtElemName);
            gbElem.Controls.Add(lblElemName);
            gbElem.Location = new Point(0, 0);
            gbElem.Name = "gbElem";
            gbElem.Padding = new Padding(10, 3, 10, 11);
            gbElem.Size = new Size(300, 448);
            gbElem.TabIndex = 0;
            gbElem.TabStop = false;
            gbElem.Text = "Element Parameters";
            // 
            // rbBool
            // 
            rbBool.AutoSize = true;
            rbBool.Location = new Point(13, 305);
            rbBool.Name = "rbBool";
            rbBool.Size = new Size(53, 21);
            rbBool.TabIndex = 18;
            rbBool.TabStop = true;
            rbBool.Text = "bool";
            rbBool.UseVisualStyleBackColor = true;
            rbBool.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbDouble
            // 
            rbDouble.AutoSize = true;
            rbDouble.Location = new Point(153, 277);
            rbDouble.Name = "rbDouble";
            rbDouble.Size = new Size(67, 21);
            rbDouble.TabIndex = 17;
            rbDouble.TabStop = true;
            rbDouble.Text = "double";
            rbDouble.UseVisualStyleBackColor = true;
            rbDouble.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbFloat
            // 
            rbFloat.AutoSize = true;
            rbFloat.Location = new Point(13, 277);
            rbFloat.Name = "rbFloat";
            rbFloat.Size = new Size(52, 21);
            rbFloat.TabIndex = 16;
            rbFloat.TabStop = true;
            rbFloat.Text = "float";
            rbFloat.UseVisualStyleBackColor = true;
            rbFloat.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbLong
            // 
            rbLong.AutoSize = true;
            rbLong.Location = new Point(153, 248);
            rbLong.Name = "rbLong";
            rbLong.Size = new Size(52, 21);
            rbLong.TabIndex = 15;
            rbLong.TabStop = true;
            rbLong.Text = "long";
            rbLong.UseVisualStyleBackColor = true;
            rbLong.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbULong
            // 
            rbULong.AutoSize = true;
            rbULong.Location = new Point(13, 248);
            rbULong.Name = "rbULong";
            rbULong.Size = new Size(59, 21);
            rbULong.TabIndex = 14;
            rbULong.TabStop = true;
            rbULong.Text = "ulong";
            rbULong.UseVisualStyleBackColor = true;
            rbULong.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbInt
            // 
            rbInt.AutoSize = true;
            rbInt.Location = new Point(153, 220);
            rbInt.Name = "rbInt";
            rbInt.Size = new Size(40, 21);
            rbInt.TabIndex = 13;
            rbInt.TabStop = true;
            rbInt.Text = "int";
            rbInt.UseVisualStyleBackColor = true;
            rbInt.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbUInt
            // 
            rbUInt.AutoSize = true;
            rbUInt.Location = new Point(13, 220);
            rbUInt.Name = "rbUInt";
            rbUInt.Size = new Size(47, 21);
            rbUInt.TabIndex = 12;
            rbUInt.TabStop = true;
            rbUInt.Text = "uint";
            rbUInt.UseVisualStyleBackColor = true;
            rbUInt.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbShort
            // 
            rbShort.AutoSize = true;
            rbShort.Location = new Point(153, 192);
            rbShort.Name = "rbShort";
            rbShort.Size = new Size(56, 21);
            rbShort.TabIndex = 11;
            rbShort.TabStop = true;
            rbShort.Text = "short";
            rbShort.UseVisualStyleBackColor = true;
            rbShort.CheckedChanged += rbType_CheckedChanged;
            // 
            // rbUShort
            // 
            rbUShort.AutoSize = true;
            rbUShort.Location = new Point(13, 192);
            rbUShort.Name = "rbUShort";
            rbUShort.Size = new Size(63, 21);
            rbUShort.TabIndex = 10;
            rbUShort.TabStop = true;
            rbUShort.Text = "ushort";
            rbUShort.UseVisualStyleBackColor = true;
            rbUShort.CheckedChanged += rbType_CheckedChanged;
            // 
            // lblElemType
            // 
            lblElemType.AutoSize = true;
            lblElemType.Location = new Point(10, 171);
            lblElemType.Name = "lblElemType";
            lblElemType.Size = new Size(39, 17);
            lblElemType.TabIndex = 9;
            lblElemType.Text = "Type:";
            // 
            // txtElemTagNum
            // 
            txtElemTagNum.Location = new Point(13, 142);
            txtElemTagNum.Name = "txtElemTagNum";
            txtElemTagNum.ReadOnly = true;
            txtElemTagNum.Size = new Size(274, 23);
            txtElemTagNum.TabIndex = 6;
            // 
            // lblElemTagNum
            // 
            lblElemTagNum.AutoSize = true;
            lblElemTagNum.Location = new Point(10, 121);
            lblElemTagNum.Name = "lblElemTagNum";
            lblElemTagNum.Size = new Size(79, 17);
            lblElemTagNum.TabIndex = 5;
            lblElemTagNum.Text = "Tag number";
            // 
            // pnlElemTagCodeWarn
            // 
            pnlElemTagCodeWarn.Controls.Add(lblElemTagCodeWarn);
            pnlElemTagCodeWarn.Controls.Add(pbElemTagCodeWarn);
            pnlElemTagCodeWarn.Location = new Point(153, 92);
            pnlElemTagCodeWarn.Name = "pnlElemTagCodeWarn";
            pnlElemTagCodeWarn.Size = new Size(134, 26);
            pnlElemTagCodeWarn.TabIndex = 4;
            // 
            // lblElemTagCodeWarn
            // 
            lblElemTagCodeWarn.AutoSize = true;
            lblElemTagCodeWarn.ForeColor = Color.Red;
            lblElemTagCodeWarn.Location = new Point(19, 5);
            lblElemTagCodeWarn.Name = "lblElemTagCodeWarn";
            lblElemTagCodeWarn.Size = new Size(79, 17);
            lblElemTagCodeWarn.TabIndex = 0;
            lblElemTagCodeWarn.Text = "Fill out code";
            // 
            // pbElemTagCodeWarn
            // 
            pbElemTagCodeWarn.Image = Properties.Resources.warning;
            pbElemTagCodeWarn.Location = new Point(0, 3);
            pbElemTagCodeWarn.Name = "pbElemTagCodeWarn";
            pbElemTagCodeWarn.Size = new Size(16, 18);
            pbElemTagCodeWarn.TabIndex = 0;
            pbElemTagCodeWarn.TabStop = false;
            // 
            // txtElemTagCode
            // 
            txtElemTagCode.Location = new Point(13, 92);
            txtElemTagCode.Name = "txtElemTagCode";
            txtElemTagCode.Size = new Size(134, 23);
            txtElemTagCode.TabIndex = 3;
            txtElemTagCode.TextChanged += txtElemTagCode_TextChanged;
            // 
            // lblElemTagCode
            // 
            lblElemTagCode.AutoSize = true;
            lblElemTagCode.Location = new Point(10, 71);
            lblElemTagCode.Name = "lblElemTagCode";
            lblElemTagCode.Size = new Size(63, 17);
            lblElemTagCode.TabIndex = 2;
            lblElemTagCode.Text = "Tag code";
            // 
            // txtElemName
            // 
            txtElemName.Location = new Point(13, 42);
            txtElemName.Name = "txtElemName";
            txtElemName.Size = new Size(274, 23);
            txtElemName.TabIndex = 1;
            txtElemName.TextChanged += txtElemName_TextChanged;
            // 
            // lblElemName
            // 
            lblElemName.AutoSize = true;
            lblElemName.Location = new Point(10, 22);
            lblElemName.Name = "lblElemName";
            lblElemName.Size = new Size(43, 17);
            lblElemName.TabIndex = 0;
            lblElemName.Text = "Name";
            // 
            // CtrlElem
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbElem);
            Name = "CtrlElem";
            Size = new Size(300, 448);
            gbElem.ResumeLayout(false);
            gbElem.PerformLayout();
            pnlElemTagCodeWarn.ResumeLayout(false);
            pnlElemTagCodeWarn.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbElemTagCodeWarn).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbElem;
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
        private System.Windows.Forms.TextBox txtElemTagNum;
        private System.Windows.Forms.Label lblElemTagNum;
        private System.Windows.Forms.TextBox txtElemName;
        private System.Windows.Forms.Label lblElemName;
        private System.Windows.Forms.Label lblElemTagCode;
        private System.Windows.Forms.TextBox txtElemTagCode;
        private System.Windows.Forms.Panel pnlElemTagCodeWarn;
        private System.Windows.Forms.PictureBox pbElemTagCodeWarn;
        private System.Windows.Forms.Label lblElemTagCodeWarn;
    }
}
