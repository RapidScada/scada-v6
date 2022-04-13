// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Comm.Drivers.DrvMqttClient.View.Properties;
using Scada.Forms;

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


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MqttClientConfigProvider(string configDir, int deviceNum)
            : base()
        {
            ConfigFileName = Path.Combine(configDir, MqttClientDeviceConfig.GetFileName(deviceNum));
            Config = new MqttClientDeviceConfig();
        }


        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        public MqttClientDeviceConfig DeviceConfig => Config as MqttClientDeviceConfig;


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
            TreeNode subscriptionsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.SubscriptionsNode, ImageKey.FolderClosed);
            TreeNode commandsNode = TreeViewExtensions.CreateNode(
                DriverPhrases.CommandsNode, ImageKey.FolderClosed);

            foreach (SubscriptionConfig subscription in DeviceConfig.Subscriptions)
            {
                subscriptionsNode.Nodes.Add(
                    TreeViewExtensions.CreateNode(subscription.DisplayName, ImageKey.Elem, subscription));
            }

            foreach (CommandConfig command in DeviceConfig.Commands)
            {
                commandsNode.Nodes.Add(TreeViewExtensions.CreateNode(command.DisplayName, ImageKey.Cmd, command));
            }

            return new TreeNode[] { optionsNode, subscriptionsNode, commandsNode };
        }
    }
}
