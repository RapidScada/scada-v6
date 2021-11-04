// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtProjectTools.Code;
using Scada.Admin.Extensions.ExtProjectTools.Forms;
using Scada.Data.Entities;
using Scada.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtProjectTools.Controls
{
    /// <summary>
    /// Represents a control that contains a main menu and a toolbar.
    /// <para>Представляет элемент управления, содержащий главное меню и панель инструментов.</para>
    /// </summary>
    public partial class CtrlMainMenu : UserControl
    {
        private readonly IAdminContext adminContext;      // the Administrator context
        private readonly RecentSelection recentSelection; // the recently selected objects


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CtrlMainMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlMainMenu(IAdminContext adminContext)
            : this()
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            recentSelection = new RecentSelection();

            SetMenuItemsEnabled();
            adminContext.CurrentProjectChanged += AdminContext_CurrentProjectChanged;
        }


        /// <summary>
        /// Enables or disables main menu items and toolbar buttons.
        /// </summary>
        private void SetMenuItemsEnabled()
        {
            bool projectIsOpen = adminContext.CurrentProject != null;
            miAddLine.Enabled = btnAddLine.Enabled = projectIsOpen;
            miAddDevice.Enabled = btnAddDevice.Enabled = projectIsOpen;
            miCreateChannels.Enabled = btnCreateChannels.Enabled = projectIsOpen;
            miCloneChannels.Enabled = projectIsOpen;
            miChannelMapByDevice.Enabled = projectIsOpen;
            miChannelMapByObject.Enabled = projectIsOpen;
            miCheckIntegrity.Enabled = projectIsOpen;
            miImportTable.Enabled = projectIsOpen;
            miExportTable.Enabled = projectIsOpen;
        }

        /// <summary>
        /// Gets menu items to add to the main menu.
        /// </summary>
        public ToolStripItem[] GetMainMenuItems()
        {
            return new ToolStripItem[] { miProjectTools };
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
            SetMenuItemsEnabled();
            recentSelection.Reset();
        }

        private void miAddLine_Click(object sender, EventArgs e)
        {
            // add communication line
            if (adminContext.CurrentProject != null)
            {
                FrmLineAdd frmLineAdd = new(adminContext.CurrentProject, recentSelection);

                if (frmLineAdd.ShowDialog() == DialogResult.OK)
                {
                    adminContext.MainForm.RefreshBaseTables(typeof(CommLine));

                    // add the communication line to the explorer
                    /*if (frmLineAdd.LineConfig != null &&
                        FindInstance(frmLineAdd.InstanceName, out TreeNode instanceNode, out LiveInstance liveInstance))
                    {
                        if (liveInstance.IsReady)
                        {
                            TreeNode commLinesNode = instanceNode.FindFirst(CommNodeType.CommLines);
                            TreeNode commLineNode = commShell.CreateCommLineNode(frmLineAdd.CommLineSettings,
                                liveInstance.CommEnvironment);
                            commLineNode.ContextMenuStrip = cmsCommLine;
                            commLinesNode.Nodes.Add(commLineNode);
                            tvExplorer.SelectedNode = commLineNode;
                        }
                        else
                        {
                            PrepareInstanceNode(instanceNode, liveInstance);
                            tvExplorer.SelectedNode = FindTreeNode(frmLineAdd.CommLineSettings, instanceNode);
                        }

                        SaveCommSettigns(liveInstance);
                    }*/
                }
            }
        }

        private void miAddDevice_Click(object sender, EventArgs e)
        {
            // add device
            /*if (adminContext.CurrentProject != null)
            {
                FrmDeviceAdd frmDeviceAdd = new FrmDeviceAdd(project, appData.AppState.RecentSelection);

                if (frmDeviceAdd.ShowDialog() == DialogResult.OK)
                {
                    RefreshBaseTables(typeof(KP));

                    if (frmDeviceAdd.KPSettings != null &&
                        FindInstance(frmDeviceAdd.InstanceName, out TreeNode instanceNode, out LiveInstance liveInstance))
                    {
                        // add the device to the explorer
                        if (liveInstance.IsReady)
                        {
                            TreeNode commLineNode = FindTreeNode(frmDeviceAdd.CommLineSettings, instanceNode);
                            TreeNode kpNode = commShell.CreateDeviceNode(frmDeviceAdd.KPSettings,
                                frmDeviceAdd.CommLineSettings, liveInstance.CommEnvironment);
                            kpNode.ContextMenuStrip = cmsDevice;
                            commLineNode.Nodes.Add(kpNode);
                            tvExplorer.SelectedNode = kpNode;
                            UpdateLineParams(kpNode);
                        }
                        else
                        {
                            PrepareInstanceNode(instanceNode, liveInstance);
                            tvExplorer.SelectedNode = FindTreeNode(frmDeviceAdd.KPSettings, instanceNode);
                        }

                        // set the device request parameters by default
                        if (liveInstance.CommEnvironment.TryGetKPView(frmDeviceAdd.KPSettings, true, null,
                            out KPView kpView, out string errMsg))
                        {
                            frmDeviceAdd.KPSettings.SetReqParams(kpView.DefaultReqParams);
                        }
                        else
                        {
                            ScadaUiUtils.ShowError(errMsg);
                        }

                        SaveCommSettigns(liveInstance);
                    }
                }
            }*/
        }

        private void miCreateChannels_Click(object sender, EventArgs e)
        {

        }

        private void miCloneChannels_Click(object sender, EventArgs e)
        {
            // clone channels
            if (adminContext.CurrentProject != null)
            {
                FrmCnlClone frmCnlClone = new(adminContext, adminContext.CurrentProject.ConfigBase);
                frmCnlClone.ShowDialog();

                if (frmCnlClone.ChannelsCloned)
                    adminContext.MainForm.RefreshBaseTables(typeof(Cnl));
            }
        }

        private void miChannelMap_Click(object sender, EventArgs e)
        {
            // generate channel map
            if (adminContext.CurrentProject != null)
            {
                new ChannelMap(adminContext.ErrLog, adminContext.CurrentProject.ConfigBase)
                {
                    GroupByDevices = sender == miChannelMapByDevice
                }
                .Generate(Path.Combine(adminContext.AppDirs.LogDir, ChannelMap.MapFileName));
            }
        }

        private void miCheckIntegrity_Click(object sender, EventArgs e)
        {
            // check integrity
            if (adminContext.CurrentProject != null)
            {
                new IntegrityCheck(adminContext.ErrLog, adminContext.CurrentProject.ConfigBase)
                    .Execute(Path.Combine(adminContext.AppDirs.LogDir, IntegrityCheck.OutputFileName));
            }
        }

        private void miImportTable_Click(object sender, EventArgs e)
        {
            // import table
            if (adminContext.CurrentProject != null)
            {
                FrmTableImport frmTableImport = new(adminContext.ErrLog, adminContext.CurrentProject.ConfigBase)
                {
                    SelectedItemType = adminContext.MainForm.ActiveBaseTable
                };

                if (frmTableImport.ShowDialog() == DialogResult.OK)
                    adminContext.MainForm.RefreshBaseTables(frmTableImport.SelectedItemType);
            }
        }

        private void miExportTable_Click(object sender, EventArgs e)
        {
            // export table
            if (adminContext.CurrentProject != null)
            {
                new FrmTableExport(adminContext.ErrLog, adminContext.CurrentProject.ConfigBase)
                {
                    SelectedItemType = adminContext.MainForm.ActiveBaseTable
                }
                .ShowDialog();
            }
        }
    }
}
