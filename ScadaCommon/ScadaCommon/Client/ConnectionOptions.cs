/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Represents client connection options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.Linq;
using System.Xml;

namespace Scada.Client
{
    /// <summary>
    /// Represents client connection options.
    /// <para>Представляет параметры соединения клиента.</para>
    /// </summary>
    [Serializable]
    public class ConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectionOptions()
        {
            AccessKey = "";
            Name = "";
            Host = "localhost";
            Port = 10000;
            Username = "";
            Password = "";
            Instance = "";
            Timeout = 10000;
            SecretKey = null;
        }


        /// <summary>
        /// Gets or sets the access key used by a client pool.
        /// </summary>
        protected internal string AccessKey { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the server host or IP address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the server TCP port number.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the system instance name.
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the send and receive timeout, ms.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the secret key for password encryption.
        /// </summary>
        /// <remarks>If null, password is not encrypted.</remarks>
        public byte[] SecretKey { get; set; }

        /// <summary>
        /// Gets a value indicating whether the host is local.
        /// </summary>
        public bool IsLocal
        {
            get
            {
                return 
                    string.Equals(Host, "localhost", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Host, "127.0.0.1", StringComparison.Ordinal);
            }
        }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            Name = xmlNode.GetChildAsString("Name");
            Host = xmlNode.GetChildAsString("Host");
            Port = xmlNode.GetChildAsInt("Port", Port);
            Username = xmlNode.GetChildAsString("Username");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            Instance = xmlNode.GetChildAsString("Instance");
            Timeout = xmlNode.GetChildAsInt("Timeout", Timeout);
            string secretKeyStr = xmlNode.GetChildAsString("SecretKey");

            if (string.IsNullOrEmpty(secretKeyStr))
            {
                SecretKey = null;
            }
            else
            {
                SecretKey = ScadaUtils.HexToBytes(secretKeyStr, false, true);
                if (SecretKey.Length != ScadaUtils.SecretKeySize)
                    throw new ScadaException(CommonPhrases.InvalidParamVal, "SecretKey");
            }
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            if (!string.IsNullOrEmpty(Name))
                xmlElem.AppendElem("Name", Name);

            xmlElem.AppendElem("Host", Host);
            xmlElem.AppendElem("Port", Port);
            xmlElem.AppendElem("Username", Username);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            
            if (!string.IsNullOrEmpty(Instance))
                xmlElem.AppendElem("Instance", Instance);

            xmlElem.AppendElem("Timeout", Timeout);
            xmlElem.AppendElem("SecretKey", ScadaUtils.BytesToHex(SecretKey));
        }

        /// <summary>
        /// Copies the current object to the other.
        /// </summary>
        public void CopyTo(ConnectionOptions destination)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            destination.Name = Name;
            destination.Host = Host;
            destination.Port = Port;
            destination.Username = Username;
            destination.Password = Password;
            destination.Instance = Instance;
            destination.Timeout = Timeout;
            destination.SecretKey = SecretKey?.ToArray();
        }

        /// <summary>
        /// Determines whether two specified connection options have the same value.
        /// </summary>
        public static bool Equals(ConnectionOptions a, ConnectionOptions b)
        {
            if (a == b)
            {
                return true;
            }
            else if (a == null || b == null)
            {
                return false;
            }
            else
            {
                return
                    a.Name == b.Name &&
                    a.Host == b.Host &&
                    a.Port == b.Port &&
                    a.Username == b.Username &&
                    a.Password == b.Password &&
                    a.Instance == b.Instance &&
                    a.Timeout == b.Timeout &&
                    ScadaUtils.SequenceEqual(a.SecretKey, b.SecretKey);
            }
        }
    }
}
