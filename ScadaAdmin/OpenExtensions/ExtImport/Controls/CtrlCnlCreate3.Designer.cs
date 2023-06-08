namespace Scada.Admin.Extensions.ExtImport.Controls
{
	partial class CtrlCnlCreate3
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
			txtDevice = new TextBox();
			lblDevice = new Label();
			gbCnlNums = new GroupBox();
			btnReset = new Button();
			btnMap = new Button();
			numEndCnlNum = new NumericUpDown();
			lblEndCnlNum = new Label();
			numStartCnlNum = new NumericUpDown();
			lblStartCnlNum = new Label();
			radioButton1 = new RadioButton();
			txtPathFile = new TextBox();
			btnSelectFile = new Button();
			radioButton2 = new RadioButton();
			groupBox1 = new GroupBox();
			gbCnlNums.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numEndCnlNum).BeginInit();
			((System.ComponentModel.ISupportInitialize)numStartCnlNum).BeginInit();
			groupBox1.SuspendLayout();
			SuspendLayout();
			// 
			// txtDevice
			// 
			txtDevice.Location = new Point(18, 26);
			txtDevice.Margin = new Padding(3, 4, 3, 4);
			txtDevice.Name = "txtDevice";
			txtDevice.ReadOnly = true;
			txtDevice.Size = new Size(417, 27);
			txtDevice.TabIndex = 3;
			// 
			// lblDevice
			// 
			lblDevice.AutoSize = true;
			lblDevice.Location = new Point(18, 0);
			lblDevice.Name = "lblDevice";
			lblDevice.Size = new Size(54, 20);
			lblDevice.TabIndex = 2;
			lblDevice.Text = "Device";
			// 
			// gbCnlNums
			// 
			gbCnlNums.Controls.Add(btnReset);
			gbCnlNums.Controls.Add(btnMap);
			gbCnlNums.Controls.Add(numEndCnlNum);
			gbCnlNums.Controls.Add(lblEndCnlNum);
			gbCnlNums.Controls.Add(numStartCnlNum);
			gbCnlNums.Controls.Add(lblStartCnlNum);
			gbCnlNums.Location = new Point(14, 183);
			gbCnlNums.Margin = new Padding(1);
			gbCnlNums.Name = "gbCnlNums";
			gbCnlNums.Size = new Size(426, 130);
			gbCnlNums.TabIndex = 2;
			gbCnlNums.TabStop = false;
			gbCnlNums.Text = "Channel Numbers";
			// 
			// btnReset
			// 
			btnReset.Location = new Point(311, 88);
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
			btnMap.Location = new Point(311, 49);
			btnMap.Margin = new Padding(3, 4, 3, 4);
			btnMap.Name = "btnMap";
			btnMap.Size = new Size(86, 31);
			btnMap.TabIndex = 4;
			btnMap.Text = "Map";
			btnMap.UseVisualStyleBackColor = true;
			btnMap.Click += btnMap_Click;
			// 
			// numEndCnlNum
			// 
			numEndCnlNum.Increment = new decimal(new int[] { 0, 0, 0, 0 });
			numEndCnlNum.Location = new Point(162, 49);
			numEndCnlNum.Margin = new Padding(3, 4, 3, 4);
			numEndCnlNum.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
			numEndCnlNum.Name = "numEndCnlNum";
			numEndCnlNum.ReadOnly = true;
			numEndCnlNum.Size = new Size(142, 27);
			numEndCnlNum.TabIndex = 3;
			// 
			// lblEndCnlNum
			// 
			lblEndCnlNum.AutoSize = true;
			lblEndCnlNum.Location = new Point(151, 24);
			lblEndCnlNum.Name = "lblEndCnlNum";
			lblEndCnlNum.Size = new Size(34, 20);
			lblEndCnlNum.TabIndex = 2;
			lblEndCnlNum.Text = "End";
			// 
			// numStartCnlNum
			// 
			numStartCnlNum.Location = new Point(15, 49);
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
			lblStartCnlNum.Location = new Point(3, 24);
			lblStartCnlNum.Name = "lblStartCnlNum";
			lblStartCnlNum.Size = new Size(40, 20);
			lblStartCnlNum.TabIndex = 0;
			lblStartCnlNum.Text = "Start";
			// 
			// radioButton1
			// 
			radioButton1.AutoSize = true;
			radioButton1.Location = new Point(6, 26);
			radioButton1.Name = "radioButton1";
			radioButton1.Size = new Size(205, 24);
			radioButton1.TabIndex = 9;
			radioButton1.TabStop = true;
			radioButton1.Text = "Importer depuis un fichier ";
			radioButton1.UseVisualStyleBackColor = true;
			radioButton1.CheckedChanged += radioButton1_CheckedChanged;
			radioButton1.MouseClick += rdoEnableImport_MouseClick;
			// 
			// txtPathFile
			// 
			txtPathFile.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
			txtPathFile.Location = new Point(8, 53);
			txtPathFile.Margin = new Padding(5, 5, 3, 3);
			txtPathFile.Name = "txtPathFile";
			txtPathFile.ReadOnly = true;
			txtPathFile.Size = new Size(348, 27);
			txtPathFile.TabIndex = 8;
			txtPathFile.TabStop = false;
			txtPathFile.Visible = false;
			// 
			// btnSelectFile
			// 
			btnSelectFile.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
			btnSelectFile.ForeColor = SystemColors.ActiveCaptionText;
			btnSelectFile.ImageAlign = ContentAlignment.TopCenter;
			btnSelectFile.Location = new Point(362, 50);
			btnSelectFile.Margin = new Padding(3, 4, 3, 4);
			btnSelectFile.Name = "btnSelectFile";
			btnSelectFile.Size = new Size(41, 31);
			btnSelectFile.TabIndex = 7;
			btnSelectFile.Text = "...";
			btnSelectFile.UseVisualStyleBackColor = true;
			btnSelectFile.Visible = false;
			btnSelectFile.Click += btnSelectFile_Click;
			// 
			// radioButton2
			// 
			radioButton2.AutoSize = true;
			radioButton2.Location = new Point(6, 86);
			radioButton2.Name = "radioButton2";
			radioButton2.Size = new Size(227, 24);
			radioButton2.TabIndex = 10;
			radioButton2.TabStop = true;
			radioButton2.Text = "Importer depuis l'équipement";
			radioButton2.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(radioButton1);
			groupBox1.Controls.Add(radioButton2);
			groupBox1.Controls.Add(txtPathFile);
			groupBox1.Controls.Add(btnSelectFile);
			groupBox1.Location = new Point(14, 60);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(426, 119);
			groupBox1.TabIndex = 11;
			groupBox1.TabStop = false;
			// 
			// CtrlCnlCreate3
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(groupBox1);
			Controls.Add(gbCnlNums);
			Controls.Add(txtDevice);
			Controls.Add(lblDevice);
			Margin = new Padding(1);
			Name = "CtrlCnlCreate3";
			Size = new Size(466, 348);
			gbCnlNums.ResumeLayout(false);
			gbCnlNums.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numEndCnlNum).EndInit();
			((System.ComponentModel.ISupportInitialize)numStartCnlNum).EndInit();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox txtDevice;
		private System.Windows.Forms.Label lblDevice;
		private System.Windows.Forms.GroupBox gbCnlNums;
		private System.Windows.Forms.NumericUpDown numEndCnlNum;
		private System.Windows.Forms.Label lblEndCnlNum;
		private System.Windows.Forms.NumericUpDown numStartCnlNum;
		private System.Windows.Forms.Label lblStartCnlNum;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnMap;
		private RadioButton radioButton1;
		private TextBox txtPathFile;
		private Button btnSelectFile;
		private RadioButton radioButton2;
		private GroupBox groupBox1;
	}
}
