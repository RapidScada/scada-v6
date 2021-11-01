// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control that contains context menus for the explorer tree.
    /// <para>Представляет элемент управления, содержащий контекстные меню для дерева проводника.</para>
    /// </summary>
    public partial class CtrlContextMenu : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlContextMenu()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets the communication line context menu.
        /// </summary>
        public ContextMenuStrip LineMenu => cmsLine;

        /// <summary>
        /// Gets the device context menu.
        /// </summary>
        public ContextMenuStrip DeviceMenu => cmsDevice;


        private void cmsLine_Opening(object sender, CancelEventArgs e)
        {
            // enable or disable menu items
            /*TreeNode selectedNode = tvExplorer.SelectedNode;
            bool isCommLineNode = selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine);
            bool isLocal = FindClosestInstance(selectedNode, out LiveInstance liveInstance) &&
                liveInstance.CommEnvironment.AgentClient != null && liveInstance.CommEnvironment.AgentClient.IsLocal;

            miCommLineMoveUp.Enabled = isCommLineNode && selectedNode.PrevNode != null;
            miCommLineMoveDown.Enabled = isCommLineNode && selectedNode.NextNode != null;
            miCommLineDelete.Enabled = isCommLineNode;

            miCommLineStart.Enabled = isCommLineNode && isLocal;
            miCommLineStop.Enabled = isCommLineNode && isLocal;
            miCommLineRestart.Enabled = isCommLineNode && isLocal;*/
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
            // add new communication line
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null &&
                (selectedNode.TagIs(CommNodeType.CommLines) || selectedNode.TagIs(CommNodeType.CommLine)) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                TreeNode commLinesNode = selectedNode.FindClosest(CommNodeType.CommLines);
                TreeNode commLineNode = commShell.CreateCommLineNode(liveInstance.CommEnvironment);
                commLineNode.ContextMenuStrip = cmsCommLine;
                commLineNode.Expand();
                tvExplorer.Insert(commLinesNode, commLineNode);
                SaveCommConfig(liveInstance);
            }*/
        }

        private void miLineMoveUp_Click(object sender, EventArgs e)
        {
            // move up selected communication line
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                tvExplorer.MoveUpSelectedNode(TreeViewUtils.MoveBehavior.WithinParent);
                SaveCommConfig(liveInstance);
            }*/
        }

        private void miLineMoveDown_Click(object sender, EventArgs e)
        {
            // move up selected communication line
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance))
            {
                tvExplorer.MoveDownSelectedNode(TreeViewUtils.MoveBehavior.WithinParent);
                SaveCommConfig(liveInstance);
            }*/
        }

        private void miLineDelete_Click(object sender, EventArgs e)
        {
            // delete selected communication line
            /*TreeNode selectedNode = tvExplorer.SelectedNode;

            if (selectedNode != null && selectedNode.TagIs(CommNodeType.CommLine) &&
                FindClosestInstance(selectedNode, out LiveInstance liveInstance) &&
                MessageBox.Show(AppPhrases.ConfirmDeleteCommLine, CommonPhrases.QuestionCaption, 
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseChildForms(selectedNode);
                tvExplorer.RemoveSelectedNode();
                SaveCommConfig(liveInstance);
            }*/
        }

        private void miLineStart_Click(object sender, EventArgs e)
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

        private void miLineStop_Click(object sender, EventArgs e)
        {

        }

        private void miLineRestart_Click(object sender, EventArgs e)
        {

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

        /*private bool SaveCommConfig(LiveInstance liveInstance)
        {
            if (liveInstance.ProjectInstance.CommApp.SaveConfig(out string errMsg))
            {
                return true;
            }
            else
            {
                Log.HandleError(errMsg);
                return false;
            }
        }*/
    }
}
