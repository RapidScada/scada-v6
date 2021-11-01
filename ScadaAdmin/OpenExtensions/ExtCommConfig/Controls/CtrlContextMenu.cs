// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm.Config;
using Scada.Forms;
using Scada.Lang;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control that contains context menus for the explorer tree.
    /// <para>Представляет элемент управления, содержащий контекстные меню для дерева проводника.</para>
    /// </summary>
    public partial class CtrlContextMenu : UserControl
    {
        private readonly IAdminContext adminContext; // the Administrator context


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlContextMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlContextMenu(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
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

        private void miLineImport_Click(object sender, EventArgs e)
        {
            // import Communicator settings
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                CommEnvironment commEnv = liveInstance.CommEnvironment;
                FrmCommImport frmCommImport = new FrmCommImport(project, liveInstance.ProjectInstance, commEnv);
                TreeNode lastAddedNode = null;

                if (selectedNode.TagIs(CommNodeType.CommLines))
                {
                    // import communication lines and devices
                    if (frmCommImport.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Comm.Settings.CommLine commLineSettings in frmCommImport.ImportedCommLines)
                        {
                            TreeNode commLineNode = commShell.CreateCommLineNode(commLineSettings, commEnv);
                            selectedNode.Nodes.Add(commLineNode);
                            lastAddedNode = commLineNode;
                        }
                    }
                }
                else if (selectedNode.TagIs(CommNodeType.CommLine))
                {
                    // import only devices
                    Comm.Settings.CommLine commLineSettings = GetRelatedObject<Comm.Settings.CommLine>(selectedNode);
                    frmCommImport.CommLineSettings = commLineSettings;

                    if (frmCommImport.ShowDialog() == DialogResult.OK)
                    {
                        foreach (Comm.Settings.KP kpSettings in frmCommImport.ImportedDevices)
                        {
                            TreeNode kpNode = commShell.CreateDeviceNode(kpSettings, commLineSettings, commEnv);
                            selectedNode.Nodes.Add(kpNode);
                            lastAddedNode = kpNode;
                        }

                        UpdateLineParams(lastAddedNode);
                    }
                }

                if (lastAddedNode != null)
                {
                    explorerBuilder.SetContextMenus(selectedNode);
                    tvExplorer.SelectedNode = lastAddedNode;
                    SaveCommConfig(liveInstance);
                }
            }*/
        }

        private void miLineSync_Click(object sender, EventArgs e)
        {
            // synchronize Communicator settings
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                (selectedNode.TagIs(CommNodeType.CommLines) || selectedNode.TagIs(CommNodeType.CommLine)) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                FrmCommSync frmCommSync = new FrmCommSync(project, liveInstance.ProjectInstance)
                {
                    CommLineSettings = GetRelatedObject<Comm.Settings.CommLine>(selectedNode, false)
                };

                if (frmCommSync.ShowDialog() == DialogResult.OK)
                {
                    TreeNode commLinesNode = selectedNode.FindClosest(CommNodeType.CommLines);

                    if (frmCommSync.CommLineSettings == null)
                    {
                        commShell.UpdateNodeText(commLinesNode);
                        UpdateChildFormHints(commLinesNode);

                        foreach (TreeNode commLineNode in commLinesNode.Nodes)
                        {
                            UpdateLineParams(commLineNode.FindFirst(CommNodeType.LineParams));
                        }
                    }
                    else
                    {
                        TreeNode commLineNode = FindTreeNode(frmCommSync.CommLineSettings, commLinesNode);
                        commShell.UpdateNodeText(commLineNode);
                        UpdateChildFormHints(commLineNode);
                        UpdateLineParams(commLineNode.FindFirst(CommNodeType.LineParams));
                    }

                    SaveCommConfig(liveInstance);
                }
            }*/
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
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Comm.Settings.CommLine commLine = GetRelatedObject<Comm.Settings.CommLine>(selectedNode);
                CommLineCmd commLineCmd;

                if (sender == miCommLineStart)
                    commLineCmd = CommLineCmd.StartLine;
                else if (sender == miCommLineStop)
                    commLineCmd = CommLineCmd.StopLine;
                else
                    commLineCmd = CommLineCmd.RestartLine;

                if (new CommLineCommand(commLine, liveInstance.CommEnvironment).Send(commLineCmd, out string msg))
                    ScadaUiUtils.ShowInfo(msg);
                else
                    ScadaUiUtils.ShowError(msg);
            }*/
        }


        private void cmsDevice_Opening(object sender, CancelEventArgs e)
        {
            /*if (FindClosestInstance(tvExplorer.SelectedNode, out LiveInstance liveInstance))
            {
                IAgentClient agentClient = liveInstance.CommEnvironment.AgentClient;
                miDeviceCommand.Enabled = agentClient != null && agentClient.IsLocal;
            }
            else
            {
                miDeviceCommand.Enabled = false;
                miDeviceProperties.Enabled = false;
            }*/
        }

        private void miDeviceCommand_Click(object sender, EventArgs e)
        {
            // show device command form
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.Device) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Comm.Settings.KP kp = GetRelatedObject<Comm.Settings.KP>(selectedNode);
                new FrmDeviceCommand(kp, liveInstance.CommEnvironment).ShowDialog();
            }*/
        }

        private void miDeviceProperties_Click(object sender, EventArgs e)
        {
            // show the device properties
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.Device) &&
                selectedNode.FindClosest(CommNodeType.CommLine) is TreeNode commLineNode &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                Comm.Settings.CommLine commLine = GetRelatedObject<Comm.Settings.CommLine>(commLineNode);
                Comm.Settings.KP kp = GetRelatedObject<Comm.Settings.KP>(selectedNode);

                if (liveInstance.CommEnvironment.TryGetKPView(kp, false, commLine.CustomParams, 
                    out KPView kpView, out string errMsg))
                {
                    if (kpView.CanShowProps)
                    {
                        kpView.ShowProps();

                        if (kpView.KPProps.Modified)
                        {
                            kp.CmdLine = kpView.KPProps.CmdLine;
                            UpdateLineParams(selectedNode);
                            SaveCommConfig(liveInstance);
                        }
                    }
                    else
                    {
                        ScadaUiUtils.ShowWarning(CommShellPhrases.NoDeviceProps);
                    }
                }
                else
                {
                    ScadaUiUtils.ShowError(errMsg);
                }
            }*/
        }
    }
}
