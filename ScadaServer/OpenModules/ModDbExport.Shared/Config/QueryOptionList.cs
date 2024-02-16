// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents a list of query options.
    /// <para>Представляет список параметров запросов.</para>
    /// </summary>
    [Serializable]
    internal class QueryOptionList : List<QueryOptions>, ITreeNode
    {
        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        [field: NonSerialized]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        public IList Children
        {
            get
            {
                return this;
            }
        }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));

            foreach (XmlElement queryElem in xmlNode.SelectNodes("Query"))
            {
                QueryOptions queryOptions = new() { Parent = this };
                queryOptions.LoadFromXml(queryElem);
                Add(queryOptions);
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));

            foreach (QueryOptions queryOptions in this)
            {
                queryOptions.SaveToXml(xmlElem.AppendElem("Query"));
            }
        }
    }
}
