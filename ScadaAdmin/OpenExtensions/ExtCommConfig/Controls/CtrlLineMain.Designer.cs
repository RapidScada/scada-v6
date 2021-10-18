
namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    partial class CtrlLineMain
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
            this.gbLine = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.numCommLineNum = new System.Windows.Forms.NumericUpDown();
            this.lblCommLineNum = new System.Windows.Forms.Label();
            this.chkIsBound = new System.Windows.Forms.CheckBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.gbLineOptions = new System.Windows.Forms.GroupBox();
            this.lblReqRetries = new System.Windows.Forms.Label();
            this.numReqRetries = new System.Windows.Forms.NumericUpDown();
            this.lblCycleDelay = new System.Windows.Forms.Label();
            this.numCycleDelay = new System.Windows.Forms.NumericUpDown();
            this.lblCmdEnabled = new System.Windows.Forms.Label();
            this.chkCmdEnabled = new System.Windows.Forms.CheckBox();
            this.lblPollAfterCmd = new System.Windows.Forms.Label();
            this.chkPollAfterCmd = new System.Windows.Forms.CheckBox();
            this.lblDetailedLog = new System.Windows.Forms.Label();
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.gbLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommLineNum)).BeginInit();
            this.gbLineOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReqRetries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // gbLine
            // 
            this.gbLine.Controls.Add(this.txtName);
            this.gbLine.Controls.Add(this.lblName);
            this.gbLine.Controls.Add(this.numCommLineNum);
            this.gbLine.Controls.Add(this.lblCommLineNum);
            this.gbLine.Controls.Add(this.chkIsBound);
            this.gbLine.Controls.Add(this.chkActive);
            this.gbLine.Location = new System.Drawing.Point(0, 0);
            this.gbLine.Name = "gbLine";
            this.gbLine.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLine.Size = new System.Drawing.Size(500, 98);
            this.gbLine.TabIndex = 0;
            this.gbLine.TabStop = false;
            this.gbLine.Text = "Communication Line";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(119, 62);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(368, 23);
            this.txtName.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(116, 44);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(39, 15);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Name";
            // 
            // numCommLineNum
            // 
            this.numCommLineNum.Location = new System.Drawing.Point(13, 62);
            this.numCommLineNum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numCommLineNum.Name = "numCommLineNum";
            this.numCommLineNum.Size = new System.Drawing.Size(100, 23);
            this.numCommLineNum.TabIndex = 3;
            // 
            // lblCommLineNum
            // 
            this.lblCommLineNum.AutoSize = true;
            this.lblCommLineNum.Location = new System.Drawing.Point(10, 44);
            this.lblCommLineNum.Name = "lblCommLineNum";
            this.lblCommLineNum.Size = new System.Drawing.Size(51, 15);
            this.lblCommLineNum.TabIndex = 2;
            this.lblCommLineNum.Text = "Number";
            // 
            // chkIsBound
            // 
            this.chkIsBound.AutoSize = true;
            this.chkIsBound.Location = new System.Drawing.Point(119, 22);
            this.chkIsBound.Name = "chkIsBound";
            this.chkIsBound.Size = new System.Drawing.Size(220, 19);
            this.chkIsBound.TabIndex = 1;
            this.chkIsBound.Text = "Bound to the configuration database";
            this.chkIsBound.UseVisualStyleBackColor = true;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(13, 22);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(59, 19);
            this.chkActive.TabIndex = 0;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // gbLineOptions
            // 
            this.gbLineOptions.Controls.Add(this.chkDetailedLog);
            this.gbLineOptions.Controls.Add(this.lblDetailedLog);
            this.gbLineOptions.Controls.Add(this.chkPollAfterCmd);
            this.gbLineOptions.Controls.Add(this.lblPollAfterCmd);
            this.gbLineOptions.Controls.Add(this.chkCmdEnabled);
            this.gbLineOptions.Controls.Add(this.lblCmdEnabled);
            this.gbLineOptions.Controls.Add(this.numCycleDelay);
            this.gbLineOptions.Controls.Add(this.lblCycleDelay);
            this.gbLineOptions.Controls.Add(this.numReqRetries);
            this.gbLineOptions.Controls.Add(this.lblReqRetries);
            this.gbLineOptions.Location = new System.Drawing.Point(0, 104);
            this.gbLineOptions.Name = "gbLineOptions";
            this.gbLineOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbLineOptions.Size = new System.Drawing.Size(500, 174);
            this.gbLineOptions.TabIndex = 1;
            this.gbLineOptions.TabStop = false;
            this.gbLineOptions.Text = "Line Options";
            // 
            // lblReqRetries
            // 
            this.lblReqRetries.AutoSize = true;
            this.lblReqRetries.Location = new System.Drawing.Point(13, 26);
            this.lblReqRetries.Name = "lblReqRetries";
            this.lblReqRetries.Size = new System.Drawing.Size(187, 15);
            this.lblReqRetries.TabIndex = 0;
            this.lblReqRetries.Text = "Number of request retries on error";
            // 
            // numReqRetries
            // 
            this.numReqRetries.Location = new System.Drawing.Point(387, 22);
            this.numReqRetries.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReqRetries.Name = "numReqRetries";
            this.numReqRetries.Size = new System.Drawing.Size(100, 23);
            this.numReqRetries.TabIndex = 1;
            this.numReqRetries.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCycleDelay
            // 
            this.lblCycleDelay.AutoSize = true;
            this.lblCycleDelay.Location = new System.Drawing.Point(13, 55);
            this.lblCycleDelay.Name = "lblCycleDelay";
            this.lblCycleDelay.Size = new System.Drawing.Size(155, 15);
            this.lblCycleDelay.TabIndex = 2;
            this.lblCycleDelay.Text = "Delay after polling cycle, ms";
            // 
            // numCycleDelay
            // 
            this.numCycleDelay.Location = new System.Drawing.Point(387, 51);
            this.numCycleDelay.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numCycleDelay.Name = "numCycleDelay";
            this.numCycleDelay.Size = new System.Drawing.Size(100, 23);
            this.numCycleDelay.TabIndex = 3;
            // 
            // lblCmdEnabled
            // 
            this.lblCmdEnabled.AutoSize = true;
            this.lblCmdEnabled.Location = new System.Drawing.Point(13, 84);
            this.lblCmdEnabled.Name = "lblCmdEnabled";
            this.lblCmdEnabled.Size = new System.Drawing.Size(114, 15);
            this.lblCmdEnabled.TabIndex = 4;
            this.lblCmdEnabled.Text = "Commands enabled";
            // 
            // chkCmdEnabled
            // 
            this.chkCmdEnabled.AutoSize = true;
            this.chkCmdEnabled.Location = new System.Drawing.Point(430, 84);
            this.chkCmdEnabled.Name = "chkCmdEnabled";
            this.chkCmdEnabled.Size = new System.Drawing.Size(15, 14);
            this.chkCmdEnabled.TabIndex = 5;
            this.chkCmdEnabled.UseVisualStyleBackColor = true;
            // 
            // lblPollAfterCmd
            // 
            this.lblPollAfterCmd.AutoSize = true;
            this.lblPollAfterCmd.Location = new System.Drawing.Point(13, 113);
            this.lblPollAfterCmd.Name = "lblPollAfterCmd";
            this.lblPollAfterCmd.Size = new System.Drawing.Size(149, 15);
            this.lblPollAfterCmd.TabIndex = 6;
            this.lblPollAfterCmd.Text = "Poll device after command";
            // 
            // chkPollAfterCmd
            // 
            this.chkPollAfterCmd.AutoSize = true;
            this.chkPollAfterCmd.Location = new System.Drawing.Point(430, 113);
            this.chkPollAfterCmd.Name = "chkPollAfterCmd";
            this.chkPollAfterCmd.Size = new System.Drawing.Size(15, 14);
            this.chkPollAfterCmd.TabIndex = 7;
            this.chkPollAfterCmd.UseVisualStyleBackColor = true;
            // 
            // lblDetailedLog
            // 
            this.lblDetailedLog.AutoSize = true;
            this.lblDetailedLog.Location = new System.Drawing.Point(13, 142);
            this.lblDetailedLog.Name = "lblDetailedLog";
            this.lblDetailedLog.Size = new System.Drawing.Size(70, 15);
            this.lblDetailedLog.TabIndex = 8;
            this.lblDetailedLog.Text = "Detailed log";
            // 
            // chkDetailedLog
            // 
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(430, 142);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.Size = new System.Drawing.Size(15, 14);
            this.chkDetailedLog.TabIndex = 9;
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            // 
            // CtrlLineMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbLineOptions);
            this.Controls.Add(this.gbLine);
            this.Name = "CtrlLineMain";
            this.Size = new System.Drawing.Size(550, 600);
            this.gbLine.ResumeLayout(false);
            this.gbLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommLineNum)).EndInit();
            this.gbLineOptions.ResumeLayout(false);
            this.gbLineOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReqRetries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLine;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.NumericUpDown numCommLineNum;
        private System.Windows.Forms.Label lblCommLineNum;
        private System.Windows.Forms.CheckBox chkIsBound;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.GroupBox gbLineOptions;
        private System.Windows.Forms.NumericUpDown numCycleDelay;
        private System.Windows.Forms.Label lblCycleDelay;
        private System.Windows.Forms.NumericUpDown numReqRetries;
        private System.Windows.Forms.Label lblReqRetries;
        private System.Windows.Forms.CheckBox chkDetailedLog;
        private System.Windows.Forms.Label lblDetailedLog;
        private System.Windows.Forms.CheckBox chkPollAfterCmd;
        private System.Windows.Forms.Label lblPollAfterCmd;
        private System.Windows.Forms.CheckBox chkCmdEnabled;
        private System.Windows.Forms.Label lblCmdEnabled;
    }
}
