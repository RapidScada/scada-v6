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
 * Summary  : Formats channel and event data for display
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2022
 */

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Lang;
using System;
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
        /// The command data display length in bytes.
        /// </summary>
        public const int DataDisplayLength = 8;

        /// <summary>
        /// The culture for formatting values.
        /// </summary>
        protected readonly CultureInfo culture;
        /// <summary>
        /// The configuration database.
        /// </summary>
        protected readonly ConfigDataset configDataset;
        /// <summary>
        /// The enumeration dictionary.
        /// </summary>
        protected readonly EnumDict enums;
        /// <summary>
        /// The user's time zone.
        /// </summary>
        protected readonly TimeZoneInfo timeZone;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(ConfigDataset configDataset)
            : this(configDataset, new EnumDict(configDataset), TimeZoneInfo.Local)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(ConfigDataset configDataset, EnumDict enums)
            : this(configDataset, enums, TimeZoneInfo.Local)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(ConfigDataset configDataset, EnumDict enums, TimeZoneInfo timeZone)
        {
            culture = Locale.Culture;
            this.configDataset = configDataset ?? throw new ArgumentNullException(nameof(configDataset));
            this.enums = enums ?? throw new ArgumentNullException(nameof(enums));
            this.timeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));
        }


        /// <summary>
        /// Determines whether the specified format represents a hexadecimal number.
        /// </summary>
        private bool FormatIsHex(string format)
        {
            return format != null && format.Length > 0 && (format[0] == 'x' || format[0] == 'X');
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
                    return FormatIsHex(format)
                        ? ((int)cnlVal).ToString(format, culture) + 'h'
                        : cnlVal.ToString(format, culture);

                case DataTypeID.Int64:
                    string s = CnlDataConverter.DoubleToInt64(cnlVal).ToString(format, culture);
                    return FormatIsHex(format) ? s + 'h' : s;

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
            string DateToString(DateTime dt)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dt, timeZone).ToString(format, culture);
            }

            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    return DateToString(DateTime.FromOADate(cnlVal));

                case DataTypeID.Int64:
                    return DateToString(ScadaUtils.TicksToTime(CnlDataConverter.DoubleToInt64(cnlVal)));

                default:
                    return FormatError;
            }
        }


        /// <summary>
        /// Formats the channel data according to the specified data type and format.
        /// </summary>
        public CnlDataFormatted FormatCnlData(CnlData cnlData, int dataTypeID, int formatID, int unitID)
        {
            CnlDataFormatted cnlDataFormatted = new CnlDataFormatted();
            Format format = formatID > 0 ? configDataset.FormatTable.GetItem(formatID) : null;
            Unit unit = unitID > 0 ? configDataset.UnitTable.GetItem(unitID) : null;
            EnumFormat enumFormat = null;

            if (format != null && format.IsEnum)
                enums.TryGetValue(format.FormatID, out enumFormat);

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

                if (!string.IsNullOrEmpty(unit?.Name))
                    cnlDataFormatted.DispVal += " " + unit.Name;
            }
            catch
            {
                cnlDataFormatted.DispVal = FormatError;
            }

            // color
            try
            {
                // color determined by status
                CnlStatus cnlStatus = configDataset.CnlStatusTable.GetItem(cnlData.Stat);
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
        /// Formats the channel data according to the channel properties.
        /// </summary>
        public CnlDataFormatted FormatCnlData(CnlData cnlData, Cnl cnl, bool appendUnit)
        {
            return FormatCnlData(cnlData, 
                cnl?.DataTypeID ?? DataTypeID.Double, 
                cnl?.FormatID ?? 0, 
                appendUnit ? cnl?.UnitID ?? 0 : 0);
        }

        /// <summary>
        /// Formats the channel data according to the channel properties.
        /// </summary>
        public CnlDataFormatted FormatCnlData(CnlData cnlData, int cnlNum, bool appendUnit)
        {
            return FormatCnlData(cnlData, cnlNum > 0 ? configDataset.CnlTable.GetItem(cnlNum) : null, appendUnit);
        }

        /// <summary>
        /// Formats the event according to the channel properties.
        /// </summary>
        public EventFormatted FormatEvent(Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            EventFormatted eventFormatted = new EventFormatted
            {
                Time = TimeZoneInfo.ConvertTimeFromUtc(ev.Timestamp, timeZone).ToLocalizedString()
            };

            // object
            if (ev.ObjNum > 0)
                eventFormatted.Obj = configDataset.ObjTable.GetItem(ev.ObjNum)?.Name ?? "";

            // device
            if (ev.DeviceNum > 0)
                eventFormatted.Dev = configDataset.DeviceTable.GetItem(ev.DeviceNum)?.Name ?? "";

            // channel
            Cnl cnl = null;

            if (ev.CnlNum > 0)
            {
                cnl = configDataset.CnlTable.GetItem(ev.CnlNum);
                eventFormatted.Cnl = cnl?.Name ?? "";
            }

            // description
            StringBuilder sbDescr = new StringBuilder();
            CnlDataFormatted dataFormatted;

            if (ev.TextFormat == EventTextFormat.Command)
            {
                // Command Value, Data. Custom text
                sbDescr.Append(CommonPhrases.CommandDescrPrefix);
                dataFormatted = FormatCnlData(new CnlData(ev.CnlVal, CnlStatusID.Defined), 
                    DataTypeID.Double, cnl?.FormatID ?? 0, cnl?.UnitID ?? 0);

                if (ev.CnlStat > 0)
                    sbDescr.Append(dataFormatted.DispVal);

                if (ev.Data != null && ev.Data.Length > 0)
                {
                    sbDescr
                        .Append(ev.CnlStat > 0 ? ", " : "")
                        .Append("0x")
                        .Append(ScadaUtils.BytesToHex(ev.Data, 0, Math.Min(DataDisplayLength, ev.Data.Length)))
                        .Append(DataDisplayLength < ev.Data.Length ? "..." : "");
                }
            }
            else
            {
                // Status, Value. Custom text
                dataFormatted = FormatCnlData(new CnlData(ev.CnlVal, ev.CnlStat), cnl, true);

                if (ev.TextFormat == EventTextFormat.Full || ev.TextFormat == EventTextFormat.AutoText)
                {
                    string statusName = configDataset.CnlStatusTable.GetItem(ev.CnlStat)?.Name ??
                        string.Format(CommonPhrases.StatusFormat, ev.CnlStat);
                    sbDescr.Append(statusName).Append(", ").Append(dataFormatted.DispVal);
                }
            }

            if (!string.IsNullOrEmpty(ev.Text) && ev.TextFormat != EventTextFormat.AutoText)
            {
                if (sbDescr.Length > 0)
                    sbDescr.Append(". ");

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
                    configDataset.UserTable.GetItem(ev.AckUserID)?.Name ?? "",
                    TimeZoneInfo.ConvertTimeFromUtc(ev.AckTimestamp, timeZone));
            }

            // color
            if (dataFormatted.Colors.Length > 0)
                eventFormatted.Color = dataFormatted.Colors[0];

            // beep
            if (cnl != null && new EventMask(cnl.EventMask).Beep)
                eventFormatted.Beep = true;

            return eventFormatted;
        }
    }
}
