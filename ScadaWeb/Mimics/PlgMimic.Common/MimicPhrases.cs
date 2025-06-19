// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMimic
{
    /// <summary>
    /// The phrases used by mimics.
    /// <para>Фразы, используемые мнемосхемами.</para>
    /// </summary>
    public static class MimicPhrases
    {
        // Scada.Web.Plugins.PlgMimic.Components.StandardComponentGroup
        public static string StandardGroup { get; private set; }
        public static string TextComponent { get; private set; }
        public static string PictureComponent { get; private set; }
        public static string PanelComponent { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Web.Plugins.PlgMimic.Components.StandardComponentGroup");
            StandardGroup = dict[nameof(StandardGroup)];
            TextComponent = dict[nameof(TextComponent)];
            PictureComponent = dict[nameof(PictureComponent)];
            PanelComponent = dict[nameof(PanelComponent)];
        }
    }
}
