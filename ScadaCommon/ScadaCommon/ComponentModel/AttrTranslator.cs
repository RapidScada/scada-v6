/*
 * Copyright 2022 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ScadaCommon
 * Summary  : Type attributes translation
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2017
 */

using Scada.Lang;
using System;
using System.Collections.Generic;
using NCM = System.ComponentModel;
using SCM = Scada.ComponentModel;

namespace Scada.ComponentModel
{
    /// <summary>
    /// Translates property attributes.
    /// <para>Переводит атрибуты свойств.</para>
    /// </summary>
    public static class AttrTranslator
    {
        /// <summary>
        /// Contains attribute values of a property.
        /// </summary>
        private class PropAttrs
        {
            public string DisplayName { get; set; } = "";
            public string Category { get; set; } = "";
            public string Description { get; set; } = "";
        }


        /// <summary>
        /// Builds a dictionary that contains attribute values accessed by property name.
        /// </summary>
        private static Dictionary<string, PropAttrs> BuildPropAttrsDict(LocaleDict dict)
        {
            Dictionary<string, PropAttrs> propAttrsDict = new Dictionary<string, PropAttrs>();

            foreach (string phraseKey in dict.Phrases.Keys)
            {
                string phraseVal = dict.Phrases[phraseKey];
                int dotIdx = phraseKey.IndexOf('.');

                if (0 < dotIdx && dotIdx < phraseKey.Length - 1)
                {
                    // PropertyName.AttributeName
                    string propName = phraseKey.Substring(0, dotIdx);
                    string attrName = phraseKey.Substring(dotIdx + 1);
                    bool itemFound = propAttrsDict.TryGetValue(propName, out PropAttrs propAttrs);

                    if (!itemFound)
                    {
                        propAttrs = new PropAttrs();
                        propAttrsDict.Add(propName, propAttrs);
                    }

                    if (attrName == "DisplayName")
                        propAttrs.DisplayName = phraseVal;
                    else if (attrName == "Category")
                        propAttrs.Category = phraseVal;
                    else if (attrName == "Description")
                        propAttrs.Description = phraseVal;
                }
            }

            return propAttrsDict;
        }

        /// <summary>
        /// Translates attributes of the specified type.
        /// </summary>
        /// <remarks>A dictionary with a key that matches the full type name is selected.</remarks>
        public static void Translate(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (Locale.Dictionaries.TryGetValue(type.FullName, out LocaleDict localeDict))
            {
                Dictionary<string, PropAttrs> propAttrsDict = BuildPropAttrsDict(localeDict);
                NCM.PropertyDescriptorCollection allProps = NCM.TypeDescriptor.GetProperties(type);

                foreach (NCM.PropertyDescriptor prop in allProps)
                {
                    if (propAttrsDict.TryGetValue(prop.Name, out PropAttrs propAttrs))
                    {
                        foreach (Attribute attr in prop.Attributes)
                        {
                            if (attr is SCM.DisplayNameAttribute displayNameAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.DisplayName))
                                    displayNameAttribute.DisplayNameValue = propAttrs.DisplayName;
                            }
                            else if (attr is SCM.CategoryAttribute categoryAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.Category))
                                    categoryAttribute.CategoryName = propAttrs.Category;
                            }
                            else if (attr is SCM.DescriptionAttribute descriptionAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.Description))
                                    descriptionAttribute.DescriptionValue = propAttrs.Description;
                            }
                        }
                    }
                }
            }
        }
    }
}
