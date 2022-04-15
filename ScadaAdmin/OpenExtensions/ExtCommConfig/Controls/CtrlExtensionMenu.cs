// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Extensions.ExtCommConfig.Forms;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Agent;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinControl;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control that provides extension menus.
    /// <para>Представляет элемент управления, предоставляющий меню расширения.</para>
    /// </summary>
    public partial class CtrlExtensionMenu : UserControl
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlExtensionMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlExtensionMenu(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            recentSelection = new RecentSelection();

            SetMenuItemsEnabled();
            adminContext.CurrentProjectChanged += AdminContext_CurrentProjectChanged;
            adminContext.MessageToExtension += AdminContext_MessageToExtension;
        }


        /// <summary>
        /// Gets the explorer tree.
        /// </summary>
        private TreeView ExplorerTree => adminContext.MainForm.ExplorerTree;

        /// <summary>
        /// Gets the selected node of the explorer tree.
        /// </summary>
        private TreeNode SelectedNode => adminContext.MainForm.SelectedNode;

        /// <summary>
        /// Gets the communication line context menu.
        /// </summary>
        public ContextMenuStrip LineMenu => cmsLine;

        /// <summary>
        /// Gets the device context menu.
        /// </summary>
        public ContextMenuStrip DeviceMenu => cmsDevice;

        /// <summary>
        /// Gets all context menus.
        /// </summary>
        public ContextMenuStrip[] AllContextMenus => new ContextMenuStrip[] { cmsLine, cmsDevice };


        /// <summary>
        /// Enables or disables main menu items and toolbar buttons.
        /// </summary>
        private void SetMenuItemsEnabled()
        {
            bool projectIsOpen = adminContext.CurrentProject != null;
            miAddLine.Enabled = btnAddLine.Enabled = projectIsOpen;
            miAddDevice.Enabled = btnAddDevice.Enabled = projectIsOpen;
            miCreateChannels.Enabled = btnCreateChannels.Enabled = projectIsOpen;
        }

        /// <summary>
        /// Gets the Communicator application from the selected node, and validates the node type.
        /// </summary>
        private bool GetCommApp(out CommApp commApp, params string[] allowedNodeTypes)
        {
            if (SelectedNode?.Tag is CommNodeTag commNodeTag &&
                (allowedNodeTypes == null || allowedNodeTypes.Contains(commNodeTag.NodeType)))
            {
                commApp = commNodeTag.CommApp;
                return true;
            }
            else
            {
                commApp = null;
                return false;
            }
        }

        /// <summary>
        /// Saves the Communicator configuration.
        /// </summary>
        private void SaveCommConfig(CommApp commApp)
        {
            if (!commApp.SaveConfig(out string errMsg))
                adminContext.ErrLog.HandleError(errMsg);
        }

        /// <summary>
        /// Refreshes an open child form that shows the communication line configuration.
        /// </summary>
        private void RefreshLineConfigForm(TreeNode lineNode)
        {
            if (lineNode.FindFirst(CommNodeType.LineOptions) is TreeNode lineOptionsNode &&
                lineOptionsNode.Tag is TreeNodeTag tag && tag.ExistingForm is IChildForm childForm)
            {
                childForm.ChildFormTag.SendMessage(this, AdminMessage.RefreshData);
            }
        }

        /// <summary>
        /// Updates the specified communication line node.
        /// </summary>
        private void UpdateLineNode(string instanceName, int commLineNum)
        {
            if (adminContext.MainForm.FindInstanceNode(instanceName, out bool justPrepared) is TreeNode instanceNode &&
                !justPrepared && instanceNode.FindFirst(CommNodeType.Lines) is TreeNode linesNode)
            {
                foreach (TreeNode lineNode in linesNode.Nodes)
                {
                    if (lineNode.GetRelatedObject() is LineConfig lineConfig &&
                        lineConfig.CommLineNum == commLineNum)
                    {
                        adminContext.MainForm.CloseChildForms(lineNode, false);
                        new TreeViewBuilder(adminContext, this).UpdateLineNode(lineNode);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Finds a tree node that contains the specified related object.
        /// </summary>
        private static TreeNode FindNode(TreeNode startNode, object relatedObject)
        {
            foreach (TreeNode node in startNode.IterateNodes())
            {
                if (node.GetRelatedObject() == relatedObject)
                    return node;
            }

            return null;
        }


        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public ToolStripItem[] GetMainMenuItems()
        {
            return new ToolStripItem[] { miWizards };
        }

        /// <summary>
        /// Gets tool buttons to add to the toolbar.
        /// </summary>
        public ToolStripItem[] GetToobarButtons()
        {
            return new ToolStripItem[] { btnAddLine, btnAddDevice, btnCreateChannels };
        }


        private void AdminContext_CurrentProjectChanged(object sender, EventArgs e)
        {
            ExtensionUtils.ResetDriverViewCache();
            SetMenuItemsEnabled();
            recentSelection.Reset();
        }

        private void AdminContext_MessageToExtension(object sender, MessageEventArgs e)
        {
            if (e.Message == KnownExtensionMessage.UpdateLineNode)
            {
                UpdateLineNode(
                    (string)e.Arguments["InstanceName"],
                    (int)e.Arguments["CommLineNum"]);
            }
        }

        private void miAddLine_Click(object sender, EventArgs e)
        {
            // add communication line
            if (adminContext.CurrentProject != null)
            {
                FrmLineAdd frmLineAdd = new(adminContext.CurrentProject, recentSelection);

                if (frmLineAdd.ShowDialog() == DialogResult.OK)
                {
                    adminContext.MainForm.RefreshBaseTables(typeof(CommLine), true);

                    if (frmLineAdd.AddedToComm)
                    {
                        // update explorer
                        if (adminContext.MainForm.FindInstanceNode(frmLineAdd.Instance.Name, out bool justPrepared) is
                            TreeNode instanceNode)
                        {
                            if (justPrepared)
                            {
                                ExplorerTree.SelectedNode = FindNode(instanceNode, frmLineAdd.LineConfig);
                            }
                            else if (instanceNode.FindFirst(CommNodeType.Lines) is TreeNode linesNode)
                            {
                                TreeNode lineNode = new TreeViewBuilder(adminContext, this)
                                    .CreateLineNode(frmLineAdd.Instance.CommApp, frmLineAdd.LineConfig);
                                linesNode.Nodes.Add(lineNode);
                                ExplorerTree.SelectedNode = lineNode;
                            }
                        }

                        // save configuration
                        SaveCommConfig(frmLineAdd.Instance.CommApp);
                    }
                }
            }
        }

        private void miAddDevice_Click(object sender, EventArgs e)
        {
            // add device
            if (adminContext.CurrentProject != null)
            {
                FrmDeviceAdd frmDeviceAdd = new(adminContext, adminContext.CurrentProject, recentSelection);

                if (frmDeviceAdd.ShowDialog() == DialogResult.OK)
                {
                    adminContext.MainForm.RefreshBaseTables(typeof(Device), true);

                    if (frmDeviceAdd.AddedToComm)
                    {
                        // update explorer
                        if (adminContext.MainForm.FindInstanceNode(frmDeviceAdd.Instance.Name, out bool justPrepared) is
                            TreeNode instanceNode)
                        {
                            if (justPrepared)
                            {
                                ExplorerTree.SelectedNode = FindNode(instanceNode, frmDeviceAdd.DeviceConfig);
                            }
                            else if (FindNode(instanceNode, frmDeviceAdd.LineConfig) is TreeNode lineNode)
                            {
                                TreeNode deviceNode = new TreeViewBuilder(adminContext, this)
                                    .CreateDeviceNode(frmDeviceAdd.Instance.CommApp, frmDeviceAdd.DeviceConfig);
                                lineNode.Nodes.Add(deviceNode);
                                ExplorerTree.SelectedNode = deviceNode;
                                RefreshLineConfigForm(lineNode);
                            }
                        }

                        // save configuration
                        SaveCommConfig(frmDeviceAdd.Instance.CommApp);
                    }
                }
            }
        }

        private void miCreateChannels_Click(object sender, EventArgs e)
        {
            // create channels
            if (adminContext.CurrentProject != null)
            {
                FrmCnlCreate frmCnlCreate = new(adminContext, adminContext.CurrentProject, recentSelection);

                if (frmCnlCreate.ShowDialog() == DialogResult.OK)
                    adminContext.MainForm.RefreshBaseTables(typeof(Cnl), true);
            }
        }


        private void cmsLine_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable menu items
            bool isLineNode = SelectedNode != null && SelectedNode.TagIs(CommNodeType.Line);
            miLineMoveUp.Enabled = isLineNode && SelectedNode.PrevNode != null;
            miLineMoveDown.Enabled = isLineNode && SelectedNode.NextNode != null;
            miLineDelete.Enabled = isLineNode;

            miLineStart.Enabled = isLineNode;
            miLineStop.Enabled = isLineNode;
            miLineRestart.Enabled = isLineNode;
        }

        private void miLineSync_Click(object sender, EventArgs e)
        {
            // sync communication lines and devices
            if (GetCommApp(out CommApp commApp, CommNodeType.Lines, CommNodeType.Line))
            {
                FrmSync frmSync = new(adminContext, adminContext.CurrentProject, commApp);

                if (SelectedNode.GetRelatedObject() is LineConfig lineConfig)
                    frmSync.SelectedLineNum = lineConfig.CommLineNum;

                if (frmSync.ShowDialog() == DialogResult.OK)
                {
                    if (frmSync.BaseToComm)
                    {
                        // update explorer and open forms
                        TreeNode linesNode = SelectedNode?.FindClosest(CommNodeType.Lines);

                        if (frmSync.AddedToComm)
                        {
                            adminContext.MainForm.CloseChildForms(linesNode, false);
                            new TreeViewBuilder(adminContext, this).UpdateLinesNode(linesNode);
                        }
                        else
                        {
                            foreach (TreeNode lineNode in linesNode.Nodes)
                            {
                                TreeViewBuilder.UpdateLineNodeText(lineNode);
                                RefreshLineConfigForm(lineNode);

                                foreach (TreeNode lineSubnode in lineNode.Nodes)
                                {
                                    if (lineSubnode.TagIs(CommNodeType.Device))
                                        TreeViewBuilder.UpdateDeviceNodeText(lineSubnode);
                                }
                            }

                            adminContext.MainForm.UpdateChildFormHints(linesNode);
                        }

                        // save configuration
                        SaveCommConfig(commApp);
                    }
                    else
                    {
                        // refresh open tables
                        adminContext.MainForm.RefreshBaseTables(typeof(CommLine), true);
                        adminContext.MainForm.RefreshBaseTables(typeof(Device), true);
                    }
                };
            }
        }

        private void miLineAdd_Click(object sender, EventArgs e)
        {
            // add new line
            if (GetCommApp(out CommApp commApp, CommNodeType.Lines, CommNodeType.Line))
            {
                TreeNode linesNode = SelectedNode.FindClosest(CommNodeType.Lines);
                TreeNode lineNode = new TreeViewBuilder(adminContext, this).CreateLineNode(commApp, new LineConfig());
                lineNode.Expand();
                ExplorerTree.Insert(linesNode, lineNode);
                SaveCommConfig(commApp);
            }
        }

        private void miLineMoveUp_Click(object sender, EventArgs e)
        {
            // move up selected line
            if (GetCommApp(out CommApp commApp, CommNodeType.Line))
            {
                ExplorerTree.MoveUpSelectedNode(TreeNodeBehavior.WithinParent);
                SaveCommConfig(commApp);
            }
        }

        private void miLineMoveDown_Click(object sender, EventArgs e)
        {
            // move up selected line
            if (GetCommApp(out CommApp commApp, CommNodeType.Line))
            {
                ExplorerTree.MoveDownSelectedNode(TreeNodeBehavior.WithinParent);
                SaveCommConfig(commApp);
            }
        }

        private void miLineDelete_Click(object sender, EventArgs e)
        {
            // delete selected line
            if (GetCommApp(out CommApp commApp, CommNodeType.Line) &&
                MessageBox.Show(ExtensionPhrases.ConfirmDeleteLine, CommonPhrases.QuestionCaption,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                adminContext.MainForm.CloseChildForms(SelectedNode, false);
                ExplorerTree.RemoveSelectedNode();
                SaveCommConfig(commApp);
            }
        }

        private void miLineStartStop_Click(object sender, EventArgs e)
        {
            // start, stop or restart communication line
            if (SelectedNode?.GetRelatedObject() is LineConfig lineConfig)
            {
                if (adminContext.MainForm.GetAgentClient(SelectedNode, false) is IAgentClient agentClient)
                {
                    TeleCommand cmd = new() 
                    { 
                        CmdVal = lineConfig.CommLineNum
                    };

                    if (sender == miLineStart)
                        cmd.CmdCode = CommCommands.StartLine;
                    else if (sender == miLineStop)
                        cmd.CmdCode = CommCommands.StopLine;
                    else
                        cmd.CmdCode = CommCommands.RestartLine;

                    if (ExtensionUtils.SendCommand(adminContext, agentClient, cmd))
                        ScadaUiUtils.ShowInfo(CommonPhrases.CommandSent);
                }
                else
                {
                    ScadaUiUtils.ShowWarning(AdminPhrases.AgentNotEnabled);
                }
            }
        }


        private void miDeviceChannels_Click(object sender, EventArgs e)
        {
            // select channel table node
            if (SelectedNode?.GetRelatedObject() is DeviceConfig deviceConfig)
            {
                if (adminContext.MainForm.FindBaseTableNode(typeof(Cnl), deviceConfig.DeviceNum) is
                    TreeNode treeNode)
                {
                    ExplorerTree.SelectedNode = treeNode;
                }
                else
                {
                    ScadaUiUtils.ShowWarning(ExtensionPhrases.CnlNodeNotFound);
                }
            }
        }

        private void miDeviceCommand_Click(object sender, EventArgs e)
        {
            // show device command form
            if (SelectedNode?.GetRelatedObject() is DeviceConfig deviceConfig)
            {
                if (adminContext.MainForm.GetAgentClient(SelectedNode, false) is IAgentClient agentClient)
                {
                    FrmDeviceCommand frmDeviceCommand = new(adminContext, deviceConfig);
                    frmDeviceCommand.AgentClient = agentClient;
                    frmDeviceCommand.ShowDialog();
                }
                else
                {
                    ScadaUiUtils.ShowWarning(AdminPhrases.AgentNotEnabled);
                }
            }
        }

        private void miDevicePoll_Click(object sender, EventArgs e)
        {
            // send command to poll device
            if (SelectedNode?.GetRelatedObject() is DeviceConfig deviceConfig)
            {
                if (adminContext.MainForm.GetAgentClient(SelectedNode, false) is IAgentClient agentClient)
                {
                    if (ExtensionUtils.SendCommand(adminContext, agentClient, new TeleCommand
                    {
                        CreationTime = DateTime.UtcNow,
                        DeviceNum = deviceConfig.DeviceNum,
                        CmdCode = CommCommands.PollDevice
                    }))
                    {
                        ScadaUiUtils.ShowInfo(CommonPhrases.CommandSent);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowWarning(AdminPhrases.AgentNotEnabled);
                }
            }
        }

        private void miDeviceProperties_Click(object sender, EventArgs e)
        {
            // show device properties
            if (GetCommApp(out CommApp commApp, CommNodeType.Device) &&
                SelectedNode?.GetRelatedObject() is DeviceConfig deviceConfig)
            {
                ExtensionUtils.ShowDeviceProperties(adminContext, commApp, deviceConfig, SelectedNode);
            }
        }
    }
}
