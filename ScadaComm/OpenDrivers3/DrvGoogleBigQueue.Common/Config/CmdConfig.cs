// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol;
using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Config
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
            CustomFuncCode = 0;
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
        /// Gets the quantity of addresses.
        /// </summary>
        public virtual int Quantity
        {
            get
            {
                return BigQueueUtils.GetQuantity(ElemType);
            }
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Multiple = xmlElem.GetAttrAsBool("multiple");
            CustomFuncCode = xmlElem.GetAttrAsInt("funcCode");
            Address = xmlElem.GetAttrAsInt("address");
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

            xmlElem.SetAttribute("cmdNum", CmdNum);
            xmlElem.SetAttribute("cmdCode", CmdCode);
            xmlElem.SetAttribute("name", Name);
        }
    }
}
