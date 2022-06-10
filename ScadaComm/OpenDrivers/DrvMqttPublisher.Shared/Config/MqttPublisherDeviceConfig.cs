// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using System.Xml;

namespace Scada.Comm.Drivers.DrvMqttPublisher.Config
{
    /// <summary>
    /// Represents a configuration of an MQTT publisher device.
    /// <para>Представляет конфигурацию устройства для публикации данных с помощью MQTT.</para>
    /// </summary>
    [Serializable]
    internal class MqttPublisherDeviceConfig : DeviceConfigBase
    {
        /// <summary>
        /// Gets the device options.
        /// </summary>
        public DeviceOptions DeviceOptions { get; private set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ItemList Items { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            DeviceOptions = new DeviceOptions();
            Items = new ItemList();
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

            if (rootElem.SelectSingleNode("Items") is XmlNode itemsNode)
            {
                foreach (XmlElement itemElem in itemsNode.SelectNodes("Item"))
                {
                    ItemConfig itemConfig = new() { Parent = Items };
                    itemConfig.LoadFromXml(itemElem);
                    Items.Add(itemConfig);
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

            XmlElement rootElem = xmlDoc.CreateElement("MqttPublisherDeviceConfig");
            xmlDoc.AppendChild(rootElem);

            DeviceOptions.SaveToXml(rootElem.AppendElem("DeviceOptions"));
            XmlElement itemsElem = rootElem.AppendElem("Items");

            foreach (ItemConfig itemConfig in Items)
            {
                itemConfig.SaveToXml(itemsElem.AppendElem("Item"));
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
