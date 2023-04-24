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
			lblStep = new Label();
			ctrlCnlImport1 = new Controls.CtrlCnlImport1();
			ctrlCnlImport2 = new Controls.CtrlCnlImport2();
			ctrlCnlImport3 = new Controls.CtrlCnlImport3();
			chkPreview = new CheckBox();
			btnBack = new Button();
			btnNext = new Button();
			btnCreate = new Button();
			btnCancel = new Button();
			SuspendLayout();
			// 
			// lblStep
			// 
			lblStep.BackColor = SystemColors.ActiveCaption;
			lblStep.Dock = DockStyle.Top;
			lblStep.Location = new Point(0, 0);
			lblStep.Name = "lblStep";
			lblStep.Size = new Size(384, 30);
			lblStep.TabIndex = 0;
			lblStep.Text = "Step 1 of 3: Step description";
			lblStep.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ctrlCnlImport1
			// 
			ctrlCnlImport1.Location = new Point(12, 33);
			ctrlCnlImport1.Margin = new Padding(3, 4, 3, 4);
			ctrlCnlImport1.Name = "ctrlCnlImport1";
			ctrlCnlImport1.Size = new Size(360, 200);
			ctrlCnlImport1.TabIndex = 1;
			ctrlCnlImport1.SelectedDeviceChanged += ctrlCnlImport1_SelectedDeviceChanged;
			// 
			// ctrlCnlImport2
			// 
			ctrlCnlImport2.DeviceName = "";
			ctrlCnlImport2.Location = new Point(12, 33);
			ctrlCnlImport2.Margin = new Padding(3, 4, 3, 4);
			ctrlCnlImport2.Name = "ctrlCnlImport2";
			ctrlCnlImport2.Size = new Size(360, 100);
			ctrlCnlImport2.TabIndex = 2;
			// 
			// ctrlCnlImport3
			// 
			ctrlCnlImport3.Location = new Point(0, 22);
			ctrlCnlImport3.Name = "ctrlCnlImport3";
			ctrlCnlImport3.Size = new Size(384, 223);
			ctrlCnlImport3.TabIndex = 3;
			ctrlCnlImport3.Load += ctrlCnlImport3_Load;
			// 
			// chkPreview
			// 
			chkPreview.AutoSize = true;
			chkPreview.Checked = true;
			chkPreview.CheckState = CheckState.Checked;
			chkPreview.Location = new Point(12, 251);
			chkPreview.Name = "chkPreview";
			chkPreview.Size = new Size(67, 19);
			chkPreview.TabIndex = 4;
			chkPreview.Text = "Preview";
			chkPreview.UseVisualStyleBackColor = true;
			// 
			// btnBack
			// 
			btnBack.Location = new Point(135, 249);
			btnBack.Name = "btnBack";
			btnBack.Size = new Size(75, 23);
			btnBack.TabIndex = 5;
			btnBack.Text = "< Back";
			btnBack.UseVisualStyleBackColor = true;
			btnBack.Click += btnBack_Click;
			// 
			// btnNext
			// 
			btnNext.Location = new Point(216, 249);
			btnNext.Name = "btnNext";
			btnNext.Size = new Size(75, 23);
			btnNext.TabIndex = 6;
			btnNext.Text = "Next >";
			btnNext.UseVisualStyleBackColor = true;
			btnNext.Click += btnNext_Click;
			// 
			// btnCreate
			// 
			btnCreate.Location = new Point(216, 249);
			btnCreate.Name = "btnCreate";
			btnCreate.Size = new Size(75, 23);
			btnCreate.TabIndex = 7;
			btnCreate.Text = "Create";
			btnCreate.UseVisualStyleBackColor = true;
			btnCreate.Click += btnCreate_Click;
			// 
			// btnCancel
			// 
			btnCancel.Location = new Point(297, 249);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(75, 23);
			btnCancel.TabIndex = 8;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// FrmCnlImport
			// 
			AcceptButton = btnCreate;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(384, 274);
			Controls.Add(btnCancel);
			Controls.Add(btnCreate);
			Controls.Add(btnNext);
			Controls.Add(btnBack);
			Controls.Add(chkPreview);
			Controls.Add(ctrlCnlImport3);
			Controls.Add(ctrlCnlImport2);
			Controls.Add(ctrlCnlImport1);
			Controls.Add(lblStep);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FrmCnlImport";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Import Channels";
			Load += FrmCnlImport_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblStep;
		private Controls.CtrlCnlImport1 ctrlCnlImport1;
		private Controls.CtrlCnlImport2 ctrlCnlImport2;
		private Controls.CtrlCnlImport3 ctrlCnlImport3;
		private System.Windows.Forms.CheckBox chkPreview;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnCancel;
	}
}