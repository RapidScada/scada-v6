namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmGeneralOptions
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
            gbGeneralOptions = new System.Windows.Forms.GroupBox();
            numMaxLogSize = new System.Windows.Forms.NumericUpDown();
            lblMaxLogSize = new System.Windows.Forms.Label();
            numStopWait = new System.Windows.Forms.NumericUpDown();
            lblStopWait = new System.Windows.Forms.Label();
            chkStartLinesOnCommand = new System.Windows.Forms.CheckBox();
            lblStartLinesOnCommand = new System.Windows.Forms.Label();
            chkEnableFileCommands = new System.Windows.Forms.CheckBox();
            lblEnableFileCommands = new System.Windows.Forms.Label();
            chkEnableCommands = new System.Windows.Forms.CheckBox();
            lblEnableCommands = new System.Windows.Forms.Label();
            numSendAllDataPeriod = new System.Windows.Forms.NumericUpDown();
            lblSendAllDataPeriod = new System.Windows.Forms.Label();
            chkSendModifiedData = new System.Windows.Forms.CheckBox();
            lblSendModifiedData = new System.Windows.Forms.Label();
            chkIsBound = new System.Windows.Forms.CheckBox();
            lblIsBound = new System.Windows.Forms.Label();
            ctrlClientConnection = new Scada.Forms.Controls.CtrlClientConnection();
            gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxLogSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStopWait).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSendAllDataPeriod).BeginInit();
            SuspendLayout();
            // 
            // gbGeneralOptions
            // 
            gbGeneralOptions.Controls.Add(numMaxLogSize);
            gbGeneralOptions.Controls.Add(lblMaxLogSize);
            gbGeneralOptions.Controls.Add(numStopWait);
            gbGeneralOptions.Controls.Add(lblStopWait);
            gbGeneralOptions.Controls.Add(chkStartLinesOnCommand);
            gbGeneralOptions.Controls.Add(lblStartLinesOnCommand);
            gbGeneralOptions.Controls.Add(chkEnableFileCommands);
            gbGeneralOptions.Controls.Add(lblEnableFileCommands);
            gbGeneralOptions.Controls.Add(chkEnableCommands);
            gbGeneralOptions.Controls.Add(lblEnableCommands);
            gbGeneralOptions.Controls.Add(numSendAllDataPeriod);
            gbGeneralOptions.Controls.Add(lblSendAllDataPeriod);
            gbGeneralOptions.Controls.Add(chkSendModifiedData);
            gbGeneralOptions.Controls.Add(lblSendModifiedData);
            gbGeneralOptions.Controls.Add(chkIsBound);
            gbGeneralOptions.Controls.Add(lblIsBound);
            gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            gbGeneralOptions.Name = "gbGeneralOptions";
            gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            gbGeneralOptions.Size = new System.Drawing.Size(500, 261);
            gbGeneralOptions.TabIndex = 0;
            gbGeneralOptions.TabStop = false;
            gbGeneralOptions.Text = "General Options";
            // 
            // numMaxLogSize
            // 
            numMaxLogSize.Location = new System.Drawing.Point(387, 225);
            numMaxLogSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxLogSize.Name = "numMaxLogSize";
            numMaxLogSize.Size = new System.Drawing.Size(100, 23);
            numMaxLogSize.TabIndex = 15;
            numMaxLogSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxLogSize.ValueChanged += control_Changed;
            // 
            // lblMaxLogSize
            // 
            lblMaxLogSize.AutoSize = true;
            lblMaxLogSize.Location = new System.Drawing.Point(10, 229);
            lblMaxLogSize.Name = "lblMaxLogSize";
            lblMaxLogSize.Size = new System.Drawing.Size(147, 15);
            lblMaxLogSize.TabIndex = 14;
            lblMaxLogSize.Text = "Maximum log file size, MB";
            // 
            // numStopWait
            // 
            numStopWait.Location = new System.Drawing.Point(387, 196);
            numStopWait.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numStopWait.Name = "numStopWait";
            numStopWait.Size = new System.Drawing.Size(100, 23);
            numStopWait.TabIndex = 13;
            numStopWait.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numStopWait.ValueChanged += control_Changed;
            // 
            // lblStopWait
            // 
            lblStopWait.AutoSize = true;
            lblStopWait.Location = new System.Drawing.Point(10, 200);
            lblStopWait.Name = "lblStopWait";
            lblStopWait.Size = new System.Drawing.Size(137, 15);
            lblStopWait.TabIndex = 12;
            lblStopWait.Text = "Wait for service stop, sec";
            // 
            // chkStartLinesOnCommand
            // 
            chkStartLinesOnCommand.AutoSize = true;
            chkStartLinesOnCommand.Location = new System.Drawing.Point(472, 171);
            chkStartLinesOnCommand.Name = "chkStartLinesOnCommand";
            chkStartLinesOnCommand.Size = new System.Drawing.Size(15, 14);
            chkStartLinesOnCommand.TabIndex = 11;
            chkStartLinesOnCommand.UseVisualStyleBackColor = true;
            chkStartLinesOnCommand.CheckedChanged += control_Changed;
            // 
            // lblStartLinesOnCommand
            // 
            lblStartLinesOnCommand.AutoSize = true;
            lblStartLinesOnCommand.Location = new System.Drawing.Point(10, 171);
            lblStartLinesOnCommand.Name = "lblStartLinesOnCommand";
            lblStartLinesOnCommand.Size = new System.Drawing.Size(221, 15);
            lblStartLinesOnCommand.TabIndex = 10;
            lblStartLinesOnCommand.Text = "Start communication lines on command";
            // 
            // chkEnableFileCommands
            // 
            chkEnableFileCommands.AutoSize = true;
            chkEnableFileCommands.Location = new System.Drawing.Point(472, 142);
            chkEnableFileCommands.Name = "chkEnableFileCommands";
            chkEnableFileCommands.Size = new System.Drawing.Size(15, 14);
            chkEnableFileCommands.TabIndex = 9;
            chkEnableFileCommands.UseVisualStyleBackColor = true;
            chkEnableFileCommands.CheckedChanged += control_Changed;
            // 
            // lblEnableFileCommands
            // 
            lblEnableFileCommands.AutoSize = true;
            lblEnableFileCommands.Location = new System.Drawing.Point(10, 142);
            lblEnableFileCommands.Name = "lblEnableFileCommands";
            lblEnableFileCommands.Size = new System.Drawing.Size(209, 15);
            lblEnableFileCommands.TabIndex = 8;
            lblEnableFileCommands.Text = "Read telecontrol commands from files";
            // 
            // chkEnableCommands
            // 
            chkEnableCommands.AutoSize = true;
            chkEnableCommands.Location = new System.Drawing.Point(472, 113);
            chkEnableCommands.Name = "chkEnableCommands";
            chkEnableCommands.Size = new System.Drawing.Size(15, 14);
            chkEnableCommands.TabIndex = 7;
            chkEnableCommands.UseVisualStyleBackColor = true;
            chkEnableCommands.CheckedChanged += control_Changed;
            // 
            // lblEnableCommands
            // 
            lblEnableCommands.AutoSize = true;
            lblEnableCommands.Location = new System.Drawing.Point(10, 113);
            lblEnableCommands.Name = "lblEnableCommands";
            lblEnableCommands.Size = new System.Drawing.Size(165, 15);
            lblEnableCommands.TabIndex = 6;
            lblEnableCommands.Text = "Enable telecontrol commands";
            // 
            // numSendAllDataPeriod
            // 
            numSendAllDataPeriod.Location = new System.Drawing.Point(387, 80);
            numSendAllDataPeriod.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            numSendAllDataPeriod.Name = "numSendAllDataPeriod";
            numSendAllDataPeriod.Size = new System.Drawing.Size(100, 23);
            numSendAllDataPeriod.TabIndex = 5;
            numSendAllDataPeriod.ValueChanged += control_Changed;
            // 
            // lblSendAllDataPeriod
            // 
            lblSendAllDataPeriod.AutoSize = true;
            lblSendAllDataPeriod.Location = new System.Drawing.Point(10, 84);
            lblSendAllDataPeriod.Name = "lblSendAllDataPeriod";
            lblSendAllDataPeriod.Size = new System.Drawing.Size(240, 15);
            lblSendAllDataPeriod.TabIndex = 4;
            lblSendAllDataPeriod.Text = "Period of sending data of all device tags, sec";
            // 
            // chkSendModifiedData
            // 
            chkSendModifiedData.AutoSize = true;
            chkSendModifiedData.Location = new System.Drawing.Point(472, 55);
            chkSendModifiedData.Name = "chkSendModifiedData";
            chkSendModifiedData.Size = new System.Drawing.Size(15, 14);
            chkSendModifiedData.TabIndex = 3;
            chkSendModifiedData.UseVisualStyleBackColor = true;
            chkSendModifiedData.CheckedChanged += control_Changed;
            // 
            // lblSendModifiedData
            // 
            lblSendModifiedData.AutoSize = true;
            lblSendModifiedData.Location = new System.Drawing.Point(10, 55);
            lblSendModifiedData.Name = "lblSendModifiedData";
            lblSendModifiedData.Size = new System.Drawing.Size(212, 15);
            lblSendModifiedData.TabIndex = 2;
            lblSendModifiedData.Text = "Send only modified data of device tags";
            // 
            // chkIsBound
            // 
            chkIsBound.AutoSize = true;
            chkIsBound.Location = new System.Drawing.Point(472, 26);
            chkIsBound.Name = "chkIsBound";
            chkIsBound.Size = new System.Drawing.Size(15, 14);
            chkIsBound.TabIndex = 1;
            chkIsBound.UseVisualStyleBackColor = true;
            chkIsBound.CheckedChanged += control_Changed;
            // 
            // lblIsBound
            // 
            lblIsBound.AutoSize = true;
            lblIsBound.Location = new System.Drawing.Point(10, 26);
            lblIsBound.Name = "lblIsBound";
            lblIsBound.Size = new System.Drawing.Size(276, 15);
            lblIsBound.TabIndex = 0;
            lblIsBound.Text = "Application is bound to the configuration database";
            // 
            // ctrlClientConnection
            // 
            ctrlClientConnection.ConnectionOptions = null;
            ctrlClientConnection.InstanceEnabled = false;
            ctrlClientConnection.Location = new System.Drawing.Point(12, 279);
            ctrlClientConnection.Name = "ctrlClientConnection";
            ctrlClientConnection.NameEnabled = false;
            ctrlClientConnection.Size = new System.Drawing.Size(500, 366);
            ctrlClientConnection.TabIndex = 1;
            ctrlClientConnection.ConnectionOptionsChanged += control_Changed;
            // 
            // FrmGeneralOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(684, 661);
            Controls.Add(ctrlClientConnection);
            Controls.Add(gbGeneralOptions);
            Name = "FrmGeneralOptions";
            Text = "General Options";
            Load += FrmGeneralOptions_Load;
            gbGeneralOptions.ResumeLayout(false);
            gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxLogSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStopWait).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSendAllDataPeriod).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbGeneralOptions;
        private System.Windows.Forms.NumericUpDown numMaxLogSize;
        private System.Windows.Forms.Label lblMaxLogSize;
        private System.Windows.Forms.CheckBox chkIsBound;
        private System.Windows.Forms.Label lblIsBound;
        private System.Windows.Forms.CheckBox chkSendModifiedData;
        private System.Windows.Forms.Label lblSendModifiedData;
        private System.Windows.Forms.NumericUpDown numSendAllDataPeriod;
        private System.Windows.Forms.Label lblSendAllDataPeriod;
        private System.Windows.Forms.CheckBox chkEnableCommands;
        private System.Windows.Forms.Label lblEnableCommands;
        private System.Windows.Forms.CheckBox chkEnableFileCommands;
        private System.Windows.Forms.Label lblEnableFileCommands;
        private Scada.Forms.Controls.CtrlClientConnection ctrlClientConnection;
        private System.Windows.Forms.CheckBox chkStartLinesOnCommand;
        private System.Windows.Forms.Label lblStartLinesOnCommand;
        private System.Windows.Forms.NumericUpDown numStopWait;
        private System.Windows.Forms.Label lblStopWait;
    }
}