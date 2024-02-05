// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Xml;

namespace Scada.Server.Modules.ModActiveDirectory.Config
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
        public const string DefaultFileName = "ModActiveDirectory.xml";


        /// <summary>
        /// Gets or sets the domain controller host or IP address.
        /// </summary>
        [DisplayName, Category, Description]
        public string LdapServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable user search in Active Directory and PostgreSQL.
        /// </summary>
        [DisplayName, Category, Description]
        public bool EnableSearch { get; set; }

        /// <summary>
        /// Gets or sets the search root in Active Directory.
        /// </summary>
        [DisplayName, Category, Description]
        public string SearchRoot { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            LdapServer = "";
            SearchRoot = "";
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            LdapServer = rootElem.GetChildAsString("LdapServer");
            EnableSearch = rootElem.GetChildAsBool("EnableSearch");
            SearchRoot = rootElem.GetChildAsString("SearchRoot");
        }
        
        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ModActiveDirectory");
            xmlDoc.AppendChild(rootElem);

            rootElem.AppendElem("LdapServer", LdapServer);
            rootElem.AppendElem("EnableSearch", EnableSearch);
            rootElem.AppendElem("SearchRoot", SearchRoot);

            xmlDoc.Save(writer);
        }
    }
}
