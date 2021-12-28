namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    partial class CtrlCnlCreate3
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
            this.txtDevice = new System.Windows.Forms.TextBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.gbCnlNums = new System.Windows.Forms.GroupBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.numEndCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblEndCnlNum = new System.Windows.Forms.Label();
            this.numStartCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblStartCnlNum = new System.Windows.Forms.Label();
            this.gbCnlNums.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartCnlNum)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDevice
            // 
            this.txtDevice.Location = new System.Drawing.Point(0, 18);
            this.txtDevice.Name = "txtDevice";
            this.txtDevice.ReadOnly = true;
            this.txtDevice.Size = new System.Drawing.Size(360, 23);
            this.txtDevice.TabIndex = 3;
            // 
            // lblDevice
            // 
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(-3, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 15);
            this.lblDevice.TabIndex = 2;
            this.lblDevice.Text = "Device";
            // 
            // gbCnlNums
            // 
            this.gbCnlNums.Controls.Add(this.btnReset);
            this.gbCnlNums.Controls.Add(this.btnMap);
            this.gbCnlNums.Controls.Add(this.numEndCnlNum);
            this.gbCnlNums.Controls.Add(this.lblEndCnlNum);
            this.gbCnlNums.Controls.Add(this.numStartCnlNum);
            this.gbCnlNums.Controls.Add(this.lblStartCnlNum);
            this.gbCnlNums.Location = new System.Drawing.Point(0, 47);
            this.gbCnlNums.Name = "gbCnlNums";
            this.gbCnlNums.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCnlNums.Size = new System.Drawing.Size(360, 102);
            this.gbCnlNums.TabIndex = 2;
            this.gbCnlNums.TabStop = false;
            this.gbCnlNums.Text = "Channel Numbers";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(272, 66);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnMap
            // 
            this.btnMap.Location = new System.Drawing.Point(272, 37);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(75, 23);
            this.btnMap.TabIndex = 4;
            this.btnMap.Text = "Map";
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // numEndCnlNum
            // 
            this.numEndCnlNum.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEndCnlNum.Location = new System.Drawing.Point(142, 37);
            this.numEndCnlNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numEndCnlNum.Name = "numEndCnlNum";
            this.numEndCnlNum.ReadOnly = true;
            this.numEndCnlNum.Size = new System.Drawing.Size(124, 23);
            this.numEndCnlNum.TabIndex = 3;
            // 
            // lblEndCnlNum
            // 
            this.lblEndCnlNum.AutoSize = true;
            this.lblEndCnlNum.Location = new System.Drawing.Point(139, 19);
            this.lblEndCnlNum.Name = "lblEndCnlNum";
            this.lblEndCnlNum.Size = new System.Drawing.Size(27, 15);
            this.lblEndCnlNum.TabIndex = 2;
            this.lblEndCnlNum.Text = "End";
            // 
            // numStartCnlNum
            // 
            this.numStartCnlNum.Location = new System.Drawing.Point(13, 37);
            this.numStartCnlNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numStartCnlNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartCnlNum.Name = "numStartCnlNum";
            this.numStartCnlNum.Size = new System.Drawing.Size(123, 23);
            this.numStartCnlNum.TabIndex = 1;
            this.numStartCnlNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartCnlNum.ValueChanged += new System.EventHandler(this.numStartCnlNum_ValueChanged);
            // 
            // lblStartCnlNum
            // 
            this.lblStartCnlNum.AutoSize = true;
            this.lblStartCnlNum.Location = new System.Drawing.Point(10, 19);
            this.lblStartCnlNum.Name = "lblStartCnlNum";
            this.lblStartCnlNum.Size = new System.Drawing.Size(31, 15);
            this.lblStartCnlNum.TabIndex = 0;
            this.lblStartCnlNum.Text = "Start";
            // 
            // CtrlCnlCreate3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCnlNums);
            this.Controls.Add(this.txtDevice);
            this.Controls.Add(this.lblDevice);
            this.Name = "CtrlCnlCreate3";
            this.Size = new System.Drawing.Size(360, 160);
            this.gbCnlNums.ResumeLayout(false);
            this.gbCnlNums.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartCnlNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}
