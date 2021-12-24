namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmCnlCreate
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
            this.lblStep = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStep
            // 
            this.lblStep.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblStep.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStep.Location = new System.Drawing.Point(0, 0);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(384, 30);
            this.lblStep.TabIndex = 0;
            this.lblStep.Text = "Step 1 of 3: Step description";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmCnlCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 450);
            this.Controls.Add(this.lblStep);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCnlCreate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Channels";
            this.Load += new System.EventHandler(this.FrmCnlCreate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStep;
    }
}