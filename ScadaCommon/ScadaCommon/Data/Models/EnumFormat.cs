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
 * Module   : ScadaCommon
 * Summary  : Represents a enumeration retrieved from the Format table
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Data.Entities;
using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a enumeration retrieved from the Format table.
    /// <para>Представляет перечисление, полученное из таблицы Форматы.</para>
    /// </summary>
    public class EnumFormat
    {
        /// <summary>
        /// The default color for the Off enumeration value.
        /// </summary>
        private const string EnumOffColor = "Red";
        /// <summary>
        /// The default color for the On enumeration value.
        /// </summary>
        private const string EnumOnColor = "Green";
        /// <summary>
        /// Separates enumeration values.
        /// </summary>
        private static readonly char[] EnumSeparator = new char[] { ';', '\n' };


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumFormat(int valueCount)
        {
            FormatID = 0;
            Values = new string[valueCount];
            Colors = new string[valueCount];
        }


        /// <summary>
        /// Gets or sets the format ID.
        /// </summary>
        public int FormatID { get; set;  }

        /// <summary>
        /// Gets the display values corresponding to channel values.
        /// </summary>
        public string[] Values { get; }

        /// <summary>
        /// Gets the colors corresponding to channel values.
        /// </summary>
        public string[] Colors { get; }

        /// <summary>
        /// Gets the number of values.
        /// </summary>
        public int Count
        {
            get
            {
                return Values.Length;
            }
        }


        /// <summary>
        /// Converts the specified string to a enumeration.
        /// </summary>
        public static EnumFormat Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new EnumFormat(0);
            }
            else
            {
                string[] parts = s.Split(EnumSeparator);
                int valueCount = parts.Length;
                EnumFormat enumFormat = new EnumFormat(valueCount);

                for (int i = 0; i < valueCount; i++)
                {
                    string part = parts[i];
                    int colonIdx = part.IndexOf(':');
                    string value = colonIdx < 0 ? part.Trim() : part.Substring(0, colonIdx).Trim();
                    string color = colonIdx < 0 ? "" : part.Substring(colonIdx + 1).Trim();
                    enumFormat.Values[i] = value;

                    if (color != "")
                        enumFormat.Colors[i] = color;
                    else if (i == 0 && valueCount <= 2)
                        enumFormat.Colors[i] = EnumOffColor;
                    else if (i == 1 && valueCount <= 2)
                        enumFormat.Colors[i] = EnumOnColor;
                    else
                        enumFormat.Colors[i] = "";
                }

                return enumFormat;
            }
        }

        /// <summary>
        /// Converts the specified format entity to a enumeration.
        /// </summary>
        public static EnumFormat Parse(Format format)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            EnumFormat enumFormat = Parse(format.Frmt);
            enumFormat.FormatID = format.FormatID;
            return enumFormat;
        }
    }
}
