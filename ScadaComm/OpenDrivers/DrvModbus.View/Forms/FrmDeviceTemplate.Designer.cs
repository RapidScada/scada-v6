namespace Scada.Comm.Drivers.DrvModbus.View.Forms
{
    partial class FrmDeviceTemplate
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Element groups");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Commands");
            this.treeView = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCollapseElemGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.miCloneElemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddElemGroup = new System.Windows.Forms.ToolStripButton();
            this.btnAddElem = new System.Windows.Forms.ToolStripButton();
            this.btnAddCmd = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnEditOptions = new System.Windows.Forms.ToolStripButton();
            this.btnEditOptionsExt = new System.Windows.Forms.ToolStripButton();
            this.btnValidate = new System.Windows.Forms.ToolStripButton();
            this.gbTemplate = new System.Windows.Forms.GroupBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ctrlElemGroup = new Scada.Comm.Drivers.DrvModbus.View.Controls.CtrlElemGroup();
            this.ctrlElem = new Scada.Comm.Drivers.DrvModbus.View.Controls.CtrlElem();
            this.ctrlCmd = new Scada.Comm.Drivers.DrvModbus.View.Controls.CtrlCmd();
            this.cmsTree.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.gbTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.ContextMenuStrip = this.cmsTree;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(13, 22);
            this.treeView.Name = "treeView";
            treeNode1.ImageKey = "(по умолчанию)";
            treeNode1.Name = "elemGroupsNode";
            treeNode1.SelectedImageKey = "(по умолчанию)";
            treeNode1.Text = "Element groups";
            treeNode2.ImageKey = "(по умолчанию)";
            treeNode2.Name = "commandsNode";
            treeNode2.SelectedImageKey = "(по умолчанию)";
            treeNode2.Text = "Commands";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowRootLines = false;
            this.treeView.Size = new System.Drawing.Size(274, 466);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            // 
            // cmsTree
            // 
            this.cmsTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCollapseElemGroups,
            this.miCloneElemConfig});
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(214, 70);
            this.cmsTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTree_Opening);
            // 
            // miCollapseElemGroups
            // 
            this.miCollapseElemGroups.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.collapse_all;
            this.miCollapseElemGroups.Name = "miCollapseElemGroups";
            this.miCollapseElemGroups.Size = new System.Drawing.Size(213, 22);
            this.miCollapseElemGroups.Text = "Collapse Element Groups";
            this.miCollapseElemGroups.Click += new System.EventHandler(this.miCollapseElemGroups_Click);
            // 
            // miCloneElemConfig
            // 
            this.miCloneElemConfig.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.clone;
            this.miCloneElemConfig.Name = "miCloneElemConfig";
            this.miCloneElemConfig.Size = new System.Drawing.Size(213, 22);
            this.miCloneElemConfig.Text = "Clone Element Parameters";
            this.miCloneElemConfig.Click += new System.EventHandler(this.miCloneElemConfig_Click);
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnSaveAs,
            this.toolStripSeparator1,
            this.btnAddElemGroup,
            this.btnAddElem,
            this.btnAddCmd,
            this.btnMoveUp,
            this.btnMoveDown,
            this.btnDelete,
            this.toolStripSeparator2,
            this.btnEditOptions,
            this.btnEditOptionsExt,
            this.btnValidate});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(630, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.blank;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(23, 22);
            this.btnNew.ToolTipText = "New Template";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.open;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.ToolTipText = "Open Template";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.ToolTipText = "Save Template";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAs.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.save_as;
            this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(23, 22);
            this.btnSaveAs.ToolTipText = "Save Template As";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddElemGroup
            // 
            this.btnAddElemGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddElemGroup.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.group;
            this.btnAddElemGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddElemGroup.Name = "btnAddElemGroup";
            this.btnAddElemGroup.Size = new System.Drawing.Size(23, 22);
            this.btnAddElemGroup.ToolTipText = "Add Element Group";
            this.btnAddElemGroup.Click += new System.EventHandler(this.btnAddElemGroup_Click);
            // 
            // btnAddElem
            // 
            this.btnAddElem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddElem.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.elem;
            this.btnAddElem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddElem.Name = "btnAddElem";
            this.btnAddElem.Size = new System.Drawing.Size(23, 22);
            this.btnAddElem.ToolTipText = "Add Element";
            this.btnAddElem.Click += new System.EventHandler(this.btnAddElem_Click);
            // 
            // btnAddCmd
            // 
            this.btnAddCmd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddCmd.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.cmd;
            this.btnAddCmd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCmd.Name = "btnAddCmd";
            this.btnAddCmd.Size = new System.Drawing.Size(23, 22);
            this.btnAddCmd.ToolTipText = "Add Command";
            this.btnAddCmd.Click += new System.EventHandler(this.btnAddCmd_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.move_up;
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.ToolTipText = "Move Up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.move_down;
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.ToolTipText = "Move Down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.ToolTipText = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnEditOptions
            // 
            this.btnEditOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditOptions.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.options;
            this.btnEditOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditOptions.Name = "btnEditOptions";
            this.btnEditOptions.Size = new System.Drawing.Size(23, 22);
            this.btnEditOptions.ToolTipText = "Edit Template Options";
            this.btnEditOptions.Click += new System.EventHandler(this.btnEditOptions_Click);
            // 
            // btnEditOptionsExt
            // 
            this.btnEditOptionsExt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditOptionsExt.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.options_extended;
            this.btnEditOptionsExt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditOptionsExt.Name = "btnEditOptionsExt";
            this.btnEditOptionsExt.Size = new System.Drawing.Size(23, 22);
            this.btnEditOptionsExt.ToolTipText = "Edit Extended Options";
            this.btnEditOptionsExt.Click += new System.EventHandler(this.btnEditOptionsExt_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnValidate.Image = global::Scada.Comm.Drivers.DrvModbus.View.Properties.Resources.validate;
            this.btnValidate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(23, 22);
            this.btnValidate.ToolTipText = "Validate Template";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // gbTemplate
            // 
            this.gbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTemplate.Controls.Add(this.treeView);
            this.gbTemplate.Location = new System.Drawing.Point(12, 28);
            this.gbTemplate.Name = "gbTemplate";
            this.gbTemplate.Padding = new System.Windows.Forms.Padding(10, 3, 10, 10);
            this.gbTemplate.Size = new System.Drawing.Size(300, 501);
            this.gbTemplate.TabIndex = 1;
            this.gbTemplate.TabStop = false;
            this.gbTemplate.Text = "Device Template";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.xml";
            this.openFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            this.openFileDialog.FilterIndex = 0;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.xml";
            this.saveFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            this.saveFileDialog.FilterIndex = 0;
            // 
            // ctrlElemGroup
            // 
            this.ctrlElemGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlElemGroup.ElemGroup = null;
            this.ctrlElemGroup.Location = new System.Drawing.Point(318, 28);
            this.ctrlElemGroup.Name = "ctrlElemGroup";
            this.ctrlElemGroup.Size = new System.Drawing.Size(300, 273);
            this.ctrlElemGroup.TabIndex = 2;
            this.ctrlElemGroup.TemplateOptions = null;
            this.ctrlElemGroup.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlElemGroup_ObjectChanged);
            // 
            // ctrlElem
            // 
            this.ctrlElem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlElem.ElemTag = null;
            this.ctrlElem.Location = new System.Drawing.Point(318, 70);
            this.ctrlElem.Name = "ctrlElem";
            this.ctrlElem.Size = new System.Drawing.Size(300, 395);
            this.ctrlElem.TabIndex = 3;
            this.ctrlElem.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlElem_ObjectChanged);
            // 
            // ctrlCmd
            // 
            this.ctrlCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlCmd.Cmd = null;
            this.ctrlCmd.Location = new System.Drawing.Point(318, 120);
            this.ctrlCmd.Name = "ctrlCmd";
            this.ctrlCmd.Size = new System.Drawing.Size(300, 409);
            this.ctrlCmd.TabIndex = 4;
            this.ctrlCmd.TemplateOptions = null;
            this.ctrlCmd.ObjectChanged += new System.EventHandler<Scada.Forms.ObjectChangedEventArgs>(this.ctrlCmd_ObjectChanged);
            // 
            // FrmDeviceTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 541);
            this.Controls.Add(this.ctrlElemGroup);
            this.Controls.Add(this.ctrlElem);
            this.Controls.Add(this.ctrlCmd);
            this.Controls.Add(this.gbTemplate);
            this.Controls.Add(this.toolStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(646, 450);
            this.Name = "FrmDeviceTemplate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MODBUS. Device Template Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDevTemplate_FormClosing);
            this.Load += new System.EventHandler(this.FrmDevTemplate_Load);
            this.cmsTree.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.gbTemplate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnAddElemGroup;
        private System.Windows.Forms.ToolStripButton btnAddCmd;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.GroupBox gbTemplate;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripButton btnSaveAs;
        private System.Windows.Forms.ToolStripButton btnAddElem;
        private Scada.Comm.Drivers.DrvModbus.View.Controls.CtrlCmd ctrlCmd;
        private Scada.Comm.Drivers.DrvModbus.View.Controls.CtrlElem ctrlElem;
        private Scada.Comm.Drivers.DrvModbus.View.Controls.CtrlElemGroup ctrlElemGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnEditOptions;
        private System.Windows.Forms.ToolStripButton btnEditOptionsExt;
        private System.Windows.Forms.ToolStripButton btnValidate;
        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ToolStripMenuItem miCollapseElemGroups;
        private System.Windows.Forms.ToolStripMenuItem miCloneElemConfig;
    }
}