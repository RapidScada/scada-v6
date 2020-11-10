using System;
using System.Globalization;
using System.Xml;

namespace Scada
{
	partial class ScadaUtils
	{
        /// <summary>
        /// Converts the value to be written to an XML file into a string.
        /// </summary>
        public static string XmlValToStr(object value)
        {
            if (value == null)
                return "";
            else if (value is bool)
                return value.ToString().ToLowerInvariant();
            else if (value is double)
                return ((double)value).ToString(NumberFormatInfo.InvariantInfo);
            else if (value is DateTime)
                return ((DateTime)value).ToString(DateTimeFormatInfo.InvariantInfo);
            else if (value is TimeSpan)
                return ((TimeSpan)value).ToString("", DateTimeFormatInfo.InvariantInfo);
            else
                return value.ToString();
        }

        /// <summary>
        /// Converts the specified string read from an XML document to a double-precision floating point number.
        /// </summary>
        public static double XmlParseDouble(string s)
        {
            return double.Parse(s, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts the specified string read from an XML document to a DateTime structure.
        /// </summary>
        /// <remarks>The Kind property of the returned structure is Unspecified.</remarks>
        public static DateTime XmlParseDateTime(string s)
        {
            return DateTime.Parse(s, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts the specified string read from an XML document to a date.
        /// </summary>
        public static DateTime XmlParseDate(string s)
        {
            return XmlParseDateTime(s).Date;
        }

        /// <summary>
        /// Converts the specified string read from an XML document to a time span.
        /// </summary>
        public static TimeSpan XmlParseTimeSpan(string s)
        {
            return TimeSpan.Parse(s, DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts the specified string read from an XML document to an enumeration element.
        /// </summary>
        public static T XmlParseEnum<T>(string s) where T : struct
        {
            return (T)Enum.Parse(typeof(T), s, true);
        }


        /// <summary>
        /// Creates a new XML element and appends it to the parent.
        /// </summary>
        public static XmlElement AppendElem(this XmlElement parentXmlElem, string elemName, object innerText = null)
        {
            XmlElement xmlElem = parentXmlElem.OwnerDocument.CreateElement(elemName);
            string val = XmlValToStr(innerText);
            if (!string.IsNullOrEmpty(val))
                xmlElem.InnerText = val;
            return (XmlElement)parentXmlElem.AppendChild(xmlElem);
        }

        /// <summary>
        /// Creates and appends to the parent a new XML element of the option 
        /// with the specified name, value and description.
        /// </summary>
        public static XmlElement AppendOptionElem(this XmlElement parentXmlElem, string optionName, object value,
            string descr = "")
        {
            XmlElement paramElem = parentXmlElem.OwnerDocument.CreateElement("Option");
            paramElem.SetAttribute("name", optionName);
            paramElem.SetAttribute("value", XmlValToStr(value));
            if (!string.IsNullOrEmpty(descr))
                paramElem.SetAttribute("descr", descr);
            return (XmlElement)parentXmlElem.AppendChild(paramElem);
        }

        /// <summary>
        /// Finds an XML element of the option having the specified name.
        /// </summary>
        public static XmlElement GetOptionElem(this XmlElement parentXmlElem, string optionName)
        {
            XmlNodeList xmlNodes = parentXmlElem.SelectNodes(string.Format("Option[@name='{0}'][1]", optionName));
            return xmlNodes.Count > 0 ? xmlNodes[0] as XmlElement : null;
        }

        /// <summary>
        /// Gets the child XML node value as a string.
        /// </summary>
        /// <remarks>If the XML node doesn't exist, an InvalidOperationException exception is thrown.</remarks>
        public static string GetChildAsString(this XmlNode parentXmlNode, string childNodeName, string defaultVal = "")
        {
            XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
            return node == null ? defaultVal : node.InnerText;
        }

        /// <summary>
        /// Gets the child XML node value as a boolean.
        /// </summary>
        public static bool GetChildAsBool(this XmlNode parentXmlNode, string childNodeName, bool defaultVal = false)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : bool.Parse(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Gets the child XML node value as a 32-bit signed integer.
        /// </summary>
        public static int GetChildAsInt(this XmlNode parentXmlNode, string childNodeName, int defaultVal = 0)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : int.Parse(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Gets the child XML node value as a 64-bit signed integer.
        /// </summary>
        public static long GetChildAsLong(this XmlNode parentXmlNode, string childNodeName, long defaultVal = 0)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : long.Parse(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Gets the child XML node value as a double-precision floating point number.
        /// </summary>
        public static double GetChildAsDouble(this XmlNode parentXmlNode, string childNodeName, double defaultVal = 0)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : XmlParseDouble(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Gets the child XML node value as a date and time.
        /// </summary>
        public static DateTime GetChildAsDateTime(this XmlNode parentXmlNode, string childNodeName, DateTime defaultVal)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : XmlParseDateTime(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Gets the child XML node value as a date and time.
        /// </summary>
        public static DateTime GetChildAsDateTime(this XmlNode parentXmlNode, string childNodeName, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(parentXmlNode.GetChildAsDateTime(childNodeName, DateTime.MinValue), kind);
        }

        /// <summary>
        /// Gets the child XML node value as a date and time.
        /// </summary>
        public static DateTime GetChildAsDateTime(this XmlNode parentXmlNode, string childNodeName)
        {
            return parentXmlNode.GetChildAsDateTime(childNodeName, DateTime.MinValue);
        }

        /// <summary>
        /// Gets the child XML node value as a time span.
        /// </summary>
        public static TimeSpan GetChildAsTimeSpan(this XmlNode parentXmlNode, string childNodeName, TimeSpan defaultVal)
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : XmlParseTimeSpan(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }

        /// <summary>
        /// Gets the child XML node value as a time span.
        /// </summary>
        public static TimeSpan GetChildAsTimeSpan(this XmlNode parentXmlNode, string childNodeName)
        {
            return parentXmlNode.GetChildAsTimeSpan(childNodeName, TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the child XML node value as an enumeration element.
        /// </summary>
        public static T GetChildAsEnum<T>(this XmlNode parentXmlNode, string childNodeName, 
            T defaultVal = default(T)) where T : struct
        {
            try
            {
                XmlNode node = parentXmlNode.SelectSingleNode(childNodeName);
                return node == null ? defaultVal : XmlParseEnum<T>(node.InnerText);
            }
            catch (FormatException)
            {
                throw NewFormatException(childNodeName);
            }
        }


        /// <summary>
        /// Sets the value of the XML attribute.
        /// </summary>
        public static void SetAttribute(this XmlElement xmlElem, string attrName, object value)
        {
            xmlElem.SetAttribute(attrName, XmlValToStr(value));
        }

        /// <summary>
        /// Gets the XML attribute value as a string.
        /// </summary>
        public static string GetAttrAsString(this XmlElement xmlElem, string attrName, string defaultVal = "")
        {
            return xmlElem.HasAttribute(attrName) ?
                xmlElem.GetAttribute(attrName) : defaultVal;
        }

        /// <summary>
        /// Gets the XML attribute value as a boolean.
        /// </summary>
        public static bool GetAttrAsBool(this XmlElement xmlElem, string attrName, bool defaultVal = false)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    bool.Parse(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as a 32-bit signed integer.
        /// </summary>
        public static int GetAttrAsInt(this XmlElement xmlElem, string attrName, int defaultVal = 0)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    int.Parse(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as a 64-bit signed integer.
        /// </summary>
        public static long GetAttrAsLong(this XmlElement xmlElem, string attrName, long defaultVal = 0)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ?
                    long.Parse(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as a double-precision floating point number.
        /// </summary>
        public static double GetAttrAsDouble(this XmlElement xmlElem, string attrName, double defaultVal = 0)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    XmlParseDouble(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as a date and time.
        /// </summary>
        public static DateTime GetAttrAsDateTime(this XmlElement xmlElem, string attrName, DateTime defaultVal)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ?
                    XmlParseDateTime(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as a date and time.
        /// </summary>
        public static DateTime GetAttrAsDateTime(this XmlElement xmlElem, string attrName, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(xmlElem.GetAttrAsDateTime(attrName, DateTime.MinValue), kind);
        }

        /// <summary>
        /// Gets the XML attribute value as a date and time.
        /// </summary>
        public static DateTime GetAttrAsDateTime(this XmlElement xmlElem, string attrName)
        {
            return xmlElem.GetAttrAsDateTime(attrName, DateTime.MinValue);
        }

        /// <summary>
        /// Gets the XML attribute value as a time span.
        /// </summary>
        public static TimeSpan GetAttrAsTimeSpan(this XmlElement xmlElem, string attrName, TimeSpan defaultVal)
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ?
                    XmlParseTimeSpan(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }

        /// <summary>
        /// Gets the XML attribute value as a time span.
        /// </summary>
        public static TimeSpan GetAttrAsTimeSpan(this XmlElement xmlElem, string attrName)
        {
            return xmlElem.GetAttrAsTimeSpan(attrName, TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the XML attribute value as an enumeration element.
        /// </summary>
        public static T GetAttrAsEnum<T>(this XmlElement xmlElem, string attrName, 
            T defaultVal = default(T)) where T : struct
        {
            try
            {
                return xmlElem.HasAttribute(attrName) ? 
                    XmlParseEnum<T>(xmlElem.GetAttribute(attrName)) : defaultVal;
            }
            catch (FormatException)
            {
                throw NewFormatException(attrName);
            }
        }
    }
}
