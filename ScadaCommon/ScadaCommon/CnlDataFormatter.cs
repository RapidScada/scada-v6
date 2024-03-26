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
 * Summary  : Formats channel and event data for display
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2024
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
        /// Provides information about formatting result.
        /// </summary>
        public class ResultInfo
        {
            /// <summary>
            /// Gets a value indicating whether the result is a float number.
            /// </summary>
            public bool IsFloat { get; set; }
            /// <summary>
            /// Gets a value indicating whether the result is a hexadecimal number.
            /// </summary>
            public bool IsHex { get; set; }
            /// <summary>
            /// Resets the information.
            /// </summary>
            internal void Reset()
            {
                IsFloat = false;
                IsHex = false;
            }
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
        /// The command data display length in bytes.
        /// </summary>
        public const int DataDisplayLength = 8;

        /// <summary>
        /// The configuration database.
        /// </summary>
        private readonly ConfigDataset configDataset;
        /// <summary>
        /// The user's time zone.
        /// </summary>
        private readonly TimeZoneInfo timeZone;
        /// <summary>
        /// The culture for formatting values.
        /// </summary>
        private CultureInfo culture;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(ConfigDataset configDataset)
            : this(configDataset, TimeZoneInfo.Local)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlDataFormatter(ConfigDataset configDataset, TimeZoneInfo timeZone)
        {
            this.configDataset = configDataset ?? throw new ArgumentNullException(nameof(configDataset));
            this.timeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));

            Culture = Locale.Culture;
            LastResultInfo = new ResultInfo();
        }


        /// <summary>
        /// Gets or sets the culture for formatting values.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return culture;
            }
            set
            {
                culture = value ?? Locale.Culture;
            }
        }

        /// <summary>
        /// Gets the information about the last formatting.
        /// </summary>
        public ResultInfo LastResultInfo { get; }


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
        private string FormatByDataType(double cnlVal, int dataTypeID)
        {
            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    LastResultInfo.IsFloat = true;
                    return cnlVal.ToString(DefaultFormat, culture);

                case DataTypeID.Int64:
                    LastResultInfo.IsFloat = true;
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
        private string FormatNumber(double cnlVal, int dataTypeID, string format)
        {
            switch (dataTypeID)
            {
                case DataTypeID.Double:
                    if (FormatIsHex(format))
                    {
                        LastResultInfo.IsHex = true;
                        return ((int)cnlVal).ToString(format, culture) + 'h';
                    }
                    else
                    {
                        LastResultInfo.IsFloat = true;
                        return cnlVal.ToString(format, culture);
                    }

                case DataTypeID.Int64:
                    string s = CnlDataConverter.DoubleToInt64(cnlVal).ToString(format, culture);

                    if (FormatIsHex(format))
                    {
                        LastResultInfo.IsHex = true;
                        return s + 'h';
                    }
                    else
                    {
                        LastResultInfo.IsFloat = true;
                        return s;
                    }

                default:
                    return FormatError;
            }
        }

        /// <summary>
        /// Formats the channel value, which is an enumeration.
        /// </summary>
        private string FormatEnum(double cnlVal, int dataTypeID, EnumFormat format)
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
        private string FormatDate(double cnlVal, int dataTypeID, string format)
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
        /// Gets the display name of the user, even if the user is not found in the database.
        /// </summary>
        private string GetUserDisplayName(int userID)
        {
            User user = configDataset.UserTable.GetItem(userID);
            return user == null || string.IsNullOrEmpty(user.Name)
                ? string.Format(CommonPhrases.UnknownUser, userID)
                : user.Name;
        }


        /// <summary>
        /// Formats the channel data according to the specified data type and format.
        /// </summary>
        public CnlDataFormatted FormatCnlData(CnlData cnlData, int dataTypeID, int formatID, int unitID)
        {
            LastResultInfo.Reset();
            CnlDataFormatted cnlDataFormatted = new CnlDataFormatted();
            Format format = formatID > 0 ? configDataset.FormatTable.GetItem(formatID) : null;
            Unit unit = unitID > 0 ? configDataset.UnitTable.GetItem(unitID) : null;
            EnumFormat enumFormat = null;

            if (format != null && format.IsEnum)
                configDataset.Enums.TryGetValue(format.FormatID, out enumFormat);

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
                Time = TimeZoneInfo.ConvertTimeFromUtc(ev.Timestamp, timeZone).ToLocalizedString(culture)
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
                // Command: Value, Data. Custom text
                sbDescr.Append(CommonPhrases.CommandDescrPrefix);
                bool showValue = ev.CnlStat > CnlStatusID.Undefined;
                dataFormatted = FormatCnlData(new CnlData(ev.CnlVal, CnlStatusID.Defined),
                    DataTypeID.Double, cnl?.OutFormatID ?? cnl?.FormatID ?? 0, cnl?.UnitID ?? 0);

                if (showValue)
                    sbDescr.Append(dataFormatted.DispVal);

                if (ev.Data != null && ev.Data.Length > 0)
                {
                    sbDescr
                        .Append(showValue ? ", " : "")
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
            KnownSeverity knownSeverity = Severity.Closest(ev.Severity);

            if (knownSeverity != KnownSeverity.Undefined)
            {
                switch (knownSeverity)
                {
                    case KnownSeverity.Critical:
                        eventFormatted.Sev = CommonPhrases.CriticalSeverity;
                        break;
                    case KnownSeverity.Major:
                        eventFormatted.Sev = CommonPhrases.MajorSeverity;
                        break;
                    case KnownSeverity.Minor:
                        eventFormatted.Sev = CommonPhrases.MinorSeverity;
                        break;
                    case KnownSeverity.Info:
                        eventFormatted.Sev = CommonPhrases.InfoSeverity;
                        break;
                }

                eventFormatted.Sev += ", " + ev.Severity;
            }

            // acknowledgement
            if (ev.Ack)
            {
                eventFormatted.Ack = GetUserDisplayName(ev.AckUserID) + ", " +
                    TimeZoneInfo.ConvertTimeFromUtc(ev.AckTimestamp, timeZone).ToLocalizedString();
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
