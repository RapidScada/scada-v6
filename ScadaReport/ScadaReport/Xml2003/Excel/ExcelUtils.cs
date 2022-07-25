// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;
using System.Xml.Linq;

namespace Scada.Report.Xml2003.Excel
{
    /// <summary>
    /// The class provides helper methods for manipulating an Excel workbook.
    /// <para>Класс, предоставляющий вспомогательные методы для для работы с книгой Excel.</para>
    /// </summary>
    internal static class ExcelUtils
    {
        /// <summary>
        /// The line break symbol used by Excel.
        /// </summary>
        public const string ExcelLineBreak = "&#10;";


        /// <summary>
        /// Gets the value of the XML node attribute.
        /// </summary>
        public static string GetAttribute(XmlNode xmlNode, string name)
        {
            if (xmlNode == null)
            {
                return "";
            }
            else
            {
                XmlAttribute xmlAttr = xmlNode.Attributes[name];
                return xmlAttr == null ? "" : xmlAttr.Value;
            }
        }

        /// <summary>
        /// Sets the XML node attribute, creating it if necessary.
        /// </summary>
        public static void SetAttribute(XmlNode xmlNode, string localName, string namespaceURI, string value,
            bool removeEmpty = false)
        {
            if (xmlNode != null)
            {
                if (string.IsNullOrEmpty(value) && removeEmpty)
                {
                    xmlNode.Attributes.RemoveNamedItem(localName, namespaceURI);
                }
                else if (xmlNode.Attributes.GetNamedItem(localName, namespaceURI) is not XmlAttribute xmlAttr)
                {
                    xmlAttr = xmlNode.OwnerDocument.CreateAttribute("", localName, namespaceURI);
                    xmlAttr.Value = value;
                    xmlNode.Attributes.SetNamedItem(xmlAttr);
                }
                else
                {
                    xmlAttr.Value = value;
                }
            }
        }

        /// <summary>
        /// Sets the XML node text to a string, which may contain line breaks.
        /// </summary>
        public static void SetNodeTextWithBreak(XmlNode xmlNode, object text, string lineBreak = "\n")
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            string textStr = text?.ToString();

            if (string.IsNullOrEmpty(textStr))
            {
                xmlNode.InnerText = "";
            }
            else
            {
                string[] parts = textStr.Split(lineBreak, StringSplitOptions.None);

                if (parts.Length <= 1)
                    xmlNode.InnerText = textStr;
                else
                    xmlNode.InnerXml = string.Join(ExcelLineBreak, parts.Select(s => new XText(s).ToString()));
            }
        }
    }
}
