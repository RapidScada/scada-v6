
using System;
using System.Drawing;


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
			gbCnlNums = new GroupBox();
			btnReset = new Button();
			btnMap = new Button();
			numStartCnlNum = new NumericUpDown();
			lblStartCnlNum = new Label();
			gbCnlNums.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numStartCnlNum).BeginInit();
			SuspendLayout();
			// 
			// btnImport
			// 
			btnImport.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
			btnImport.ForeColor = SystemColors.ActiveCaptionText;
			btnImport.ImageAlign = ContentAlignment.TopCenter;
			btnImport.Location = new Point(377, 30);
			btnImport.Margin = new Padding(3, 4, 3, 4);
			btnImport.Name = "btnImport";
			btnImport.Size = new Size(43, 30);
			btnImport.TabIndex = 0;
			btnImport.Text = "...";
			btnImport.UseVisualStyleBackColor = true;
			btnImport.Click += btnImport_Click;
			// 
			// lblImport
			// 
			lblImport.AutoSize = true;
			lblImport.Location = new Point(13, 5);
			lblImport.Name = "lblImport";
			lblImport.Size = new Size(126, 20);
			lblImport.TabIndex = 1;
			lblImport.Text = "Fichier à importer";
			// 
			// txtPathFile
			// 
			txtPathFile.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			txtPathFile.Location = new Point(13, 30);
			txtPathFile.Margin = new Padding(5, 5, 3, 3);
			txtPathFile.Name = "txtPathFile";
			txtPathFile.ReadOnly = true;
			txtPathFile.Size = new Size(358, 27);
			txtPathFile.TabIndex = 2;
			txtPathFile.TabStop = false;
			// 
			// gbCnlNums
			// 
			gbCnlNums.Controls.Add(btnReset);
			gbCnlNums.Controls.Add(btnMap);
			gbCnlNums.Controls.Add(numStartCnlNum);
			gbCnlNums.Controls.Add(lblStartCnlNum);
			gbCnlNums.Location = new Point(13, 72);
			gbCnlNums.Margin = new Padding(3, 4, 3, 4);
			gbCnlNums.Name = "gbCnlNums";
			gbCnlNums.Padding = new Padding(11, 4, 11, 13);
			gbCnlNums.Size = new Size(407, 120);
			gbCnlNums.TabIndex = 3;
			gbCnlNums.TabStop = false;
			gbCnlNums.Text = "Channel Numbers";
			// 
			// btnReset
			// 
			btnReset.Location = new Point(255, 88);
			btnReset.Margin = new Padding(3, 4, 3, 4);
			btnReset.Name = "btnReset";
			btnReset.Size = new Size(86, 31);
			btnReset.TabIndex = 5;
			btnReset.Text = "Reset";
			btnReset.UseVisualStyleBackColor = true;
			btnReset.Click += btnReset_Click;
			// 
			// btnMap
			// 
			btnMap.Location = new Point(255, 49);
			btnMap.Margin = new Padding(3, 4, 3, 4);
			btnMap.Name = "btnMap";
			btnMap.Size = new Size(86, 31);
			btnMap.TabIndex = 4;
			btnMap.Text = "Map";
			btnMap.UseVisualStyleBackColor = true;
			btnMap.Click += btnMap_Click;
			// 
			// numStartCnlNum
			// 
			numStartCnlNum.Location = new Point(76, 49);
			numStartCnlNum.Margin = new Padding(3, 4, 3, 4);
			numStartCnlNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
			numStartCnlNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numStartCnlNum.Name = "numStartCnlNum";
			numStartCnlNum.Size = new Size(141, 27);
			numStartCnlNum.TabIndex = 1;
			numStartCnlNum.Value = new decimal(new int[] { 1, 0, 0, 0 });
			numStartCnlNum.ValueChanged += numStartCnlNum_ValueChanged;
			// 
			// lblStartCnlNum
			// 
			lblStartCnlNum.AutoSize = true;
			lblStartCnlNum.Location = new Point(76, 24);
			lblStartCnlNum.Name = "lblStartCnlNum";
			lblStartCnlNum.Size = new Size(40, 20);
			lblStartCnlNum.TabIndex = 0;
			lblStartCnlNum.Text = "Start";
			// 
			// CtrlCnlImport3
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(gbCnlNums);
			Controls.Add(txtPathFile);
			Controls.Add(lblImport);
			Controls.Add(btnImport);
			Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			Name = "CtrlCnlImport3";
			Size = new Size(436, 204);
			gbCnlNums.ResumeLayout(false);
			gbCnlNums.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numStartCnlNum).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Button btnImport;
		private Label lblImport;
		private TextBox txtPathFile;
		private GroupBox gbCnlNums;
		private Button btnReset;
		private Button btnMap;
		private NumericUpDown numStartCnlNum;
		private Label lblStartCnlNum;
	}
}