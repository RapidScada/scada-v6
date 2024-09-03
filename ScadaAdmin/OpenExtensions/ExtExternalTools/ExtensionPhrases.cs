// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtExternalTools
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    internal static class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtExternalTools.ExtExternalToolsLogic
        public static string ParentMenuItem { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtExternalTools.ExtExternalToolsLogic");
            ParentMenuItem = dict[nameof(ParentMenuItem)];
        }
    }
}
