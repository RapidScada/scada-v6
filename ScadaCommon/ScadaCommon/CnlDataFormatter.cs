/*
 * Copyright 2021 Rapid Software LLC
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
 * Summary  : Formats channel data for display
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2021
 */

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using System;
using System.Collections.Generic;

namespace Scada
{
    /// <summary>
    /// Formats channel data for display.
    /// <para>Форматирует данные канала для отображения.</para>
    /// </summary>
    public class CnlDataFormatter
    {
        /// <summary>
        /// Represents parsed properties of a enumeration format.
        /// </summary>
        protected class EnumFormat
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public EnumFormat(int formatID, int valueCount)
            {
                FormatID = formatID;
                Values = new string[valueCount];
                Colors = new string[valueCount];
            }

            /// <summary>
            /// Gets the format ID.
            /// </summary>
            public int FormatID { get; }

            /// <summary>
            /// Gets the display values corresponding to channel values.
            /// </summary>
            public string[] Values { get; }

            /// <summary>
            /// Gets the colors corresponding to channel values.
            /// </summary>
            public string[] Colors { get; }
        }

        /// <summary>
        /// The default number format.
        /// </summary>
        public const string DefaultFormat = "N3";
        /// <summary>
        /// The formatting result indicating an error.
        /// </summary>
        public const string FormatError = "!!!";
        /// <summary>
        /// The default color for the Off enumeration value.
        /// </summary>
        public const string EnumOffColor = "Red";
        /// <summary>
        /// The default color for the On enumeration value.
        /// </summary>
        public const string EnumOnColor = "Green";
        /// <summary>
        /// Separates enumeration values.
        /// </summary>
        private static readonly char[] EnumSeparator = new char[] { ';', '\n' };


        /// <summary>
        /// The channel status table.
        /// </summary>
        protected readonly BaseTable<CnlStatus> cnlStatusTable;
        /// <summary>
        /// The format table.
        /// </summary>
        protected readonly BaseTable<Format> formatTable;
        /// <summary>
        /// The dictionary containing the enumeration colors.
        /// </summary>
        protected readonly Dictionary<int, EnumFormat> enumFormats;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(BaseDataSet baseDataSet)
        {
            if (baseDataSet == null)
                throw new ArgumentNullException(nameof(baseDataSet));

            cnlStatusTable = baseDataSet.CnlStatusTable;
            formatTable = baseDataSet.FormatTable;
            enumFormats = new Dictionary<int, EnumFormat>();
            FillEnumColors();
        }


        /// <summary>
        /// Fills the enumeration color dictionary.
        /// </summary>
        protected void FillEnumColors()
        {
            foreach (Format format in formatTable.EnumerateItems())
            {
                if (format.IsEnum && !string.IsNullOrEmpty(format.Frmt))
                {
                    string[] parts = format.Frmt.Split(EnumSeparator);
                    int valueCount = parts.Length;
                    EnumFormat enumFormat = new EnumFormat(format.FormatID, valueCount);
                    enumFormats.Add(format.FormatID, enumFormat);

                    for (int i = 0; i < valueCount; i++)
                    {
                        string part = parts[i];
                        int colonIdx = part.IndexOf(':');
                        string value = part.Substring(0, colonIdx - 1).Trim();
                        string color = colonIdx < 0 ? "" : part.Substring(colonIdx + 1).Trim();
                        enumFormat.Values[i] = value;

                        if (color != "")
                            enumFormat.Colors[i] = color;
                        else if (i == 0)
                            enumFormat.Colors[i] = EnumOffColor;
                        else if (i == 1)
                            enumFormat.Colors[i] = EnumOnColor;
                        else
                            enumFormat.Colors[i] = "";
                    }
                }
            }
        }

        /// <summary>
        /// Formats the channel value depending on the data type.
        /// </summary>
        protected string FormatByDataType(double cnlVal, int dataTypeID)
        {
            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    return cnlVal.ToString(DefaultFormat);

                case DataTypeID.Int64:
                    return CnlDataConverter.DoubleToInt64(cnlVal).ToString();

                case DataTypeID.ASCII:
                    return CnlDataConverter.DoubleToAscii(cnlVal).ToString();

                case DataTypeID.Unicode:
                    return CnlDataConverter.DoubleToUnicode(cnlVal).ToString();

                default:
                    return FormatError;
            }
        }

        /// <summary>
        /// Formats the channel value, which is a number.
        /// </summary>
        protected string FormatNumber(double cnlVal, int dataTypeID, string format)
        {
            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    return cnlVal.ToString(format);

                case DataTypeID.Int64:
                    return CnlDataConverter.DoubleToInt64(cnlVal).ToString(format);

                default:
                    return FormatError;
            }
        }

        /// <summary>
        /// Formats the channel value, which is an enumeration.
        /// </summary>
        protected string FormatEnum(double cnlVal, int dataTypeID, EnumFormat format)
        {
            string GetEnumValue(int intVal)
            {
                return format != null && 0 <= intVal && intVal < format.Values.Length
                    ? format.Values[intVal]
                    : intVal.ToString();
            }

            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    return GetEnumValue((int)cnlVal);

                case DataTypeID.Int64:
                    return GetEnumValue((int)CnlDataConverter.DoubleToInt64(cnlVal));

                default:
                    return FormatError;
            }
        }

        /// <summary>
        /// Formats the channel value, which is a date and time.
        /// </summary>
        protected string FormatDate(double cnlVal, int dataTypeID, string format)
        {
            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    return DateTime.FromOADate(cnlVal).ToString(format);

                case DataTypeID.Int64:
                    return ScadaUtils.TicksToTime(CnlDataConverter.DoubleToInt64(cnlVal)).ToString(format);

                default:
                    return FormatError;
            }
        }


        /// <summary>
        /// Formats the input channel data according to the channel properties.
        /// </summary>
        public CnlDataFormatted FormatCnlData(CnlData cnlData, InCnl inCnl)
        {
            CnlDataFormatted cnlDataFormatted = new CnlDataFormatted();
            int dataTypeID = inCnl?.DataTypeID ?? DataTypeID.Double;
            Format format = inCnl?.FormatID == null ? null : formatTable.GetItem(inCnl.FormatID.Value);            
            EnumFormat enumFormat = null;

            if (format != null && format.IsEnum)
                enumFormats.TryGetValue(format.FormatID, out enumFormat);

            // displayed value
            try
            {
                if (cnlData.IsUndefined)
                    cnlDataFormatted.DispVal = CommonPhrases.UndefinedSign;
                else if (format == null)
                    cnlDataFormatted.DispVal = FormatByDataType(cnlData.Val, dataTypeID);
                else if (format.IsNumber)
                    cnlDataFormatted.DispVal = FormatNumber(cnlData.Val, dataTypeID, format.Frmt);
                else if (format.IsEnum)
                    cnlDataFormatted.DispVal = FormatEnum(cnlData.Val, dataTypeID, enumFormat);
                else if (format.IsDate)
                    cnlDataFormatted.DispVal = FormatDate(cnlData.Val, dataTypeID, format.Frmt);
                else // format.IsString or not specified
                    cnlDataFormatted.DispVal = FormatByDataType(cnlData.Val, dataTypeID);
            }
            catch
            {
                cnlDataFormatted.DispVal = FormatError;
            }

            // color
            try
            {
                // color determined by status
                CnlStatus cnlStatus = cnlStatusTable.GetItem(cnlData.Stat);
                cnlDataFormatted.SetColors(cnlStatus);

                // color determined by value
                if (enumFormat != null && cnlData.Stat == CnlStatusID.Defined &&
                    0 <= cnlData.Val && cnlData.Val < enumFormat.Colors.Length &&
                    enumFormat.Colors[(int)cnlData.Val] is string color && color != "")
                {
                    cnlDataFormatted.Color1 = color;
                }
            }
            catch
            {
                cnlDataFormatted.SetColorsToDefault();
            }

            return cnlDataFormatted;
        }
    }
}
