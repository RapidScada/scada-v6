// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDeviceAlarm.Config
{
    /// <summary>
    /// Represents query options.
    /// <para>Представляет параметры запроса.</para>
    /// </summary>
    [Serializable]
    internal class TriggerOptions : ITreeNode
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TriggerOptions()
        {
            Active = true;
            Name = "";
            TriggerKind = TriggerKind.Status;
            Filter = new QueryFilter();
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
        public TriggerKind TriggerKind { get; set; }

        /// <summary>
        /// Gets the query filter.
        /// </summary>
        public QueryFilter Filter { get; }

        /// <summary>
        /// 通道状态值-告警
        /// </summary>
        public int StatusCnlNum { get; set; }

        /// <summary>
        /// 通道状态值持续时间-告警
        /// </summary>
        public int StatusPeriod { get; set; }

        /// <summary>
        /// 通道值采样频率-告警
        /// </summary>
        public int DataUnchangedPeriod { get; set; }

        /// <summary>
        /// 通道值判断次数-告警
        /// </summary>
        public int DataUnchangedNumber { get; set; }


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
            TriggerKind = xmlElem.GetAttrAsEnum("dataKind", TriggerKind);
            StatusCnlNum = xmlElem.GetAttrAsInt("statusCnlNum", StatusCnlNum);
            StatusPeriod = xmlElem.GetAttrAsInt("statusPeriod", StatusPeriod);
            DataUnchangedPeriod = xmlElem.GetAttrAsInt("dataUnchangedPeriod", DataUnchangedPeriod);
            DataUnchangedNumber = xmlElem.GetAttrAsInt("dataUnchangedNumber", DataUnchangedNumber);

            if (xmlElem.SelectSingleNode("Filter") is XmlElement filterElem)
                Filter.LoadFromXml(filterElem);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("dataKind", TriggerKind);
            xmlElem.SetAttribute("statusCnlNum", StatusCnlNum);
            xmlElem.SetAttribute("statusPeriod", StatusPeriod);
            xmlElem.SetAttribute("dataUnchangedPeriod", DataUnchangedPeriod);
            xmlElem.SetAttribute("dataUnchangedNumber", DataUnchangedNumber);

            Filter.SaveToXml(xmlElem.AppendElem("Filter"));
        }
    }
}
