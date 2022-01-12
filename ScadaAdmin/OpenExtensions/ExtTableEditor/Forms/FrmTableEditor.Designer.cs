namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    partial class FrmTableEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.lblHint = new System.Windows.Forms.Label();
            this.splVert = new System.Windows.Forms.Splitter();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.colCnlNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAutoText = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHidden = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnRefreshBase = new System.Windows.Forms.ToolStripButton();
            this.btnAddItem = new System.Windows.Forms.ToolStripButton();
            this.btnAddEmptyItem = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUpItem = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDownItem = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.treeView);
            this.pnlLeft.Controls.Add(this.lblHint);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 25);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(300, 436);
            this.pnlLeft.TabIndex = 1;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(300, 376);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeCollapse);
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            // 
            // lblHint
            // 
            this.lblHint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblHint.Location = new System.Drawing.Point(0, 376);
            this.lblHint.Name = "lblHint";
            this.lblHint.Padding = new System.Windows.Forms.Padding(3);
            this.lblHint.Size = new System.Drawing.Size(300, 60);
            this.lblHint.TabIndex = 1;
            this.lblHint.Text = "Press Enter or double-click a node to add it to the table. Right-click a device n" +
    "ode to display the context menu.";
            // 
            // splVert
            // 
            this.splVert.Location = new System.Drawing.Point(300, 25);
            this.splVert.MinExtra = 100;
            this.splVert.MinSize = 100;
            this.splVert.Name = "splVert";
            this.splVert.Size = new System.Drawing.Size(3, 436);
            this.splVert.TabIndex = 2;
            this.splVert.TabStop = false;
            // 
            // dataGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCnlNum,
            this.colDeviceNum,
            this.colAutoText,
            this.colText,
            this.colHidden});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.DataSource = this.bindingSource;
            this.dataGridView.Location = new System.Drawing.Point(303, 25);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(431, 436);
            this.dataGridView.TabIndex = 3;
            this.dataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView_CellValidating);
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView_RowsRemoved);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // colCnlNum
            // 
            this.colCnlNum.DataPropertyName = "CnlNum";
            this.colCnlNum.HeaderText = "Channel";
            this.colCnlNum.Name = "colCnlNum";
            this.colCnlNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDeviceNum
            // 
            this.colDeviceNum.DataPropertyName = "DeviceNum";
            this.colDeviceNum.HeaderText = "Device";
            this.colDeviceNum.Name = "colDeviceNum";
            this.colDeviceNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colAutoText
            // 
            this.colAutoText.DataPropertyName = "AutoText";
            this.colAutoText.HeaderText = "Auto Text";
            this.colAutoText.Name = "colAutoText";
            // 
            // colText
            // 
            this.colText.DataPropertyName = "Text";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colText.DefaultCellStyle = dataGridViewCellStyle2;
            this.colText.HeaderText = "Text";
            this.colText.Name = "colText";
            this.colText.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colText.Width = 300;
            // 
            // colHidden
            // 
            this.colHidden.DataPropertyName = "Hidden";
            this.colHidden.HeaderText = "Hidden";
            this.colHidden.Name = "colHidden";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshBase,
            this.btnAddItem,
            this.btnAddEmptyItem,
            this.btnMoveUpItem,
            this.btnMoveDownItem,
            this.btnDeleteItem});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(734, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnRefreshBase
            // 
            this.btnRefreshBase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefreshBase.Image = global::Scada.Admin.Extensions.ExtTableEditor.Properties.Resources.refresh;
            this.btnRefreshBase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshBase.Name = "btnRefreshBase";
            this.btnRefreshBase.Size = new System.Drawing.Size(23, 22);
            this.btnRefreshBase.ToolTipText = "Refresh Configuration Database";
            this.btnRefreshBase.Click += new System.EventHandler(this.btnRefreshBase_Click);
            // 
            // btnAddItem
            // 
            this.btnAddItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddItem.Image = global::Scada.Admin.Extensions.ExtTableEditor.Properties.Resources.add;
            this.btnAddItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(23, 22);
            this.btnAddItem.ToolTipText = "Add Item";
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // btnAddEmptyItem
            // 
            this.btnAddEmptyItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddEmptyItem.Image = global::Scada.Admin.Extensions.ExtTableEditor.Properties.Resources.add_empty;
            this.btnAddEmptyItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddEmptyItem.Name = "btnAddEmptyItem";
            this.btnAddEmptyItem.Size = new System.Drawing.Size(23, 22);
            this.btnAddEmptyItem.ToolTipText = "Add Empty Item";
            this.btnAddEmptyItem.Click += new System.EventHandler(this.btnAddEmptyItem_Click);
            // 
            // btnMoveUpItem
            // 
            this.btnMoveUpItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUpItem.Image = global::Scada.Admin.Extensions.ExtTableEditor.Properties.Resources.move_up;
            this.btnMoveUpItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUpItem.Name = "btnMoveUpItem";
            this.btnMoveUpItem.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUpItem.ToolTipText = "Move Item Up";
            this.btnMoveUpItem.Click += new System.EventHandler(this.btnMoveUpItem_Click);
            // 
            // btnMoveDownItem
            // 
            this.btnMoveDownItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDownItem.Image = global::Scada.Admin.Extensions.ExtTableEditor.Properties.Resources.move_down;
            this.btnMoveDownItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDownItem.Name = "btnMoveDownItem";
            this.btnMoveDownItem.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDownItem.ToolTipText = "Move Item Down";
            this.btnMoveDownItem.Click += new System.EventHandler(this.btnMoveDownItem_Click);
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteItem.Image = global::Scada.Admin.Extensions.ExtTableEditor.Properties.Resources.delete;
            this.btnDeleteItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteItem.ToolTipText = "Delete Selected Items";
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // FrmTableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.splVert);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.toolStrip);
            this.Name = "FrmTableEditor";
            this.Text = "Table Editor";
            this.Load += new System.EventHandler(this.FrmTableEditor_Load);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel pnlLeft;
        private Splitter splVert;
        private DataGridView dataGridView;
        private TreeView treeView;
        private Label lblHint;
        private ToolStrip toolStrip;
        private ToolStripButton btnRefreshBase;
        private ToolStripButton btnAddItem;
        private ToolStripButton btnAddEmptyItem;
        private ToolStripButton btnMoveUpItem;
        private ToolStripButton btnMoveDownItem;
        private ToolStripButton btnDeleteItem;
        private DataGridViewTextBoxColumn colCnlNum;
        private DataGridViewTextBoxColumn colDeviceNum;
        private DataGridViewCheckBoxColumn colAutoText;
        private DataGridViewTextBoxColumn colText;
        private DataGridViewCheckBoxColumn colHidden;
        private BindingSource bindingSource;
    }
}