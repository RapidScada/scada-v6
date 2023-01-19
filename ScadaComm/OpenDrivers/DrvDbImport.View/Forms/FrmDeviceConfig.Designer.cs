namespace Scada.Comm.Drivers.DrvDbImport.View.Forms
{
    partial class FrmDeviceConfig
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAddQuery = new System.Windows.Forms.ToolStripButton();
            this.btnAddCommand = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.ctrlDbConnection = new Scada.Forms.Controls.CtrlDbConnection();
            this.ctrlCommand = new Scada.Comm.Drivers.DrvDbImport.View.Controls.CtrlCommand();
            this.ctrlQuery = new Scada.Comm.Drivers.DrvDbImport.View.Controls.CtrlQuery();
            this.lblHint = new System.Windows.Forms.Label();
            this.gbDevice = new System.Windows.Forms.GroupBox();
            this.tvDevice = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.pnlBottom.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.gbDevice.SuspendLayout();
            this.cmsTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 496);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(734, 45);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(566, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(647, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(485, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddQuery,
            this.btnAddCommand,
            this.toolStripSeparator1,
            this.btnMoveUp,
            this.btnMoveDown,
            this.btnDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "Add export target";
            // 
            // btnAddQuery
            // 
            this.btnAddQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddQuery.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.query;
            this.btnAddQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddQuery.Name = "btnAddQuery";
            this.btnAddQuery.Size = new System.Drawing.Size(23, 22);
            this.btnAddQuery.Text = "Add Query";
            this.btnAddQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
            // 
            // btnAddCommand
            // 
            this.btnAddCommand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCommand.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.cmd;
            this.btnAddCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCommand.Name = "btnAddCommand";
            this.btnAddCommand.Size = new System.Drawing.Size(23, 22);
            this.btnAddCommand.Text = "Add Command";
            this.btnAddCommand.Click += new System.EventHandler(this.btnAddCommand_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.move_up;
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.ToolTipText = "Move Up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.move_down;
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.ToolTipText = "Move Down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::Scada.Comm.Drivers.DrvDbImport.View.Properties.Resource.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTipText = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlInfo);
            this.pnlMain.Controls.Add(this.gbDevice);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 25);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(734, 471);
            this.pnlMain.TabIndex = 1;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlInfo.Controls.Add(this.ctrlDbConnection);
            this.pnlInfo.Controls.Add(this.ctrlCommand);
            this.pnlInfo.Controls.Add(this.ctrlQuery);
            this.pnlInfo.Controls.Add(this.lblHint);
            this.pnlInfo.Location = new System.Drawing.Point(318, 3);
            this.pnlInfo.Margin = new System.Windows.Forms.Padding(0);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(404, 462);
            this.pnlInfo.TabIndex = 2;
            // 
            // ctrlDbConnection
            // 
            this.ctrlDbConnection.BuildConnectionStringFunc = null;
            this.ctrlDbConnection.ConnectionOptions = null;
            this.ctrlDbConnection.DbmsEnabled = true;
            this.ctrlDbConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlDbConnection.Location = new System.Drawing.Point(0, 0);
            this.ctrlDbConnection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlDbConnection.Name = "ctrlDbConnection";
            this.ctrlDbConnection.NameEnabled = true;
            this.ctrlDbConnection.Size = new System.Drawing.Size(404, 462);
            this.ctrlDbConnection.TabIndex = 0;
            this.ctrlDbConnection.ConnectionOptionsChanged += new System.EventHandler(this.ctrlDbConnection_ConnectionOptionsChanged);
            // 
            // ctrlCommand
            // 
            this.ctrlCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlCommand.Location = new System.Drawing.Point(0, 0);
            this.ctrlCommand.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlCommand.Name = "ctrlCommand";
            this.ctrlCommand.Size = new System.Drawing.Size(404, 462);
            this.ctrlCommand.TabIndex = 2;
            this.ctrlCommand.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlCommand_ObjectChanged);
            // 
            // ctrlQuery
            // 
            this.ctrlQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlQuery.Location = new System.Drawing.Point(0, 0);
            this.ctrlQuery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ctrlQuery.Name = "ctrlQuery";
            this.ctrlQuery.Size = new System.Drawing.Size(404, 462);
            this.ctrlQuery.TabIndex = 1;
            this.ctrlQuery.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlQuery_ObjectChanged);
            // 
            // lblHint
            // 
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblHint.Location = new System.Drawing.Point(0, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(404, 74);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "Add";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbDevice
            // 
            this.gbDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbDevice.Controls.Add(this.tvDevice);
            this.gbDevice.Location = new System.Drawing.Point(12, 3);
            this.gbDevice.Name = "gbDevice";
            this.gbDevice.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbDevice.Size = new System.Drawing.Size(300, 462);
            this.gbDevice.TabIndex = 1;
            this.gbDevice.TabStop = false;
            this.gbDevice.Text = "Device Configuration";
            // 
            // tvDevice
            // 
            this.tvDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvDevice.ContextMenuStrip = this.cmsTree;
            this.tvDevice.HideSelection = false;
            this.tvDevice.ImageIndex = 0;
            this.tvDevice.ImageList = this.ilTree;
            this.tvDevice.Location = new System.Drawing.Point(13, 22);
            this.tvDevice.Name = "tvDevice";
            this.tvDevice.SelectedImageIndex = 0;
            this.tvDevice.Size = new System.Drawing.Size(274, 427);
            this.tvDevice.TabIndex = 0;
            this.tvDevice.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDevice_AfterSelect);
            // 
            // cmsTree
            // 
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCollapseAll});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(137, 26);
            // 
            // miCollapseAll
            // 
            this.miCollapseAll.Name = "miCollapseAll";
            this.miCollapseAll.Size = new System.Drawing.Size(136, 22);
            this.miCollapseAll.Text = "Collapse All";
            this.miCollapseAll.Click += new System.EventHandler(this.miCollapseAll_Click);
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FrmDeviceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(734, 541);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FrmDeviceConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Device {0} Properties";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDeviceConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmDeviceConfig_Load);
            this.pnlBottom.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.gbDevice.ResumeLayout(false);
            this.cmsTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlBottom;
        private Button btnClose;
        private Button btnSave;
        private ToolStrip toolStrip;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnMoveUp;
        private ToolStripButton btnMoveDown;
        private ToolStripButton btnDelete;
        private Panel pnlMain;
        private GroupBox gbDevice;
        private TreeView tvDevice;
        private Panel pnlInfo;
        private ImageList ilTree;
        private ToolStripButton btnAddQuery;
        private ToolStripButton btnAddCommand;
        private Button btnCancel;
        private Label lblHint;
        private ContextMenuStrip cmsTree;
        private ToolStripMenuItem miCollapseAll;
        private Controls.CtrlQuery ctrlQuery;
        private Controls.CtrlCommand ctrlCommand;
        private Scada.Forms.Controls.CtrlDbConnection ctrlDbConnection;
    }
}