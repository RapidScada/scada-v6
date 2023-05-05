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
			CtrlCnlImport1 = new Controls.CtrlCnlImport1();
			CtrlCnlImport2 = new Controls.CtrlCnlImport2();
			CtrlCnlImport3 = new Controls.CtrlCnlImport3();
			CtrlCnlImport4 = new Controls.CtrlCnlImport4();
			//chkPreview = new CheckBox();
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
			lblStep.Size = new Size(439, 40);
			lblStep.TabIndex = 0;
			lblStep.Text = "Step 1 of 3: Step description";
			lblStep.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// CtrlCnlImport1
			// 
			CtrlCnlImport1.Location = new Point(14, 44);
			CtrlCnlImport1.Margin = new Padding(3, 5, 3, 5);
			CtrlCnlImport1.Name = "CtrlCnlImport1";
			CtrlCnlImport1.Size = new Size(411, 267);
			CtrlCnlImport1.TabIndex = 1;
			CtrlCnlImport1.SelectedDeviceChanged += ctrlCnlCreate1_SelectedDeviceChanged;
			// 
			// CtrlCnlImport2
			// 
			CtrlCnlImport2.DeviceName = "";
			CtrlCnlImport2.Location = new Point(14, 44);
			CtrlCnlImport2.Margin = new Padding(3, 5, 3, 5);
			CtrlCnlImport2.Name = "CtrlCnlImport2";
			CtrlCnlImport2.Size = new Size(411, 133);
			CtrlCnlImport2.TabIndex = 2;
			// 
			// CtrlCnlImport3
			// 
			CtrlCnlImport3.ctrlCnlImport4 = null;
			CtrlCnlImport3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			CtrlCnlImport3.Location = new Point(14, 44);
			CtrlCnlImport3.Margin = new Padding(3, 4, 3, 4);
			CtrlCnlImport3.Name = "CtrlCnlImport3";
			CtrlCnlImport3.Size = new Size(411, 213);
			CtrlCnlImport3.TabIndex = 3;
			// 
			// CtrlCnlImport4
			// 
			CtrlCnlImport4.Location = new Point(0, 0);
			CtrlCnlImport4.Margin = new Padding(3, 4, 3, 4);
			CtrlCnlImport4.Name = "CtrlCnlImport4";
			CtrlCnlImport4.Size = new Size(1153, 564);
			CtrlCnlImport4.TabIndex = 0;
			// 
			// chkPreview
			// 
			//chkPreview.AutoSize = true;
			//chkPreview.Checked = true;
			//chkPreview.CheckState = CheckState.Checked;
			//chkPreview.Location = new Point(14, 335);
			//chkPreview.Margin = new Padding(3, 4, 3, 4);
			//chkPreview.Name = "chkPreview";
			//chkPreview.Size = new Size(82, 24);
			//chkPreview.TabIndex = 4;
			//chkPreview.Text = "Preview";
			//chkPreview.UseVisualStyleBackColor = true;
			// 
			// btnBack
			// 
			btnBack.Location = new Point(154, 332);
			btnBack.Margin = new Padding(3, 4, 3, 4);
			btnBack.Name = "btnBack";
			btnBack.Size = new Size(86, 31);
			btnBack.TabIndex = 5;
			btnBack.Text = "< Back";
			btnBack.UseVisualStyleBackColor = true;
			btnBack.Click += btnBack_Click;
			// 
			// btnNext
			// 
			btnNext.Location = new Point(247, 332);
			btnNext.Margin = new Padding(3, 4, 3, 4);
			btnNext.Name = "btnNext";
			btnNext.Size = new Size(86, 31);
			btnNext.TabIndex = 6;
			btnNext.Text = "Next >";
			btnNext.UseVisualStyleBackColor = true;
			btnNext.Click += btnNext_Click;
			// 
			// btnCreate
			// 
			btnCreate.Location = new Point(247, 332);
			btnCreate.Margin = new Padding(3, 4, 3, 4);
			btnCreate.Name = "btnCreate";
			btnCreate.Size = new Size(86, 31);
			btnCreate.TabIndex = 7;
			btnCreate.Text = "Next >";
			btnCreate.UseVisualStyleBackColor = true;
			btnCreate.Click += btnCreate_Click;
			// 
			// btnCancel
			// 
			btnCancel.Location = new Point(339, 332);
			btnCancel.Margin = new Padding(3, 4, 3, 4);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(86, 31);
			btnCancel.TabIndex = 8;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// FrmCnlImport
			// 
			AcceptButton = btnCreate;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(439, 379);
			Controls.Add(btnCancel);
			Controls.Add(btnCreate);
			Controls.Add(btnNext);
			Controls.Add(btnBack);
			Controls.Add(chkPreview);
			Controls.Add(CtrlCnlImport3);
			Controls.Add(CtrlCnlImport2);
			Controls.Add(CtrlCnlImport1);
			Controls.Add(lblStep);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Margin = new Padding(3, 4, 3, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FrmCnlImport";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Import Channels";
			Load += FrmCnlCreate_Load;
			ResumeLayout(false);
			PerformLayout();
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

