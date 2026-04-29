// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents the OPC server connection options.
    /// <para>Представляет параметры соединения с OPC-сервером.</para>
    /// </summary>
    public class OpcConnectionOptions
    {
        /// <summary>
        /// Gets or sets the server URL.
        /// </summary>
        public string ServerUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the security mode.
        /// </summary>
        public MessageSecurityMode SecurityMode { get; set; } = MessageSecurityMode.None;

        /// <summary>
        /// Gets or sets the security policy.
        /// </summary>
        public SecurityPolicy SecurityPolicy { get; set; } = SecurityPolicy.None;

        /// <summary>
        /// Gets or sets the authentication mode.
        /// </summary>
        public AuthenticationMode AuthenticationMode { get; set; } = AuthenticationMode.Anonymous;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = "";

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// Gets or sets the length of time that no new data is available before a client reconnects to the server.
        /// </summary>
        public int ReconnectIfIdle { get; set; } = 60;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ServerUrl = xmlNode.GetChildAsString("ServerUrl");
            SecurityMode = xmlNode.GetChildAsEnum("SecurityMode", MessageSecurityMode.None);
            SecurityPolicy = xmlNode.GetChildAsEnum<SecurityPolicy>("SecurityPolicy");
            AuthenticationMode = xmlNode.GetChildAsEnum<AuthenticationMode>("AuthenticationMode");
            Username = xmlNode.GetChildAsString("Username");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            ReconnectIfIdle = xmlNode.GetChildAsInt("ReconnectIfIdle", ReconnectIfIdle);
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            xmlNode.AppendElem("ServerUrl", ServerUrl);
            xmlNode.AppendElem("SecurityMode", SecurityMode);
            xmlNode.AppendElem("SecurityPolicy", SecurityPolicy);
            xmlNode.AppendElem("AuthenticationMode", AuthenticationMode);
            xmlNode.AppendElem("Username", Username);
            xmlNode.AppendElem("Password", ScadaUtils.Encrypt(Password));
            xmlNode.AppendElem("ReconnectIfIdle", ReconnectIfIdle);
        }

        /// <summary>
        /// Gets the security policy as a string.
        /// </summary>
        public string GetSecurityPolicy()
        {
            return SecurityPolicy switch
            {
                SecurityPolicy.Basic128Rsa15 => SecurityPolicies.Basic128Rsa15,
                SecurityPolicy.Basic256 => SecurityPolicies.Basic256,
                SecurityPolicy.Basic256Sha256 => SecurityPolicies.Basic256Sha256,
                SecurityPolicy.Aes128_Sha256_RsaOaep => SecurityPolicies.Aes128_Sha256_RsaOaep,
                SecurityPolicy.Aes256_Sha256_RsaPss => SecurityPolicies.Aes256_Sha256_RsaPss,
                SecurityPolicy.Https => SecurityPolicies.Https,
                _ => SecurityPolicies.None,
            };
        }
    }
}
