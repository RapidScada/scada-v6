// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Code;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Controls
{
    /// <summary>
    /// Represents a control for selecting synchronized communication lines and devices.
    /// <para>Представляет элемент управления для выбора синхронизируемых линий связи и устройств.</para>
    /// </summary>
    public partial class CtrlSync2 : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CtrlSync2()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Fills the tree view according to the configuration database.
        /// </summary>
        public void FillTreeView(ConfigDataset configDataset, int selectedLineNum)
        {
            ArgumentNullException.ThrowIfNull(configDataset, nameof(configDataset));

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                foreach (CommLine commLine in configDataset.CommLineTable.Enumerate())
                {
                    TreeNode lineNode = new(CommUtils.GetLineTitle(commLine))
                    {
                        Tag = new TreeNodeTag(commLine, CommNodeType.Line)
                    };

                    foreach (Device device in configDataset.DeviceTable.Select(
                        new TableFilter("CommLineNum", commLine.CommLineNum), true))
                    {
                        lineNode.Nodes.Add(new TreeNode(CommUtils.GetDeviceTitle(device))
                        {
                            Tag = new TreeNodeTag(device, CommNodeType.Device)
                        });
                    }

                    treeView.Nodes.Add(lineNode);

                    if (commLine.CommLineNum == selectedLineNum)
                    {
                        lineNode.Checked = true;
                        lineNode.Expand();
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the tree view according to the Communicator configuration.
        /// </summary>
        public void FillTreeView(CommConfig commConfig, int selectedLineNum)
        {
            ArgumentNullException.ThrowIfNull(commConfig, nameof(commConfig));

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                foreach (LineConfig lineConfig in commConfig.Lines)
                {
                    TreeNode lineNode = new(CommUtils.GetLineTitle(lineConfig))
                    {
                        Tag = new TreeNodeTag(lineConfig, CommNodeType.Line)
                    };

                    foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
                    {
                        lineNode.Nodes.Add(new TreeNode(CommUtils.GetDeviceTitle(deviceConfig))
                        {
                            Tag = new TreeNodeTag(deviceConfig, CommNodeType.Device)
                        });
                    }

                    treeView.Nodes.Add(lineNode);

                    if (lineConfig.CommLineNum == selectedLineNum)
                    {
                        lineNode.Checked = true;
                        lineNode.Expand();
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Gets the selected tree nodes representing communication lines.
        /// </summary>
        public List<TreeNode> GetSelectedLineNodes()
        {
            List<TreeNode> selectedNodes = new();

            foreach (TreeNode treeNode in treeView.Nodes)
            {
                if (treeNode.Checked)
                    selectedNodes.Add(treeNode);
            }

            return selectedNodes;
        }


        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView.AfterCheck -= treeView_AfterCheck;
            TreeNode node = e.Node;
            bool isChecked = node.Checked;

            // select parent node
            if (node.Parent != null && isChecked)
                node.Parent.Checked = true;

            // select child nodes
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Checked = isChecked;
            }

            if (isChecked)
                node.Expand();

            treeView.AfterCheck += treeView_AfterCheck;
        }
    }
}
