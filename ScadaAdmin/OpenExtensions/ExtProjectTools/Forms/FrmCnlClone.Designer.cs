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
            gbSrcNums = new GroupBox();
            numSrcEndNum = new NumericUpDown();
            lblSrcEndNum = new Label();
            numSrcStartNum = new NumericUpDown();
            lblSrcStartNum = new Label();
            gbDestNums = new GroupBox();
            numDestEndNum = new NumericUpDown();
            lblDestEndNum = new Label();
            numDestStartNum = new NumericUpDown();
            lblDestStartNum = new Label();
            gbOptions = new GroupBox();
            chkUpdateFormulas = new CheckBox();
            cbReplaceDevice = new ComboBox();
            lblReplaceDevice = new Label();
            cbReplaceObj = new ComboBox();
            lblReplaceObj = new Label();
            btnClose = new Button();
            btnClone = new Button();
            gbSrcNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSrcEndNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSrcStartNum).BeginInit();
            gbDestNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDestEndNum).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDestStartNum).BeginInit();
            gbOptions.SuspendLayout();
            SuspendLayout();
            // 
            // gbSrcNums
            // 
            gbSrcNums.Controls.Add(numSrcEndNum);
            gbSrcNums.Controls.Add(lblSrcEndNum);
            gbSrcNums.Controls.Add(numSrcStartNum);
            gbSrcNums.Controls.Add(lblSrcStartNum);
            gbSrcNums.Location = new Point(12, 12);
            gbSrcNums.Name = "gbSrcNums";
            gbSrcNums.Padding = new Padding(10, 3, 10, 10);
            gbSrcNums.Size = new Size(310, 73);
            gbSrcNums.TabIndex = 0;
            gbSrcNums.TabStop = false;
            gbSrcNums.Text = "Source Channel Numbers";
            // 
            // numSrcEndNum
            // 
            numSrcEndNum.Location = new Point(158, 37);
            numSrcEndNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numSrcEndNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSrcEndNum.Name = "numSrcEndNum";
            numSrcEndNum.Size = new Size(139, 23);
            numSrcEndNum.TabIndex = 3;
            numSrcEndNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numSrcEndNum.ValueChanged += num_ValueChanged;
            // 
            // lblSrcEndNum
            // 
            lblSrcEndNum.AutoSize = true;
            lblSrcEndNum.Location = new Point(155, 19);
            lblSrcEndNum.Name = "lblSrcEndNum";
            lblSrcEndNum.Size = new Size(27, 15);
            lblSrcEndNum.TabIndex = 2;
            lblSrcEndNum.Text = "End";
            // 
            // numSrcStartNum
            // 
            numSrcStartNum.Location = new Point(13, 37);
            numSrcStartNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numSrcStartNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSrcStartNum.Name = "numSrcStartNum";
            numSrcStartNum.Size = new Size(139, 23);
            numSrcStartNum.TabIndex = 1;
            numSrcStartNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numSrcStartNum.ValueChanged += num_ValueChanged;
            // 
            // lblSrcStartNum
            // 
            lblSrcStartNum.AutoSize = true;
            lblSrcStartNum.Location = new Point(10, 19);
            lblSrcStartNum.Name = "lblSrcStartNum";
            lblSrcStartNum.Size = new Size(31, 15);
            lblSrcStartNum.TabIndex = 0;
            lblSrcStartNum.Text = "Start";
            // 
            // gbDestNums
            // 
            gbDestNums.Controls.Add(numDestEndNum);
            gbDestNums.Controls.Add(lblDestEndNum);
            gbDestNums.Controls.Add(numDestStartNum);
            gbDestNums.Controls.Add(lblDestStartNum);
            gbDestNums.Location = new Point(12, 91);
            gbDestNums.Name = "gbDestNums";
            gbDestNums.Padding = new Padding(10, 3, 10, 10);
            gbDestNums.Size = new Size(310, 73);
            gbDestNums.TabIndex = 1;
            gbDestNums.TabStop = false;
            gbDestNums.Text = "Destination Channel Numbers";
            // 
            // numDestEndNum
            // 
            numDestEndNum.Location = new Point(158, 37);
            numDestEndNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numDestEndNum.Name = "numDestEndNum";
            numDestEndNum.ReadOnly = true;
            numDestEndNum.Size = new Size(139, 23);
            numDestEndNum.TabIndex = 3;
            // 
            // lblDestEndNum
            // 
            lblDestEndNum.AutoSize = true;
            lblDestEndNum.Location = new Point(155, 19);
            lblDestEndNum.Name = "lblDestEndNum";
            lblDestEndNum.Size = new Size(27, 15);
            lblDestEndNum.TabIndex = 2;
            lblDestEndNum.Text = "End";
            // 
            // numDestStartNum
            // 
            numDestStartNum.Location = new Point(13, 37);
            numDestStartNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numDestStartNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDestStartNum.Name = "numDestStartNum";
            numDestStartNum.Size = new Size(139, 23);
            numDestStartNum.TabIndex = 1;
            numDestStartNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numDestStartNum.ValueChanged += num_ValueChanged;
            // 
            // lblDestStartNum
            // 
            lblDestStartNum.AutoSize = true;
            lblDestStartNum.Location = new Point(10, 19);
            lblDestStartNum.Name = "lblDestStartNum";
            lblDestStartNum.Size = new Size(31, 15);
            lblDestStartNum.TabIndex = 0;
            lblDestStartNum.Text = "Start";
            // 
            // gbOptions
            // 
            gbOptions.Controls.Add(chkUpdateFormulas);
            gbOptions.Controls.Add(cbReplaceDevice);
            gbOptions.Controls.Add(lblReplaceDevice);
            gbOptions.Controls.Add(cbReplaceObj);
            gbOptions.Controls.Add(lblReplaceObj);
            gbOptions.Location = new Point(12, 170);
            gbOptions.Name = "gbOptions";
            gbOptions.Padding = new Padding(10, 3, 10, 10);
            gbOptions.Size = new Size(310, 142);
            gbOptions.TabIndex = 2;
            gbOptions.TabStop = false;
            gbOptions.Text = "Options";
            // 
            // chkUpdateFormulas
            // 
            chkUpdateFormulas.AutoSize = true;
            chkUpdateFormulas.Location = new Point(13, 110);
            chkUpdateFormulas.Name = "chkUpdateFormulas";
            chkUpdateFormulas.Size = new Size(222, 19);
            chkUpdateFormulas.TabIndex = 4;
            chkUpdateFormulas.Text = "Update channel numbers in formulas";
            chkUpdateFormulas.UseVisualStyleBackColor = true;
            // 
            // cbReplaceDevice
            // 
            cbReplaceDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbReplaceDevice.FormattingEnabled = true;
            cbReplaceDevice.Location = new Point(13, 81);
            cbReplaceDevice.Name = "cbReplaceDevice";
            cbReplaceDevice.Size = new Size(284, 23);
            cbReplaceDevice.TabIndex = 3;
            // 
            // lblReplaceDevice
            // 
            lblReplaceDevice.AutoSize = true;
            lblReplaceDevice.Location = new Point(10, 63);
            lblReplaceDevice.Name = "lblReplaceDevice";
            lblReplaceDevice.Size = new Size(85, 15);
            lblReplaceDevice.TabIndex = 2;
            lblReplaceDevice.Text = "Replace device";
            // 
            // cbReplaceObj
            // 
            cbReplaceObj.DropDownStyle = ComboBoxStyle.DropDownList;
            cbReplaceObj.FormattingEnabled = true;
            cbReplaceObj.Location = new Point(13, 37);
            cbReplaceObj.Name = "cbReplaceObj";
            cbReplaceObj.Size = new Size(284, 23);
            cbReplaceObj.TabIndex = 1;
            // 
            // lblReplaceObj
            // 
            lblReplaceObj.AutoSize = true;
            lblReplaceObj.Location = new Point(10, 19);
            lblReplaceObj.Name = "lblReplaceObj";
            lblReplaceObj.Size = new Size(84, 15);
            lblReplaceObj.TabIndex = 0;
            lblReplaceObj.Text = "Replace object";
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(247, 328);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnClone
            // 
            btnClone.Location = new Point(151, 328);
            btnClone.Name = "btnClone";
            btnClone.Size = new Size(90, 23);
            btnClone.TabIndex = 3;
            btnClone.Text = "Clone";
            btnClone.UseVisualStyleBackColor = true;
            btnClone.Click += btnClone_Click;
            // 
            // FrmCnlClone
            // 
            AcceptButton = btnClone;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(334, 363);
            Controls.Add(btnClose);
            Controls.Add(btnClone);
            Controls.Add(gbOptions);
            Controls.Add(gbDestNums);
            Controls.Add(gbSrcNums);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCnlClone";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Clone Channels";
            Load += FrmCnlClone_Load;
            gbSrcNums.ResumeLayout(false);
            gbSrcNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSrcEndNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSrcStartNum).EndInit();
            gbDestNums.ResumeLayout(false);
            gbDestNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDestEndNum).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDestStartNum).EndInit();
            gbOptions.ResumeLayout(false);
            gbOptions.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox gbSrcNums;
        private NumericUpDown numSrcEndNum;
        private Label lblSrcEndNum;
        private NumericUpDown numSrcStartNum;
        private Label lblSrcStartNum;
        private GroupBox gbDestNums;
        private NumericUpDown numDestEndNum;
        private Label lblDestEndNum;
        private NumericUpDown numDestStartNum;
        private Label lblDestStartNum;
        private GroupBox gbOptions;
        private Button btnClose;
        private Button btnClone;
        private ComboBox cbReplaceDevice;
        private Label lblReplaceDevice;
        private ComboBox cbReplaceObj;
        private Label lblReplaceObj;
        private CheckBox chkUpdateFormulas;
    }
}