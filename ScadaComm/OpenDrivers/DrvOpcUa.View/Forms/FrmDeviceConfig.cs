﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using Opc.Ua.Client;
using Scada.Comm.Drivers.DrvOpcUa.Config;
using Scada.Comm.Drivers.DrvOpcUa.View.Properties;
using Scada.Comm.Lang;
using Scada.Forms;
using Scada.Lang;
using Scada.Log;

namespace Scada.Comm.Drivers.DrvOpcUa.View.Forms
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
            public const string Empty = "empty.png";
            public const string FolderClosed = "folder_closed.png";
            public const string FolderOpen = "folder_open.png";
            public const string Method = "method.png";
            public const string Object = "obj.png";
            public const string Variable = "variable.png";
        }

        private readonly AppDirs appDirs;              // the application directories
        private readonly int lineNum;                  // the communication line number
        private readonly int deviceNum;                // the device number
        private readonly OpcLineConfig lineConfig;     // the communication line configuration
        private readonly OpcDeviceConfig deviceConfig; // the device configuration

        private string lineConfigFileName;             // the line configuration file name
        private string deviceConfigFileName;           // the configuration file name
        private bool lineConfigModified;               // the line configuration has been modified
        private bool deviceConfigModified;             // the device configuration has been modified
        private bool changing;                         // controls are being changed programmatically
        private TreeNode subscriptionsNode;            // the tree node of the subscriptions
        private TreeNode commandsNode;                 // the tree node of the commands
        private ISession opcSession;                   // the OPC session


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private FrmDeviceConfig()
        {
            InitializeComponent();
            ctrlSubscription.Top = ctrlItem.Top = ctrlCommand.Top = ctrlEmptyItem.Top;
            ctrlSubscription.Visible = ctrlItem.Visible = ctrlCommand.Visible = false;
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
            lineConfig = new OpcLineConfig();
            deviceConfig = new OpcDeviceConfig();

            lineConfigFileName = "";
            deviceConfigFileName = "";
            lineConfigModified = false;
            deviceConfigModified = false;
            changing = false;
            subscriptionsNode = null;
            commandsNode = null;
            opcSession = null;
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
            ilTree.Images.Add(ImageKey.Command, Resources.cmd);
            ilTree.Images.Add(ImageKey.Empty, Resources.empty);
            ilTree.Images.Add(ImageKey.FolderClosed, Resources.folder_closed);
            ilTree.Images.Add(ImageKey.FolderOpen, Resources.folder_open);
            ilTree.Images.Add(ImageKey.Method, Resources.method);
            ilTree.Images.Add(ImageKey.Object, Resources.obj);
            ilTree.Images.Add(ImageKey.Variable, Resources.variable);
        }

        /// <summary>
        /// Sets the controls according to the configuration.
        /// </summary>
        private void ConfigToControls()
        {
            changing = true;
            txtServerUrl.Text = lineConfig.ConnectionOptions.ServerUrl;
            FillDeviceTree();
            changing = false;
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

                subscriptionsNode = TreeViewExtensions.CreateNode(DriverPhrases.SubscriptionsNode, ImageKey.FolderClosed);
                commandsNode = TreeViewExtensions.CreateNode(DriverPhrases.CommandsNode, ImageKey.FolderClosed);
                int tagNum = 1;

                foreach (SubscriptionConfig subscriptionConfig in deviceConfig.Subscriptions)
                {
                    TreeNode subscriptionNode = CreateSubscriptionNode(subscriptionConfig);
                    subscriptionsNode.Nodes.Add(subscriptionNode);

                    foreach (ItemConfig itemConfig in subscriptionConfig.Items)
                    {
                        subscriptionNode.Nodes.Add(CreateItemNode(itemConfig));
                        itemConfig.Tag = new ItemConfigTag(tagNum);
                        tagNum++;
                    }
                }

                foreach (CommandConfig commandConfig in deviceConfig.Commands)
                {
                    commandsNode.Nodes.Add(CreateCommandNode(commandConfig));
                }

                tvDevice.Nodes.Add(subscriptionsNode);
                tvDevice.Nodes.Add(commandsNode);
                subscriptionsNode.Expand();
                commandsNode.Expand();
            }
            finally
            {
                tvDevice.EndUpdate();
            }
        }

        /// <summary>
        /// Creates a new subscription node according to the subscription configuration.
        /// </summary>
        private static TreeNode CreateSubscriptionNode(SubscriptionConfig subscriptionConfig)
        {
            return TreeViewExtensions.CreateNode(
                GetDisplayName(subscriptionConfig.DisplayName, DriverPhrases.UnnamedSubscription), 
                ImageKey.FolderClosed,
                subscriptionConfig);
        }

        /// <summary>
        /// Creates a new monitored item node according to the item configuration.
        /// </summary>
        private static TreeNode CreateItemNode(ItemConfig itemConfig)
        {
            return TreeViewExtensions.CreateNode(
                GetDisplayName(itemConfig.DisplayName, DriverPhrases.UnnamedItem), 
                ImageKey.Variable,
                itemConfig);
        }

        /// <summary>
        /// Creates a new command node according to the command configuration.
        /// </summary>
        private static TreeNode CreateCommandNode(CommandConfig commandConfig)
        {
            return TreeViewExtensions.CreateNode(
                GetDisplayName(commandConfig.DisplayName, DriverPhrases.UnnamedCommand), 
                ImageKey.Command,
                commandConfig);
        }

        /// <summary>
        /// Returns the specified display name or the default name.
        /// </summary>
        private static string GetDisplayName(string displayName, string defaultName)
        {
            return string.IsNullOrEmpty(displayName) ? defaultName : displayName;
        }

        /// <summary>
        /// Selects an image key depending on the node class.
        /// </summary>
        private static string SelectImageKey(NodeClass nodeClass)
        {
            if (nodeClass.HasFlag(NodeClass.Object))
                return ImageKey.Object;
            else if (nodeClass.HasFlag(NodeClass.Method))
                return ImageKey.Method;
            else
                return ImageKey.Variable;
        }

        /// <summary>
        /// Sets the node image as open or closed folder.
        /// </summary>
        private static void SetFolderImage(TreeNode treeNode)
        {
            if (treeNode.ImageKey.StartsWith("folder_"))
                treeNode.SetImageKey(treeNode.IsExpanded ? ImageKey.FolderOpen : ImageKey.FolderClosed);
        }

        /// <summary>
        /// Sets the enabled property of the server browsing buttons.
        /// </summary>
        private void SetServerButtonsEnabled()
        {
            if (opcSession == null)
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnViewAttrs.Enabled = false;
            }
            else
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnViewAttrs.Enabled = tvServer.SelectedNode != null;
            }
        }

        /// <summary>
        /// Sets the enabled property of the buttons that manipulate the device tree.
        /// </summary>
        private void SetDeviceButtonsEnabled()
        {
            btnAddItem.Enabled = tvServer.SelectedNode?.Tag is ServerNodeTag serverNodeTag &&
                serverNodeTag.ClassIs(NodeClass.Variable, NodeClass.Method);

            bool deviceNodeTagDefined = tvDevice.SelectedNode?.Tag != null;
            btnMoveUpItem.Enabled = deviceNodeTagDefined && tvDevice.SelectedNode.PrevNode != null;
            btnMoveDownItem.Enabled = deviceNodeTagDefined && tvDevice.SelectedNode.NextNode != null;
            btnDeleteItem.Enabled = deviceNodeTagDefined;
        }

        /// <summary>
        /// Update signals if 2 elements are reversed.
        /// </summary>
        private void SwapSignals(TreeNode treeNode1, TreeNode treeNode2)
        {
            if (treeNode1?.Tag is ItemConfig itemConfig1 &&
                treeNode2?.Tag is ItemConfig itemConfig2 &&
                itemConfig1.Tag is ItemConfigTag itemConfigTag1 &&
                itemConfig2.Tag is ItemConfigTag itemConfigTag2)
            {
                (itemConfigTag1.TagNum, itemConfigTag2.TagNum) = (itemConfigTag2.TagNum, itemConfigTag1.TagNum);
                ctrlItem.RefreshTagNum();
            }
        }

        /// <summary>
        /// Update tag numbers starting from the specified node.
        /// </summary>
        private void UpdateTagNums(TreeNode startNode)
        {
            TreeNode startSubscrNode = startNode?.FindClosest(typeof(SubscriptionConfig));

            if (startSubscrNode != null)
            {
                // define initial tag number
                int tagNum = 1;
                TreeNode subscrNode = startSubscrNode.PrevNode;

                while (subscrNode != null)
                {
                    if (subscrNode.LastNode?.Tag is ItemConfig itemConfig &&
                        itemConfig.Tag is ItemConfigTag tag)
                    {
                        tagNum = tag.TagNum + 1;
                        break;
                    }

                    subscrNode = subscrNode.PrevNode;
                }

                // recalculate tag numbers
                subscrNode = startSubscrNode;

                while (subscrNode != null)
                {
                    foreach (TreeNode itemNode in subscrNode.Nodes)
                    {
                        if (itemNode.Tag is ItemConfig itemConfig &&
                            itemConfig.Tag is ItemConfigTag tag)
                        {
                            tag.TagNum = tagNum;
                            tagNum++;
                        }
                    }

                    subscrNode = subscrNode.NextNode;
                }

                ctrlItem.RefreshTagNum();
            }
        }

        /// <summary>
        /// Connects to the OPC server.
        /// </summary>
        private async Task<bool> ConnectToOpcServer()
        {
            try
            {
                OpcClientHelperView helper = new(lineConfig.ConnectionOptions, LogStub.Instance, appDirs)
                { 
                    AutoAccept = true 
                };

                await helper.ConnectAsync();
                opcSession = helper.OpcSession;
                return true;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.BuildErrorMessage(DriverPhrases.ConnectServerError));
                opcSession = null;
                return false;
            }
            finally
            {
                SetServerButtonsEnabled();
            }
        }

        /// <summary>
        /// Disconnects from the OPC server.
        /// </summary>
        private void DisconnectFromOpcServer()
        {
            try
            {
                if (opcSession != null)
                {
                    opcSession.Close();
                    opcSession = null;
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.BuildErrorMessage(DriverPhrases.DisconnectServerError));
            }
            finally
            {
                tvServer.Nodes.Clear();
                SetServerButtonsEnabled();
                SetDeviceButtonsEnabled();
            }
        }

        /// <summary>
        /// Browses the server node.
        /// </summary>
        private void BrowseServerNode(TreeNode treeNode)
        {
            try
            {
                tvServer.BeginUpdate();
                bool fillNodeRequired = false;
                TreeNodeCollection nodeCollection = null;
                ServerNodeTag serverNodeTag = null;
                NodeId nodeId = null;

                if (treeNode == null)
                {
                    fillNodeRequired = true;
                    nodeCollection = tvServer.Nodes;
                    serverNodeTag = null;
                    nodeId = ObjectIds.ObjectsFolder;
                }
                else if (treeNode.Tag is ServerNodeTag nodeTag)
                {
                    fillNodeRequired = !nodeTag.IsFilled;
                    nodeCollection = treeNode.Nodes;
                    serverNodeTag = nodeTag;
                    nodeId = nodeTag.NodeId;
                }

                if (fillNodeRequired && nodeId != null && opcSession != null)
                {
                    Browser browser = new(opcSession)
                    {
                        BrowseDirection = BrowseDirection.Forward,
                        NodeClassMask = (int)NodeClass.Variable | (int)NodeClass.Object | (int)NodeClass.Method,
                        ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences
                    };

                    ReferenceDescriptionCollection browseResults = browser.Browse(nodeId);
                    nodeCollection.Clear();

                    foreach (ReferenceDescription rd in browseResults)
                    {
                        TreeNode childNode = TreeViewExtensions.CreateNode(rd.DisplayName, SelectImageKey(rd.NodeClass));
                        childNode.Tag = new ServerNodeTag(rd, opcSession.NamespaceUris);

                        // allow to expand any node
                        TreeNode emptyNode = TreeViewExtensions.CreateNode(DriverPhrases.EmptyNode, ImageKey.Empty);
                        childNode.Nodes.Add(emptyNode);

                        nodeCollection.Add(childNode);
                    }

                    if (serverNodeTag != null)
                        serverNodeTag.IsFilled = true;
                }
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.BuildErrorMessage(DriverPhrases.BrowseServerError));
            }
            finally
            {
                tvServer.EndUpdate();
            }
        }

        /// <summary>
        /// Adds a new item to the configuration.
        /// </summary>
        private bool AddItem(TreeNode serverNode)
        {
            if (serverNode?.Tag is ServerNodeTag serverNodeTag && 
                serverNodeTag.ClassIs(NodeClass.Variable, NodeClass.Method))
            {
                if (TreeViewExtensions.GetTopParentNode(tvDevice.SelectedNode) == commandsNode || 
                    serverNodeTag.ClassIs(NodeClass.Method))
                {
                    AddCommand(serverNodeTag, serverNode.Parent?.Tag as ServerNodeTag);
                }
                else
                {
                    AddItemToSubscription(serverNodeTag);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a new subscription item to the configuration.
        /// </summary>
        private void AddItemToSubscription(ServerNodeTag serverNodeTag)
        {
            // create new monitored item
            ItemConfig itemConfig = new()
            {
                NodeID = serverNodeTag.NodeIdStr,
                DisplayName = serverNodeTag.DisplayName,
                TagCode = GetTagCode(serverNodeTag),
                Tag = new ItemConfigTag(0)
            };

            if (GetDataType(serverNodeTag.NodeId, out string dataTypeName, out bool isArray))
            {
                itemConfig.DataTypeName = dataTypeName;
                itemConfig.IsArray = isArray;
            }

            // find subscription
            TreeNode deviceNode = tvDevice.SelectedNode;
            TreeNode subscriptionNode = deviceNode?.FindClosest(typeof(SubscriptionConfig)) ??
                subscriptionsNode.LastNode;
            SubscriptionConfig subscriptionConfig;

            // add new subscription
            if (subscriptionNode == null)
            {
                subscriptionConfig = new SubscriptionConfig();
                subscriptionNode = CreateSubscriptionNode(subscriptionConfig);
                tvDevice.Insert(subscriptionsNode, subscriptionNode,
                    deviceConfig.Subscriptions, subscriptionConfig);
            }
            else
            {
                subscriptionConfig = (SubscriptionConfig)subscriptionNode.Tag;
            }

            // add monitored item
            TreeNode itemNode = CreateItemNode(itemConfig);
            tvDevice.Insert(subscriptionNode, itemNode, subscriptionConfig.Items, itemConfig);
            UpdateTagNums(itemNode);
            DeviceConfigModified = true;
        }

        /// <summary>
        /// Adds a new command to the configuration.
        /// </summary>
        private void AddCommand(ServerNodeTag serverNodeTag, ServerNodeTag parentServerNodeTag)
        {
            // add new command
            CommandConfig commandConfig = new()
            {
                NodeID = serverNodeTag.NodeIdStr,
                DisplayName = serverNodeTag.DisplayName,
                CmdCode = GetTagCode(serverNodeTag),
                IsMethod = serverNodeTag.ClassIs(NodeClass.Method)
            };

            if (commandConfig.IsMethod)
            {
                if (parentServerNodeTag != null)
                    commandConfig.ParentNodeID = parentServerNodeTag.NodeIdStr;
            }
            else if (GetDataType(serverNodeTag.NodeId, out string dataTypeName, out _))
            {
                commandConfig.DataTypeName = dataTypeName;
            }

            tvDevice.Insert(commandsNode, CreateCommandNode(commandConfig), deviceConfig.Commands, commandConfig);
            DeviceConfigModified = true;
        }

        /// <summary>
        /// Gets the tag code depending on the editing options.
        /// </summary>
        private string GetTagCode(ServerNodeTag serverNodeTag)
        {
            return deviceConfig.EditingOptions.DefaultTagCode switch
            {
                DefaultTagCode.NodeID => serverNodeTag.NodeIdStr,
                _ => serverNodeTag.DisplayName
            };
        }

        /// <summary>
        /// Gets the data type name of the node.
        /// </summary>
        private bool GetDataType(NodeId nodeId, out string dataTypeName, out bool isArray)
        {
            if (nodeId == null)
                throw new ArgumentNullException(nameof(nodeId));
            if (opcSession == null)
                throw new InvalidOperationException("OPC session must not be null.");

            try
            {
                ReadValueIdCollection nodesToRead = new()
                {
                    new ReadValueId
                    {
                        NodeId = nodeId,
                        AttributeId = Attributes.Value
                    }
                };

                opcSession.Read(null, 0, TimestampsToReturn.Neither, nodesToRead,
                    out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos);
                ClientBase.ValidateResponse(results, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);
                DataValue nodeValue = results[0];

                if (StatusCode.IsNotGood(nodeValue.StatusCode) || nodeValue.Value == null)
                    throw new ScadaException(DriverPhrases.UnableToReadData);

                Type dataType = nodeValue.Value.GetType();
                Type elemType = dataType.IsArray ? dataType.GetElementType() : dataType;
                dataTypeName = elemType.FullName;
                isArray = dataType.IsArray && elemType != typeof(string); // string array not supported
                return true;
            }
            catch (Exception ex)
            {
                ScadaUiUtils.ShowError(ex.BuildErrorMessage(DriverPhrases.GetDataTypeError));
                dataTypeName = "";
                isArray = false;
                return false;
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
            FormTranslator.Translate(this, GetType().FullName, new FormTranslatorOptions { ToolTip = toolTip });
            FormTranslator.Translate(ctrlSubscription, ctrlSubscription.GetType().FullName);
            FormTranslator.Translate(ctrlItem, ctrlItem.GetType().FullName);
            FormTranslator.Translate(ctrlCommand, ctrlCommand.GetType().FullName);
            Text = string.Format(Text, deviceNum);

            // load configuration
            lineConfigFileName = Path.Combine(appDirs.ConfigDir, OpcLineConfig.GetFileName(lineNum));
            deviceConfigFileName = Path.Combine(appDirs.ConfigDir, OpcDeviceConfig.GetFileName(deviceNum));

            if (File.Exists(lineConfigFileName) && !lineConfig.Load(lineConfigFileName, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            if (File.Exists(deviceConfigFileName) && !deviceConfig.Load(deviceConfigFileName, out errMsg))
                ScadaUiUtils.ShowError(errMsg);

            // display configuration
            TakeTreeViewImages();
            ConfigToControls();
            SetServerButtonsEnabled();
            SetDeviceButtonsEnabled();
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

        private void FrmDeviceConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            opcSession?.Close();
        }


        private void txtServerUrl_TextChanged(object sender, EventArgs e)
        {
            if (!changing)
            {
                lineConfig.ConnectionOptions.ServerUrl = txtServerUrl.Text;
                LineConfigModified = true;
            }
        }

        private void btnSecurityOptions_Click(object sender, EventArgs e)
        {
            if (new FrmSecurityOptions(lineConfig.ConnectionOptions).ShowDialog() == DialogResult.OK)
                LineConfigModified = true;
        }

        private async void btnConnect_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lineConfig.ConnectionOptions.ServerUrl))
                ScadaUiUtils.ShowError(DriverPhrases.ServerUrlRequired);
            else if (await ConnectToOpcServer())
                BrowseServerNode(null);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectFromOpcServer();
        }

        private void btnViewAttrs_Click(object sender, EventArgs e)
        {
            if (opcSession != null && tvServer.SelectedNode?.Tag is ServerNodeTag serverNodeTag)
                new FrmNodeAttr(opcSession, serverNodeTag.NodeId).ShowDialog();
        }

        private void tvServer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetServerButtonsEnabled();
            SetDeviceButtonsEnabled();
        }

        private void tvServer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            BrowseServerNode(e.Node);
        }

        private void tvServer_KeyDown(object sender, KeyEventArgs e)
        {
            TreeNode selectedNode = tvServer.SelectedNode;

            if (e.KeyCode == Keys.Enter && AddItem(selectedNode))
            {
                // go to the next node
                if (selectedNode.NextNode != null)
                    tvServer.SelectedNode = selectedNode.NextNode;
                else if (selectedNode.Parent?.NextNode != null)
                    tvServer.SelectedNode = selectedNode.Parent.NextNode;
            }
        }

        private void tvServer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                AddItem(tvServer.SelectedNode);
        }


        private void btnAddItem_Click(object sender, EventArgs e)
        {
            AddItem(tvServer.SelectedNode);
        }

        private void btnAddSubscription_Click(object sender, EventArgs e)
        {
            // add a new subscription
            SubscriptionConfig subscriptionConfig = new();
            TreeNode subscriptionNode = CreateSubscriptionNode(subscriptionConfig);
            tvDevice.Insert(subscriptionsNode, subscriptionNode,
                deviceConfig.Subscriptions, subscriptionConfig);
            ctrlSubscription.SetFocus();
            DeviceConfigModified = true;
        }

        private void btnMoveUpItem_Click(object sender, EventArgs e)
        {
            // move up the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = selectedNode?.Tag;

            if (deviceNodeTag is SubscriptionConfig)
            {
                tvDevice.MoveUpSelectedNode(deviceConfig.Subscriptions);
                UpdateTagNums(selectedNode);
            }
            else if (deviceNodeTag is ItemConfig)
            {
                if (selectedNode.Parent.Tag is SubscriptionConfig subscriptionConfig)
                {
                    tvDevice.MoveUpSelectedNode(subscriptionConfig.Items);
                    SwapSignals(selectedNode, selectedNode.NextNode);
                }
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.MoveUpSelectedNode(deviceConfig.Commands);
            }

            DeviceConfigModified = true;
        }

        private void btnMoveDownItem_Click(object sender, EventArgs e)
        {
            // move down the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = tvDevice.SelectedNode?.Tag;

            if (deviceNodeTag is SubscriptionConfig)
            {
                tvDevice.MoveDownSelectedNode(deviceConfig.Subscriptions);
                UpdateTagNums(selectedNode);
            }
            else if (deviceNodeTag is ItemConfig)
            {
                if (selectedNode.Parent.Tag is SubscriptionConfig subscriptionConfig)
                {
                    tvDevice.MoveDownSelectedNode(subscriptionConfig.Items);
                    SwapSignals(selectedNode, selectedNode.PrevNode);
                }
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.MoveDownSelectedNode(deviceConfig.Commands);
            }

            DeviceConfigModified = true;
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            // delete the selected item
            TreeNode selectedNode = tvDevice.SelectedNode;
            object deviceNodeTag = selectedNode?.Tag;

            if (deviceNodeTag is SubscriptionConfig)
            {
                TreeNode nextSubscrNode = selectedNode.NextNode;
                tvDevice.RemoveNode(selectedNode, deviceConfig.Subscriptions);
                UpdateTagNums(nextSubscrNode);
            }
            else if (deviceNodeTag is ItemConfig)
            {
                if (selectedNode.Parent.Tag is SubscriptionConfig subscriptionConfig)
                {
                    TreeNode subscrNode = selectedNode.Parent;
                    tvDevice.RemoveNode(selectedNode, subscriptionConfig.Items);
                    UpdateTagNums(subscrNode);
                }
            }
            else if (deviceNodeTag is CommandConfig)
            {
                tvDevice.RemoveNode(selectedNode, deviceConfig.Commands);
            }

            DeviceConfigModified = true;
        }

        private void tvDevice_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetDeviceButtonsEnabled();

            // show parameters of the selected item
            ctrlEmptyItem.Visible = false;
            ctrlSubscription.Visible = false;
            ctrlItem.Visible = false;
            ctrlCommand.Visible = false;
            object deviceNodeTag = e.Node?.Tag;

            if (deviceNodeTag is SubscriptionConfig subscriptionConfig)
            {
                ctrlSubscription.SubscriptionConfig = subscriptionConfig;
                ctrlSubscription.Visible = true;
            }
            else if (deviceNodeTag is ItemConfig itemConfig)
            {
                ctrlItem.ItemConfig = itemConfig;
                ctrlItem.Visible = true;
            }
            else if (deviceNodeTag is CommandConfig commandConfig)
            {
                ctrlCommand.CommandConfig = commandConfig;
                ctrlCommand.Visible = true;
            }
            else
            {
                ctrlEmptyItem.Visible = true;
            }
        }

        private void tvDevice_AfterExpand(object sender, TreeViewEventArgs e)
        {
            SetFolderImage(e.Node);
        }

        private void tvDevice_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            SetFolderImage(e.Node);
        }

        private void ctrlItem_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            DeviceConfigModified = true;
            TreeNode selectedNode = tvDevice.SelectedNode;
            TreeUpdateTypes treeUpdateTypes = (TreeUpdateTypes)e.ChangeArgument;

            if (e.ChangedObject is SubscriptionConfig subscriptionConfig)
            {
                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = GetDisplayName(subscriptionConfig.DisplayName, DriverPhrases.UnnamedSubscription);
            }
            else if (e.ChangedObject is ItemConfig itemConfig)
            {
                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = GetDisplayName(itemConfig.DisplayName, DriverPhrases.UnnamedItem);

                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.UpdateTagNums))
                    UpdateTagNums(selectedNode);
            }
            else if (e.ChangedObject is CommandConfig commandConfig)
            {
                if (treeUpdateTypes.HasFlag(TreeUpdateTypes.CurrentNode))
                    selectedNode.Text = GetDisplayName(commandConfig.DisplayName, DriverPhrases.UnnamedCommand);
            }
        }

        private void btnEditingOptions_Click(object sender, EventArgs e)
        {
            if (new FrmEditingOptions(deviceConfig.EditingOptions).ShowDialog() == DialogResult.OK)
                DeviceConfigModified = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }
    }
}
