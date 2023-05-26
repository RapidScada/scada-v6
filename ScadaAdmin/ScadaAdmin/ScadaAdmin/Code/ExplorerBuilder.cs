/*
 * Copyright 2023 Rapid Software LLC
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
using Scada.Config;
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
            ConfigDatabaseNode = null;
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
        public TreeNode ConfigDatabaseNode { get; private set; }

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
        private TreeNode CreateConfigDatabaseNode(ConfigDatabase configDatabase)
        {
            TreeNode configDatabaseNode = TreeViewExtensions.CreateNode(AppPhrases.ConfigDatabaseNode, "database.png");
            configDatabaseNode.Tag = new TreeNodeTag(project.ConfigDatabase, ExplorerNodeType.ConfigDatabase);

            // primary tables sorted in the order they are configured
            TreeNode primaryNode = TreeViewExtensions.CreateNode(AppPhrases.PrimaryTablesNode, "folder_closed.png");
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.ObjTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.CommLineTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.DeviceTable));

            TreeNode cnlTableNode = CreateBaseTableNode(configDatabase.CnlTable);
            cnlTableNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            primaryNode.Nodes.Add(cnlTableNode);
            FillCnlTableNodeInternal(cnlTableNode, configDatabase);
            if (appData.AppConfig.subFolderEnabled) FillCnlTableNodesByObjects(cnlTableNode, configDatabase);

            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.LimTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.ViewTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.RoleTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.RoleRefTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.ObjRightTable));
            primaryNode.Nodes.Add(CreateBaseTableNode(configDatabase.UserTable));
            configDatabaseNode.Nodes.Add(primaryNode);

            // secondary tables in alphabetical order
            TreeNode secondaryNode = TreeViewExtensions.CreateNode(AppPhrases.SecondaryTablesNode, "folder_closed.png");
            SortedList<string, TreeNode> secondaryNodes = new()
            {
                { configDatabase.ArchiveTable.Title, CreateBaseTableNode(configDatabase.ArchiveTable) },
                { configDatabase.ArchiveKindTable.Title, CreateBaseTableNode(configDatabase.ArchiveKindTable) },
                { configDatabase.CnlStatusTable.Title, CreateBaseTableNode(configDatabase.CnlStatusTable) },
                { configDatabase.CnlTypeTable.Title, CreateBaseTableNode(configDatabase.CnlTypeTable) },
                { configDatabase.DataTypeTable.Title, CreateBaseTableNode(configDatabase.DataTypeTable) },
                { configDatabase.DevTypeTable.Title, CreateBaseTableNode(configDatabase.DevTypeTable) },
                { configDatabase.FormatTable.Title, CreateBaseTableNode(configDatabase.FormatTable) },
                { configDatabase.QuantityTable.Title, CreateBaseTableNode(configDatabase.QuantityTable) },
                { configDatabase.ScriptTable.Title, CreateBaseTableNode(configDatabase.ScriptTable) },
                { configDatabase.UnitTable.Title, CreateBaseTableNode(configDatabase.UnitTable) },
                { configDatabase.ViewTypeTable.Title, CreateBaseTableNode(configDatabase.ViewTypeTable) }
            };

            foreach (TreeNode tableNode in secondaryNodes.Values)
            {
                secondaryNode.Nodes.Add(tableNode);
            }

            configDatabaseNode.Nodes.Add(secondaryNode);
            return configDatabaseNode;
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
        private void FillCnlTableNodeInternal(TreeNode cnlTableNode, ConfigDatabase configDatabase)
        {
            foreach (Device device in configDatabase.DeviceTable.EnumerateItems())
            {
                string nodeText = string.Format(CommonPhrases.EntityCaption, device.DeviceNum, device.Name);
                TreeNode cnlsByDeviceNode = TreeViewExtensions.CreateNode(nodeText, "table.png");
                cnlsByDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
                cnlsByDeviceNode.Tag = CreateBaseTableTag(configDatabase.CnlTable, CreateFilterByDevice(device));
                cnlTableNode.Nodes.Add(cnlsByDeviceNode);

            }

            TreeNode cnlsEmptyDeviceNode = TreeViewExtensions.CreateNode(AdminPhrases.EmptyDevice, "table.png");
            cnlsEmptyDeviceNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            cnlsEmptyDeviceNode.Tag = CreateBaseTableTag(configDatabase.CnlTable, CreateFilterByDevice(null));
            cnlTableNode.Nodes.Add(cnlsEmptyDeviceNode);
        }

        private void FillCnlTableNodesByObjects(TreeNode CnlTable, ConfigDatabase configDatabase)
        {
            TreeNode folderNode = TreeViewExtensions.CreateNode(AppPhrases.MainObjectFolder, "folder_closed.png");
            Dictionary<int, TreeNode> nodeList = new Dictionary<int, TreeNode>();
            if (HasParentChildLoopInObjects(configDatabase))
            {
                return;
            }
            foreach(Obj obj in configDatabase.ObjTable.Enumerate().Where(x => x.ParentObjNum == null))
            {
                TreeNode cnlsByObjectNode = addNode(configDatabase, obj);
                folderNode.Nodes.Add(cnlsByObjectNode);
            }

            CnlTable.Nodes.Add(folderNode);

        }

        private bool HasParentChildLoopInObjects(ConfigDatabase configDatabase)
        {
            List<int> objNums = new List<int>();
            if (configDatabase.ObjTable.Enumerate().Count() == 0) return false;
            if (configDatabase.ObjTable.Enumerate().Where(x => x.ParentObjNum == null).Count() == 0) 
            {
                MessageBox.Show(AppPhrases.InfiniteLoopNoParentError,"error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return true; 
            }
            while(objNums.Count != configDatabase.ObjTable.Count()){
                Obj obj = configDatabase.ObjTable.Enumerate().Where(x => !objNums.Contains(x.ObjNum)).FirstOrDefault();
                if (ChildrenNodeAlreadyChecked(obj, objNums, configDatabase))
                {
                    MessageBox.Show(AppPhrases.InfiniteLoopError+obj.Name, "error",MessageBoxButtons.OK,MessageBoxIcon.Error);

                    return true;
                }
            }

            return false;
        }
        private bool ChildrenNodeAlreadyChecked(Obj obj,List<int> objNums, ConfigDatabase configDatabase)
        {

            if (obj == null) return false;
            objNums.Add(obj.ObjNum);
            foreach (Obj item in configDatabase.ObjTable.Enumerate().Where(x=>x.ParentObjNum == obj.ObjNum))
            {
                if (objNums.Contains(item.ObjNum)) return true;
                return ChildrenNodeAlreadyChecked(item,objNums, configDatabase);
            }

            return false;
        }

        private TreeNode addNode(ConfigDatabase configDatabase, Obj obj)
        {
            
            string nodeText = string.Format(CommonPhrases.EntityCaption, obj.ObjNum, obj.Name);
            TreeNode cnlsByObjectNode = TreeViewExtensions.CreateNode(nodeText, "folder_closed.png");
            cnlsByObjectNode.ContextMenuStrip = contextMenus.CnlTableMenu;
            cnlsByObjectNode.Tag = CreateBaseTableTag(configDatabase.CnlTable, CreateFilterByObject(obj));
            if (hasChildrenObjects(obj, configDatabase))
            {
                foreach (Obj item in configDatabase.ObjTable.Enumerate().Where(x=>x.ParentObjNum==obj.ObjNum))
                {
                    cnlsByObjectNode.Nodes.Add(addNode(configDatabase, item));
                }
            }
            return cnlsByObjectNode;
        }

        private bool hasChildrenObjects (Obj obj, ConfigDatabase configDatabase)
        {
            foreach (Obj items in configDatabase.ObjTable.Enumerate())
            {
                if (items.ParentObjNum == obj.ObjNum) return true;
            }
            return false;
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
        private static TableFilter CreateFilterByObject(Obj obj)
        {
            TableFilter res = new TableFilter("ObjNum", obj.ObjNum)
            {
                Title = string.Format(AppPhrases.ObjectFilter, obj.ObjNum)
            };

            

            return res;

        }


        /// <summary>
        /// Creates tree nodes according to the project structure.
        /// </summary>
        public void CreateNodes(ScadaProject project)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
            ProjectNode = null;
            ConfigDatabaseNode = null;
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
                ConfigDatabaseNode = CreateConfigDatabaseNode(project.ConfigDatabase);
                ProjectNode.Nodes.Add(ConfigDatabaseNode);

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
        public void FillCnlTableNode(TreeNode cnlTableNode, ConfigDatabase configDatabase)
        {
            try
            {
                treeView.BeginUpdate();
                cnlTableNode.Nodes.Clear();
                FillCnlTableNodeInternal(cnlTableNode, configDatabase);

                if (appData.AppConfig.subFolderEnabled) FillCnlTableNodesByObjects(cnlTableNode, configDatabase);
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
