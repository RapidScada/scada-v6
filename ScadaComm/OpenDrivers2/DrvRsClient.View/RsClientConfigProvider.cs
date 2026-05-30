// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvRsClient.Config;
using Scada.Comm.Drivers.DrvRsClient.View.Properties;
using Scada.Forms;
using Scada.Lang;
using System.Collections;

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
            public const string EditLineConfig = nameof(EditLineConfig);
            public const string FillItemNames = nameof(FillItemNames);
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RsClientConfigProvider(string configDir, int lineNum, int deviceNum)
            : base()
        {
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
        /// Gets toolbar buttons for custom purposes.
        /// </summary>
        public override ToolStripItem[] GetCustomButtons()
        {
            return
            [
                new ToolStripButton(Resources.connect)
                {
                    ToolTipText = DriverPhrases.EditLineConfigButton,
                    Tag = ButtonTag.EditLineConfig
                },
                new ToolStripButton(Resources.text)
                {
                    ToolTipText = DriverPhrases.FillItemNamesButton,
                    Tag = ButtonTag.FillItemNames
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
            ITreeNode parentObj = null;
            ITreeNode objToInsert = null;

            switch (GetButtonTag(button))
            {
                case ButtonTag.AddItemGroup:
                    ItemGroupConfig itemGroupConfig = new();
                    nodeToInsert = CreateItemGroupNode(itemGroupConfig);
                    parentObj = DeviceConfig.ItemGroups;
                    objToInsert = itemGroupConfig;
                    break;

                case ButtonTag.AddItem:
                    parentNode = treeView.SelectedNode.FindClosest(typeof(ItemGroupConfig));
                    parentObj = parentNode?.GetRelatedObject() as ITreeNode;

                    if (parentObj != null)
                    {
                        ItemConfig itemConfig = new();
                        nodeToInsert = CreateItemNode(itemConfig);
                        objToInsert = itemConfig;
                    }

                    break;
            }

            if (objToInsert != null)
            {
                objToInsert.Parent = parentObj;
                treeView.Insert(parentNode, nodeToInsert, parentObj.Children, objToInsert);
            }
        }

        /// <summary>
        /// Handles a click on the custom button.
        /// </summary>
        public override void HandleCustomButtonClick(object button, TreeView treeView, ref bool configModified)
        {
            switch (GetButtonTag(button))
            {
                case ButtonTag.EditLineConfig:
                    break;

                case ButtonTag.FillItemNames:
                    break;
            }
        }

        /// <summary>
        /// Determines whether the specified action can be performed.
        /// </summary>
        public override bool AllowAction(ConfigAction action, object button, TreeNode selectedNode)
        {
            string buttonTag = GetButtonTag(button);
            return action switch
            {
                ConfigAction.Add =>
                    buttonTag == ButtonTag.AddItemGroup ||
                    buttonTag == ButtonTag.AddItem && selectedNode.FindClosest(typeof(ItemGroupConfig)) != null,
                _ => base.AllowAction(action, button, selectedNode)
            };
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
            List<TreeNode> itemGroupNodes = [];

            foreach (ItemGroupConfig itemGroupConfig in DeviceConfig.ItemGroups)
            {
                itemGroupNodes.Add(CreateItemGroupNode(itemGroupConfig));
            }

            return [.. itemGroupNodes];
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
    }
}
