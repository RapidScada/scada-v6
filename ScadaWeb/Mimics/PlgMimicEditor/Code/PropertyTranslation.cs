// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Contains translation of mimic and component properties.
    /// <para>Содержит перевод свойств мнемосхемы и компонентов.</para>
    /// </summary>
    public class PropertyTranslation
    {
        /// <summary>
        /// Gets the translation of property categories.
        /// </summary>
        public LocaleDict CategoryDict { get; private set; }

        /// <summary>
        /// Gets the translation of the mimic properties. 
        /// Key is a property name. Value is a display name.
        /// </summary>
        public LocaleDict MimicDict { get; private set; }

        /// <summary>
        /// Gets the translation of the base component properties. 
        /// Key is a property name. Value is a display name.
        /// </summary>
        public LocaleDict ComponentDict { get; private set; }

        /// <summary>
        /// Gets the translation of component properties.
        /// Key is a component type name. Value is a dictionary containing property names and their display names.
        /// </summary>
        public Dictionary<string, LocaleDict> ComponentDicts { get; private set; } = [];


        /// <summary>
        /// Initializes the dictionaries.
        /// </summary>
        public void Init(ComponentList componentList)
        {
            ArgumentNullException.ThrowIfNull(componentList, nameof(componentList));

            const string DictKeyPrefix = "Scada.Web.Plugins.PlgMimicEditor.MimicModel.Js.";
            CategoryDict = Locale.GetDictionary(DictKeyPrefix + "Category");
            MimicDict = Locale.GetDictionary(DictKeyPrefix + "Mimic");
            ComponentDict = Locale.GetDictionary(DictKeyPrefix + "Component");
        }
    }
}
