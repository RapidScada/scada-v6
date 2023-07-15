// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Web.Plugins.PlgScheme.Model.PropertyGrid
{
    /// <summary>
    /// Type attributes translation
    /// <para>Перевод атрибутов типа</para>
    /// </summary>
    public static class AttrTranslator
    {
        /// <summary>
        /// Атрибуты свойства
        /// </summary>
        private class PropAttrs
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PropAttrs()
            {
                DisplayName = "";
                Category = "";
                Description = "";
            }

            /// <summary>
            /// Получить или установить отображаемое имя
            /// </summary>
            public string DisplayName { get; set; }
            /// <summary>
            /// Получить или установить категорию
            /// </summary>
            public string Category { get; set; }
            /// <summary>
            /// Получить или установить описание
            /// </summary>
            public string Description { get; set; }
        }


        /// <summary>
        /// Получить словарь атрибутов свойств
        /// </summary>
        private static Dictionary<string, PropAttrs> GetPropAttrsDict(LocaleDict dict)
        {
            Dictionary<string, PropAttrs> propAttrsDict = new();

            foreach (string phraseKey in dict.Phrases.Keys)
            {
                string phraseVal = dict.Phrases[phraseKey];
                int dotPos = phraseKey.IndexOf('.');

                if (0 < dotPos && dotPos < phraseKey.Length - 1)
                {
                    // если точка в середине ключа фразы, то слева от точки - имя свойства, справа - имя атрибута
                    string propName = phraseKey[..dotPos];
                    string attrName = phraseKey[(dotPos + 1)..];
                    bool attrAssigned = true;

                    if (!propAttrsDict.TryGetValue(propName, out PropAttrs propAttrs))
                        propAttrs = new PropAttrs();

                    if (attrName == "DisplayName")
                    {
                        propAttrs.DisplayName = phraseVal;
                    }
                    else if (attrName == "Category")
                    {
                        propAttrs.Category = phraseVal;
                    }
                    else if (attrName == "Description")
                    {
                        propAttrs.Description = phraseVal;
                    }
                    else
                    {
                        attrAssigned = false;
                    }

                    if (attrAssigned)
                        propAttrsDict[propName] = propAttrs;
                }
            }

            return propAttrsDict;
        }

        /// <summary>
        /// Перевести атрибуты заданного типа
        /// </summary>
        /// <remarks>Используется словарь с ключём, совпадающим с полным именем типа</remarks>
        public static void TranslateAttrs(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            /*string dictKey = type.FullName;

            if (Locale.Dictionaries.TryGetValue(dictKey, out LocaleDict dict))
            {
                Dictionary<string, PropAttrs> propAttrsDict = GetPropAttrsDict(dict);
                PropertyDescriptorCollection allProps = TypeDescriptor.GetProperties(type);

                foreach (PropertyDescriptor prop in allProps)
                {
                    if (propAttrsDict.TryGetValue(prop.Name, out PropAttrs propAttrs))
                    {
                        AttributeCollection attrs = prop.Attributes;

                        foreach (Attribute attr in attrs)
                        {
                            if (attr is DisplayNameAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.DisplayName))
                                    ((DisplayNameAttribute)attr).DisplayNameValue = propAttrs.DisplayName;
                            }
                            else if (attr is CategoryAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.Category))
                                    ((CategoryAttribute)attr).CategoryName = propAttrs.Category;
                            }
                            else if (attr is DescriptionAttribute)
                            {
                                if (!string.IsNullOrEmpty(propAttrs.Description))
                                    ((DescriptionAttribute)attr).DescriptionValue = propAttrs.Description;
                            }
                        }
                    }
                }
            }*/
        }
    }
}
