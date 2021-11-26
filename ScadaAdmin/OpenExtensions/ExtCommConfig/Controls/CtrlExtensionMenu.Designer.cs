
namespace Scada.Admin.Extensions.ExtCommConfig.Controls
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
            this.components = new System.ComponentModel.Container();
            this.cmsLine = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miLineImport = new System.Windows.Forms.ToolStripMenuItem();
            this.miLineSync = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.miLineAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miLineMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miLineMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.miLineDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miCommLineSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.miLineStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miLineStop = new System.Windows.Forms.ToolStripMenuItem();
            this.miLineRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDevice = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDeviceCommand = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeviceProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miWizards = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddLine = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateChannels = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAddLine = new System.Windows.Forms.ToolStripButton();
            this.btnAddDevice = new System.Windows.Forms.ToolStripButton();
            this.btnCreateChannels = new System.Windows.Forms.ToolStripButton();
            this.cmsLine.SuspendLayout();
            this.cmsDevice.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsLine
            // 
            this.cmsLine.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLineImport,
            this.miLineSync,
            this.miCommLineSep1,
            this.miLineAdd,
            this.miLineMoveUp,
            this.miLineMoveDown,
            this.miLineDelete,
            this.miCommLineSep2,
            this.miLineStart,
            this.miLineStop,
            this.miLineRestart});
            this.cmsLine.Name = "cmsCommLine";
            this.cmsLine.Size = new System.Drawing.Size(164, 214);
            this.cmsLine.Opening += new System.ComponentModel.CancelEventHandler(this.cmsLine_Opening);
            // 
            // miLineImport
            // 
            this.miLineImport.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.import;
            this.miLineImport.Name = "miLineImport";
            this.miLineImport.Size = new System.Drawing.Size(163, 22);
            this.miLineImport.Text = "Import...";
            this.miLineImport.Click += new System.EventHandler(this.miLineImport_Click);
            // 
            // miLineSync
            // 
            this.miLineSync.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.sync;
            this.miLineSync.Name = "miLineSync";
            this.miLineSync.Size = new System.Drawing.Size(163, 22);
            this.miLineSync.Text = "Synchronize...";
            this.miLineSync.Click += new System.EventHandler(this.miLineSync_Click);
            // 
            // miCommLineSep1
            // 
            this.miCommLineSep1.Name = "miCommLineSep1";
            this.miCommLineSep1.Size = new System.Drawing.Size(160, 6);
            // 
            // miLineAdd
            // 
            this.miLineAdd.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.add;
            this.miLineAdd.Name = "miLineAdd";
            this.miLineAdd.Size = new System.Drawing.Size(163, 22);
            this.miLineAdd.Text = "Add Line";
            this.miLineAdd.Click += new System.EventHandler(this.miLineAdd_Click);
            // 
            // miLineMoveUp
            // 
            this.miLineMoveUp.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.move_up;
            this.miLineMoveUp.Name = "miLineMoveUp";
            this.miLineMoveUp.Size = new System.Drawing.Size(163, 22);
            this.miLineMoveUp.Text = "Move Line Up";
            this.miLineMoveUp.Click += new System.EventHandler(this.miLineMoveUp_Click);
            // 
            // miLineMoveDown
            // 
            this.miLineMoveDown.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.move_down;
            this.miLineMoveDown.Name = "miLineMoveDown";
            this.miLineMoveDown.Size = new System.Drawing.Size(163, 22);
            this.miLineMoveDown.Text = "Move Line Down";
            this.miLineMoveDown.Click += new System.EventHandler(this.miLineMoveDown_Click);
            // 
            // miLineDelete
            // 
            this.miLineDelete.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.delete;
            this.miLineDelete.Name = "miLineDelete";
            this.miLineDelete.Size = new System.Drawing.Size(163, 22);
            this.miLineDelete.Text = "Delete Line";
            this.miLineDelete.Click += new System.EventHandler(this.miLineDelete_Click);
            // 
            // miCommLineSep2
            // 
            this.miCommLineSep2.Name = "miCommLineSep2";
            this.miCommLineSep2.Size = new System.Drawing.Size(160, 6);
            // 
            // miLineStart
            // 
            this.miLineStart.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.start;
            this.miLineStart.Name = "miLineStart";
            this.miLineStart.Size = new System.Drawing.Size(163, 22);
            this.miLineStart.Text = "Start Line";
            this.miLineStart.Click += new System.EventHandler(this.miLineStartStop_Click);
            // 
            // miLineStop
            // 
            this.miLineStop.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.stop;
            this.miLineStop.Name = "miLineStop";
            this.miLineStop.Size = new System.Drawing.Size(163, 22);
            this.miLineStop.Text = "Stop Line";
            this.miLineStop.Click += new System.EventHandler(this.miLineStartStop_Click);
            // 
            // miLineRestart
            // 
            this.miLineRestart.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.restart;
            this.miLineRestart.Name = "miLineRestart";
            this.miLineRestart.Size = new System.Drawing.Size(163, 22);
            this.miLineRestart.Text = "Restart Line";
            this.miLineRestart.Click += new System.EventHandler(this.miLineStartStop_Click);
            // 
            // cmsDevice
            // 
            this.cmsDevice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeviceCommand,
            this.miDeviceProperties});
            this.cmsDevice.Name = "cmsDevice";
            this.cmsDevice.Size = new System.Drawing.Size(170, 48);
            // 
            // miDeviceCommand
            // 
            this.miDeviceCommand.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.cmd;
            this.miDeviceCommand.Name = "miDeviceCommand";
            this.miDeviceCommand.Size = new System.Drawing.Size(169, 22);
            this.miDeviceCommand.Text = "Send Command...";
            this.miDeviceCommand.Click += new System.EventHandler(this.miDeviceCommand_Click);
            // 
            // miDeviceProperties
            // 
            this.miDeviceProperties.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.properties;
            this.miDeviceProperties.Name = "miDeviceProperties";
            this.miDeviceProperties.Size = new System.Drawing.Size(169, 22);
            this.miDeviceProperties.Text = "Properies";
            this.miDeviceProperties.Click += new System.EventHandler(this.miDeviceProperties_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTools});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(150, 24);
            this.menuStrip.TabIndex = 2;
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miWizards});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(46, 20);
            this.miTools.Text = "Tools";
            // 
            // miWizards
            // 
            this.miWizards.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddLine,
            this.miAddDevice,
            this.miCreateChannels});
            this.miWizards.Name = "miWizards";
            this.miWizards.Size = new System.Drawing.Size(180, 22);
            this.miWizards.Text = "Wizards";
            // 
            // miAddLine
            // 
            this.miAddLine.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.add_line;
            this.miAddLine.Name = "miAddLine";
            this.miAddLine.Size = new System.Drawing.Size(180, 22);
            this.miAddLine.Text = "Add Line...";
            this.miAddLine.Click += new System.EventHandler(this.miAddLine_Click);
            // 
            // miAddDevice
            // 
            this.miAddDevice.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.add_device;
            this.miAddDevice.Name = "miAddDevice";
            this.miAddDevice.Size = new System.Drawing.Size(180, 22);
            this.miAddDevice.Text = "Add Device...";
            this.miAddDevice.Click += new System.EventHandler(this.miAddDevice_Click);
            // 
            // miCreateChannels
            // 
            this.miCreateChannels.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.create_cnls;
            this.miCreateChannels.Name = "miCreateChannels";
            this.miCreateChannels.Size = new System.Drawing.Size(180, 22);
            this.miCreateChannels.Text = "Create Channels...";
            this.miCreateChannels.Click += new System.EventHandler(this.miCreateChannels_Click);
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
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnAddLine
            // 
            this.btnAddLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddLine.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.add_line;
            this.btnAddLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(23, 22);
            this.btnAddLine.ToolTipText = "Add Communication Line";
            this.btnAddLine.Click += new System.EventHandler(this.miAddLine_Click);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddDevice.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.add_device;
            this.btnAddDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(23, 22);
            this.btnAddDevice.ToolTipText = "Add Device";
            this.btnAddDevice.Click += new System.EventHandler(this.miAddDevice_Click);
            // 
            // btnCreateChannels
            // 
            this.btnCreateChannels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCreateChannels.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.create_cnls;
            this.btnCreateChannels.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateChannels.Name = "btnCreateChannels";
            this.btnCreateChannels.Size = new System.Drawing.Size(23, 22);
            this.btnCreateChannels.ToolTipText = "Create Channels";
            this.btnCreateChannels.Click += new System.EventHandler(this.miCreateChannels_Click);
            // 
            // CtrlExtensionMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Name = "CtrlExtensionMenu";
            this.cmsLine.ResumeLayout(false);
            this.cmsDevice.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsLine;
        private System.Windows.Forms.ToolStripMenuItem miLineImport;
        private System.Windows.Forms.ToolStripMenuItem miLineSync;
        private System.Windows.Forms.ToolStripSeparator miCommLineSep1;
        private System.Windows.Forms.ToolStripMenuItem miLineAdd;
        private System.Windows.Forms.ToolStripMenuItem miLineMoveUp;
        private System.Windows.Forms.ToolStripMenuItem miLineMoveDown;
        private System.Windows.Forms.ToolStripMenuItem miLineDelete;
        private System.Windows.Forms.ToolStripSeparator miCommLineSep2;
        private System.Windows.Forms.ToolStripMenuItem miLineStart;
        private System.Windows.Forms.ToolStripMenuItem miLineStop;
        private System.Windows.Forms.ToolStripMenuItem miLineRestart;
        private System.Windows.Forms.ContextMenuStrip cmsDevice;
        private System.Windows.Forms.ToolStripMenuItem miDeviceCommand;
        private System.Windows.Forms.ToolStripMenuItem miDeviceProperties;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miWizards;
        private System.Windows.Forms.ToolStripMenuItem miAddLine;
        private System.Windows.Forms.ToolStripMenuItem miAddDevice;
        private System.Windows.Forms.ToolStripMenuItem miCreateChannels;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnAddLine;
        private System.Windows.Forms.ToolStripButton btnAddDevice;
        private System.Windows.Forms.ToolStripButton btnCreateChannels;
    }
}
