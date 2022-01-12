// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Web.Plugins.PlgMain;
using WinControl;

namespace Scada.Admin.Extensions.ExtTableEditor.Forms
{
    /// <summary>
    /// Represents a form for editing a table view.
    /// <para>Представляет форму для редактирования табличного представления.</para>
    /// </summary>
    public partial class FrmTableEditor : Form, IChildForm
    {
        private readonly IAdminContext adminContext; // the Administrator context
        private readonly TableView tableView;        // the table view being edited
        private string fileName;                     // the full name of the edited file


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmTableEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmTableEditor(IAdminContext adminContext, string fileName)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            tableView = new TableView(new Data.Entities.View());
            Text = Path.GetFileName(fileName);
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }


        /// <summary>
        /// Loads the table view.
        /// </summary>
        private void LoadTableView()
        {
            if (!tableView.LoadFromFile(fileName, out string errMsg))
                adminContext.ErrLog.HandleError(errMsg);
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
            FormTranslator.Translate(this, GetType().FullName);
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
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

        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAddEmptyItem_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveUpItem_Click(object sender, EventArgs e)
        {

        }

        private void btnMoveDownItem_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {

        }


        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }


        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
