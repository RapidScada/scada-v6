// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents current data export options.
    /// <para>Представляет параметры экспорта текущих данных.</para>
    /// </summary>
    [Serializable]
    internal class CurDataExportOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CurDataExportOptions()
        {
            Trigger = ExportTrigger.OnReceive;
            TimerPeriod = 10;
            AllDataPeriod = 60;
            SkipUnchanged = false;
            IncludeCalculated = false;
        }


        /// <summary>
        /// Gets or sets the trigger.
        /// </summary>
        public ExportTrigger Trigger { get; set; }

        /// <summary>
        /// Gets or sets the timer period, in seconds.
        /// </summary>
        public int TimerPeriod { get; set; }

        /// <summary>
        /// Gets or sets the period for exporting all channel data, in seconds.
        /// </summary>
        public int AllDataPeriod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip unchanged measured data.
        /// </summary>
        public bool SkipUnchanged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to export data of calculated channels.
        /// </summary>
        public bool IncludeCalculated { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Trigger = xmlElem.GetChildAsEnum("Trigger", Trigger);
            TimerPeriod = xmlElem.GetChildAsInt("TimerPeriod", TimerPeriod);
            AllDataPeriod = xmlElem.GetChildAsInt("AllDataPeriod", AllDataPeriod);
            SkipUnchanged = xmlElem.GetChildAsBool("SkipUnchanged", SkipUnchanged);
            IncludeCalculated = xmlElem.GetChildAsBool("IncludeCalculated", IncludeCalculated);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("Trigger", Trigger);
            xmlElem.AppendElem("TimerPeriod", TimerPeriod);
            xmlElem.AppendElem("AllDataPeriod", AllDataPeriod);
            xmlElem.AppendElem("SkipUnchanged", SkipUnchanged);
            xmlElem.AppendElem("IncludeCalculated", IncludeCalculated);
        }
    }
}
