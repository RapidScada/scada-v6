
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
      this.btnImport = new System.Windows.Forms.Button();
      this.lblImport = new System.Windows.Forms.Label();
      this.txtPathFile = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // btnImport
      // 
      this.btnImport.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.btnImport.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
      this.btnImport.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.btnImport.Location = new System.Drawing.Point(332, 20);
      this.btnImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(28, 23);
      this.btnImport.TabIndex = 0;
      this.btnImport.Text = "...";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // lblImport
      // 
      this.lblImport.AutoSize = true;
      this.lblImport.Location = new System.Drawing.Point(0, 0);
      this.lblImport.Name = "lblImport";
      this.lblImport.Size = new System.Drawing.Size(100, 15);
      this.lblImport.TabIndex = 1;
      this.lblImport.Text = "Fichier à importer";
      // 
      // txtPathFile
      // 
      this.txtPathFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.txtPathFile.Location = new System.Drawing.Point(0, 20);
      this.txtPathFile.Margin = new System.Windows.Forms.Padding(5, 5, 3, 3);
      this.txtPathFile.Name = "txtPathFile";
      this.txtPathFile.ReadOnly = true;
      this.txtPathFile.Size = new System.Drawing.Size(326, 23);
      this.txtPathFile.TabIndex = 2;
      this.txtPathFile.TabStop = false;
      // 
      // CtrlCnlImport3
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.txtPathFile);
      this.Controls.Add(this.lblImport);
      this.Controls.Add(this.btnImport);
      this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.Name = "CtrlCnlImport3";
      this.Size = new System.Drawing.Size(360, 100);
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnImport;
		private Label lblImport;
		private TextBox txtPathFile;
	}
}