namespace Scada.Admin.Extensions.ExtImport.Forms
{
	partial class FrmCnlImport
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
			this.CtrlCnlImport1 = new Scada.Admin.Extensions.ExtImport.Controls.CtrlCnlImport1();
			this.CtrlCnlImport2 = new Scada.Admin.Extensions.ExtImport.Controls.CtrlCnlImport2();
			this.CtrlCnlImport3 = new Scada.Admin.Extensions.ExtImport.Controls.CtrlCnlImport3();
			this.CtrlCnlImport4 = new Scada.Admin.Extensions.ExtImport.Controls.CtrlCnlImport4();
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
			this.CtrlCnlImport1.Location = new System.Drawing.Point(12, 33);
			this.CtrlCnlImport1.Name = "ctrlCnlCreate1";
			this.CtrlCnlImport1.Size = new System.Drawing.Size(360, 200);
			this.CtrlCnlImport1.TabIndex = 1;
			this.CtrlCnlImport1.SelectedDeviceChanged += new System.EventHandler(this.ctrlCnlCreate1_SelectedDeviceChanged);
			// 
			// ctrlCnlCreate2
			// 
			this.CtrlCnlImport2.DeviceName = "";
			this.CtrlCnlImport2.Location = new System.Drawing.Point(12, 33);
			this.CtrlCnlImport2.Name = "ctrlCnlCreate2";
			this.CtrlCnlImport2.Size = new System.Drawing.Size(360, 100);
			this.CtrlCnlImport2.TabIndex = 2;
			// 
			// ctrlCnlCreate3
			// 
			//this.CtrlCnlImport3.DeviceName = "";
			this.CtrlCnlImport3.Location = new System.Drawing.Point(12, 33);
			this.CtrlCnlImport3.Name = "ctrlCnlCreate3";
			this.CtrlCnlImport3.Size = new System.Drawing.Size(360, 160);
			this.CtrlCnlImport3.TabIndex = 3;
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
			this.Controls.Add(this.CtrlCnlImport3);
			this.Controls.Add(this.CtrlCnlImport2);
			this.Controls.Add(this.CtrlCnlImport1);
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
		private Controls.CtrlCnlImport1 CtrlCnlImport1;
		private Controls.CtrlCnlImport2 CtrlCnlImport2;
		private Controls.CtrlCnlImport3 CtrlCnlImport3;
		private Controls.CtrlCnlImport4 CtrlCnlImport4;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
    }
}

