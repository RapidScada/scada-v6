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
    public abstract class MimicBase : IContainer
    {
        private string rootElemName = "Mimic"; // the XML root element name


        /// <summary>
        /// Gets the dependencies on the faceplates.
        /// </summary>
        public List<FaceplateMeta> Dependencies { get; } = [];

        /// <summary>
        /// Gets the document that contains mimic properties.
        /// </summary>
        public ExpandoObject Document { get; } = new();

        /// <summary>
        /// Gets the top-level components contained in the mimic.
        /// </summary>
        public List<Component> Components { get; } = [];

        /// <summary>
        /// Gets the images.
        /// </summary>
        public List<Image> Images { get; } = [];


        /// <summary>
        /// Loads the mimic from the XML node.
        /// </summary>
        protected void LoadFromXml(XmlElement rootElem)
        {
            if (rootElem.SelectSingleNode("Dependencies") is XmlNode dependenciesNode)
            {
                foreach (XmlElement faceplateElem in dependenciesNode.SelectNodes("Faceplate"))
                {
                    FaceplateMeta faceplateMeta = new() { IsTransitive = false };
                    faceplateMeta.LoadFromXml(faceplateElem);
                    Dependencies.Add(faceplateMeta);
                }
            }

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
                    Component component = new();

                    if (component.LoadFromXml(childNode, componentIDs))
                        Components.Add(component);
                }
            }

            if (rootElem.SelectSingleNode("Images") is XmlNode imagesNode)
            {
                HashSet<string> imageNames = [];

                foreach (XmlNode imageNode in imagesNode.SelectNodes("Image"))
                {
                    Image image = new();
                    image.LoadFromXml(imageNode);

                    if (!string.IsNullOrEmpty(image.Name) && imageNames.Add(image.Name))
                        Images.Add(image);
                }
            }

            Dependencies.Sort();
            Images.Sort();
        }

        /// <summary>
        /// Saves the mimic into the XML node.
        /// </summary>
        protected void SaveToXml(XmlElement rootElem)
        {
            XmlElement dependenciesElem = rootElem.AppendElem("Dependencies");
            XmlElement documentElem = rootElem.AppendElem("Document");
            XmlElement componentsElem = rootElem.AppendElem("Components");
            XmlElement imagesElem = rootElem.AppendElem("Images");

            foreach (FaceplateMeta faceplateMeta in Dependencies.OrderBy(d => d.TypeName))
            {
                faceplateMeta.SaveToXml(dependenciesElem.AppendElem("Faceplate"));
            }

            foreach (KeyValuePair<string, object> kvp in Document)
            {
                ExpandoExtensions.SaveProperty(documentElem, kvp.Key, kvp.Value);
            }

            foreach (Component component in Components)
            {
                if (!string.IsNullOrEmpty(component.TypeName))
                {
                    component.SaveToXml(componentsElem.AppendElem(component.TypeName));
                }
            }

            foreach (Image image in Images.OrderBy(i => i.Name))
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
            rootElemName = xmlDoc.DocumentElement.Name;
            LoadFromXml(xmlDoc.DocumentElement);
        }

        /// <summary>
        /// Saves the mimic diagram.
        /// </summary>
        public virtual void Save(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement(rootElemName);
            xmlDoc.AppendChild(rootElem);
            SaveToXml(rootElem);

            xmlDoc.Save(stream);
        }
    }
}
