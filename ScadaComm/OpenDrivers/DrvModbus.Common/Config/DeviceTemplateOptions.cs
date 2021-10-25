// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;
using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents device template options.
    /// <para>Представляет параметры шаблона устройства.</para>
    /// </summary>
    public class DeviceTemplateOptions
    {
        private int[] byteOrderArr2; // the default byte order array for 2-byte elements
        private int[] byteOrderArr4; // the default byte order array for 4-byte elements
        private int[] byteOrderArr8; // the default byte order array for 8-byte elements


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTemplateOptions()
        {
            byteOrderArr2 = null;
            byteOrderArr4 = null;
            byteOrderArr8 = null;

            ZeroAddr = false;
            DecAddr = true;
            DefByteOrder2 = "";
            DefByteOrder4 = "";
            DefByteOrder8 = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether addresses are displayed starting from zero.
        /// </summary>
        public bool ZeroAddr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether addresses are displayed as decimals.
        /// </summary>
        public bool DecAddr { get; set; }

        /// <summary>
        /// Gets or sets the default byte order as string for 2-byte elements.
        /// </summary>
        public string DefByteOrder2 { get; set; }

        /// <summary>
        /// Gets or sets the default byte order as string for 4-byte elements.
        /// </summary>
        public string DefByteOrder4 { get; set; }

        /// <summary>
        /// Gets or sets the default byte order as string for 8-byte elements.
        /// </summary>
        public string DefByteOrder8 { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            byteOrderArr2 = null;
            byteOrderArr4 = null;
            byteOrderArr8 = null;

            ZeroAddr = xmlElem.GetChildAsBool("ZeroAddr", false);
            DecAddr = xmlElem.GetChildAsBool("DecAddr", true);
            DefByteOrder2 = xmlElem.GetChildAsString("DefByteOrder2");
            DefByteOrder4 = xmlElem.GetChildAsString("DefByteOrder4");
            DefByteOrder8 = xmlElem.GetChildAsString("DefByteOrder8");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("ZeroAddr", ZeroAddr);
            xmlElem.AppendElem("DecAddr", DecAddr);
            xmlElem.AppendElem("DefByteOrder2", DefByteOrder2);
            xmlElem.AppendElem("DefByteOrder4", DefByteOrder4);
            xmlElem.AppendElem("DefByteOrder8", DefByteOrder8);
        }

        /// <summary>
        /// Gets the appropriate default byte order as array.
        /// </summary>
        public int[] GetDefaultByteOrder(int byteCnt)
        {
            switch (byteCnt)
            {
                case 2:
                    if (byteOrderArr2 == null)
                        byteOrderArr2 = ModbusUtils.ParseByteOrder(DefByteOrder2);
                    return byteOrderArr2;

                case 4:
                    if (byteOrderArr4 == null)
                        byteOrderArr4 = ModbusUtils.ParseByteOrder(DefByteOrder4);
                    return byteOrderArr4;

                case 8:
                    if (byteOrderArr8 == null)
                        byteOrderArr8 = ModbusUtils.ParseByteOrder(DefByteOrder8);
                    return byteOrderArr8;

                default:
                    return null;
            }
        }
    }
}
