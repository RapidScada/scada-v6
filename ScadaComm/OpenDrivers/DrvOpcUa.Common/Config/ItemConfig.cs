// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents a monitored item configuration.
    /// <para>Представляет конфигурацию отслеживаемого элемента.</para>
    /// </summary>
    public class ItemConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ItemConfig()
        {
            Active = true;
            NodeID = "";
            DisplayName = "";
            TagCode = "";
            DataTypeName = "";
            IsArray = false;
            DataLen = 0;
            Tag = null;
        }

        
        /// <summary>
        /// Gets or sets a value indicating that the item is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the OPC UA node ID.
        /// </summary>
        public string NodeID { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the tag code associated with the item.
        /// </summary>
        public string TagCode { get; set; }

        /// <summary>
        /// Gets or sets the data type name of an OPC variable.
        /// </summary>
        public string DataTypeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the item data type is a string.
        /// </summary>
        public bool IsString
        {
            get
            {
                return DriverUtils.DataTypeEquals(DataTypeName, typeof(string));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that the item data type is an array.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Get or sets the data length if the item represents a string or an array.
        /// </summary>
        public int DataLen { get; set; }

        /// <summary>
        /// Gets the normalized data length.
        /// </summary>
        public int DataLength
        {
            get
            {
                return Math.Max(DataLen, 1);
            }
        }

        /// <summary>
        /// Gets or sets the object that contains data related to the item.
        /// </summary>
        public object Tag { get; set; }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            Active = xmlElem.GetAttrAsBool("active");
            NodeID = xmlElem.GetAttrAsString("nodeID");
            DisplayName = xmlElem.GetAttrAsString("displayName");
            TagCode = xmlElem.GetAttrAsString("tagCode");
            DataTypeName = xmlElem.GetAttrAsString("dataType");
            IsArray = xmlElem.GetAttrAsBool("isArray");
            DataLen = xmlElem.GetAttrAsInt("dataLen");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("nodeID", NodeID);
            xmlElem.SetAttribute("displayName", DisplayName);
            xmlElem.SetAttribute("tagCode", TagCode);
            xmlElem.SetAttribute("dataType", DataTypeName);
            xmlElem.SetAttribute("isArray", IsArray);

            if (IsString || IsArray)
                xmlElem.SetAttribute("dataLen", DataLength);
        }
    }
}
