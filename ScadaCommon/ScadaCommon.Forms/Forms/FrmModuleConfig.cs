// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Scada.Forms.Forms
{
    /// <summary>
    /// Represents a universal form for editing module configuration.
    /// <para>Представляет универсальную форму для редактирования конфигурации модуля.</para>
    /// </summary>
    public partial class FrmModuleConfig : Form
    {
        private readonly ConfigProvider configProvider; // provides access to the module configuration
        private bool modified;                          // indicates that the module configuration is modified


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmModuleConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmModuleConfig(ConfigProvider configProvider)
            : this()
        {
            this.configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            modified = false;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the module configuration is modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
                btnSave.Enabled = modified;
                btnCancel.Enabled = modified;
            }
        }


        /// <summary>
        /// Adds buttons to the toolbar.
        /// </summary>
        private void AddToolButtons()
        {
            btnAdd.Visible = false;
            btnAddWithChoice.Visible = false;
            ToolStripItem[] buttons = configProvider.GetAddButtons();

            if (buttons != null)
            {
                if (buttons.Length > 1)
                {
                    btnAddWithChoice.Visible = true;
                    btnAddWithChoice.DropDownItems.AddRange(buttons);
                }
                else if (buttons.Length > 0)
                {
                    btnAdd.Visible = true;
                    btnAdd.ToolTipText = buttons[0].Text;
                }

                foreach (ToolStripItem button in buttons)
                {
                    button.Click += btnAdd_Click;
                }
            }
        }

        /// <summary>
        /// Takes the tree view and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            Dictionary<string, Image> images = configProvider.GetTreeViewImages();

            if (images != null)
            {
                foreach (KeyValuePair<string, Image> pair in images)
                {
                    ilTree.Images.Add(pair.Key, pair.Value);
                }
            }
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
                TreeNode[] treeNodes = configProvider.GetTreeNodes();

                if (treeNodes != null)
                {
                    foreach (TreeNode treeNode in treeNodes)
                    {
                        treeView.Nodes.Add(treeNode);
                    }
                }

                if (treeView.Nodes.Count > 0)
                    treeView.SelectedNode = treeView.Nodes[0];
                else
                    SetButtonsEnabled();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Enables or disables the toolbar buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            TreeNode selectedNode = treeView.SelectedNode;
            btnAdd.Enabled = configProvider.AllowAction(ConfigAction.Add, btnAdd, selectedNode);
            btnMoveUp.Enabled = configProvider.AllowAction(ConfigAction.MoveUp, btnMoveUp, selectedNode);
            btnMoveDown.Enabled = configProvider.AllowAction(ConfigAction.MoveDown, btnMoveDown, selectedNode);
            btnDelete.Enabled = configProvider.AllowAction(ConfigAction.Delete, btnDelete, selectedNode);

            if (btnAddWithChoice.Visible)
            {
                bool anyItemEnabled = false;

                foreach (ToolStripItem item in btnAddWithChoice.DropDownItems)
                {
                    item.Enabled = configProvider.AllowAction(ConfigAction.Add, item, selectedNode);

                    if (item.Enabled)
                        anyItemEnabled = true;
                }

                btnAddWithChoice.Enabled = anyItemEnabled;
            }
        }


        private void FrmModuleConfig_Load(object sender, EventArgs e)
        {
            FormTranslator.Translate(this, GetType().FullName, 
                new FormTranslatorOptions { ContextMenus = new ContextMenuStrip[] { cmsTree } });

            if (!string.IsNullOrEmpty(configProvider.FormTitle))
                Text = configProvider.FormTitle;

            propertyGrid.ToolbarVisible = configProvider.GridToolbarVisible;
            propertyGrid.HelpVisible = configProvider.GridHelpVisible;
            propertyGrid.PropertySort = configProvider.GridSort;

            if (!configProvider.LoadConfig(out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            configProvider.BackupConfig();
            Modified = false;

            AddToolButtons();
            TakeTreeViewImages();
            FillTreeView();
        }

        private void FrmModuleConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommonPhrases.SaveConfigConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!configProvider.SaveConfig(out string errMsg))
                        {
                            ScadaUiUtils.ShowError(errMsg);
                            e.Cancel = true;
                        }
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            configProvider.HandleAddButtonClick(sender, treeView);
            Modified = true;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            treeView.MoveUpSelectedNode(TreeNodeBehavior.WithinParent);
            Modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            treeView.MoveDownSelectedNode(TreeNodeBehavior.WithinParent);
            Modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            treeView.RemoveSelectedNode();
            Modified = true;

            if (treeView.Nodes.Count == 0)
            {
                SetButtonsEnabled();
                propertyGrid.SelectedObject = null;
            }
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid.SelectedObject = configProvider.GetSelectedObject(treeView.SelectedNode);
            SetButtonsEnabled();
        }

        private void treeView_AfterCollapseExpand(object sender, TreeViewEventArgs e)
        {
            string nodeImage = configProvider.GetNodeImage(e.Node);

            if (!string.IsNullOrEmpty(nodeImage))
                e.Node.SetImageKey(nodeImage);
        }

        private void miCollapseAll_Click(object sender, EventArgs e)
        {
            treeView.CollapseAll();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (treeView.SelectedNode is TreeNode selectedNode)
            {
                string nodeImage = configProvider.GetNodeImage(propertyGrid.SelectedObject, selectedNode.IsExpanded);
                string nodeText = configProvider.GetNodeText(propertyGrid.SelectedObject);

                if (!string.IsNullOrEmpty(nodeImage))
                    selectedNode.SetImageKey(nodeImage);

                if (selectedNode.Text != nodeText)
                    selectedNode.Text = nodeText;
            }

            Modified = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (configProvider.SaveConfig(out string errMsg))
                Modified = false;
            else
                ScadaUiUtils.ShowError(errMsg);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            configProvider.RestoreConfig();
            FillTreeView();
            Modified = false;
        }
    }
}
