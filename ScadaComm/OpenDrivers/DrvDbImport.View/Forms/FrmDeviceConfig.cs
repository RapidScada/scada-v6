// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvDbImport.Config;
using Scada.Comm.Drivers.DrvDbImport.View.Properties;
using Scada.Comm.Lang;
using Scada.Dbms;
using Scada.Forms;
using Scada.Lang;

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
            public const string Command = "cmd.png";
            public const string Commands = "cmds.png";
            public const string Connect = "connect.png";
            public const string Queries = "queries.png";
            public const string Query = "query.png";
            public const string QueryInactive = "query_inactive.png";
        }


        private readonly AppDirs appDirs;                // the application directories
        private readonly int lineNum;                    // the communication line number
        private readonly int deviceNum;                  // the device number
        
        private DbLineConfig lineConfig;                  // the communication line configuration
        private DbDeviceConfig deviceConfig;              // the device configuration       
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
                btnCancel.Enabled = Modified;
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
            ilTree.Images.Add(ImageKey.Command, Resource.cmd);
            ilTree.Images.Add(ImageKey.Commands, Resource.cmds);
            ilTree.Images.Add(ImageKey.Connect, Resource.connect);
            ilTree.Images.Add(ImageKey.Queries, Resource.queries);
            ilTree.Images.Add(ImageKey.Query, Resource.query);
            ilTree.Images.Add(ImageKey.QueryInactive, Resource.query_inactive);
        }

        /// <summary>
        /// Hides the controls that display.
        /// </summary>
        private void HideControls()
        {
            ctrlDbConnection.Visible = ctrlQuery.Visible = ctrlCommand.Visible = lblHint.Visible = false;
        }

        /// <summary>
        /// Selects a node image key corresponding to the specified object.
        /// </summary>
        private static string ChooseNodeImage(object obj)
        {
            if (obj is DbConnectionOptions)
            {
                return ImageKey.Connect;
            }
            else if (obj is CommandConfig)
            {
                return ImageKey.Command;
            }
            else if (obj is QueryConfig queryConfig)
            {
                return queryConfig.Active ? ImageKey.Query : ImageKey.QueryInactive;
            }
            else
            {
                return "";
            }
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
        /// Checks queries name for uniqueness.
        /// </summary>
        private bool CheckQueryNamesUnique()
        {
            return deviceConfig.Queries.Count ==
                deviceConfig.Queries.DistinctBy(g => g.Name.ToLowerInvariant()).Count();
        }

        /// <summary>
        /// Checks command name for uniqueness.
        /// </summary>
        private bool CheckCommandNamesUnique()
        {
            return deviceConfig.Commands.Count ==
                deviceConfig.Commands.DistinctBy(g => g.Name.ToLowerInvariant()).Count();
        }

        /// <summary>
        /// Checks query names for empty value.
        /// </summary>
        private bool CheckQueryNamesEmpty()
        {
            return deviceConfig.Queries.Any(g => string.IsNullOrEmpty(g.Name));
        }

        /// <summary>
        /// Checks command names for empty value.
        /// </summary>
        private bool CheckCommandNamesEmpty()
        {
            return deviceConfig.Commands.Any(g => string.IsNullOrEmpty(g.Name));
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

                connectionNode = TreeViewExtensions.CreateNode(DriverPhrases.ConnectionNode, 
                    ChooseNodeImage(lineConfig.ConnectionOptions), lineConfig);
                queriesNode = TreeViewExtensions.CreateNode(DriverPhrases.QueriesNode, ImageKey.Queries);
                commandsNode = TreeViewExtensions.CreateNode(DriverPhrases.CommandsNode, ImageKey.Commands);

                tvDevice.Nodes.Add(connectionNode);
                tvDevice.Nodes.Add(queriesNode);
                
                foreach (QueryConfig queryConfig in deviceConfig.Queries)
                {
                    queriesNode.Nodes.Add(TreeViewExtensions.CreateNode(string.IsNullOrEmpty(queryConfig.Name) ?
                        DriverPhrases.UnnamedQuery : queryConfig.Name, 
                        ChooseNodeImage(queryConfig), queryConfig));
                }

                tvDevice.Nodes.Add(commandsNode);
                commandsNode.Expand();

                foreach (CommandConfig commandConfig in deviceConfig.Commands)
                {
                    commandsNode.Nodes.Add(TreeViewExtensions.CreateNode(string.IsNullOrEmpty(commandConfig.Name) ?
                        DriverPhrases.UnnamedCommand : commandConfig.Name, 
                        ChooseNodeImage(commandConfig), commandConfig));
                }

                if (tvDevice.Nodes.Count > 0)
                {
                    tvDevice.SelectedNode = tvDevice.Nodes[0];
                }
                else
                {
                    SetButtonsEnabled();
                    HideControls();
                }

                tvDevice.ExpandAll();
            }
            finally
            {
                tvDevice.EndUpdate();
            }
        }

        /// <summary>
        /// Saves the line and device configuration.
        /// </summary>
        private bool SaveConfig()
        {
            bool saveOK = true;

            // save line configuration
            if (LineConfigModified)
            {
                if (lineConfig.Save(lineConfigFileName, out string errMsg))
                {
                    LineConfigModified = false;
                }
                else
                {
                    saveOK = false;
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            // save device configuration
            if (DeviceConfigModified)
            {
                if (deviceConfig.Save(deviceConfigFileName, out string errMsg))
                {
                    DeviceConfigModified = false;
                }
                else
                {
                    saveOK = false;
                    ScadaUiUtils.ShowError(errMsg);
                }
            }

            return saveOK;
        }


        private void FrmDeviceConfig_Load(object sender, EventArgs e)
        {
            // translate form
            FormTranslator.Translate(this, GetType().FullName, 
                new FormTranslatorOptions { ContextMenus = new ContextMenuStrip[] { cmsTree } });

            FormTranslator.Translate(ctrlCommand, ctrlCommand.GetType().FullName,
                new FormTranslatorOptions { ToolTip = ctrlQuery.ToolTip, SkipUserControls = false });
            FormTranslator.Translate(ctrlDbConnection, ctrlDbConnection.GetType().FullName);
            FormTranslator.Translate(ctrlQuery, ctrlQuery.GetType().FullName,
                new FormTranslatorOptions { ToolTip = ctrlQuery.ToolTip, SkipUserControls = false });
            Text = string.Format(Text, deviceNum);

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

        private void FrmDeviceConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modified)
            {
                DialogResult result = MessageBox.Show(CommPhrases.SaveDeviceConfigConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        e.Cancel = !SaveConfig();
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }


        private void tvDevice_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // show properties of the selected object
            object selectedObject = e.Node.Tag;
            HideControls();

            if (selectedObject is DbLineConfig dbLineConfig)
            {
                ctrlDbConnection.ConnectionOptions = dbLineConfig.ConnectionOptions;
                ctrlDbConnection.Visible = true;
            }
            else if (selectedObject is QueryConfig queryConfig)
            {
                ctrlQuery.QueryConfig = queryConfig;
                ctrlQuery.Visible = true;
            }
            else if (selectedObject is CommandConfig commandConfig)
            {
                ctrlCommand.CommandConfig = commandConfig;
                ctrlCommand.Visible = true;
            }
            else 
            {
                lblHint.Visible = true;
                lblHint.Text = DriverPhrases.SelectChildNode;
            }
            
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
                    ChooseNodeImage(queryConfig), queryConfig), deviceConfig.Queries, queryConfig);
                
                DeviceConfigModified = true;
                ctrlQuery.SetFocus();
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
                    ChooseNodeImage(commandConfig), commandConfig), deviceConfig.Commands, commandConfig);

                DeviceConfigModified = true;
                ctrlCommand.SetFocus();
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            // move up the selected item
            object deviceNodeTag = tvDevice.SelectedNode?.Tag;

            if (deviceNodeTag is QueryConfig)
            {
                tvDevice.MoveUpSelectedNode(deviceConfig.Queries);
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.MoveUpSelectedNode(deviceConfig.Commands);
            }
            
            DeviceConfigModified = true;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            // move down the selected item
            object deviceNodeTag = tvDevice.SelectedNode?.Tag;

            if (deviceNodeTag is QueryConfig)
            {
                tvDevice.MoveDownSelectedNode(deviceConfig.Queries);
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.MoveDownSelectedNode(deviceConfig.Commands);
            }

            DeviceConfigModified = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // delete the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = selectedNode?.Tag;

            if (deviceNodeTag is QueryConfig)
            {
                tvDevice.RemoveNode(selectedNode, deviceConfig.Queries);
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.RemoveNode(selectedNode, deviceConfig.Commands);
            }
            
            DeviceConfigModified = true;

            if (tvDevice.Nodes.Count == 0)
            {
                SetButtonsEnabled();
                HideControls();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // cancel configuration changes
            lineConfig = lineConfigCopy;
            deviceConfig = deviceConfigCopy;
            lineConfigCopy = lineConfig.DeepClone();
            deviceConfigCopy = deviceConfig.DeepClone();
            FillDeviceTree();
            LineConfigModified = false;
            DeviceConfigModified = false;
        }

        private void ctrlQuery_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (tvDevice.SelectedNode != null &&
               (e.ChangeArgument is not TreeUpdateTypes treeUpdateTypes ||
               treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode)) &&
               e.ChangedObject is QueryConfig queryConfig)
            {
                tvDevice.SelectedNode.Text = string.IsNullOrEmpty(queryConfig.Name) ?
                    DriverPhrases.UnnamedQuery : queryConfig.Name;
                tvDevice.SelectedNode.SetImageKey(ChooseNodeImage(tvDevice.SelectedNode.Tag));
            }

            DeviceConfigModified = true;
        }

        private void ctrlCommand_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (tvDevice.SelectedNode != null &&
              (e.ChangeArgument is not TreeUpdateTypes treeUpdateTypes ||
              treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode)) &&
              e.ChangedObject is CommandConfig commandConfig)
            {
                tvDevice.SelectedNode.Text = string.IsNullOrEmpty(commandConfig.Name) ? 
                    DriverPhrases.UnnamedCommand : commandConfig.Name;
                tvDevice.SelectedNode.SetImageKey(ChooseNodeImage(tvDevice.SelectedNode.Tag));
            }

            DeviceConfigModified = true;
        }

        private void ctrlDbConnection_ConnectionOptionsChanged(object sender, EventArgs e)
        {
            LineConfigModified = true;
        }
    }
}
