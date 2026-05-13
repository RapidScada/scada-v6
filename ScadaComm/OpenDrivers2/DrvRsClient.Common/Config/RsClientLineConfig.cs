// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Config;
using System.Xml;

namespace Scada.Comm.Drivers.DrvRsClient.Config
{
    /// <summary>
    /// Represents a communication line configuration for SCADA clients.
    /// <para>Представляет конфигурацию линии связи для SCADA-клиентов.</para>
    /// </summary>
    public class RsClientLineConfig : ConfigBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use the default connection
        /// specified in the Communicator options.
        /// </summary>
        public bool UseDefaultConnection { get; set; }

        /// <summary>
        /// Gets the connection options if the default connection is not used.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            UseDefaultConnection = true;
            ConnectionOptions = new ConnectionOptions();
        }

        /// <summary>
        /// Loads the configuration from the XML document.
        /// </summary>
        protected override void LoadFromXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.DocumentElement;
            UseDefaultConnection = rootElem.GetChildAsBool("UseDefaultConnection");

            if (rootElem.SelectSingleNode("ConnectionOptions") is XmlNode connectionOptionsNode)
                ConnectionOptions.LoadFromXml(connectionOptionsNode);
        }

        /// <summary>
        /// Saves the configuration into the XML document.
        /// </summary>
        protected override void SaveToXml(XmlDocument xmlDoc)
        {
            XmlElement rootElem = xmlDoc.CreateElement("RsClientLineConfig");
            xmlDoc.AppendChild(rootElem);
            rootElem.AppendElem("UseDefaultConnection", UseDefaultConnection);
            ConnectionOptions.SaveToXml(rootElem.AppendElem("ConnectionOptions"));
        }

        /// <summary>
        /// Gets the short name of the line configuration file.
        /// </summary>
        public static string GetFileName(int lineNum)
        {
            return $"{DriverUtils.DriverCode}_line{lineNum:D3}.xml";
        }
    }
}
