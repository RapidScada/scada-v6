// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using Scada.Config;
using System.IO;
using System.Xml;

namespace Scada.Admin.Extensions.ExtDepPostgreSql.Config
{
    /// <summary>
    /// Represents an extension configuration.
    /// <para>Представляет конфигурацию расширения.</para>
    /// </summary>
    internal class ExtensionConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ExtDepPostgreSql.xml";

        /// <summary>
        /// Gets or sets the method for clearing the configuration database.
        /// </summary>
        [DisplayName, Category, Description]
        public ClearBaseMethod ClearBaseMethod { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            ClearBaseMethod = ClearBaseMethod.DropTables;
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;
            ClearBaseMethod = rootElem.GetChildAsEnum("ClearBaseMethod", ClearBaseMethod);
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ExtDepPostgreSql");
            rootElem.AppendElem("ClearBaseMethod", ClearBaseMethod);

            xmlDoc.AppendChild(rootElem);
            xmlDoc.Save(writer);
        }
    }
}
