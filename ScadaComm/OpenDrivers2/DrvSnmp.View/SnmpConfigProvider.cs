// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvSnmp.Config;
using Scada.Comm.Drivers.DrvSnmp.View.Properties;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvSnmp.View
{
    /// <summary>
    /// Represents an intermediary between a driver configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией драйвера и формой конфигурации.</para>
    /// </summary>
    internal class SnmpConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Specifies the image keys for the configuration tree.
        /// </summary>
        private static class ImageKey
        {
            public const string Cmd = "cmd.png";
            public const string Elem = "elem.png";
            public const string FolderClosed = "folder_closed.png";
            public const string FolderClosedInactive = "folder_closed_inactive.png";
            public const string FolderOpen = "folder_open.png";
            public const string FolderOpenInactive = "folder_open_inactive.png";
            public const string Options = "options.png";
        }

        /// <summary>
        /// Specifies the tags of the add buttons.
        /// </summary>
        private static class AddButtonTag
        {
            public const string VarGroup = nameof(VarGroup);
            public const string Variable = nameof(Variable);
        }

        private TreeNode varGroupsNode;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SnmpConfigProvider(string configDir, int deviceNum)
            : base()
        {
            varGroupsNode = null;

            ConfigFileName = Path.Combine(configDir, SnmpDeviceConfig.GetFileName(deviceNum));
            Config = new SnmpDeviceConfig();
            FormTitle = string.Format(DriverPhrases.FormTitle, deviceNum);
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public SnmpDeviceConfig DeviceConfig => Config as SnmpDeviceConfig;


        /// <summary>
        /// Creates a tree node according to the variable group configuration.
        /// </summary>
        private static TreeNode CreateVarGroupNode(VarGroupConfig varGroupConfig)
        {
            TreeNode varGroupNode = TreeViewExtensions.CreateNode(GetVarGroupNodeText(varGroupConfig),
                ImageKey.FolderClosed, varGroupConfig);

            foreach (VariableConfig variableConfig in varGroupConfig.Variables)
            {
                varGroupNode.Nodes.Add(CreateVariableNode(variableConfig));
            }

            return varGroupNode;
        }

        /// <summary>
        /// Creates a tree node according to the variable configuration.
        /// </summary>
        private static TreeNode CreateVariableNode(VariableConfig variableConfig)
        {
            return TreeViewExtensions.CreateNode(GetVariableNodeText(variableConfig), ImageKey.Elem, variableConfig);
        }

        /// <summary>
        /// Gets a text for the subscription tree node.
        /// </summary>
        private static string GetVarGroupNodeText(VarGroupConfig varGroupConfig)
        {
            return string.IsNullOrEmpty(varGroupConfig.Name)
                ? DriverPhrases.UnnamedGroup
                : varGroupConfig.Name;
        }

        /// <summary>
        /// Gets a text for the variable tree node.
        /// </summary>
        private static string GetVariableNodeText(VariableConfig variableConfig)
        {
            return string.IsNullOrEmpty(variableConfig.Name) && string.IsNullOrEmpty(variableConfig.OID)
                ? DriverPhrases.UnnamedVariable
                : variableConfig.ToString();
        }


        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public override void RestoreConfig()
        {
            base.RestoreConfig();
            DeviceConfig.VarGroups.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public override ToolStripItem[] GetAddButtons()
        {
            return new ToolStripItem[]
            {
                new ToolStripMenuItem(DriverPhrases.AddVarGroupButton, Resources.folder_open)
                {
                    Tag = AddButtonTag.VarGroup
                },
                new ToolStripMenuItem(DriverPhrases.AddVariableButton, Resources.elem)
                {
                    Tag = AddButtonTag.Variable
                }
            };
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public override void HandleAddButtonClick(object button, TreeView treeView)
        {
            TreeNode parentNode = null;
            TreeNode nodeToInsert = null;
            object buttonTag = (button as ToolStripItem)?.Tag;

            if (buttonTag.Equals(AddButtonTag.VarGroup))
            {
                parentNode = varGroupsNode;
                nodeToInsert = CreateVarGroupNode(new VarGroupConfig());
            }
            else if (buttonTag.Equals(AddButtonTag.Variable))
            {
                parentNode = treeView.SelectedNode.FindClosest(typeof(VarGroupConfig));
                nodeToInsert = CreateVariableNode(new VariableConfig());
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

            return action == ConfigAction.Add ||
                selectedNode?.Tag is VarGroupConfig ||
                selectedNode?.Tag is VariableConfig;
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
                { ImageKey.FolderClosedInactive, Resources.folder_closed_inactive },
                { ImageKey.FolderOpen, Resources.folder_open },
                { ImageKey.FolderOpenInactive, Resources.folder_open_inactive },
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
            varGroupsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.VarGroupsNode, ImageKey.FolderClosed, DeviceConfig.VarGroups);

            foreach (VarGroupConfig varGroupConfig in DeviceConfig.VarGroups)
            {
                varGroupsNode.Nodes.Add(CreateVarGroupNode(varGroupConfig));
            }

            return new TreeNode[] { optionsNode, varGroupsNode };
        }

        /// <summary>
        /// Gets an image key for the specified object.
        /// </summary>
        public override string GetNodeImage(object obj, bool expanded)
        {
            if (obj is VarGroupList)
            {
                return expanded ? ImageKey.FolderOpen : ImageKey.FolderClosed;
            }
            else if (obj is VarGroupConfig varGroupConfig)
            {
                return varGroupConfig.Active
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
            if (obj is DeviceOptions)
                return DriverPhrases.OptionsNode;
            else if (obj is VarGroupConfig varGroupConfig)
                return GetVarGroupNodeText(varGroupConfig);
            else if (obj is VariableConfig variableConfig)
                return GetVariableNodeText(variableConfig);
            else
                return base.GetNodeText(obj);
        }

        /// <summary>
        /// Gets a selected object for editing its properties.
        /// </summary>
        public override object GetSelectedObject(TreeNode selectedNode)
        {
            object tag = selectedNode?.Tag;
            return tag is VarGroupList ? null : tag;
        }
    }
}
