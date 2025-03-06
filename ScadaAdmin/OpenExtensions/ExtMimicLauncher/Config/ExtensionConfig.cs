// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Xml;

namespace Scada.Admin.Extensions.ExtMimicLauncher.Config
{
    internal class ExtensionConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ExtMimicLauncher.xml";


        /// <summary>
        /// Gets or sets the web application address.
        /// </summary>
        public string WebUrl { get; set; }

        /// <summary>
        /// Gets or sets the browser for editing mimics.
        /// </summary>
        public Browser Browser { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            WebUrl = "http://localhost:10008";
            Browser = Browser.Default;
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected override void LoadFromXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.DocumentElement;
            WebUrl = rootElem.GetChildAsString("WebUrl", WebUrl);
            Browser = rootElem.GetChildAsEnum("Browser", Browser);
        }

        /// <summary>
        /// Saves the configuration into the XML document.
        /// </summary>
        protected override void SaveToXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.CreateElement("ExtMimicLauncher");
            xmlDoc.AppendChild(rootElem);

            rootElem.AppendElem("WebUrl", WebUrl);
            rootElem.AppendElem("Browser", Browser);
        }
    }
}
