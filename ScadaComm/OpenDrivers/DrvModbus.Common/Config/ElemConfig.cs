// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;
using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents an element configuration.
    /// <para>Представляет конфигурацию элемента.</para>
    /// </summary>
    public class ElemConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ElemConfig()
        {
            Name = "";
            ElemType = ElemType.Undefined;
            ByteOrder = "";
        }


        /// <summary>
        /// Gets or sets the element name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the element type.
        /// </summary>
        public ElemType ElemType { get; set; }

        /// <summary>
        /// Gets or sets the byte order.
        /// </summary>
        public string ByteOrder { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Name = xmlElem.GetAttrAsString("name");
            ElemType = xmlElem.GetAttrAsEnum("elemType", ElemType.Bool);
            ByteOrder = xmlElem.GetAttrAsString("byteOrder");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("elemType", ElemType);

            if (!string.IsNullOrEmpty(ByteOrder))
                xmlElem.SetAttribute("byteOrder", ByteOrder);
        }
    }
}
