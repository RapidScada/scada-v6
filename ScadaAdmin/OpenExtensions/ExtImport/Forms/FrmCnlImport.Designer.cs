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
			lblStep.Size = new Size(439, 40);
			lblStep.TabIndex = 0;
			lblStep.Text = "Step 1 of 3: Step description";
			lblStep.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// ctrlCnlImport1
			// 
			ctrlCnlImport1.Location = new Point(14, 44);
			ctrlCnlImport1.Margin = new Padding(3, 5, 3, 5);
			ctrlCnlImport1.Name = "ctrlCnlImport1";
			ctrlCnlImport1.Size = new Size(411, 267);
			ctrlCnlImport1.TabIndex = 1;
			ctrlCnlImport1.Load += ctrlCnlImport1_Load;
			// 
			// ctrlCnlImport2
			// 
			ctrlCnlImport2.DeviceName = "";
			ctrlCnlImport2.Location = new Point(14, 44);
			ctrlCnlImport2.Margin = new Padding(3, 5, 3, 5);
			ctrlCnlImport2.Name = "ctrlCnlImport2";
			ctrlCnlImport2.Size = new Size(411, 133);
			ctrlCnlImport2.TabIndex = 2;
			// 
			// chkPreview
			// 
			chkPreview.AutoSize = true;
			chkPreview.Checked = true;
			chkPreview.CheckState = CheckState.Checked;
			chkPreview.Location = new Point(14, 335);
			chkPreview.Margin = new Padding(3, 4, 3, 4);
			chkPreview.Name = "chkPreview";
			chkPreview.Size = new Size(82, 24);
			chkPreview.TabIndex = 4;
			chkPreview.Text = "Preview";
			chkPreview.UseVisualStyleBackColor = true;
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
			// 
			// btnCreate
			// 
			btnCreate.Location = new Point(247, 332);
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
			ClientSize = new Size(439, 365);
			Controls.Add(btnCancel);
			Controls.Add(btnCreate);
			Controls.Add(btnNext);
			Controls.Add(btnBack);
			Controls.Add(chkPreview);
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
			Text = "Import Channels";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblStep;
		private Controls.CtrlCnlImport1 ctrlCnlImport1;
		private Controls.CtrlCnlImport2 ctrlCnlImport2;
		//private Controls.CtrlCnlImport3 ctrlCnlImport3;
		private System.Windows.Forms.CheckBox chkPreview;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnCancel;
	}
}