// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Template
{
    /// <summary>
    /// Represents component bindings to channels.
    /// <para>Представляет привязки компонента к каналам.</para>
    /// </summary>
    public class ComponentBinding
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ComponentBinding()
        {
            CompID = 0;
            InCnlNum = 0;
            CtrlCnlNum = 0;
        }


        /// <summary>
        /// Gets or sets the component ID.
        /// </summary>
        public int CompID { get; set; }

        /// <summary>
        /// Gets or sets the input channel number to which the component is bound.
        /// </summary>
        public int InCnlNum { get; set; }

        /// <summary>
        /// Gets or sets the output channel number to which the component is bound.
        /// </summary>
        public int CtrlCnlNum { get; set; }

        /// <summary>
        /// Loads the bindongs from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            if (xmlElem == null)
                throw new ArgumentNullException(nameof(xmlElem));

            CompID = xmlElem.GetAttrAsInt("compID");
            InCnlNum = xmlElem.GetAttrAsInt("inCnlNum");
            CtrlCnlNum = xmlElem.GetAttrAsInt("ctrlCnlNum");
        }
    }
}
