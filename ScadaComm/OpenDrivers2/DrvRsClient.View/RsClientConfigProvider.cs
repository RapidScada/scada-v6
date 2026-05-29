// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvRsClient.Config;
using Scada.Comm.Drivers.DrvRsClient.View.Properties;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvRsClient.View
{
    /// <summary>
    /// Represents an intermediary between a driver configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией драйвера и формой конфигурации.</para>
    /// </summary>
    internal class RsClientConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Specifies the image keys for the configuration tree.
        /// </summary>
        private static class ImageKey
        {
            public const string FolderClosed = "folder_closed.png";
            public const string FolderClosedInactive = "folder_closed_inactive.png";
            public const string FolderOpen = "folder_open.png";
            public const string FolderOpenInactive = "folder_open_inactive.png";
            public const string Item = "item.png";
        }

        /// <summary>
        /// Specifies the button tags.
        /// </summary>
        private static class ButtonTag
        {
            public const string AddItemGroup = nameof(AddItemGroup);
            public const string AddItem = nameof(AddItem);
            public const string LineConfig = nameof(LineConfig);
            public const string FillChannelNames = nameof(FillChannelNames);
        }

        private TreeNode itemGroupsNode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RsClientConfigProvider(string configDir, int lineNum, int deviceNum)
            : base()
        {
            itemGroupsNode = null;

            ConfigFileName = RsClientDeviceConfig.GetFullFileName(configDir, deviceNum);
            Config = new RsClientDeviceConfig();
            FormTitle = string.Format(DriverPhrases.FormTitle, deviceNum);
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public RsClientDeviceConfig DeviceConfig => Config as RsClientDeviceConfig;
        
        
        /// <summary>
        /// Creates a tree node according to the variable group configuration.
        /// </summary>
        private static TreeNode CreateItemGroupNode(ItemGroupConfig itemGroupConfig)
        {
            TreeNode itemGroupNode = TreeViewExtensions.CreateNode(GetItemGroupNodeText(itemGroupConfig),
                ImageKey.FolderClosed, itemGroupConfig);

            foreach (ItemConfig itemConfig in itemGroupConfig.Items)
            {
                itemGroupNode.Nodes.Add(CreateItemNode(itemConfig));
            }

            return itemGroupNode;
        }

        /// <summary>
        /// Creates a tree node according to the variable configuration.
        /// </summary>
        private static TreeNode CreateItemNode(ItemConfig itemConfig)
        {
            return TreeViewExtensions.CreateNode(GetItemNodeText(itemConfig), ImageKey.Item, itemConfig);
        }

        /// <summary>
        /// Gets a text for the subscription tree node.
        /// </summary>
        private static string GetItemGroupNodeText(ItemGroupConfig itemGroupConfig)
        {
            return string.IsNullOrEmpty(itemGroupConfig.Name)
                ? DriverPhrases.UnnamedGroup
                : itemGroupConfig.Name;
        }

        /// <summary>
        /// Gets a text for the variable tree node.
        /// </summary>
        private static string GetItemNodeText(ItemConfig itemConfig)
        {
            return string.IsNullOrEmpty(itemConfig.Name)
                ? string.Format(DriverPhrases.UnnamedItem, itemConfig.CnlNum)
                : string.Format(CommonPhrases.EntityCaption, itemConfig.CnlNum, itemConfig.Name);
        }

        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public override void RestoreConfig()
        {
            base.RestoreConfig();
            DeviceConfig.ItemGroups.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public override ToolStripItem[] GetAddButtons()
        {
            return
            [
                new ToolStripMenuItem(DriverPhrases.AddItemGroupButton, Resources.folder_open)
                {
                    Tag = ButtonTag.AddItemGroup
                },
                new ToolStripMenuItem(DriverPhrases.AddItemButton, Resources.item)
                {
                    Tag = ButtonTag.AddItem
                }
            ];
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public override void HandleAddButtonClick(object button, TreeView treeView)
        {
            TreeNode parentNode = null;
            TreeNode nodeToInsert = null;
            object buttonTag = (button as ToolStripItem)?.Tag;

            if (buttonTag.Equals(ButtonTag.AddItemGroup))
            {
                parentNode = itemGroupsNode;
                nodeToInsert = CreateItemGroupNode(new ItemGroupConfig());
            }
            else if (buttonTag.Equals(ButtonTag.AddItem))
            {
                parentNode = treeView.SelectedNode.FindClosest(typeof(ItemGroupConfig));
                nodeToInsert = CreateItemNode(new ItemConfig());
            }

            treeView.Insert(parentNode, nodeToInsert);
        }

        /// <summary>
        /// Determines whether the specified action can be performed.
        /// </summary>
        public override bool AllowAction(ConfigAction action, object button, TreeNode selectedNode)
        {
            if (!base.AllowAction(action, button, selectedNode))
                return false;

            object tag = selectedNode?.Tag;
            return action == ConfigAction.Add && (tag is ItemGroupConfig || tag is ItemConfig);
        }

        /// <summary>
        /// Gets images used by the configuration tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return new Dictionary<string, Image>
            {
                { ImageKey.FolderClosed, Resources.folder_closed },
                { ImageKey.FolderClosedInactive, Resources.folder_closed_inactive },
                { ImageKey.FolderOpen, Resources.folder_open },
                { ImageKey.FolderOpenInactive, Resources.folder_open_inactive },
                { ImageKey.Item, Resources.item }
            };
        }

        /// <summary>
        /// Gets tree nodes to add to the configuration tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes()
        {
            itemGroupsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.ItemGroupsNode, ImageKey.FolderClosed, DeviceConfig.ItemGroups);

            foreach (ItemGroupConfig itemGroupConfig in DeviceConfig.ItemGroups)
            {
                itemGroupsNode.Nodes.Add(CreateItemGroupNode(itemGroupConfig));
            }

            return [itemGroupsNode];
        }

        /// <summary>
        /// Gets an image key for the specified object.
        /// </summary>
        public override string GetNodeImage(object obj, bool expanded)
        {
            if (obj is ItemGroupList)
            {
                return expanded ? ImageKey.FolderOpen : ImageKey.FolderClosed;
            }
            else if (obj is ItemGroupConfig itemGroupConfig)
            {
                return itemGroupConfig.Active
                    ? (expanded ? ImageKey.FolderOpen : ImageKey.FolderClosed)
                    : (expanded ? ImageKey.FolderOpenInactive : ImageKey.FolderClosedInactive);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets a tree node text for the specified object.
        /// </summary>
        public override string GetNodeText(object obj)
        {
            if (obj is ItemGroupConfig itemGroupConfig)
                return GetItemGroupNodeText(itemGroupConfig);
            else if (obj is ItemConfig itemConfig)
                return GetItemNodeText(itemConfig);
            else
                return base.GetNodeText(obj);
        }

        /// <summary>
        /// Gets a selected object for editing its properties.
        /// </summary>
        public override object GetSelectedObject(TreeNode selectedNode)
        {
            object tag = selectedNode?.Tag;
            return tag is ItemGroupList ? null : tag;
        }
    }
}
