// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;

namespace Scada.Web.Plugins.PlgMain.Code
{
    /// <summary>
    /// Contains plugin data.
    /// <para>Содержит данные плагина.</para>
    /// </summary>
    public class PluginContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginContext()
        {
            Options = new PluginOptions(new OptionList());
        }

        /// <summary>
        /// Gets or sets the plugin options.
        /// </summary>
        public PluginOptions Options { get; private set; }

        /// <summary>
        /// Loads the plugin options.
        /// </summary>
        public void LoadOptions(OptionList options)
        {
            Options = new PluginOptions(options);
        }
    }
}
