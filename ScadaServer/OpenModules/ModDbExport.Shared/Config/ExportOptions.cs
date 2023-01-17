// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents export options.
    /// <para>Представляет параметры экспорта.</para>
    /// </summary>
    [Serializable]
    internal class ExportOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ExportOptions()
        {
            CurDataExportOptions = new CurDataExportOptions();
            ArcReplicationOptions = new ArcReplicationOptions();
        }


        /// <summary>
        /// Gets the current data export options.
        /// </summary>
        public CurDataExportOptions CurDataExportOptions { get; }

        /// <summary>
        /// Gets the archive replication options.
        /// </summary>
        public ArcReplicationOptions ArcReplicationOptions { get; }
        
        
        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));

            if (xmlNode.SelectSingleNode("CurDataExportOptions") is XmlElement xmlElem1)
                CurDataExportOptions.LoadFromXml(xmlElem1);

            if (xmlNode.SelectSingleNode("ArcReplicationOptions") is XmlElement xmlElem2)
                ArcReplicationOptions.LoadFromXml(xmlElem2);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            CurDataExportOptions.SaveToXml(xmlElem.AppendElem("CurDataExportOptions"));
            ArcReplicationOptions.SaveToXml(xmlElem.AppendElem("ArcReplicationOptions"));
        }
    }
}
