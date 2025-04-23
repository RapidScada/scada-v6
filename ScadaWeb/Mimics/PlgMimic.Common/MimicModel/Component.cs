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
    public sealed class Component : IContainer
    {
        /// <summary>
        /// The component properties that are loaded explicitly.
        /// </summary>
        public static readonly HashSet<string> KnownProperties = 
            ["ID", "Name", "TypeName", "ParentID", "Components"];


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
        /// Gets the top-level child components.
        /// </summary>
        [JsonIgnore]
        public List<Component> Components { get; } = [];

        /// <summary>
        /// Gets the component bindings.
        /// </summary>
        public ComponentBindings Bindings { get; } = new();

        /// <summary>
        /// Gets the component access options.
        /// </summary>
        public ComponentAccess Access { get; } = new();


        /// <summary>
        /// Loads the component from the XML node. Returns true if the component has been added to the map.
        /// </summary>
        public bool LoadFromXml(XmlNode xmlNode, HashSet<int> componentIDs)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ArgumentNullException.ThrowIfNull(componentIDs, nameof(componentIDs));

            ID = xmlNode.GetChildAsInt("ID");
            Name = xmlNode.GetChildAsString("Name");
            TypeName = xmlNode.Name;

            if (ID > 0 && componentIDs.Add(ID))
            {
                // load properties
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (!KnownProperties.Contains(childNode.Name))
                        Properties.LoadProperty(childNode);
                }

                // load child components
                if (xmlNode.SelectSingleNode("Components") is XmlNode componentsNode)
                {
                    foreach (XmlNode childNode in componentsNode.ChildNodes)
                    {
                        Component component = new();

                        if (component.LoadFromXml(childNode, componentIDs))
                        {
                            component.ParentID = ID;
                            Components.Add(component);
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Saves the component into the XML node.
        /// </summary>
        public void SaveToXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            xmlNode.AppendElem("ID", ID);
            xmlNode.AppendElem("Name", Name);

            foreach (KeyValuePair<string, object> kvp in Properties)
            {
                ExpandoExtensions.SaveProperty(xmlNode, kvp.Key, kvp.Value);
            }

            if (Components.Count > 0)
            {
                XmlElement componentsElem = xmlNode.AppendElem("Components");

                foreach (Component component in Components)
                {
                    component.SaveToXml(componentsElem.AppendElem(component.TypeName));
                }
            }
        }

        /// <summary>
        /// Gets all child components recursively.
        /// </summary>
        public IEnumerable<Component> GetAllChildren()
        {
            foreach (Component component in Components)
            {
                yield return component;

                foreach (Component childComponent in component.GetAllChildren())
                {
                    yield return childComponent;
                }
            }
        }
    }
}
