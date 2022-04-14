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
 * Module   : ScadaSchemeCommon
 * Summary  : Converts enumeration objects to and from various other representations
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2022
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using Scada.Lang;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Scada.ComponentModel
{
    /// <summary>
    /// Converts enumeration objects to and from various other representations.
    /// <para>Преобразует объекты перечислений в различные представления и обратно.</para>
    /// </summary>
    public class EnumConverter : System.ComponentModel.EnumConverter
    {
        private readonly Type enumType;
        private readonly LocaleDict enumDict;

        public EnumConverter(Type type)
            : base(type)
        {
            enumType = type;
            Locale.Dictionaries.TryGetValue(enumType.FullName, out enumDict);
        }

        private string TranslateEnumField(string fieldName)
        {
            if (enumDict == null)
            {
                // get value from attribute
                FieldInfo fi = enumType.GetField(fieldName);
                DescriptionAttribute da =
                    (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                return da == null ? fieldName : da.Description;
            }
            else
            {
                // get value from dictionary
                return enumDict[fieldName];
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, 
            Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                string fieldName = Enum.GetName(enumType, value);
                return TranslateEnumField(fieldName);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string strVal)
            {
                foreach (string fieldName in Enum.GetNames(enumType))
                {
                    if (TranslateEnumField(fieldName) == strVal)
                        return Enum.Parse(enumType, fieldName);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
