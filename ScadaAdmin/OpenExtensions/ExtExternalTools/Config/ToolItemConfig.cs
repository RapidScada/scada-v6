// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Admin.Extensions.ExtExternalTools.Config
{
    /// <summary>
    /// Represents a tool item configuration.
    /// <para>Представляет конфигурацию элемента, соответствующего инструменту.</para>
    /// </summary>
    internal class ToolItemConfig
    {
        /// <summary>
        /// Gets or sets the title displayed as the menu item text.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets or sets the file name to execute.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// Gets or sets the command-line arguments.
        /// </summary>
        public string Arguments { get; set; } = "";

        /// <summary>
        /// Gets or sets the working directory for the process to be started.
        /// </summary>
        public string WorkingDirectory { get; set; } = "";


        /// <summary>
        /// Loads the options from the XML node.
        /// </summary>
        public void LoadFromXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            Title = xmlNode.GetChildAsString("Title");
            FileName = xmlNode.GetChildAsString("FileName");
            Arguments = xmlNode.GetChildAsString("Arguments");
            WorkingDirectory = xmlNode.GetChildAsString("WorkingDirectory");
        }

        /// <summary>
        /// Saves the options into the XML node.
        /// </summary>
        public void SaveToXml(XmlNode xmlNode)
        {
            ArgumentNullException.ThrowIfNull(xmlNode, nameof(xmlNode));
            xmlNode.AppendElem("Title", Title);
            xmlNode.AppendElem("FileName", FileName);
            xmlNode.AppendElem("Arguments", Arguments);
            xmlNode.AppendElem("WorkingDirectory", WorkingDirectory);
        }
    }
}
