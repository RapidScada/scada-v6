/*
 * Copyright 2020 Mikhail Shiryaev
 * All rights reserved
 * 
 * Product  : Rapid SCADA
 * Module   : ModArcInfluxDb
 * Summary  : Represents a module configuration
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Lang;
using Scada.Server.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Scada.Server.Modules.ModArcInfluxDb.Logic.Config
{
    /// <summary>
    /// Represents a module configuration.
    /// <para>Представляет конфигурацию модуля.</para>
    /// </summary>
    internal class ModuleConfig
    {
        /// <summary>
        /// The configuration file name.
        /// </summary>
        private const string ConfigFileName = "ModArcInfluxDb.xml";


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleConfig()
        {
            SetToDefault();
        }


        /// <summary>
        /// Gets the connection options accessed by connection name.
        /// </summary>
        public SortedList<string, ConnectionOptions> Connections { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        private void SetToDefault()
        {
            Connections = new SortedList<string, ConnectionOptions>();
        }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (!File.Exists(fileName))
                    throw new FileNotFoundException(string.Format(CommonPhrases.NamedFileNotFound, fileName));

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                if (xmlDoc.DocumentElement.SelectSingleNode("Connections") is XmlNode connectionsNode)
                {
                    foreach (XmlNode connectionNode in connectionsNode.SelectNodes("Connection"))
                    {
                        ConnectionOptions connectionOptions = new ConnectionOptions();
                        connectionOptions.LoadFromXml(connectionNode);
                        Connections[connectionOptions.Name] = connectionOptions;
                    }
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ScadaUtils.BuildErrorMessage(ex, CommonPhrases.LoadConfigError);
                return false;
            }
        }

        /// <summary>
        /// Gets the full name of the configuration file.
        /// </summary>
        public static string GetFilePath(string configDir)
        {
            return Path.Combine(configDir, ConfigFileName);
        }
    }
}
