// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgScheme.Code
{
    /// <summary>
    /// The phrases used by the plugin.
    /// <para>Фразы, используемые плагином.</para>
    /// </summary>
    internal static class PluginPhrases
    {
        // Scada.Web.Plugins.PlgScheme.Areas.Scheme.Pages.SchemeView
        public static string SchemeViewTitle { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgScheme.Areas.Scheme.Pages.SchemeView");
            SchemeViewTitle = dict["SchemeViewTitle"];
        }
    }
}
