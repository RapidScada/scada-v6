// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using System.IO;
using System.Xml;

namespace Scada.Comm.Drivers.DrvEmail
{
    /// <summary>
    /// Represents a device configuration.
    /// <para>Представляет конфигурацию КП.</para>
    /// </summary>
    internal class EmailDeviceConfig : CustomDeviceConfig
    {
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
        protected override void SetToDefault()
        {
            // Gmail: smtp.gmail.com, 587
            // Yandex: smtp.yandex.ru, 25
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
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

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
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("DrvEmail");
            xmlDoc.AppendChild(rootElem);

            rootElem.AppendElem("Host", Host);
            rootElem.AppendElem("Port", Port);
            rootElem.AppendElem("Username", Username);
            rootElem.AppendElem("Password", ScadaUtils.Encrypt(Password));
            rootElem.AppendElem("EnableSsl", EnableSsl);
            rootElem.AppendElem("SenderAddress", SenderAddress);
            rootElem.AppendElem("SenderDisplayName", SenderDisplayName);

            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Gets the short name of the device configuration file.
        /// </summary>
        public static string GetFileName(int deviceNum)
        {
            return GetFileName(DriverUtils.DriverCode, deviceNum);
        }
    }
}
