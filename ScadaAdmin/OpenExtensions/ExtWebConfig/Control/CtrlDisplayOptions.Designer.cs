namespace Scada.Admin.Extensions.ExtWebConfig.Control
{
    partial class CtrlDisplayOptions
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
            this.gbDisplayOptions = new System.Windows.Forms.GroupBox();
            this.numRefreshRate = new System.Windows.Forms.NumericUpDown();
            this.lblRefreshRate = new System.Windows.Forms.Label();
            this.chkShowViewExplorer = new System.Windows.Forms.CheckBox();
            this.lblShowViewExplorer = new System.Windows.Forms.Label();
            this.chkShowMainMenu = new System.Windows.Forms.CheckBox();
            this.lblShowMainMenu = new System.Windows.Forms.Label();
            this.chkShowHeader = new System.Windows.Forms.CheckBox();
            this.lblShowHeader = new System.Windows.Forms.Label();
            this.gbDisplayOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRefreshRate)).BeginInit();
            this.SuspendLayout();
            // 
            // gbDisplayOptions
            // 
            this.gbDisplayOptions.Controls.Add(this.numRefreshRate);
            this.gbDisplayOptions.Controls.Add(this.lblRefreshRate);
            this.gbDisplayOptions.Controls.Add(this.chkShowViewExplorer);
            this.gbDisplayOptions.Controls.Add(this.lblShowViewExplorer);
            this.gbDisplayOptions.Controls.Add(this.chkShowMainMenu);
            this.gbDisplayOptions.Controls.Add(this.lblShowMainMenu);
            this.gbDisplayOptions.Controls.Add(this.chkShowHeader);
            this.gbDisplayOptions.Controls.Add(this.lblShowHeader);
            this.gbDisplayOptions.Location = new System.Drawing.Point(0, 0);
            this.gbDisplayOptions.Name = "gbDisplayOptions";
            this.gbDisplayOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDisplayOptions.Size = new System.Drawing.Size(500, 145);
            this.gbDisplayOptions.TabIndex = 0;
            this.gbDisplayOptions.TabStop = false;
            this.gbDisplayOptions.Text = "Display Options";
            // 
            // numRefreshRate
            // 
            this.numRefreshRate.Location = new System.Drawing.Point(387, 109);
            this.numRefreshRate.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numRefreshRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRefreshRate.Name = "numRefreshRate";
            this.numRefreshRate.Size = new System.Drawing.Size(100, 23);
            this.numRefreshRate.TabIndex = 7;
            this.numRefreshRate.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numRefreshRate.ValueChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblRefreshRate
            // 
            this.lblRefreshRate.AutoSize = true;
            this.lblRefreshRate.Location = new System.Drawing.Point(10, 113);
            this.lblRefreshRate.Name = "lblRefreshRate";
            this.lblRefreshRate.Size = new System.Drawing.Size(91, 15);
            this.lblRefreshRate.TabIndex = 6;
            this.lblRefreshRate.Text = "Refresh rate, ms";
            // 
            // chkShowViewExplorer
            // 
            this.chkShowViewExplorer.AutoSize = true;
            this.chkShowViewExplorer.Location = new System.Drawing.Point(472, 84);
            this.chkShowViewExplorer.Name = "chkShowViewExplorer";
            this.chkShowViewExplorer.Size = new System.Drawing.Size(15, 14);
            this.chkShowViewExplorer.TabIndex = 5;
            this.chkShowViewExplorer.UseVisualStyleBackColor = true;
            this.chkShowViewExplorer.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblShowViewExplorer
            // 
            this.lblShowViewExplorer.AutoSize = true;
            this.lblShowViewExplorer.Location = new System.Drawing.Point(10, 84);
            this.lblShowViewExplorer.Name = "lblShowViewExplorer";
            this.lblShowViewExplorer.Size = new System.Drawing.Size(109, 15);
            this.lblShowViewExplorer.TabIndex = 4;
            this.lblShowViewExplorer.Text = "Show view explorer";
            // 
            // chkShowMainMenu
            // 
            this.chkShowMainMenu.AutoSize = true;
            this.chkShowMainMenu.Location = new System.Drawing.Point(472, 55);
            this.chkShowMainMenu.Name = "chkShowMainMenu";
            this.chkShowMainMenu.Size = new System.Drawing.Size(15, 14);
            this.chkShowMainMenu.TabIndex = 3;
            this.chkShowMainMenu.UseVisualStyleBackColor = true;
            this.chkShowMainMenu.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblShowMainMenu
            // 
            this.lblShowMainMenu.AutoSize = true;
            this.lblShowMainMenu.Location = new System.Drawing.Point(10, 55);
            this.lblShowMainMenu.Name = "lblShowMainMenu";
            this.lblShowMainMenu.Size = new System.Drawing.Size(100, 15);
            this.lblShowMainMenu.TabIndex = 2;
            this.lblShowMainMenu.Text = "Show main menu";
            // 
            // chkShowHeader
            // 
            this.chkShowHeader.AutoSize = true;
            this.chkShowHeader.Location = new System.Drawing.Point(472, 26);
            this.chkShowHeader.Name = "chkShowHeader";
            this.chkShowHeader.Size = new System.Drawing.Size(15, 14);
            this.chkShowHeader.TabIndex = 1;
            this.chkShowHeader.UseVisualStyleBackColor = true;
            this.chkShowHeader.CheckedChanged += new System.EventHandler(this.control_Changed);
            // 
            // lblShowHeader
            // 
            this.lblShowHeader.AutoSize = true;
            this.lblShowHeader.Location = new System.Drawing.Point(10, 26);
            this.lblShowHeader.Name = "lblShowHeader";
            this.lblShowHeader.Size = new System.Drawing.Size(104, 15);
            this.lblShowHeader.TabIndex = 0;
            this.lblShowHeader.Text = "Show page header";
            // 
            // CtrlDisplayOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDisplayOptions);
            this.Name = "CtrlDisplayOptions";
            this.Size = new System.Drawing.Size(550, 550);
            this.Load += new System.EventHandler(this.CtrlDisplayOptions_Load);
            this.gbDisplayOptions.ResumeLayout(false);
            this.gbDisplayOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRefreshRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbDisplayOptions;
        private CheckBox chkShowMainMenu;
        private Label lblShowMainMenu;
        private CheckBox chkShowHeader;
        private Label lblShowHeader;
        private CheckBox chkShowViewExplorer;
        private Label lblShowViewExplorer;
        private Label lblRefreshRate;
        private NumericUpDown numRefreshRate;
    }
}
