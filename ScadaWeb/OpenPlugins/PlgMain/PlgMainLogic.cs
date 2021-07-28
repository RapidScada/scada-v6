// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Scada.Data.Entities;
using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgMain.Code;
using Scada.Web.Services;
using Scada.Web.TreeView;
using Scada.Web.Users;
using System.Collections.Generic;

namespace Scada.Web.Plugins.PlgMain
{
    /// <summary>
    /// Represents a plugin logic.
    /// <para>Представляет логику плагина.</para>
    /// </summary>
    public class PlgMainLogic : PluginLogic
    {
        private readonly PluginContext pluginContext;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgMainLogic(IWebContext webContext)
            : base(webContext)
        {
            pluginContext = new PluginContext();
        }


        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => PluginUtils.PluginCode;

        /// <summary>
        /// Gets the plugin features.
        /// </summary>
        public override PluginFeatures Features => new()
        {
            CommandScriptUrl = "~/plugins/Main/js/command-feature.js",
            EventAckScriptUrl = "~/plugins/Main/js/event-ack-feature.js",
        };

        /// <summary>
        /// Gets the view specifications.
        /// </summary>
        public override ICollection<ViewSpec> ViewSpecs => new ViewSpec[] { new TableViewSpec() };

        /// <summary>
        /// Gets the data window specifications.
        /// </summary>
        public override ICollection<DataWindowSpec> DataWindowSpecs => new DataWindowSpec[] { new EventWindowSpec() };


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "PlgMain", out string errMsg))
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);

            PluginPhrases.Init();
        }

        /// <summary>
        /// Loads configuration.
        /// </summary>
        public override void LoadConfig()
        {
            pluginContext.LoadOptions(WebContext.AppConfig.GetOptions("Main"));
        }

        /// <summary>
        /// Adds services to the DI container.
        /// </summary>
        public override void AddServices(IServiceCollection services)
        {
            services.AddSingleton(pluginContext);
        }

        /// <summary>
        /// Gets menu items available for the specified user.
        /// </summary>
        public override List<MenuItem> GetUserMenuItems(User user, UserRights userRights)
        {
            MenuItem reportsItem = MenuItem.FromKnownMenuItem(KnownMenuItem.Reports);
            reportsItem.Subitems.Add(new MenuItem { Text = "Data report", Url = "~/Main/DataRep", SortOrder = 0 });
            reportsItem.Subitems.Add(new MenuItem { Text = "Event report", Url = "~/Main/EventRep", SortOrder = 1 });
            return new List<MenuItem>() { reportsItem };
        }
    }
}
