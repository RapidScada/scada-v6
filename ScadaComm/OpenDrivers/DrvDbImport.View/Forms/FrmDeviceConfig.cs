// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Comm.Drivers.DrvDbImport.View.Properties;
using Scada.Data.Entities;
using Scada.Forms;
using Scada.Comm.Channels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Scada.Config;
using Scada.Forms.Controls;

namespace Scada.Comm.Drivers.DrvDbImport.View.Forms
{
    /// <summary>
    /// Represents a device configuration form.
    /// <para>Представляет форму конфигурации устройства.</para>
    /// </summary>
    public partial class FrmDeviceConfig : Form
    {
        /// <summary>
        /// Specifies the image keys.
        /// </summary>
        private static class ImageKey
        {
            /*public const string Command = "cmd.png";
            public const string Empty = "empty.png";*/
            public const string Connect = "connect.png";
            public const string FolderClosed = "folder_closed.png";
            public const string Query = "query.png";
            /*public const string FolderOpen = "folder_open.png";
            public const string Method = "method.png";
            public const string Object = "obj.png";
            public const string Variable = "variable.png";*/
        }


        private readonly AppDirs appDirs;                // the application directories
        private readonly int lineNum;                    // the communication line number
        private readonly int deviceNum;                   // the device number
        private readonly DbLineConfig lineConfig;         // the communication line configuration
        private readonly DbDeviceConfig deviceConfig;     // the device configuration
        
        private string lineConfigFileName;                // the line configuration file name
        private string deviceConfigFileName;              // the configuration file name
        private bool lineConfigModified;                  // the line configuration has been modified
        private bool deviceConfigModified;                // the device configuration has been modified        
        private DbLineConfig lineConfigCopy;              // the configuration copy to revert changes
        private DbDeviceConfig deviceConfigCopy;          // the configuration copy to revert changes

        private TreeNode connectionNode;                  // the tree node of the connection
        private TreeNode queriesNode;                     // the tree node of the queries
        private TreeNode commandsNode;                    // the tree node of the commands


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceConfig()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FrmDeviceConfig(AppDirs appDirs, int lineNum, int deviceNum)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.lineNum = lineNum;
            this.deviceNum = deviceNum;
            lineConfig = new DbLineConfig();
            deviceConfig = new DbDeviceConfig();
            lineConfigCopy = null;
            deviceConfigCopy = null;

            lineConfigFileName = "";
            deviceConfigFileName = "";
            lineConfigModified = false;
            deviceConfigModified = false;
            connectionNode = null;
            queriesNode = null;
            commandsNode = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the line configuration has been modified.
        /// </summary>
        private bool LineConfigModified
        {
            get
            {
                return lineConfigModified;
            }
            set
            {
                lineConfigModified = value;
                btnSave.Enabled = Modified;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the device configuration has been modified.
        /// </summary>
        private bool DeviceConfigModified
        {
            get
            {
                return deviceConfigModified;
            }
            set
            {
                deviceConfigModified = value;
                btnSave.Enabled = Modified;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration has been modified.
        /// </summary>
        private bool Modified => LineConfigModified || DeviceConfigModified;

        
        /// <summary>
        /// Takes the tree view images and loads them into an image list.
        /// </summary>
        private void TakeTreeViewImages()
        {
            // loading images from resources instead of storing in image list prevents them from corruption
            ilTree.Images.Add(ImageKey.Connect, Resource.connect);
            //ilTree.Images.Add(ImageKey.Empty, Resources.empty);
            ilTree.Images.Add(ImageKey.FolderClosed, Resource.folder_closed);
            ilTree.Images.Add(ImageKey.Query, Resource.query);
            //ilTree.Images.Add(ImageKey.FolderOpen, Resources.folder_open);
            /*ilTree.Images.Add(ImageKey.Method, Resources.method);
            ilTree.Images.Add(ImageKey.Object, Resources.obj);
            ilTree.Images.Add(ImageKey.Variable, Resources.variable);*/
        }

        /// <summary>
        /// Hides the controls that display.
        /// </summary>
        private void HideControls()
        {
            /*ctrlDbConnection.Visible = ctrlQuery.Visible =
                ctrlCommand.Visible = lblHint.Visible = false;*/
        }

        /// <summary>
        /// Enables or disables the buttons.
        /// </summary>
        private void SetButtonsEnabled()
        {
            TreeNode selectedNode = tvDevice.SelectedNode;

            btnMoveUp.Enabled = tvDevice.MoveUpSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent) &&
                (tvDevice.SelectedNode.Tag is QueryConfig || tvDevice.SelectedNode.Tag is CommandConfig);

            btnMoveDown.Enabled = tvDevice.MoveDownSelectedNodeIsEnabled(TreeNodeBehavior.WithinParent) &&
                (tvDevice.SelectedNode.Tag is QueryConfig || tvDevice.SelectedNode.Tag is CommandConfig);

            btnDelete.Enabled = selectedNode != null &&
                (tvDevice.SelectedNode.Tag is QueryConfig || tvDevice.SelectedNode.Tag is CommandConfig);

            btnAddQuery.Enabled = btnAddCommand.Enabled = selectedNode != null;
        }

        /// <summary>
        /// Fills the device tree.
        /// </summary>
        private void FillDeviceTree()
        {
            try
            {
                tvDevice.BeginUpdate();
                tvDevice.Nodes.Clear();

                connectionNode = TreeViewExtensions.CreateNode(DriverPhrases.ConnectionNode, ImageKey.Connect);
                queriesNode = TreeViewExtensions.CreateNode(DriverPhrases.QueriesNode, ImageKey.Query);
                commandsNode = TreeViewExtensions.CreateNode(DriverPhrases.CommandsNode, ImageKey.FolderClosed);

                tvDevice.Nodes.Add(connectionNode);
                connectionNode.Expand();
                tvDevice.Nodes.Add(queriesNode);
                queriesNode.Expand();
                
                foreach (QueryConfig queryConfig in deviceConfig.Queries)
                {
                    queriesNode.Nodes.Add(TreeViewExtensions.CreateNode(queryConfig.Name,
                         "", queryConfig));
                }

                tvDevice.Nodes.Add(commandsNode);
                commandsNode.Expand();

                foreach (CommandConfig commandConfig in deviceConfig.Commands)
                {
                    commandsNode.Nodes.Add(TreeViewExtensions.CreateNode(commandConfig.Name,
                         "", commandConfig));
                }

                if (tvDevice.Nodes.Count > 0)
                {
                    tvDevice.Nodes[0].ExpandAll();
                    tvDevice.SelectedNode = tvDevice.Nodes[0].FirstNode;
                }
                else
                {
                    SetButtonsEnabled();
                    HideControls();
                    lblHint.Visible = true;
                }
            }
            finally
            {
                tvDevice.EndUpdate();
            }
        }


        private void FrmDeviceConfig_Load(object sender, EventArgs e)
        {
            // translate form
            FormTranslator.Translate(this, GetType().FullName, 
                new FormTranslatorOptions { ContextMenus = new ContextMenuStrip[] { cmsTree } });

            // load configuration
            lineConfigFileName = Path.Combine(appDirs.ConfigDir, DbLineConfig.GetFileName(lineNum));
            deviceConfigFileName = Path.Combine(appDirs.ConfigDir, DbDeviceConfig.GetFileName(deviceNum));

            if (File.Exists(lineConfigFileName) && !lineConfig.Load(lineConfigFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            if (File.Exists(deviceConfigFileName) && !deviceConfig.Load(deviceConfigFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            lineConfigCopy = lineConfig.DeepClone();
            deviceConfigCopy = deviceConfig.DeepClone();

            // display configuration
            TakeTreeViewImages();
            FillDeviceTree();
            LineConfigModified = false;
            DeviceConfigModified = false;
        }


        private void tvDevice_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetButtonsEnabled();
        }

        private void btnAddQuery_Click(object sender, EventArgs e)
        {
            if (queriesNode != null)
            {
                queriesNode.Expand();

                // add query
                QueryConfig queryConfig = new()
                {
                    Name = string.Format(DriverPhrases.QueryName, queriesNode.GetNodeCount(true) + 1)
                };

                tvDevice.Insert(queriesNode, TreeViewExtensions.CreateNode(queryConfig.Name,
                    "", queryConfig), deviceConfig.Queries, queryConfig);
                
                deviceConfigModified = true;
                //ctrlQuery.SetFocus();
            }
        }

        private void btnAddCommand_Click(object sender, EventArgs e)
        {
            if (commandsNode != null)
            {
                commandsNode.Expand();

                // add command
                CommandConfig commandConfig = new()
                {
                    Name = string.Format(DriverPhrases.CommandName, commandsNode.GetNodeCount(true) + 1)
                };

                tvDevice.Insert(commandsNode, TreeViewExtensions.CreateNode(commandConfig.Name,
                    "", commandConfig), deviceConfig.Commands, commandConfig);

                deviceConfigModified = true;
                //ctrlCommand.SetFocus();
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            tvDevice.MoveUpSelectedNode(TreeNodeBehavior.WithinParent);
            deviceConfigModified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            tvDevice.MoveDownSelectedNode(TreeNodeBehavior.WithinParent);
            deviceConfigModified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            tvDevice.RemoveSelectedNode();
            deviceConfigModified = true;

            if (tvDevice.Nodes.Count == 0)
            {
                SetButtonsEnabled();
                HideControls();
                lblHint.Visible = true;
                lblHint.Text = DriverPhrases.AddRoots;
            }
        }

        private void miCollapseAll_Click(object sender, EventArgs e)
        {
            if (tvDevice.Nodes.Count > 0)
            {
                tvDevice.SelectedNode = null;
                tvDevice.CollapseAll();
                tvDevice.SelectedNode = tvDevice.Nodes[0];
            }
        }
    }
}
