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
            Name = "Standard";
            const string IconPath = "~/plugins/MimicEditor/images/";

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "text-icon.png",
                DisplayName = "Text",
                TypeName = "Text"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "picture-icon.png",
                DisplayName = "Picture",
                TypeName = "Picture"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "panel-icon.png",
                DisplayName = "Panel",
                TypeName = "Panel"
            });

            Items.Sort();
        }
    }
}
