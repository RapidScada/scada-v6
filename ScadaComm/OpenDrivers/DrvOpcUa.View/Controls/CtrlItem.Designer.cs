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
            this.gbItem = new System.Windows.Forms.GroupBox();
            this.numDataLen = new System.Windows.Forms.NumericUpDown();
            this.lblDataLen = new System.Windows.Forms.Label();
            this.chkIsArray = new System.Windows.Forms.CheckBox();
            this.chkIsString = new System.Windows.Forms.CheckBox();
            this.pbDataTypeWarning = new System.Windows.Forms.PictureBox();
            this.cbDataType = new System.Windows.Forms.ComboBox();
            this.lblDataType = new System.Windows.Forms.Label();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.txtTagNum = new System.Windows.Forms.TextBox();
            this.lblTagNum = new System.Windows.Forms.Label();
            this.txtTagCode = new System.Windows.Forms.TextBox();
            this.lblTagCode = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.chkItemActive = new System.Windows.Forms.CheckBox();
            this.gbItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDataTypeWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // gbItem
            // 
            this.gbItem.Controls.Add(this.numDataLen);
            this.gbItem.Controls.Add(this.lblDataLen);
            this.gbItem.Controls.Add(this.chkIsArray);
            this.gbItem.Controls.Add(this.chkIsString);
            this.gbItem.Controls.Add(this.pbDataTypeWarning);
            this.gbItem.Controls.Add(this.cbDataType);
            this.gbItem.Controls.Add(this.lblDataType);
            this.gbItem.Controls.Add(this.txtNodeID);
            this.gbItem.Controls.Add(this.lblNodeID);
            this.gbItem.Controls.Add(this.txtTagNum);
            this.gbItem.Controls.Add(this.lblTagNum);
            this.gbItem.Controls.Add(this.txtTagCode);
            this.gbItem.Controls.Add(this.lblTagCode);
            this.gbItem.Controls.Add(this.txtDisplayName);
            this.gbItem.Controls.Add(this.lblDisplayName);
            this.gbItem.Controls.Add(this.chkItemActive);
            this.gbItem.Location = new System.Drawing.Point(0, 0);
            this.gbItem.Name = "gbItem";
            this.gbItem.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbItem.Size = new System.Drawing.Size(250, 500);
            this.gbItem.TabIndex = 0;
            this.gbItem.TabStop = false;
            this.gbItem.Text = "Item Parameters";
            // 
            // numDataLen
            // 
            this.numDataLen.Location = new System.Drawing.Point(13, 332);
            this.numDataLen.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDataLen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDataLen.Name = "numDataLen";
            this.numDataLen.Size = new System.Drawing.Size(120, 23);
            this.numDataLen.TabIndex = 12;
            this.numDataLen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDataLen.ValueChanged += new System.EventHandler(this.numArrayLen_ValueChanged);
            // 
            // lblDataLen
            // 
            this.lblDataLen.AutoSize = true;
            this.lblDataLen.Location = new System.Drawing.Point(10, 314);
            this.lblDataLen.Name = "lblDataLen";
            this.lblDataLen.Size = new System.Drawing.Size(118, 15);
            this.lblDataLen.TabIndex = 11;
            this.lblDataLen.Text = "String or array length";
            // 
            // chkIsArray
            // 
            this.chkIsArray.AutoSize = true;
            this.chkIsArray.Location = new System.Drawing.Point(13, 292);
            this.chkIsArray.Name = "chkIsArray";
            this.chkIsArray.Size = new System.Drawing.Size(63, 19);
            this.chkIsArray.TabIndex = 10;
            this.chkIsArray.Text = "Is array";
            this.chkIsArray.UseVisualStyleBackColor = true;
            this.chkIsArray.CheckedChanged += new System.EventHandler(this.chkIsArray_CheckedChanged);
            // 
            // chkIsString
            // 
            this.chkIsString.AutoSize = true;
            this.chkIsString.Enabled = false;
            this.chkIsString.Location = new System.Drawing.Point(13, 267);
            this.chkIsString.Name = "chkIsString";
            this.chkIsString.Size = new System.Drawing.Size(67, 19);
            this.chkIsString.TabIndex = 9;
            this.chkIsString.Text = "Is string";
            this.chkIsString.UseVisualStyleBackColor = true;
            // 
            // pbDataTypeWarning
            // 
            this.pbDataTypeWarning.BackColor = System.Drawing.SystemColors.Window;
            this.pbDataTypeWarning.Image = global::Scada.Comm.Drivers.DrvOpcUa.View.Properties.Resources.warning;
            this.pbDataTypeWarning.Location = new System.Drawing.Point(201, 241);
            this.pbDataTypeWarning.Name = "pbDataTypeWarning";
            this.pbDataTypeWarning.Size = new System.Drawing.Size(16, 16);
            this.pbDataTypeWarning.TabIndex = 17;
            this.pbDataTypeWarning.TabStop = false;
            // 
            // cbDataType
            // 
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Location = new System.Drawing.Point(13, 238);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Size = new System.Drawing.Size(224, 23);
            this.cbDataType.TabIndex = 14;
            this.cbDataType.TextChanged += new System.EventHandler(this.cbDataType_TextChanged);
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(10, 220);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(57, 15);
            this.lblDataType.TabIndex = 13;
            this.lblDataType.Text = "Data type";
            // 
            // txtNodeID
            // 
            this.txtNodeID.Location = new System.Drawing.Point(13, 194);
            this.txtNodeID.Name = "txtNodeID";
            this.txtNodeID.ReadOnly = true;
            this.txtNodeID.Size = new System.Drawing.Size(224, 23);
            this.txtNodeID.TabIndex = 8;
            // 
            // lblNodeID
            // 
            this.lblNodeID.AutoSize = true;
            this.lblNodeID.Location = new System.Drawing.Point(10, 176);
            this.lblNodeID.Name = "lblNodeID";
            this.lblNodeID.Size = new System.Drawing.Size(50, 15);
            this.lblNodeID.TabIndex = 7;
            this.lblNodeID.Text = "Node ID";
            // 
            // txtTagNum
            // 
            this.txtTagNum.Location = new System.Drawing.Point(13, 150);
            this.txtTagNum.Name = "txtTagNum";
            this.txtTagNum.ReadOnly = true;
            this.txtTagNum.Size = new System.Drawing.Size(120, 23);
            this.txtTagNum.TabIndex = 6;
            // 
            // lblTagNum
            // 
            this.lblTagNum.AutoSize = true;
            this.lblTagNum.Location = new System.Drawing.Point(10, 132);
            this.lblTagNum.Name = "lblTagNum";
            this.lblTagNum.Size = new System.Drawing.Size(70, 15);
            this.lblTagNum.TabIndex = 5;
            this.lblTagNum.Text = "Tag number";
            // 
            // txtTagCode
            // 
            this.txtTagCode.Location = new System.Drawing.Point(13, 106);
            this.txtTagCode.Name = "txtTagCode";
            this.txtTagCode.Size = new System.Drawing.Size(224, 23);
            this.txtTagCode.TabIndex = 4;
            this.txtTagCode.TextChanged += new System.EventHandler(this.txtTagCode_TextChanged);
            // 
            // lblTagCode
            // 
            this.lblTagCode.AutoSize = true;
            this.lblTagCode.Location = new System.Drawing.Point(10, 88);
            this.lblTagCode.Name = "lblTagCode";
            this.lblTagCode.Size = new System.Drawing.Size(54, 15);
            this.lblTagCode.TabIndex = 3;
            this.lblTagCode.Text = "Tag code";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(13, 62);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(224, 23);
            this.txtDisplayName.TabIndex = 2;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(10, 44);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(78, 15);
            this.lblDisplayName.TabIndex = 1;
            this.lblDisplayName.Text = "Display name";
            // 
            // chkItemActive
            // 
            this.chkItemActive.AutoSize = true;
            this.chkItemActive.Location = new System.Drawing.Point(13, 22);
            this.chkItemActive.Name = "chkItemActive";
            this.chkItemActive.Size = new System.Drawing.Size(59, 19);
            this.chkItemActive.TabIndex = 0;
            this.chkItemActive.Text = "Active";
            this.chkItemActive.UseVisualStyleBackColor = true;
            this.chkItemActive.CheckedChanged += new System.EventHandler(this.chkItemActive_CheckedChanged);
            // 
            // CtrlItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbItem);
            this.Name = "CtrlItem";
            this.Size = new System.Drawing.Size(250, 500);
            this.gbItem.ResumeLayout(false);
            this.gbItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataLen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDataTypeWarning)).EndInit();
            this.ResumeLayout(false);

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
