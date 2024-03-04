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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlLeft = new Panel();
            treeView = new TreeView();
            ilTree = new ImageList(components);
            lblHint = new Label();
            splVert = new Splitter();
            dataGridView = new DataGridView();
            colCnlNum = new DataGridViewTextBoxColumn();
            colDeviceNum = new DataGridViewTextBoxColumn();
            colAutoText = new DataGridViewCheckBoxColumn();
            colText = new DataGridViewTextBoxColumn();
            colHidden = new DataGridViewCheckBoxColumn();
            bindingSource = new BindingSource(components);
            toolStrip = new ToolStrip();
            btnRefreshBase = new ToolStripButton();
            btnAddItem = new ToolStripButton();
            btnAddEmptyItem = new ToolStripButton();
            btnMoveUpItem = new ToolStripButton();
            btnMoveDownItem = new ToolStripButton();
            btnDeleteItem = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnTableOptions = new ToolStripButton();
            cmsDevice = new ContextMenuStrip(components);
            miDeviceAddDevice = new ToolStripMenuItem();
            miDeviceAddAllChannels = new ToolStripMenuItem();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource).BeginInit();
            toolStrip.SuspendLayout();
            cmsDevice.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(treeView);
            pnlLeft.Controls.Add(lblHint);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 25);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(300, 436);
            pnlLeft.TabIndex = 1;
            // 
            // treeView
            // 
            treeView.Dock = DockStyle.Fill;
            treeView.ImageIndex = 0;
            treeView.ImageList = ilTree;
            treeView.Location = new Point(0, 0);
            treeView.Name = "treeView";
            treeView.SelectedImageIndex = 0;
            treeView.Size = new Size(300, 376);
            treeView.TabIndex = 0;
            treeView.BeforeCollapse += treeView_BeforeCollapse;
            treeView.BeforeExpand += treeView_BeforeExpand;
            treeView.AfterSelect += treeView_AfterSelect;
            treeView.NodeMouseClick += treeView_NodeMouseClick;
            treeView.NodeMouseDoubleClick += treeView_NodeMouseDoubleClick;
            treeView.KeyDown += treeView_KeyDown;
            treeView.MouseDown += treeView_MouseDown;
            // 
            // ilTree
            // 
            ilTree.ColorDepth = ColorDepth.Depth32Bit;
            ilTree.ImageSize = new Size(16, 16);
            ilTree.TransparentColor = Color.Transparent;
            // 
            // lblHint
            // 
            lblHint.BorderStyle = BorderStyle.FixedSingle;
            lblHint.Dock = DockStyle.Bottom;
            lblHint.Location = new Point(0, 376);
            lblHint.Name = "lblHint";
            lblHint.Padding = new Padding(3);
            lblHint.Size = new Size(300, 60);
            lblHint.TabIndex = 1;
            lblHint.Text = "Press Enter or double-click a node to add it to the table. Right-click a device node to display the context menu.";
            // 
            // splVert
            // 
            splVert.Location = new Point(300, 25);
            splVert.MinExtra = 100;
            splVert.MinSize = 100;
            splVert.Name = "splVert";
            splVert.Size = new Size(3, 436);
            splVert.TabIndex = 2;
            splVert.TabStop = false;
            // 
            // dataGridView
            // 
            dataGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { colCnlNum, colDeviceNum, colAutoText, colText, colHidden });
            dataGridView.DataSource = bindingSource;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(303, 25);
            dataGridView.Name = "dataGridView";
            dataGridView.ShowCellToolTips = false;
            dataGridView.Size = new Size(431, 436);
            dataGridView.TabIndex = 3;
            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.CellValidating += dataGridView_CellValidating;
            dataGridView.CellValueChanged += dataGridView_CellValueChanged;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            dataGridView.UserDeletedRow += dataGridView_UserDeletedRow;
            // 
            // colCnlNum
            // 
            colCnlNum.DataPropertyName = "CnlNum";
            colCnlNum.HeaderText = "Channel";
            colCnlNum.Name = "colCnlNum";
            colCnlNum.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colDeviceNum
            // 
            colDeviceNum.DataPropertyName = "DeviceNum";
            colDeviceNum.HeaderText = "Device";
            colDeviceNum.Name = "colDeviceNum";
            colDeviceNum.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colAutoText
            // 
            colAutoText.DataPropertyName = "AutoText";
            colAutoText.HeaderText = "Auto Text";
            colAutoText.Name = "colAutoText";
            // 
            // colText
            // 
            colText.DataPropertyName = "Text";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colText.DefaultCellStyle = dataGridViewCellStyle2;
            colText.HeaderText = "Text";
            colText.Name = "colText";
            colText.SortMode = DataGridViewColumnSortMode.NotSortable;
            colText.Width = 300;
            // 
            // colHidden
            // 
            colHidden.DataPropertyName = "Hidden";
            colHidden.HeaderText = "Hidden";
            colHidden.Name = "colHidden";
            // 
            // bindingSource
            // 
            bindingSource.AllowNew = false;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnRefreshBase, btnAddItem, btnAddEmptyItem, btnMoveUpItem, btnMoveDownItem, btnDeleteItem, toolStripSeparator1, btnTableOptions });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(734, 25);
            toolStrip.TabIndex = 0;
            // 
            // btnRefreshBase
            // 
            btnRefreshBase.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnRefreshBase.Image = Properties.Resources.refresh;
            btnRefreshBase.ImageTransparentColor = Color.Magenta;
            btnRefreshBase.Name = "btnRefreshBase";
            btnRefreshBase.Size = new Size(23, 22);
            btnRefreshBase.ToolTipText = "Refresh Configuration Database";
            btnRefreshBase.Click += btnRefreshBase_Click;
            // 
            // btnAddItem
            // 
            btnAddItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddItem.Image = Properties.Resources.add;
            btnAddItem.ImageTransparentColor = Color.Magenta;
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(23, 22);
            btnAddItem.ToolTipText = "Add Item";
            btnAddItem.Click += btnAddItem_Click;
            // 
            // btnAddEmptyItem
            // 
            btnAddEmptyItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnAddEmptyItem.Image = Properties.Resources.add_empty;
            btnAddEmptyItem.ImageTransparentColor = Color.Magenta;
            btnAddEmptyItem.Name = "btnAddEmptyItem";
            btnAddEmptyItem.Size = new Size(23, 22);
            btnAddEmptyItem.ToolTipText = "Add Empty Item";
            btnAddEmptyItem.Click += btnAddEmptyItem_Click;
            // 
            // btnMoveUpItem
            // 
            btnMoveUpItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveUpItem.Image = Properties.Resources.move_up;
            btnMoveUpItem.ImageTransparentColor = Color.Magenta;
            btnMoveUpItem.Name = "btnMoveUpItem";
            btnMoveUpItem.Size = new Size(23, 22);
            btnMoveUpItem.ToolTipText = "Move Item Up";
            btnMoveUpItem.Click += btnMoveUpDownItem_Click;
            // 
            // btnMoveDownItem
            // 
            btnMoveDownItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMoveDownItem.Image = Properties.Resources.move_down;
            btnMoveDownItem.ImageTransparentColor = Color.Magenta;
            btnMoveDownItem.Name = "btnMoveDownItem";
            btnMoveDownItem.Size = new Size(23, 22);
            btnMoveDownItem.ToolTipText = "Move Item Down";
            btnMoveDownItem.Click += btnMoveUpDownItem_Click;
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDeleteItem.Image = Properties.Resources.delete;
            btnDeleteItem.ImageTransparentColor = Color.Magenta;
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(23, 22);
            btnDeleteItem.ToolTipText = "Delete Selected Items";
            btnDeleteItem.Click += btnDeleteItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnTableOptions
            // 
            btnTableOptions.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnTableOptions.Image = Properties.Resources.options;
            btnTableOptions.ImageTransparentColor = Color.Magenta;
            btnTableOptions.Name = "btnTableOptions";
            btnTableOptions.Size = new Size(23, 22);
            btnTableOptions.ToolTipText = "Table View Options";
            btnTableOptions.Click += btnTableOptions_Click;
            // 
            // cmsDevice
            // 
            cmsDevice.Items.AddRange(new ToolStripItem[] { miDeviceAddDevice, miDeviceAddAllChannels });
            cmsDevice.Name = "cmsDevice";
            cmsDevice.Size = new Size(166, 48);
            cmsDevice.Opening += cmsDevice_Opening;
            // 
            // miDeviceAddDevice
            // 
            miDeviceAddDevice.Name = "miDeviceAddDevice";
            miDeviceAddDevice.Size = new Size(165, 22);
            miDeviceAddDevice.Text = "Add Device";
            miDeviceAddDevice.Click += miDeviceAddDevice_Click;
            // 
            // miDeviceAddAllChannels
            // 
            miDeviceAddAllChannels.Name = "miDeviceAddAllChannels";
            miDeviceAddAllChannels.Size = new Size(165, 22);
            miDeviceAddAllChannels.Text = "Add All Channels";
            miDeviceAddAllChannels.Click += miDeviceAddAllChannels_Click;
            // 
            // FrmTableEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 461);
            Controls.Add(dataGridView);
            Controls.Add(splVert);
            Controls.Add(pnlLeft);
            Controls.Add(toolStrip);
            Name = "FrmTableEditor";
            Text = "Table Editor";
            Load += FrmTableEditor_Load;
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource).EndInit();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            cmsDevice.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private ContextMenuStrip cmsDevice;
        private ImageList ilTree;
        private ToolStripMenuItem miDeviceAddDevice;
        private ToolStripMenuItem miDeviceAddAllChannels;
        private ToolStripButton btnTableOptions;
        private ToolStripSeparator toolStripSeparator1;
    }
}