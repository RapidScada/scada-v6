namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
{
    partial class CtrlEntityID
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
            this.lblStartCnlNum = new System.Windows.Forms.Label();
            this.numStartCnlNum = new System.Windows.Forms.NumericUpDown();
            this.lblStartDeviceNum = new System.Windows.Forms.Label();
            this.numStartDeviceNum = new System.Windows.Forms.NumericUpDown();
            this.btnCnlMap = new System.Windows.Forms.Button();
            this.btnDeviceMap = new System.Windows.Forms.Button();
            this.lblObj = new System.Windows.Forms.Label();
            this.cbObj = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numStartCnlNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartDeviceNum)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStartCnlNum
            // 
            this.lblStartCnlNum.AutoSize = true;
            this.lblStartCnlNum.Location = new System.Drawing.Point(9, 60);
            this.lblStartCnlNum.Name = "lblStartCnlNum";
            this.lblStartCnlNum.Size = new System.Drawing.Size(138, 15);
            this.lblStartCnlNum.TabIndex = 3;
            this.lblStartCnlNum.Text = "Starting channel number";
            // 
            // numStartCnlNum
            // 
            this.numStartCnlNum.Location = new System.Drawing.Point(12, 78);
            this.numStartCnlNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
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
            this.numStartCnlNum.Size = new System.Drawing.Size(200, 23);
            this.numStartCnlNum.TabIndex = 4;
            this.numStartCnlNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStartDeviceNum
            // 
            this.lblStartDeviceNum.AutoSize = true;
            this.lblStartDeviceNum.Location = new System.Drawing.Point(9, 9);
            this.lblStartDeviceNum.Name = "lblStartDeviceNum";
            this.lblStartDeviceNum.Size = new System.Drawing.Size(130, 15);
            this.lblStartDeviceNum.TabIndex = 0;
            this.lblStartDeviceNum.Text = "Starting device number";
            // 
            // numStartDeviceNum
            // 
            this.numStartDeviceNum.Location = new System.Drawing.Point(12, 27);
            this.numStartDeviceNum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numStartDeviceNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numStartDeviceNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartDeviceNum.Name = "numStartDeviceNum";
            this.numStartDeviceNum.Size = new System.Drawing.Size(200, 23);
            this.numStartDeviceNum.TabIndex = 1;
            this.numStartDeviceNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnCnlMap
            // 
            this.btnCnlMap.Location = new System.Drawing.Point(218, 78);
            this.btnCnlMap.Name = "btnCnlMap";
            this.btnCnlMap.Size = new System.Drawing.Size(75, 23);
            this.btnCnlMap.TabIndex = 5;
            this.btnCnlMap.Text = "Map";
            this.btnCnlMap.UseVisualStyleBackColor = true;
            this.btnCnlMap.Click += new System.EventHandler(this.btnCnlMap_Click);
            // 
            // btnDeviceMap
            // 
            this.btnDeviceMap.Location = new System.Drawing.Point(218, 27);
            this.btnDeviceMap.Name = "btnDeviceMap";
            this.btnDeviceMap.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceMap.TabIndex = 2;
            this.btnDeviceMap.Text = "Map";
            this.btnDeviceMap.UseVisualStyleBackColor = true;
            this.btnDeviceMap.Click += new System.EventHandler(this.btnDeviceMap_Click);
            // 
            // lblObj
            // 
            this.lblObj.AutoSize = true;
            this.lblObj.Location = new System.Drawing.Point(9, 111);
            this.lblObj.Name = "lblObj";
            this.lblObj.Size = new System.Drawing.Size(42, 15);
            this.lblObj.TabIndex = 6;
            this.lblObj.Text = "Object";
            // 
            // cbObj
            // 
            this.cbObj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObj.FormattingEnabled = true;
            this.cbObj.Location = new System.Drawing.Point(12, 129);
            this.cbObj.Name = "cbObj";
            this.cbObj.Size = new System.Drawing.Size(200, 23);
            this.cbObj.TabIndex = 7;
            // 
            // CtrlEntityID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbObj);
            this.Controls.Add(this.lblObj);
            this.Controls.Add(this.btnDeviceMap);
            this.Controls.Add(this.numStartCnlNum);
            this.Controls.Add(this.lblStartCnlNum);
            this.Controls.Add(this.btnCnlMap);
            this.Controls.Add(this.lblStartDeviceNum);
            this.Controls.Add(this.numStartDeviceNum);
            this.Name = "CtrlEntityID";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.Size = new System.Drawing.Size(500, 200);
            this.Load += new System.EventHandler(this.CtrlEntityID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numStartCnlNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartDeviceNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblStartCnlNum;
        private NumericUpDown numStartCnlNum;
        private Label lblStartDeviceNum;
        private NumericUpDown numStartDeviceNum;
        private Button btnCnlMap;
        private Button btnDeviceMap;
        private Label lblObj;
        private ComboBox cbObj;
    }
}
