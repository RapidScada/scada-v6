// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents a device template.
    /// <para>Представляет шаблон устройства.</para>
    /// </summary>
    public class DeviceTemplate : ConfigBase
    {
        /// <summary>
        /// Gets the device template options.
        /// </summary>
        public DeviceTemplateOptions Options { get; private set; }

        /// <summary>
        /// Gets the configuration of the element groups.
        /// </summary>
        public List<ElemGroupConfig> ElemGroups { get; private set; }

        /// <summary>
        /// Gets the configuration of the commands.
        /// </summary>
        public List<CmdConfig> Cmds { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Options = new DeviceTemplateOptions();
            ElemGroups = new List<ElemGroupConfig>();
            Cmds = new List<CmdConfig>();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            // template options
            if ((rootElem.SelectSingleNode("Options") ?? rootElem.SelectSingleNode("Settings")) is XmlElement optionsElem)
                Options.LoadFromXml(optionsElem);

            // element groups
            if (rootElem.SelectSingleNode("ElemGroups") is XmlNode elemGroupsNode)
            {
                int tagCnt = 0;

                foreach (XmlElement elemGroupElem in elemGroupsNode.SelectNodes("ElemGroup"))
                {
                    ElemGroupConfig elemGroupConfig = CreateElemGroupConfig();
                    elemGroupConfig.LoadFromXml(elemGroupElem);
                    elemGroupConfig.StartTagNum = tagCnt + 1;
                    ElemGroups.Add(elemGroupConfig);
                    tagCnt += elemGroupConfig.Elems.Count;
                }
            }

            // commands
            if (rootElem.SelectSingleNode("Cmds") is XmlNode cmdsNode)
            {
                foreach (XmlElement cmdElem in cmdsNode.SelectNodes("Cmd"))
                {
                    CmdConfig cmdConfig = CreateCmdConfig();
                    cmdConfig.LoadFromXml(cmdElem);
                    Cmds.Add(cmdConfig);
                }
            }
        }

        /// <summary>
        /// Saves the configuration to the specified writer.
        /// </summary>
        protected override void Save(TextWriter writer)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            XmlElement rootElem = xmlDoc.CreateElement("DeviceTemplate");
            xmlDoc.AppendChild(rootElem);

            // template options
            Options.SaveToXml(rootElem.AppendElem("Options"));

            // element groups
            XmlElement elemGroupsElem = rootElem.AppendElem("ElemGroups");
            foreach (ElemGroupConfig elemGroupConfig in ElemGroups)
            {
                elemGroupConfig.SaveToXml(elemGroupsElem.AppendElem("ElemGroup"));
            }

            // commands
            XmlElement cmdsElem = rootElem.AppendElem("Cmds");
            foreach (CmdConfig cmdConfig in Cmds)
            {
                cmdConfig.SaveToXml(cmdsElem.AppendElem("Cmd"));
            }

            xmlDoc.Save(writer);
        }

        /// <summary>
        /// Builds an error message for the load operation.
        /// </summary>
        protected override string BuildLoadErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(ModbusDriverPhrases.LoadTemplateError);
        }

        /// <summary>
        /// Builds an error message for the save operation.
        /// </summary>
        protected override string BuildSaveErrorMessage(Exception ex)
        {
            return ex.BuildErrorMessage(ModbusDriverPhrases.SaveTemplateError);
        }


        /// <summary>
        /// Creates a new element group configuration.
        /// </summary>
        public virtual ElemGroupConfig CreateElemGroupConfig()
        {
            return new ElemGroupConfig();
        }

        /// <summary>
        /// Creates a new command configuration.
        /// </summary>
        public virtual CmdConfig CreateCmdConfig()
        {
            return new CmdConfig();
        }
    }
}
