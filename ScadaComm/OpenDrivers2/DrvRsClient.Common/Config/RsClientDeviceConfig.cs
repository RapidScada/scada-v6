// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Storages;
using System.Xml;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    /// <summary>
    /// Represents a configuration of a device corresponding to a SCADA client.
    /// <para>Представляет конфигурацию устройства, соответствующего SCADA-клиенту.</para>
    /// </summary>
    [Serializable]
    public class RsClientDeviceConfig : DeviceConfigBase
    {
        /// <summary>
        /// Gets the item groups.
        /// </summary>
        public ItemGroupList ItemGroups { get; private set; }


        /// <summary>
        /// Gets the short name of the device configuration file.
        /// </summary>
        private static string GetFileName(int deviceNum)
        {
            return GetFileName(DriverUtils.DriverCode, deviceNum);
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            ItemGroups = [];
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected override void LoadFromXml(XmlDocument xmlDoc)
        {
            if (xmlDoc.DocumentElement.SelectSingleNode("ItemGroups") is XmlNode itemGroupsNode)
            {
                foreach (XmlElement itemGroupElem in itemGroupsNode.SelectNodes("ItemGroup"))
                {
                    ItemGroupConfig itemGroupConfig = new() { Parent = ItemGroups };
                    itemGroupConfig.LoadFromXml(itemGroupElem);
                    ItemGroups.Add(itemGroupConfig);
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
            XmlElement itemGroupsElem = rootElem.AppendElem("ItemGroups");

            foreach (ItemGroupConfig itemGroupConfig in ItemGroups)
            {
                itemGroupConfig.SaveToXml(itemGroupsElem.AppendElem("ItemGroup"));
            }
        }

        /// <summary>
        /// Loads the configuration from the specified storage.
        /// </summary>
        public bool Load(IStorage storage, int deviceNum, out string errMsg)
        {
            return Load(storage, GetFileName(deviceNum), out errMsg);
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string directory, int deviceNum, out string errMsg)
        {
            return Load(GetFullFileName(directory, deviceNum), out errMsg);
        }

        /// <summary>
        /// Gets the full name of the device configuration file.
        /// </summary>
        public static string GetFullFileName(string directory, int deviceNum)
        {
            return Path.Combine(directory, GetFileName(deviceNum));
        }
    }
}
