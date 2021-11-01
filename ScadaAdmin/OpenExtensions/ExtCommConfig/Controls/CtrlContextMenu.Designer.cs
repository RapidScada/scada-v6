
namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    partial class CtrlContextMenu
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
            this.cmsLine.SuspendLayout();
            this.cmsDevice.SuspendLayout();
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
            this.miLineStart.Click += new System.EventHandler(this.miLineStart_Click);
            // 
            // miLineStop
            // 
            this.miLineStop.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.stop;
            this.miLineStop.Name = "miLineStop";
            this.miLineStop.Size = new System.Drawing.Size(163, 22);
            this.miLineStop.Text = "Stop Line";
            this.miLineStop.Click += new System.EventHandler(this.miLineStop_Click);
            // 
            // miLineRestart
            // 
            this.miLineRestart.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.restart;
            this.miLineRestart.Name = "miLineRestart";
            this.miLineRestart.Size = new System.Drawing.Size(163, 22);
            this.miLineRestart.Text = "Restart Line";
            this.miLineRestart.Click += new System.EventHandler(this.miLineRestart_Click);
            // 
            // cmsDevice
            // 
            this.cmsDevice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeviceCommand,
            this.miDeviceProperties});
            this.cmsDevice.Name = "cmsDevice";
            this.cmsDevice.Size = new System.Drawing.Size(181, 70);
            this.cmsDevice.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDevice_Opening);
            // 
            // miDeviceCommand
            // 
            this.miDeviceCommand.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.cmd;
            this.miDeviceCommand.Name = "miDeviceCommand";
            this.miDeviceCommand.Size = new System.Drawing.Size(180, 22);
            this.miDeviceCommand.Text = "Send Command...";
            this.miDeviceCommand.Click += new System.EventHandler(this.miDeviceCommand_Click);
            // 
            // miDeviceProperties
            // 
            this.miDeviceProperties.Image = global::Scada.Admin.Extensions.ExtCommConfig.Properties.Resources.properties;
            this.miDeviceProperties.Name = "miDeviceProperties";
            this.miDeviceProperties.Size = new System.Drawing.Size(180, 22);
            this.miDeviceProperties.Text = "Properies";
            this.miDeviceProperties.Click += new System.EventHandler(this.miDeviceProperties_Click);
            // 
            // CtrlContextMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CtrlContextMenu";
            this.cmsLine.ResumeLayout(false);
            this.cmsDevice.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}
