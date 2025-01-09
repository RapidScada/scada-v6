// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using System.Xml;

namespace Scada.Server.Modules.ModDeviceAlarm.Config
{
    /// <summary>
    /// Represents options for archive replication.
    /// <para>Представляет параметры репликации архивов.</para>
    /// </summary>
    [Serializable]
    internal class ArcReplicationOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcReplicationOptions()
        {
            Enabled = true;
            AutoExport = false;
            MinDepth = 10;
            MaxDepth = 3600;
            ReadingStep = 60;
            HistArchiveBit = ArchiveBit.Minute;
            EventArchiveBit = ArchiveBit.Event;
        }


        /// <summary>
        /// Gets or sets a value indicating whether to replicate data.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically export archives.
        /// </summary>
        public bool AutoExport { get; set; }

        /// <summary>
        /// Gets or sets the right end of the time interval of exported data, relative to the current time, in seconds.
        /// </summary>
        public int MinDepth { get; set; }

        /// <summary>
        /// Gets or sets the left end of the time interval of exported data, relative to the current time, in seconds.
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the width of the time interval of exported data, in seconds.
        /// </summary>
        public int ReadingStep { get; set; }

        /// <summary>
        /// Gets the bit number of the historical archive used for export.
        /// </summary>
        public int HistArchiveBit { get; set; }

        /// <summary>
        /// Gets the bit number of the event archive used for export.
        /// </summary>
        public int EventArchiveBit { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Enabled = xmlElem.GetChildAsBool("Enabled", Enabled);
            AutoExport = xmlElem.GetChildAsBool("AutoExport", AutoExport);
            MinDepth = xmlElem.GetChildAsInt("MinDepth", MinDepth);
            MaxDepth = xmlElem.GetChildAsInt("MaxDepth", MaxDepth);
            ReadingStep = xmlElem.GetChildAsInt("ReadingStep", ReadingStep);
            HistArchiveBit = xmlElem.GetChildAsInt("HistArchiveBit", HistArchiveBit);
            EventArchiveBit = xmlElem.GetChildAsInt("EventArchiveBit", EventArchiveBit);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("Enabled", Enabled);
            xmlElem.AppendElem("AutoExport", AutoExport);
            xmlElem.AppendElem("MinDepth", MinDepth);
            xmlElem.AppendElem("MaxDepth", MaxDepth);
            xmlElem.AppendElem("ReadingStep", ReadingStep);
            xmlElem.AppendElem("HistArchiveBit", HistArchiveBit);
            xmlElem.AppendElem("EventArchiveBit", EventArchiveBit);
        }
    }
}
