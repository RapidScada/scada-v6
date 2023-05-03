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
			toolStrip.ImageScalingSize = new Size(20, 20);
			toolStrip.Items.AddRange(new ToolStripItem[] { btnImport });
			toolStrip.Location = new Point(0, 24);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new Size(161, 27);
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
			btnImport.Size = new Size(24, 24);
			btnImport.Text = "toolStripButton1";
			btnImport.ToolTipText = "Import Channel";
			btnImport.Click += miImportCnl_Click;
			// 
			// menuStrip
			// 
			menuStrip.ImageScalingSize = new Size(20, 20);
			menuStrip.Items.AddRange(new ToolStripItem[] { miTools });
			menuStrip.Location = new Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Padding = new Padding(5, 2, 0, 2);
			menuStrip.Size = new Size(161, 24);
			menuStrip.TabIndex = 1;
			menuStrip.Text = "menuStrip1";
			// 
			// miTools
			// 
			miTools.DropDownItems.AddRange(new ToolStripItem[] { miImport });
			miTools.Name = "miTools";
			miTools.Size = new Size(46, 20);
			miTools.Text = "Tools";
			// 
			// miImport
			// 
			miImport.DropDownItems.AddRange(new ToolStripItem[] { miImportChannel });
			miImport.Enabled = false;
			miImport.Name = "miImport";
			miImport.Size = new Size(110, 22);
			miImport.Text = "Import";
			// 
			// miImportChannel
			// 
			miImportChannel.Image = Properties.Resources.import;
			miImportChannel.Name = "miImportChannel";
			miImportChannel.Size = new Size(160, 22);
			miImportChannel.Text = "Import Channel ";
			miImportChannel.Click += miImportCnl_Click;
			// 
			// CtrlExtensionMenu
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(toolStrip);
			Controls.Add(menuStrip);
			Margin = new Padding(3, 2, 3, 2);
			Name = "CtrlExtensionMenu";
			Size = new Size(161, 112);
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