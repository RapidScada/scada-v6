
namespace Scada.Admin.Extensions.ExtProjectTools.Controls
{
    partial class CtrlMainMenu
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAddLine = new System.Windows.Forms.ToolStripButton();
            this.btnAddDevice = new System.Windows.Forms.ToolStripButton();
            this.btnCreateChannels = new System.Windows.Forms.ToolStripButton();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miProjectTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miCloneChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.miChannelMapByDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miChannelMapByObject = new System.Windows.Forms.ToolStripMenuItem();
            this.miCheckIntegrity = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miImportTable = new System.Windows.Forms.ToolStripMenuItem();
            this.miExportTable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddLine,
            this.btnAddDevice,
            this.btnCreateChannels});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(150, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnAddLine
            // 
            this.btnAddLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddLine.Image = global::Scada.Admin.Extensions.ExtProjectTools.Properties.Resources.add_line;
            this.btnAddLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(23, 22);
            this.btnAddLine.ToolTipText = "Add Communication Line";
            this.btnAddLine.Click += new System.EventHandler(this.miAddLine_Click);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddDevice.Image = global::Scada.Admin.Extensions.ExtProjectTools.Properties.Resources.add_device;
            this.btnAddDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(23, 22);
            this.btnAddDevice.ToolTipText = "Add Device";
            this.btnAddDevice.Click += new System.EventHandler(this.miAddDevice_Click);
            // 
            // btnCreateChannels
            // 
            this.btnCreateChannels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateChannels.Image = global::Scada.Admin.Extensions.ExtProjectTools.Properties.Resources.create_cnls;
            this.btnCreateChannels.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateChannels.Name = "btnCreateChannels";
            this.btnCreateChannels.Size = new System.Drawing.Size(23, 22);
            this.btnCreateChannels.ToolTipText = "Create Channels";
            this.btnCreateChannels.Click += new System.EventHandler(this.miCreateChannels_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(150, 24);
            this.menuStrip.TabIndex = 1;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miProjectTools});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // miProjectTools
            // 
            this.miProjectTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddLine,
            this.miAddDevice,
            this.miCreateChannels,
            this.miSep1,
            this.miCloneChannels,
            this.miChannelMapByDevice,
            this.miChannelMapByObject,
            this.miCheckIntegrity,
            this.miSep2,
            this.miImportTable,
            this.miExportTable});
            this.miProjectTools.Name = "miProjectTools";
            this.miProjectTools.Size = new System.Drawing.Size(141, 22);
            this.miProjectTools.Text = "Project Tools";
            // 
            // miAddLine
            // 
            this.miAddLine.Image = global::Scada.Admin.Extensions.ExtProjectTools.Properties.Resources.add_line;
            this.miAddLine.Name = "miAddLine";
            this.miAddLine.Size = new System.Drawing.Size(199, 22);
            this.miAddLine.Text = "Add Line...";
            this.miAddLine.Click += new System.EventHandler(this.miAddLine_Click);
            // 
            // miAddDevice
            // 
            this.miAddDevice.Image = global::Scada.Admin.Extensions.ExtProjectTools.Properties.Resources.add_device;
            this.miAddDevice.Name = "miAddDevice";
            this.miAddDevice.Size = new System.Drawing.Size(199, 22);
            this.miAddDevice.Text = "Add Device...";
            this.miAddDevice.Click += new System.EventHandler(this.miAddDevice_Click);
            // 
            // miCreateChannels
            // 
            this.miCreateChannels.Image = global::Scada.Admin.Extensions.ExtProjectTools.Properties.Resources.create_cnls;
            this.miCreateChannels.Name = "miCreateChannels";
            this.miCreateChannels.Size = new System.Drawing.Size(199, 22);
            this.miCreateChannels.Text = "Create Channels...";
            this.miCreateChannels.Click += new System.EventHandler(this.miCreateChannels_Click);
            // 
            // miSep1
            // 
            this.miSep1.Name = "miSep1";
            this.miSep1.Size = new System.Drawing.Size(196, 6);
            // 
            // miCloneChannels
            // 
            this.miCloneChannels.Name = "miCloneChannels";
            this.miCloneChannels.Size = new System.Drawing.Size(199, 22);
            this.miCloneChannels.Text = "Clone Channels...";
            this.miCloneChannels.Click += new System.EventHandler(this.miCloneChannels_Click);
            // 
            // miChannelMapByDevice
            // 
            this.miChannelMapByDevice.Name = "miChannelMapByDevice";
            this.miChannelMapByDevice.Size = new System.Drawing.Size(199, 22);
            this.miChannelMapByDevice.Text = "Channel Map by Device";
            this.miChannelMapByDevice.Click += new System.EventHandler(this.miChannelMap_Click);
            // 
            // miChannelMapByObject
            // 
            this.miChannelMapByObject.Name = "miChannelMapByObject";
            this.miChannelMapByObject.Size = new System.Drawing.Size(199, 22);
            this.miChannelMapByObject.Text = "Channel Map by Object";
            this.miChannelMapByObject.Click += new System.EventHandler(this.miChannelMap_Click);
            // 
            // miCheckIntegrity
            // 
            this.miCheckIntegrity.Name = "miCheckIntegrity";
            this.miCheckIntegrity.Size = new System.Drawing.Size(199, 22);
            this.miCheckIntegrity.Text = "Check Integrity";
            this.miCheckIntegrity.Click += new System.EventHandler(this.miCheckIntegrity_Click);
            // 
            // miSep2
            // 
            this.miSep2.Name = "miSep2";
            this.miSep2.Size = new System.Drawing.Size(196, 6);
            // 
            // miImportTable
            // 
            this.miImportTable.Name = "miImportTable";
            this.miImportTable.Size = new System.Drawing.Size(199, 22);
            this.miImportTable.Text = "Import Table...";
            this.miImportTable.Click += new System.EventHandler(this.miImportTable_Click);
            // 
            // miExportTable
            // 
            this.miExportTable.Name = "miExportTable";
            this.miExportTable.Size = new System.Drawing.Size(199, 22);
            this.miExportTable.Text = "Export Table...";
            this.miExportTable.Click += new System.EventHandler(this.miExportTable_Click);
            // 
            // CtrlMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Name = "CtrlMainMenu";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miProjectTools;
        private System.Windows.Forms.ToolStripMenuItem miAddLine;
        private System.Windows.Forms.ToolStripButton btnAddLine;
        private System.Windows.Forms.ToolStripMenuItem miAddDevice;
        private System.Windows.Forms.ToolStripMenuItem miCreateChannels;
        private System.Windows.Forms.ToolStripSeparator miSep1;
        private System.Windows.Forms.ToolStripButton btnAddDevice;
        private System.Windows.Forms.ToolStripButton btnCreateChannels;
        private System.Windows.Forms.ToolStripMenuItem miCloneChannels;
        private System.Windows.Forms.ToolStripMenuItem miChannelMapByDevice;
        private System.Windows.Forms.ToolStripMenuItem miCheckIntegrity;
        private System.Windows.Forms.ToolStripSeparator miSep2;
        private System.Windows.Forms.ToolStripMenuItem miImportTable;
        private System.Windows.Forms.ToolStripMenuItem miExportTable;
        private System.Windows.Forms.ToolStripMenuItem miChannelMapByObject;
    }
}
