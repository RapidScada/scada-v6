// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModArcInfluxDb.Config
{
    /// <summary>
    /// Represents options for connecting to InfluxDB server.
    /// <para>Представляет параметры соединения с сервером InfluxDB.</para>
    /// </summary>
    internal class ConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectionOptions()
        {
            Name = "";
            Url = "";
            Token = "";
            Username = "";
            Password = "";
            Bucket = "";
            Org = "";
        }


        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the database URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the database token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the database username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the database password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the bucket.
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public string Org { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            Name = xmlNode.GetChildAsString("Name");
            Url = xmlNode.GetChildAsString("Url");
            Token = xmlNode.GetChildAsString("Token");
            Username = xmlNode.GetChildAsString("Username");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            Bucket = xmlNode.GetChildAsString("Bucket");
            Org = xmlNode.GetChildAsString("Org");
        }
    }
}
