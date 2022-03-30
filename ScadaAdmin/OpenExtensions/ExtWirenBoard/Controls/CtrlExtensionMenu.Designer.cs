namespace Scada.Admin.Extensions.ExtWirenBoard.Controls
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miWirenBoard = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnCreateConfig = new System.Windows.Forms.ToolStripButton();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTools});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(150, 24);
            this.menuStrip.TabIndex = 0;
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWirenBoard});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(46, 20);
            this.miTools.Text = "Tools";
            // 
            // miWirenBoard
            // 
            this.miWirenBoard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCreateConfig});
            this.miWirenBoard.Name = "miWirenBoard";
            this.miWirenBoard.Size = new System.Drawing.Size(180, 22);
            this.miWirenBoard.Text = "Wiren Board";
            // 
            // miCreateConfig
            // 
            this.miCreateConfig.Image = global::Scada.Admin.Extensions.ExtWirenBoard.Properties.Resources.wb;
            this.miCreateConfig.Name = "miCreateConfig";
            this.miCreateConfig.Size = new System.Drawing.Size(194, 22);
            this.miCreateConfig.Text = "Create Configuration...";
            this.miCreateConfig.Click += new System.EventHandler(this.miCreateConfig_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreateConfig});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(150, 25);
            this.toolStrip.TabIndex = 1;
            // 
            // btnCreateConfig
            // 
            this.btnCreateConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateConfig.Image = global::Scada.Admin.Extensions.ExtWirenBoard.Properties.Resources.wb;
            this.btnCreateConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateConfig.Name = "btnCreateConfig";
            this.btnCreateConfig.Size = new System.Drawing.Size(23, 22);
            this.btnCreateConfig.ToolTipText = "Create project configuration for Wiren Board";
            this.btnCreateConfig.Click += new System.EventHandler(this.miCreateConfig_Click);
            // 
            // CtrlExtensionMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Name = "CtrlExtensionMenu";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem miTools;
        private ToolStrip toolStrip;
        private ToolStripMenuItem miWirenBoard;
        private ToolStripMenuItem miCreateConfig;
        private ToolStripButton btnCreateConfig;
    }
}
