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
        // Scada.Admin.Extensions.ExtWebConfig.ExtWebConfigLogic
        public static string LogsNode { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtWebConfig.ExtWebConfigLogic");
            LogsNode = dict["LogsNode"];
        }
    }
}
