
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
            this.chkDetailedLog = new System.Windows.Forms.CheckBox();
            this.lblDetailedLog = new System.Windows.Forms.Label();
            this.chkPollAfterCmd = new System.Windows.Forms.CheckBox();
            this.lblPollAfterCmd = new System.Windows.Forms.Label();
            this.chkCmdEnabled = new System.Windows.Forms.CheckBox();
            this.lblCmdEnabled = new System.Windows.Forms.Label();
            this.numCycleDelay = new System.Windows.Forms.NumericUpDown();
            this.lblCycleDelay = new System.Windows.Forms.Label();
            this.numReqRetries = new System.Windows.Forms.NumericUpDown();
            this.lblReqRetries = new System.Windows.Forms.Label();
            this.gbChannel = new System.Windows.Forms.GroupBox();
            this.txtChannelOptions = new System.Windows.Forms.TextBox();
            this.lblChannelOptions = new System.Windows.Forms.Label();
            this.btnChannelProperties = new System.Windows.Forms.Button();
            this.cbChannelType = new System.Windows.Forms.ComboBox();
            this.lblChannelType = new System.Windows.Forms.Label();
            this.gbLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommLineNum)).BeginInit();
            this.gbLineOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReqRetries)).BeginInit();
            this.gbChannel.SuspendLayout();
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
            // chkDetailedLog
            // 
            this.chkDetailedLog.AutoSize = true;
            this.chkDetailedLog.Location = new System.Drawing.Point(430, 142);
            this.chkDetailedLog.Name = "chkDetailedLog";
            this.chkDetailedLog.Size = new System.Drawing.Size(15, 14);
            this.chkDetailedLog.TabIndex = 9;
            this.chkDetailedLog.UseVisualStyleBackColor = true;
            // 
            // lblDetailedLog
            // 
            this.lblDetailedLog.AutoSize = true;
            this.lblDetailedLog.Location = new System.Drawing.Point(10, 142);
            this.lblDetailedLog.Name = "lblDetailedLog";
            this.lblDetailedLog.Size = new System.Drawing.Size(70, 15);
            this.lblDetailedLog.TabIndex = 8;
            this.lblDetailedLog.Text = "Detailed log";
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
            // lblPollAfterCmd
            // 
            this.lblPollAfterCmd.AutoSize = true;
            this.lblPollAfterCmd.Location = new System.Drawing.Point(10, 113);
            this.lblPollAfterCmd.Name = "lblPollAfterCmd";
            this.lblPollAfterCmd.Size = new System.Drawing.Size(149, 15);
            this.lblPollAfterCmd.TabIndex = 6;
            this.lblPollAfterCmd.Text = "Poll device after command";
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
            // lblCmdEnabled
            // 
            this.lblCmdEnabled.AutoSize = true;
            this.lblCmdEnabled.Location = new System.Drawing.Point(10, 84);
            this.lblCmdEnabled.Name = "lblCmdEnabled";
            this.lblCmdEnabled.Size = new System.Drawing.Size(114, 15);
            this.lblCmdEnabled.TabIndex = 4;
            this.lblCmdEnabled.Text = "Commands enabled";
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
            // lblCycleDelay
            // 
            this.lblCycleDelay.AutoSize = true;
            this.lblCycleDelay.Location = new System.Drawing.Point(10, 55);
            this.lblCycleDelay.Name = "lblCycleDelay";
            this.lblCycleDelay.Size = new System.Drawing.Size(155, 15);
            this.lblCycleDelay.TabIndex = 2;
            this.lblCycleDelay.Text = "Delay after polling cycle, ms";
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
            // lblReqRetries
            // 
            this.lblReqRetries.AutoSize = true;
            this.lblReqRetries.Location = new System.Drawing.Point(10, 26);
            this.lblReqRetries.Name = "lblReqRetries";
            this.lblReqRetries.Size = new System.Drawing.Size(187, 15);
            this.lblReqRetries.TabIndex = 0;
            this.lblReqRetries.Text = "Number of request retries on error";
            // 
            // gbChannel
            // 
            this.gbChannel.Controls.Add(this.txtChannelOptions);
            this.gbChannel.Controls.Add(this.lblChannelOptions);
            this.gbChannel.Controls.Add(this.btnChannelProperties);
            this.gbChannel.Controls.Add(this.cbChannelType);
            this.gbChannel.Controls.Add(this.lblChannelType);
            this.gbChannel.Location = new System.Drawing.Point(0, 284);
            this.gbChannel.Name = "gbChannel";
            this.gbChannel.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbChannel.Size = new System.Drawing.Size(500, 244);
            this.gbChannel.TabIndex = 2;
            this.gbChannel.TabStop = false;
            this.gbChannel.Text = "Communication Channel";
            // 
            // txtChannelOptions
            // 
            this.txtChannelOptions.Location = new System.Drawing.Point(13, 81);
            this.txtChannelOptions.Multiline = true;
            this.txtChannelOptions.Name = "txtChannelOptions";
            this.txtChannelOptions.ReadOnly = true;
            this.txtChannelOptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChannelOptions.Size = new System.Drawing.Size(474, 150);
            this.txtChannelOptions.TabIndex = 4;
            // 
            // lblChannelOptions
            // 
            this.lblChannelOptions.AutoSize = true;
            this.lblChannelOptions.Location = new System.Drawing.Point(10, 63);
            this.lblChannelOptions.Name = "lblChannelOptions";
            this.lblChannelOptions.Size = new System.Drawing.Size(49, 15);
            this.lblChannelOptions.TabIndex = 3;
            this.lblChannelOptions.Text = "Options";
            // 
            // btnChannelProperties
            // 
            this.btnChannelProperties.Location = new System.Drawing.Point(412, 37);
            this.btnChannelProperties.Name = "btnChannelProperties";
            this.btnChannelProperties.Size = new System.Drawing.Size(75, 23);
            this.btnChannelProperties.TabIndex = 2;
            this.btnChannelProperties.Text = "Properties";
            this.btnChannelProperties.UseVisualStyleBackColor = true;
            // 
            // cbChannelType
            // 
            this.cbChannelType.FormattingEnabled = true;
            this.cbChannelType.Location = new System.Drawing.Point(13, 37);
            this.cbChannelType.Name = "cbChannelType";
            this.cbChannelType.Size = new System.Drawing.Size(393, 23);
            this.cbChannelType.TabIndex = 1;
            // 
            // lblChannelType
            // 
            this.lblChannelType.AutoSize = true;
            this.lblChannelType.Location = new System.Drawing.Point(10, 19);
            this.lblChannelType.Name = "lblChannelType";
            this.lblChannelType.Size = new System.Drawing.Size(31, 15);
            this.lblChannelType.TabIndex = 0;
            this.lblChannelType.Text = "Type";
            // 
            // CtrlLineMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbChannel);
            this.Controls.Add(this.gbLineOptions);
            this.Controls.Add(this.gbLine);
            this.Name = "CtrlLineMain";
            this.Size = new System.Drawing.Size(550, 550);
            this.gbLine.ResumeLayout(false);
            this.gbLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommLineNum)).EndInit();
            this.gbLineOptions.ResumeLayout(false);
            this.gbLineOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCycleDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReqRetries)).EndInit();
            this.gbChannel.ResumeLayout(false);
            this.gbChannel.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbChannel;
        private System.Windows.Forms.Button btnChannelProperties;
        private System.Windows.Forms.ComboBox cbChannelType;
        private System.Windows.Forms.Label lblChannelType;
        private System.Windows.Forms.TextBox txtChannelOptions;
        private System.Windows.Forms.Label lblChannelOptions;
    }
}
