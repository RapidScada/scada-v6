// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModArcInfluxDb.Config
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    internal class ModuleConfig : ModuleConfigBase
    {
        /// <summary>
        /// The configuration file name.
        /// </summary>
        private const string ConfigFileName = "ModArcInfluxDb.xml";


        /// <summary>
        /// Gets the connection options accessed by connection name.
        /// </summary>
        public SortedList<string, ConnectionOptions> Connections { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Connections = new SortedList<string, ConnectionOptions>();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);

            if (xmlDoc.DocumentElement.SelectSingleNode("Connections") is XmlNode connectionsNode)
            {
                foreach (XmlNode connectionNode in connectionsNode.SelectNodes("Connection"))
                {
                    ConnectionOptions connectionOptions = new();
                    connectionOptions.LoadFromXml(connectionNode);
                    Connections[connectionOptions.Name] = connectionOptions;
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

            XmlElement rootElem = xmlDoc.CreateElement("ModArcInfluxDb");
            xmlDoc.AppendChild(rootElem);

            XmlElement connectionsElem = rootElem.AppendElem("Connections");
            foreach (ConnectionOptions connectionOptions in Connections.Values)
            {
                connectionOptions.SaveToXml(connectionsElem.AppendElem("Connection"));
            }

            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Gets the full name of the configuration file.
        /// </summary>
        public static string GetFilePath(string configDir)
        {
            return Path.Combine(configDir, ConfigFileName);
        }
    }
}
