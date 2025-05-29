// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.Config
{
    /// <summary>
    /// Represents mimic editor options.
    /// <para>Представляет параметры редактора мнемосхем.</para>
    /// </summary>
    public class EditorOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether components are aligned to the grid.
        /// </summary>
        public bool UseGrid { get; set; } = true;

        /// <summary>
        /// Gets or sets the grid size in pixels.
        /// </summary>
        public int GridSize { get; set; } = 10;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            UseGrid = xmlNode.GetChildAsBool("UseGrid", UseGrid);
            GridSize = xmlNode.GetChildAsInt("GridSize", GridSize);
        }
    }
}
