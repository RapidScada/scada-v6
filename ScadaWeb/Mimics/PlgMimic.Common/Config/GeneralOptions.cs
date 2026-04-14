// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.Config
{
    /// <summary>
    /// Represents general options of mimic diagrams used in runtime and editing modes.
    /// <para>Представляет основные параметры мнемосхем, используемые режимах выполнения и редактирования.</para>
    /// </summary>
    public class GeneralOptions
    {
        /// <summary>
        /// Gets or sets the URL of a custom stylesheet.
        /// </summary>
        public string CustomCss { get; set; } = "";

        /// <summary>
        /// Gets or sets the URL of a custom script.
        /// </summary>
        public string CustomJs { get; set; } = "";


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            CustomCss = xmlNode.GetChildAsString("CustomCss");
            CustomJs = xmlNode.GetChildAsString("CustomJs");
        }
    }
}
