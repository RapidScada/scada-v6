// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a panel that can contain child components.
    /// <para>Представляет панель, которая может содержать дочерние компоненты.</para>
    /// </summary>
    public class Panel : Component, IContainer
    {
        /// <summary>
        /// The XML node name for panels.
        /// </summary>
        public const string NodeName = "Panel";

        /// <summary>
        /// The panel properties that are loaded explicitly.
        /// </summary>
        private static readonly HashSet<string> PanelKnownProperties = [.. ComponentKnownProperties, "Components"];


        /// <summary>
        /// Gets the top-level components contained in the panel.
        /// </summary>
        [JsonIgnore]
        public List<Component> Components { get; } = [];

        /// <summary>
        /// Gets the properties that are loaded explicitly.
        /// </summary>
        public override HashSet<string> KnownProperties => PanelKnownProperties;


        /// <summary>
        /// Loads the panel from the XML node.
        /// </summary>
        public override bool LoadFromXml(XmlNode xmlNode, Dictionary<int, Component> componentMap)
        {
            if (!base.LoadFromXml(xmlNode, componentMap))
                return false;

            if (xmlNode.SelectSingleNode("Components") is XmlNode componentsNode)
            {
                foreach (XmlNode childNode in componentsNode.ChildNodes)
                {
                    Component component = childNode.Name == NodeName 
                        ? new Panel() 
                        : new Component();

                    if (component.LoadFromXml(childNode, componentMap))
                    {
                        component.ParentID = ID;
                        Components.Add(component);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Saves the panel into the XML node.
        /// </summary>
        public override void SaveToXml(XmlNode xmlNode)
        {
            base.SaveToXml(xmlNode);
            XmlElement componentsElem = xmlNode.AppendElem("Components");

            foreach (Component component in Components)
            {
                component.SaveToXml(componentsElem.AppendElem(component.TypeName));
            }
        }

        /// <summary>
        /// Gets all child components contained by the panel, recursively.
        /// </summary>
        public IEnumerable<Component> GetAllChildren()
        {
            foreach (Component component in Components)
            {
                yield return component;

                if (component is Panel panel)
                {
                    foreach (Component childComponent in panel.GetAllChildren())
                    {
                        yield return childComponent;
                    }
                }
            }
        }
    }
}
