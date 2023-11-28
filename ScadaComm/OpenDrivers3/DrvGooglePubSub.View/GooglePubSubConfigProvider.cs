// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvGooglePubSub.Config;
using Scada.Comm.Drivers.DrvGooglePubSub.View.Properties;
using Scada.Forms;
using System.Collections;

namespace Scada.Comm.Drivers.DrvGooglePubSub.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class GooglePubSubConfigProvider : ConfigProvider
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
        private TreeNode subscriptionsNode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public GooglePubSubConfigProvider(string configDir, int deviceNum)
            : base()
        {
            btnAddSubscription = null;
            subscriptionsNode = null;

            ConfigFileName = Path.Combine(configDir, GooglePubSubDeviceConfig.GetFileName(deviceNum));
            Config = new GooglePubSubDeviceConfig();
            FormTitle = string.Format(DriverPhrases.FormTitle, deviceNum);
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public GooglePubSubDeviceConfig DeviceConfig => Config as GooglePubSubDeviceConfig;


        /// <summary>
        /// Creates a tree node according to the subscription configuration.
        /// </summary>
        private static TreeNode CreateSubscriptionNode(SubscriptionConfig subscription)
        {
            return TreeViewExtensions.CreateNode(GetSubscriptionNodeText(subscription), ImageKey.Elem, subscription);
        }

        /// <summary>
        /// Gets a text for the subscription tree node.
        /// </summary>
        private static string GetSubscriptionNodeText(SubscriptionConfig subscription)
        {
            return string.IsNullOrEmpty(subscription.DisplayName)
                ? DriverPhrases.UnnamedSubscription
                : subscription.DisplayName;
        }

        /// <summary>
        /// Gets a text for the command tree node.
        /// </summary>
        private static string GetCommandNodeText(CommandConfig command)
        {
            return string.IsNullOrEmpty(command.DisplayName)
                ? DriverPhrases.UnnamedCommand
                : command.DisplayName;
        }


        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public override void RestoreConfig()
        {
            base.RestoreConfig();
            DeviceConfig.Subscriptions.RestoreHierarchy();
            //DeviceConfig.Commands.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public override ToolStripItem[] GetAddButtons()
        {
            btnAddSubscription = new ToolStripMenuItem(DriverPhrases.AddSubscriptionButton, Resources.elem);
            return new ToolStripItem[] { btnAddSubscription };
            //btnAddCommand = new ToolStripMenuItem(DriverPhrases.AddCommandButton, Resources.cmd);
            //return new ToolStripItem[] { btnAddSubscription, btnAddCommand };
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public override void HandleAddButtonClick(object button, TreeView treeView)
        {
            TreeNode groupNode = null;
            TreeNode nodeToInsert = null;

            //if (button == btnAddSubscription)
            //{
            //    groupNode = subscriptionsNode;
            //    nodeToInsert = CreateSubscriptionNode(new SubscriptionConfig());
            //}
            //else if (button == btnAddCommand)
            //{
            //    groupNode = commandsNode;
            //    nodeToInsert = CreateCommandNode(new CommandConfig());
            //}
            groupNode = subscriptionsNode;
            nodeToInsert = CreateSubscriptionNode(new SubscriptionConfig());
            treeView.Insert(groupNode, nodeToInsert);
        }

        /// <summary>
        /// Determines whether the specified action can be performed.
        /// </summary>
        public override bool AllowAction(ConfigAction action, object button, TreeNode selectedNode)
        {
            return base.AllowAction(action, button, selectedNode) &&
                (action == ConfigAction.Add || selectedNode?.Tag is BaseItemConfig);
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
            //commandsNode = TreeViewExtensions.CreateNode(
            //    DriverPhrases.CommandsNode, ImageKey.FolderClosed, DeviceConfig.Commands);

            foreach (SubscriptionConfig subscription in DeviceConfig.Subscriptions)
            {
                subscriptionsNode.Nodes.Add(CreateSubscriptionNode(subscription));
            }

            //foreach (CommandConfig command in DeviceConfig.Commands)
            //{
            //    commandsNode.Nodes.Add(CreateCommandNode(command));
            //}

            return new TreeNode[] { optionsNode, subscriptionsNode };
        }

        /// <summary>
        /// Gets an image key for the specified tree node.
        /// </summary>
        public override string GetNodeImage(TreeNode treeNode)
        {
            if (treeNode?.Tag is IList)
                return treeNode.IsExpanded ? ImageKey.FolderOpen : ImageKey.FolderClosed;
            else
                return "";
        }

        /// <summary>
        /// Gets a tree node text for the specified object.
        /// </summary>
        public override string GetNodeText(object obj)
        {
            if (obj is DeviceOptions)
                return DriverPhrases.OptionsNode;
            else if (obj is SubscriptionConfig subscription)
                return GetSubscriptionNodeText(subscription);
            else if (obj is CommandConfig command)
                return GetCommandNodeText(command);
            else
                return base.GetNodeText(obj);
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
