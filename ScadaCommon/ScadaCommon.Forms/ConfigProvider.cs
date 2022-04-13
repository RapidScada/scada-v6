// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;
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
        }


        /// <summary>
        /// Gets or sets the configuration file name.
        /// </summary>
        protected string ConfigFileName { get; set; }

        /// <summary>
        /// Gets or sets the module configuration.
        /// </summary>
        protected BaseConfig Config { get; set; }

        /// <summary>
        /// Gets or sets the copy of the module configuration.
        /// </summary>
        protected BaseConfig ConfigCopy { get; set; }


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
        /// Selects a node image key corresponding to the specified object.
        /// </summary>
        public virtual string ChooseNodeImage(TreeNode treeNode, bool expanded = false)
        {
            ArgumentNullException.ThrowIfNull(treeNode, nameof(treeNode));

            if (treeNode.ImageKey.StartsWith("folder_"))
                return expanded ? "folder_open.png" : "folder_closed.png";
            else
                return treeNode.ImageKey;
        }
    }
}
