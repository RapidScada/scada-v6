// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Web.Lang;
using Scada.Web.Plugins.PlgWebPage.Code;
using Scada.Web.Services;

namespace Scada.Web.Plugins.PlgWebPage
{
    /// <summary>
    /// Represents a plugin logic.
    /// <para>Представляет логику плагина.</para>
    /// </summary>
    public class PlgWebPageLogic : PluginLogic
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PlgWebPageLogic(IWebContext webContext)
            : base(webContext)
        {
        }


        /// <summary>
        /// Gets the plugin code.
        /// </summary>
        public override string Code => "PlgWebPage";

        /// <summary>
        /// Gets the view specifications.
        /// </summary>
        public override ICollection<ViewSpec> ViewSpecs => new ViewSpec[] { new WebPageViewSpec() };


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "PlgWebPage", out string errMsg))
                Log.WriteError(WebPhrases.PluginMessage, Code, errMsg);
        }
    }
}
