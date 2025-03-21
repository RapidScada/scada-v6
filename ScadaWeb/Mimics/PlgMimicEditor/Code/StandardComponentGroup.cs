// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents a group of standard components.
    /// <para>Представляет группу стандартных компонентов.</para>
    /// </summary>
    internal class StandardComponentGroup : ComponentGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public StandardComponentGroup()
        {
            Name = EditorPhrases.StandardGroup;
            const string IconPath = "~/plugins/MimicEditor/images/";

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "text-icon.png",
                DisplayName = EditorPhrases.TextComponent,
                TypeName = "Text"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "picture-icon.png",
                DisplayName = EditorPhrases.PictureComponent,
                TypeName = "Picture"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "panel-icon.png",
                DisplayName = EditorPhrases.PanelComponent,
                TypeName = "Panel"
            });

            Items.Sort();
        }
    }
}
