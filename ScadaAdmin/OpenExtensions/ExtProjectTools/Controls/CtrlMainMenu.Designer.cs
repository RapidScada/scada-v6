﻿
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
            menuStrip = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            miProjectTools = new ToolStripMenuItem();
            miCloneChannels = new ToolStripMenuItem();
            miChannelMapByDevice = new ToolStripMenuItem();
            miChannelMapByObject = new ToolStripMenuItem();
            miDeviceMap = new ToolStripMenuItem();
            miCheckIntegrity = new ToolStripMenuItem();
            miEncryptPassword = new ToolStripMenuItem();
            miSep = new ToolStripSeparator();
            miImportTable = new ToolStripMenuItem();
            miExportTable = new ToolStripMenuItem();
            miObjectMap = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(150, 24);
            menuStrip.TabIndex = 1;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { miProjectTools });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // miProjectTools
            // 
            miProjectTools.DropDownItems.AddRange(new ToolStripItem[] { miCloneChannels, miChannelMapByDevice, miChannelMapByObject, miDeviceMap, miObjectMap, miCheckIntegrity, miEncryptPassword, miSep, miImportTable, miExportTable });
            miProjectTools.Name = "miProjectTools";
            miProjectTools.Size = new Size(180, 22);
            miProjectTools.Text = "Project Tools";
            // 
            // miCloneChannels
            // 
            miCloneChannels.Name = "miCloneChannels";
            miCloneChannels.Size = new Size(199, 22);
            miCloneChannels.Text = "Clone Channels...";
            miCloneChannels.Click += miCloneChannels_Click;
            // 
            // miChannelMapByDevice
            // 
            miChannelMapByDevice.Name = "miChannelMapByDevice";
            miChannelMapByDevice.Size = new Size(199, 22);
            miChannelMapByDevice.Text = "Channel Map by Device";
            miChannelMapByDevice.Click += miChannelMap_Click;
            // 
            // miChannelMapByObject
            // 
            miChannelMapByObject.Name = "miChannelMapByObject";
            miChannelMapByObject.Size = new Size(199, 22);
            miChannelMapByObject.Text = "Channel Map by Object";
            miChannelMapByObject.Click += miChannelMap_Click;
            // 
            // miDeviceMap
            // 
            miDeviceMap.Name = "miDeviceMap";
            miDeviceMap.Size = new Size(199, 22);
            miDeviceMap.Text = "Device Map";
            miDeviceMap.Click += miDeviceMap_Click;
            // 
            // miCheckIntegrity
            // 
            miCheckIntegrity.Name = "miCheckIntegrity";
            miCheckIntegrity.Size = new Size(199, 22);
            miCheckIntegrity.Text = "Check Integrity";
            miCheckIntegrity.Click += miCheckIntegrity_Click;
            // 
            // miEncryptPassword
            // 
            miEncryptPassword.Name = "miEncryptPassword";
            miEncryptPassword.Size = new Size(199, 22);
            miEncryptPassword.Text = "Encrypt Password...";
            miEncryptPassword.Click += miEncryptPassword_Click;
            // 
            // miSep
            // 
            miSep.Name = "miSep";
            miSep.Size = new Size(196, 6);
            // 
            // miImportTable
            // 
            miImportTable.Name = "miImportTable";
            miImportTable.Size = new Size(199, 22);
            miImportTable.Text = "Import Table...";
            miImportTable.Click += miImportTable_Click;
            // 
            // miExportTable
            // 
            miExportTable.Name = "miExportTable";
            miExportTable.Size = new Size(199, 22);
            miExportTable.Text = "Export Table...";
            miExportTable.Click += miExportTable_Click;
            // 
            // miObjectMap
            // 
            miObjectMap.Name = "miObjectMap";
            miObjectMap.Size = new Size(199, 22);
            miObjectMap.Text = "Object Map";
            miObjectMap.Click += miObjectMap_Click;
            // 
            // CtrlMainMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(menuStrip);
            Name = "CtrlMainMenu";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem miProjectTools;
        private ToolStripMenuItem miCloneChannels;
        private ToolStripMenuItem miChannelMapByDevice;
        private ToolStripMenuItem miCheckIntegrity;
        private ToolStripSeparator miSep;
        private ToolStripMenuItem miImportTable;
        private ToolStripMenuItem miExportTable;
        private ToolStripMenuItem miChannelMapByObject;
        private ToolStripMenuItem miDeviceMap;
        private ToolStripMenuItem miEncryptPassword;
        private ToolStripMenuItem miObjectMap;
    }
}
