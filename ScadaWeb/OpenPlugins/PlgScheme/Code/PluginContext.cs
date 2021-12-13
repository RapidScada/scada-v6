// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using Scada.Lang;
using Scada.Log;
using Scada.Web.Config;
using Scada.Web.Lang;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgScheme.Code
{
    /// <summary>
    /// Contains plugin data.
    /// <para>Содержит данные плагина.</para>
    /// </summary>
    public class PluginContext
    {
        private readonly ILog log; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginContext(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Options = new SchemeOptions(new OptionList());
            CompManager = new CompManager(log);
        }


        /// <summary>
        /// Gets or sets the scheme options.
        /// </summary>
        public SchemeOptions Options { get; private set; }

        /// <summary>
        /// Gets the component manager.
        /// </summary>
        public CompManager CompManager { get; }


        /// <summary>
        /// Loads the scheme options.
        /// </summary>
        public void LoadOptions(WebConfig webConfig)
        {
            Options = new SchemeOptions(webConfig.GetOptions("Scheme"))
            {
                RefreshRate = webConfig.DisplayOptions.RefreshRate
            };
        }

        /// <summary>
        /// Retrieves scheme components from the installed plugins.
        /// </summary>
        public void RetrieveComponents(IEnumerable<PluginLogic> plugins)
        {
            ArgumentNullException.ThrowIfNull(plugins, nameof(plugins));

            log.WriteAction(WebPhrases.PluginMessage, PluginUtils.PluginCode, Locale.IsRussian ?
                "Извлечение компонентов схем из установленных плагинов" :
                "Retrieve scheme components from the installed plugins");

            foreach (PluginLogic pluginLogic in plugins)
            {
                try
                {
                    if (pluginLogic is ISchemeComp schemeComp)
                        CompManager.AddComponents(schemeComp);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, WebPhrases.PluginMessage, PluginUtils.PluginCode,
                        string.Format(Locale.IsRussian ?
                            "Ошибка при извлечении компонентов из плагина {0}" :
                            "Error retrieving components from the {0} plugin", pluginLogic.Code));
                }
            }
        }
    }
}
