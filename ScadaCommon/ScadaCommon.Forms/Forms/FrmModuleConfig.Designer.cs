namespace Scada.Forms.Forms
{
    partial class FrmModuleConfig
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
            toolStrip = new ToolStrip();
            btnAdd = new ToolStripButton();
            btnAddWithChoice = new ToolStripDropDownButton();
            btnMoveUp = new ToolStripButton();
            btnMoveDown = new ToolStripButton();
            btnDelete = new ToolStripButton();
            pnlBottom = new Panel();
            btnSave = new Button();
            btnClose = new Button();
            btnCancel = new Button();
            tableLayoutPanel = new TableLayoutPanel();
            treeView = new TreeView();
            cmsTree = new ContextMenuStrip(components);
            miCollapseAll = new ToolStripMenuItem();
            ilTree = new ImageList(components);
            propertyGrid = new PropertyGrid();
            toolStrip.SuspendLayout();
            pnlBottom.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            cmsTree.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnAdd, btnAddWithChoice, btnMoveUp, btnMoveDown, btnDelete });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(734, 25);
            toolStrip.TabIndex = 0;
            toolStrip.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            btnAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAdd.Image = Properties.Resources.add;
            btnAdd.ImageTransparentColor = Color.Magenta;
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(23, 22);
            btnAdd.ToolTipText = "Add";
            btnAdd.Click += btnAdd_Click;
            // 
            // btnAddWithChoice
            // 
            btnAddWithChoice.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddWithChoice.Image = Properties.Resources.add;
            btnAddWithChoice.ImageTransparentColor = Color.Magenta;
            btnAddWithChoice.Name = "btnAddWithChoice";
            btnAddWithChoice.Size = new Size(29, 22);
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
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Controls.Add(btnClose);
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 496);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(734, 45);
            pnlBottom.TabIndex = 2;
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
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel.Controls.Add(treeView, 0, 0);
            tableLayoutPanel.Controls.Add(propertyGrid, 1, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 25);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Size = new Size(734, 471);
            tableLayoutPanel.TabIndex = 1;
            // 
            // treeView
            // 
            treeView.ContextMenuStrip = cmsTree;
            treeView.Dock = DockStyle.Fill;
            treeView.HideSelection = false;
            treeView.ImageIndex = 0;
            treeView.ImageList = ilTree;
            treeView.Location = new Point(3, 3);
            treeView.Name = "treeView";
            treeView.SelectedImageIndex = 0;
            treeView.Size = new Size(287, 465);
            treeView.TabIndex = 0;
            treeView.AfterCollapse += treeView_AfterCollapseExpand;
            treeView.AfterExpand += treeView_AfterCollapseExpand;
            treeView.AfterSelect += treeView_AfterSelect;
            // 
            // cmsTree
            // 
            cmsTree.Items.AddRange(new ToolStripItem[] { miCollapseAll });
            cmsTree.Name = "cmsTree";
            cmsTree.Size = new Size(137, 26);
            // 
            // miCollapseAll
            // 
            miCollapseAll.Image = Properties.Resources.collapse_all;
            miCollapseAll.Name = "miCollapseAll";
            miCollapseAll.Size = new Size(136, 22);
            miCollapseAll.Text = "Collapse All";
            miCollapseAll.Click += miCollapseAll_Click;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth32Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // propertyGrid
            // 
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.Location = new Point(296, 3);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(435, 465);
            propertyGrid.TabIndex = 1;
            propertyGrid.PropertyValueChanged += propertyGrid_PropertyValueChanged;
            propertyGrid.SelectedGridItemChanged += propertyGrid_SelectedGridItemChanged;
            // 
            // FrmModuleConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(734, 541);
            Controls.Add(tableLayoutPanel);
            Controls.Add(pnlBottom);
            Controls.Add(toolStrip);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmModuleConfig";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Module Configuration";
            FormClosing += FrmModuleConfig_FormClosing;
            Load += FrmModuleConfig_Load;
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            pnlBottom.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            cmsTree.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip;
        private Panel pnlBottom;
        private Button btnSave;
        private Button btnClose;
        private Button btnCancel;
        private TableLayoutPanel tableLayoutPanel;
        private TreeView treeView;
        private PropertyGrid propertyGrid;
        private ToolStripButton btnAdd;
        private ToolStripButton btnMoveUp;
        private ToolStripDropDownButton btnAddWithChoice;
        private ToolStripButton btnMoveDown;
        private ToolStripButton btnDelete;
        private ImageList ilTree;
        private ContextMenuStrip cmsTree;
        private ToolStripMenuItem miCollapseAll;
    }
}