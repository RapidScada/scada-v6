// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using System.Xml;

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Represents an editor configuration.
    /// <para>Представляет конфигурацию редактора.</para>
    /// </summary>
    public class EditorConfig
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = "ScadaSchemeEditorConfig.xml";

        private readonly AppDirs appDirs;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EditorConfig(AppDirs appDirs)
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            ResourceDir = "";
        }

        /// <summary>
        /// Gets or sets the resource directory.
        /// </summary>
        public string ResourceDir { get; set; }

        /// <summary>
        /// Loads the configuration from the specified file.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.Load(fileName);
                XmlElement rootElem = xmlDoc.DocumentElement;

                ResourceDir = ScadaUtils.FirstNonEmpty(
                    rootElem.GetChildAsString("ResourceDir"),
                    Path.Combine(appDirs.ExeDir, "Resources"));

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.BuildErrorMessage(CommonPhrases.LoadConfigError);
                return false;
            }
        }
    }
}
