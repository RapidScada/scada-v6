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
            Active = true;
            Elems = new List<ElemConfig>();
            StartTagNum = 0;
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
        /// Gets a value indicating whether the read only flag is applicable for the elements.
        /// </summary>
        public virtual bool ReadOnlyEnabled
        {
            get
            {
                return DataBlock == DataBlock.Coils || DataBlock == DataBlock.HoldingRegisters;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the bit mask flag is applicable for the elements.
        /// </summary>
        public virtual bool BitMaskEnabled
        {
            get
            {
                return DataBlock == DataBlock.InputRegisters || DataBlock == DataBlock.HoldingRegisters;
            }
        }

        /// <summary>
        /// Gets or sets the device tag number that corresponds to the start element.
        /// </summary>
        public int StartTagNum { get; set; }


        /// <summary>
        /// Creates a new element configuration.
        /// </summary>
        public virtual ElemConfig CreateElemConfig()
        {
            return new ElemConfig { ElemType = DefaultElemType };
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
            bool defaultReadOnly = !ReadOnlyEnabled;
            bool defaultBitMask = !BitMaskEnabled;
            int maxElemCnt = MaxElemCnt;

            foreach (XmlElement elemElem in xmlElem.SelectNodes("Elem"))
            {
                if (Elems.Count >= maxElemCnt)
                    break;

                ElemConfig elemConfig = CreateElemConfig();
                elemConfig.ElemType = elemElem.GetAttrAsEnum("type", defaultElemType);
                elemConfig.ByteOrder = elemElem.GetAttrAsString("byteOrder");
                elemConfig.ReadOnly = elemElem.GetAttrAsBool("readOnly", defaultReadOnly);
                elemConfig.IsBitMask = elemElem.GetAttrAsBool("isBitMask", defaultBitMask);
                elemConfig.TagCode = elemElem.GetAttrAsString("tagCode");
                elemConfig.Name = elemElem.GetAttrAsString("name");
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
            bool readOnlyEnabled = ReadOnlyEnabled;
            bool bitMaskEnabled = BitMaskEnabled;

            foreach (ElemConfig elemConfig in Elems)
            {
                XmlElement elemElem = xmlElem.AppendElem("Elem");

                if (elemTypeEnabled)
                    elemElem.SetAttribute("type", elemConfig.ElemType.ToString().ToLowerInvariant());

                if (byteOrderEnabled && !string.IsNullOrEmpty(elemConfig.ByteOrder))
                    elemElem.SetAttribute("byteOrder", elemConfig.ByteOrder);

                if (readOnlyEnabled)
                    elemElem.SetAttribute("readOnly", elemConfig.ReadOnly);

                if (bitMaskEnabled)
                    elemElem.SetAttribute("isBitMask", elemConfig.IsBitMask);

                elemElem.SetAttribute("tagCode", elemConfig.TagCode);
                elemElem.SetAttribute("name", elemConfig.Name);
            }
        }
    }
}
