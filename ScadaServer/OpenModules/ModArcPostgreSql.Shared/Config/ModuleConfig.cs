// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using Scada.Lang;
using System.Xml;

namespace Scada.Server.Modules.ModArcPostgreSql.Config
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    internal class ModuleConfig : ModuleConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ModArcPostgreSql.xml";


        /// <summary>
        /// Gets the connection options accessed by connection name.
        /// </summary>
        public SortedList<string, DbConnectionOptions> Connections { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Connections = [];
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
                    DbConnectionOptions connectionOptions = new();
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
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ModArcPostgreSql");
            xmlDoc.AppendChild(rootElem);

            XmlElement connectionsElem = rootElem.AppendElem("Connections");
            foreach (DbConnectionOptions connectionOptions in Connections.Values)
            {
                connectionOptions.SaveToXml(connectionsElem.AppendElem("Connection"));
            }

            xmlDoc.Save(writer);
        }


        /// <summary>
        /// Gets the connection options by name, or raises an exception.
        /// </summary>
        public DbConnectionOptions GetConnectionOptions(string connectionName)
        {
            return Connections.TryGetValue(connectionName, out DbConnectionOptions options)
                ? options
                : throw new ScadaException(CommonPhrases.ConnectionNotFound, connectionName);
        }
    }
}
