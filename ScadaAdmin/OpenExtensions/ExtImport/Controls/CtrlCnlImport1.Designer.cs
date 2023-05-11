namespace Scada.Admin.Extensions.ExtImport.Controls
{
	partial class CtrlCnlImport1
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
			lblCommLine = new Label();
			cbCommLine = new ComboBox();
			lblDevice = new Label();
			cbDevice = new ComboBox();
			txtInfo = new TextBox();
			pbStatus = new PictureBox();
			((System.ComponentModel.ISupportInitialize)pbStatus).BeginInit();
			SuspendLayout();
			// 
			// lblCommLine
			// 
			lblCommLine.AutoSize = true;
			lblCommLine.Location = new Point(-3, 0);
			lblCommLine.Name = "lblCommLine";
			lblCommLine.Size = new Size(116, 15);
			lblCommLine.TabIndex = 0;
			lblCommLine.Text = "Communication line";
			// 
			// cbCommLine
			// 
			cbCommLine.DisplayMember = "Name";
			cbCommLine.DropDownStyle = ComboBoxStyle.DropDownList;
			cbCommLine.FormattingEnabled = true;
			cbCommLine.Location = new Point(0, 18);
			cbCommLine.Name = "cbCommLine";
			cbCommLine.Size = new Size(360, 23);
			cbCommLine.TabIndex = 1;
			cbCommLine.ValueMember = "CommLineNum";
			cbCommLine.SelectedIndexChanged += cbCommLine_SelectedIndexChanged;
			// 
			// lblDevice
			// 
			lblDevice.AutoSize = true;
			lblDevice.Location = new Point(-3, 44);
			lblDevice.Name = "lblDevice";
			lblDevice.Size = new Size(42, 15);
			lblDevice.TabIndex = 2;
			lblDevice.Text = "Device";
			// 
			// cbDevice
			// 
			cbDevice.DisplayMember = "Name";
			cbDevice.DropDownStyle = ComboBoxStyle.DropDownList;
			cbDevice.FormattingEnabled = true;
			cbDevice.Location = new Point(0, 62);
			cbDevice.Name = "cbDevice";
			cbDevice.Size = new Size(360, 23);
			cbDevice.TabIndex = 3;
			cbDevice.ValueMember = "DeviceNum";
			cbDevice.SelectedIndexChanged += cbDevice_SelectedIndexChanged;
			// 
			// txtInfo
			// 
			txtInfo.Location = new Point(0, 91);
			txtInfo.Multiline = true;
			txtInfo.Name = "txtInfo";
			txtInfo.ReadOnly = true;
			txtInfo.Size = new Size(338, 109);
			txtInfo.TabIndex = 4;
			// 
			// pbStatus
			// 
			pbStatus.Image = Properties.Resources.success;
			pbStatus.Location = new Point(344, 91);
			pbStatus.Name = "pbStatus";
			pbStatus.Size = new Size(16, 16);
			pbStatus.TabIndex = 5;
			pbStatus.TabStop = false;
			pbStatus.Click += pbStatus_Click;
			// 
			// CtrlCnlImport1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(pbStatus);
			Controls.Add(txtInfo);
			Controls.Add(cbDevice);
			Controls.Add(lblDevice);
			Controls.Add(cbCommLine);
			Controls.Add(lblCommLine);
			Name = "CtrlCnlImport1";
			Size = new Size(369, 218);
			((System.ComponentModel.ISupportInitialize)pbStatus).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblCommLine;
		private System.Windows.Forms.ComboBox cbCommLine;
		private System.Windows.Forms.Label lblDevice;
		private System.Windows.Forms.ComboBox cbDevice;
		private System.Windows.Forms.TextBox txtInfo;
		private System.Windows.Forms.PictureBox pbStatus;
	}
}