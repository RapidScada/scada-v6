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
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnSelectEventArchiveBit = new System.Windows.Forms.Button();
            this.numEventArchiveBit = new System.Windows.Forms.NumericUpDown();
            this.lblEventArchiveBit = new System.Windows.Forms.Label();
            this.btnSelectHistArchiveBit = new System.Windows.Forms.Button();
            this.numHistArchiveBit = new System.Windows.Forms.NumericUpDown();
            this.lblHistArchiveBit = new System.Windows.Forms.Label();
            this.numReadingStep = new System.Windows.Forms.NumericUpDown();
            this.lblReadingStep = new System.Windows.Forms.Label();
            this.numMaxDepth = new System.Windows.Forms.NumericUpDown();
            this.lblMaxDepth = new System.Windows.Forms.Label();
            this.numMinDepth = new System.Windows.Forms.NumericUpDown();
            this.lblMinDepth = new System.Windows.Forms.Label();
            this.chkAutoExport = new System.Windows.Forms.CheckBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEventArchiveBit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHistArchiveBit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadingStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.btnSelectEventArchiveBit);
            this.gbOptions.Controls.Add(this.numEventArchiveBit);
            this.gbOptions.Controls.Add(this.lblEventArchiveBit);
            this.gbOptions.Controls.Add(this.btnSelectHistArchiveBit);
            this.gbOptions.Controls.Add(this.numHistArchiveBit);
            this.gbOptions.Controls.Add(this.lblHistArchiveBit);
            this.gbOptions.Controls.Add(this.numReadingStep);
            this.gbOptions.Controls.Add(this.lblReadingStep);
            this.gbOptions.Controls.Add(this.numMaxDepth);
            this.gbOptions.Controls.Add(this.lblMaxDepth);
            this.gbOptions.Controls.Add(this.numMinDepth);
            this.gbOptions.Controls.Add(this.lblMinDepth);
            this.gbOptions.Controls.Add(this.chkAutoExport);
            this.gbOptions.Controls.Add(this.chkEnabled);
            this.gbOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOptions.Location = new System.Drawing.Point(0, 0);
            this.gbOptions.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbOptions.Size = new System.Drawing.Size(404, 462);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Archive Replication Options";
            // 
            // btnSelectEventArchiveBit
            // 
            this.btnSelectEventArchiveBit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectEventArchiveBit.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectEventArchiveBit.Location = new System.Drawing.Point(139, 305);
            this.btnSelectEventArchiveBit.Name = "btnSelectEventArchiveBit";
            this.btnSelectEventArchiveBit.Size = new System.Drawing.Size(23, 23);
            this.btnSelectEventArchiveBit.TabIndex = 14;
            this.btnSelectEventArchiveBit.UseVisualStyleBackColor = true;
            this.btnSelectEventArchiveBit.Click += new System.EventHandler(this.btnSelectEventArchiveBit_Click);
            // 
            // numEventArchiveBit
            // 
            this.numEventArchiveBit.Location = new System.Drawing.Point(13, 305);
            this.numEventArchiveBit.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numEventArchiveBit.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numEventArchiveBit.Name = "numEventArchiveBit";
            this.numEventArchiveBit.Size = new System.Drawing.Size(120, 23);
            this.numEventArchiveBit.TabIndex = 13;
            this.numEventArchiveBit.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numEventArchiveBit.ValueChanged += new System.EventHandler(this.numEventArchiveBit_ValueChanged);
            // 
            // lblEventArchiveBit
            // 
            this.lblEventArchiveBit.AutoSize = true;
            this.lblEventArchiveBit.Location = new System.Drawing.Point(10, 287);
            this.lblEventArchiveBit.Name = "lblEventArchiveBit";
            this.lblEventArchiveBit.Size = new System.Drawing.Size(108, 15);
            this.lblEventArchiveBit.TabIndex = 12;
            this.lblEventArchiveBit.Text = "Bit of event archive";
            // 
            // btnSelectHistArchiveBit
            // 
            this.btnSelectHistArchiveBit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectHistArchiveBit.Image = global::Scada.Server.Modules.ModDbExport.View.Properties.Resources.find;
            this.btnSelectHistArchiveBit.Location = new System.Drawing.Point(139, 254);
            this.btnSelectHistArchiveBit.Name = "btnSelectHistArchiveBit";
            this.btnSelectHistArchiveBit.Size = new System.Drawing.Size(23, 23);
            this.btnSelectHistArchiveBit.TabIndex = 11;
            this.btnSelectHistArchiveBit.UseVisualStyleBackColor = true;
            this.btnSelectHistArchiveBit.Click += new System.EventHandler(this.btnSelectHistArchiveBit_Click);
            // 
            // numHistArchiveBit
            // 
            this.numHistArchiveBit.Location = new System.Drawing.Point(13, 254);
            this.numHistArchiveBit.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numHistArchiveBit.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numHistArchiveBit.Name = "numHistArchiveBit";
            this.numHistArchiveBit.Size = new System.Drawing.Size(120, 23);
            this.numHistArchiveBit.TabIndex = 10;
            this.numHistArchiveBit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHistArchiveBit.ValueChanged += new System.EventHandler(this.numHistArchiveBit_ValueChanged);
            // 
            // lblHistArchiveBit
            // 
            this.lblHistArchiveBit.AutoSize = true;
            this.lblHistArchiveBit.Location = new System.Drawing.Point(10, 236);
            this.lblHistArchiveBit.Name = "lblHistArchiveBit";
            this.lblHistArchiveBit.Size = new System.Drawing.Size(127, 15);
            this.lblHistArchiveBit.TabIndex = 9;
            this.lblHistArchiveBit.Text = "Bit of historical archive";
            // 
            // numReadingStep
            // 
            this.numReadingStep.Location = new System.Drawing.Point(13, 203);
            this.numReadingStep.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numReadingStep.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numReadingStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReadingStep.Name = "numReadingStep";
            this.numReadingStep.Size = new System.Drawing.Size(120, 23);
            this.numReadingStep.TabIndex = 8;
            this.numReadingStep.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numReadingStep.ValueChanged += new System.EventHandler(this.numReadingStep_ValueChanged);
            // 
            // lblReadingStep
            // 
            this.lblReadingStep.AutoSize = true;
            this.lblReadingStep.Location = new System.Drawing.Point(10, 185);
            this.lblReadingStep.Name = "lblReadingStep";
            this.lblReadingStep.Size = new System.Drawing.Size(98, 15);
            this.lblReadingStep.TabIndex = 7;
            this.lblReadingStep.Text = "Reading step, sec";
            // 
            // numMaxDepth
            // 
            this.numMaxDepth.Location = new System.Drawing.Point(13, 152);
            this.numMaxDepth.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numMaxDepth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numMaxDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxDepth.Name = "numMaxDepth";
            this.numMaxDepth.Size = new System.Drawing.Size(120, 23);
            this.numMaxDepth.TabIndex = 6;
            this.numMaxDepth.Value = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.numMaxDepth.ValueChanged += new System.EventHandler(this.numMaxDepth_ValueChanged);
            // 
            // lblMaxDepth
            // 
            this.lblMaxDepth.AutoSize = true;
            this.lblMaxDepth.Location = new System.Drawing.Point(10, 134);
            this.lblMaxDepth.Name = "lblMaxDepth";
            this.lblMaxDepth.Size = new System.Drawing.Size(116, 15);
            this.lblMaxDepth.TabIndex = 5;
            this.lblMaxDepth.Text = "Maximum depth,sec";
            // 
            // numMinDepth
            // 
            this.numMinDepth.Location = new System.Drawing.Point(13, 101);
            this.numMinDepth.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.numMinDepth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numMinDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinDepth.Name = "numMinDepth";
            this.numMinDepth.Size = new System.Drawing.Size(120, 23);
            this.numMinDepth.TabIndex = 4;
            this.numMinDepth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMinDepth.ValueChanged += new System.EventHandler(this.numMinDepth_ValueChanged);
            // 
            // lblMinDepth
            // 
            this.lblMinDepth.AutoSize = true;
            this.lblMinDepth.Location = new System.Drawing.Point(10, 83);
            this.lblMinDepth.Name = "lblMinDepth";
            this.lblMinDepth.Size = new System.Drawing.Size(117, 15);
            this.lblMinDepth.TabIndex = 3;
            this.lblMinDepth.Text = "Minimum depth, sec";
            // 
            // chkAutoExport
            // 
            this.chkAutoExport.AutoSize = true;
            this.chkAutoExport.Location = new System.Drawing.Point(13, 54);
            this.chkAutoExport.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkAutoExport.Name = "chkAutoExport";
            this.chkAutoExport.Size = new System.Drawing.Size(183, 19);
            this.chkAutoExport.TabIndex = 2;
            this.chkAutoExport.Text = "Automatically export archives";
            this.chkAutoExport.UseVisualStyleBackColor = true;
            this.chkAutoExport.CheckedChanged += new System.EventHandler(this.chkAutoExport_CheckedChanged);
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(13, 22);
            this.chkEnabled.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(68, 19);
            this.chkEnabled.TabIndex = 1;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // CtrlArcReplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOptions);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.Name = "CtrlArcReplication";
            this.Size = new System.Drawing.Size(404, 462);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEventArchiveBit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHistArchiveBit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadingStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinDepth)).EndInit();
            this.ResumeLayout(false);

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
