namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmDeviceCommand
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
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdCode = new System.Windows.Forms.Label();
            this.txtCmdCode = new System.Windows.Forms.TextBox();
            this.rbNumVal = new System.Windows.Forms.RadioButton();
            this.rbStrData = new System.Windows.Forms.RadioButton();
            this.rbHexData = new System.Windows.Forms.RadioButton();
            this.pnlNumVal = new System.Windows.Forms.Panel();
            this.btnOn = new System.Windows.Forms.Button();
            this.btnOff = new System.Windows.Forms.Button();
            this.txtCmdVal = new System.Windows.Forms.TextBox();
            this.txtCmdData = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            this.pnlNumVal.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(9, 9);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(109, 15);
            this.lblCmdNum.TabIndex = 0;
            this.lblCmdNum.Text = "Command number";
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(12, 27);
            this.numCmdNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numCmdNum.Name = "numCmdNum";
            this.numCmdNum.Size = new System.Drawing.Size(150, 23);
            this.numCmdNum.TabIndex = 1;
            // 
            // lblCmdCode
            // 
            this.lblCmdCode.AutoSize = true;
            this.lblCmdCode.Location = new System.Drawing.Point(9, 53);
            this.lblCmdCode.Name = "lblCmdCode";
            this.lblCmdCode.Size = new System.Drawing.Size(93, 15);
            this.lblCmdCode.TabIndex = 2;
            this.lblCmdCode.Text = "Command code";
            // 
            // txtCmdCode
            // 
            this.txtCmdCode.Location = new System.Drawing.Point(12, 71);
            this.txtCmdCode.Name = "txtCmdCode";
            this.txtCmdCode.Size = new System.Drawing.Size(150, 23);
            this.txtCmdCode.TabIndex = 3;
            // 
            // rbNumVal
            // 
            this.rbNumVal.AutoSize = true;
            this.rbNumVal.Location = new System.Drawing.Point(12, 100);
            this.rbNumVal.Name = "rbNumVal";
            this.rbNumVal.Size = new System.Drawing.Size(71, 19);
            this.rbNumVal.TabIndex = 4;
            this.rbNumVal.TabStop = true;
            this.rbNumVal.Text = "Numeric";
            this.rbNumVal.UseVisualStyleBackColor = true;
            this.rbNumVal.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbStrData
            // 
            this.rbStrData.AutoSize = true;
            this.rbStrData.Location = new System.Drawing.Point(112, 100);
            this.rbStrData.Name = "rbStrData";
            this.rbStrData.Size = new System.Drawing.Size(82, 19);
            this.rbStrData.TabIndex = 5;
            this.rbStrData.TabStop = true;
            this.rbStrData.Text = "String data";
            this.rbStrData.UseVisualStyleBackColor = true;
            this.rbStrData.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbHexData
            // 
            this.rbHexData.AutoSize = true;
            this.rbHexData.Location = new System.Drawing.Point(212, 100);
            this.rbHexData.Name = "rbHexData";
            this.rbHexData.Size = new System.Drawing.Size(120, 19);
            this.rbHexData.TabIndex = 6;
            this.rbHexData.TabStop = true;
            this.rbHexData.Text = "Hexadecimal data";
            this.rbHexData.UseVisualStyleBackColor = true;
            this.rbHexData.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // pnlNumVal
            // 
            this.pnlNumVal.Controls.Add(this.btnOn);
            this.pnlNumVal.Controls.Add(this.btnOff);
            this.pnlNumVal.Controls.Add(this.txtCmdVal);
            this.pnlNumVal.Location = new System.Drawing.Point(12, 125);
            this.pnlNumVal.Name = "pnlNumVal";
            this.pnlNumVal.Size = new System.Drawing.Size(360, 50);
            this.pnlNumVal.TabIndex = 7;
            // 
            // btnOn
            // 
            this.btnOn.Location = new System.Drawing.Point(310, 0);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(50, 23);
            this.btnOn.TabIndex = 2;
            this.btnOn.Text = "On";
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnOff
            // 
            this.btnOff.Location = new System.Drawing.Point(254, 0);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(50, 23);
            this.btnOff.TabIndex = 1;
            this.btnOff.Text = "Off";
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // txtCmdVal
            // 
            this.txtCmdVal.Location = new System.Drawing.Point(0, 0);
            this.txtCmdVal.Name = "txtCmdVal";
            this.txtCmdVal.Size = new System.Drawing.Size(248, 23);
            this.txtCmdVal.TabIndex = 0;
            this.txtCmdVal.Text = "0";
            // 
            // txtCmdData
            // 
            this.txtCmdData.AcceptsReturn = true;
            this.txtCmdData.Location = new System.Drawing.Point(12, 125);
            this.txtCmdData.Multiline = true;
            this.txtCmdData.Name = "txtCmdData";
            this.txtCmdData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCmdData.Size = new System.Drawing.Size(360, 150);
            this.txtCmdData.TabIndex = 8;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(216, 291);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 9;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(297, 291);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmDeviceCommand
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(384, 326);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.pnlNumVal);
            this.Controls.Add(this.txtCmdData);
            this.Controls.Add(this.rbHexData);
            this.Controls.Add(this.rbStrData);
            this.Controls.Add(this.rbNumVal);
            this.Controls.Add(this.txtCmdCode);
            this.Controls.Add(this.lblCmdCode);
            this.Controls.Add(this.numCmdNum);
            this.Controls.Add(this.lblCmdNum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeviceCommand";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Command to {0}";
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            this.pnlNumVal.ResumeLayout(false);
            this.pnlNumVal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCmdNum;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.Label lblCmdCode;
        private System.Windows.Forms.TextBox txtCmdCode;
        private System.Windows.Forms.RadioButton rbNumVal;
        private System.Windows.Forms.RadioButton rbStrData;
        private System.Windows.Forms.RadioButton rbHexData;
        private System.Windows.Forms.Panel pnlNumVal;
        private System.Windows.Forms.TextBox txtCmdVal;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.TextBox txtCmdData;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClose;
    }
}