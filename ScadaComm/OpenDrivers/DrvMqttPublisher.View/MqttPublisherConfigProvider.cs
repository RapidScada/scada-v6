// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqttPublisher.Config;
using Scada.Comm.Drivers.DrvMqttPublisher.View.Properties;
using Scada.Forms;
using System.Collections;

namespace Scada.Comm.Drivers.DrvMqttPublisher.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class MqttPublisherConfigProvider : ConfigProvider
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

        private TreeNode itemsNode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttPublisherConfigProvider(string configDir, int deviceNum)
            : base()
        {
            itemsNode = null;

            ConfigFileName = Path.Combine(configDir, MqttPublisherDeviceConfig.GetFileName(deviceNum));
            Config = new MqttPublisherDeviceConfig();
            FormTitle = string.Format(DriverPhrases.FormTitle, deviceNum);
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public MqttPublisherDeviceConfig DeviceConfig => Config as MqttPublisherDeviceConfig;


        /// <summary>
        /// Creates a tree node according to the item configuration.
        /// </summary>
        private static TreeNode CreateItemNode(ItemConfig item)
        {
            return TreeViewExtensions.CreateNode(GetItemNodeText(item), ImageKey.Elem, item);
        }

        /// <summary>
        /// Gets a text for the item tree node.
        /// </summary>
        private static string GetItemNodeText(ItemConfig item)
        {
            return string.Format(DriverPhrases.ItemNodeFormat, item.CnlNum);
        }


        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public override void RestoreConfig()
        {
            base.RestoreConfig();
            DeviceConfig.Items.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public override ToolStripItem[] GetAddButtons()
        {
            return new ToolStripItem[] 
            { 
                new ToolStripMenuItem(DriverPhrases.AddItemButton, Resources.elem) 
            };
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public override void HandleAddButtonClick(object button, TreeView treeView)
        {
            treeView.Insert(itemsNode, CreateItemNode(new ItemConfig()));
        }

        /// <summary>
        /// Determines if the specified action can be performed.
        /// </summary>
        public override bool AllowAction(ConfigAction action, object button, TreeNode selectedNode)
        {
            return base.AllowAction(action, button, selectedNode) &&
                (action == ConfigAction.Add || selectedNode?.Tag is ItemConfig);
        }

        /// <summary>
        /// Gets images used by the configuration tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
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
            itemsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.ItemsNode, ImageKey.FolderClosed, DeviceConfig.Items);

            foreach (ItemConfig item in DeviceConfig.Items)
            {
                itemsNode.Nodes.Add(CreateItemNode(item));
            }

            return new TreeNode[] { optionsNode, itemsNode };
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
            else if (obj is ItemConfig item)
                return GetItemNodeText(item);
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
