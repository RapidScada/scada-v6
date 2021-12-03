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
            this.txtTagNum = new System.Windows.Forms.TextBox();
            this.lblTagNum = new System.Windows.Forms.Label();
            this.numArrayLen = new System.Windows.Forms.NumericUpDown();
            this.lblArrayLen = new System.Windows.Forms.Label();
            this.chkIsArray = new System.Windows.Forms.CheckBox();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.chkItemActive = new System.Windows.Forms.CheckBox();
            this.lblTagCode = new System.Windows.Forms.Label();
            this.txtTagCode = new System.Windows.Forms.TextBox();
            this.gbItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArrayLen)).BeginInit();
            this.SuspendLayout();
            // 
            // gbItem
            // 
            this.gbItem.Controls.Add(this.numArrayLen);
            this.gbItem.Controls.Add(this.lblArrayLen);
            this.gbItem.Controls.Add(this.chkIsArray);
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
            // numArrayLen
            // 
            this.numArrayLen.Location = new System.Drawing.Point(13, 263);
            this.numArrayLen.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numArrayLen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArrayLen.Name = "numArrayLen";
            this.numArrayLen.Size = new System.Drawing.Size(120, 23);
            this.numArrayLen.TabIndex = 11;
            this.numArrayLen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArrayLen.ValueChanged += new System.EventHandler(this.numArrayLen_ValueChanged);
            // 
            // lblArrayLen
            // 
            this.lblArrayLen.AutoSize = true;
            this.lblArrayLen.Location = new System.Drawing.Point(10, 245);
            this.lblArrayLen.Name = "lblArrayLen";
            this.lblArrayLen.Size = new System.Drawing.Size(72, 15);
            this.lblArrayLen.TabIndex = 10;
            this.lblArrayLen.Text = "Array length";
            // 
            // chkIsArray
            // 
            this.chkIsArray.AutoSize = true;
            this.chkIsArray.Location = new System.Drawing.Point(13, 223);
            this.chkIsArray.Name = "chkIsArray";
            this.chkIsArray.Size = new System.Drawing.Size(63, 19);
            this.chkIsArray.TabIndex = 9;
            this.chkIsArray.Text = "Is array";
            this.chkIsArray.UseVisualStyleBackColor = true;
            this.chkIsArray.CheckedChanged += new System.EventHandler(this.chkIsArray_CheckedChanged);
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
            // lblTagCode
            // 
            this.lblTagCode.AutoSize = true;
            this.lblTagCode.Location = new System.Drawing.Point(10, 88);
            this.lblTagCode.Name = "lblTagCode";
            this.lblTagCode.Size = new System.Drawing.Size(54, 15);
            this.lblTagCode.TabIndex = 3;
            this.lblTagCode.Text = "Tag code";
            // 
            // txtTagCode
            // 
            this.txtTagCode.Location = new System.Drawing.Point(13, 106);
            this.txtTagCode.Name = "txtTagCode";
            this.txtTagCode.Size = new System.Drawing.Size(224, 23);
            this.txtTagCode.TabIndex = 4;
            this.txtTagCode.TextChanged += new System.EventHandler(this.txtTagCode_TextChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.numArrayLen)).EndInit();
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
        private System.Windows.Forms.Label lblArrayLen;
        private System.Windows.Forms.NumericUpDown numArrayLen;
        private System.Windows.Forms.Label lblTagNum;
        private System.Windows.Forms.TextBox txtTagNum;
        private TextBox txtTagCode;
        private Label lblTagCode;
    }
}
