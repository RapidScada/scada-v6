// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtTableEditor.Properties;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using Scada.Web.Plugins.PlgMain;
using System.ComponentModel;
using WinControl;

namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    /// <summary>
    /// Represents a form for editing a table view.
    /// <para>Представляет форму для редактирования табличного представления.</para>
    /// </summary>
    public partial class FrmTableEditor : Form, IChildForm
    {
        private readonly IAdminContext adminContext;    // the Administrator context
        private readonly ConfigDatabase configDatabase; // the configuration database of the current project
        private readonly TableView tableView;           // the table view being edited
        private string fileName;                        // the full name of the edited file
        private bool preventNodeExpand;                 // prevent a tree node from expanding or collapsing


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableEditor()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableEditor(IAdminContext adminContext, string fileName)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            configDatabase = adminContext.CurrentProject?.ConfigDatabase ?? 
                throw new ScadaException("Configuration database must not be null.");
            tableView = new TableView(new Data.Entities.View());
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            preventNodeExpand = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Takes the tree view images and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilTree.Images.Add("device.png", Resources.device);
            ilTree.Images.Add("empty.png", Resources.empty);
            ilTree.Images.Add("cnl.png", Resources.cnl);
        }

        /// <summary>
        /// Fills the tree view.
        /// </summary>
        private void FillTreeView()
        {
            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                foreach (Device device in configDatabase.DeviceTable.EnumerateItems())
                {
                    string nodeText = string.Format(CommonPhrases.EntityCaption, device.DeviceNum, device.Name);
                    TreeNode deviceNode = TreeViewExtensions.CreateNode(nodeText, "device.png");
                    deviceNode.ContextMenuStrip = cmsDevice;
                    deviceNode.Tag = device;
                    deviceNode.Nodes.Add(TreeViewExtensions.CreateNode(CommonPhrases.EmptyData, "empty.png"));
                    treeView.Nodes.Add(deviceNode);
                }

                TreeNode emptyDeviceNode = TreeViewExtensions.CreateNode(AdminPhrases.EmptyDevice, "device.png");
                emptyDeviceNode.ContextMenuStrip = cmsDevice;
                emptyDeviceNode.Tag = new Device { DeviceNum = 0, Name = AdminPhrases.EmptyDevice };
                emptyDeviceNode.Nodes.Add(TreeViewExtensions.CreateNode(CommonPhrases.EmptyData, "empty.png"));
                treeView.Nodes.Add(emptyDeviceNode);
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the device node by channel nodes.
        /// </summary>
        private void FillDeviceNode(TreeNode deviceNode, Device device)
        {
            try
            {
                treeView.BeginUpdate();
                deviceNode.Nodes.Clear();

                foreach (Cnl cnl in configDatabase.CnlTable.SelectItems(
                    new TableFilter("DeviceNum", device.DeviceNum), true))
                {
                    string nodeText = string.Format(CommonPhrases.EntityCaption, cnl.CnlNum, cnl.Name);
                    TreeNode cnlNode = TreeViewExtensions.CreateNode(nodeText, "cnl.png");
                    cnlNode.Tag = cnl;
                    deviceNode.Nodes.Add(cnlNode);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the device node if it hasn't been filled yet.
        /// </summary>
        private void FillDeviceNodeIfNeeded(TreeNode deviceNode)
        {
            if (deviceNode.Tag is Device device && deviceNode.Nodes.Count > 0 && deviceNode.Nodes[0].Tag == null)
                FillDeviceNode(deviceNode, device);
        }

        /// <summary>
        /// Loads the table view.
        /// </summary>
        private void LoadTableView()
        {
            if (tableView.LoadFromFile(fileName, out string errMsg))
            {
                SetTableItemAutoText();
                bindingSource.DataSource = tableView.Items;
                ChildFormTag.Modified = false;
                SetButtonsEnabled();
            }
            else
            {
                adminContext.ErrLog.HandleError(errMsg);
            }
        }

        /// <summary>
        /// Sets the text of the table view items automatically based on the configuration database.
        /// </summary>
        private void SetTableItemAutoText()
        {
            foreach (TableItem item in tableView.Items)
            {
                if (item.AutoText)
                {
                    if (item.CnlNum > 0)
                    {
                        if (configDatabase.CnlTable.GetItem(item.CnlNum) is Cnl cnl)
                            item.Text = cnl.Name;
                    }
                    else if (item.DeviceNum > 0)
                    {
                        if (configDatabase.DeviceTable.GetItem(item.DeviceNum) is Device device)
                            item.Text = device.Name;
                    }
                }
            }
        }

        /// <summary>
        /// Enables or disables the tool buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            int selRowCnt = dataGridView.SelectedRows.Count;
            int selRowInd = -1;

            if (selRowCnt > 0)
            {
                selRowInd = dataGridView.SelectedRows[0].Index;
            }
            else if (dataGridView.CurrentRow != null)
            {
                selRowCnt = 1;
                selRowInd = dataGridView.CurrentRow.Index;
            }

            btnAddItem.Enabled = treeView.SelectedNode != null;
            btnMoveUpItem.Enabled = selRowCnt == 1 && selRowInd > 0;
            btnMoveDownItem.Enabled = selRowCnt == 1 && selRowInd < dataGridView.Rows.Count - 1;
            btnDeleteItem.Enabled = selRowCnt > 0;
        }

        /// <summary>
        /// Validates a cell after editing.
        /// </summary>
        private bool ValidateCell(int colInd, int rowInd, string cellVal, out string errMsg)
        {
            errMsg = "";

            if (0 <= colInd && colInd < dataGridView.ColumnCount &&
                0 <= rowInd && rowInd < dataGridView.RowCount &&
                dataGridView.Columns[colInd].ValueType == typeof(int))
            {
                if (string.IsNullOrWhiteSpace(cellVal) && dataGridView.EditingControl is TextBox textBox)
                {
                    textBox.Text = "0"; // replace empty value to 0
                }
                else if (!(int.TryParse(cellVal, out int intVal) && 0 <= intVal && intVal <= ushort.MaxValue))
                {
                    errMsg = string.Format(CommonPhrases.IntegerInRangeRequired, 0, ConfigDatabase.MaxID);
                }
            }

            return string.IsNullOrEmpty(errMsg);
        }

        /// <summary>
        /// Gets the selected row.
        /// </summary>
        private bool GetSelectedRow(out DataGridViewRow row)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                row = dataGridView.SelectedRows[0];
                return true;
            }
            else if (dataGridView.CurrentRow != null)
            {
                row = dataGridView.CurrentRow;
                return true;
            }
            else
            {
                row = null;
                return false;
            }
        }

        /// <summary>
        /// Selects the table item with the specified index.
        /// </summary>
        private void SelectTableItem(int rowInd, bool setFocus = true)
        {
            dataGridView.ClearSelection();
            int rowCnt = dataGridView.Rows.Count;

            if (rowInd >= 0 && rowCnt > 0)
            {
                if (rowInd >= rowCnt)
                    rowInd = rowCnt - 1;

                dataGridView.Rows[rowInd].Selected = true;
                dataGridView.CurrentCell = dataGridView.Rows[rowInd].Cells[0];
            }

            if (setFocus)
                dataGridView.Select();
        }

        /// <summary>
        /// Inserts the item in the table.
        /// </summary>
        private void InsertTableItem(TableItem item)
        {
            // get insert index
            int newInd;

            if (dataGridView.SelectedRows.Count > 0)
            {
                newInd = -1;

                foreach (DataGridViewRow row in dataGridView.SelectedRows)
                {
                    if (newInd < row.Index)
                        newInd = row.Index;
                }

                newInd++;
            }
            else if (dataGridView.CurrentRow != null)
            {
                newInd = dataGridView.CurrentRow.Index + 1;
            }
            else
            {
                newInd = dataGridView.Rows.Count;
            }

            // insert item
            tableView.Items.Insert(newInd, item);
            bindingSource.ResetBindings(false);
            SelectTableItem(newInd, !treeView.Focused);
            ChildFormTag.Modified = true;
        }

        /// <summary>
        /// Creates and inserts an item in the table.
        /// </summary>
        private void InsertTableItem(TreeNode node)
        {
            if (node == null)
                return;

            if (node.Tag is Device device)
            {
                if (device.DeviceNum > 0)
                {
                    InsertTableItem(new TableItem
                    {
                        DeviceNum = device.DeviceNum,
                        Text = device.Name,
                        AutoText = true
                    });
                }
            }
            else if (node.Tag is Cnl cnl)
            {
                InsertTableItem(new TableItem 
                { 
                    CnlNum = cnl.CnlNum, 
                    Text = cnl.Name, 
                    AutoText = true 
                });
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            if (tableView.SaveToFile(fileName, out string errMsg))
                ChildFormTag.Modified = false;
            else
                adminContext.ErrLog.HandleError(errMsg);
        }


        private void FrmTableEditor_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, 
                new FormTranslatorOptions { ContextMenus = new ContextMenuStrip[] { cmsDevice } });
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
            Text = Path.GetFileName(fileName);

            TakeTreeViewImages();
            FillTreeView();
            LoadTableView();
        }

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            // update file name in case of renaming a file or its parent directory
            if (e.Message == AdminMessage.UpdateFileName &&
                e.GetArgument("FileName") is string newFileName && newFileName != "")
            {
                fileName = newFileName;
                Text = Path.GetFileName(fileName);
            }
        }

        private void btnRefreshBase_Click(object sender, EventArgs e)
        {
            FillTreeView();
            SetTableItemAutoText();
            bindingSource.ResetBindings(false);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            InsertTableItem(treeView.SelectedNode);
        }

        private void btnAddEmptyItem_Click(object sender, EventArgs e)
        {
            InsertTableItem(new TableItem());
        }

        private void btnMoveUpDownItem_Click(object sender, EventArgs e)
        {
            // move selected table item up or down
            if (GetSelectedRow(out DataGridViewRow row))
            {
                int curInd = row.Index;
                List<TableItem> items = tableView.Items;

                if (sender == btnMoveUpItem && curInd > 0 ||
                    sender == btnMoveDownItem && curInd < items.Count - 1)
                {
                    int newInd = sender == btnMoveUpItem ? curInd - 1 : curInd + 1;
                    TableItem item = items[curInd];
                    items.RemoveAt(curInd);
                    items.Insert(newInd, item);

                    bindingSource.ResetBindings(false);
                    SelectTableItem(newInd);
                    ChildFormTag.Modified = true;
                }
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            // delete selected table items
            DataGridViewSelectedRowCollection selRows = dataGridView.SelectedRows;
            int selRowCnt = selRows.Count;
            int selRowInd = -1;

            if (selRowCnt > 0)
            {
                int[] selRowInds = new int[selRowCnt];

                for (int i = 0; i < selRowCnt; i++)
                {
                    selRowInds[i] = selRows[i].Index;
                }

                Array.Sort(selRowInds);
                selRowInd = selRowInds[selRowCnt - 1] - selRowCnt + 1;

                for (int i = selRowCnt - 1; i >= 0; i--)
                {
                    tableView.Items.RemoveAt(selRowInds[i]);
                }
            }
            else if (dataGridView.CurrentRow != null)
            {
                selRowInd = dataGridView.CurrentRow.Index;
                tableView.Items.RemoveAt(selRowInd);
            }

            bindingSource.ResetBindings(false);
            SelectTableItem(selRowInd);
            ChildFormTag.Modified = true;
        }


        private void cmsDevice_Opening(object sender, CancelEventArgs e)
        {
            if (treeView.SelectedNode?.Tag is Device device)
                miDeviceAddDevice.Enabled = device.DeviceNum > 0;
            else
                e.Cancel = true;
        }

        private void miDeviceAddDevice_Click(object sender, EventArgs e)
        {
            InsertTableItem(treeView.SelectedNode);
        }

        private void miDeviceAddAllChannels_Click(object sender, EventArgs e)
        {
            // insert table items for selected device and related channels
            if (treeView.SelectedNode?.Tag is Device device)
            {
                FillDeviceNodeIfNeeded(treeView.SelectedNode);
                InsertTableItem(new TableItem { Text = device.Name });

                foreach (TreeNode cnlNode in treeView.SelectedNode.Nodes)
                {
                    InsertTableItem(cnlNode);
                }
            }
        }


        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && treeView.SelectedNode is TreeNode node)
            {
                InsertTableItem(node);

                // select next node
                if (node.NextNode != null)
                    treeView.SelectedNode = node.NextNode;
            }
        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            // prevent device node from expanding on double click
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                TreeNode node = treeView.GetNodeAt(e.Location);
                preventNodeExpand = node?.Tag is Device;
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // select tree node on right click
            if (e.Button == MouseButtons.Right && e.Node != null)
                treeView.SelectedNode = e.Node;
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                InsertTableItem(e.Node);
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // prevent node from expanding
            if (preventNodeExpand)
            {
                e.Cancel = true;
                preventNodeExpand = false;
                return;
            }

            // fill node on demand
            FillDeviceNodeIfNeeded(e.Node);
        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            // prevent node from collapsing
            if (preventNodeExpand)
            {
                e.Cancel = true;
                preventNodeExpand = false;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnAddItem.Enabled = treeView.SelectedNode != null;
        }


        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colCnlNum.Index || e.ColumnIndex == colDeviceNum.Index)
            {
                // hide zero entity numbers
                if (e.Value is int intVal && intVal <= 0)
                    e.Value = "";
            }
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView.CurrentCell != null && dataGridView.CurrentCell.IsInEditMode &&
                !ValidateCell(e.ColumnIndex, e.RowIndex, e.FormattedValue?.ToString(), out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
                e.Cancel = true;
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                ChildFormTag.Modified = true;
        }

        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ChildFormTag.Modified = true;
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            SetButtonsEnabled();
        }
    }
}
