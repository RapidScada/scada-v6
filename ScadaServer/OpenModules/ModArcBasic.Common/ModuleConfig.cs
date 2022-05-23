// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.IO;
using System.Xml;

namespace Scada.Server.Modules.ModArcBasic
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    public class ModuleConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ModArcBasic.xml";


        /// <summary>
        /// Gets or sets a value indicating whether to use the default archive directories.
        /// </summary>
        public bool UseDefaultDir { get; set; }

        /// <summary>
        /// Gets or sets the root directory of the main archive.
        /// </summary>
        public string ArcDir { get; set; }

        /// <summary>
        /// Gets or sets the root directory of the archive copy.
        /// </summary>
        public string ArcCopyDir { get; set; }
        
        
        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            UseDefaultDir = true;
            ArcDir = "";
            ArcCopyDir = "";
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            UseDefaultDir = rootElem.GetChildAsBool("UseDefaultDir");
            ArcDir = ScadaUtils.NormalDir(rootElem.GetChildAsString("ArcDir"));
            ArcCopyDir = ScadaUtils.NormalDir(rootElem.GetChildAsString("ArcCopyDir"));
        }
        
        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("ModArcBasic");
            xmlDoc.AppendChild(rootElem);

            rootElem.AppendElem("UseDefaultDir", UseDefaultDir);
            rootElem.AppendElem("ArcDir", ArcDir);
            rootElem.AppendElem("ArcCopyDir", ArcCopyDir);

            xmlDoc.Save(writer);
        }


        /// <summary>
        /// Sets the default directories relative the instance directory.
        /// </summary>
        public void SetToDefault(string instanceDir)
        {
            ArcDir = Path.Combine(instanceDir, "Archive");
            ArcCopyDir = Path.Combine(instanceDir, "ArchiveCopy");
        }

        /// <summary>
        /// Selects between the main directory and the copy directory.
        /// </summary>
        public string SelectArcDir(bool useCopyDir)
        {
            return useCopyDir ? ArcCopyDir : ArcDir;
        }
    }
}
