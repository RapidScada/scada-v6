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
        /// Gets or sets the component ID that is unique within the mimic.
        /// </summary>
        public int ID { get; set; } = 0;

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
        public ComponentBindings Bindings { get; set; } = null;

        /// <summary>
        /// Gets the component access options.
        /// </summary>
        public ComponentAccess Access { get; set; } = null;

        /// <summary>
        /// Gets or sets the ID of the parent component.
        /// </summary>
        public int ParentID { get; set; } = 0;

        /// <summary>
        /// Gets the parent container.
        /// </summary>
        [JsonIgnore]
        public IContainer Parent { get; set; } = null;

        /// <summary>
        /// Gets the top-level child components.
        /// </summary>
        [JsonIgnore]
        public List<Component> Components { get; } = [];


        /// <summary>
        /// Loads the component from the XML node. Returns true if the component has been added to the map.
        /// </summary>
        public bool LoadFromXml(XmlNode xmlNode, HashSet<int> componentIDs)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ArgumentNullException.ThrowIfNull(componentIDs, nameof(componentIDs));

            ID = xmlNode.GetChildAsInt(KnownProperty.ID);
            TypeName = xmlNode.Name;

            if (ID > 0 && componentIDs.Add(ID))
            {
                // load properties
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (childNode.Name != KnownProperty.Components)
                        Properties.LoadProperty(childNode);
                }

                // load child components
                if (xmlNode.SelectSingleNode(KnownProperty.Components) is XmlNode componentsNode)
                {
                    foreach (XmlNode childNode in componentsNode.ChildNodes)
                    {
                        Component component = new();

                        if (component.LoadFromXml(childNode, componentIDs))
                        {
                            component.ParentID = ID;
                            component.Parent = this;
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
            xmlNode.AppendElem(KnownProperty.ID, ID);

            foreach (KeyValuePair<string, object> kvp in Properties)
            {
                if (!KnownProperty.All.Contains(kvp.Key))
                    ExpandoExtensions.SaveProperty(xmlNode, kvp.Key, kvp.Value);
            }

            if (Components.Count > 0)
            {
                XmlElement componentsElem = xmlNode.AppendElem(KnownProperty.Components);

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
