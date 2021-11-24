// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Admin.Extensions.ExtCommConfig.Controls;
using Scada.Admin.Extensions.ExtCommConfig.Forms;
using Scada.Admin.Project;
using Scada.Comm;
using Scada.Comm.Config;
using Scada.Forms;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// Creates nodes for the explorer tree.
    /// <para>Создает узлы для дерева проводника.</para>
    /// </summary>
    internal class TreeViewBuilder
    {
        private readonly IAdminContext adminContext;    // the Administrator context
        private readonly CtrlExtensionMenu menuControl; // contains the menus


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TreeViewBuilder(IAdminContext adminContext, CtrlExtensionMenu menuControl)
        {
            this.adminContext = adminContext ?? throw new ArgumentNullException(nameof(adminContext));
            this.menuControl = menuControl ?? throw new ArgumentNullException(nameof(menuControl));
        }


        /// <summary>
        /// Creates a tree node that represents communication lines.
        /// </summary>
        private TreeNode CreateLinesNode(CommApp commApp)
        {
            TreeNode linesNode = TreeViewExtensions.CreateNode(ExtensionPhrases.LinesNode, ImageKey.Lines);
            linesNode.ContextMenuStrip = menuControl.LineMenu;
            linesNode.Tag = new CommNodeTag(commApp, commApp.AppConfig, CommNodeType.Lines);

            foreach (LineConfig lineConfig in commApp.AppConfig.Lines)
            {
                linesNode.Nodes.Add(CreateLineNode(commApp, lineConfig));
            }

            return linesNode;
        }

        /// <summary>
        /// Creates a tree node that represents the device.
        /// </summary>
        private TreeNode CreateDeviceNode(CommApp commApp, LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new TreeNode(CommUtils.GetDeviceTitle(deviceConfig))
            {
                ImageKey = ImageKey.Device,
                SelectedImageKey = ImageKey.Device,
                Tag = new CommNodeTag(commApp, null, CommNodeType.Device)
                {
                    FormType = typeof(FrmDeviceData),
                    FormArgs = new object[] { adminContext, commApp, lineConfig, deviceConfig }
                }
            };
        }


        /// <summary>
        /// Creates a tree node that represents the specified communication line.
        /// </summary>
        public TreeNode CreateLineNode(CommApp commApp, LineConfig lineConfig)
        {
            TreeNode lineNode = TreeViewExtensions.CreateNode(
                CommUtils.GetLineTitle(lineConfig.CommLineNum, lineConfig.Name),
                lineConfig.Active ? ImageKey.Line : ImageKey.LineInactive);
            lineNode.ContextMenuStrip = menuControl.LineMenu;
            lineNode.Tag = new CommNodeTag(commApp, lineConfig, CommNodeType.Line);

            lineNode.Nodes.AddRange(new TreeNode[]
            {
                new TreeNode(ExtensionPhrases.LineOptionsNode)
                {
                    ImageKey = ImageKey.Options,
                    SelectedImageKey = ImageKey.Options,
                    Tag = new CommNodeTag(commApp, null, CommNodeType.LineOptions)
                    {
                        FormType = typeof(FrmLineConfig),
                        FormArgs = new object[] { adminContext, commApp, lineConfig }
                    }
                },
                new TreeNode(ExtensionPhrases.LineStatsNode)
                {
                    ImageKey = ImageKey.Stats,
                    SelectedImageKey = ImageKey.Stats,
                    Tag = new CommNodeTag(commApp, null, CommNodeType.LineStats)
                    {
                        FormType = typeof(FrmLineStats),
                        FormArgs = new object[] { adminContext, lineConfig }
                    }
                }
            });

            foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
            {
                lineNode.Nodes.Add(CreateDeviceNode(commApp, lineConfig, deviceConfig));
            }

            return lineNode;
        }
        
        /// <summary>
        /// Updates the node that represents a communication line and its child nodes.
        /// </summary>
        public void UpdateLineNode(TreeNode lineNode)
        {
            if (lineNode == null)
                throw new ArgumentNullException(nameof(lineNode));

            try
            {
                lineNode.TreeView?.BeginUpdate();

                // update line text and image
                CommNodeTag nodeTag = (CommNodeTag)lineNode.Tag;
                LineConfig lineConfig = (LineConfig)nodeTag.RelatedObject;
                lineNode.Text = CommUtils.GetLineTitle(lineConfig.CommLineNum, lineConfig.Name);
                lineNode.SetImageKey(lineConfig.Active ? ImageKey.Line : ImageKey.LineInactive);

                // remove existing device nodes
                foreach (TreeNode lineSubnode in new ArrayList(lineNode.Nodes))
                {
                    if (lineSubnode.TagIs(CommNodeType.Device))
                        lineSubnode.Remove();
                }

                // add new device nodes
                foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
                {
                    lineNode.Nodes.Add(CreateDeviceNode(nodeTag.CommApp, lineConfig, deviceConfig));
                }
            }
            finally
            {
                lineNode.TreeView?.EndUpdate();
            }
        }

        /// <summary>
        /// Creates tree nodes for the explorer tree.
        /// </summary>
        public TreeNode[] CreateTreeNodes(CommApp commApp)
        {
            if (commApp == null)
                throw new ArgumentNullException(nameof(commApp));

            return new TreeNode[]
            {
                new TreeNode(ExtensionPhrases.GeneralOptionsNode)
                {
                    ImageKey = ImageKey.Options,
                    SelectedImageKey = ImageKey.Options,
                    Tag = new CommNodeTag(commApp, null, CommNodeType.GeneralOptions)
                    {
                        FormType = typeof(FrmGeneralOptions),
                        FormArgs = new object[] { adminContext.ErrLog, commApp }
                    }
                },
                new TreeNode(ExtensionPhrases.DriversNode)
                {
                    ImageKey = ImageKey.Driver,
                    SelectedImageKey = ImageKey.Driver,
                    Tag = new CommNodeTag(commApp, null, CommNodeType.Drivers)
                    {
                        FormType = typeof(FrmDrivers),
                        FormArgs = new object[] { adminContext, commApp }
                    }
                },
                new TreeNode(ExtensionPhrases.DataSourcesNode)
                {
                    ImageKey = ImageKey.DataSource,
                    SelectedImageKey = ImageKey.DataSource,
                    Tag = new CommNodeTag(commApp, null, CommNodeType.DataSources)
                    {
                        FormType = typeof(FrmDataSources),
                        FormArgs = new object[] { adminContext, commApp }
                    }
                },
                CreateLinesNode(commApp)
            };
        }
    }
}
