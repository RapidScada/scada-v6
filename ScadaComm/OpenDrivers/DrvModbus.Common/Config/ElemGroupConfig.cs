// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents an element group configuration.
    /// <para>Представляет конфигурацию группы элементов.</para>
    /// </summary>
    public class ElemGroupConfig : DataUnitConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ElemGroupConfig()
            : base()
        {
            Active = false;
            Elems = new List<ElemConfig>();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the element group is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets the configuration of the elements.
        /// </summary>
        public List<ElemConfig> Elems { get; private set; }


        /// <summary>
        /// Creates a new element configuration.
        /// </summary>
        protected virtual ElemConfig CreateElemConfig()
        {
            return new ElemConfig();
        }

        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active");
            DataBlock = xmlElem.GetAttrAsEnum("dataBlock", xmlElem.GetAttrAsEnum<DataBlock>("tableType"));
            Address = xmlElem.GetAttrAsInt("address");
            Name = xmlElem.GetAttrAsString("name");

            ElemType defaultElemType = DefaultElemType;
            int maxElemCnt = MaxElemCnt;

            foreach (XmlElement elemElem in xmlElem.SelectNodes("Elem"))
            {
                if (Elems.Count >= maxElemCnt)
                    break;

                ElemConfig elemConfig = CreateElemConfig();
                elemConfig.Name = elemElem.GetAttrAsString("name");
                elemConfig.ElemType = elemElem.GetAttrAsEnum("type", defaultElemType);
                elemConfig.ByteOrder = elemElem.GetAttrAsString("byteOrder");
                Elems.Add(elemConfig);
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("dataBlock", DataBlock);
            xmlElem.SetAttribute("address", Address);
            xmlElem.SetAttribute("name", Name);

            bool elemTypeEnabled = ElemTypeEnabled;
            bool byteOrderEnabled = ByteOrderEnabled;

            foreach (ElemConfig elemConfig in Elems)
            {
                XmlElement elemElem = xmlElem.AppendElem("Elem");
                elemElem.SetAttribute("name", elemConfig.Name);

                if (elemTypeEnabled)
                    elemElem.SetAttribute("type", elemConfig.ElemType.ToString().ToLowerInvariant());

                if (byteOrderEnabled && !string.IsNullOrEmpty(elemConfig.ByteOrder))
                    elemElem.SetAttribute("byteOrder", elemConfig.ByteOrder);
            }
        }
    }
}
