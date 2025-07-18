// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMimic.Code;
using Scada.Web.Plugins.PlgMimic.Config;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgMimic
{
    /// <summary>
    /// Implements the plugin logic.
    /// <para>Реализует логику плагина.</para>
    /// </summary>
    public class PlgMimicLogic : PluginLogic
    {
        private readonly PluginContext pluginContext;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgMimicLogic(IWebContext webContext)
            : base(webContext)
        {
            pluginContext = new PluginContext();
            Info = new MimicPluginInfo();
        }


        /// <summary>
        /// Gets the view specifications.
        /// </summary>
        public override ICollection<ViewSpec> ViewSpecs => [new MimicViewSpec()];


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, Code, out string errMsg))
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);

            MimicPhrases.Init();
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public override void LoadConfig()
        {
            if (!pluginContext.PluginConfig.Load(WebContext.Storage, MimicPluginConfig.DefaultFileName,
                out string errMsg))
            {
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);
            }
        }

        /// <summary>
        /// Adds services to the DI container.
        /// </summary>
        public override void AddServices(IServiceCollection services)
        {
            services.AddSingleton(pluginContext);
        }
    }
}
