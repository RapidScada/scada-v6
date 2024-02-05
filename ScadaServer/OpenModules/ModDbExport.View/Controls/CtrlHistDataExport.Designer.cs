﻿namespace Scada.Server.Modules.ModDbExport.View.Controls
{
    partial class CtrlHistDataExport
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
            chkIncludeCalculated = new CheckBox();
            btnSelectHistArchiveBit = new Button();
            numHistArchiveBit = new NumericUpDown();
            lblHistArchiveBit = new Label();
            gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numHistArchiveBit).BeginInit();
            SuspendLayout();
            // 
            // gbOptions
            // 
            gbOptions.Controls.Add(btnSelectHistArchiveBit);
            gbOptions.Controls.Add(numHistArchiveBit);
            gbOptions.Controls.Add(lblHistArchiveBit);
            gbOptions.Controls.Add(chkIncludeCalculated);
            gbOptions.Dock = DockStyle.Fill;
            gbOptions.Location = new Point(0, 0);
            gbOptions.Name = "gbOptions";
            gbOptions.Padding = new Padding(10, 3, 10, 10);
            gbOptions.Size = new Size(404, 462);
            gbOptions.TabIndex = 0;
            gbOptions.TabStop = false;
            gbOptions.Text = "Historical Data Export Options";
            // 
            // chkIncludeCalculated
            // 
            chkIncludeCalculated.AutoSize = true;
            chkIncludeCalculated.Location = new Point(13, 22);
            chkIncludeCalculated.Margin = new Padding(3, 3, 3, 10);
            chkIncludeCalculated.Name = "chkIncludeCalculated";
            chkIncludeCalculated.Size = new Size(172, 19);
            chkIncludeCalculated.TabIndex = 0;
            chkIncludeCalculated.Text = "Include calculated channels";
            chkIncludeCalculated.UseVisualStyleBackColor = true;
            chkIncludeCalculated.CheckedChanged += chkIncludeCalculated_CheckedChanged;
            // 
            // btnSelectHistArchiveBit
            // 
            btnSelectHistArchiveBit.FlatStyle = FlatStyle.Popup;
            btnSelectHistArchiveBit.Image = Properties.Resources.find;
            btnSelectHistArchiveBit.Location = new Point(139, 69);
            btnSelectHistArchiveBit.Name = "btnSelectHistArchiveBit";
            btnSelectHistArchiveBit.Size = new Size(23, 23);
            btnSelectHistArchiveBit.TabIndex = 3;
            btnSelectHistArchiveBit.UseVisualStyleBackColor = true;
            btnSelectHistArchiveBit.Click += btnSelectHistArchiveBit_Click;
            // 
            // numHistArchiveBit
            // 
            numHistArchiveBit.Location = new Point(13, 69);
            numHistArchiveBit.Margin = new Padding(3, 3, 3, 10);
            numHistArchiveBit.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            numHistArchiveBit.Name = "numHistArchiveBit";
            numHistArchiveBit.Size = new Size(120, 23);
            numHistArchiveBit.TabIndex = 2;
            numHistArchiveBit.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numHistArchiveBit.ValueChanged += numHistArchiveBit_ValueChanged;
            // 
            // lblHistArchiveBit
            // 
            lblHistArchiveBit.AutoSize = true;
            lblHistArchiveBit.Location = new Point(10, 51);
            lblHistArchiveBit.Name = "lblHistArchiveBit";
            lblHistArchiveBit.Size = new Size(127, 15);
            lblHistArchiveBit.TabIndex = 1;
            lblHistArchiveBit.Text = "Bit of historical archive";
            // 
            // CtrlHistDataExport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gbOptions);
            Margin = new Padding(3, 3, 3, 10);
            Name = "CtrlHistDataExport";
            Size = new Size(404, 462);
            gbOptions.ResumeLayout(false);
            gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numHistArchiveBit).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbOptions;
        private CheckBox chkIncludeCalculated;
        private Button btnSelectHistArchiveBit;
        private NumericUpDown numHistArchiveBit;
        private Label lblHistArchiveBit;
    }
}
