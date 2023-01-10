// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Comm.Drivers.DrvDbImport.Config
{
    /// <summary>
    /// Represents a query configuration.
    /// <para>Представляет конфигурацию запроса.</para>
    /// </summary>
    internal class QueryConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueryConfig()
        {
            Active = true;
            Name = "";
            TagCodes = new List<string>();
            SingleRow = false;
            Sql = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the query is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the query name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the codes of the tags that are read by the query.
        /// </summary>
        public List<string> TagCodes { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the query should return a single row.
        /// </summary>
        public bool SingleRow { get; set; }

        /// <summary>
        /// Gets or sets the SQL request.
        /// </summary>
        public string Sql { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active", Active);
            Name = xmlElem.GetAttrAsString("name", Name);

            if (xmlElem.SelectSingleNode("Tags") is XmlNode tagsNode)
            {
                foreach (XmlNode tagNode in tagsNode.ChildNodes)
                {
                    TagCodes.Add(tagNode.InnerText);
                }
            }

            SingleRow = xmlElem.GetChildAsBool("SingleRow");
            Sql = xmlElem.GetChildAsString("Sql");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);

            XmlElement tagsElem = xmlElem.AppendElem("Tags");
            foreach (string tagCode in TagCodes)
            {
                tagsElem.AppendElem("Tag", tagCode);
            }

            xmlElem.AppendElem("SingleRow", SingleRow);
            xmlElem.AppendElem("Sql", Sql);
        }
    }
}
