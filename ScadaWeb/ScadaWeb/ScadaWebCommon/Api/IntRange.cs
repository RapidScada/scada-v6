/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaWebCommon
 * Summary  : Represents a list of ordered and unique integers
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace Scada.Web.Api
{
    /// <summary>
    /// Represents a list of ordered and unique integers.
    /// <para>Представляет список упорядоченных и уникальных целых чисел.</para>
    /// </summary>
    [TypeConverter(typeof(IntRangeConverter))]
    public class IntRange : Collection<int>
    {
        /// <summary>
        /// Implements a converter for the IntRange type.
        /// </summary>
        public class IntRangeConverter : TypeConverter
        {
            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter.
            /// </summary>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            /// Converts the given value to the type of this converter.
            /// </summary>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return new IntRange(ScadaUtils.ParseRange(value == null ? "" : value.ToString(), true, true));
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private IntRange(IList<int> list)
            : base(list)
        {
        }
    }
}
