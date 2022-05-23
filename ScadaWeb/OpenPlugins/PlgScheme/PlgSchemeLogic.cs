// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgScheme.Code;
using Scada.Web.Services;
using System;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgScheme
{
    /// <summary>
    /// Represents a plugin logic.
    /// <para>Представляет логику плагина.</para>
    /// </summary>
    public class PlgSchemeLogic : PluginLogic
    {
        private readonly PluginContext pluginContext;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgSchemeLogic(IWebContext webContext)
            : base(webContext)
        {
            pluginContext = new PluginContext(Log);
        }


        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => PluginUtils.PluginCode;

        /// <summary>
        /// Gets the view specifications.
        /// </summary>
        public override ICollection<ViewSpec> ViewSpecs => new ViewSpec[] { new SchemeViewSpec() };


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, Code, out string errMsg))
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);

            SchemePhrases.Init();
            PluginPhrases.Init();
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public override void LoadConfig()
        {
            pluginContext.LoadOptions(WebContext.AppConfig);
            pluginContext.RetrieveComponents(WebContext.PluginHolder.EnumeratePlugins());
        }

        /// <summary>
        /// Adds services to the DI container.
        /// </summary>
        public override void AddServices(IServiceCollection services)
        {
            services.AddSingleton(pluginContext);
        }

        /// <summary>
        /// Prepares the specified view provided by the plugin.
        /// </summary>
        public override void PrepareView(ViewBase view)
        {
            if (view is SchemeView schemeView)
                schemeView.CompManager = pluginContext.CompManager;
        }
    }
}
