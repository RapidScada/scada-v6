// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Lang;
using Scada.Config;
using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttClient.Config
{
    /// <summary>
    /// Represents a configuration of an MQTT client device.
    /// <para>Представляет конфигурацию устройства MQTT-клиент.</para>
    /// </summary>
    [Serializable]
    public class MqttClientDeviceConfig : BaseConfig
    {
        /// <summary>
        /// Gets the device options.
        /// </summary>
        public DeviceOptions DeviceOptions { get; private set; }

        /// <summary>
        /// Gets the subscriptions.
        /// </summary>
        public SubscriptionList Subscriptions { get; private set; }

        /// <summary>
        /// Gets the commands.
        /// </summary>
        public CommandList Commands { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            DeviceOptions = new DeviceOptions();
            Subscriptions = new SubscriptionList();
            Commands = new CommandList();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("DeviceOptions") is XmlNode deviceOptionsNode)
                DeviceOptions.LoadFromXml(deviceOptionsNode);

            if (rootElem.SelectSingleNode("Subscriptions") is XmlNode subscriptionsNode)
            {
                foreach (XmlElement subscriptionElem in subscriptionsNode.SelectNodes("Subscription"))
                {
                    SubscriptionConfig subscriptionConfig = new() { Parent = Subscriptions };
                    subscriptionConfig.LoadFromXml(subscriptionElem);
                    Subscriptions.Add(subscriptionConfig);
                }
            }

            if (rootElem.SelectSingleNode("Commands") is XmlNode commandsNode)
            {
                foreach (XmlElement commandElem in commandsNode.SelectNodes("Command"))
                {
                    CommandConfig commandConfig = new() { Parent = Commands };
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
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("MqttClientDeviceConfig");
            xmlDoc.AppendChild(rootElem);

            DeviceOptions.SaveToXml(rootElem.AppendElem("DeviceOptions"));
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
        /// Builds an error message for the load operation.
        /// </summary>
        protected override string BuildLoadErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(CommPhrases.LoadDeviceConfigError);
        }

        /// <summary>
        /// Builds an error message for the save operation.
        /// </summary>
        protected override string BuildSaveErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(CommPhrases.SaveDeviceConfigError);
        }

        /// <summary>
        /// Gets the short name of the device configuration file.
        /// </summary>
        public static string GetFileName(int deviceNum)
        {
            return $"{DriverUtils.DriverCode}_{deviceNum:D3}.xml";
        }
    }
}
