using Scada.Admin.Extensions.ExtImport.Controls;

namespace Scada.Admin.Extensions.ExtImport.Forms
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
			lblStep = new Label();
			ctrlCnlCreate1 = new Controls.CtrlCnlCreate1();
			ctrlCnlCreate2 = new Controls.CtrlCnlCreate2();
			ctrlCnlCreate3 = new Controls.CtrlCnlCreate3();
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
			lblStep.Size = new Size(482, 40);
			lblStep.TabIndex = 0;
			lblStep.Text = "Step 1 of 3: Step description";
			lblStep.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ctrlCnlCreate1
			// 
			ctrlCnlCreate1.Location = new Point(16, 44);
			ctrlCnlCreate1.Margin = new Padding(3, 5, 3, 5);
			ctrlCnlCreate1.Name = "ctrlCnlCreate1";
			ctrlCnlCreate1.Size = new Size(447, 315);
			ctrlCnlCreate1.TabIndex = 1;
			ctrlCnlCreate1.SelectedDeviceChanged += ctrlCnlCreate1_SelectedDeviceChanged;
			// 
			// ctrlCnlCreate2
			// 
			ctrlCnlCreate2.DeviceName = "";
			ctrlCnlCreate2.Location = new Point(17, 44);
			ctrlCnlCreate2.Margin = new Padding(3, 5, 3, 5);
			ctrlCnlCreate2.Name = "ctrlCnlCreate2";
			ctrlCnlCreate2.Size = new Size(411, 133);
			ctrlCnlCreate2.TabIndex = 2;
			// 
			// ctrlCnlCreate3
			// 
			ctrlCnlCreate3.DeviceName = "";
			ctrlCnlCreate3.Location = new Point(2, 44);
			ctrlCnlCreate3.Margin = new Padding(1);
			ctrlCnlCreate3.Name = "ctrlCnlCreate3";
			ctrlCnlCreate3.Size = new Size(451, 331);
			ctrlCnlCreate3.TabIndex = 3;
			ctrlCnlCreate3.SelectedFileChanged += CtrlCnlImport3_SelectedDeviceChanged;
			ctrlCnlCreate3.rdbCheckStateChanged += CtrlCnlCnl3_rdbCheckStateChanged;
			// 
			// chkPreview
			// 
			chkPreview.AutoSize = true;
			chkPreview.Checked = true;
			chkPreview.CheckState = CheckState.Checked;
			chkPreview.Location = new Point(12, 384);
			chkPreview.Margin = new Padding(3, 4, 3, 4);
			chkPreview.Name = "chkPreview";
			chkPreview.Size = new Size(82, 24);
			chkPreview.TabIndex = 4;
			chkPreview.Text = "Preview";
			chkPreview.UseVisualStyleBackColor = true;
			// 
			// btnBack
			// 
			btnBack.Location = new Point(183, 380);
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
			btnNext.Location = new Point(275, 380);
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
			btnCreate.Location = new Point(275, 380);
			btnCreate.Margin = new Padding(3, 4, 3, 4);
			btnCreate.Name = "btnCreate";
			btnCreate.Size = new Size(86, 31);
			btnCreate.TabIndex = 7;
			btnCreate.Text = "Create";
			btnCreate.UseVisualStyleBackColor = true;
			btnCreate.Click += btnCreate_Click;
			// 
			// btnCancel
			// 
			btnCancel.Location = new Point(367, 380);
			btnCancel.Margin = new Padding(3, 4, 3, 4);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(86, 31);
			btnCancel.TabIndex = 8;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// FrmCnlCreate
			// 
			AcceptButton = btnCreate;
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new Size(482, 424);
			Controls.Add(btnCancel);
			Controls.Add(btnCreate);
			Controls.Add(btnNext);
			Controls.Add(btnBack);
			Controls.Add(chkPreview);
			Controls.Add(ctrlCnlCreate3);
			Controls.Add(ctrlCnlCreate2);
			Controls.Add(ctrlCnlCreate1);
			Controls.Add(lblStep);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Margin = new Padding(3, 4, 3, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FrmCnlCreate";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Channels";
			Load += FrmCnlCreate_Load;
			ResumeLayout(false);
			PerformLayout();
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