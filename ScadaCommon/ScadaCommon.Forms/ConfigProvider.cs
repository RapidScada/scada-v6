// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Scada.Forms
{
    /// <summary>
    /// Represents an intermediary between a configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией и формой конфигурации.</para>
    /// </summary>
    public abstract class ConfigProvider
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConfigProvider()
        {
            ConfigFileName = "";
            Config = null;
            ConfigCopy = null;
            FormTitle = "";
            GridToolbarVisible = false;
            GridHelpVisible = true;
            GridSort = PropertySort.CategorizedAlphabetical;
        }


        /// <summary>
        /// Gets or sets the configuration file name.
        /// </summary>
        protected string ConfigFileName { get; set; }

        /// <summary>
        /// Gets or sets the module configuration.
        /// </summary>
        protected ConfigBase Config { get; set; }

        /// <summary>
        /// Gets or sets the copy of the module configuration.
        /// </summary>
        protected ConfigBase ConfigCopy { get; set; }

        /// <summary>
        /// Get the title of the configuration form.
        /// </summary>
        public string FormTitle { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the property grid toolbar is visible.
        /// </summary>
        public bool GridToolbarVisible { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether to show the description panel of the property grid.
        /// </summary>
        public bool GridHelpVisible { get; protected set; }

        /// <summary>
        /// Gets the type of property grid sorting.
        /// </summary>
        public PropertySort GridSort { get; protected set; }


        /// <summary>
        /// Loads the configuration.
        /// </summary>
        public virtual bool LoadConfig(out string errMsg)
        {
            if (Config != null && File.Exists(ConfigFileName))
            {
                return Config.Load(ConfigFileName, out errMsg);
            }
            else
            {
                errMsg = "";
                return true;
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        public virtual bool SaveConfig(out string errMsg)
        {
            if (Config == null)
            {
                errMsg = "";
                return true;
            }
            else
            {
                return Config.Save(ConfigFileName, out errMsg);
            }
        }

        /// <summary>
        /// Stores a configuration copy.
        /// </summary>
        public virtual void BackupConfig()
        {
            ConfigCopy = Config?.DeepClone();
        }

        /// <summary>
        /// Restores a configuration from the copy.
        /// </summary>
        public virtual void RestoreConfig()
        {
            Config = ConfigCopy;
            ConfigCopy = Config?.DeepClone();

            if (Config is ITreeNode treeNode)
                treeNode.RestoreHierarchy();
        }

        /// <summary>
        /// Gets toolbar buttons for adding new items.
        /// </summary>
        public virtual ToolStripItem[] GetAddButtons()
        {
            return null;
        }

        /// <summary>
        /// Handles a click on the add item button.
        /// </summary>
        public virtual void HandleAddButtonClick(object button, TreeView treeView)
        {
        }

        /// <summary>
        /// Determines if the specified action can be performed.
        /// </summary>
        public virtual bool AllowAction(ConfigAction action, object button, TreeNode selectedNode)
        {
            return action switch
            {
                ConfigAction.MoveUp => 
                    TreeViewExtensions.MoveUpIsEnabled(selectedNode, TreeNodeBehavior.WithinParent),

                ConfigAction.MoveDown => 
                    TreeViewExtensions.MoveDownIsEnabled(selectedNode, TreeNodeBehavior.WithinParent),

                ConfigAction.Delete => selectedNode != null,

                _ => true
            };
        }

        /// <summary>
        /// Gets images used by the configuration tree.
        /// </summary>
        public virtual Dictionary<string, Image> GetTreeViewImages()
        {
            return null;
        }

        /// <summary>
        /// Gets tree nodes to add to the configuration tree.
        /// </summary>
        public virtual TreeNode[] GetTreeNodes()
        {
            return null;
        }

        /// <summary>
        /// Gets an image key for the specified tree node.
        /// </summary>
        /// <remarks>An empty string keeps the node image unchanged.</remarks>
        public virtual string GetNodeImage(TreeNode treeNode)
        {
            return treeNode == null ? "" : GetNodeImage(treeNode.Tag, treeNode.IsExpanded);
        }

        /// <summary>
        /// Gets an image key for the specified object.
        /// </summary>
        public virtual string GetNodeImage(object obj, bool expanded)
        {
            return "";
        }

        /// <summary>
        /// Gets a tree node text for the specified object.
        /// </summary>
        public virtual string GetNodeText(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        /// <summary>
        /// Gets a selected object for editing its properties.
        /// </summary>
        public virtual object GetSelectedObject(TreeNode selectedNode)
        {
            return selectedNode?.Tag;
        }
    }
}
