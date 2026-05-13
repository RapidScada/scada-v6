// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using System.Xml;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    /// <summary>
    /// Represents a configuration of a device corresponding to a SCADA client.
    /// <para>Представляет конфигурацию устройства, соответствующего SCADA-клиенту.</para>
    /// </summary>
    public class RsClientDeviceConfig : DeviceConfigBase
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        public List<ItemConfig> Items { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Items = [];
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected override void LoadFromXml(XmlDocument xmlDoc)
        {
            if (xmlDoc.DocumentElement.SelectSingleNode("Items") is XmlNode itemsNode)
            {
                foreach (XmlElement itemElem in itemsNode.SelectNodes("Item"))
                {
                    ItemConfig itemConfig = new();
                    itemConfig.LoadFromXml(itemElem);
                    Items.Add(itemConfig);
                }
            }
        }

        /// <summary>
        /// Saves the configuration into the XML document.
        /// </summary>
        protected override void SaveToXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.CreateElement("RsClientDeviceConfig");
            xmlDoc.AppendChild(rootElem);
            XmlElement itemsElem = rootElem.AppendElem("Items");

            foreach (ItemConfig itemConfig in Items)
            {
                itemConfig.SaveToXml(itemsElem.AppendElem("Item"));
            }
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
