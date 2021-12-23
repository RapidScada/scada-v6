namespace Scada.Admin.Extensions.ExtCommConfig.Forms
{
    partial class FrmSync
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
            this.ctrlSync1 = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlSync1();
            this.ctrlSync2 = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlSync2();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctrlSync1
            // 
            this.ctrlSync1.Location = new System.Drawing.Point(12, 12);
            this.ctrlSync1.Name = "ctrlSync1";
            this.ctrlSync1.Size = new System.Drawing.Size(350, 220);
            this.ctrlSync1.TabIndex = 0;
            // 
            // ctrlSync2
            // 
            this.ctrlSync2.Location = new System.Drawing.Point(12, 238);
            this.ctrlSync2.Name = "ctrlSync2";
            this.ctrlSync2.Size = new System.Drawing.Size(360, 418);
            this.ctrlSync2.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(125, 446);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSync
            // 
            this.btnSync.Location = new System.Drawing.Point(211, 446);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(80, 23);
            this.btnSync.TabIndex = 3;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 446);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmSync
            // 
            this.AcceptButton = this.btnSync;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 481);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSync);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.ctrlSync2);
            this.Controls.Add(this.ctrlSync1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSync";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Synchronize Lines and Devices";
            this.Load += new System.EventHandler(this.FrmSync_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CtrlSync1 ctrlSync1;
        private Controls.CtrlSync2 ctrlSync2;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnCancel;
    }
}