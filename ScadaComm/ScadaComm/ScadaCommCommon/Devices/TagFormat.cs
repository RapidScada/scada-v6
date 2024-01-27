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
 * Module   : ScadaCommCommon
 * Summary  : Represents a display format of a tag
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Lang;
using System;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a display format of a tag.
    /// <para>Представляет формат отображения тега.</para>
    /// </summary>
    public class TagFormat
    {
        /// <summary>
        /// The default floating point format.
        /// </summary>
        public static readonly TagFormat FloatNumber = new TagFormat(TagFormatType.Number, "N3");
        /// <summary>
        /// The default integer format.
        /// </summary>
        public static readonly TagFormat IntNumber = new TagFormat(TagFormatType.Number, "N0");
        /// <summary>
        /// The default hexadecimal format.
        /// </summary>
        public static readonly TagFormat HexNumber = new TagFormat(TagFormatType.Number, "X4");
        /// <summary>
        /// The default date and time format.
        /// </summary>
        public static readonly TagFormat DateTime = new TagFormat(TagFormatType.Date, "G");
        /// <summary>
        /// The default string format.
        /// </summary>
        public static readonly TagFormat String = new TagFormat(TagFormatType.String);
        /// <summary>
        /// The discrete value format.
        /// </summary>
        public static readonly TagFormat OffOn = new TagFormat(TagFormatType.Enum, 
            new string[] { CommPhrases.Off, CommPhrases.On });


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TagFormat(TagFormatType formatType)
        {
            FormatType = formatType;
            Format = "";
            EnumValues = null;
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TagFormat(TagFormatType formatType, string format)
        {
            FormatType = formatType;
            Format = format ?? throw new ArgumentNullException(nameof(format));
            EnumValues = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TagFormat(TagFormatType formatType, string[] enumValues)
        {
            FormatType = formatType;
            Format = "";
            EnumValues = enumValues ?? throw new ArgumentNullException(nameof(enumValues));
        }


        /// <summary>
        /// Gets the format type.
        /// </summary>
        public TagFormatType FormatType { get; }

        /// <summary>
        /// Gets the format string.
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// Gets the enumeration values.
        /// </summary>
        public string[] EnumValues { get; }


        /// <summary>
        /// Gets the default format corresponding to the tag data type.
        /// </summary>
        public static TagFormat GetDefault(TagDataType dataType)
        {
            switch (dataType)
            {
                case TagDataType.Int64:
                    return IntNumber;
                case TagDataType.ASCII:
                case TagDataType.Unicode:
                    return String;
                default:
                    return FloatNumber;
            }
        }
    }
}
