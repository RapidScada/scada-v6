// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Dynamic;
using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// The class provides extension methods for the ExpandoObject class.
    /// <para>Класс, предоставляющий методы расширения для класса ExpandoObject.</para>
    /// </summary>
    public static class ExpandoExtensions
    {
        /// <summary>
        /// Loads the object property from the XML node.
        /// </summary>
        public static void LoadProperty(this ExpandoObject obj, XmlNode propertyNode)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            ArgumentNullException.ThrowIfNull(propertyNode, nameof(propertyNode));

            if (propertyNode.NodeType == XmlNodeType.Element)
            {
                IDictionary<string, object> dict = obj;
                List<XmlElement> childElements = propertyNode.ChildNodes.OfType<XmlElement>().ToList();

                if (childElements.Count > 0)
                {
                    ExpandoObject childObj = new();
                    dict.Add(propertyNode.Name, childObj);

                    foreach (XmlElement childElement in childElements)
                    {
                        LoadProperty(childObj, childElement);
                    }
                }
                else
                {
                    dict.Add(propertyNode.Name, propertyNode.InnerText);
                }
            }
        }

        /// <summary>
        /// Saves the object property into the XML node.
        /// </summary>
        public static void SaveProperty(XmlNode objectNode, string propertyName, object propertyValue)
        {
            ArgumentNullException.ThrowIfNull(objectNode, nameof(objectNode));
            ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
            
            XmlNode propertyNode = objectNode.OwnerDocument.CreateElement(propertyName);
            objectNode.AppendChild(propertyNode);

            if (propertyValue != null)
            {
                if (propertyValue is ExpandoObject obj)
                {
                    foreach (KeyValuePair<string, object> kvp in obj)
                    {
                        SaveProperty(propertyNode, kvp.Key, kvp.Value);
                    }
                }
                else
                {
                    propertyNode.InnerText = propertyValue.ToString();
                }
            }
        }
    }
}
