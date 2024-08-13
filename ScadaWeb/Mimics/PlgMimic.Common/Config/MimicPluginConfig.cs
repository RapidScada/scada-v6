// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;

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
        /// Gets the general options.
        /// </summary>
        public GeneralOptions GeneralOptions { get; private set; }

        /// <summary>
        /// Gets the fonts.
        /// </summary>
        public List<FontOptions> Fonts { get; private set; }
        
        
        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            GeneralOptions = new GeneralOptions();
            Fonts = [];
        }

        /// <summary>
        /// Loads the configuration from the specified reader.
        /// </summary>
        protected override void Load(TextReader reader)
        {

        }
    }
}
