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
 * Summary  : Formats channel and event data for display
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
using System.Globalization;
using System.Text;

namespace Scada
{
    /// <summary>
    /// Formats channel and event data for display.
    /// <para>Форматирует данные канала и события для отображения.</para>
    /// </summary>
    public class CnlDataFormatter
    {
        /// <summary>
        /// The default number format.
        /// </summary>
        public const string DefaultFormat = "N3";
        /// <summary>
        /// The formatting result indicating an error.
        /// </summary>
        public const string FormatError = "!!!";

        /// <summary>
        /// The culture for formatting values.
        /// </summary>
        protected readonly CultureInfo culture;
        /// <summary>
        /// The configuration database.
        /// </summary>
        protected readonly BaseDataSet baseDataSet;
        /// <summary>
        /// The dictionary containing the enumeration colors.
        /// </summary>
        protected readonly Dictionary<int, EnumFormat> enumFormats;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(BaseDataSet baseDataSet)
        {
            culture = Locale.Culture;
            this.baseDataSet = baseDataSet ?? throw new ArgumentNullException(nameof(baseDataSet));
            enumFormats = new Dictionary<int, EnumFormat>();
            FillEnumFormats();
        }


        /// <summary>
        /// Fills the enumeration format dictionary.
        /// </summary>
        protected void FillEnumFormats()
        {
            foreach (Format format in baseDataSet.FormatTable.EnumerateItems())
            {
                if (format.IsEnum && !string.IsNullOrEmpty(format.Frmt))
                {
                    EnumFormat enumFormat = EnumFormat.Parse(format.Frmt);
                    enumFormat.FormatID = format.FormatID;
                    enumFormats.Add(format.FormatID, enumFormat);
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
                    return cnlVal.ToString(DefaultFormat, culture);

                case DataTypeID.Int64:
                    return CnlDataConverter.DoubleToInt64(cnlVal).ToString(culture);

                case DataTypeID.ASCII:
                    return CnlDataConverter.DoubleToAscii(cnlVal);

                case DataTypeID.Unicode:
                    return CnlDataConverter.DoubleToUnicode(cnlVal);

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
                    return cnlVal.ToString(format, culture);

                case DataTypeID.Int64:
                    return CnlDataConverter.DoubleToInt64(cnlVal).ToString(format, culture);

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
                    return DateTime.FromOADate(cnlVal).ToString(format, culture);

                case DataTypeID.Int64:
                    return ScadaUtils.TicksToTime(CnlDataConverter.DoubleToInt64(cnlVal)).ToString(format, culture);

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
            Format format = inCnl?.FormatID == null ? null : baseDataSet.FormatTable.GetItem(inCnl.FormatID.Value);
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
                CnlStatus cnlStatus = baseDataSet.CnlStatusTable.GetItem(cnlData.Stat);
                cnlDataFormatted.SetColors(cnlStatus);

                // color determined by value
                if (enumFormat != null && cnlData.Stat == CnlStatusID.Defined &&
                    0 <= cnlData.Val && cnlData.Val < enumFormat.Colors.Length &&
                    enumFormat.Colors[(int)cnlData.Val] is string color && color != "")
                {
                    cnlDataFormatted.SetFirstColor(color);
                }
            }
            catch
            {
                cnlDataFormatted.SetColorsToDefault();
            }

            return cnlDataFormatted;
        }

        /// <summary>
        /// Formats the input channel data according to the channel properties.
        /// </summary>
        public CnlDataFormatted FormatCnlData(CnlData cnlData, int cnlNum)
        {
            return FormatCnlData(cnlData, cnlNum > 0 ? baseDataSet.InCnlTable.GetItem(cnlNum) : null);
        }

        /// <summary>
        /// Formats the event according to the channel properties.
        /// </summary>
        public EventFormatted FormatEvent(Event ev, TimeZoneInfo timeZone)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));
            if (timeZone == null)
                throw new ArgumentNullException(nameof(timeZone));

            EventFormatted eventFormatted = new EventFormatted
            {
                Time = TimeZoneInfo.ConvertTimeFromUtc(ev.Timestamp, timeZone).ToLocalizedString()
            };

            // object
            if (ev.ObjNum > 0)
                eventFormatted.Obj = baseDataSet.ObjTable.GetItem(ev.ObjNum)?.Name ?? "";

            // device
            if (ev.DeviceNum > 0)
                eventFormatted.Dev = baseDataSet.DeviceTable.GetItem(ev.DeviceNum)?.Name ?? "";

            // channel
            InCnl inCnl = ev.CnlNum > 0 ? baseDataSet.InCnlTable.GetItem(ev.CnlNum) : null;

            if (ev.CnlNum > 0)
                eventFormatted.Cnl = inCnl?.Name ?? "";
            else if (ev.OutCnlNum > 0)
                eventFormatted.Cnl = baseDataSet.OutCnlTable.GetItem(ev.OutCnlNum)?.Name ?? "";

            // description in the form:
            // Status, Value. Custom text
            CnlDataFormatted dataFormatted = FormatCnlData(new CnlData(ev.CnlVal, ev.CnlStat), inCnl);
            StringBuilder sbDescr = new StringBuilder();

            if (ev.TextFormat == EventTextFormat.Full || ev.TextFormat == EventTextFormat.AutoText)
            {
                string statusName = baseDataSet.CnlStatusTable.GetItem(ev.CnlStat)?.Name;

                if (string.IsNullOrEmpty(statusName))
                    statusName = (Locale.IsRussian ? "Статус " : "Status ") + ev.CnlStat;

                sbDescr.Append(statusName).Append(", ").Append(dataFormatted.DispVal);
            }

            if (!string.IsNullOrEmpty(ev.Text))
            {
                if (ev.TextFormat == EventTextFormat.Full)
                    sbDescr.Append(". ");

                if (ev.TextFormat == EventTextFormat.Full || ev.TextFormat == EventTextFormat.CustomText)
                    sbDescr.Append(ev.Text);
            }

            eventFormatted.Descr = sbDescr.ToString();

            // severity
            int knownSeverity = Severity.Closest(ev.Severity);

            if (knownSeverity != Severity.Undefined)
            {
                switch (knownSeverity)
                {
                    case Severity.Critical:
                        eventFormatted.Sev = CommonPhrases.CriticalSeverity;
                        break;
                    case Severity.Major:
                        eventFormatted.Sev = CommonPhrases.MajorSeverity;
                        break;
                    case Severity.Minor:
                        eventFormatted.Sev = CommonPhrases.MinorSeverity;
                        break;
                    case Severity.Info:
                        eventFormatted.Sev = CommonPhrases.InfoSeverity;
                        break;
                }

                eventFormatted.Sev += ", " + ev.Severity;
            }

            // acknowledgement
            if (ev.Ack)
            {
                eventFormatted.Ack = string.Join(", ",
                    baseDataSet.UserTable.GetItem(ev.AckUserID)?.Name ?? "",
                    TimeZoneInfo.ConvertTimeFromUtc(ev.AckTimestamp, timeZone));
            }

            // color
            if (dataFormatted.Colors.Length > 0)
                eventFormatted.Color = dataFormatted.Colors[0];

            // beep
            if (inCnl != null && new EventMask(inCnl.EventMask).Beep)
                eventFormatted.Beep = true;

            return eventFormatted;
        }
    }
}
