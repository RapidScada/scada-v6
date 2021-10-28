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
            DataBlock = DataBlock.Coils;
            Multiple = false;
            CustomFuncCode = 0;
            ElemType = ElemType.Undefined;
            ElemCnt = 1;
            ByteOrder = "";
            CmdNum = 0;
            CmdCode = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the command writes multiple elements.
        /// </summary>
        public bool Multiple { get; set; }

        /// <summary>
        /// Gets or sets the custom function code.
        /// </summary>
        public int CustomFuncCode { get; set; }

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
        /// Gets or sets the command number.
        /// </summary>
        public int CmdNum { get; set; }

        /// <summary>
        /// Gets or sets the command code.
        /// </summary>
        public string CmdCode { get; set; }

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
            CustomFuncCode = xmlElem.GetAttrAsInt("funcCode");
            Address = xmlElem.GetAttrAsInt("address");
            ElemType = xmlElem.GetAttrAsEnum("elemType", DefaultElemType);
            ElemCnt = xmlElem.GetAttrAsInt("elemCnt", 1);
            ByteOrder = xmlElem.GetAttrAsString("byteOrder");
            CmdNum = xmlElem.GetAttrAsInt("cmdNum");
            CmdCode = xmlElem.GetAttrAsString("cmdCode");
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

            if (DataBlock == DataBlock.Custom)
            {
                xmlElem.SetAttribute("funcCode", CustomFuncCode);
            }
            else
            {
                xmlElem.SetAttribute("address", Address);

                if (ElemTypeEnabled)
                    xmlElem.SetAttribute("elemType", ElemType.ToString().ToLowerInvariant());

                if (Multiple)
                    xmlElem.SetAttribute("elemCnt", ElemCnt);

                if (ByteOrderEnabled && !string.IsNullOrEmpty(ByteOrder))
                    xmlElem.SetAttribute("byteOrder", ByteOrder);
            }

            xmlElem.SetAttribute("cmdNum", CmdNum);
            xmlElem.SetAttribute("cmdCode", CmdCode);
            xmlElem.SetAttribute("name", Name);
        }
    }
}
