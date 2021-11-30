// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Opc.Ua;
using System;
using System.Xml;

namespace Scada.Comm.Drivers.DrvOpcUa.Config
{
    /// <summary>
    /// Represents the OPC server connection options.
    /// <para>Представляет параметры соединения с OPC-сервером.</para>
    /// </summary>
    public class ConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectionOptions()
        {
            ServerUrl = "";
            SecurityMode = MessageSecurityMode.None;
            SecurityPolicy = SecurityPolicy.None;
            AuthenticationMode = AuthenticationMode.Anonymous;
            Username = "";
            Password = "";
        }


        /// <summary>
        /// Gets or sets the server URL.
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the security mode.
        /// </summary>
        public MessageSecurityMode SecurityMode { get; set; }

        /// <summary>
        /// Gets or sets the security policy.
        /// </summary>
        public SecurityPolicy SecurityPolicy { get; set; }

        /// <summary>
        /// Gets or sets the authentication mode.
        /// </summary>
        public AuthenticationMode AuthenticationMode { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            ServerUrl = xmlNode.GetChildAsString("ServerUrl");
            SecurityMode = xmlNode.GetChildAsEnum("SecurityMode", MessageSecurityMode.None);
            SecurityPolicy = xmlNode.GetChildAsEnum<SecurityPolicy>("SecurityPolicy");
            AuthenticationMode = xmlNode.GetChildAsEnum<AuthenticationMode>("AuthenticationMode");
            Username = xmlNode.GetChildAsString("Username");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("ServerUrl", ServerUrl);
            xmlElem.AppendElem("SecurityMode", SecurityMode);
            xmlElem.AppendElem("SecurityPolicy", SecurityPolicy);
            xmlElem.AppendElem("AuthenticationMode", AuthenticationMode);
            xmlElem.AppendElem("Username", Username);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
        }

        /// <summary>
        /// Gets the security policy as a string.
        /// </summary>
        public string GetSecurityPolicy()
        {
            switch (SecurityPolicy)
            {
                case SecurityPolicy.Basic128Rsa15:
                    return SecurityPolicies.Basic128Rsa15;

                case SecurityPolicy.Basic256:
                    return SecurityPolicies.Basic256;

                case SecurityPolicy.Basic256Sha256:
                    return SecurityPolicies.Basic256Sha256;

                case SecurityPolicy.Aes128_Sha256_RsaOaep:
                    return SecurityPolicies.Aes128_Sha256_RsaOaep;

                case SecurityPolicy.Aes256_Sha256_RsaPss:
                    return SecurityPolicies.Aes256_Sha256_RsaPss;

                case SecurityPolicy.Https:
                    return SecurityPolicies.Https;

                default:
                    return SecurityPolicies.None;
            }
        }
    }
}
