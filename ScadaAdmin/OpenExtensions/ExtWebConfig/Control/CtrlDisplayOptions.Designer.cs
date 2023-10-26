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
            gbDisplayOptions = new GroupBox();
            numRefreshRate = new NumericUpDown();
            lblRefreshRate = new Label();
            chkShowViewExplorer = new CheckBox();
            lblShowViewExplorer = new Label();
            chkShowMainMenu = new CheckBox();
            lblShowMainMenu = new Label();
            chkShowHeader = new CheckBox();
            lblShowHeader = new Label();
            lblShowEventView = new Label();
            chkShowEventView = new CheckBox();
            gbDisplayOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRefreshRate).BeginInit();
            SuspendLayout();
            // 
            // gbDisplayOptions
            // 
            gbDisplayOptions.Controls.Add(numRefreshRate);
            gbDisplayOptions.Controls.Add(lblRefreshRate);
            gbDisplayOptions.Controls.Add(chkShowEventView);
            gbDisplayOptions.Controls.Add(chkShowViewExplorer);
            gbDisplayOptions.Controls.Add(lblShowEventView);
            gbDisplayOptions.Controls.Add(lblShowViewExplorer);
            gbDisplayOptions.Controls.Add(chkShowMainMenu);
            gbDisplayOptions.Controls.Add(lblShowMainMenu);
            gbDisplayOptions.Controls.Add(chkShowHeader);
            gbDisplayOptions.Controls.Add(lblShowHeader);
            gbDisplayOptions.Location = new Point(0, 0);
            gbDisplayOptions.Name = "gbDisplayOptions";
            gbDisplayOptions.Padding = new Padding(10, 3, 10, 11);
            gbDisplayOptions.Size = new Size(500, 209);
            gbDisplayOptions.TabIndex = 0;
            gbDisplayOptions.TabStop = false;
            gbDisplayOptions.Text = "Display Options";
            // 
            // numRefreshRate
            // 
            numRefreshRate.Location = new Point(387, 124);
            numRefreshRate.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numRefreshRate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numRefreshRate.Name = "numRefreshRate";
            numRefreshRate.Size = new Size(100, 23);
            numRefreshRate.TabIndex = 7;
            numRefreshRate.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            numRefreshRate.ValueChanged += control_Changed;
            // 
            // lblRefreshRate
            // 
            this.lblRefreshRate.AutoSize = true;
            this.lblRefreshRate.Location = new System.Drawing.Point(10, 113);
            this.lblRefreshRate.Name = "lblRefreshRate";
            this.lblRefreshRate.Size = new System.Drawing.Size(115, 15);
            this.lblRefreshRate.TabIndex = 6;
            this.lblRefreshRate.Text = "Data refresh rate, ms";
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
            lblShowHeader.AutoSize = true;
            lblShowHeader.Location = new Point(10, 29);
            lblShowHeader.Name = "lblShowHeader";
            lblShowHeader.Size = new Size(118, 17);
            lblShowHeader.TabIndex = 0;
            lblShowHeader.Text = "Show page header";
            // 
            // lblShowEventView
            // 
            lblShowEventView.AutoSize = true;
            lblShowEventView.Location = new Point(10, 162);
            lblShowEventView.Name = "lblShowEventView";
            lblShowEventView.Size = new Size(103, 17);
            lblShowEventView.TabIndex = 4;
            lblShowEventView.Text = "Show event view";
            // 
            // chkShowEventView
            // 
            chkShowEventView.AutoSize = true;
            chkShowEventView.Location = new Point(472, 162);
            chkShowEventView.Name = "chkShowEventView";
            chkShowEventView.Size = new Size(15, 14);
            chkShowEventView.TabIndex = 5;
            chkShowEventView.UseVisualStyleBackColor = true;
            chkShowEventView.CheckedChanged += control_Changed;
            // 
            // CtrlDisplayOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDisplayOptions);
            this.Name = "CtrlDisplayOptions";
            this.Size = new System.Drawing.Size(550, 200);
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
        private CheckBox chkShowEventView;
        private Label lblShowEventView;
    }
}
