// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using System.Xml;

namespace Scada.Comm.Drivers.DrvSnmp.Config
{
    /// <summary>
    /// Represents a configuration of an SNMP device.
    /// <para>Представляет конфигурацию SNMP-устройства.</para>
    /// </summary>
    [Serializable]
    internal class SnmpDeviceConfig : DeviceConfigBase
    {
        /// <summary>
        /// Gets the device options.
        /// </summary>
        public DeviceOptions DeviceOptions { get; private set; }

        /// <summary>
        /// Gets the variable groups.
        /// </summary>
        public VarGroupList VarGroups { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            DeviceOptions = new DeviceOptions();
            VarGroups = new VarGroupList();
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

            if (rootElem.SelectSingleNode("VarGroups") is XmlNode varGroupsNode)
            {
                foreach (XmlElement varGroupElem in varGroupsNode.SelectNodes("VarGroup"))
                {
                    VarGroupConfig varGroupConfig = new() { Parent = VarGroups };
                    varGroupConfig.LoadFromXml(varGroupElem);
                    VarGroups.Add(varGroupConfig);
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

            XmlElement rootElem = xmlDoc.CreateElement("SnmpDeviceConfig");
            xmlDoc.AppendChild(rootElem);

            DeviceOptions.SaveToXml(rootElem.AppendElem("DeviceOptions"));
            XmlElement varGroupsElem = rootElem.AppendElem("VarGroups");

            foreach (VarGroupConfig varGroupConfig in VarGroups)
            {
                varGroupConfig.SaveToXml(varGroupsElem.AppendElem("VarGroup"));
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
