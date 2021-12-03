namespace Scada.Comm.Drivers.DrvOpcUa.View.Controls
{
    partial class CtrlCommand
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
            this.gbCommand = new System.Windows.Forms.GroupBox();
            this.numCmdNum = new System.Windows.Forms.NumericUpDown();
            this.lblCmdNum = new System.Windows.Forms.Label();
            this.txtDataType = new System.Windows.Forms.TextBox();
            this.lblDataType = new System.Windows.Forms.Label();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.gbCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCommand
            // 
            this.gbCommand.Controls.Add(this.numCmdNum);
            this.gbCommand.Controls.Add(this.lblCmdNum);
            this.gbCommand.Controls.Add(this.txtDataType);
            this.gbCommand.Controls.Add(this.lblDataType);
            this.gbCommand.Controls.Add(this.txtNodeID);
            this.gbCommand.Controls.Add(this.lblNodeID);
            this.gbCommand.Controls.Add(this.txtDisplayName);
            this.gbCommand.Controls.Add(this.lblDisplayName);
            this.gbCommand.Location = new System.Drawing.Point(0, 0);
            this.gbCommand.Name = "gbCommand";
            this.gbCommand.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbCommand.Size = new System.Drawing.Size(250, 500);
            this.gbCommand.TabIndex = 0;
            this.gbCommand.TabStop = false;
            this.gbCommand.Text = "Command Parameters";
            // 
            // numCmdNum
            // 
            this.numCmdNum.Location = new System.Drawing.Point(13, 169);
            this.numCmdNum.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numCmdNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdNum.Name = "numCmdNum";
            this.numCmdNum.Size = new System.Drawing.Size(120, 23);
            this.numCmdNum.TabIndex = 7;
            this.numCmdNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCmdNum.ValueChanged += new System.EventHandler(this.numCmdNum_ValueChanged);
            // 
            // lblCmdNum
            // 
            this.lblCmdNum.AutoSize = true;
            this.lblCmdNum.Location = new System.Drawing.Point(10, 151);
            this.lblCmdNum.Name = "lblCmdNum";
            this.lblCmdNum.Size = new System.Drawing.Size(109, 15);
            this.lblCmdNum.TabIndex = 6;
            this.lblCmdNum.Text = "Command number";
            // 
            // txtDataType
            // 
            this.txtDataType.Location = new System.Drawing.Point(13, 125);
            this.txtDataType.Name = "txtDataType";
            this.txtDataType.ReadOnly = true;
            this.txtDataType.Size = new System.Drawing.Size(224, 23);
            this.txtDataType.TabIndex = 5;
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(10, 107);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(57, 15);
            this.lblDataType.TabIndex = 4;
            this.lblDataType.Text = "Data type";
            // 
            // txtNodeID
            // 
            this.txtNodeID.Location = new System.Drawing.Point(13, 81);
            this.txtNodeID.Name = "txtNodeID";
            this.txtNodeID.ReadOnly = true;
            this.txtNodeID.Size = new System.Drawing.Size(224, 23);
            this.txtNodeID.TabIndex = 3;
            // 
            // lblNodeID
            // 
            this.lblNodeID.AutoSize = true;
            this.lblNodeID.Location = new System.Drawing.Point(10, 63);
            this.lblNodeID.Name = "lblNodeID";
            this.lblNodeID.Size = new System.Drawing.Size(50, 15);
            this.lblNodeID.TabIndex = 2;
            this.lblNodeID.Text = "Node ID";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(13, 37);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(224, 23);
            this.txtDisplayName.TabIndex = 1;
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(10, 19);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(78, 15);
            this.lblDisplayName.TabIndex = 0;
            this.lblDisplayName.Text = "Display name";
            // 
            // CtrlCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCommand);
            this.Name = "CtrlCommand";
            this.Size = new System.Drawing.Size(250, 500);
            this.gbCommand.ResumeLayout(false);
            this.gbCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCmdNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCommand;
        private System.Windows.Forms.TextBox txtNodeID;
        private System.Windows.Forms.Label lblNodeID;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox txtDataType;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.NumericUpDown numCmdNum;
        private System.Windows.Forms.Label lblCmdNum;
    }
}
