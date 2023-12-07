namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View.Forms
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
            components = new System.ComponentModel.Container();
            TreeNode treeNode1 = new TreeNode("Element groups");
            treeView = new TreeView();
            cmsTree = new ContextMenuStrip(components);
            miCollapseElemGroups = new ToolStripMenuItem();
            miCloneElemConfig = new ToolStripMenuItem();
            ilTree = new ImageList(components);
            toolStrip = new ToolStrip();
            btnNew = new ToolStripButton();
            btnOpen = new ToolStripButton();
            btnSave = new ToolStripButton();
            btnSaveAs = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnAddElemGroup = new ToolStripButton();
            btnAddElem = new ToolStripButton();
            btnMoveUp = new ToolStripButton();
            btnMoveDown = new ToolStripButton();
            btnDelete = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnEditOptions = new ToolStripButton();
            btnEditOptionsExt = new ToolStripButton();
            btnValidate = new ToolStripButton();
            gbTemplate = new GroupBox();
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            ctrlElemGroup = new Controls.CtrlElemGroup();
            ctrlElem = new Controls.CtrlElem();
            cmsTree.SuspendLayout();
            toolStrip.SuspendLayout();
            gbTemplate.SuspendLayout();
            SuspendLayout();
            // 
            // treeView
            // 
            treeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeView.ContextMenuStrip = cmsTree;
            treeView.HideSelection = false;
            treeView.ImageIndex = 0;
            treeView.ImageList = ilTree;
            treeView.Location = new Point(13, 25);
            treeView.Name = "treeView";
            treeNode1.ImageKey = "(по умолчанию)";
            treeNode1.Name = "elemGroupsNode";
            treeNode1.SelectedImageKey = "(по умолчанию)";
            treeNode1.Text = "Element groups";
            treeView.Nodes.AddRange(new TreeNode[] { treeNode1 });
            treeView.SelectedImageIndex = 0;
            treeView.ShowRootLines = false;
            treeView.Size = new Size(274, 528);
            treeView.TabIndex = 0;
            treeView.AfterSelect += treeView_AfterSelect;
            treeView.NodeMouseClick += treeView_NodeMouseClick;
            // 
            // cmsTree
            // 
            cmsTree.Items.AddRange(new ToolStripItem[] { miCollapseElemGroups, miCloneElemConfig });
            cmsTree.Name = "cmsTree";
            cmsTree.Size = new Size(230, 48);
            cmsTree.Opening += cmsTree_Opening;
            // 
            // miCollapseElemGroups
            // 
            miCollapseElemGroups.Image = Properties.Resources.collapse_all;
            miCollapseElemGroups.Name = "miCollapseElemGroups";
            miCollapseElemGroups.Size = new Size(229, 22);
            miCollapseElemGroups.Text = "Collapse Element Groups";
            miCollapseElemGroups.Click += miCollapseElemGroups_Click;
            // 
            // miCloneElemConfig
            // 
            miCloneElemConfig.Image = Properties.Resources.clone;
            miCloneElemConfig.Name = "miCloneElemConfig";
            miCloneElemConfig.Size = new Size(229, 22);
            miCloneElemConfig.Text = "Clone Element Parameters";
            miCloneElemConfig.Click += miCloneElemConfig_Click;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth24Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnNew, btnOpen, btnSave, btnSaveAs, toolStripSeparator1, btnAddElemGroup, btnAddElem, btnMoveUp, btnMoveDown, btnDelete, toolStripSeparator2, btnEditOptions, btnEditOptionsExt, btnValidate });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(630, 25);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // btnNew
            // 
            btnNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnNew.Image = Properties.Resources.blank;
            btnNew.ImageTransparentColor = Color.Magenta;
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(23, 22);
            btnNew.ToolTipText = "New Template";
            btnNew.Click += btnNew_Click;
            // 
            // btnOpen
            // 
            btnOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnOpen.Image = Properties.Resources.open;
            btnOpen.ImageTransparentColor = Color.Magenta;
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(23, 22);
            btnOpen.ToolTipText = "Open Template";
            btnOpen.Click += btnOpen_Click;
            // 
            // btnSave
            // 
            btnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageTransparentColor = Color.Magenta;
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(23, 22);
            btnSave.ToolTipText = "Save Template";
            btnSave.Click += btnSave_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSaveAs.Image = Properties.Resources.save_as;
            btnSaveAs.ImageTransparentColor = Color.Magenta;
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(23, 22);
            btnSaveAs.ToolTipText = "Save Template As";
            btnSaveAs.Click += btnSave_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnAddElemGroup
            // 
            btnAddElemGroup.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddElemGroup.Image = Properties.Resources.group;
            btnAddElemGroup.ImageTransparentColor = Color.Magenta;
            btnAddElemGroup.Name = "btnAddElemGroup";
            btnAddElemGroup.Size = new Size(23, 22);
            btnAddElemGroup.ToolTipText = "Add Element Group";
            btnAddElemGroup.Click += btnAddElemGroup_Click;
            // 
            // btnAddElem
            // 
            btnAddElem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddElem.Image = Properties.Resources.elem;
            btnAddElem.ImageTransparentColor = Color.Magenta;
            btnAddElem.Name = "btnAddElem";
            btnAddElem.Size = new Size(23, 22);
            btnAddElem.ToolTipText = "Add Element";
            btnAddElem.Click += btnAddElem_Click;
            // 
            // btnMoveUp
            // 
            btnMoveUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveUp.Image = Properties.Resources.move_up;
            btnMoveUp.ImageTransparentColor = Color.Magenta;
            btnMoveUp.Name = "btnMoveUp";
            btnMoveUp.Size = new Size(23, 22);
            btnMoveUp.ToolTipText = "Move Up";
            btnMoveUp.Click += btnMoveUp_Click;
            // 
            // btnMoveDown
            // 
            btnMoveDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveDown.Image = Properties.Resources.move_down;
            btnMoveDown.ImageTransparentColor = Color.Magenta;
            btnMoveDown.Name = "btnMoveDown";
            btnMoveDown.Size = new Size(23, 22);
            btnMoveDown.ToolTipText = "Move Down";
            btnMoveDown.Click += btnMoveDown_Click;
            // 
            // btnDelete
            // 
            btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDelete.Image = Properties.Resources.delete;
            btnDelete.ImageTransparentColor = Color.Magenta;
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(23, 22);
            btnDelete.ToolTipText = "Delete";
            btnDelete.Click += btnDelete_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // btnEditOptions
            // 
            btnEditOptions.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnEditOptions.Image = Properties.Resources.options;
            btnEditOptions.ImageTransparentColor = Color.Magenta;
            btnEditOptions.Name = "btnEditOptions";
            btnEditOptions.Size = new Size(23, 22);
            btnEditOptions.ToolTipText = "Edit Template Options";
            btnEditOptions.Visible = false;
            btnEditOptions.Click += btnEditOptions_Click;
            // 
            // btnEditOptionsExt
            // 
            btnEditOptionsExt.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnEditOptionsExt.Image = Properties.Resources.options_extended;
            btnEditOptionsExt.ImageTransparentColor = Color.Magenta;
            btnEditOptionsExt.Name = "btnEditOptionsExt";
            btnEditOptionsExt.Size = new Size(23, 22);
            btnEditOptionsExt.ToolTipText = "Edit Extended Options";
            btnEditOptionsExt.Click += btnEditOptionsExt_Click;
            // 
            // btnValidate
            // 
            btnValidate.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnValidate.Image = Properties.Resources.validate;
            btnValidate.ImageTransparentColor = Color.Magenta;
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(23, 22);
            btnValidate.ToolTipText = "Validate Template";
            btnValidate.Click += btnValidate_Click;
            // 
            // gbTemplate
            // 
            gbTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gbTemplate.Controls.Add(treeView);
            gbTemplate.Location = new Point(12, 32);
            gbTemplate.Name = "gbTemplate";
            gbTemplate.Padding = new Padding(10, 3, 10, 11);
            gbTemplate.Size = new Size(300, 568);
            gbTemplate.TabIndex = 1;
            gbTemplate.TabStop = false;
            gbTemplate.Text = "Device Template";
            // 
            // openFileDialog
            // 
            openFileDialog.DefaultExt = "*.xml";
            openFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 0;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "*.xml";
            saveFileDialog.Filter = "Template Files (*.xml)|*.xml|All Files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            // 
            // ctrlElemGroup
            // 
            ctrlElemGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ctrlElemGroup.ElemGroup = null;
            ctrlElemGroup.Location = new Point(318, 32);
            ctrlElemGroup.Name = "ctrlElemGroup";
            ctrlElemGroup.Size = new Size(300, 412);
            ctrlElemGroup.TabIndex = 2;
            ctrlElemGroup.TemplateOptions = null;
            ctrlElemGroup.ObjectChanged += ctrlElemGroup_ObjectChanged;
            // 
            // ctrlElem
            // 
            ctrlElem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ctrlElem.ElemTag = null;
            ctrlElem.Location = new Point(318, 79);
            ctrlElem.Name = "ctrlElem";
            ctrlElem.Size = new Size(300, 448);
            ctrlElem.TabIndex = 3;
            ctrlElem.ObjectChanged += ctrlElem_ObjectChanged;
            // 
            // FrmDeviceTemplate
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 613);
            Controls.Add(ctrlElemGroup);
            Controls.Add(ctrlElem);
            Controls.Add(gbTemplate);
            Controls.Add(toolStrip);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(646, 505);
            Name = "FrmDeviceTemplate";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "MODBUS. Device Template Editor";
            FormClosing += FrmDevTemplate_FormClosing;
            Load += FrmDevTemplate_Load;
            cmsTree.ResumeLayout(false);
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            gbTemplate.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnAddElemGroup;
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
        private Scada.Comm.Drivers.DrvGoogleBigQueue.View.Controls.CtrlElem ctrlElem;
        private Scada.Comm.Drivers.DrvGoogleBigQueue.View.Controls.CtrlElemGroup ctrlElemGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnEditOptions;
        private System.Windows.Forms.ToolStripButton btnEditOptionsExt;
        private System.Windows.Forms.ToolStripButton btnValidate;
        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ToolStripMenuItem miCollapseElemGroups;
        private System.Windows.Forms.ToolStripMenuItem miCloneElemConfig;
    }
}