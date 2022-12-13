// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents query options.
    /// <para>Представляет параметры запроса.</para>
    /// </summary>
    [Serializable]
    internal class QueryOptions : ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QueryOptions()
        {
            Active = true;
            Name = "";
            DataKind = DataKind.Current;
            Filter = new QueryFilter();
            SingleQuery = false;
            Sql = "";
            Parent = null;
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
        /// Gets or sets the data kind.
        /// </summary>
        public DataKind DataKind { get; set; }

        /// <summary>
        /// Gets the query filter.
        /// </summary>
        public QueryFilter Filter { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to execute a single SQL query 
        /// for all channels specified by the filter.
        /// </summary>
        public bool SingleQuery { get; set; }

        /// <summary>
        /// Gets or sets the SQL request.
        /// </summary>
        public string Sql { get; set; }


        /// <summary>
        /// Gets a value indicating whether a filter is applicable to the query options.
        /// </summary>
        public bool FilterApplicable => DataKind != DataKind.EventAck;

        /// <summary>
        /// Gets a value indicating whether an object filter is applicable to the query options.
        /// </summary>
        public bool ObjectFilterApplicable => DataKind == DataKind.Event || DataKind == DataKind.Command;

        /// <summary>
        /// Gets a value indicating whether the single query option is applicable to the query options.
        /// </summary>
        public bool SingleQueryApplicable => DataKind == DataKind.Current || DataKind == DataKind.Historical;


        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        public IList Children
        {
            get
            {
                return null;
            }
        }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active", Active);
            Name = xmlElem.GetAttrAsString("name", Name);
            DataKind = xmlElem.GetAttrAsEnum("dataKind", DataKind);

            if (xmlElem.SelectSingleNode("Filter") is XmlElement filterElem)
                Filter.LoadFromXml(filterElem);

            SingleQuery = xmlElem.GetChildAsBool("SingleQuery");
            Sql = xmlElem.GetChildAsString("Sql");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("dataKind", DataKind);

            Filter.SaveToXml(xmlElem.AppendElem("Filter"));
            xmlElem.AppendElem("SingleQuery", SingleQuery);
            xmlElem.AppendElem("Sql", Sql);
        }
    }
}
