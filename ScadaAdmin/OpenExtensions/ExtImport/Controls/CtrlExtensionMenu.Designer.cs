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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlExtensionMenu));
            toolStrip = new ToolStrip();
            btnImport = new ToolStripButton();
            menuStrip = new MenuStrip();
            miTools = new ToolStripMenuItem();
            miImport = new ToolStripMenuItem();
            toolStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.ImageScalingSize = new Size(20, 20);
            toolStrip.Items.AddRange(new ToolStripItem[] { btnImport });
            toolStrip.Location = new Point(0, 28);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(150, 27);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // btnImport
            // 
            btnImport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnImport.Enabled = false;
            btnImport.Image = (Image)resources.GetObject("btnImport.Image");
            btnImport.ImageTransparentColor = Color.Magenta;
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(29, 24);
            btnImport.Text = "toolStripButton1";
            btnImport.Click += btnImport_Click;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { miTools });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(150, 28);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // miTools
            // 
            miTools.DropDownItems.AddRange(new ToolStripItem[] { miImport });
            miTools.Name = "miTools";
            miTools.Size = new Size(58, 24);
            miTools.Text = "Tools";
            // 
            // importToolStripMenuItem
            // 
            miImport.Enabled = false;
            miImport.Name = "importToolStripMenuItem";
            miImport.Size = new Size(224, 26);
            miImport.Text = "Import";
            miImport.Click += importToolStripMenuItem_Click;
            // 
            // CtrlExtensionMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(toolStrip);
            Controls.Add(menuStrip);
            Name = "CtrlExtensionMenu";
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
    }
}
