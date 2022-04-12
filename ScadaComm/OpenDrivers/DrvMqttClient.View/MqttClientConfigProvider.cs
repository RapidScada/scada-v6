// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Forms;

namespace Scada.Comm.Drivers.DrvMqttClient.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class MqttClientConfigProvider : ModuleConfigProvider
    {
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
        /// Gets images used by the configuration tree.
        /// </summary>
        public override Dictionary<string, Image> GetTreeViewImages()
        {
            return null;
        }

        /// <summary>
        /// Gets tree nodes to add to the configuration tree.
        /// </summary>
        public override TreeNode[] GetTreeNodes()
        {
            TreeNode optionsNode = TreeViewExtensions.CreateNode("Options", "", DeviceConfig.DeviceOptions);
            TreeNode subscriptionsNode = TreeViewExtensions.CreateNode("Subscriptions", "");
            TreeNode commandsNode = TreeViewExtensions.CreateNode("Commands", "");

            foreach (SubscriptionConfig subscription in DeviceConfig.Subscriptions)
            {
                subscriptionsNode.Nodes.Add(TreeViewExtensions.CreateNode(subscription.DisplayName, "", subscription));
            }

            foreach (CommandConfig command in DeviceConfig.Commands)
            {
                commandsNode.Nodes.Add(TreeViewExtensions.CreateNode(command.DisplayName, "", command));
            }

            return new TreeNode[] { optionsNode, subscriptionsNode, commandsNode };
        }
    }
}
