namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    partial class FrmCnlClone
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
            this.gbSrcNums = new System.Windows.Forms.GroupBox();
            this.numSrcEndNum = new System.Windows.Forms.NumericUpDown();
            this.lblSrcEndNum = new System.Windows.Forms.Label();
            this.numSrcStartNum = new System.Windows.Forms.NumericUpDown();
            this.lblSrcStartNum = new System.Windows.Forms.Label();
            this.gbDestNums = new System.Windows.Forms.GroupBox();
            this.numDestEndNum = new System.Windows.Forms.NumericUpDown();
            this.lblDestEndNum = new System.Windows.Forms.Label();
            this.numDestStartNum = new System.Windows.Forms.NumericUpDown();
            this.lblDestStartNum = new System.Windows.Forms.Label();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.chkUpdateFormulas = new System.Windows.Forms.CheckBox();
            this.cbReplaceDevice = new System.Windows.Forms.ComboBox();
            this.lblReplaceDevice = new System.Windows.Forms.Label();
            this.cbReplaceObj = new System.Windows.Forms.ComboBox();
            this.lblReplaceObj = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClone = new System.Windows.Forms.Button();
            this.gbSrcNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcEndNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcStartNum)).BeginInit();
            this.gbDestNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDestEndNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestStartNum)).BeginInit();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSrcNums
            // 
            this.gbSrcNums.Controls.Add(this.numSrcEndNum);
            this.gbSrcNums.Controls.Add(this.lblSrcEndNum);
            this.gbSrcNums.Controls.Add(this.numSrcStartNum);
            this.gbSrcNums.Controls.Add(this.lblSrcStartNum);
            this.gbSrcNums.Location = new System.Drawing.Point(12, 12);
            this.gbSrcNums.Name = "gbSrcNums";
            this.gbSrcNums.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbSrcNums.Size = new System.Drawing.Size(310, 73);
            this.gbSrcNums.TabIndex = 0;
            this.gbSrcNums.TabStop = false;
            this.gbSrcNums.Text = "Source Channel Numbers";
            // 
            // numSrcEndNum
            // 
            this.numSrcEndNum.Location = new System.Drawing.Point(158, 37);
            this.numSrcEndNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numSrcEndNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcEndNum.Name = "numSrcEndNum";
            this.numSrcEndNum.Size = new System.Drawing.Size(139, 23);
            this.numSrcEndNum.TabIndex = 3;
            this.numSrcEndNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcEndNum.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblSrcEndNum
            // 
            this.lblSrcEndNum.AutoSize = true;
            this.lblSrcEndNum.Location = new System.Drawing.Point(155, 19);
            this.lblSrcEndNum.Name = "lblSrcEndNum";
            this.lblSrcEndNum.Size = new System.Drawing.Size(27, 15);
            this.lblSrcEndNum.TabIndex = 2;
            this.lblSrcEndNum.Text = "End";
            // 
            // numSrcStartNum
            // 
            this.numSrcStartNum.Location = new System.Drawing.Point(13, 37);
            this.numSrcStartNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numSrcStartNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcStartNum.Name = "numSrcStartNum";
            this.numSrcStartNum.Size = new System.Drawing.Size(139, 23);
            this.numSrcStartNum.TabIndex = 1;
            this.numSrcStartNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSrcStartNum.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblSrcStartNum
            // 
            this.lblSrcStartNum.AutoSize = true;
            this.lblSrcStartNum.Location = new System.Drawing.Point(10, 19);
            this.lblSrcStartNum.Name = "lblSrcStartNum";
            this.lblSrcStartNum.Size = new System.Drawing.Size(31, 15);
            this.lblSrcStartNum.TabIndex = 0;
            this.lblSrcStartNum.Text = "Start";
            // 
            // gbDestNums
            // 
            this.gbDestNums.Controls.Add(this.numDestEndNum);
            this.gbDestNums.Controls.Add(this.lblDestEndNum);
            this.gbDestNums.Controls.Add(this.numDestStartNum);
            this.gbDestNums.Controls.Add(this.lblDestStartNum);
            this.gbDestNums.Location = new System.Drawing.Point(12, 91);
            this.gbDestNums.Name = "gbDestNums";
            this.gbDestNums.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDestNums.Size = new System.Drawing.Size(310, 73);
            this.gbDestNums.TabIndex = 1;
            this.gbDestNums.TabStop = false;
            this.gbDestNums.Text = "Destination Channel Numbers";
            // 
            // numDestEndNum
            // 
            this.numDestEndNum.Location = new System.Drawing.Point(158, 37);
            this.numDestEndNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDestEndNum.Name = "numDestEndNum";
            this.numDestEndNum.ReadOnly = true;
            this.numDestEndNum.Size = new System.Drawing.Size(139, 23);
            this.numDestEndNum.TabIndex = 3;
            // 
            // lblDestEndNum
            // 
            this.lblDestEndNum.AutoSize = true;
            this.lblDestEndNum.Location = new System.Drawing.Point(155, 19);
            this.lblDestEndNum.Name = "lblDestEndNum";
            this.lblDestEndNum.Size = new System.Drawing.Size(27, 15);
            this.lblDestEndNum.TabIndex = 2;
            this.lblDestEndNum.Text = "End";
            // 
            // numDestStartNum
            // 
            this.numDestStartNum.Location = new System.Drawing.Point(13, 37);
            this.numDestStartNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numDestStartNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDestStartNum.Name = "numDestStartNum";
            this.numDestStartNum.Size = new System.Drawing.Size(139, 23);
            this.numDestStartNum.TabIndex = 1;
            this.numDestStartNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDestStartNum.ValueChanged += new System.EventHandler(this.num_ValueChanged);
            // 
            // lblDestStartNum
            // 
            this.lblDestStartNum.AutoSize = true;
            this.lblDestStartNum.Location = new System.Drawing.Point(10, 19);
            this.lblDestStartNum.Name = "lblDestStartNum";
            this.lblDestStartNum.Size = new System.Drawing.Size(31, 15);
            this.lblDestStartNum.TabIndex = 0;
            this.lblDestStartNum.Text = "Start";
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.chkUpdateFormulas);
            this.gbOptions.Controls.Add(this.cbReplaceDevice);
            this.gbOptions.Controls.Add(this.lblReplaceDevice);
            this.gbOptions.Controls.Add(this.cbReplaceObj);
            this.gbOptions.Controls.Add(this.lblReplaceObj);
            this.gbOptions.Location = new System.Drawing.Point(12, 170);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(310, 98);
            this.gbOptions.TabIndex = 2;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // chkUpdateFormulas
            // 
            this.chkUpdateFormulas.AutoSize = true;
            this.chkUpdateFormulas.Location = new System.Drawing.Point(13, 66);
            this.chkUpdateFormulas.Name = "chkUpdateFormulas";
            this.chkUpdateFormulas.Size = new System.Drawing.Size(222, 19);
            this.chkUpdateFormulas.TabIndex = 4;
            this.chkUpdateFormulas.Text = "Update channel numbers in formulas";
            this.chkUpdateFormulas.UseVisualStyleBackColor = true;
            // 
            // cbReplaceDevice
            // 
            this.cbReplaceDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplaceDevice.FormattingEnabled = true;
            this.cbReplaceDevice.Location = new System.Drawing.Point(158, 37);
            this.cbReplaceDevice.Name = "cbReplaceDevice";
            this.cbReplaceDevice.Size = new System.Drawing.Size(139, 23);
            this.cbReplaceDevice.TabIndex = 3;
            // 
            // lblReplaceDevice
            // 
            this.lblReplaceDevice.AutoSize = true;
            this.lblReplaceDevice.Location = new System.Drawing.Point(155, 19);
            this.lblReplaceDevice.Name = "lblReplaceDevice";
            this.lblReplaceDevice.Size = new System.Drawing.Size(85, 15);
            this.lblReplaceDevice.TabIndex = 2;
            this.lblReplaceDevice.Text = "Replace device";
            // 
            // cbReplaceObj
            // 
            this.cbReplaceObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReplaceObj.FormattingEnabled = true;
            this.cbReplaceObj.Location = new System.Drawing.Point(13, 37);
            this.cbReplaceObj.Name = "cbReplaceObj";
            this.cbReplaceObj.Size = new System.Drawing.Size(139, 23);
            this.cbReplaceObj.TabIndex = 1;
            // 
            // lblReplaceObj
            // 
            this.lblReplaceObj.AutoSize = true;
            this.lblReplaceObj.Location = new System.Drawing.Point(10, 19);
            this.lblReplaceObj.Name = "lblReplaceObj";
            this.lblReplaceObj.Size = new System.Drawing.Size(84, 15);
            this.lblReplaceObj.TabIndex = 0;
            this.lblReplaceObj.Text = "Replace object";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(247, 284);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnClone
            // 
            this.btnClone.Location = new System.Drawing.Point(151, 284);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(90, 23);
            this.btnClone.TabIndex = 3;
            this.btnClone.Text = "Clone";
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // FrmCnlClone
            // 
            this.AcceptButton = this.btnClone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(334, 319);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClone);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbDestNums);
            this.Controls.Add(this.gbSrcNums);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCnlClone";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Clone Channels";
            this.Load += new System.EventHandler(this.FrmCnlClone_Load);
            this.gbSrcNums.ResumeLayout(false);
            this.gbSrcNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcEndNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcStartNum)).EndInit();
            this.gbDestNums.ResumeLayout(false);
            this.gbDestNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDestEndNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDestStartNum)).EndInit();
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbSrcNums;
        private System.Windows.Forms.NumericUpDown numSrcEndNum;
        private System.Windows.Forms.Label lblSrcEndNum;
        private System.Windows.Forms.NumericUpDown numSrcStartNum;
        private System.Windows.Forms.Label lblSrcStartNum;
        private System.Windows.Forms.GroupBox gbDestNums;
        private System.Windows.Forms.NumericUpDown numDestEndNum;
        private System.Windows.Forms.Label lblDestEndNum;
        private System.Windows.Forms.NumericUpDown numDestStartNum;
        private System.Windows.Forms.Label lblDestStartNum;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.ComboBox cbReplaceDevice;
        private System.Windows.Forms.Label lblReplaceDevice;
        private System.Windows.Forms.ComboBox cbReplaceObj;
        private System.Windows.Forms.Label lblReplaceObj;
        private System.Windows.Forms.CheckBox chkUpdateFormulas;
    }
}