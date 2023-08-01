using Scada.Admin.Extensions.ExtImport.Code;

namespace Scada.Admin.Extensions.ExtImport.Controls
{
    partial class CtrlImport3
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
            txtDevice = new TextBox();
            lblDevice = new Label();
            gbCnlNums = new GroupBox();
            btnReset = new Button();
            btnMap = new Button();
            numEndCnlNum = new NumericUpDown();
            lblEndCnlNum = new Label();
            numStartCnlNum = new NumericUpDown();
            lblStartCnlNum = new Label();
            radioButton1 = new RadioButton();
            txtPathFile = new TextBox();
            btnSelectFile = new Button();
            radioButton2 = new RadioButton();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtSeparator = new TextBox();
            cbBoxSuffix = new ComboBox();
            cbBoxPrefix = new ComboBox();
            gbCnlNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numEndCnlNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStartCnlNum).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // txtDevice
            // 
            txtDevice.Location = new Point(23, 26);
            txtDevice.Margin = new Padding(3, 4, 3, 4);
            txtDevice.Name = "txtDevice";
            txtDevice.ReadOnly = true;
            txtDevice.Size = new Size(428, 27);
            txtDevice.TabIndex = 3;
            // 
            // lblDevice
            // 
            lblDevice.AutoSize = true;
            lblDevice.Location = new Point(18, 0);
            lblDevice.Name = "lblDevice";
            lblDevice.Size = new Size(54, 20);
            lblDevice.TabIndex = 2;
            lblDevice.Text = "Device";
            // 
            // gbCnlNums
            // 
            gbCnlNums.Controls.Add(btnReset);
            gbCnlNums.Controls.Add(btnMap);
            gbCnlNums.Controls.Add(numEndCnlNum);
            gbCnlNums.Controls.Add(lblEndCnlNum);
            gbCnlNums.Controls.Add(numStartCnlNum);
            gbCnlNums.Controls.Add(lblStartCnlNum);
            gbCnlNums.Location = new Point(18, 297);
            gbCnlNums.Margin = new Padding(1);
            gbCnlNums.Name = "gbCnlNums";
            gbCnlNums.Size = new Size(461, 129);
            gbCnlNums.TabIndex = 2;
            gbCnlNums.TabStop = false;
            gbCnlNums.Text = ExtensionPhrases.GbCnlNums;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(330, 87);
            btnReset.Margin = new Padding(3, 4, 3, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(86, 31);
            btnReset.TabIndex = 5;
            btnReset.Text = ExtensionPhrases.BtnReset;
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnMap
            // 
            btnMap.Location = new Point(330, 46);
            btnMap.Margin = new Padding(3, 4, 3, 4);
            btnMap.Name = "btnMap";
            btnMap.Size = new Size(86, 31);
            btnMap.TabIndex = 4;
            btnMap.Text = ExtensionPhrases.BtnMap;
            btnMap.UseVisualStyleBackColor = true;
            btnMap.Click += btnMap_Click;
            // 
            // numEndCnlNum
            // 
            numEndCnlNum.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            numEndCnlNum.Location = new Point(164, 52);
            numEndCnlNum.Margin = new Padding(3, 4, 3, 4);
            numEndCnlNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numEndCnlNum.Name = "numEndCnlNum";
            numEndCnlNum.ReadOnly = true;
            numEndCnlNum.Size = new Size(142, 27);
            numEndCnlNum.TabIndex = 3;
            // 
            // lblEndCnlNum
            // 
            lblEndCnlNum.AutoSize = true;
            lblEndCnlNum.Location = new Point(162, 23);
            lblEndCnlNum.Name = "lblEndCnlNum";
            lblEndCnlNum.Size = new Size(34, 20);
            lblEndCnlNum.TabIndex = 2;
            lblEndCnlNum.Text = ExtensionPhrases.LblEndCnlNum;
            // 
            // numStartCnlNum
            // 
            numStartCnlNum.Location = new Point(15, 52);
            numStartCnlNum.Margin = new Padding(3, 4, 3, 4);
            numStartCnlNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numStartCnlNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numStartCnlNum.Name = "numStartCnlNum";
            numStartCnlNum.Size = new Size(141, 27);
            numStartCnlNum.TabIndex = 1;
            numStartCnlNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numStartCnlNum.ValueChanged += numStartCnlNum_ValueChanged;
            // 
            // lblStartCnlNum
            // 
            lblStartCnlNum.AutoSize = true;
            lblStartCnlNum.Location = new Point(11, 23);
            lblStartCnlNum.Name = "lblStartCnlNum";
            lblStartCnlNum.Size = new Size(40, 20);
            lblStartCnlNum.TabIndex = 0;
            lblStartCnlNum.Text = ExtensionPhrases.LblStartCnlNum;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(6, 26);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(295, 24);
            radioButton1.TabIndex = 9;
            radioButton1.TabStop = true;
            radioButton1.Text = ExtensionPhrases.RdBtnImport1;
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton1.MouseClick += rdoEnableImport_MouseClick;
            // 
            // txtPathFile
            // 
            txtPathFile.Enabled = false;
            txtPathFile.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtPathFile.Location = new Point(27, 53);
            txtPathFile.Margin = new Padding(5, 5, 3, 3);
            txtPathFile.Name = "txtPathFile";
            txtPathFile.ReadOnly = true;
            txtPathFile.Size = new Size(348, 27);
            txtPathFile.TabIndex = 8;
            txtPathFile.TabStop = false;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Enabled = false;
            btnSelectFile.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            btnSelectFile.ForeColor = SystemColors.ActiveCaptionText;
            btnSelectFile.ImageAlign = ContentAlignment.TopCenter;
            btnSelectFile.Location = new Point(379, 50);
            btnSelectFile.Margin = new Padding(3, 4, 3, 4);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(41, 31);
            btnSelectFile.TabIndex = 7;
            btnSelectFile.Text = "...";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(6, 93);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(374, 24);
            radioButton2.TabIndex = 10;
            radioButton2.TabStop = true;
            radioButton2.Text = ExtensionPhrases.RdBtnImport2;
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(txtPathFile);
            groupBox1.Controls.Add(btnSelectFile);
            groupBox1.Location = new Point(14, 59);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(465, 131);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = ExtensionPhrases.GrpImportLbl;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(txtSeparator);
            groupBox2.Controls.Add(cbBoxSuffix);
            groupBox2.Controls.Add(cbBoxPrefix);
            groupBox2.Location = new Point(18, 196);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(461, 97);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = ExtensionPhrases.GrpFormatLbl;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 32);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 14;
            label3.Text = "Prefix ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(193, 32);
            label2.Name = "label2";
            label2.Size = new Size(74, 20);
            label2.TabIndex = 13;
            label2.Text = "Separator";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(285, 32);
            label1.Name = "label1";
            label1.Size = new Size(46, 20);
            label1.TabIndex = 12;
            label1.Text = "Suffix";
            // 
            // txtSeparator
            // 
            txtSeparator.Location = new Point(197, 56);
            txtSeparator.Name = "txtSeparator";
            txtSeparator.Size = new Size(74, 27);
            txtSeparator.TabIndex = 11;
            txtSeparator.Text = "-";
            txtSeparator.TextChanged += txtSeparator_TextChanged;
            // 
            // cbBoxSuffix
            // 
            cbBoxSuffix.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBoxSuffix.FormattingEnabled = true;
            cbBoxSuffix.Location = new Point(289, 55);
            cbBoxSuffix.Name = "cbBoxSuffix";
            cbBoxSuffix.Size = new Size(148, 28);
            cbBoxSuffix.TabIndex = 10;
            cbBoxSuffix.SelectionChangeCommitted += cbBoxSuffix_SelectionChangeCommitted;
            // 
            // cbBoxPrefix
            // 
            cbBoxPrefix.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBoxPrefix.FormattingEnabled = true;
            cbBoxPrefix.Location = new Point(14, 55);
            cbBoxPrefix.Name = "cbBoxPrefix";
            cbBoxPrefix.Size = new Size(169, 28);
            cbBoxPrefix.TabIndex = 9;
            cbBoxPrefix.SelectionChangeCommitted += cbBoxPrefix_SelectionChangeCommitted;
            // 
            // CtrlImport3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(gbCnlNums);
            Controls.Add(txtDevice);
            Controls.Add(lblDevice);
            Margin = new Padding(1);
            Name = "CtrlImport3";
            Size = new Size(497, 462);
            gbCnlNums.ResumeLayout(false);
            gbCnlNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numEndCnlNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStartCnlNum).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtDevice;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.GroupBox gbCnlNums;
        private System.Windows.Forms.NumericUpDown numEndCnlNum;
        private System.Windows.Forms.Label lblEndCnlNum;
        private System.Windows.Forms.NumericUpDown numStartCnlNum;
        private System.Windows.Forms.Label lblStartCnlNum;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnMap;
        private RadioButton radioButton1;
        private TextBox txtPathFile;
        private Button btnSelectFile;
        private RadioButton radioButton2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtSeparator;
        private ComboBox cbBoxSuffix;
        private ComboBox cbBoxPrefix;
    }
}
