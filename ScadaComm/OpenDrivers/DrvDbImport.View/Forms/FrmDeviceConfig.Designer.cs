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
            components = new System.ComponentModel.Container();
            pnlBottom = new Panel();
            btnClose = new Button();
            btnCancel = new Button();
            btnSave = new Button();
            toolStrip = new ToolStrip();
            btnAddQuery = new ToolStripButton();
            btnAddCommand = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnMoveUp = new ToolStripButton();
            btnMoveDown = new ToolStripButton();
            btnDelete = new ToolStripButton();
            pnlMain = new Panel();
            pnlOptions = new Panel();
            ctrlDbConnection = new Scada.Forms.Controls.CtrlDbConnection();
            ctrlCommand = new Scada.Comm.Drivers.DrvDbImport.View.Controls.CtrlCommand();
            ctrlQuery = new Scada.Comm.Drivers.DrvDbImport.View.Controls.CtrlQuery();
            lblHint = new Label();
            gbDevice = new GroupBox();
            tvDevice = new TreeView();
            cmsTree = new ContextMenuStrip(components);
            miCollapseAll = new ToolStripMenuItem();
            ilTree = new ImageList(components);
            pnlBottom.SuspendLayout();
            toolStrip.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlOptions.SuspendLayout();
            gbDevice.SuspendLayout();
            cmsTree.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 496);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(734, 45);
            pnlBottom.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(647, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 2;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.Location = new Point(566, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(485, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAddQuery, btnAddCommand, toolStripSeparator1, btnMoveUp, btnMoveDown, btnDelete });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(734, 25);
            toolStrip.TabIndex = 0;
            // 
            // btnAddQuery
            // 
            btnAddQuery.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddQuery.Image = Properties.Resource.query;
            btnAddQuery.ImageTransparentColor = Color.Magenta;
            btnAddQuery.Name = "btnAddQuery";
            btnAddQuery.Size = new Size(23, 22);
            btnAddQuery.Text = "Add Query";
            btnAddQuery.Click += btnAddQuery_Click;
            // 
            // btnAddCommand
            // 
            btnAddCommand.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddCommand.Image = Properties.Resource.cmd;
            btnAddCommand.ImageTransparentColor = Color.Magenta;
            btnAddCommand.Name = "btnAddCommand";
            btnAddCommand.Size = new Size(23, 22);
            btnAddCommand.Text = "Add Command";
            btnAddCommand.Click += btnAddCommand_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnMoveUp
            // 
            btnMoveUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveUp.Image = Properties.Resource.move_up;
            btnMoveUp.ImageTransparentColor = Color.Magenta;
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(23, 22);
            btnMoveUp.Text = "Move Up";
            btnMoveUp.ToolTipText = "Move Up";
            btnMoveUp.Click += btnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveDown.Image = Properties.Resource.move_down;
            btnMoveDown.ImageTransparentColor = Color.Magenta;
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(23, 22);
            btnMoveDown.Text = "Move Down";
            btnMoveDown.ToolTipText = "Move Down";
            btnMoveDown.Click += btnMoveDown_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDelete.Image = Properties.Resource.delete;
            btnDelete.ImageTransparentColor = Color.Magenta;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(23, 22);
            btnDelete.Text = "Delete";
            btnDelete.ToolTipText = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlOptions);
            pnlMain.Controls.Add(gbDevice);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 25);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(734, 471);
            pnlMain.TabIndex = 1;
            // 
            // pnlOptions
            // 
            pnlOptions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlOptions.Controls.Add(ctrlDbConnection);
            pnlOptions.Controls.Add(ctrlCommand);
            pnlOptions.Controls.Add(ctrlQuery);
            pnlOptions.Controls.Add(lblHint);
            pnlOptions.Location = new Point(318, 3);
            pnlOptions.Margin = new Padding(0);
            pnlOptions.Name = "pnlOptions";
            pnlOptions.Size = new Size(404, 462);
            pnlOptions.TabIndex = 2;
            // 
            // ctrlDbConnection
            // 
            ctrlDbConnection.BuildConnectionStringFunc = null;
            ctrlDbConnection.ConnectionOptions = null;
            ctrlDbConnection.DbmsEnabled = true;
            ctrlDbConnection.Dock = DockStyle.Fill;
            ctrlDbConnection.Location = new Point(0, 0);
            ctrlDbConnection.Margin = new Padding(3, 3, 3, 10);
            ctrlDbConnection.Name = "ctrlDbConnection";
            ctrlDbConnection.NameEnabled = true;
            ctrlDbConnection.Size = new Size(404, 462);
            ctrlDbConnection.TabIndex = 0;
            ctrlDbConnection.ConnectionOptionsChanged += ctrlDbConnection_ConnectionOptionsChanged;
            // 
            // ctrlCommand
            // 
            ctrlCommand.Dock = DockStyle.Fill;
            ctrlCommand.Location = new Point(0, 0);
            ctrlCommand.Margin = new Padding(3, 3, 3, 10);
            ctrlCommand.Name = "ctrlCommand";
            ctrlCommand.Size = new Size(404, 462);
            ctrlCommand.TabIndex = 2;
            ctrlCommand.ObjectChanged += ctrlCommand_ObjectChanged;
            // 
            // ctrlQuery
            // 
            ctrlQuery.Dock = DockStyle.Fill;
            ctrlQuery.Location = new Point(0, 0);
            ctrlQuery.Margin = new Padding(3, 3, 3, 10);
            ctrlQuery.Name = "ctrlQuery";
            ctrlQuery.Size = new Size(404, 462);
            ctrlQuery.TabIndex = 1;
            ctrlQuery.ObjectChanged += ctrlQuery_ObjectChanged;
            // 
            // lblHint
            // 
            lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblHint.Font = new Font("Segoe UI", 11.25F);
            lblHint.ForeColor = SystemColors.GrayText;
            lblHint.Location = new Point(0, 0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(404, 74);
            lblHint.TabIndex = 0;
            lblHint.Text = "Add";
            lblHint.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbDevice
            // 
            gbDevice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            gbDevice.Controls.Add(tvDevice);
            gbDevice.Location = new Point(12, 3);
            gbDevice.Name = "gbDevice";
            gbDevice.Padding = new Padding(10, 3, 10, 10);
            gbDevice.Size = new Size(300, 462);
            gbDevice.TabIndex = 1;
            gbDevice.TabStop = false;
            gbDevice.Text = "Device Configuration";
            // 
            // tvDevice
            // 
            tvDevice.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tvDevice.ContextMenuStrip = cmsTree;
            tvDevice.HideSelection = false;
            tvDevice.ImageIndex = 0;
            tvDevice.ImageList = ilTree;
            tvDevice.Location = new Point(13, 22);
            tvDevice.Name = "tvDevice";
            tvDevice.SelectedImageIndex = 0;
            tvDevice.Size = new Size(274, 427);
            tvDevice.TabIndex = 0;
            tvDevice.AfterSelect += tvDevice_AfterSelect;
            // 
            // cmsTree
            // 
            cmsTree.Items.AddRange(new ToolStripItem[] { miCollapseAll });
            cmsTree.Name = "cmsTree";
            cmsTree.Size = new Size(137, 26);
            // 
            // miCollapseAll
            // 
            miCollapseAll.Image = Properties.Resource.collapse_all;
            miCollapseAll.Name = "miCollapseAll";
            miCollapseAll.Size = new Size(136, 22);
            miCollapseAll.Text = "Collapse All";
            miCollapseAll.Click += miCollapseAll_Click;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth24Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // FrmDeviceConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(734, 541);
            Controls.Add(pnlMain);
            Controls.Add(toolStrip);
            Controls.Add(pnlBottom);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(500, 300);
            Name = "FrmDeviceConfig";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Device {0} Properties - DB Import";
            FormClosing += FrmDeviceConfig_FormClosing;
            Load += FrmDeviceConfig_Load;
            pnlBottom.ResumeLayout(false);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlOptions.ResumeLayout(false);
            gbDevice.ResumeLayout(false);
            cmsTree.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

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
        private Panel pnlOptions;
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