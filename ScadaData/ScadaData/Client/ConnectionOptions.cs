/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Module   : ScadaData
 * Summary  : Represents client connection options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
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
            Host = "localhost";
            Port = 10000;
            User = "guest";
            Password = "12345";
            Instance = "";
            Timeout = 10000;
            SecretKey = null;
        }


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
        public string User { get; set; }

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
        public byte[] SecretKey { get; set; }


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            if (xmlNode == null)
                throw new ArgumentNullException(nameof(xmlNode));

            Host = xmlNode.GetChildAsString("Host");
            Port = xmlNode.GetChildAsInt("Port");
            User = xmlNode.GetChildAsString("User");
            Password = ScadaUtils.Decrypt(xmlNode.GetChildAsString("Password"));
            Instance = xmlNode.GetChildAsString("Instance");
            Timeout = xmlNode.GetChildAsInt("Timeout");
            SecretKey = ScadaUtils.HexToBytes(xmlNode.GetChildAsString("SecretKey"));

            if (SecretKey.Length != ScadaUtils.SecretKeySize)
                throw new ScadaException(string.Format(CommonPhrases.InvalidParamVal, "SecretKey"));
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            xmlElem.AppendElem("Host", Host);
            xmlElem.AppendElem("Port", Port);
            xmlElem.AppendElem("User", User);
            xmlElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            xmlElem.AppendElem("Instance", Instance);
            xmlElem.AppendElem("Timeout", Timeout);
            xmlElem.AppendElem("SecretKey", ScadaUtils.BytesToHex(SecretKey));
        }
    }
}
