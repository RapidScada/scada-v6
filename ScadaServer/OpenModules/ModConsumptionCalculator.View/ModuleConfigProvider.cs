// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Modules.ModConsumptionCalculator.Config;
using Scada.Server.Modules.ModConsumptionCalculator.View.Properties;
using System.Collections;

namespace Scada.Server.Modules.ModConsumptionCalculator.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class ModuleConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Specifies the image keys for the configuration tree.
        /// </summary>
        private static class ImageKey
        {
            public const string Elem = "elem.png";
            public const string FolderClosed = "folder_closed.png";
            public const string FolderOpen = "folder_open.png";
            public const string Options = "options.png";
        }

        private ToolStripMenuItem btnAddGroup;
        private ToolStripMenuItem btnAddItem;
        private TreeNode optionsNode;
        private TreeNode groupsNode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleConfigProvider(string configDir)
            : base()
        {
            btnAddGroup = null;
            btnAddItem = null;
            optionsNode = null;
            groupsNode = null;

            ConfigFileName = Path.Combine(configDir, ModuleConfig.DefaultFileName);
            Config = new ModuleConfig();
        }


        /// <summary>
        /// Gets the module configuration.
        /// </summary>
        public ModuleConfig ModuleConfig => Config as ModuleConfig;


        /// <summary>
        /// Creates a tree node according to the group configuration.
        /// </summary>
        private static TreeNode CreateGroupNode(CalcGroupConfig calcGroupConfig)
        {
            TreeNode groupNode = TreeViewExtensions.CreateNode(GetGroupNodeText(calcGroupConfig),
                ImageKey.FolderClosed, calcGroupConfig);

            foreach (ItemConfig itemConfig in calcGroupConfig.Items)
            {
                groupNode.Nodes.Add(CreateItemNode(itemConfig));
            }

            return groupNode;
        }

        /// <summary>
        /// Creates a tree node according to the item configuration.
        /// </summary>
        private static TreeNode CreateItemNode(ItemConfig itemConfig)
        {
            return TreeViewExtensions.CreateNode(GetItemNodeText(itemConfig), ImageKey.Elem, itemConfig);
        }

        /// <summary>
        /// Gets a text for a group tree node.
        /// </summary>
        private static string GetGroupNodeText(CalcGroupConfig calcGroupConfig)
        {
            return string.IsNullOrEmpty(calcGroupConfig.Name)
                ? ModulePhrases.UnnamedGroup
                : calcGroupConfig.Name;
        }

        /// <summary>
        /// Gets a text for an item tree node.
        /// </summary>
        private static string GetItemNodeText(ItemConfig itemConfig)
        {
            return string.Format(ModulePhrases.ItemNode, itemConfig.SrcCnlNum, itemConfig.DestCnlNum);
        }

        /// <summary>
        /// Gets a group tree node closest to the selected node.
        /// </summary>
        private static bool GetGroupNode(TreeNode selectedNode, out TreeNode groupNode)
        {
            groupNode = selectedNode?.FindClosest(typeof(CalcGroupConfig));
            return groupNode != null;
        }


        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public override void RestoreConfig()
        {
            base.RestoreConfig();
            ModuleConfig.CalcGroups.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public override ToolStripItem[] GetAddButtons()
        {
            btnAddGroup = new ToolStripMenuItem(ModulePhrases.AddGroupButton, Resources.folder_open);
            btnAddItem = new ToolStripMenuItem(ModulePhrases.AddItemButton, Resources.elem);
            return [btnAddGroup, btnAddItem];
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public override void HandleAddButtonClick(object button, TreeView treeView)
        {
            if (button == btnAddGroup)
            {
                treeView.Insert(groupsNode, CreateGroupNode(new CalcGroupConfig()));
            }
            else if (button == btnAddItem && 
                GetGroupNode(treeView.SelectedNode, out TreeNode groupNode))
            {
                treeView.Insert(groupNode, CreateItemNode(new ItemConfig()));
            }
        }

        /// <summary>
        /// Determines whether the specified action can be performed.
        /// </summary>
        public override bool AllowAction(ConfigAction action, object button, TreeNode selectedNode)
        {
            if (!base.AllowAction(action, button, selectedNode))
                return false;

            return action == ConfigAction.Add
                ? button == btnAddGroup || button == btnAddItem && GetGroupNode(selectedNode, out _)
                : selectedNode != optionsNode && selectedNode != groupsNode;
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
            optionsNode = TreeViewExtensions.CreateNode(
                ModulePhrases.GeneralOptionsNode, ImageKey.Options, ModuleConfig.GeneralOptions);
            groupsNode = TreeViewExtensions.CreateNode(
                ModulePhrases.GroupsNode, ImageKey.FolderClosed, ModuleConfig.CalcGroups);

            if (ModuleConfig.CalcGroups.Count > 0)
            {
                foreach (CalcGroupConfig calcGroupConfig in ModuleConfig.CalcGroups)
                {
                    groupsNode.Nodes.Add(CreateGroupNode(calcGroupConfig));
                }

                groupsNode.Expand();
            }

            return [optionsNode, groupsNode];
        }

        /// <summary>
        /// Gets an image key for the specified tree node.
        /// </summary>
        public override string GetNodeImage(TreeNode treeNode)
        {
            return treeNode == groupsNode || treeNode?.Tag is CalcGroupConfig
                ? (treeNode.IsExpanded ? ImageKey.FolderOpen : ImageKey.FolderClosed)
                : "";
        }

        /// <summary>
        /// Gets a tree node text for the specified object.
        /// </summary>
        public override string GetNodeText(object obj)
        {
            if (obj is GeneralOptions)
                return ModulePhrases.GeneralOptionsNode;
            else if (obj is CalcGroupConfig calcGroupConfig)
                return GetGroupNodeText(calcGroupConfig);
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
            return selectedNode == groupsNode ? null : selectedNode?.Tag;
        }
    }
}
