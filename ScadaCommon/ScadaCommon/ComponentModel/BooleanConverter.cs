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
 * Summary  : Converts boolean values to and from localized representations
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

namespace Scada.ComponentModel
{
    /// <summary>
    /// Converts boolean values to and from localized representations.
    /// <para>Преобразует логические значения в локализованные представления и обратно.</para>
    /// </summary>
    public class BooleanConverter : System.ComponentModel.BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return (bool)value ? CommonPhrases.TrueValue : CommonPhrases.FalseValue;
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value is string strVal ?
                string.Equals(strVal, CommonPhrases.TrueValue, StringComparison.OrdinalIgnoreCase) :
                base.ConvertFrom(context, culture, value);
        }
    }
}
