// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml;

namespace Scada.Server.Modules.ModDifCalculator.Config
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    [Serializable]
    internal class ModuleConfig : ModuleConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = ModuleUtils.ModuleCode + ".xml";


        /// <summary>
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the configuration of the groups.
        /// </summary>
        public CalcGroupList CalcGroups { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            CalcGroups = [];
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("GeneralOptions") is XmlNode generalOptionsNode)
                GeneralOptions.LoadFromXml(generalOptionsNode);

            if (rootElem.SelectSingleNode("CalcGroups") is XmlNode calcGroupsNode)
            {
                foreach (XmlElement calcGroupElem in calcGroupsNode.SelectNodes("CalcGroup"))
                {
                    CalcGroupConfig calcGroupConfig = new() { Parent = CalcGroups };
                    calcGroupConfig.LoadFromXml(calcGroupElem);
                    CalcGroups.Add(calcGroupConfig);
                }
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

            XmlElement rootElem = xmlDoc.CreateElement(ModuleUtils.ModuleCode);
            xmlDoc.AppendChild(rootElem);

            GeneralOptions.SaveToXml(rootElem.AppendElem("GeneralOptions"));

            XmlElement calcGroupsElem = rootElem.AppendElem("CalcGroups");
            foreach (CalcGroupConfig calcGroupConfig in CalcGroups)
            {
                calcGroupConfig.SaveToXml(calcGroupsElem.AppendElem("CalcGroup"));
            }

            xmlDoc.Save(writer);
        }
    }
}
