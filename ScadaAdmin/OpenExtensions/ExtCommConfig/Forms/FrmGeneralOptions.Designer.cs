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
            this.gbGeneralOptions = new System.Windows.Forms.GroupBox();
            this.numMaxLogSize = new System.Windows.Forms.NumericUpDown();
            this.lblMaxLogSize = new System.Windows.Forms.Label();
            this.chkEnableFileCommands = new System.Windows.Forms.CheckBox();
            this.lblEnableFileCommands = new System.Windows.Forms.Label();
            this.chkEnableCommands = new System.Windows.Forms.CheckBox();
            this.lblEnableCommands = new System.Windows.Forms.Label();
            this.numSendAllDataPeriod = new System.Windows.Forms.NumericUpDown();
            this.lblSendAllDataPeriod = new System.Windows.Forms.Label();
            this.chkSendModifiedData = new System.Windows.Forms.CheckBox();
            this.lblSendModifiedData = new System.Windows.Forms.Label();
            this.chkIsBound = new System.Windows.Forms.CheckBox();
            this.lblIsBound = new System.Windows.Forms.Label();
            this.ctrlClientConnection = new Scada.Forms.Controls.CtrlClientConnection();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAllDataPeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // gbGeneralOptions
            // 
            this.gbGeneralOptions.Controls.Add(this.numMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.lblMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.chkEnableFileCommands);
            this.gbGeneralOptions.Controls.Add(this.lblEnableFileCommands);
            this.gbGeneralOptions.Controls.Add(this.chkEnableCommands);
            this.gbGeneralOptions.Controls.Add(this.lblEnableCommands);
            this.gbGeneralOptions.Controls.Add(this.numSendAllDataPeriod);
            this.gbGeneralOptions.Controls.Add(this.lblSendAllDataPeriod);
            this.gbGeneralOptions.Controls.Add(this.chkSendModifiedData);
            this.gbGeneralOptions.Controls.Add(this.lblSendModifiedData);
            this.gbGeneralOptions.Controls.Add(this.chkIsBound);
            this.gbGeneralOptions.Controls.Add(this.lblIsBound);
            this.gbGeneralOptions.Location = new System.Drawing.Point(12, 12);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(500, 203);
            this.gbGeneralOptions.TabIndex = 0;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // numMaxLogSize
            // 
            this.numMaxLogSize.Location = new System.Drawing.Point(387, 167);
            this.numMaxLogSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxLogSize.Name = "numMaxLogSize";
            this.numMaxLogSize.Size = new System.Drawing.Size(100, 23);
            this.numMaxLogSize.TabIndex = 11;
            this.numMaxLogSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxLogSize.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblMaxLogSize
            // 
            this.lblMaxLogSize.AutoSize = true;
            this.lblMaxLogSize.Location = new System.Drawing.Point(17, 171);
            this.lblMaxLogSize.Name = "lblMaxLogSize";
            this.lblMaxLogSize.Size = new System.Drawing.Size(147, 15);
            this.lblMaxLogSize.TabIndex = 10;
            this.lblMaxLogSize.Text = "Maximum log file size, MB";
            // 
            // chkEnableFileCommands
            // 
            this.chkEnableFileCommands.AutoSize = true;
            this.chkEnableFileCommands.Location = new System.Drawing.Point(430, 142);
            this.chkEnableFileCommands.Name = "chkEnableFileCommands";
            this.chkEnableFileCommands.Size = new System.Drawing.Size(15, 14);
            this.chkEnableFileCommands.TabIndex = 9;
            this.chkEnableFileCommands.UseVisualStyleBackColor = true;
            this.chkEnableFileCommands.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblEnableFileCommands
            // 
            this.lblEnableFileCommands.AutoSize = true;
            this.lblEnableFileCommands.Location = new System.Drawing.Point(17, 142);
            this.lblEnableFileCommands.Name = "lblEnableFileCommands";
            this.lblEnableFileCommands.Size = new System.Drawing.Size(209, 15);
            this.lblEnableFileCommands.TabIndex = 8;
            this.lblEnableFileCommands.Text = "Read telecontrol commands from files";
            // 
            // chkEnableCommands
            // 
            this.chkEnableCommands.AutoSize = true;
            this.chkEnableCommands.Location = new System.Drawing.Point(430, 113);
            this.chkEnableCommands.Name = "chkEnableCommands";
            this.chkEnableCommands.Size = new System.Drawing.Size(15, 14);
            this.chkEnableCommands.TabIndex = 7;
            this.chkEnableCommands.UseVisualStyleBackColor = true;
            this.chkEnableCommands.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblEnableCommands
            // 
            this.lblEnableCommands.AutoSize = true;
            this.lblEnableCommands.Location = new System.Drawing.Point(17, 113);
            this.lblEnableCommands.Name = "lblEnableCommands";
            this.lblEnableCommands.Size = new System.Drawing.Size(165, 15);
            this.lblEnableCommands.TabIndex = 6;
            this.lblEnableCommands.Text = "Enable telecontrol commands";
            // 
            // numSendAllDataPeriod
            // 
            this.numSendAllDataPeriod.Location = new System.Drawing.Point(387, 80);
            this.numSendAllDataPeriod.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numSendAllDataPeriod.Name = "numSendAllDataPeriod";
            this.numSendAllDataPeriod.Size = new System.Drawing.Size(100, 23);
            this.numSendAllDataPeriod.TabIndex = 5;
            this.numSendAllDataPeriod.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblSendAllDataPeriod
            // 
            this.lblSendAllDataPeriod.AutoSize = true;
            this.lblSendAllDataPeriod.Location = new System.Drawing.Point(17, 84);
            this.lblSendAllDataPeriod.Name = "lblSendAllDataPeriod";
            this.lblSendAllDataPeriod.Size = new System.Drawing.Size(240, 15);
            this.lblSendAllDataPeriod.TabIndex = 4;
            this.lblSendAllDataPeriod.Text = "Period of sending data of all device tags, sec";
            // 
            // chkSendModifiedData
            // 
            this.chkSendModifiedData.AutoSize = true;
            this.chkSendModifiedData.Location = new System.Drawing.Point(430, 55);
            this.chkSendModifiedData.Name = "chkSendModifiedData";
            this.chkSendModifiedData.Size = new System.Drawing.Size(15, 14);
            this.chkSendModifiedData.TabIndex = 3;
            this.chkSendModifiedData.UseVisualStyleBackColor = true;
            this.chkSendModifiedData.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblSendModifiedData
            // 
            this.lblSendModifiedData.AutoSize = true;
            this.lblSendModifiedData.Location = new System.Drawing.Point(17, 55);
            this.lblSendModifiedData.Name = "lblSendModifiedData";
            this.lblSendModifiedData.Size = new System.Drawing.Size(212, 15);
            this.lblSendModifiedData.TabIndex = 2;
            this.lblSendModifiedData.Text = "Send only modified data of device tags";
            // 
            // chkIsBound
            // 
            this.chkIsBound.AutoSize = true;
            this.chkIsBound.Location = new System.Drawing.Point(430, 26);
            this.chkIsBound.Name = "chkIsBound";
            this.chkIsBound.Size = new System.Drawing.Size(15, 14);
            this.chkIsBound.TabIndex = 1;
            this.chkIsBound.UseVisualStyleBackColor = true;
            this.chkIsBound.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblIsBound
            // 
            this.lblIsBound.AutoSize = true;
            this.lblIsBound.Location = new System.Drawing.Point(17, 26);
            this.lblIsBound.Name = "lblIsBound";
            this.lblIsBound.Size = new System.Drawing.Size(276, 15);
            this.lblIsBound.TabIndex = 0;
            this.lblIsBound.Text = "Application is bound to the configuration database";
            // 
            // ctrlClientConnection
            // 
            this.ctrlClientConnection.ConnectionOptions = null;
            this.ctrlClientConnection.InstanceEnabled = false;
            this.ctrlClientConnection.Location = new System.Drawing.Point(12, 221);
            this.ctrlClientConnection.Name = "ctrlClientConnection";
            this.ctrlClientConnection.NameEnabled = false;
            this.ctrlClientConnection.Size = new System.Drawing.Size(500, 366);
            this.ctrlClientConnection.TabIndex = 1;
            this.ctrlClientConnection.ConnectionOptionsChanged += new System.EventHandler(this.control_Changed);
            // 
            // FrmGeneralOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 611);
            this.Controls.Add(this.ctrlClientConnection);
            this.Controls.Add(this.gbGeneralOptions);
            this.Name = "FrmGeneralOptions";
            this.Text = "General Options";
            this.Load += new System.EventHandler(this.FrmGeneralOptions_Load);
            this.gbGeneralOptions.ResumeLayout(false);
            this.gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAllDataPeriod)).EndInit();
            this.ResumeLayout(false);

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
    }
}