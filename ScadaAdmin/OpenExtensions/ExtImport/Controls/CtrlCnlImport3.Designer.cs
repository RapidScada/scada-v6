
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
			SuspendLayout();
			// 
			// btnImport
			// 
			btnImport.Location = new Point(137, 80);
			btnImport.Margin = new Padding(3, 4, 3, 4);
			btnImport.Name = "btnImport";
			btnImport.Size = new Size(137, 53);
			btnImport.TabIndex = 0;
			btnImport.Text = "Import";
			btnImport.UseVisualStyleBackColor = true;
			btnImport.Click += btnImport_Click;
			// 
			// CtrlCnlImport3
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(btnImport);
			Margin = new Padding(3, 4, 3, 4);
			Name = "CtrlCnlImport3";
			Size = new Size(411, 213);
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button btnImport;

	}
}