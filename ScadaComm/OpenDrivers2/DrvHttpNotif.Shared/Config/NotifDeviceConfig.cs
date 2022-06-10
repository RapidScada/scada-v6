// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Drivers.DrvHttpNotif.Config
{
    /// <summary>
    /// Represents a device configuration.
    /// <para>Представляет конфигурацию КП.</para>
    /// </summary>
    internal class NotifDeviceConfig : DeviceConfigBase
    {
        /// <summary>
        /// The default character that marks the beginning of a parameter.
        /// </summary>
        private const char DefaultParamBegin = '{';
        /// <summary>
        /// The default character that marks the end of a parameter.
        /// </summary>
        private const char DefaultParamEnd = '}';
        /// <summary>
        /// The default address separator.
        /// </summary>
        private const string DefaultAddrSep = ";";



        /// <summary>
        /// Gets or sets the request URI.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the request HTTP method.
        /// </summary>
        public RequestMethod Method { get; set; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public List<Header> Headers { get; private set; }

        /// <summary>
        /// Gets or sets the contents of the HTTP message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the escaping method for content parameters.
        /// </summary>
        public EscapingMethod ContentEscaping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether URI and content can include parameters.
        /// </summary>
        public bool ParamEnabled { get; set; }

        /// <summary>
        /// Gets or sets the character that marks the beginning of a parameter.
        /// </summary>
        public char ParamBegin { get; set; }

        /// <summary>
        /// Gets or sets the character that marks the end of a parameter.
        /// </summary>
        public char ParamEnd { get; set; }

        /// <summary>
        /// Gets or sets the string that separates multiple phone numbers and email addresses.
        /// </summary>
        public string AddrSep { get; set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Uri = "";
            Method = RequestMethod.Get;
            Headers = new List<Header>();
            Content = "";
            ContentType = "";
            ContentEscaping = EscapingMethod.None;
            ParamEnabled = true;
            ParamBegin = DefaultParamBegin;
            ParamEnd = DefaultParamEnd;
            AddrSep = DefaultAddrSep;
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            Uri = rootElem.GetChildAsString("Uri");
            Method = rootElem.GetChildAsEnum("Method", RequestMethod.Get);

            if (rootElem.SelectSingleNode("Headers") is XmlNode headersNode)
            {
                foreach (XmlElement headerElem in headersNode.SelectNodes("Header"))
                {
                    Headers.Add(new Header
                    {
                        Name = headerElem.GetAttrAsString("name"),
                        Value = headerElem.GetAttrAsString("value")
                    });
                }
            }

            Content = rootElem.GetChildAsString("Content");
            ContentType = rootElem.GetChildAsString("ContentType");
            ContentEscaping = rootElem.GetChildAsEnum("ContentEscaping", EscapingMethod.None);
            ParamEnabled = rootElem.GetChildAsBool("ParamEnabled", true);
            SetParamBegin(rootElem.GetChildAsString("ParamBegin"));
            SetParamEnd(rootElem.GetChildAsString("ParamEnd"));
            AddrSep = rootElem.GetChildAsString("AddrSep", AddrSep);
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("DrvHttpNotif");
            xmlDoc.AppendChild(rootElem);

            rootElem.AppendElem("Uri", Uri);
            rootElem.AppendElem("Method", Method);
            XmlElement headersElem = rootElem.AppendElem("Headers");

            foreach (Header header in Headers)
            {
                XmlElement headerElem = headersElem.AppendElem("Header");
                headerElem.SetAttribute("name", header.Name);
                headerElem.SetAttribute("value", header.Value);
            }

            rootElem.AppendElem("Content", Content);
            rootElem.AppendElem("ContentType", ContentType);
            rootElem.AppendElem("ContentEscaping", ContentEscaping);
            rootElem.AppendElem("ParamEnabled", ParamEnabled);
            rootElem.AppendElem("ParamBegin", ParamBegin);
            rootElem.AppendElem("ParamEnd", ParamEnd);
            rootElem.AppendElem("AddrSep", AddrSep);

            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Sets the character that marks the beginning of a parameter.
        /// </summary>
        public void SetParamBegin(string s)
        {
            ParamBegin = string.IsNullOrEmpty(s) ? DefaultParamBegin : s[0];
        }

        /// <summary>
        /// Sets the character that marks the end of a parameter.
        /// </summary>
        public void SetParamEnd(string s)
        {
            ParamEnd = string.IsNullOrEmpty(s) ? DefaultParamEnd : s[0];
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
