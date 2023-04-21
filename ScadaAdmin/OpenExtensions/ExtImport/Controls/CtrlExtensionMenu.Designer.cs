namespace Scada.Admin.Extensions.ExtImport.Controls
{
	partial class CtrlExtensionMenu
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
			toolStrip = new ToolStrip();
			btnImport = new ToolStripButton();
			menuStrip = new MenuStrip();
			miTools = new ToolStripMenuItem();
			miImport = new ToolStripMenuItem();
			miImportChannel = new ToolStripMenuItem();
			toolStrip.SuspendLayout();
			menuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// toolStrip
			// 
			toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnImport });
			toolStrip.Location = new System.Drawing.Point(0, 28);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new System.Drawing.Size(184, 27);
			toolStrip.TabIndex = 0;
			toolStrip.Text = "toolStrip1";
			// 
			// btnImport
			// 
			btnImport.DisplayStyle = ToolStripItemDisplayStyle.Image;
			btnImport.Enabled = false;
			btnImport.Image = Properties.Resources.import;
			btnImport.ImageTransparentColor = Color.Magenta;
			btnImport.Name = "btnImport";
			btnImport.Size = new System.Drawing.Size(29, 24);
			btnImport.Text = "toolStripButton1";
			btnImport.ToolTipText = "Import Channel";
			btnImport.Click += miImportLine_Click;
			// 
			// menuStrip
			// 
			menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { miTools });
			menuStrip.Location = new System.Drawing.Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Size = new System.Drawing.Size(184, 28);
			menuStrip.TabIndex = 1;
			menuStrip.Text = "menuStrip1";
			// 
			// miTools
			// 
			miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miImport });
			miTools.Name = "miTools";
			miTools.Size = new Size(58, 24);
			miTools.Text = "Tools";
			// 
			// miImport
			// 
			miImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { miImportChannel });
			miImport.Enabled = false;
			miImport.Name = "miImport";
			miImport.Size = new System.Drawing.Size(224, 26);
			miImport.Text = "Import";

			// 
			// miImportChannel
			// 
			miImportChannel.Image = Properties.Resources.import;
			miImportChannel.Name = "miImportChannel";
			miImportChannel.Size = new System.Drawing.Size(224, 26);
			miImportChannel.Text = "Import Channel";
			miImportChannel.Click += miImportLine_Click;
			// 
			// CtrlExtensionMenu
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(toolStrip);
			Controls.Add(menuStrip);
			Name = "CtrlExtensionMenu";
			Size = new System.Drawing.Size(184, 150);
			toolStrip.ResumeLayout(false);
			toolStrip.PerformLayout();
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ToolStrip toolStrip;
		private MenuStrip menuStrip;
		private ToolStripMenuItem miTools;
		private ToolStripMenuItem miImport;
		private ToolStripButton btnImport;
		private ToolStripMenuItem miImportChannel;
	}
}
