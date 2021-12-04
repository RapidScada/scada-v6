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
            IsString = false;
            IsArray = false;
            ArrayLen = 0;
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
        /// Gets or sets a value indicating that the item data type is a string.
        /// </summary>
        public bool IsString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the item data type is an array.
        /// </summary>
        public bool IsArray { get; set; }

        /// <summary>
        /// Get or sets the array length if the item represents an array.
        /// </summary>
        public int ArrayLen { get; set; }

        /// <summary>
        /// Gets the normalized array length.
        /// </summary>
        public int ArrayLength
        {
            get
            {
                return Math.Max(ArrayLen, 1);
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
            IsString = xmlElem.GetAttrAsBool("isString");
            IsArray = xmlElem.GetAttrAsBool("isArray");
            ArrayLen = xmlElem.GetAttrAsInt("arrayLen");
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
            xmlElem.SetAttribute("isString", IsString);
            xmlElem.SetAttribute("isArray", IsArray);

            if (IsString || IsArray)
                xmlElem.SetAttribute("arrayLen", ArrayLen);
        }
    }
}
