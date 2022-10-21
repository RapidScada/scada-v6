// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtWebConfig.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtWebConfig.Forms.FrmApplicationOptions
        public static string ApplicationConfigTitle { get; private set; }

        // Scada.Admin.Extensions.ExtWebConfig.ExtWebConfigLogic
        public static string LogsNode { get; private set; }
        public static string ApplicationOptionsNode { get; private set; }
        public static string PluginsNode { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtWebConfig.Forms.FrmApplicationOptions");
            ApplicationConfigTitle = dict[nameof(ApplicationConfigTitle)];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtWebConfig.ExtWebConfigLogic");
            LogsNode = dict[nameof(LogsNode)];
            ApplicationOptionsNode = dict[nameof(ApplicationOptionsNode)];
            PluginsNode = dict[nameof(PluginsNode)];
        }
    }
}
