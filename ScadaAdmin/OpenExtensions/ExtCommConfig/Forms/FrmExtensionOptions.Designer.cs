namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmExtensionOptions
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
            this.lblMultiplicity = new System.Windows.Forms.Label();
            this.numMultiplicity = new System.Windows.Forms.NumericUpDown();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.lblShift = new System.Windows.Forms.Label();
            this.numShift = new System.Windows.Forms.NumericUpDown();
            this.lblGap = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.chkPrependDeviceName = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numMultiplicity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMultiplicity
            // 
            this.lblMultiplicity.AutoSize = true;
            this.lblMultiplicity.Location = new System.Drawing.Point(9, 9);
            this.lblMultiplicity.Name = "lblMultiplicity";
            this.lblMultiplicity.Size = new System.Drawing.Size(67, 15);
            this.lblMultiplicity.TabIndex = 0;
            this.lblMultiplicity.Text = "Multiplicity";
            // 
            // numMultiplicity
            // 
            this.numMultiplicity.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMultiplicity.Location = new System.Drawing.Point(12, 27);
            this.numMultiplicity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMultiplicity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMultiplicity.Name = "numMultiplicity";
            this.numMultiplicity.Size = new System.Drawing.Size(150, 23);
            this.numMultiplicity.TabIndex = 1;
            this.numMultiplicity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblExplanation
            // 
            this.lblExplanation.AutoSize = true;
            this.lblExplanation.Location = new System.Drawing.Point(168, 31);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(38, 15);
            this.lblExplanation.TabIndex = 2;
            this.lblExplanation.Text = "× N +";
            // 
            // lblShift
            // 
            this.lblShift.AutoSize = true;
            this.lblShift.Location = new System.Drawing.Point(219, 9);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(31, 15);
            this.lblShift.TabIndex = 3;
            this.lblShift.Text = "Shift";
            // 
            // numShift
            // 
            this.numShift.Location = new System.Drawing.Point(222, 27);
            this.numShift.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numShift.Name = "numShift";
            this.numShift.Size = new System.Drawing.Size(150, 23);
            this.numShift.TabIndex = 4;
            this.numShift.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblGap
            // 
            this.lblGap.AutoSize = true;
            this.lblGap.Location = new System.Drawing.Point(9, 53);
            this.lblGap.Name = "lblGap";
            this.lblGap.Size = new System.Drawing.Size(28, 15);
            this.lblGap.TabIndex = 5;
            this.lblGap.Text = "Gap";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(12, 71);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(150, 23);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chkPrependDeviceName
            // 
            this.chkPrependDeviceName.AutoSize = true;
            this.chkPrependDeviceName.Location = new System.Drawing.Point(12, 100);
            this.chkPrependDeviceName.Name = "chkPrependDeviceName";
            this.chkPrependDeviceName.Size = new System.Drawing.Size(140, 19);
            this.chkPrependDeviceName.TabIndex = 7;
            this.chkPrependDeviceName.Text = "Prepend device name";
            this.chkPrependDeviceName.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 135);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 135);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmExtensionOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 170);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkPrependDeviceName);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.lblGap);
            this.Controls.Add(this.numShift);
            this.Controls.Add(this.lblShift);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.numMultiplicity);
            this.Controls.Add(this.lblMultiplicity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExtensionOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extension Options";
            ((System.ComponentModel.ISupportInitialize)(this.numMultiplicity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMultiplicity;
        private System.Windows.Forms.NumericUpDown numMultiplicity;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.NumericUpDown numShift;
        private System.Windows.Forms.Label lblGap;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.CheckBox chkPrependDeviceName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}