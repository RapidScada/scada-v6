// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Dynamic;
using System.Text.Json.Serialization;
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
        /// The component properties that are loaded explicitly.
        /// </summary>
        protected static readonly HashSet<string> ComponentKnownProperties = ["ID", "Name", "TypeName", "ParentID"];


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
        /// Gets the properties that are loaded explicitly.
        /// </summary>
        [JsonIgnore]
        public virtual HashSet<string> KnownProperties => ComponentKnownProperties;


        /// <summary>
        /// Loads the component from the XML node. Returns true if the component has been added to the map.
        /// </summary>
        public virtual bool LoadFromXml(XmlNode xmlNode, Dictionary<int, Component> componentMap)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ArgumentNullException.ThrowIfNull(componentMap, nameof(componentMap));

            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");
            TypeName = xmlNode.Name;

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                if (!KnownProperties.Contains(childNode.Name))
                    Properties.LoadProperty(childNode);
            }

            return ID > 0 && componentMap.TryAdd(ID, this);
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
