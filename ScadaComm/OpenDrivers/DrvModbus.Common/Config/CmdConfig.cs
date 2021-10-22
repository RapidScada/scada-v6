// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;
using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents a command configuration.
    /// <para>Представляет конфигурацию команды.</para>
    /// </summary>
    public class CmdConfig : DataUnitConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CmdConfig()
            : base()
        {
            Multiple = false;
            ElemType = ElemType.Undefined;
            ElemCnt = 1;
            ByteOrder = "";
            CmdNum = 0;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the command writes multiple elements.
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// Gets or sets the command element type.
        /// </summary>
        public ElemType ElemType { get; set; }

        /// <summary>
        /// Gets or sets the number of elements written by the command.
        /// </summary>
        public int ElemCnt { get; set; }

        /// <summary>
        /// Gets or sets the byte order.
        /// </summary>
        public string ByteOrder { get; set; }

        /// <summary>
        /// Gets or sets the device command number.
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Gets a value indicating whether the data type selection is applicable for the command.
        /// </summary>
        public override bool ElemTypeEnabled
        {
            get
            {
                return DataBlock == DataBlock.HoldingRegisters && Multiple;
            }
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            DataBlock = xmlElem.GetAttrAsEnum("dataBlock", xmlElem.GetAttrAsEnum<DataBlock>("tableType"));
            Multiple = xmlElem.GetAttrAsBool("multiple");
            Address = xmlElem.GetAttrAsInt("address");
            ElemType = xmlElem.GetAttrAsEnum("elemType", DefaultElemType);
            ElemCnt = xmlElem.GetAttrAsInt("elemCnt", 1);
            ByteOrder = xmlElem.GetAttrAsString("byteOrder");
            CmdNum = xmlElem.GetAttrAsInt("cmdNum");
            Name = xmlElem.GetAttrAsString("name");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("dataBlock", DataBlock);
            xmlElem.SetAttribute("multiple", Multiple);
            xmlElem.SetAttribute("address", Address);

            if (ElemTypeEnabled)
                xmlElem.SetAttribute("elemType", ElemType.ToString().ToLowerInvariant());

            if (Multiple)
                xmlElem.SetAttribute("elemCnt", ElemCnt);

            if (ByteOrderEnabled)
                xmlElem.SetAttribute("byteOrder", ByteOrder);

            xmlElem.SetAttribute("cmdNum", CmdNum);
            xmlElem.SetAttribute("name", Name);
        }
    }
}
