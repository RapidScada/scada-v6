// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Dynamic;
using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// A base class for mimic diagrams and faceplates.
    /// <para>Базовый класс для мнемосхем и фейсплейтов.</para>
    /// </summary>
    public abstract class MimicBase
    {
        /// <summary>
        /// Gets the mimic document that groups its properties.
        /// </summary>
        public ExpandoObject Document { get; } = new();

        /// <summary>
        /// Gets the components contained within the mimic.
        /// </summary>
        public List<Component> Components { get; } = [];

        /// <summary>
        /// Gets the images accessed by name.
        /// </summary>
        public Dictionary<string, Image> Images { get; } = [];


        /// <summary>
        /// Loads the mimic from the XML node.
        /// </summary>
        protected virtual void LoadFromXml(XmlElement rootElem)
        {
            if (rootElem.SelectSingleNode("Document") is XmlNode documentNode)
            {
                foreach (XmlNode childNode in documentNode.ChildNodes)
                {
                    Document.LoadProperty(childNode);
                }
            }

            if (rootElem.SelectSingleNode("Components") is XmlNode componentsNode)
            {
                HashSet<int> componentIDs = [];

                foreach (XmlNode childNode in componentsNode.ChildNodes)
                {
                    Component component = childNode.Name == Panel.NodeName ? new Panel() : new Component();
                    component.LoadFromXml(childNode, componentIDs);

                    if (component.ID > 0 && !componentIDs.Contains(component.ID))
                        Components.Add(component);
                }
            }

            if (rootElem.SelectSingleNode("Images") is XmlNode imagesNode)
            {
                foreach (XmlNode imageNode in imagesNode.SelectNodes("Image"))
                {
                    Image image = new();
                    image.LoadFromXml(imageNode);

                    if (!string.IsNullOrEmpty(image.Name))
                        Images.TryAdd(image.Name, image);
                }
            }
        }

        /// <summary>
        /// Saves the mimic into the XML node.
        /// </summary>
        protected virtual void SaveToXml(XmlElement rootElem)
        {
            XmlElement documentElem = rootElem.AppendElem("Document");
            XmlElement componentsElem = rootElem.AppendElem("Components");
            XmlElement imagesElem = rootElem.AppendElem("Images");

            foreach (KeyValuePair<string, object> kvp in Document)
            {
                ExpandoExtensions.SaveProperty(documentElem, kvp.Key, kvp.Value);
            }

            foreach (Component component in Components)
            {
                component.SaveToXml(componentsElem.AppendElem(component.TypeName));
            }

            foreach (Image image in EnumerateImages())
            {
                image.SaveToXml(imagesElem.AppendElem("Image"));
            }
        }


        /// <summary>
        /// Loads the mimic diagram.
        /// </summary>
        public virtual void Load(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            XmlDocument xmlDoc = new();
            xmlDoc.Load(stream);
            LoadFromXml(xmlDoc.DocumentElement);
        }

        /// <summary>
        /// Saves the mimic diagram.
        /// </summary>
        public virtual void Save(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            using StreamWriter writer = new(stream);
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("Mimic");
            xmlDoc.AppendChild(rootElem);
            SaveToXml(rootElem);
        }

        /// <summary>
        /// Enumerates the images ordered by name.
        /// </summary>
        public IEnumerable<Image> EnumerateImages()
        {
            return Images.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
        }
    }
}
