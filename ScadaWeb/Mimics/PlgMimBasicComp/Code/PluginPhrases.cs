// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMimBasicComp.Code
{
    /// <summary>
    /// The phrases used by the plugin.
    /// <para>Фразы, используемые плагином.</para>
    /// </summary>
    internal class PluginPhrases
    {
        // Scada.Web.Plugins.PlgMimBasicComp.Code.BasicComponentGroup
        public static string BasicGroup { get; private set; }
        public static string ButtonComponent { get; private set; }
        public static string LedComponent { get; private set; }
        public static string LinkComponent { get; private set; }
        public static string ToggleComponent { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMimBasicComp.Code.BasicComponentGroup");
            BasicGroup = dict[nameof(BasicGroup)];
            ButtonComponent = dict[nameof(ButtonComponent)];
            LedComponent = dict[nameof(LedComponent)];
            LinkComponent = dict[nameof(LinkComponent)];
            ToggleComponent = dict[nameof(ToggleComponent)];
        }
    }
}
