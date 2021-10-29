// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;
using Scada.Comm.Drivers.DrvModbus.View.Properties;
using Scada.Forms;
using Scada.Lang;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Comm.Drivers.DrvModbus.View.Forms
{
    /// <summary>
    /// Represents a form for editing a device template.
    /// <para>Представляет форму для редактирования шаблона устройства.</para>
    /// </summary>
    public partial class FrmDeviceTemplate : Form
    {
        /// <summary>
        /// Имя файла нового шаблона устройства
        /// </summary>
        private const string NewFileName = "KpModbus_NewTemplate.xml";

        private readonly AppDirs appDirs;          // the application directories
        private readonly CustomUi customUi;        // the UI customization object
        private readonly TreeNode elemGroupsNode;  // the tree node containing element groups
        private readonly TreeNode commandsNode;    // the tree node containing commands

        private DeviceTemplate template;           // the device template for editing
        private bool modified;                     // indicates that the device template is modified
        private TreeNode selectedNode;             // the selected tree node
        private ElemGroupConfig selectedElemGroup; // the selected element group configuration
        private ElemTag selectedElemTag;           // the selected element metadata
        private CmdConfig selectedCmd;             // the selected command configuration


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceTemplate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceTemplate(AppDirs appDirs, CustomUi customUi)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.customUi = customUi ?? throw new ArgumentNullException(nameof(customUi));
            elemGroupsNode = treeView.Nodes["elemGroupsNode"];
            commandsNode = treeView.Nodes["commandsNode"];

            template = null;
            modified = false;
            selectedNode = null;
            selectedElemGroup = null;
            selectedElemTag = null;
            selectedCmd = null;

            SaveOnly = false;
            FileName = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the device template is modified.
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
                SetFormTitle();
                btnSave.Enabled = modified;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only the save file command is allowed.
        /// </summary>
        public bool SaveOnly { get; set; }

        /// <summary>
        /// Gets or sets the device template file name.
        /// </summary>
        /// <remarks>Empty if the device template is not saved on disk.</remarks>
        public string FileName { get; set; }


        /// <summary>
        /// Takes the tree view and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilTree.Images.Add("cmd.png", Resources.cmd);
            ilTree.Images.Add("cmds.png", Resources.cmds);
            ilTree.Images.Add("elem.png", Resources.elem);
            ilTree.Images.Add("group.png", Resources.group);
            ilTree.Images.Add("group_inactive.png", Resources.group_inactive);

            elemGroupsNode.SetImageKey("group.png");
            commandsNode.SetImageKey("cmds.png");
        }

        /// <summary>
        /// Sets the form title.
        /// </summary>
        private void SetFormTitle()
        {
            Text = (Modified ? "*" : "") + string.Format(DriverPhrases.TemplateTitle, 
                string.IsNullOrEmpty(FileName) ? NewFileName : Path.GetFileName(FileName));
        }

        /// <summary>
        /// Loads the device template from the specified file.
        /// </summary>
        private void LoadTemplate(string fileName)
        {
            template = customUi.CreateDeviceTemplate();

            if (!template.Load(fileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            FileName = fileName;
            SetFormTitle();
            FillTree();
            Modified = false;
        }

        /// <summary>
        /// Fills the tree view according to the device template.
        /// </summary>
        private void FillTree()
        {
            // reset selected objects
            selectedNode = null;
            selectedElemGroup = null;
            selectedElemTag = null;
            selectedCmd = null;
            ShowElemGroupConfig(null);

            try
            {
                treeView.BeginUpdate();
                elemGroupsNode.Nodes.Clear();
                commandsNode.Nodes.Clear();

                // fill element groups
                foreach (ElemGroupConfig elemGroup in template.ElemGroups)
                {
                    elemGroupsNode.Nodes.Add(CreateElemGroupNode(elemGroup));
                }

                // fill commands
                foreach (CmdConfig modbusCmd in template.Cmds)
                {
                    commandsNode.Nodes.Add(CreateCmdNode(modbusCmd));
                }

                elemGroupsNode.Expand();
                commandsNode.Expand();
                treeView.SelectedNode = elemGroupsNode;
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a node that represents the specified element group.
        /// </summary>
        private TreeNode CreateElemGroupNode(ElemGroupConfig elemGroup)
        {
            TreeNode groupNode = new(GetElemGroupNodeText(elemGroup)) { Tag = elemGroup };
            groupNode.SetImageKey(elemGroup.Active ? "group.png" : "group_inactive.png");

            int elemAddr = elemGroup.Address;
            int elemTagNum = elemGroup.StartTagNum;

            foreach (ElemConfig elem in elemGroup.Elems)
            {
                groupNode.Nodes.Add(CreateElemNode(new ElemTag(template.Options, elemGroup, elem)
                {
                    Address = elemAddr,
                    TagNum = elemTagNum++
                }));

                elemAddr += elem.Quantity;
            }

            return groupNode;
        }

        /// <summary>
        /// Creates a node that represents the specified element.
        /// </summary>
        private static TreeNode CreateElemNode(ElemTag elemTag)
        {
            return TreeViewExtensions.CreateNode(elemTag, "elem.png");
        }

        /// <summary>
        /// Creates a node that represents the specified command.
        /// </summary>
        private TreeNode CreateCmdNode(CmdConfig cmd)
        {
            TreeNode cmdNode = new(GetCmdNodeText(cmd)) { Tag = cmd };
            cmdNode.SetImageKey("cmd.png");
            return cmdNode;
        }

        /// <summary>
        /// Gets the command node text.
        /// </summary>
        private static string GetElemGroupNodeText(ElemGroupConfig elemGroup)
        {
            return string.Format("{0} ({1})",
                string.IsNullOrEmpty(elemGroup.Name) ? DriverPhrases.UnnamedElemGroup : elemGroup.Name,
                ModbusUtils.GetDataBlockName(elemGroup.DataBlock));
        }

        /// <summary>
        /// Gets the command node text.
        /// </summary>
        private string GetCmdNodeText(CmdConfig cmd)
        {
            string cmdName = string.IsNullOrEmpty(cmd.Name) ? DriverPhrases.UnnamedCommand : cmd.Name;
            string blockName = ModbusUtils.GetDataBlockName(cmd.DataBlock);

            if (cmd.DataBlock == DataBlock.Custom)
            {
                return string.Format("{0} ({1})", cmdName, blockName);
            }
            else
            {
                string addrRange = ModbusUtils.GetAddressRange(cmd.Address,
                    cmd.ElemCnt * ModbusUtils.GetQuantity(cmd.ElemType),
                    template.Options.ZeroAddr, template.Options.DecAddr);
                return string.Format("{0} ({1}, {2})", cmdName, blockName, addrRange);
            }
        }

        /// <summary>
        /// Updates the image and text of the selected element group node.
        /// </summary>
        private void UpdateElemGroupNode()
        {
            if (selectedElemGroup != null)
            {
                selectedNode.SetImageKey(selectedElemGroup.Active ? "group.png" : "group_inactive.png");
                selectedNode.Text = GetElemGroupNodeText(selectedElemGroup);
            }
        }

        /// <summary>
        /// Updates the child nodes of the specified element group node.
        /// Affects the element address, tag number and node text.
        /// </summary>
        private void UpdateElemNodes(TreeNode groupNode)
        {
            if (groupNode?.Tag is not ElemGroupConfig elemGroup)
                return;

            treeView.BeginUpdate();
            int elemAddr = elemGroup.Address;
            int elemTagNum = elemGroup.StartTagNum;

            foreach (TreeNode elemNode in groupNode.Nodes)
            {
                ElemTag elemTag = (ElemTag)elemNode.Tag;
                elemTag.Address = elemAddr;
                elemTag.TagNum = elemTagNum++;
                elemNode.Text = elemTag.NodeText;
                elemAddr += (ushort)elemTag.Elem.Quantity;
            }

            treeView.EndUpdate();
        }

        /// <summary>
        /// Updates the tag numbers starting from the specified element group node.
        /// </summary>
        private void UpdateTagNums(TreeNode startGroupNode)
        {
            // validate start node
            if (startGroupNode?.Tag is not ElemGroupConfig)
                return;

            // get start tag number
            TreeNode prevGroupNode = startGroupNode.PrevNode;
            int tagNum = prevGroupNode?.Tag is ElemGroupConfig prevElemGroup
                ? prevElemGroup.StartTagNum + prevElemGroup.Elems.Count
                : 1;

            // update element groups and elements
            for (int nodeIdx = startGroupNode.Index, nodeCnt = elemGroupsNode.Nodes.Count; 
                nodeIdx < nodeCnt; nodeIdx++)
            {
                TreeNode groupNode = elemGroupsNode.Nodes[nodeIdx];
                ElemGroupConfig elemGroup = (ElemGroupConfig)groupNode.Tag;
                elemGroup.StartTagNum = tagNum;

                int elemTagNum = tagNum;
                tagNum += elemGroup.Elems.Count;

                foreach (TreeNode elemNode in groupNode.Nodes)
                {
                    ElemTag elem = (ElemTag)elemNode.Tag;
                    elem.TagNum = elemTagNum++;
                }
            }
        }

        /// <summary>
        /// Updates the text of the selected command node.
        /// </summary>
        private void UpdateCmdNode()
        {
            if (selectedCmd != null)
                selectedNode.Text = GetCmdNodeText(selectedCmd);
        }

        /// <summary>
        /// Shows the element group configuration.
        /// </summary>
        private void ShowElemGroupConfig(ElemGroupConfig elemGroup)
        {
            ctrlElemGroup.Visible = true;
            ctrlElem.Visible = false;
            ctrlCmd.Visible = false;

            ctrlElemGroup.TemplateOptions = template.Options;
            ctrlElemGroup.ElemGroup = elemGroup;
        }

        /// <summary>
        /// Shows the element configuration.
        /// </summary>
        private void ShowElemConfig(ElemTag elemTag)
        {
            ctrlElemGroup.Visible = false;
            ctrlElem.Visible = true;
            ctrlCmd.Visible = false;

            ctrlElem.ElemTag = elemTag;
        }

        /// <summary>
        /// Shows the command configuration.
        /// </summary>
        private void ShowCmdConfig(CmdConfig cmd)
        {
            ctrlElemGroup.Visible = false;
            ctrlElem.Visible = false;
            ctrlCmd.Visible = true;

            ctrlCmd.TemplateOptions = template.Options;
            ctrlCmd.Cmd = cmd;
        }

        /// <summary>
        /// Shows the configuration of the currently selected item.
        /// </summary>
        private void ShowSelectedItem()
        {
            if (selectedElemGroup != null)
                ShowElemGroupConfig(selectedElemGroup);
            else if (selectedElemTag != null)
                ShowElemConfig(selectedElemTag);
            else if (selectedCmd != null)
                ShowCmdConfig(selectedCmd);
            else if (selectedNode == elemGroupsNode)
                ShowElemGroupConfig(null);
            else if (selectedNode == commandsNode)
                ShowCmdConfig(null);
            else // never performed
                DisableItemConfig();
        }

        /// <summary>
        /// Disables the controls for editing item configuration.
        /// </summary>
        private void DisableItemConfig()
        {
            ctrlElemGroup.ElemGroup = null;
            ctrlElem.ElemTag = null;
            ctrlCmd.Cmd = null;
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            btnAddElem.Enabled = selectedElemGroup != null || selectedElemTag != null;

            if (selectedElemGroup == null && selectedCmd == null &&
                (selectedElemTag == null || selectedElemTag.ElemGroup.Elems.Count <= 1)) // do not delete last element
            {
                btnMoveUp.Enabled = false;
                btnMoveDown.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnMoveUp.Enabled = treeView.MoveUpSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent);
                btnMoveDown.Enabled = treeView.MoveDownSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent);
                btnDelete.Enabled = true;
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        private bool SaveChanges(bool saveAs)
        {
            // define file name
            string newFileName = "";

            if (saveAs || string.IsNullOrEmpty(FileName))
            {
                saveFileDialog.FileName = string.IsNullOrEmpty(FileName) ? NewFileName : Path.GetFileName(FileName);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    newFileName = saveFileDialog.FileName;
            }
            else
            {
                newFileName = FileName;
            }

            if (newFileName == "")
            {
                return false;
            }
            else
            {
                // save device template
                if (template.Save(newFileName, out string errMsg))
                {
                    FileName = newFileName;
                    Modified = false;
                    return true;
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                    return false;
                }
            }
        }

        /// <summary>
        /// Confirms that the device template can be closed.
        /// </summary>
        private bool ConfirmCloseTemplate()
        {
            if (Modified)
            {
                return MessageBox.Show(DriverPhrases.SaveTemplateConfirm, CommonPhrases.QuestionCaption, 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) switch
                {
                    DialogResult.Yes => SaveChanges(false),
                    DialogResult.No => true,
                    _ => false,
                };
            }
            else
            {
                return true;
            }
        }


        private void FrmDevTemplate_Load(object sender, EventArgs e)
        {
            // translate form
            FormTranslator.Translate(this, GetType().FullName);
            FormTranslator.Translate(ctrlElemGroup, ctrlElemGroup.GetType().FullName);
            FormTranslator.Translate(ctrlElem, ctrlElem.GetType().FullName);
            FormTranslator.Translate(ctrlCmd, ctrlCmd.GetType().FullName);
            openFileDialog.SetFilter(CommonPhrases.XmlFileFilter);
            saveFileDialog.SetFilter(CommonPhrases.XmlFileFilter);
            elemGroupsNode.Text = DriverPhrases.ElemGroupsNode;
            commandsNode.Text = DriverPhrases.CommandsNode;

            // setup controls
            TakeTreeViewImages();
            openFileDialog.InitialDirectory = appDirs.ConfigDir;
            saveFileDialog.InitialDirectory = appDirs.ConfigDir;
            ctrlElem.Top = ctrlCmd.Top = ctrlElemGroup.Top;
            btnEditOptionsExt.Visible = customUi.ExtendedOptionsAvailable;

            if (SaveOnly)
            {
                btnNew.Visible = false;
                btnOpen.Visible = false;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                template = customUi.CreateDeviceTemplate();
                FillTree();
                Modified = false;
            }
            else
            {
                LoadTemplate(FileName);
            }
        }

        private void FrmDevTemplate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ConfirmCloseTemplate();
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            // create device template
            if (ConfirmCloseTemplate())
            {
                template = customUi.CreateDeviceTemplate();
                FileName = "";
                SetFormTitle();
                FillTree();
                Modified = false;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // open device template
            if (ConfirmCloseTemplate())
            {
                openFileDialog.FileName = "";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    LoadTemplate(openFileDialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // save device template
            SaveChanges(sender == btnSaveAs);
        }


        private void btnAddElemGroup_Click(object sender, EventArgs e)
        {
            // add new element group
            ElemGroupConfig elemGroup = template.CreateElemGroupConfig();
            elemGroup.Elems.Add(elemGroup.CreateElemConfig()); // at least one element

            TreeNode groupNode = CreateElemGroupNode(elemGroup);
            treeView.Insert(elemGroupsNode, groupNode, template.ElemGroups, elemGroup);
            groupNode.Expand();

            UpdateTagNums(groupNode);
            ctrlElemGroup.SetFocus();
            Modified = true;
        }

        private void btnAddElem_Click(object sender, EventArgs e)
        {
            // add new element
            if ((selectedElemGroup ?? selectedElemTag?.ElemGroup) is not ElemGroupConfig elemGroup)
                return;

            if (elemGroup.Elems.Count >= elemGroup.MaxElemCnt)
            {
                MessageBox.Show(string.Format(DriverPhrases.ElemCntExceeded, elemGroup.MaxElemCnt),
                    CommonPhrases.WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ElemConfig elem = elemGroup.CreateElemConfig();
            elem.ElemType = elemGroup.DefaultElemType;
            ElemTag elemTag = new ElemTag(template.Options, elemGroup, elem);

            TreeNode elemNode = CreateElemNode(elemTag);
            TreeNode groupNode = selectedNode.Tag is ElemTag ? selectedNode.Parent : selectedNode;
            treeView.Insert(groupNode, elemNode, elemGroup.Elems, elem);

            UpdateElemNodes(groupNode);
            UpdateTagNums(groupNode);
            ShowElemConfig(elemTag);
            ctrlElem.SetFocus();
            Modified = true;
        }

        private void btnAddCmd_Click(object sender, EventArgs e)
        {
            // add new command
            CmdConfig cmd = template.CreateCmdConfig();
            treeView.Insert(commandsNode, CreateCmdNode(cmd), template.Cmds, cmd);
            ctrlCmd.SetFocus();
            Modified = true;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (selectedElemGroup != null)
            {
                // move up element group
                treeView.MoveUpSelectedNode(template.ElemGroups);
                UpdateTagNums(selectedNode);
            }
            else if (selectedElemTag != null)
            {
                // move up element
                treeView.MoveUpSelectedNode(selectedElemTag.ElemGroup.Elems);
                UpdateElemNodes(selectedNode.Parent);
                ShowElemConfig(selectedElemTag);
            }
            else if (selectedCmd != null)
            {
                // move up command
                treeView.MoveUpSelectedNode(template.Cmds);
            }

            btnMoveUp.Enabled = treeView.MoveUpSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent);
            btnMoveDown.Enabled = treeView.MoveDownSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent);
            Modified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (selectedElemGroup != null)
            {
                // move down element group
                treeView.MoveDownSelectedNode(template.ElemGroups);
                UpdateTagNums(selectedNode.NextNode);
            }
            else if (selectedElemTag != null)
            {
                // move down element
                treeView.MoveDownSelectedNode(selectedElemTag.ElemGroup.Elems);
                UpdateElemNodes(selectedNode.Parent);
                ShowElemConfig(selectedElemTag);
            }
            else if (selectedCmd != null)
            {
                // move down command
                treeView.MoveDownSelectedNode(template.Cmds);
            }

            btnMoveUp.Enabled = treeView.MoveUpSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent);
            btnMoveDown.Enabled = treeView.MoveDownSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent);
            Modified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedElemGroup != null)
            {
                // delete element group
                treeView.RemoveSelectedNode(template.ElemGroups);
            }
            else if (selectedElemTag != null)
            {
                // delete element
                TreeNode groupNode = selectedNode.Parent;
                treeView.RemoveSelectedNode(selectedElemTag.ElemGroup.Elems);
                UpdateElemNodes(groupNode);
                UpdateTagNums(groupNode);
                ShowSelectedItem();
                SetButtonsEnabled();
            }
            else if (selectedCmd != null)
            {
                // delete command
                treeView.RemoveSelectedNode(template.Cmds);
            }

            Modified = true;
        }


        private void btnEditOptions_Click(object sender, EventArgs e)
        {
            // edit template options
            FrmTemplateOptions frmTemplateOptions = new(template.Options);

            if (frmTemplateOptions.ShowDialog() == DialogResult.OK)
            {
                FillTree();
                Modified = true;
            }
        }

        private void btnEditOptionsExt_Click(object sender, EventArgs e)
        {
            // edit extended template options
            if (customUi.ShowExtendedOptions(template))
            {
                FillTree();
                Modified = true;
            }
        }


        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedNode = e.Node;
            selectedElemGroup = selectedNode.Tag as ElemGroupConfig;
            selectedElemTag = selectedNode.Tag as ElemTag;
            selectedCmd = selectedNode.Tag as CmdConfig;
            ShowSelectedItem();
            SetButtonsEnabled();
        }

        private void ctrlElemGroup_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (selectedElemGroup == null)
                return;

            Modified = true;
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                UpdateElemGroupNode();

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.ChildNodes))
                UpdateElemNodes(selectedNode);

            if (treeUpdateTypes.HasFlag(TreeUpdateTypes.ChildCount))
            {
                treeView.BeginUpdate();
                int oldElemCnt = selectedNode.Nodes.Count;
                int newElemCnt = selectedElemGroup.Elems.Count;

                if (oldElemCnt < newElemCnt)
                {
                    // add tree nodes for new elements
                    int elemAddr = selectedElemGroup.Address;

                    for (int elemIdx = 0; elemIdx < newElemCnt; elemIdx++)
                    {
                        ElemConfig elem = selectedElemGroup.Elems[elemIdx];

                        if (elemIdx >= oldElemCnt)
                        {
                            selectedNode.Nodes.Add(CreateElemNode(
                                new ElemTag(template.Options, selectedElemGroup, elem)
                                {
                                    Address = elemAddr
                                }));
                        }

                        elemAddr += elem.Quantity;
                    }
                }
                else if (oldElemCnt > newElemCnt)
                {
                    // remove redundant tree nodes
                    for (int i = newElemCnt; i < oldElemCnt; i++)
                    {
                        selectedNode.Nodes.RemoveAt(newElemCnt);
                    }
                }

                UpdateTagNums(selectedNode);
                treeView.EndUpdate();
            }
        }

        private void ctrlElem_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (selectedElemTag != null)
            {
                Modified = true;
                TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = selectedElemTag.NodeText;

                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.NextSiblings))
                    UpdateElemNodes(selectedNode.Parent);
            }
        }

        private void ctrlCmd_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (selectedCmd != null)
            {
                Modified = true;
                TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    UpdateCmdNode();
            }
        }
    }
}
