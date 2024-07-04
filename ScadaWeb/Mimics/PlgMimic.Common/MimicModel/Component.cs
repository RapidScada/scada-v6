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
        /// The names of component nodes that are loaded explicitly.
        /// </summary>
        protected static readonly HashSet<string> ComponentKnownNodes = ["ID", "Name"];


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
        /// Gets or sets the ID of the parent component.
        /// </summary>
        public int ParentID { get; set; } = 0;

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
        /// Gets the names of nodes that are loaded explicitly.
        /// </summary>
        public virtual HashSet<string> KnownNodes => ComponentKnownNodes;


        /// <summary>
        /// Loads the component from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlNode xmlNode, HashSet<int> componentIDs)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");
            TypeName = xmlNode.Name;

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!KnownNodes.Contains(childNode.Name))
                    Properties.LoadProperty(childNode);
            }
        }

        /// <summary>
        /// Saves the component into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            xmlNode.AppendElem("ID", ID);
            xmlNode.AppendElem("Name", Name);

            foreach (KeyValuePair<string, object> kvp in Properties)
            {
                ExpandoExtensions.SaveProperty(xmlNode, kvp.Key, kvp.Value);
            }
        }
    }
}
