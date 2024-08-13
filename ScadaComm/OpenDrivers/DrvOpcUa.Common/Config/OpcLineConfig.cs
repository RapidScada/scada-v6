// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a configuration of an OPC communication line.
    /// <para>Представляет конфигурацию линии связи OPC.</para>
    /// </summary>
    public class OpcLineConfig : ConfigBase
    {
        /// <summary>
        /// Gets the connection options.
        /// </summary>
        public OpcConnectionOptions ConnectionOptions { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            ConnectionOptions = new OpcConnectionOptions();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                ConnectionOptions.LoadFromXml(connectionOptionsNode);
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("OpcLineConfig");
            xmlDoc.AppendChild(rootElem);

            ConnectionOptions.SaveToXml(rootElem.AppendElem("ConnectionOptions"));
            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Gets the short name of the line configuration file.
        /// </summary>
        public static string GetFileName(int lineNum)
        {
            return $"{DriverUtils.DriverCode}_line{lineNum:D3}.xml";
        }
    }
}
