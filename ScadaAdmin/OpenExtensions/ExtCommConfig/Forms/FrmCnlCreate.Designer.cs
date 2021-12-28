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
            this.ctrlCnlCreate1 = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlCnlCreate1();
            this.ctrlCnlCreate2 = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlCnlCreate2();
            this.ctrlCnlCreate3 = new Scada.Admin.Extensions.ExtCommConfig.Controls.CtrlCnlCreate3();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            // ctrlCnlCreate1
            // 
            this.ctrlCnlCreate1.Location = new System.Drawing.Point(12, 33);
            this.ctrlCnlCreate1.Name = "ctrlCnlCreate1";
            this.ctrlCnlCreate1.Size = new System.Drawing.Size(360, 200);
            this.ctrlCnlCreate1.TabIndex = 1;
            this.ctrlCnlCreate1.SelectedDeviceChanged += new System.EventHandler(this.ctrlCnlCreate1_SelectedDeviceChanged);
            // 
            // ctrlCnlCreate2
            // 
            this.ctrlCnlCreate2.DeviceName = "";
            this.ctrlCnlCreate2.Location = new System.Drawing.Point(12, 33);
            this.ctrlCnlCreate2.Name = "ctrlCnlCreate2";
            this.ctrlCnlCreate2.Size = new System.Drawing.Size(360, 100);
            this.ctrlCnlCreate2.TabIndex = 2;
            // 
            // ctrlCnlCreate3
            // 
            this.ctrlCnlCreate3.DeviceName = "";
            this.ctrlCnlCreate3.Location = new System.Drawing.Point(12, 33);
            this.ctrlCnlCreate3.Name = "ctrlCnlCreate3";
            this.ctrlCnlCreate3.Size = new System.Drawing.Size(360, 160);
            this.ctrlCnlCreate3.TabIndex = 3;
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(12, 251);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(67, 19);
            this.chkPreview.TabIndex = 4;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(135, 249);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(216, 249);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 6;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(216, 249);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 249);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmCnlCreate
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 284);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.ctrlCnlCreate3);
            this.Controls.Add(this.ctrlCnlCreate2);
            this.Controls.Add(this.ctrlCnlCreate1);
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStep;
        private Controls.CtrlCnlCreate1 ctrlCnlCreate1;
        private Controls.CtrlCnlCreate2 ctrlCnlCreate2;
        private Controls.CtrlCnlCreate3 ctrlCnlCreate3;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
    }
}