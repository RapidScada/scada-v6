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
        /// Gets the translation of the basic component properties. 
        /// Key is a property name. Value is a display name.
        /// </summary>
        public LocaleDict ComponentDict { get; private set; }

        /// <summary>
        /// Gets the translation of the basic structure properties. 
        /// Key is a property name. Value is a display name.
        /// </summary>
        public LocaleDict StructureDict { get; private set; }

        /// <summary>
        /// Gets the translation of component properties.
        /// Key is a component type name. 
        /// Value is a dictionary of property names and their display names.
        /// </summary>
        public Dictionary<string, LocaleDict> ComponentDicts { get; private set; } = [];

        /// <summary>
        /// Gets the translation of enumerations.
        /// Key is an enumeration name. 
        /// Value is a dictionary of property values and corresponding display values.
        /// </summary>
        public Dictionary<string, LocaleDict> EnumDicts { get; private set; } = [];

        /// <summary>
        /// Gets the translation of structures.
        /// Key is a structure name. 
        /// Value is a dictionary of field names and their display names.
        /// </summary>
        public Dictionary<string, LocaleDict> StructDicts { get; private set; } = [];


        /// <summary>
        /// Initializes the dictionaries.
        /// </summary>
        public void Init(ModelMeta modelMeta)
        {
            ArgumentNullException.ThrowIfNull(modelMeta, nameof(modelMeta));
            CategoryDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Category");
            MimicDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Mimic");
            ComponentDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Component");
            StructureDict = Locale.GetDictionary(MimicConst.MimicModelPrefix + "Structure");
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
                foreach (string enumName in subtypeGroup.EnumNames)
                {
                    EnumDicts[enumName] = Locale.GetDictionary(subtypeGroup.DictionaryPrefix + enumName);
                }

                foreach (string structName in subtypeGroup.StructNames)
                {
                    StructDicts[structName] = Locale.GetDictionary(subtypeGroup.DictionaryPrefix + structName);
                }
            }
        }
    }
}
