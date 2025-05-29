// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.Config
{
    /// <summary>
    /// Represents font options.
    /// <para>Представляет параметры шрифта.</para>
    /// </summary>
    public class FontOptions
    {
        /// <summary>
        /// Gets or sets the font name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string Family { get; set; } = "";

        /// <summary>
        /// Gets or sets the stylesheet URL.
        /// </summary>
        public string Url { get; set; } = "";


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            Name = xmlNode.GetChildAsString("Name");
            Family = xmlNode.GetChildAsString("Family");
            Url = xmlNode.GetChildAsString("Url");
        }
    }
}
