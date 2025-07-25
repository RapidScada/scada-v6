// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Log;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Components;
using Scada.Web.Plugins.PlgMimic.Config;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Contains plugin data.
    /// <para>Содержит данные плагина.</para>
    /// </summary>
    public class PluginContext
    {
        private readonly IWebContext webContext; // the web application context
        private readonly ILog log;               // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginContext(IWebContext webContext)
        {
            this.webContext = webContext ?? throw new ArgumentNullException(nameof(webContext));
            log = webContext.Log;

            PluginConfig = new MimicPluginConfig();
            ComponentSpecs = [];
            PageReferences = new PageReferences();
        }


        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        public MimicPluginConfig PluginConfig { get; }

        /// <summary>
        /// Gets the component library specifications.
        /// </summary>
        public List<IComponentSpec> ComponentSpecs { get; }

        /// <summary>
        /// Gets the references to insert into a page that contains a mimic.
        /// </summary>
        public PageReferences PageReferences { get; }


        /// <summary>
        /// Loads the plugin configuration.
        /// </summary>
        private void LoadConfig()
        {
            if (!PluginConfig.Load(webContext.Storage, MimicPluginConfig.DefaultFileName,
                out string errMsg))
            {
                log.WriteError(WebPhrases.PluginMessage, MimicPluginInfo.PluginCode, errMsg);
            }
        }

        /// <summary>
        /// Retrieves mimic components from the active plugins.
        /// </summary>
        private void RetrieveComponents()
        {
            ComponentSpecs.Clear();

            foreach (PluginLogic pluginLogic in webContext.PluginHolder.EnumeratePlugins())
            {
                if (pluginLogic is IComponentPlugin componentPlugin &&
                    componentPlugin.ComponentSpec is IComponentSpec componentSpec)
                {
                    ComponentSpecs.Add(componentSpec);
                }
            }
        }

        /// <summary>
        /// Fills in the page references based on the plugin configuration and available components.
        /// </summary>
        private void FillPageReferences()
        {
            PageReferences.Clear();
            PageReferences.RegisterFonts(PluginConfig.Fonts);
            PageReferences.RegisterComponents(ComponentSpecs);
        }

        /// <summary>
        /// Initializes the plugin context.
        /// </summary>
        public void Init()
        {
            LoadConfig();
            RetrieveComponents();
            FillPageReferences();
        }
    }
}
