
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miProjectTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloneChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.miChannelMapByDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miChannelMapByObject = new System.Windows.Forms.ToolStripMenuItem();
            this.miCheckIntegrity = new System.Windows.Forms.ToolStripMenuItem();
            this.miSep = new System.Windows.Forms.ToolStripSeparator();
            this.miImportTable = new System.Windows.Forms.ToolStripMenuItem();
            this.miExportTable = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeviceMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.miCloneChannels,
            this.miChannelMapByDevice,
            this.miChannelMapByObject,
            this.miDeviceMap,
            this.miCheckIntegrity,
            this.miSep,
            this.miImportTable,
            this.miExportTable});
            this.miProjectTools.Name = "miProjectTools";
            this.miProjectTools.Size = new System.Drawing.Size(180, 22);
            this.miProjectTools.Text = "Project Tools";
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
            // miSep
            // 
            this.miSep.Name = "miSep";
            this.miSep.Size = new System.Drawing.Size(196, 6);
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
            // miDeviceMap
            // 
            this.miDeviceMap.Name = "miDeviceMap";
            this.miDeviceMap.Size = new System.Drawing.Size(199, 22);
            this.miDeviceMap.Text = "Device Map";
            this.miDeviceMap.Click += new System.EventHandler(this.miDeviceMap_Click);
            // 
            // CtrlMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip);
            this.Name = "CtrlMainMenu";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miProjectTools;
        private System.Windows.Forms.ToolStripMenuItem miCloneChannels;
        private System.Windows.Forms.ToolStripMenuItem miChannelMapByDevice;
        private System.Windows.Forms.ToolStripMenuItem miCheckIntegrity;
        private System.Windows.Forms.ToolStripSeparator miSep;
        private System.Windows.Forms.ToolStripMenuItem miImportTable;
        private System.Windows.Forms.ToolStripMenuItem miExportTable;
        private System.Windows.Forms.ToolStripMenuItem miChannelMapByObject;
        private System.Windows.Forms.ToolStripMenuItem miDeviceMap;
    }
}
