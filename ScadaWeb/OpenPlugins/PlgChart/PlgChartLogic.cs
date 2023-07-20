// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// Implements the plugin logic.
    /// <para>Реализует логику плагина.</para>
    /// </summary>
    public class PlgChartLogic : PluginLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgChartLogic(IWebContext webContext)
            : base(webContext)
        {
        }


        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgChart";

        /// <summary>
        /// Gets the plugin features.
        /// </summary>
        public override PluginFeatures Features => new()
        {
            ChartScriptUrl = "~/plugins/Chart/js/chart-feature.js",
        };


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, Code, out string errMsg))
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);
        }
    }
}
