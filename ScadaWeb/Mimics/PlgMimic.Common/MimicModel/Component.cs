// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Dynamic;
using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a component of a mimic diagram.
    /// <para>Представляет компонент мнемосхемы.</para>
    /// </summary>
    public class Component
    {
        /// <summary>
        /// The set of node names that are loaded explicitly.
        /// </summary>
        private static readonly HashSet<string> KnownNodes = ["id", "name"];


        /// <summary>
        /// Gets or sets the component ID that is unique within the mimic.
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string TypeName { get; set; } = "";

        /// <summary>
        /// Gets the component properties.
        /// </summary>
        public ExpandoObject Properties { get; } = new();

        /// <summary>
        /// Gets the component bindings.
        /// </summary>
        public ComponentBindings Bindings { get; } = new();

        /// <summary>
        /// Gets the component access options.
        /// </summary>
        public ComponentAccess Access { get; } = new();
        

        /// <summary>
        /// Loads the component from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            ID = xmlElem.GetChildAsInt("ID");
            Name = xmlElem.GetChildAsString("Name");
            TypeName = xmlElem.Name;

            foreach (XmlNode childNode in xmlElem.ChildNodes)
            {
                if (!KnownNodes.Contains(childNode.Name.ToLowerInvariant()))
                    Properties.LoadProperty(childNode);
            }
        }

        /// <summary>
        /// Saves the component into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.AppendElem("ID", ID);
            xmlElem.AppendElem("Name", Name);

            foreach (KeyValuePair<string, object> kvp in Properties)
            {
                ExpandoExtensions.SaveProperty(xmlElem, kvp.Key, kvp.Value);
            }
        }
    }
}
