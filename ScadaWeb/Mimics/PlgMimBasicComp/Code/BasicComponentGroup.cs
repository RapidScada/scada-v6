// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.Components;

namespace Scada.Web.Plugins.PlgMimBasicComp.Code
{
    /// <summary>
    /// Represents a group of basic components.
    /// <para>Представляет группу основных компонентов.</para>
    /// </summary>
    public class BasicComponentGroup : ComponentGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicComponentGroup()
        {
            Name = PluginPhrases.BasicGroup;
            DictionaryPrefix = PluginConst.ComponentModelPrefix;
            const string IconPath = "~/plugins/MimBasicComp/images/";

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "button-icon.png",
                DisplayName = PluginPhrases.ButtonComponent,
                TypeName = "BasicButton"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "led-icon.png",
                DisplayName = PluginPhrases.LedComponent,
                TypeName = "BasicLed"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "link-icon.png",
                DisplayName = PluginPhrases.LinkComponent,
                TypeName = "BasicLink"
            });

            Items.Add(new ComponentItem
            {
                IconUrl = IconPath + "toggle-icon.png",
                DisplayName = PluginPhrases.ToggleComponent,
                TypeName = "BasicToggle"
            });

            Items.Sort();
        }
    }
}
