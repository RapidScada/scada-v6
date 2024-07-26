// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using CsvHelper;
using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Extensions.ExtProjectTools.Properties;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using WinControls;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents an object editor form.
    /// <para>Представляет форму редактора проектов.</para>
    /// </summary>
    public partial class FrmObjectEditor : Form, IChildForm
    {
        private readonly IAdminContext adminContext;
        private readonly ConfigDatabase configDatabase;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmObjectEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmObjectEditor(IAdminContext adminContext, ConfigDatabase configDatabase)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.configDatabase = configDatabase ?? throw new ArgumentNullException(nameof(configDatabase));
            ChildFormTag = new ChildFormTag(new ChildFormOptions { Image = Resources.obj });
            IsClosed = false;
        }


        /// <summary>
        /// Gets or sets the object associated with the form.
        /// </summary>
        public ChildFormTag ChildFormTag { get; set; }

        /// <summary>
        /// Gets a value indicating whether the form has been closed.
        /// </summary>
        public bool IsClosed { get; private set; }


        /// <summary>
        /// Adds images to the image list.
        /// </summary>
        private void AddTreeViewImages()
        {
            ilTree.Images.Add("obj.png", Resources.obj);
        }

        /// <summary>
        /// Adds objects to the tree view recursively.
        /// </summary>
        private void AddChildObjects(ITableIndex parentObjIndex, int parentObjNum, TreeNode parentNode)
        {
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                string caption = string.Format(CommonPhrases.EntityCaption, childObj.ObjNum, childObj.Name);
                TreeNode childNode = TreeViewExtensions.CreateNode(caption, "obj.png", childObj);

                if (parentNode == null)
                    tvObj.Nodes.Add(childNode);
                else
                    parentNode.Nodes.Add(childNode);

                AddChildObjects(parentObjIndex, childObj.ObjNum, childNode);
            }
        }

        /// <summary>
        /// Fills the object tree.
        /// </summary>
        private void FillTreeView()
        {
            try
            {
                tvObj.BeginUpdate();
                tvObj.Nodes.Clear();

                if (configDatabase.ObjTable.ItemCount > 0)
                {
                    if (configDatabase.ObjTable.TryGetIndex("ParentObjNum", out ITableIndex parentObjIndex))
                        AddChildObjects(parentObjIndex, 0, null);
                    else
                        throw new ScadaException(CommonPhrases.IndexNotFound);

                    if (tvObj.Nodes.Count > 0)
                        tvObj.SelectedNode = tvObj.Nodes[0];

                    tvObj.CollapseAll();
                }
            }
            finally
            {
                tvObj.EndUpdate();
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {

        }


        private void FrmObjectEditor_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName);
            AddTreeViewImages();
            FillTreeView();
        }

        private void FrmObjectEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsClosed = true;
        }

        private void FrmObjectEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.F:
                        e.Handled = true;
                        break;
                }
            }
        }


        private void btnAddObject_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteObject_Click(object sender, EventArgs e)
        {

        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }


        private void numObjNum_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbParentObj_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
