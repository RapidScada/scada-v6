// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Dbms;
using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents an export target configuration.
    /// <para>Представляет конфигурацию цели экспорта.</para>
    /// </summary>
    [Serializable]
    internal class ExportTargetConfig : ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExportTargetConfig()
        {
            GeneralOptions = new GeneralOptions();
            ConnectionOptions = new DbConnectionOptions();
            ExportOptions = new ExportOptions();
            Queries = new QueryOptionList();
            Parent = null;
        }


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; }

        /// <summary>
        /// Gets the database connection options.
        /// </summary>
        public DbConnectionOptions ConnectionOptions { get; }

        /// <summary>
        /// Gets the export options.
        /// </summary>
        public ExportOptions ExportOptions { get; }

        /// <summary>
        /// Gets the list of query options.
        /// </summary>
        public QueryOptionList Queries { get; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        [field: NonSerialized]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        public IList Children => Queries;


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));

            if (xmlElem.SelectSingleNode("GeneralOptions") is XmlNode generalOptionsNode)
                GeneralOptions.LoadFromXml(generalOptionsNode);

            if (xmlElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                ConnectionOptions.LoadFromXml(connectionOptionsNode);

            if (xmlElem.SelectSingleNode("ExportOptions") is XmlElement exportOptionsElem)
                ExportOptions.LoadFromXml(exportOptionsElem);

            if (xmlElem.SelectSingleNode("Queries") is XmlNode queriesNode)
                Queries.LoadFromXml(queriesNode);
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            GeneralOptions.SaveToXml(xmlElem.AppendElem("GeneralOptions"));
            ConnectionOptions.SaveToXml(xmlElem.AppendElem("ConnectionOptions"));
            ExportOptions.SaveToXml(xmlElem.AppendElem("ExportOptions"));
            Queries.SaveToXml(xmlElem.AppendElem("Queries"));
        }
    }
}
