/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Administrator
 * Summary  : Manipulates the explorer tree
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2022
 */

using Scada.Admin.App.Forms.Tables;
using Scada.Admin.Lang;
using Scada.Admin.Project;
using Scada.Data.Entities;
using Scada.Data.Tables;
using Scada.Forms;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Manipulates the explorer tree.
    /// <para>Манипулирует деревом проводника.</para>
    /// </summary>
    internal class ExplorerBuilder
    {
        private readonly AppData appData;           // the common data of the application
        private readonly TreeView treeView;         // the manipulated tree view 
        private readonly ContextMenus contextMenus; // references to the context menus
        private ScadaProject project;               // the current project to build tree


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExplorerBuilder(AppData appData, TreeView treeView, ContextMenus contextMenus)
        {
            this.appData = appData ?? throw new ArgumentNullException(nameof(appData));
            this.treeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
            this.contextMenus = contextMenus ?? throw new ArgumentNullException(nameof(contextMenus));
            project = null;

            ProjectNode = null;
            BaseNode = null;
            BaseTableNodes = null;
            ViewsNode = null;
            InstancesNode = null;
        }


        /// <summary>
        /// Gets the project node.
        /// </summary>
        public TreeNode ProjectNode { get; private set; }

        /// <summary>
        /// Gets the configuration database node.
        /// </summary>
        public TreeNode BaseNode { get; private set; }

        /// <summary>
        /// Gets the configuration database table nodes accesed by table name (item type name).
        /// </summary>
        public Dictionary<string, TreeNode> BaseTableNodes { get; private set; }

        /// <summary>
        /// Gets the views node.
        /// </summary>
        public TreeNode ViewsNode { get; private set; }

        /// <summary>
        /// Gets the instances node.
        /// </summary>
        public TreeNode InstancesNode { get; private set; }


        /// <summary>
        /// Creates a node that represents the configuration database.
        /// </summary>
        private TreeNode CreateBaseNode(ConfigBase configBase)
        {
            TreeNode baseNode = TreeViewExtensions.CreateNode(AppPhrases.BaseNode, "database.png");
            baseNode.Tag = new TreeNodeTag(project.ConfigBase, ExplorerNodeType.Base);

            // primary tables sorted in the order they are configured
            TreeNode primaryNode = TreeViewExtensions.CreateNode(AppPhrases.PrimaryTablesNode, "folder_closed.png");
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.ObjTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.CommLineTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.DeviceTable));

            TreeNode cnlTableNode = CreateBaseTableNode(configBase.CnlTable);
            cnlTableNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            primaryNode.Nodes.Add(cnlTableNode);
            FillCnlTableNodeInternal(cnlTableNode, configBase);

            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.LimTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.ViewTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.RoleTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.RoleRefTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.ObjRightTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configBase.UserTable));
            baseNode.Nodes.Add(primaryNode);

            // secondary tables in alphabetical order
            TreeNode secondaryNode = TreeViewExtensions.CreateNode(AppPhrases.SecondaryTablesNode, "folder_closed.png");
            SortedList<string, TreeNode> secondaryNodes = new()
            {
                { configBase.ArchiveTable.Title, CreateBaseTableNode(configBase.ArchiveTable) },
                { configBase.CnlStatusTable.Title, CreateBaseTableNode(configBase.CnlStatusTable) },
                { configBase.CnlTypeTable.Title, CreateBaseTableNode(configBase.CnlTypeTable) },
                { configBase.DataTypeTable.Title, CreateBaseTableNode(configBase.DataTypeTable) },
                { configBase.DevTypeTable.Title, CreateBaseTableNode(configBase.DevTypeTable) },
                { configBase.FormatTable.Title, CreateBaseTableNode(configBase.FormatTable) },
                { configBase.QuantityTable.Title, CreateBaseTableNode(configBase.QuantityTable) },
                { configBase.ScriptTable.Title, CreateBaseTableNode(configBase.ScriptTable) },
                { configBase.UnitTable.Title, CreateBaseTableNode(configBase.UnitTable) },
                { configBase.ViewTypeTable.Title, CreateBaseTableNode(configBase.ViewTypeTable) }
            };

            foreach (TreeNode tableNode in secondaryNodes.Values)
            {
                secondaryNode.Nodes.Add(tableNode);
            }

            baseNode.Nodes.Add(secondaryNode);
            return baseNode;
        }

        /// <summary>
        /// Creates a node that represents the table of the configuration database.
        /// </summary>
        private TreeNode CreateBaseTableNode(IBaseTable baseTable)
        {
            TreeNode baseTableNode = TreeViewExtensions.CreateNode(baseTable.Title, "table.png");
            baseTableNode.Tag = CreateBaseTableTag(baseTable);
            BaseTableNodes.Add(baseTable.Name, baseTableNode);
            return baseTableNode;
        }

        /// <summary>
        /// Creates a tag to associate with a tree node representing a table.
        /// </summary>
        private TreeNodeTag CreateBaseTableTag(IBaseTable baseTable, TableFilter tableFilter = null)
        {
            return new TreeNodeTag
            {
                FormType = typeof(FrmBaseTable),
                FormArgs = new object[] { baseTable, tableFilter, project, appData },
                RelatedObject = new BaseTableItem(baseTable, tableFilter),
                NodeType = ExplorerNodeType.BaseTable
            };
        }

        /// <summary>
        /// Fills the channel table nodes.
        /// </summary>
        private void FillCnlTableNodeInternal(TreeNode cnlTableNode, ConfigBase configBase)
        {
            foreach (Device device in configBase.DeviceTable.EnumerateItems())
            {
                string nodeText = string.Format(CommonPhrases.EntityCaption, device.DeviceNum, device.Name);
                TreeNode cnlsByDeviceNode = TreeViewExtensions.CreateNode(nodeText, "table.png");
                cnlsByDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
                cnlsByDeviceNode.Tag = CreateBaseTableTag(configBase.CnlTable, CreateFilterByDevice(device));
                cnlTableNode.Nodes.Add(cnlsByDeviceNode);
            }

            TreeNode cnlsEmptyDeviceNode = TreeViewExtensions.CreateNode(AdminPhrases.EmptyDevice, "table.png");
            cnlsEmptyDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            cnlsEmptyDeviceNode.Tag = CreateBaseTableTag(configBase.CnlTable, CreateFilterByDevice(null));
            cnlTableNode.Nodes.Add(cnlsEmptyDeviceNode);
        }

        /// <summary>
        /// Creates a node that represents the specified application.
        /// </summary>
        private TreeNode CreateAppNode(ProjectApp projectApp, string imageKey)
        {
            TreeNode appNode = TreeViewExtensions.CreateNode(projectApp.AppName, imageKey);
            appNode.ContextMenuStrip = contextMenus.AppMenu;
            appNode.Tag = new TreeNodeTag(projectApp, ExplorerNodeType.App);
            appNode.Nodes.AddRange(appData.ExtensionHolder.GetTreeNodes(projectApp).ToArray());
            appNode.Nodes.Add(CreateAppConfigNode(projectApp));
            return appNode;
        }

        /// <summary>
        /// Creates a node that contains the application configuration files.
        /// </summary>
        private TreeNode CreateAppConfigNode(ProjectApp projectApp)
        {
            TreeNode appConfigNode = TreeViewExtensions.CreateNode(AppPhrases.AppConfigNode, "folder_closed.png");
            appConfigNode.ContextMenuStrip = contextMenus.DirectoryMenu;
            appConfigNode.Tag = new TreeNodeTag(projectApp, ExplorerNodeType.AppConfig);
            appConfigNode.Nodes.Add(CreateEmptyNode());
            return appConfigNode;
        }

        /// <summary>
        /// Creates a node that represents the specified directory.
        /// </summary>
        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            TreeNode directoryNode = TreeViewExtensions.CreateNode(directoryInfo.Name, "folder_closed.png");
            directoryNode.ContextMenuStrip = contextMenus.DirectoryMenu;
            directoryNode.Tag = new TreeNodeTag(new FileItem(directoryInfo), ExplorerNodeType.Directory);
            return directoryNode;
        }

        /// <summary>
        /// Creates a node that represents the specified file.
        /// </summary>
        private TreeNode CreateFileNode(FileInfo fileInfo)
        {
            TreeNode fileNode = TreeViewExtensions.CreateNode(fileInfo.Name, "file.png");
            fileNode.ContextMenuStrip = contextMenus.FileItemMenu;
            fileNode.Tag = new TreeNodeTag(new FileItem(fileInfo), ExplorerNodeType.File);
            return fileNode;
        }

        /// <summary>
        /// Fills the tree node according to the file system.
        /// </summary>
        private void FillFileNodeInternal(TreeNode treeNode, DirectoryInfo directoryInfo)
        {
            foreach (DirectoryInfo subdirInfo in directoryInfo.EnumerateDirectories())
            {
                TreeNode subdirNode = CreateDirectoryNode(subdirInfo);
                FillFileNodeInternal(subdirNode, subdirInfo);
                treeNode.Nodes.Add(subdirNode);
            }

            foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles())
            {
                TreeNode fileNode = CreateFileNode(fileInfo);
                treeNode.Nodes.Add(fileNode);
            }
        }

        /// <summary>
        /// Creates an empty node.
        /// </summary>
        private static TreeNode CreateEmptyNode()
        {
            return TreeViewExtensions.CreateNode(CommonPhrases.EmptyData, "empty.png");
        }

        /// <summary>
        /// Creates a table filter for filtering by device.
        /// </summary>
        private static TableFilter CreateFilterByDevice(Device device)
        {
            return device == null
                ? new TableFilter("DeviceNum", null)
                {
                    Title = AppPhrases.EmptyDeviceFilter
                }
                : new TableFilter("DeviceNum", device.DeviceNum)
                {
                    Title = string.Format(AppPhrases.DeviceFilter, device.DeviceNum)
                };
        }


        /// <summary>
        /// Creates tree nodes according to the project structure.
        /// </summary>
        public void CreateNodes(ScadaProject project)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            ProjectNode = null;
            BaseNode = null;
            BaseTableNodes = new Dictionary<string, TreeNode>();
            ViewsNode = null;
            InstancesNode = null;

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();

                // project node
                ProjectNode = TreeViewExtensions.CreateNode(project.Name, "project.png", true);
                ProjectNode.ContextMenuStrip = contextMenus.ProjectMenu;
                ProjectNode.Tag = new TreeNodeTag(project, ExplorerNodeType.Project);
                treeView.Nodes.Add(ProjectNode);

                // configuration database node
                BaseNode = CreateBaseNode(project.ConfigBase);
                ProjectNode.Nodes.Add(BaseNode);

                // views node
                ViewsNode = TreeViewExtensions.CreateNode(AppPhrases.ViewsNode, "views.png");
                ViewsNode.ContextMenuStrip = contextMenus.DirectoryMenu;
                ViewsNode.Tag = new TreeNodeTag(project.Views, ExplorerNodeType.Views);
                ViewsNode.Nodes.Add(CreateEmptyNode());
                ProjectNode.Nodes.Add(ViewsNode);

                // instances node
                InstancesNode = TreeViewExtensions.CreateNode(AppPhrases.InstancesNode, "instances.png");
                InstancesNode.ContextMenuStrip = contextMenus.InstanceMenu;
                InstancesNode.Tag = new TreeNodeTag(project.Instances, ExplorerNodeType.Instances);
                project.Instances.ForEach(i => InstancesNode.Nodes.Add(CreateInstanceNode(i)));
                ProjectNode.Nodes.Add(InstancesNode);

                ProjectNode.Expand();
                InstancesNode.Expand();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a node that represents the specified instance.
        /// </summary>
        public TreeNode CreateInstanceNode(ProjectInstance projectInstance)
        {
            TreeNode instanceNode = TreeViewExtensions.CreateNode(projectInstance.Name, "instance.png");
            instanceNode.ContextMenuStrip = contextMenus.InstanceMenu;
            instanceNode.Tag = new TreeNodeTag(new LiveInstance(projectInstance), ExplorerNodeType.Instance);
            instanceNode.Nodes.Add(CreateEmptyNode());
            return instanceNode;
        }

        /// <summary>
        /// Fills the channel table node, creating child nodes.
        /// </summary>
        public void FillCnlTableNode(TreeNode cnlTableNode, ConfigBase configBase)
        {
            try
            {
                treeView.BeginUpdate();
                cnlTableNode.Nodes.Clear();
                FillCnlTableNodeInternal(cnlTableNode, configBase);
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the views node.
        /// </summary>
        public void FillViewsNode(TreeNode viewsNode)
        {
            if (viewsNode?.GetRelatedObject() is ProjectViews projectViews)
                FillFileNode(viewsNode, projectViews.ViewDir);
        }

        /// <summary>
        /// Fills the instances node.
        /// </summary>
        public void FillInstancesNode()
        {
            if (InstancesNode == null)
                return;

            try
            {
                treeView.BeginUpdate();
                InstancesNode.Nodes.Clear();
                project.Instances.ForEach(i => InstancesNode.Nodes.Add(CreateInstanceNode(i)));
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the instance node.
        /// </summary>
        public void FillInstanceNode(TreeNode instanceNode)
        {
            if (instanceNode?.GetRelatedObject() is not LiveInstance liveInstance)
                return;

            try
            {
                treeView.BeginUpdate();
                instanceNode.Nodes.Clear();
                ProjectInstance projectInstance = liveInstance.ProjectInstance;

                if (projectInstance.ServerApp.Enabled)
                    instanceNode.Nodes.Add(CreateAppNode(projectInstance.ServerApp, "server.png"));

                if (projectInstance.CommApp.Enabled)
                    instanceNode.Nodes.Add(CreateAppNode(projectInstance.CommApp, "comm.png"));

                if (projectInstance.WebApp.Enabled)
                    instanceNode.Nodes.Add(CreateAppNode(projectInstance.WebApp, "chrome.png"));
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the application node.
        /// </summary>
        public void FillAppNode(TreeNode appNode)
        {
            if (appNode?.GetRelatedObject() is not ProjectApp projectApp)
                return;

            try
            {
                treeView.BeginUpdate();
                appNode.Nodes.Clear();
                appNode.Nodes.AddRange(appData.ExtensionHolder.GetTreeNodes(projectApp).ToArray());
                appNode.Nodes.Add(CreateAppConfigNode(projectApp));
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Fills the application configuration node.
        /// </summary>
        public void FillAppConfigNode(TreeNode appConfigNode)
        {
            if (appConfigNode?.GetRelatedObject() is ProjectApp projectApp)
                FillFileNode(appConfigNode, projectApp.ConfigDir);
        }

        /// <summary>
        /// Fills the tree node according to the file system.
        /// </summary>
        public void FillFileNode(TreeNode treeNode, string directory)
        {
            try
            {
                treeView.BeginUpdate();
                treeNode.Nodes.Clear();
                FillFileNodeInternal(treeNode, new DirectoryInfo(directory));
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Inserts a newly created directory node.
        /// </summary>
        public void InsertDirectoryNode(TreeNode parentNode, string directory)
        {
            try
            {
                treeView.BeginUpdate();
                DirectoryInfo directoryInfo = new(directory);
                string name = directoryInfo.Name;
                int index = 0;

                foreach (TreeNode treeNode in parentNode.Nodes)
                {
                    if (treeNode.TagIs(ExplorerNodeType.Directory))
                    {
                        if (string.Compare(name, treeNode.Text, StringComparison.Ordinal) < 0)
                            break;
                        else
                            index = treeNode.Index + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                TreeNode directoryNode = CreateDirectoryNode(directoryInfo);
                parentNode.Nodes.Insert(index, directoryNode);
                treeView.SelectedNode = directoryNode;
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Inserts a newly created file node.
        /// </summary>
        public void InsertFileNode(TreeNode parentNode, string fileName)
        {
            try
            {
                treeView.BeginUpdate();
                FileInfo fileInfo = new(fileName);
                string name = fileInfo.Name;
                int index = 0;

                foreach (TreeNode treeNode in parentNode.Nodes)
                {
                    if (treeNode.TagIs(ExplorerNodeType.Directory))
                    {
                        index = treeNode.Index + 1;
                    }
                    else if (treeNode.TagIs(ExplorerNodeType.File))
                    {
                        if (string.Compare(name, treeNode.Text, StringComparison.Ordinal) < 0)
                            break;
                        else
                            index = treeNode.Index + 1;
                    }
                    else
                    {
                        index = treeNode.Index;
                        break;
                    }
                }

                TreeNode fileNode = CreateFileNode(fileInfo);
                parentNode.Nodes.Insert(index, fileNode);
                treeView.SelectedNode = fileNode;
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Sets the node image as open or closed folder.
        /// </summary>
        public static void SetFolderImage(TreeNode treeNode)
        {
            if (treeNode.ImageKey.StartsWith("folder_"))
                treeNode.SetImageKey(treeNode.IsExpanded ? "folder_open.png" : "folder_closed.png");
        }
    }
}
