// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvSnmp.Config
{
    /// <summary>
    /// Represents a variable configuration.
    /// <para>Представляет конфигурацию переменной.</para>
    /// </summary>
    [Serializable]
    internal class VariableConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        [DisplayName, Category, Description]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the tag code associated with the variable.
        /// </summary>
        [DisplayName, Category, Description]
        public string TagCode { get; set; } = "";

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        [DisplayName, Category, Description]
        public string OID { get; set; } = "";

        /// <summary>
        /// Gets or sets the data type of the device tag associated with the variable.
        /// </summary>
        [DisplayName, Category, Description, NCM.DefaultValue(TagDataType.Double)]
        public TagDataType DataType { get; set; } = TagDataType.Double;

        /// <summary>
        /// Get or sets the data length if the variable represents a string or an array.
        /// </summary>
        [DisplayName, Category, Description, NCM.DefaultValue(0)]
        public int DataLen { get; set; } = 0;

        /// <summary>
        /// Gets a value indicating whether the tag data type is numeric.
        /// </summary>
        [NCM.Browsable(false)]
        public bool IsNumeric => DataType == TagDataType.Double || DataType == TagDataType.Int64;

        /// <summary>
        /// Gets or sets the parent tree node.
        /// </summary>
        [NCM.Browsable(false)]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Gets the child tree nodes.
        /// </summary>
        [NCM.Browsable(false)]
        public IList Children => null;


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Name = xmlElem.GetAttrAsString("name");
            TagCode = xmlElem.GetAttrAsString("tagCode");
            OID = xmlElem.GetAttrAsString("oid");
            DataType = xmlElem.GetAttrAsEnum("dataType", DataType);
            DataLen = xmlElem.GetAttrAsInt("dataLen");
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("name", Name);
            xmlElem.SetAttribute("tagCode", TagCode);
            xmlElem.SetAttribute("oid", OID);

            if (DataType != TagDataType.Double)
                xmlElem.SetAttribute("dataType", DataType);

            if (DataLen >= 1)
                xmlElem.SetAttribute("dataLen", DataLen);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, OID);
        }
    }
}
