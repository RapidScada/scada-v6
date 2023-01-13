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
            public const string FolderOpen = "folder_open.png";
            public const string Options = "options.png";
        }


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SnmpConfigProvider(string configDir, int deviceNum)
            : base()
        {
            ConfigFileName = Path.Combine(configDir, SnmpDeviceConfig.GetFileName(deviceNum));
            Config = new SnmpDeviceConfig();
            FormTitle = string.Format(DriverPhrases.FormTitle, deviceNum);
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public SnmpDeviceConfig DeviceConfig => Config as SnmpDeviceConfig;


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
            /*itemsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.ItemsNode, ImageKey.FolderClosed, DeviceConfig.Items);

            foreach (ItemConfig item in DeviceConfig.Items)
            {
                itemsNode.Nodes.Add(CreateItemNode(item));
            }*/

            return new TreeNode[] { optionsNode };
        }
    }
}
