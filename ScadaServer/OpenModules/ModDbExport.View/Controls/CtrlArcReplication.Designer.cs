namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    partial class CtrlArcReplication
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
            gbOptions = new GroupBox();
            btnSelectEventArchiveBit = new Button();
            numEventArchiveBit = new NumericUpDown();
            lblEventArchiveBit = new Label();
            btnSelectHistArchiveBit = new Button();
            numHistArchiveBit = new NumericUpDown();
            lblHistArchiveBit = new Label();
            numReadingStep = new NumericUpDown();
            lblReadingStep = new Label();
            numMaxDepth = new NumericUpDown();
            lblMaxDepth = new Label();
            numMinDepth = new NumericUpDown();
            lblMinDepth = new Label();
            chkAutoExport = new CheckBox();
            chkEnabled = new CheckBox();
            gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numEventArchiveBit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHistArchiveBit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numReadingStep).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxDepth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinDepth).BeginInit();
            SuspendLayout();
            // 
            // gbOptions
            // 
            gbOptions.Controls.Add(btnSelectEventArchiveBit);
            gbOptions.Controls.Add(numEventArchiveBit);
            gbOptions.Controls.Add(lblEventArchiveBit);
            gbOptions.Controls.Add(btnSelectHistArchiveBit);
            gbOptions.Controls.Add(numHistArchiveBit);
            gbOptions.Controls.Add(lblHistArchiveBit);
            gbOptions.Controls.Add(numReadingStep);
            gbOptions.Controls.Add(lblReadingStep);
            gbOptions.Controls.Add(numMaxDepth);
            gbOptions.Controls.Add(lblMaxDepth);
            gbOptions.Controls.Add(numMinDepth);
            gbOptions.Controls.Add(lblMinDepth);
            gbOptions.Controls.Add(chkAutoExport);
            gbOptions.Controls.Add(chkEnabled);
            gbOptions.Dock = DockStyle.Fill;
            gbOptions.Location = new Point(0, 0);
            gbOptions.Margin = new Padding(3, 3, 3, 10);
            gbOptions.Name = "gbOptions";
            gbOptions.Padding = new Padding(10, 3, 10, 10);
            gbOptions.Size = new Size(404, 462);
            gbOptions.TabIndex = 0;
            gbOptions.TabStop = false;
            gbOptions.Text = "Archive Replication Options";
            // 
            // btnSelectEventArchiveBit
            // 
            btnSelectEventArchiveBit.FlatStyle = FlatStyle.Popup;
            btnSelectEventArchiveBit.Image = Properties.Resources.find;
            btnSelectEventArchiveBit.Location = new Point(139, 305);
            btnSelectEventArchiveBit.Name = "btnSelectEventArchiveBit";
            btnSelectEventArchiveBit.Size = new Size(23, 23);
            btnSelectEventArchiveBit.TabIndex = 14;
            btnSelectEventArchiveBit.UseVisualStyleBackColor = true;
            btnSelectEventArchiveBit.Click += btnSelectEventArchiveBit_Click;
            // 
            // numEventArchiveBit
            // 
            numEventArchiveBit.Location = new Point(13, 305);
            numEventArchiveBit.Margin = new Padding(3, 3, 3, 10);
            numEventArchiveBit.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numEventArchiveBit.Name = "numEventArchiveBit";
            numEventArchiveBit.Size = new Size(120, 23);
            numEventArchiveBit.TabIndex = 13;
            numEventArchiveBit.Value = new decimal(new int[] { 4, 0, 0, 0 });
            numEventArchiveBit.ValueChanged += numEventArchiveBit_ValueChanged;
            // 
            // lblEventArchiveBit
            // 
            lblEventArchiveBit.AutoSize = true;
            lblEventArchiveBit.Location = new Point(10, 287);
            lblEventArchiveBit.Name = "lblEventArchiveBit";
            lblEventArchiveBit.Size = new Size(108, 15);
            lblEventArchiveBit.TabIndex = 12;
            lblEventArchiveBit.Text = "Bit of event archive";
            // 
            // btnSelectHistArchiveBit
            // 
            btnSelectHistArchiveBit.FlatStyle = FlatStyle.Popup;
            btnSelectHistArchiveBit.Image = Properties.Resources.find;
            btnSelectHistArchiveBit.Location = new Point(139, 254);
            btnSelectHistArchiveBit.Name = "btnSelectHistArchiveBit";
            btnSelectHistArchiveBit.Size = new Size(23, 23);
            btnSelectHistArchiveBit.TabIndex = 11;
            btnSelectHistArchiveBit.UseVisualStyleBackColor = true;
            btnSelectHistArchiveBit.Click += btnSelectHistArchiveBit_Click;
            // 
            // numHistArchiveBit
            // 
            numHistArchiveBit.Location = new Point(13, 254);
            numHistArchiveBit.Margin = new Padding(3, 3, 3, 10);
            numHistArchiveBit.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numHistArchiveBit.Name = "numHistArchiveBit";
            numHistArchiveBit.Size = new Size(120, 23);
            numHistArchiveBit.TabIndex = 10;
            numHistArchiveBit.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numHistArchiveBit.ValueChanged += numHistArchiveBit_ValueChanged;
            // 
            // lblHistArchiveBit
            // 
            lblHistArchiveBit.AutoSize = true;
            lblHistArchiveBit.Location = new Point(10, 236);
            lblHistArchiveBit.Name = "lblHistArchiveBit";
            lblHistArchiveBit.Size = new Size(127, 15);
            lblHistArchiveBit.TabIndex = 9;
            lblHistArchiveBit.Text = "Bit of historical archive";
            // 
            // numReadingStep
            // 
            numReadingStep.Location = new Point(13, 203);
            numReadingStep.Margin = new Padding(3, 3, 3, 10);
            numReadingStep.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numReadingStep.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numReadingStep.Name = "numReadingStep";
            numReadingStep.Size = new Size(120, 23);
            numReadingStep.TabIndex = 8;
            numReadingStep.Value = new decimal(new int[] { 60, 0, 0, 0 });
            numReadingStep.ValueChanged += numReadingStep_ValueChanged;
            // 
            // lblReadingStep
            // 
            lblReadingStep.AutoSize = true;
            lblReadingStep.Location = new Point(10, 185);
            lblReadingStep.Name = "lblReadingStep";
            lblReadingStep.Size = new Size(98, 15);
            lblReadingStep.TabIndex = 7;
            lblReadingStep.Text = "Reading step, sec";
            // 
            // numMaxDepth
            // 
            numMaxDepth.Location = new Point(13, 152);
            numMaxDepth.Margin = new Padding(3, 3, 3, 10);
            numMaxDepth.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numMaxDepth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxDepth.Name = "numMaxDepth";
            numMaxDepth.Size = new Size(120, 23);
            numMaxDepth.TabIndex = 6;
            numMaxDepth.Value = new decimal(new int[] { 3600, 0, 0, 0 });
            numMaxDepth.ValueChanged += numMaxDepth_ValueChanged;
            // 
            // lblMaxDepth
            // 
            lblMaxDepth.AutoSize = true;
            lblMaxDepth.Location = new Point(10, 134);
            lblMaxDepth.Name = "lblMaxDepth";
            lblMaxDepth.Size = new Size(116, 15);
            lblMaxDepth.TabIndex = 5;
            lblMaxDepth.Text = "Maximum depth,sec";
            // 
            // numMinDepth
            // 
            numMinDepth.Location = new Point(13, 101);
            numMinDepth.Margin = new Padding(3, 3, 3, 10);
            numMinDepth.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numMinDepth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMinDepth.Name = "numMinDepth";
            numMinDepth.Size = new Size(120, 23);
            numMinDepth.TabIndex = 4;
            numMinDepth.Value = new decimal(new int[] { 10, 0, 0, 0 });
            numMinDepth.ValueChanged += numMinDepth_ValueChanged;
            // 
            // lblMinDepth
            // 
            lblMinDepth.AutoSize = true;
            lblMinDepth.Location = new Point(10, 83);
            lblMinDepth.Name = "lblMinDepth";
            lblMinDepth.Size = new Size(117, 15);
            lblMinDepth.TabIndex = 3;
            lblMinDepth.Text = "Minimum depth, sec";
            // 
            // chkAutoExport
            // 
            chkAutoExport.AutoSize = true;
            chkAutoExport.Location = new Point(13, 54);
            chkAutoExport.Margin = new Padding(3, 3, 3, 10);
            chkAutoExport.Name = "chkAutoExport";
            chkAutoExport.Size = new Size(183, 19);
            chkAutoExport.TabIndex = 2;
            chkAutoExport.Text = "Automatically export archives";
            chkAutoExport.UseVisualStyleBackColor = true;
            chkAutoExport.CheckedChanged += chkAutoExport_CheckedChanged;
            // 
            // chkEnabled
            // 
            chkEnabled.AutoSize = true;
            chkEnabled.Location = new Point(13, 22);
            chkEnabled.Margin = new Padding(3, 3, 3, 10);
            chkEnabled.Name = "chkEnabled";
            chkEnabled.Size = new Size(68, 19);
            chkEnabled.TabIndex = 1;
            chkEnabled.Text = "Enabled";
            chkEnabled.UseVisualStyleBackColor = true;
            chkEnabled.CheckedChanged += chkEnabled_CheckedChanged;
            // 
            // CtrlArcReplication
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbOptions);
            Margin = new Padding(3, 3, 3, 10);
            Name = "CtrlArcReplication";
            Size = new Size(404, 462);
            gbOptions.ResumeLayout(false);
            gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numEventArchiveBit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHistArchiveBit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numReadingStep).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxDepth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinDepth).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbOptions;
        private CheckBox chkEnabled;
        private CheckBox chkAutoExport;
        private NumericUpDown numReadingStep;
        private Label lblReadingStep;
        private NumericUpDown numMaxDepth;
        private Label lblMaxDepth;
        private NumericUpDown numMinDepth;
        private Label lblMinDepth;
        private Button btnSelectEventArchiveBit;
        private NumericUpDown numEventArchiveBit;
        private Label lblEventArchiveBit;
        private Button btnSelectHistArchiveBit;
        private NumericUpDown numHistArchiveBit;
        private Label lblHistArchiveBit;
    }
}
