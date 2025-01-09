// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDeviceAlarm.Config
{
    /// <summary>
    /// Represents a list of query options.
    /// <para>Представляет список параметров запросов.</para>
    /// </summary>
    [Serializable]
    internal class TriggerOptionList : List<TriggerOptions>, ITreeNode
    {
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
                return this;
            }
        }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));

            foreach (XmlElement queryElem in xmlNode.SelectNodes("Trigger"))
            {
                TriggerOptions queryOptions = new() { Parent = this };
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

            foreach (TriggerOptions queryOptions in this)
            {
                queryOptions.SaveToXml(xmlElem.AppendElem("Trigger"));
            }
        }
    }
}
