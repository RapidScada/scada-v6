// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// The phrases used by the plugin.
    /// <para>Фразы, используемые плагином.</para>
    /// </summary>
    internal static class PluginPhrases
    {
        public static string EditorMenuItem { get; private set; }
        public static string MimicsMenuItem { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMimicEditor.PlgMimicEditorLogic");
            EditorMenuItem = dict[nameof(EditorMenuItem)];
            MimicsMenuItem = dict[nameof(MimicsMenuItem)];
        }
    }
}
