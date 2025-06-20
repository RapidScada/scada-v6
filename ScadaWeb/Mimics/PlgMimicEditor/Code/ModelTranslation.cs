// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;
using Scada.Web.Plugins.PlgMimic;
using Scada.Web.Plugins.PlgMimic.Components;

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Contains a translation of the mimic model.
    /// <para>Содержит перевод модели мнемосхемы.</para>
    /// </summary>
    public class ModelTranslation
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
        /// Key is a component type name. 
        /// Value is a dictionary containing property names and their display names.
        /// </summary>
        public Dictionary<string, LocaleDict> ComponentDicts { get; private set; } = [];

        /// <summary>
        /// Gets the translation of subtype fields.
        /// Key is a subtype name. 
        /// For enumerations, value is a dictionary containing field values and corresponding display values.
        /// For structures, value is a dictionary containing field names and corresponding display names.
        /// </summary>
        public Dictionary<string, LocaleDict> SubtypeDicts { get; private set; } = [];


        /// <summary>
        /// Initializes the dictionaries.
        /// </summary>
        public void Init(ModelMeta modelMeta)
        {
            ArgumentNullException.ThrowIfNull(modelMeta, nameof(modelMeta));
            CategoryDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Category");
            MimicDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Mimic");
            ComponentDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Component");
            ComponentDicts = [];

            foreach (ComponentGroup componentGroup in modelMeta.ComponentGroups)
            {
                foreach (ComponentItem componentItem in componentGroup.Items)
                {
                    ComponentDicts[componentItem.TypeName] =
                        Locale.GetDictionary(componentGroup.DictionaryPrefix + componentItem.TypeName);
                }
            }

            foreach (SubtypeGroup subtypeGroup in modelMeta.SubtypeGroups)
            {
                foreach (string subtypeName in subtypeGroup.Names)
                {
                    SubtypeDicts[subtypeName] = Locale.GetDictionary(subtypeGroup.DictionaryPrefix + subtypeName);
                }
            }
        }
    }
}
