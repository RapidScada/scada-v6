
using System;


namespace Scada.Admin.Extensions.ExtImport.Controls
{
	partial class CtrlCnlImport3
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

		#region Component Designer generated code

		private void InitializeComponent()
		{
			btnImport = new Button();
			pathFile = new Button();
			label1 = new Label();
			SuspendLayout();
			// 
			// btnImport
			// 
			btnImport.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
			btnImport.Location = new Point(323, 75);
			btnImport.Margin = new Padding(3, 4, 3, 4);
			btnImport.Name = "btnImport";
			btnImport.Size = new Size(65, 53);
			btnImport.TabIndex = 0;
			btnImport.Text = ". . .";
			btnImport.TextAlign = ContentAlignment.TopCenter;
			btnImport.UseVisualStyleBackColor = true;
			btnImport.Click += btnImport_Click;
			// 
			// pathFile
			// 
			pathFile.Cursor = Cursors.Hand;
			pathFile.Location = new Point(15, 75);
			pathFile.Name = "pathFile";
			pathFile.Size = new Size(289, 53);
			pathFile.TabIndex = 0;
			pathFile.Click += button1_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(31, 147);
			label1.Name = "label1";
			label1.Size = new Size(38, 15);
			label1.TabIndex = 1;
			label1.Text = "label1";
			label1.Click += label1_Click;
			// 
			// CtrlCnlImport3
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(label1);
			Controls.Add(pathFile);
			Controls.Add(btnImport);
			Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			Name = "CtrlCnlImport3";
			Size = new Size(411, 213);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnImport;
		private Button pathFile;
		private Label label1;
	}
}