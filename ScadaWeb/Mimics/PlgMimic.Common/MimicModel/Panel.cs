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
    public class Panel : Component
    {
        /// <summary>
        /// The XML node name for panels.
        /// </summary>
        public const string NodeName = "Panel";

        /// <summary>
        /// The names of panel nodes that are loaded explicitly.
        /// </summary>
        private static readonly HashSet<string> PanelKnownNodes = [.. ComponentKnownNodes, "Components"];

        /// <summary>
        /// Gets the names of nodes that are loaded explicitly.
        /// </summary>
        protected override HashSet<string> KnownNodes => PanelKnownNodes;

        /// <summary>
        /// Gets the components contained within the panel.
        /// </summary>
        [JsonIgnore]
        public List<Component> Components { get; } = [];


        /// <summary>
        /// Loads the panel from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode, HashSet<int> componentIDs)
        {
            ArgumentNullException.ThrowIfNull(componentIDs, nameof(componentIDs));
            base.LoadFromXml(xmlNode, componentIDs);

            if (xmlNode.SelectSingleNode("Components") is XmlNode componentsNode)
            {
                foreach (XmlNode childNode in componentsNode.ChildNodes)
                {
                    Component component = childNode.Name == NodeName ? new Panel() : new Component();
                    component.LoadFromXml(childNode, componentIDs);

                    if (component.ID > 0 && !componentIDs.Contains(component.ID))
                        Components.Add(component);
                }
            }
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
    }
}
