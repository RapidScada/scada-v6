// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Xml;

namespace Scada.Server.Modules.ModDeviceAlarm.Config
{
    /// <summary>
    /// Represents a device configuration.
    /// </summary>
    [Serializable]
    public class EmailDeviceConfig
    {
        public EmailDeviceConfig()
        {
            SetToDefault();
        }

        /// <summary>
        /// Gets or sets the server name or IP address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
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
        /// Gets or sets a value indicating whether SSL is enabled.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// Gets or sets the display name of the sender.
        /// </summary>
        public string SenderDisplayName { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            // Gmail: smtp.gmail.com, 587
            Host = "smtp.gmail.com";
            Port = 587;
            Username = "example@gmail.com";
            Password = "";
            EnableSsl = true;
            SenderAddress = "example@gmail.com";
            SenderDisplayName = "Rapid SCADA";
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        public void LoadFromXml(XmlElement rootElem)
        {
            if (rootElem == null)
            {
                SetToDefault();
                return;
            }
            Host = rootElem.GetChildAsString("Host");
            Port = rootElem.GetChildAsInt("Port");
            Username = rootElem.GetChildAsString("Username", rootElem.GetChildAsString("User"));
            Password = ScadaUtils.Decrypt(rootElem.GetChildAsString("Password"));
            EnableSsl = rootElem.GetChildAsBool("EnableSsl");
            SenderAddress = rootElem.GetChildAsString("SenderAddress", Username);
            SenderDisplayName = rootElem.GetChildAsString("SenderDisplayName");
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        public void SaveToXml(XmlElement rootElem)
        {
            rootElem.AppendElem("Host", Host);
            rootElem.AppendElem("Port", Port);
            rootElem.AppendElem("Username", Username);
            rootElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            rootElem.AppendElem("EnableSsl", EnableSsl);
            rootElem.AppendElem("SenderAddress", SenderAddress);
            rootElem.AppendElem("SenderDisplayName", SenderDisplayName);
        }
    }
}
