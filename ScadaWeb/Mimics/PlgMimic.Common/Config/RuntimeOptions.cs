// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.Config
{
    /// <summary>
    /// Represents runtime options of mimic diagrams.
    /// <para>Представляет параметры мнемосхем времени выполнения.</para>
    /// </summary>
    public class RuntimeOptions
    {
        /// <summary>
        /// Gets or sets the scale type.
        /// </summary>
        public ScaleType ScaleType { get; set; } = ScaleType.Numeric;

        /// <summary>
        /// Gets or sets the scale value.
        /// </summary>
        public double ScaleValue { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets a value indicating whether to remember last scheme scale.
        /// </summary>
        public bool RememberScale { get; set; } = true;


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            ScaleType = xmlNode.GetChildAsEnum("ScaleType", ScaleType);
            ScaleValue = xmlNode.GetChildAsDouble("ScaleValue", ScaleValue);
            RememberScale = xmlNode.GetChildAsBool("RememberScale", RememberScale);
        }
    }
}
