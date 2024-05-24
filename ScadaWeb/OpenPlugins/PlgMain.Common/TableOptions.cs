// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMain
{
    /// <summary>
    /// Represents table view options.
    /// <para>Представляет параметры табличного представления.</para>
    /// </summary>
    public class TableOptions
    {
        /// <summary>
        /// The default period of the table view columns, in minutes.
        /// </summary>
        public const int DefaultPeriod = 60;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TableOptions()
        {
            UseDefault = true;
            ArchiveCode = "";
            Period = DefaultPeriod;
            ChartArgs = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether to use the default table view options.
        /// </summary>
        public bool UseDefault { get; set; }

        /// <summary>
        /// Gets or sets the archive code to get data for the table view.
        /// </summary>
        public string ArchiveCode { get; set; }

        /// <summary>
        /// Gets or sets the time period of the table view columns, in minutes.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets the arguments for displaying charts.
        /// </summary>
        public string ChartArgs { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            UseDefault = xmlNode.GetChildAsBool("UseDefault", UseDefault);
            ArchiveCode = xmlNode.GetChildAsString("ArchiveCode", ArchiveCode);
            Period = xmlNode.GetChildAsInt("Period", Period);
            ChartArgs = xmlNode.GetChildAsString("ChartArgs", ChartArgs);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("UseDefault", UseDefault);

            if (!UseDefault)
            {
                xmlElem.AppendElem("ArchiveCode", ArchiveCode);
                xmlElem.AppendElem("Period", Period);
                xmlElem.AppendElem("ChartArgs", ChartArgs);
            }
        }
    }
}
