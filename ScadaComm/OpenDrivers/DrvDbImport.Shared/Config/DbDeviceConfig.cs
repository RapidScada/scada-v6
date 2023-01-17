// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using System.Xml;

namespace Scada.Comm.Drivers.DrvDbImport.Config
{
    /// <summary>
    /// Represents a configuration of a DB import device.
    /// <para>Представляет конфигурацию устройства импорта из БД.</para>
    /// </summary>
    [Serializable]
    internal class DbDeviceConfig : DeviceConfigBase
    {
        /// <summary>
        /// Gets the queries.
        /// </summary>
        public List<QueryConfig> Queries { get; private set; }

        /// <summary>
        /// Gets the commands.
        /// </summary>
        public List<CommandConfig> Commands { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Queries = new List<QueryConfig>();
            Commands = new List<CommandConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("Queries") is XmlNode queriesNode)
            {
                foreach (XmlElement queryElem in queriesNode.SelectNodes("Query"))
                {
                    QueryConfig queryConfig = new();
                    queryConfig.LoadFromXml(queryElem);
                    Queries.Add(queryConfig);
                }
            }

            if (rootElem.SelectSingleNode("Commands") is XmlNode commandsNode)
            {
                foreach (XmlElement commandElem in commandsNode.SelectNodes("Command"))
                {
                    CommandConfig commandConfig = new();
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

            XmlElement rootElem = xmlDoc.CreateElement("DbDeviceConfig");
            xmlDoc.AppendChild(rootElem);

            XmlElement queriesElem = rootElem.AppendElem("Queries");
            XmlElement commandsElem = rootElem.AppendElem("Commands");

            foreach (QueryConfig queryConfig in Queries)
            {
                queryConfig.SaveToXml(queriesElem.AppendElem("Query"));
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
