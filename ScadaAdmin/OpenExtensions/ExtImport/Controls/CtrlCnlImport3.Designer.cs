
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
			lblImport = new Label();
			txtPathFile = new TextBox();
			SuspendLayout();
			// 
			// btnImport
			// 
			btnImport.Anchor = AnchorStyles.None;
			btnImport.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
			btnImport.ForeColor = SystemColors.ActiveCaptionText;
			btnImport.ImageAlign = ContentAlignment.TopCenter;
			btnImport.Location = new Point(384, 115);
			btnImport.Margin = new Padding(3, 4, 3, 4);
			btnImport.Name = "btnImport";
			btnImport.Size = new Size(40, 27);
			btnImport.TabIndex = 0;
			btnImport.Text = ". . .";
			btnImport.UseVisualStyleBackColor = true;
			btnImport.Click += btnImport_Click;
			// 
			// lblImport
			// 
			lblImport.Anchor = AnchorStyles.None;
			lblImport.AutoSize = true;
			lblImport.Location = new Point(42, 80);
			lblImport.Name = "lblImport";
			lblImport.Size = new Size(152, 20);
			lblImport.TabIndex = 1;
			lblImport.Text = "Selectionner le fichier";
			lblImport.Click += label1_Click;
			// 
			// txtPathFile
			// 
			txtPathFile.Anchor = AnchorStyles.None;
			txtPathFile.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			txtPathFile.Location = new Point(42, 116);
			txtPathFile.Margin = new Padding(5, 5, 3, 3);
			txtPathFile.Name = "txtPathFile";
			txtPathFile.ReadOnly = true;
			txtPathFile.Size = new Size(336, 27);
			txtPathFile.TabIndex = 2;
			txtPathFile.TabStop = false;
			txtPathFile.TextAlign = HorizontalAlignment.Center;
			// 
			// CtrlCnlImport3
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(txtPathFile);
			Controls.Add(lblImport);
			Controls.Add(btnImport);
			Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			Name = "CtrlCnlImport3";
			Size = new Size(475, 214);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnImport;
		private Label lblImport;
		private TextBox txtPathFile;
	}
}