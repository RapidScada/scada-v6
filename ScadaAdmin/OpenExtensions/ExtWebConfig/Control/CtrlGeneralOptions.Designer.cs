namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    partial class CtrlGeneralOptions
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
            this.gbGeneralOptions = new System.Windows.Forms.GroupBox();
            this.cbDefaultTimeZone = new System.Windows.Forms.ComboBox();
            this.lblMaxLogSize = new System.Windows.Forms.Label();
            this.numMaxLogSize = new System.Windows.Forms.NumericUpDown();
            this.chkShareStats = new System.Windows.Forms.CheckBox();
            this.lblShareStats = new System.Windows.Forms.Label();
            this.txtDefaultStartPage = new System.Windows.Forms.TextBox();
            this.txtDefaultCulture = new System.Windows.Forms.TextBox();
            this.chkEnableCommands = new System.Windows.Forms.CheckBox();
            this.lblEnableCommands = new System.Windows.Forms.Label();
            this.lblDefaultStartPage = new System.Windows.Forms.Label();
            this.lblDefaultTimeZone = new System.Windows.Forms.Label();
            this.lblDefaultCulture = new System.Windows.Forms.Label();
            this.gbGeneralOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).BeginInit();
            this.SuspendLayout();
            // 
            // gbGeneralOptions
            // 
            this.gbGeneralOptions.Controls.Add(this.cbDefaultTimeZone);
            this.gbGeneralOptions.Controls.Add(this.lblMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.numMaxLogSize);
            this.gbGeneralOptions.Controls.Add(this.chkShareStats);
            this.gbGeneralOptions.Controls.Add(this.lblShareStats);
            this.gbGeneralOptions.Controls.Add(this.txtDefaultStartPage);
            this.gbGeneralOptions.Controls.Add(this.txtDefaultCulture);
            this.gbGeneralOptions.Controls.Add(this.chkEnableCommands);
            this.gbGeneralOptions.Controls.Add(this.lblEnableCommands);
            this.gbGeneralOptions.Controls.Add(this.lblDefaultStartPage);
            this.gbGeneralOptions.Controls.Add(this.lblDefaultTimeZone);
            this.gbGeneralOptions.Controls.Add(this.lblDefaultCulture);
            this.gbGeneralOptions.Location = new System.Drawing.Point(0, 0);
            this.gbGeneralOptions.Name = "gbGeneralOptions";
            this.gbGeneralOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbGeneralOptions.Size = new System.Drawing.Size(500, 233);
            this.gbGeneralOptions.TabIndex = 2;
            this.gbGeneralOptions.TabStop = false;
            this.gbGeneralOptions.Text = "General Options";
            // 
            // cbDefaultTimeZone
            // 
            this.cbDefaultTimeZone.FormattingEnabled = true;
            this.cbDefaultTimeZone.Location = new System.Drawing.Point(287, 57);
            this.cbDefaultTimeZone.Name = "cbDefaultTimeZone";
            this.cbDefaultTimeZone.Size = new System.Drawing.Size(200, 23);
            this.cbDefaultTimeZone.TabIndex = 17;
            // 
            // lblMaxLogSize
            // 
            this.lblMaxLogSize.AutoSize = true;
            this.lblMaxLogSize.Location = new System.Drawing.Point(10, 201);
            this.lblMaxLogSize.Name = "lblMaxLogSize";
            this.lblMaxLogSize.Size = new System.Drawing.Size(187, 15);
            this.lblMaxLogSize.TabIndex = 16;
            this.lblMaxLogSize.Text = "Maximum log file size, megabytes";
            // 
            // numMaxLogSize
            // 
            this.numMaxLogSize.Location = new System.Drawing.Point(387, 197);
            this.numMaxLogSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMaxLogSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxLogSize.Name = "numMaxLogSize";
            this.numMaxLogSize.Size = new System.Drawing.Size(100, 23);
            this.numMaxLogSize.TabIndex = 15;
            this.numMaxLogSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkShareStats
            // 
            this.chkShareStats.AutoSize = true;
            this.chkShareStats.Location = new System.Drawing.Point(472, 172);
            this.chkShareStats.Name = "chkShareStats";
            this.chkShareStats.Size = new System.Drawing.Size(15, 14);
            this.chkShareStats.TabIndex = 14;
            this.chkShareStats.UseVisualStyleBackColor = true;
            // 
            // lblShareStats
            // 
            this.lblShareStats.AutoSize = true;
            this.lblShareStats.Location = new System.Drawing.Point(10, 172);
            this.lblShareStats.Name = "lblShareStats";
            this.lblShareStats.Size = new System.Drawing.Size(231, 15);
            this.lblShareStats.TabIndex = 13;
            this.lblShareStats.Text = "Share depersonalized stats with developers";
            // 
            // txtDefaultStartPage
            // 
            this.txtDefaultStartPage.Location = new System.Drawing.Point(13, 110);
            this.txtDefaultStartPage.Name = "txtDefaultStartPage";
            this.txtDefaultStartPage.Size = new System.Drawing.Size(474, 23);
            this.txtDefaultStartPage.TabIndex = 12;
            // 
            // txtDefaultCulture
            // 
            this.txtDefaultCulture.Location = new System.Drawing.Point(287, 26);
            this.txtDefaultCulture.Name = "txtDefaultCulture";
            this.txtDefaultCulture.Size = new System.Drawing.Size(200, 23);
            this.txtDefaultCulture.TabIndex = 10;
            // 
            // chkEnableCommands
            // 
            this.chkEnableCommands.AutoSize = true;
            this.chkEnableCommands.Location = new System.Drawing.Point(472, 143);
            this.chkEnableCommands.Name = "chkEnableCommands";
            this.chkEnableCommands.Size = new System.Drawing.Size(15, 14);
            this.chkEnableCommands.TabIndex = 7;
            this.chkEnableCommands.UseVisualStyleBackColor = true;
            // 
            // lblEnableCommands
            // 
            this.lblEnableCommands.AutoSize = true;
            this.lblEnableCommands.Location = new System.Drawing.Point(10, 143);
            this.lblEnableCommands.Name = "lblEnableCommands";
            this.lblEnableCommands.Size = new System.Drawing.Size(165, 15);
            this.lblEnableCommands.TabIndex = 6;
            this.lblEnableCommands.Text = "Enable telecontrol commands";
            // 
            // lblDefaultStartPage
            // 
            this.lblDefaultStartPage.AutoSize = true;
            this.lblDefaultStartPage.Location = new System.Drawing.Point(10, 92);
            this.lblDefaultStartPage.Name = "lblDefaultStartPage";
            this.lblDefaultStartPage.Size = new System.Drawing.Size(194, 15);
            this.lblDefaultStartPage.TabIndex = 4;
            this.lblDefaultStartPage.Text = "Default start page auto using opens";
            // 
            // lblDefaultTimeZone
            // 
            this.lblDefaultTimeZone.AutoSize = true;
            this.lblDefaultTimeZone.Location = new System.Drawing.Point(10, 61);
            this.lblDefaultTimeZone.Name = "lblDefaultTimeZone";
            this.lblDefaultTimeZone.Size = new System.Drawing.Size(150, 15);
            this.lblDefaultTimeZone.TabIndex = 2;
            this.lblDefaultTimeZone.Text = "Default time zone identifier";
            // 
            // lblDefaultCulture
            // 
            this.lblDefaultCulture.AutoSize = true;
            this.lblDefaultCulture.Location = new System.Drawing.Point(10, 30);
            this.lblDefaultCulture.Name = "lblDefaultCulture";
            this.lblDefaultCulture.Size = new System.Drawing.Size(118, 15);
            this.lblDefaultCulture.TabIndex = 0;
            this.lblDefaultCulture.Text = "Default culture name";
            // 
            // CtrlGeneralOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbGeneralOptions);
            this.Name = "CtrlGeneralOptions";
            this.Size = new System.Drawing.Size(550, 550);
            this.gbGeneralOptions.ResumeLayout(false);
            this.gbGeneralOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxLogSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbGeneralOptions;
        private Label lblMaxLogSize;
        private NumericUpDown numMaxLogSize;
        private CheckBox chkShareStats;
        private Label lblShareStats;
        private TextBox txtDefaultStartPage;
        private TextBox txtDefaultCulture;
        private CheckBox chkEnableCommands;
        private Label lblEnableCommands;
        private Label lblDefaultStartPage;
        private Label lblDefaultTimeZone;
        private Label lblDefaultCulture;
        private ComboBox cbDefaultTimeZone;
    }
}
