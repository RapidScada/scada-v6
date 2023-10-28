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
            chkShowEventView = new CheckBox();
            chkShowViewExplorer = new CheckBox();
            lblShowEventView = new Label();
            lblShowViewExplorer = new Label();
            chkShowMainMenu = new CheckBox();
            lblShowMainMenu = new Label();
            chkShowHeader = new CheckBox();
            lblShowHeader = new Label();
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
            gbDisplayOptions.Padding = new Padding(10, 3, 10, 12);
            gbDisplayOptions.Size = new Size(500, 213);
            gbDisplayOptions.TabIndex = 0;
            gbDisplayOptions.TabStop = false;
            gbDisplayOptions.Text = "Display Options";
            // 
            // numRefreshRate
            // 
            numRefreshRate.Location = new Point(387, 126);
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
            lblRefreshRate.AutoSize = true;
            lblRefreshRate.Location = new Point(10, 128);
            lblRefreshRate.Name = "lblRefreshRate";
            lblRefreshRate.Size = new Size(131, 17);
            lblRefreshRate.TabIndex = 6;
            lblRefreshRate.Text = "Data refresh rate, ms";
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
            // chkShowViewExplorer
            // 
            chkShowViewExplorer.AutoSize = true;
            chkShowViewExplorer.Location = new Point(472, 95);
            chkShowViewExplorer.Name = "chkShowViewExplorer";
            chkShowViewExplorer.Size = new Size(15, 14);
            chkShowViewExplorer.TabIndex = 5;
            chkShowViewExplorer.UseVisualStyleBackColor = true;
            chkShowViewExplorer.CheckedChanged += control_Changed;
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
            // lblShowViewExplorer
            // 
            lblShowViewExplorer.AutoSize = true;
            lblShowViewExplorer.Location = new Point(10, 95);
            lblShowViewExplorer.Name = "lblShowViewExplorer";
            lblShowViewExplorer.Size = new Size(121, 17);
            lblShowViewExplorer.TabIndex = 4;
            lblShowViewExplorer.Text = "Show view explorer";
            // 
            // chkShowMainMenu
            // 
            chkShowMainMenu.AutoSize = true;
            chkShowMainMenu.Location = new Point(472, 62);
            chkShowMainMenu.Name = "chkShowMainMenu";
            chkShowMainMenu.Size = new Size(15, 14);
            chkShowMainMenu.TabIndex = 3;
            chkShowMainMenu.UseVisualStyleBackColor = true;
            chkShowMainMenu.CheckedChanged += control_Changed;
            // 
            // lblShowMainMenu
            // 
            lblShowMainMenu.AutoSize = true;
            lblShowMainMenu.Location = new Point(10, 62);
            lblShowMainMenu.Name = "lblShowMainMenu";
            lblShowMainMenu.Size = new Size(107, 17);
            lblShowMainMenu.TabIndex = 2;
            lblShowMainMenu.Text = "Show main menu";
            // 
            // chkShowHeader
            // 
            chkShowHeader.AutoSize = true;
            chkShowHeader.Location = new Point(472, 29);
            chkShowHeader.Name = "chkShowHeader";
            chkShowHeader.Size = new Size(15, 14);
            chkShowHeader.TabIndex = 1;
            chkShowHeader.UseVisualStyleBackColor = true;
            chkShowHeader.CheckedChanged += control_Changed;
            // 
            // lblShowHeader
            // 
            lblShowHeader.AutoSize = true;
            lblShowHeader.Location = new Point(10, 33);
            lblShowHeader.Name = "lblShowHeader";
            lblShowHeader.Size = new Size(118, 17);
            lblShowHeader.TabIndex = 0;
            lblShowHeader.Text = "Show page header";
            // 
            // CtrlDisplayOptions
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbDisplayOptions);
            Name = "CtrlDisplayOptions";
            Size = new Size(550, 227);
            Load += CtrlDisplayOptions_Load;
            gbDisplayOptions.ResumeLayout(false);
            gbDisplayOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRefreshRate).EndInit();
            ResumeLayout(false);
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
