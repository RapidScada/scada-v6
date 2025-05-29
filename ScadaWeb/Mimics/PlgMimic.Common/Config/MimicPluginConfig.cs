// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Xml;

namespace Scada.Web.Plugins.PlgMimic.Config
{
    /// <summary>
    /// Represents a plugin configuration.
    /// <para>Представляет конфигурацию плагина.</para>
    /// </summary>
    public class MimicPluginConfig : ConfigBase
    {
        /// <summary>
        /// The default configuration file name.
        /// </summary>
        public const string DefaultFileName = MimicPluginInfo.PluginCode + ".xml";


        /// <summary>
        /// Gets the fonts.
        /// </summary>
        public List<FontOptions> Fonts { get; private set; }

        /// <summary>
        /// Gets the runtime options.
        /// </summary>
        public RuntimeOptions RuntimeOptions { get; private set; }

        /// <summary>
        /// Gets the editor options.
        /// </summary>
        public EditorOptions EditorOptions { get; private set; }


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            Fonts = [];
            RuntimeOptions = new RuntimeOptions();
            EditorOptions = new EditorOptions();
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(reader);
            XmlElement rootElem = xmlDoc.DocumentElement;

            if (rootElem.SelectSingleNode("Fonts") is XmlNode fontsNode)
            {
                foreach (XmlNode fontNode in fontsNode.SelectNodes("Font"))
                {
                    FontOptions fontOptions = new();
                    fontOptions.LoadFromXml(fontNode);
                    Fonts.Add(fontOptions);
                }
            }

            if (rootElem.SelectSingleNode("RuntimeOptions") is XmlNode runtimeOptionsNode)
                RuntimeOptions.LoadFromXml(runtimeOptionsNode);

            if (rootElem.SelectSingleNode("EditorOptions") is XmlNode editorOptionsNode)
                EditorOptions.LoadFromXml(editorOptionsNode);
        }
    }
}
