namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    partial class CtrlItem
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
            gbItem = new GroupBox();
            numDataLen = new NumericUpDown();
            lblDataLen = new Label();
            chkIsArray = new CheckBox();
            chkIsString = new CheckBox();
            pbDataTypeWarning = new PictureBox();
            cbDataType = new ComboBox();
            lblDataType = new Label();
            txtNodeID = new TextBox();
            lblNodeID = new Label();
            txtTagNum = new TextBox();
            lblTagNum = new Label();
            txtTagCode = new TextBox();
            lblTagCode = new Label();
            txtDisplayName = new TextBox();
            lblDisplayName = new Label();
            chkItemActive = new CheckBox();
            gbItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDataLen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbDataTypeWarning).BeginInit();
            SuspendLayout();
            // 
            // gbItem
            // 
            gbItem.Controls.Add(numDataLen);
            gbItem.Controls.Add(lblDataLen);
            gbItem.Controls.Add(chkIsArray);
            gbItem.Controls.Add(chkIsString);
            gbItem.Controls.Add(pbDataTypeWarning);
            gbItem.Controls.Add(cbDataType);
            gbItem.Controls.Add(lblDataType);
            gbItem.Controls.Add(txtNodeID);
            gbItem.Controls.Add(lblNodeID);
            gbItem.Controls.Add(txtTagNum);
            gbItem.Controls.Add(lblTagNum);
            gbItem.Controls.Add(txtTagCode);
            gbItem.Controls.Add(lblTagCode);
            gbItem.Controls.Add(txtDisplayName);
            gbItem.Controls.Add(lblDisplayName);
            gbItem.Controls.Add(chkItemActive);
            gbItem.Dock = DockStyle.Fill;
            gbItem.Location = new Point(0, 0);
            gbItem.Name = "gbItem";
            gbItem.Padding = new Padding(10, 3, 10, 10);
            gbItem.Size = new Size(250, 500);
            gbItem.TabIndex = 0;
            gbItem.TabStop = false;
            gbItem.Text = "Item Parameters";
            // 
            // numDataLen
            // 
            numDataLen.Location = new Point(13, 332);
            numDataLen.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numDataLen.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDataLen.Name = "numDataLen";
            numDataLen.Size = new Size(120, 23);
            numDataLen.TabIndex = 12;
            numDataLen.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numDataLen.ValueChanged += numArrayLen_ValueChanged;
            // 
            // lblDataLen
            // 
            lblDataLen.AutoSize = true;
            lblDataLen.Location = new Point(10, 314);
            lblDataLen.Name = "lblDataLen";
            lblDataLen.Size = new Size(118, 15);
            lblDataLen.TabIndex = 11;
            lblDataLen.Text = "String or array length";
            // 
            // chkIsArray
            // 
            chkIsArray.AutoSize = true;
            chkIsArray.Location = new Point(13, 292);
            chkIsArray.Name = "chkIsArray";
            chkIsArray.Size = new Size(63, 19);
            chkIsArray.TabIndex = 10;
            chkIsArray.Text = "Is array";
            chkIsArray.UseVisualStyleBackColor = true;
            chkIsArray.CheckedChanged += chkIsArray_CheckedChanged;
            // 
            // chkIsString
            // 
            chkIsString.AutoSize = true;
            chkIsString.Enabled = false;
            chkIsString.Location = new Point(13, 267);
            chkIsString.Name = "chkIsString";
            chkIsString.Size = new Size(67, 19);
            chkIsString.TabIndex = 9;
            chkIsString.Text = "Is string";
            chkIsString.UseVisualStyleBackColor = true;
            // 
            // pbDataTypeWarning
            // 
            pbDataTypeWarning.BackColor = SystemColors.Window;
            pbDataTypeWarning.Image = Properties.Resources.warning;
            pbDataTypeWarning.Location = new Point(201, 241);
            pbDataTypeWarning.Name = "pbDataTypeWarning";
            pbDataTypeWarning.Size = new Size(16, 16);
            pbDataTypeWarning.TabIndex = 17;
            pbDataTypeWarning.TabStop = false;
            // 
            // cbDataType
            // 
            cbDataType.FormattingEnabled = true;
            cbDataType.Location = new Point(13, 238);
            cbDataType.Name = "cbDataType";
            cbDataType.Size = new Size(224, 23);
            cbDataType.TabIndex = 14;
            cbDataType.TextChanged += cbDataType_TextChanged;
            // 
            // lblDataType
            // 
            lblDataType.AutoSize = true;
            lblDataType.Location = new Point(10, 220);
            lblDataType.Name = "lblDataType";
            lblDataType.Size = new Size(57, 15);
            lblDataType.TabIndex = 13;
            lblDataType.Text = "Data type";
            // 
            // txtNodeID
            // 
            txtNodeID.Location = new Point(13, 194);
            txtNodeID.Name = "txtNodeID";
            txtNodeID.ReadOnly = true;
            txtNodeID.Size = new Size(224, 23);
            txtNodeID.TabIndex = 8;
            // 
            // lblNodeID
            // 
            lblNodeID.AutoSize = true;
            lblNodeID.Location = new Point(10, 176);
            lblNodeID.Name = "lblNodeID";
            lblNodeID.Size = new Size(50, 15);
            lblNodeID.TabIndex = 7;
            lblNodeID.Text = "Node ID";
            // 
            // txtTagNum
            // 
            txtTagNum.Location = new Point(13, 150);
            txtTagNum.Name = "txtTagNum";
            txtTagNum.ReadOnly = true;
            txtTagNum.Size = new Size(120, 23);
            txtTagNum.TabIndex = 6;
            // 
            // lblTagNum
            // 
            lblTagNum.AutoSize = true;
            lblTagNum.Location = new Point(10, 132);
            lblTagNum.Name = "lblTagNum";
            lblTagNum.Size = new Size(71, 15);
            lblTagNum.TabIndex = 5;
            lblTagNum.Text = "Tag number";
            // 
            // txtTagCode
            // 
            txtTagCode.Location = new Point(13, 106);
            txtTagCode.Name = "txtTagCode";
            txtTagCode.Size = new Size(224, 23);
            txtTagCode.TabIndex = 4;
            txtTagCode.TextChanged += txtTagCode_TextChanged;
            // 
            // lblTagCode
            // 
            lblTagCode.AutoSize = true;
            lblTagCode.Location = new Point(10, 88);
            lblTagCode.Name = "lblTagCode";
            lblTagCode.Size = new Size(55, 15);
            lblTagCode.TabIndex = 3;
            lblTagCode.Text = "Tag code";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(13, 62);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(224, 23);
            txtDisplayName.TabIndex = 2;
            txtDisplayName.TextChanged += txtDisplayName_TextChanged;
            // 
            // lblDisplayName
            // 
            lblDisplayName.AutoSize = true;
            lblDisplayName.Location = new Point(10, 44);
            lblDisplayName.Name = "lblDisplayName";
            lblDisplayName.Size = new Size(78, 15);
            lblDisplayName.TabIndex = 1;
            lblDisplayName.Text = "Display name";
            // 
            // chkItemActive
            // 
            chkItemActive.AutoSize = true;
            chkItemActive.Location = new Point(13, 22);
            chkItemActive.Name = "chkItemActive";
            chkItemActive.Size = new Size(59, 19);
            chkItemActive.TabIndex = 0;
            chkItemActive.Text = "Active";
            chkItemActive.UseVisualStyleBackColor = true;
            chkItemActive.CheckedChanged += chkItemActive_CheckedChanged;
            // 
            // CtrlItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbItem);
            Name = "CtrlItem";
            Size = new Size(250, 500);
            gbItem.ResumeLayout(false);
            gbItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDataLen).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbDataTypeWarning).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbItem;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.CheckBox chkItemActive;
        private System.Windows.Forms.TextBox txtNodeID;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.CheckBox chkIsArray;
        private System.Windows.Forms.Label lblDataLen;
        private System.Windows.Forms.NumericUpDown numDataLen;
        private System.Windows.Forms.Label lblTagNum;
        private System.Windows.Forms.TextBox txtTagNum;
        private TextBox txtTagCode;
        private Label lblTagCode;
        private CheckBox chkIsString;
        private Label lblDataType;
        private PictureBox pbDataTypeWarning;
        private ComboBox cbDataType;
    }
}
