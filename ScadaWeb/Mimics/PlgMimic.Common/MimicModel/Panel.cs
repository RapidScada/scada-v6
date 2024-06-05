// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        /// The names of panel nodes that are loaded explicitly.
        /// </summary>
        private static readonly HashSet<string> PanelKnownNodes = [.. ComponentKnownNodes, "components"];

        /// <summary>
        /// Gets the names of nodes that are loaded explicitly.
        /// </summary>
        protected override HashSet<string> KnownNodes => PanelKnownNodes;

        /// <summary>
        /// Gets the components contained within the panel.
        /// </summary>
        public List<Component> Components { get; } = [];


        /// <summary>
        /// Loads the component from the XML node.
        /// </summary>
        public override void LoadFromXml(XmlNode xmlNode)
        {
            base.LoadFromXml(xmlNode);
        }

        /// <summary>
        /// Saves the component into the XML node.
        /// </summary>
        public override void SaveToXml(XmlNode xmlNode)
        {
            base.SaveToXml(xmlNode);
        }
    }
}
