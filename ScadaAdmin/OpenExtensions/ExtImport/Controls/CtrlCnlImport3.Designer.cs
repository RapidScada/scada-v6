
using System;


namespace Scada.Admin.Extensions.ExtCommConfig.Controls
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

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			lblDevice = new Label();
			txtDevice = new TextBox();
			lblCnlCnt = new Label();
			numCnlCnt = new NumericUpDown();
			lblStartCnlNum = new Label();
			numStartCnlNum = new NumericUpDown();
			((System.ComponentModel.ISupportInitialize)numCnlCnt).BeginInit();
			((System.ComponentModel.ISupportInitialize)numStartCnlNum).BeginInit();
			SuspendLayout();
			// 
			// lblDevice
			// 
			lblDevice.AutoSize = true;
			lblDevice.Location = new Point(0, 0);
			lblDevice.Margin = new Padding(4, 0, 4, 0);
			lblDevice.Name = "lblDevice";
			lblDevice.Size = new Size(57, 20);
			lblDevice.TabIndex = 0;
			lblDevice.Text = "Device:";
			// 
			// txtDevice
			// 
			txtDevice.Location = new Point(0, 25);
			txtDevice.Margin = new Padding(4, 5, 4, 5);
			txtDevice.Name = "txtDevice";
			txtDevice.Size = new Size(319, 27);
			txtDevice.TabIndex = 1;
			// 
			// lblCnlCnt
			// 
			lblCnlCnt.AutoSize = true;
			lblCnlCnt.Location = new Point(0, 60);
			lblCnlCnt.Margin = new Padding(4, 0, 4, 0);
			lblCnlCnt.Name = "lblCnlCnt";
			lblCnlCnt.Size = new Size(108, 20);
			lblCnlCnt.TabIndex = 2;
			lblCnlCnt.Text = "Channel Count:";
			// 
			// numCnlCnt
			// 
			numCnlCnt.Location = new Point(0, 85);
			numCnlCnt.Margin = new Padding(4, 5, 4, 5);
			numCnlCnt.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			numCnlCnt.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numCnlCnt.Name = "numCnlCnt";
			numCnlCnt.Size = new Size(93, 27);
			numCnlCnt.TabIndex = 3;
			numCnlCnt.Value = new decimal(new int[] { 1, 0, 0, 0 });
			numCnlCnt.ValueChanged += numCnlCnt_ValueChanged;
			// 
			// lblStartCnlNum
			// 
			lblStartCnlNum.AutoSize = true;
			lblStartCnlNum.Location = new Point(0, 120);
			lblStartCnlNum.Margin = new Padding(4, 0, 4, 0);
			lblStartCnlNum.Name = "lblStartCnlNum";
			lblStartCnlNum.Size = new Size(158, 20);
			lblStartCnlNum.TabIndex = 4;
			lblStartCnlNum.Text = "Start Channel Number:";
			// 
			// numStartCnlNum
			// 
			numStartCnlNum.Location = new Point(0, 145);
			numStartCnlNum.Margin = new Padding(4, 5, 4, 5);
			numStartCnlNum.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			numStartCnlNum.Name = "numStartCnlNum";
			numStartCnlNum.Size = new Size(93, 27);
			numStartCnlNum.TabIndex = 5;
			// 
			// CtrlCnlImport3
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(numStartCnlNum);
			Controls.Add(lblStartCnlNum);
			Controls.Add(numCnlCnt);
			Controls.Add(lblCnlCnt);
			Controls.Add(txtDevice);
			Controls.Add(lblDevice);
			Margin = new Padding(4, 5, 4, 5);
			Name = "CtrlCnlImport3";
			Size = new Size(335, 199);
			((System.ComponentModel.ISupportInitialize)numCnlCnt).EndInit();
			((System.ComponentModel.ISupportInitialize)numStartCnlNum).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblDevice;
		private System.Windows.Forms.TextBox txtDevice;
		private System.Windows.Forms.Label lblCnlCnt;
		private System.Windows.Forms.NumericUpDown numCnlCnt;
		private System.Windows.Forms.Label lblStartCnlNum;
		private System.Windows.Forms.NumericUpDown numStartCnlNum;
	}
}