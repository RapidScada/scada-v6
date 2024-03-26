// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents historical data export options.
    /// <para>Представляет параметры экспорта исторических данных.</para>
    /// </summary>
    [Serializable]
    internal class HistDataExportOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HistDataExportOptions()
        {
            IncludeCalculated = false;
            ExportCalculatedDelay = 10;
            HistArchiveBit = ArchiveBit.Minute;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to export data of calculated channels.
        /// </summary>
        public bool IncludeCalculated { get; set; }

        /// <summary>
        /// Gets the delay before exporting calculated historical data, seconds.
        /// </summary>
        public int ExportCalculatedDelay { get; set; }

        /// <summary>
        /// Gets the bit number of the historical archive used for export the calculated data.
        /// </summary>
        public int HistArchiveBit { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            IncludeCalculated = xmlElem.GetChildAsBool("IncludeCalculated", IncludeCalculated);
            ExportCalculatedDelay = xmlElem.GetChildAsInt("ExportCalculatedDelay", ExportCalculatedDelay);
            HistArchiveBit = xmlElem.GetChildAsInt("HistArchiveBit", HistArchiveBit);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("IncludeCalculated", IncludeCalculated);
            xmlElem.AppendElem("ExportCalculatedDelay", ExportCalculatedDelay);
            xmlElem.AppendElem("HistArchiveBit", HistArchiveBit);
        }
    }
}
