// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Xml;

namespace Scada.Server.Modules.ModDbExport.Config
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    [Serializable]
    internal class ModuleConfig : ModuleConfigBase, ITreeNode
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ModDbExport.xml";
        
        
        /// <summary>
        /// Gets the configuration of the export targets.
        /// </summary>
        public List<ExportTargetConfig> ExportTargets { get; private set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        ITreeNode ITreeNode.Parent
        {
            get => null;
            set => throw new InvalidOperationException();
        }

        /// <summary>
        /// Get a list of child nodes.
        /// </summary>
        IList ITreeNode.Children => ExportTargets;


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            ExportTargets = new List<ExportTargetConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);

            foreach (XmlElement exportTargetElem in xmlDoc.DocumentElement.SelectNodes("ExportTarget"))
            {
                ExportTargetConfig exportTargetConfig = new() { Parent = this };
                exportTargetConfig.LoadFromXml(exportTargetElem);
                ExportTargets.Add(exportTargetConfig);
            }
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ModDbExport");
            xmlDoc.AppendChild(rootElem);

            foreach (ExportTargetConfig exportTargetConfig in ExportTargets)
            {
                exportTargetConfig.SaveToXml(rootElem.AppendElem("ExportTarget"));
            }

            xmlDoc.Save(writer);
        }
    }
}
