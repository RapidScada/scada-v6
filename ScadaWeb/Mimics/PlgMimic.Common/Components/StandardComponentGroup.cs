// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Represents a group of standard components.
    /// <para>Представляет группу стандартных компонентов.</para>
    /// </summary>
    public class StandardComponentGroup : ComponentGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StandardComponentGroup()
        {
            Name = MimicPhrases.StandardGroup;
            DictionaryPrefix = MimicConst.MimicModelPrefix;
            const string IconPath = "~/plugins/Mimic/images/";

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "text-icon.png",
                DisplayName = MimicPhrases.TextComponent,
                TypeName = "Text"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "picture-icon.png",
                DisplayName = MimicPhrases.PictureComponent,
                TypeName = "Picture"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "panel-icon.png",
                DisplayName = MimicPhrases.PanelComponent,
                TypeName = "Panel"
            });

            Items.Sort();
        }
    }
}
