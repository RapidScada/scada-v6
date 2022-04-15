// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a configuration of an OPC UA device.
    /// <para>Представляет конфигурацию устройства OPC UA.</para>
    /// </summary>
    public class OpcDeviceConfig : CustomDeviceConfig
    {
        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public OpcConnectionOptions ConnectionOptions { get; private set; }

        /// <summary>
        /// Gets the editing options.
        /// </summary>
        public EditingOptions EditingOptions { get; private set; }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        public List<SubscriptionConfig> Subscriptions { get; private set; }

        /// <summary>
        /// Gets the commands.
        /// </summary>
        public List<CommandConfig> Commands { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            ConnectionOptions = new OpcConnectionOptions();
            EditingOptions = new EditingOptions();
            Subscriptions = new List<SubscriptionConfig>();
            Commands = new List<CommandConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                ConnectionOptions.LoadFromXml(connectionOptionsNode);

            if (rootElem.SelectSingleNode("EditingOptions") is XmlNode editingOptionsNode)
                EditingOptions.LoadFromXml(editingOptionsNode);

            if (rootElem.SelectSingleNode("Subscriptions") is XmlNode subscriptionsNode)
            {
                foreach (XmlElement subscriptionElem in subscriptionsNode.SelectNodes("Subscription"))
                {
                    SubscriptionConfig subscriptionConfig = new SubscriptionConfig();
                    subscriptionConfig.LoadFromXml(subscriptionElem);
                    Subscriptions.Add(subscriptionConfig);
                }
            }

            if (rootElem.SelectSingleNode("Commands") is XmlNode commandsNode)
            {
                foreach (XmlElement commandElem in commandsNode.SelectNodes("Command"))
                {
                    CommandConfig commandConfig = new CommandConfig();
                    commandConfig.LoadFromXml(commandElem);
                    Commands.Add(commandConfig);
                }
            }
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("OpcDeviceConfig");
            xmlDoc.AppendChild(rootElem);

            ConnectionOptions.SaveToXml(rootElem.AppendElem("ConnectionOptions"));
            EditingOptions.SaveToXml(rootElem.AppendElem("EditingOptions"));
            XmlElement subscriptionsElem = rootElem.AppendElem("Subscriptions");
            XmlElement commandsElem = rootElem.AppendElem("Commands");

            foreach (SubscriptionConfig subscriptionConfig in Subscriptions)
            {
                subscriptionConfig.SaveToXml(subscriptionsElem.AppendElem("Subscription"));
            }

            foreach (CommandConfig commandConfig in Commands)
            {
                commandConfig.SaveToXml(commandsElem.AppendElem("Command"));
            }

            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Gets the short name of the device configuration file.
        /// </summary>
        public static string GetFileName(int deviceNum)
        {
            return GetFileName(DriverUtils.DriverCode, deviceNum);
        }
    }
}
