// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents an image of a mimic diagram.
    /// <para>Представляет изображение мнемосхемы.</para>
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or sets the image name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] Data { get; set; } = null;

        /// <summary>
        /// Gets the data size.
        /// </summary>
        public int DataSize => Data == null ? 0 : Data.Length;


        /// <summary>
        /// Loads the image from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            Name = xmlNode.GetChildAsString("Name");
            Data = Convert.FromBase64String(xmlNode.GetChildAsString("Data"));
        }

        /// <summary>
        /// Saves the component into the XML node.
        /// </summary>
        public void SaveToXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            xmlNode.AppendElem("Name", Name);
            xmlNode.AppendElem("Data",
                Data != null && Data.Length > 0 
                ? Convert.ToBase64String(Data, Base64FormattingOptions.None) 
                : "");
        }
    }
}
