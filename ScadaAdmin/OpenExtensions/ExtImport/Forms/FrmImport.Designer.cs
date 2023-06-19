using Scada.Admin.Extensions.ExtImport.Controls;

namespace Scada.Admin.Extensions.ExtImport.Forms
{
	partial class FrmImport
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
			ctrlCnlImport1 = new CtrlImport1();
			ctrlCnlImport2 = new CtrlImport2();
			ctrlCnlmport3 = new CtrlImport3();
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
			lblStep.Size = new Size(541, 40);
			lblStep.TabIndex = 0;
			lblStep.Text = "Step 1 of 3: Step description";
			lblStep.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ctrlCnlImport1
			// 
			ctrlCnlImport1.Location = new Point(31, 56);
			ctrlCnlImport1.Margin = new Padding(3, 5, 3, 5);
			ctrlCnlImport1.Name = "ctrlCnlImport1";
			ctrlCnlImport1.Size = new Size(513, 366);
			ctrlCnlImport1.TabIndex = 1;
			ctrlCnlImport1.SelectedDeviceChanged += ctrlCnlCreate1_SelectedDeviceChanged;
			// 
			// ctrlCnlImport2
			// 
			ctrlCnlImport2.DeviceName = "";
			ctrlCnlImport2.Location = new Point(28, 61);
			ctrlCnlImport2.Margin = new Padding(3, 5, 3, 5);
			ctrlCnlImport2.Name = "ctrlCnlImport2";
			ctrlCnlImport2.Size = new Size(512, 179);
			ctrlCnlImport2.TabIndex = 2;
			// 
			// ctrlCnlmport3
			// 
			ctrlCnlmport3.DeviceName = "";
			ctrlCnlmport3.Location = new Point(10, 43);
			ctrlCnlmport3.Margin = new Padding(1);
			ctrlCnlmport3.Name = "ctrlCnlmport3";
			ctrlCnlmport3.Size = new Size(521, 438);
			ctrlCnlmport3.TabIndex = 3;
			ctrlCnlmport3.SelectedFileChanged += CtrlCnlImport3_SelectedDeviceChanged;
			ctrlCnlmport3.rdbCheckStateChanged += CtrlCnlCnl3_rdbCheckStateChanged;
			// 
			// btnBack
			// 
			btnBack.Location = new Point(230, 484);
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
			btnNext.Location = new Point(322, 486);
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
			btnCreate.Location = new Point(322, 485);
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
			btnCancel.Location = new Point(414, 484);
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
			ClientSize = new Size(541, 526);
			Controls.Add(btnCancel);
			Controls.Add(btnCreate);
			Controls.Add(btnNext);
			Controls.Add(btnBack);
			Controls.Add(ctrlCnlmport3);
			Controls.Add(ctrlCnlImport2);
			Controls.Add(ctrlCnlImport1);
			Controls.Add(lblStep);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Margin = new Padding(3, 4, 3, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FrmCnlImport";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "Create Channels";
			Load += FrmCnlCreate_Load;
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Label lblStep;
		private Controls.CtrlImport1 ctrlCnlImport1;
		private Controls.CtrlImport2 ctrlCnlImport2;
		private Controls.CtrlImport3 ctrlCnlmport3;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnCancel;
	}
}