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
            this.components = new System.ComponentModel.Container();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnAddWithChoice = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ilTree = new System.Windows.Forms.ImageList(this.components);
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.cmsTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnAddWithChoice,
            this.btnMoveUp,
            this.btnMoveDown,
            this.btnDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = global::Scada.Forms.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 22);
            this.btnAdd.ToolTipText = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddWithChoice
            // 
            this.btnAddWithChoice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddWithChoice.Image = global::Scada.Forms.Properties.Resources.add;
            this.btnAddWithChoice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddWithChoice.Name = "btnAddWithChoice";
            this.btnAddWithChoice.Size = new System.Drawing.Size(29, 22);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUp.Image = global::Scada.Forms.Properties.Resources.move_up;
            this.btnMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.ToolTipText = "Move Up";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDown.Image = global::Scada.Forms.Properties.Resources.move_down;
            this.btnMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDown.ToolTipText = "Move Down";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::Scada.Forms.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 22);
            this.btnDelete.ToolTipText = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 496);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(734, 45);
            this.pnlBottom.TabIndex = 2;
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(566, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.propertyGrid, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(734, 471);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.cmsTree;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.ilTree;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(287, 465);
            this.treeView.TabIndex = 0;
            this.treeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCollapseExpand);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCollapseExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
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
            this.miCollapseAll.Image = global::Scada.Forms.Properties.Resources.collapse_all;
            this.miCollapseAll.Name = "miCollapseAll";
            this.miCollapseAll.Size = new System.Drawing.Size(136, 22);
            this.miCollapseAll.Text = "Collapse All";
            this.miCollapseAll.Click += new System.EventHandler(this.miCollapseAll_Click);
            // 
            // ilTree
            // 
            this.ilTree.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilTree.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTree.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(296, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(435, 465);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // FrmModuleConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(734, 541);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.toolStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmModuleConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmModuleConfig_FormClosing);
            this.Load += new System.EventHandler(this.FrmModuleConfig_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.cmsTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripDropDownButton btnAddWithChoice;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ImageList ilTree;
        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ToolStripMenuItem miCollapseAll;
    }
}