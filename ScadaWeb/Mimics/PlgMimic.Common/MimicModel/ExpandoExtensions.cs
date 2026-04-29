// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
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
        /// Gets the property value.
        /// </summary>
        public static object GetValue(this ExpandoObject obj, string propertyName)
        {
            IDictionary<string, object> dict = obj;
            return dict[propertyName];
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public static T GetValue<T>(this ExpandoObject obj, string propertyName)
        {
            return (T)obj.GetValue(propertyName);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        public static void SetValue(this ExpandoObject obj, string propertyName, object propertyValue)
        {
            IDictionary<string, object> dict = obj;
            dict[propertyName] = propertyValue;
        }

        /// <summary>
        /// Removes all properties.
        /// </summary>
        public static void RemoveAll(this ExpandoObject obj)
        {
            IDictionary<string, object> dict = obj;
            dict.Remove(dict.Keys.ToList());
        }

        /// <summary>
        /// Loads the object property from the XML node.
        /// </summary>
        public static void LoadProperty(this ExpandoObject obj, XmlElement propertyElem)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            ArgumentNullException.ThrowIfNull(propertyElem, nameof(propertyElem));

            if (propertyElem.NodeType == XmlNodeType.Element)
            {
                IDictionary<string, object> dict = obj;
                List<XmlElement> childElements = propertyElem.ChildNodes.OfType<XmlElement>().ToList();

                if (propertyElem.GetAttrAsBool("isArray"))
                {
                    List<object> list = [];
                    dict.Add(propertyElem.Name, list);

                    foreach (XmlElement childElement in childElements)
                    {
                        ExpandoObject childObj = new();
                        childObj.LoadProperty(childElement);
                        list.Add(childObj.GetValue(childElement.Name));
                    }
                }
                else
                {
                    if (childElements.Count > 0)
                    {
                        ExpandoObject childObj = new();
                        dict.Add(propertyElem.Name, childObj);

                        foreach (XmlElement childElement in childElements)
                        {
                            childObj.LoadProperty(childElement);
                        }
                    }
                    else
                    {
                        dict.Add(propertyElem.Name, propertyElem.InnerText);
                    }
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
            
            XmlElement propertyElem = objectNode.OwnerDocument.CreateElement(propertyName);
            objectNode.AppendChild(propertyElem);

            if (propertyValue != null)
            {
                if (propertyValue is ExpandoObject obj)
                {
                    foreach (KeyValuePair<string, object> kvp in obj)
                    {
                        SaveProperty(propertyElem, kvp.Key, kvp.Value);
                    }
                }
                else if (propertyValue is ICollection collection)
                {
                    propertyElem.SetAttribute("isArray", true);

                    foreach (object item in collection)
                    {
                        SaveProperty(propertyElem, "Item", item);
                    }
                }
                else
                {
                    // note that string implements IEnumeration
                    string s = propertyValue.ToString();

                    if (!string.IsNullOrEmpty(s))
                        propertyElem.InnerText = s;
                }
            }
        }
    }
}
