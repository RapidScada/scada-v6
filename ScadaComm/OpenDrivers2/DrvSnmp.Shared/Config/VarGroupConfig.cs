// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using System.Collections;
using System.Xml;
using NCM = System.ComponentModel;

namespace Scada.Comm.Drivers.DrvSnmp.Config
{
    /// <summary>
    /// Represents a variable group configuration.
    /// <para>Представляет конфигурацию группы переменных.</para>
    /// </summary>
    [Serializable]
    internal class VarGroupConfig : ITreeNode
    {
        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        [DisplayName, Category, Description, NCM.TypeConverter(typeof(BooleanConverter))]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [DisplayName, Category, Description]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets the variables.
        /// </summary>
        [NCM.Browsable(false)]
        public List<VariableConfig> Variables { get; } = new();

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        [NCM.Browsable(false)]
        public ITreeNode Parent { get; set; }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        [NCM.Browsable(false)]
        public IList Children
        {
            get
            {
                return Variables;
            }
        }


        /// <summary>
        /// Loads the configuration from the XML node.
        /// </summary>
        public void LoadFromXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            Active = xmlElem.GetAttrAsBool("active");
            Name = xmlElem.GetAttrAsString("name");

            foreach (XmlElement variableElem in xmlElem.SelectNodes("Variable"))
            {
                VariableConfig variableConfig = new() { Parent = this };
                variableConfig.LoadFromXml(variableElem);
                Variables.Add(variableConfig);
            }
        }

        /// <summary>
        /// Saves the configuration into the XML node.
        /// </summary>
        public void SaveToXml(XmlElement xmlElem)
        {
            ArgumentNullException.ThrowIfNull(xmlElem, nameof(xmlElem));
            xmlElem.SetAttribute("active", Active);
            xmlElem.SetAttribute("name", Name);

            foreach (VariableConfig variableConfig in Variables)
            {
                variableConfig.SaveToXml(xmlElem.AppendElem("Variable"));
            }
        }
    }
}
