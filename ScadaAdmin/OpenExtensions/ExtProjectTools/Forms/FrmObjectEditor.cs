// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Extensions.ExtProjectTools.Properties;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using System.Windows.Forms;
using WinControls;

namespace Scada.Admin.Extensions.ExtProjectTools.Forms
{
    /// <summary>
    /// Represents an object editor form.
    /// <para>Представляет форму редактора проектов.</para>
    /// </summary>
    public partial class FrmObjectEditor : Form, IChildForm
    {
        private readonly IAdminContext adminContext;    // the application context
        private readonly ConfigDatabase configDatabase; // the configuration database
        private readonly BaseTable<Obj> objTable;       // the object table

        private ITableIndex parentObjIndex; // the index for searching objects by parent
        private TreeNode selectedNode;      // the currently selected tree node
        private Obj selectedObj;            // the currently selected object
        private bool changing;              // controls are being changed programmatically


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
            objTable = configDatabase.ObjTable;

            parentObjIndex = null;
            selectedNode = null;
            selectedObj = null;
            changing = false;

            ChildFormTag = new ChildFormTag(new ChildFormOptions { Image = Resources.obj });
            ChildFormTag.MessageToChildForm += ChildFormTag_MessageToChildForm;
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
        /// Shows objects in the editor.
        /// </summary>
        private void ShowData()
        {
            parentObjIndex = objTable.GetIndex("ParentObjNum", true);
            FillParentObjects();
            FillTreeView();
            ChildFormTag.Modified = objTable.Modified;
        }

        /// <summary>
        /// Fills the combo box with the parent objects.
        /// </summary>
        private void FillParentObjects()
        {
            Obj emptyObj = new() { ObjNum = 0, Name = " " };
            List<Obj> objs = new(objTable.ItemCount + 1) { emptyObj };
            objs.AddRange(objTable.Enumerate().OrderBy(o => o.Name));

            changing = true;
            cbParentObj.ValueMember = "ObjNum";
            cbParentObj.DisplayMember = "Name";
            cbParentObj.DataSource = objs;
            cbParentObj.SelectedValue = 0;
            changing = false;
        }

        /// <summary>
        /// Refreshes the combo box containing parent objects.
        /// </summary>
        private void RefreshParentObjects()
        {
            try
            {
                cbParentObj.SelectedIndexChanged -= cbParentObj_SelectedIndexChanged;
                cbParentObj.BeginUpdate();

                object dataSource = cbParentObj.DataSource;
                cbParentObj.DataSource = null;

                cbParentObj.ValueMember = "ObjNum";
                cbParentObj.DisplayMember = "Name";
                cbParentObj.DataSource = dataSource;
            }
            finally
            {
                cbParentObj.EndUpdate();
                cbParentObj.SelectedIndexChanged += cbParentObj_SelectedIndexChanged;
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

                if (objTable.ItemCount > 0)
                {
                    AddChildObjects(0, tvObj.Nodes);

                    if (tvObj.Nodes.Count > 0)
                        tvObj.SelectedNode = tvObj.Nodes[0];
                }
            }
            finally
            {
                tvObj.EndUpdate();
            }
        }

        /// <summary>
        /// Adds objects to the tree view recursively.
        /// </summary>
        private void AddChildObjects(int parentObjNum, TreeNodeCollection nodes)
        {
            foreach (Obj childObj in parentObjIndex.SelectItems(parentObjNum))
            {
                if (childObj.ObjNum <= 0)
                    continue; // protect from infinite loop

                TreeNode childNode = CreateObjNode(childObj);
                nodes.Add(childNode);
                AddChildObjects(childObj.ObjNum, childNode.Nodes);
            }
        }

        /// <summary>
        /// Shows the properties of the selected object.
        /// </summary>
        private void ShowObjectProperties()
        {
            changing = true;

            if (selectedObj == null)
            {
                gbObject.Enabled = false;
                numObjNum.Value = numObjNum.Minimum;
                txtName.Text = "";
                txtCode.Text = "";
                cbParentObj.SelectedIndex = -1;
                txtDescr.Text = "";
            }
            else
            {
                gbObject.Enabled = true;
                numObjNum.SetValue(selectedObj.ObjNum);
                txtName.Text = selectedObj.Name;
                txtCode.Text = selectedObj.Code;
                cbParentObj.SelectedValue = selectedObj.ParentObjNum ?? 0;
                txtDescr.Text = selectedObj.Descr;
            }

            changing = false;
        }

        /// <summary>
        /// Validates the new object number before update.
        /// </summary>
        private bool ValidateObjNum(int oldObjNum, int newObjNum, out string errMsg)
        {
            if (objTable.PkExists(newObjNum))
            {
                errMsg = ExtensionPhrases.ObjNumInUse;
                return false;
            }

            if (objTable.KeyIsReferenced(oldObjNum, true, out string tableTitle))
            {
                errMsg = string.Format(ExtensionPhrases.ObjNumReferenced, tableTitle);
                return false;
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Validates the new parent object before update.
        /// </summary>
        private bool ValidateParentObj(int objNum, int parentObjNum, out string errMsg)
        {
            if (!objTable.PkExists(parentObjNum))
            {
                errMsg = ExtensionPhrases.ParentObjectNotExists;
                return false;
            }

            if (objNum == parentObjNum ||
                ObjectsAreRelatives(objNum, parentObjNum))
            {
                errMsg = ExtensionPhrases.InvalidParentObject;
                return false;
            }

            errMsg = "";
            return true;
        }

        /// <summary>
        /// Checks whether the specified objects are a parent and child object.
        /// </summary>
        private bool ObjectsAreRelatives(int parentObjNum, int childObjNum)
        {
            Obj obj = objTable.GetItem(childObjNum);

            while (obj != null)
            {
                if (obj.ParentObjNum == null)
                    return false;

                if (obj.ParentObjNum.Value == parentObjNum)
                    return true;

                obj = objTable.GetItem(obj.ParentObjNum.Value);
            }

            return false;
        }

        /// <summary>
        /// Updates the object number.
        /// </summary>
        private void UpdateObjNum(Obj sourceObj, int oldObjNum, int newObjNum)
        {
            try
            {
                // add new object
                Obj newObj = sourceObj.DeepClone();
                newObj.ObjNum = newObjNum;
                objTable.AddItem(newObj);

                // update object references
                foreach (Obj childObj in parentObjIndex.SelectItems(oldObjNum).Cast<Obj>().ToList())
                {
                    Obj newChildObj = childObj.DeepClone();
                    newChildObj.ParentObjNum = newObjNum;
                    objTable.AddItem(newChildObj); // add with index update
                }

                // remove old object
                objTable.RemoveItem(oldObjNum);
                MarkDataAsModified();

                // update list of parent objects
                FillParentObjects();

                // update tree view
                TreeNode parentNode = selectedNode.Parent;
                TreeNodeCollection siblingNodes = parentNode == null ? tvObj.Nodes : parentNode.Nodes;

                try
                {
                    tvObj.BeginUpdate();
                    siblingNodes.Clear();
                    AddChildObjects(newObj.ParentObjNum ?? 0, siblingNodes);
                    SelectNode(newObj.ObjNum, siblingNodes);
                }
                finally
                {
                    tvObj.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                adminContext.ErrLog.HandleError(ex, ExtensionPhrases.UpdateObjNumError);
            }
        }

        /// <summary>
        /// Updates the parent object number.
        /// </summary>
        private void UpdateParentObj(Obj sourceObj, int newParentObjNum)
        {
            try
            {
                // update object
                Obj newObj = sourceObj.DeepClone();
                newObj.ParentObjNum = newParentObjNum > 0 ? newParentObjNum : null;
                objTable.AddItem(newObj); // add with index update
                MarkDataAsModified();

                // update tree view
                try
                {
                    tvObj.BeginUpdate();
                    selectedNode.Remove();

                    if (newParentObjNum > 0)
                    {
                        TreeNode parentNode = tvObj.Nodes.IterateNodes()
                            .Where(node => ((Obj)node.Tag).ObjNum == newParentObjNum).FirstOrDefault();

                        if (parentNode != null)
                        {
                            parentNode.Nodes.Clear();
                            AddChildObjects(newParentObjNum, parentNode.Nodes);
                            SelectNode(newObj.ObjNum, parentNode.Nodes);
                        }
                    }
                    else
                    {
                        tvObj.Nodes.Clear();
                        AddChildObjects(0, tvObj.Nodes);
                        SelectNode(newObj.ObjNum, tvObj.Nodes);
                    }
                }
                finally
                {
                    tvObj.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                adminContext.ErrLog.HandleError(ex, ExtensionPhrases.UpdateParentObjError);
            }
        }

        /// <summary>
        /// Selects a node corresponding to the specified object among the specified nodes.
        /// </summary>
        private void SelectNode(int objNum, TreeNodeCollection nodes)
        {
            foreach (TreeNode childNode in nodes)
            {
                if (((Obj)childNode.Tag).ObjNum == objNum)
                {
                    tvObj.SelectedNode = childNode;
                    break;
                }
            }
        }

        /// <summary>
        /// Recursively gets the hierarchy of objects starting from the specified object.
        /// </summary>
        private List<Obj> GetObjectHierarchy(Obj startObj)
        {
            List<Obj> objs = [startObj];

            void AddChildren(Obj parentObj)
            {
                foreach (Obj childObj in parentObjIndex.SelectItems(parentObj.ObjNum))
                {
                    objs.Add(childObj);
                    AddChildren(childObj);
                }
            }

            AddChildren(startObj);
            return objs;
        }

        /// <summary>
        /// Sets the data change flag.
        /// </summary>
        private void MarkDataAsModified()
        {
            objTable.Modified = true;
            ChildFormTag.Modified = true;
        }

        /// <summary>
        /// Creates a tree node that represents the specified object.
        /// </summary>
        private static TreeNode CreateObjNode(Obj obj)
        {
            return TreeViewExtensions.CreateNode(GetNodeText(obj), "obj.png", obj);
        }

        /// <summary>
        /// Gets the text for an object tree node.
        /// </summary>
        private static string GetNodeText(Obj obj)
        {
            return string.Format(CommonPhrases.EntityCaption, obj.ObjNum, obj.Name);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void Save()
        {
            if (configDatabase.SaveTable(objTable, out string errMsg))
                ChildFormTag.Modified = false;
            else
                adminContext.ErrLog.HandleError(errMsg);
        }


        private void FrmObjectEditor_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ContextMenus = [cmsTree] });
            AddTreeViewImages();
            ShowData();
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

        private void ChildFormTag_MessageToChildForm(object sender, FormMessageEventArgs e)
        {
            if (e.Message == AdminMessage.BaseReload)
            {
                ShowData();
                ChildFormTag.Modified = false;
            }
        }


        private void btnAddObject_Click(object sender, EventArgs e)
        {
            // determine new object number
            int objNum;
            TreeNode parentNode = selectedNode?.Parent;
            Obj parentObj = parentNode == null ? null : (Obj)parentNode.Tag;

            if (parentObj == null)
            {
                objNum = objTable.GetNextPk();
            }
            else
            {
                objNum = ((Obj)parentNode.LastNode.Tag).ObjNum + 1;

                if (objTable.PkExists(objNum))
                    objNum = objTable.GetNextPk();
            }

            // create object
            Obj obj = new()
            {
                ObjNum = objNum,
                Name = "",
                Code = "",
                ParentObjNum = parentObj?.ObjNum,
                Descr = ""
            };

            objTable.AddItem(obj);

            // show created object
            FillParentObjects();
            TreeNode objNode = CreateObjNode(obj);
            (parentNode == null ? tvObj.Nodes : parentNode.Nodes).Add(objNode);
            tvObj.SelectedNode = objNode;
            txtName.Focus();
        }

        private void btnDeleteObject_Click(object sender, EventArgs e)
        {
            if (selectedObj != null && selectedNode != null)
            {
                if (MessageBox.Show(ExtensionPhrases.DeleteObjectConfirm, CommonPhrases.QuestionCaption,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                List<Obj> objectsToDelete = GetObjectHierarchy(selectedObj);

                if (objectsToDelete.Any(obj => objTable.KeyIsReferenced(obj.ObjNum, true, out _)))
                {
                    ScadaUiUtils.ShowError(ExtensionPhrases.ObjectReferenced);
                }
                else
                {
                    objectsToDelete.ForEach(objTable.RemoveItem);
                    MarkDataAsModified();

                    FillParentObjects();
                    selectedNode.Remove();
                }
            }
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            ShowData();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }


        private void numObjNum_ValueChanged(object sender, EventArgs e)
        {
            if (!changing && selectedObj != null && selectedNode != null)
            {
                int oldObjNum = selectedObj.ObjNum;
                int newObjNum = Convert.ToInt32(numObjNum.Value);

                if (ValidateObjNum(oldObjNum, newObjNum, out string errMsg))
                {
                    UpdateObjNum(selectedObj, oldObjNum, newObjNum);
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                    numObjNum.ValueChanged -= numObjNum_ValueChanged;
                    numObjNum.SetValue(oldObjNum);
                    numObjNum.ValueChanged += numObjNum_ValueChanged;
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selectedObj != null && selectedNode != null)
            {
                selectedObj.Name = txtName.Text;
                selectedNode.Text = GetNodeText(selectedObj);
                MarkDataAsModified();
                RefreshParentObjects();
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selectedObj != null)
            {
                selectedObj.Code = txtCode.Text;
                MarkDataAsModified();
            }
        }

        private void cbParentObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!changing && selectedObj != null && selectedNode != null)
            {
                int oldParentObjNum = selectedObj.ParentObjNum ?? 0;
                int newParentObjNum = (int)cbParentObj.SelectedValue;

                if (ValidateParentObj(selectedObj.ObjNum, newParentObjNum, out string errMsg))
                {
                    UpdateParentObj(selectedObj, newParentObjNum);
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                    cbParentObj.SelectedIndexChanged -= cbParentObj_SelectedIndexChanged;
                    cbParentObj.SelectedValue = oldParentObjNum;
                    cbParentObj.SelectedIndexChanged += cbParentObj_SelectedIndexChanged;
                }
            }
        }

        private void txtDescr_TextChanged(object sender, EventArgs e)
        {
            if (!changing && selectedObj != null)
            {
                selectedObj.Descr = txtDescr.Text;
                MarkDataAsModified();
            }
        }


        private void tvObj_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // select a tree node on right click
            if (e.Button == MouseButtons.Right && e.Node != null)
                tvObj.SelectedNode = e.Node;
        }

        private void tvObj_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node;
            selectedObj = selectedNode.Tag as Obj;
            btnDeleteObject.Enabled = selectedObj != null;
            ShowObjectProperties();
        }

        private void cmsTree_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            miOpenChannels.Enabled = selectedObj != null;
        }

        private void miCollapseAll_Click(object sender, EventArgs e)
        {
            if (tvObj.Nodes.Count > 0)
            {
                tvObj.SelectedNode = null;
                tvObj.CollapseAll();
                tvObj.SelectedNode = tvObj.Nodes[0];
            }
        }

        private void miOpenChannels_Click(object sender, EventArgs e)
        {
            if (selectedObj != null)
            {
                adminContext.MainForm.OpenBaseTable(
                    typeof(Cnl),
                    new TableFilter("ObjNum", selectedObj.ObjNum)
                    {
                        Title = string.Format(ExtensionPhrases.ObjectFilter, selectedObj.ObjNum)
                    });
            }
        }
    }
}
