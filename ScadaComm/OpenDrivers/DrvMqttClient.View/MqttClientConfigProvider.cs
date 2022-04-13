// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Comm.Drivers.DrvMqttClient.View.Properties;
using Scada.Forms;
using System.Collections;

namespace Scada.Comm.Drivers.DrvMqttClient.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class MqttClientConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Specifies the image keys for the configuration tree.
        /// </summary>
        private static class ImageKey
        {
            public const string Cmd = "cmd.png";
            public const string Elem = "elem.png";
            public const string FolderClosed = "folder_closed.png";
            public const string FolderOpen = "folder_open.png";
            public const string Options = "options.png";
        }


        private ToolStripMenuItem btnAddSubscription;
        private ToolStripMenuItem btnAddCommand;
        private TreeNode subscriptionsNode;
        private TreeNode commandsNode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientConfigProvider(string configDir, int deviceNum)
            : base()
        {
            btnAddSubscription = null;
            btnAddCommand = null;
            subscriptionsNode = null;
            commandsNode = null;

            FormTitle = string.Format(DriverPhrases.FormTitle, deviceNum);
            ConfigFileName = Path.Combine(configDir, MqttClientDeviceConfig.GetFileName(deviceNum));
            Config = new MqttClientDeviceConfig();
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public MqttClientDeviceConfig DeviceConfig => Config as MqttClientDeviceConfig;


        /// <summary>
        /// Creates a tree node according to the subscription configuration.
        /// </summary>
        private static TreeNode CreateSubscriptionNode(SubscriptionConfig subscription)
        {
            return TreeViewExtensions.CreateNode(
                string.IsNullOrEmpty(subscription.DisplayName) 
                    ? DriverPhrases.UnnamedSubscription 
                    : subscription.DisplayName,
                ImageKey.Elem, subscription);
        }

        /// <summary>
        /// Creates a tree node according to the command configuration.
        /// </summary>
        private static TreeNode CreateCommandNode(CommandConfig command)
        {
            return TreeViewExtensions.CreateNode(
                string.IsNullOrEmpty(command.DisplayName)
                    ? DriverPhrases.UnnamedCommand
                    : command.DisplayName,
                ImageKey.Cmd, command);
        }


        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public override void RestoreConfig()
        {
            base.RestoreConfig();
            DeviceConfig.Subscriptions.RestoreHierarchy();
            DeviceConfig.Commands.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public override ToolStripItem[] GetAddButtons()
        {
            btnAddSubscription = new ToolStripMenuItem(DriverPhrases.AddSubscriptionButton, Resources.elem);
            btnAddCommand = new ToolStripMenuItem(DriverPhrases.AddCommandButton, Resources.cmd);
            return new ToolStripItem[] { btnAddSubscription, btnAddCommand };
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public override void HandleAddButtonClick(object button, TreeView treeView)
        {
            TreeNode groupNode = null;
            TreeNode nodeToInsert = null;

            if (button == btnAddSubscription)
            {
                groupNode = subscriptionsNode;
                nodeToInsert = CreateSubscriptionNode(new SubscriptionConfig());
            }
            else if (button == btnAddCommand)
            {
                groupNode = commandsNode;
                nodeToInsert = CreateCommandNode(new CommandConfig());
            }

            treeView.Insert(groupNode, nodeToInsert);
        }

        /// <summary>
        /// Determines if the selected item can be moved up.
        /// </summary>
        public override bool AllowMoveUp(TreeNode selectedNode)
        {
            return selectedNode?.Tag is BaseItemConfig && base.AllowMoveUp(selectedNode);
        }

        /// <summary>
        /// Determines if the selected item can be moved down.
        /// </summary>
        public override bool AllowMoveDown(TreeNode selectedNode)
        {
            return selectedNode?.Tag is BaseItemConfig && base.AllowMoveDown(selectedNode);
        }

        /// <summary>
        /// Determines if the selected item can be deleted.
        /// </summary>
        public override bool AllowDelete(TreeNode selectedNode)
        {
            return selectedNode?.Tag is BaseItemConfig;
        }

        /// <summary>
        /// Gets images used by the configuration tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImageKey.Cmd, Resources.cmd },
                { ImageKey.Elem, Resources.elem },
                { ImageKey.FolderClosed, Resources.folder_closed },
                { ImageKey.FolderOpen, Resources.folder_open },
                { ImageKey.Options, Resources.options }
            };
        }

        /// <summary>
        /// Gets tree nodes to add to the configuration tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes()
        {
            TreeNode optionsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.OptionsNode, ImageKey.Options, DeviceConfig.DeviceOptions);
            subscriptionsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.SubscriptionsNode, ImageKey.FolderClosed, DeviceConfig.Subscriptions);
            commandsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.CommandsNode, ImageKey.FolderClosed, DeviceConfig.Commands);

            foreach (SubscriptionConfig subscription in DeviceConfig.Subscriptions)
            {
                subscriptionsNode.Nodes.Add(CreateSubscriptionNode(subscription));
            }

            foreach (CommandConfig command in DeviceConfig.Commands)
            {
                commandsNode.Nodes.Add(CreateCommandNode(command));
            }

            return new TreeNode[] { optionsNode, subscriptionsNode, commandsNode };
        }

        /// <summary>
        /// Gets a selected object for editing its properties.
        /// </summary>
        public override object GetSelectedObject(TreeNode selectedNode)
        {
            object tag = selectedNode?.Tag;
            return tag is IList ? null : tag;
        }
    }
}
