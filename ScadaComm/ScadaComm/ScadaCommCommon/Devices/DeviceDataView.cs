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
 * Summary  : Represents device data for display
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents device data for display.
    /// <para>Представляет данные устройства для отображения.</para>
    /// </summary>
    internal class DeviceDataView
    {
        /// <summary>
        /// Represents a displayed row.
        /// </summary>
        private class Row
        {
            public Row(int cellCount)
            {
                IsEmpty = false;
                IsSubheader = false;
                Cells = new string[cellCount];
            }
            public bool IsEmpty { get; set; }
            public bool IsSubheader { get; set; }
            public string[] Cells { get; }
        }

        /// <summary>
        /// Represents a displayed column.
        /// </summary>
        private class Column
        {
            public Column()
            {
                AlignLeft = true;
                VarWidth = true;
                Width = -1;
            }
            public bool AlignLeft { get; set; }
            public bool VarWidth { get; set; }
            public int Width { get; set; }
        }

        /// <summary>
        /// Represents a displayed row.
        /// </summary>
        private class Table
        {
            public Table(int columnCount, int rowCount)
            {
                Columns = new Column[columnCount];
                Rows = new Row[rowCount];

                for (int i = 0; i < columnCount; i++)
                {
                    Columns[i] = new Column();
                }

                for (int i = 0; i < rowCount; i++)
                {
                    Rows[i] = new Row(columnCount);
                }
            }
            public Column[] Columns { get; }
            public Row[] Rows { get; }
            public void ResetColumnWidths()
            {
                foreach (Column column in Columns)
                {
                    column.Width = -1;
                }
            }
        }

        /// <summary>
        /// The number of displayed items.
        /// </summary>
        private const int ItemCount = 10;

        private readonly Queue<DeviceSlice> slices;   // the displayed historical data
        private readonly Queue<DeviceEvent> events;   // the displayed events
        private readonly Queue<TeleCommand> commands; // the displayed commands
        private readonly Table sliceTable;            // the historical data prepared for display
        private readonly Table eventTable;            // the events prepared for display
        private readonly Table commandTable;          // the commands prepared for display

        private Table curDataTable;      // the current data prepared for display
        private int[] curDataRowIndexes; // the map between tag indexes and row indexes


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceDataView()
        {
            slices = new Queue<DeviceSlice>(ItemCount);
            events = new Queue<DeviceEvent>(ItemCount);
            commands = new Queue<TeleCommand>(ItemCount);

            int rowCnt = ItemCount + 1;
            sliceTable = new Table(2, rowCnt);
            eventTable = new Table(3, rowCnt);
            commandTable = new Table(2, rowCnt);
            InitTableHeaders();

            curDataTable = null;
            curDataRowIndexes = null;
        }


        /// <summary>
        /// Initializes the table headers, excluding the current data table.
        /// </summary>
        private void InitTableHeaders()
        {
            if (Locale.IsRussian)
            {
                Row headerRow = sliceTable.Rows[0];
                headerRow.Cells[0] = "Время";
                headerRow.Cells[1] = "Описание";

                headerRow = eventTable.Rows[0];
                headerRow.Cells[0] = "Время";
                headerRow.Cells[1] = "Тег";
                headerRow.Cells[2] = "Описание";

                headerRow = commandTable.Rows[0];
                headerRow.Cells[0] = "Время";
                headerRow.Cells[1] = "Описание";
            }
            else
            {
                Row headerRow = sliceTable.Rows[0];
                headerRow.Cells[0] = "Timestamp";
                headerRow.Cells[1] = "Description";

                headerRow = eventTable.Rows[0];
                headerRow.Cells[0] = "Timestamp";
                headerRow.Cells[1] = "Tag";
                headerRow.Cells[2] = "Description";

                headerRow = commandTable.Rows[0];
                headerRow.Cells[0] = "Timestamp";
                headerRow.Cells[1] = "Description";
            }
        }

        /// <summary>
        /// Gets the command description.
        /// </summary>
        private string GetCommandDescr(TeleCommand cmd)
        {
            StringBuilder sb = new StringBuilder();

            void AppendComma()
            {
                if (sb.Length > 0)
                    sb.Append(", ");
            }

            if (cmd.CmdNum > 0)
                sb.Append("Num=").Append(cmd.CmdNum);

            if (!string.IsNullOrEmpty(cmd.CmdCode))
            {
                AppendComma();
                sb.Append("Code=").Append(cmd.CmdCode);
            }

            if (!double.IsNaN(cmd.CmdVal))
            {
                AppendComma();
                sb.Append("Val=").Append(cmd.CmdVal.ToString(CnlDataFormatter.DefaultFormat, Locale.Culture));
            }
            
            if (cmd.CmdData != null && cmd.CmdData.Length > 0)
            {
                AppendComma();
                sb.Append("Data=").Append(ScadaUtils.BytesToHex(cmd.CmdData, 0, 
                    Math.Min(CnlDataFormatter.DataDisplayLength, cmd.CmdData.Length)));

                if (CnlDataFormatter.DataDisplayLength < cmd.CmdData.Length)
                    sb.Append("...");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Appends the table to the string builder.
        /// </summary>
        private void AppendTable(StringBuilder sb, Table table)
        {
            // calculate column widths
            int totalWidth = 1;

            for (int colIdx = 0, lastIdx = table.Columns.Length - 1; colIdx <= lastIdx; colIdx++)
            {
                CalculateColumnWidth(table, colIdx);
                totalWidth += table.Columns[colIdx].Width + 3;
            }

            // build break line
            StringBuilder sbBreakLine = new StringBuilder("+-");

            for (int colIdx = 0, lastIdx = table.Columns.Length - 1; colIdx <= lastIdx; colIdx++)
            {
                sbBreakLine
                    .Append('-', table.Columns[colIdx].Width)
                    .Append(colIdx < lastIdx ? "-+-" : "-+");
            }

            string breakLine = sbBreakLine.ToString();

            // build table
            sb.AppendLine(breakLine);

            for (int rowIdx = 0, rowCnt = table.Rows.Length; rowIdx < rowCnt; rowIdx++)
            {
                Row row = table.Rows[rowIdx];

                if (row.IsEmpty)
                    continue;

                sb.Append("| ");

                if (row.IsSubheader)
                {
                    string cellText = row.Cells[0] ?? "";
                    int asteriskCnt = totalWidth - cellText.Length - 6;

                    if (asteriskCnt > 0)
                    {
                        int prefixLength = asteriskCnt / 2;
                        int suffixLength = asteriskCnt - prefixLength;
                        sb.Append('*', prefixLength).Append(' ');
                        sb.Append(cellText);
                        sb.Append(' ').Append('*', suffixLength);
                    }
                    else if (asteriskCnt == 0)
                    {
                        sb.Append(cellText);
                    }
                    else
                    {
                        sb.Append(cellText.Substring(0, totalWidth - 4));
                    }
                }
                else
                {
                    for (int colIdx = 0, lastIdx = table.Columns.Length - 1; colIdx <= lastIdx; colIdx++)
                    {
                        Column column = table.Columns[colIdx];
                        string cellText = row.Cells[colIdx] ?? "";

                        if (cellText.Length < column.Width)
                            sb.Append(column.AlignLeft ? cellText.PadRight(column.Width) : cellText.PadLeft(column.Width));
                        else
                            sb.Append(cellText);

                        if (colIdx < lastIdx)
                            sb.Append(" | ");
                    }
                }

                sb.AppendLine(" |").AppendLine(breakLine);
            }
        }

        /// <summary>
        /// Calculates the column width.
        /// </summary>
        private void CalculateColumnWidth(Table table, int colIndex)
        {
            Column column = table.Columns[colIndex];

            if (column.Width < 0 || column.VarWidth)
            {
                int maxWidth = 0;

                for (int rowIdx = 0, rowCnt = table.Rows.Length; rowIdx < rowCnt; rowIdx++)
                {
                    Row row = table.Rows[rowIdx];
                    if (!row.IsEmpty && !row.IsSubheader)
                    {
                        int cellWidth = (row.Cells[colIndex] ?? "").Length;

                        if (maxWidth < cellWidth)
                            maxWidth = cellWidth;
                    }
                }

                column.Width = maxWidth;
            }
        }

        /// <summary>
        /// Updates the column cells by padding them with spaces for a specified total length.
        /// </summary>
        /// <remarks>To improve performance.</remarks>
        private void PadColumn(Table table, int colIndex)
        {
            CalculateColumnWidth(table, colIndex);
            Column column = table.Columns[colIndex];
            bool alignLeft = column.AlignLeft;
            int columnWidth = column.Width;

            for (int rowIdx = 0, rowCnt = table.Rows.Length; rowIdx < rowCnt; rowIdx++)
            {
                Row row = table.Rows[rowIdx];

                if (!row.IsEmpty && !row.IsSubheader)
                {
                    string cellText = row.Cells[colIndex] ?? "";

                    if (cellText.Length < columnWidth)
                        row.Cells[colIndex] = alignLeft ? cellText.PadRight(columnWidth) : cellText.PadLeft(columnWidth);
                }
            }
        }


        /// <summary>
        /// Prepares the current data for display.
        /// </summary>
        public void PrepareCurData(DeviceTags deviceTags)
        {
            if (deviceTags == null)
                throw new ArgumentNullException(nameof(deviceTags));

            // calculate row count
            int rowCount = 1;

            foreach (TagGroup tagGroup in deviceTags.TagGroups)
            {
                if (!tagGroup.Hidden)
                {
                    if (!deviceTags.FlattenGroups)
                        rowCount++;

                    foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
                    {
                        rowCount++;

                        if (deviceTag.IsNumericArray)
                            rowCount += deviceTag.DataLength;
                    }
                }
            }

            // initialize table
            if (rowCount > 1)
            {
                curDataTable = new Table(5, rowCount);
                curDataTable.Columns[3].VarWidth = true;
                curDataTable.Columns[3].AlignLeft = false;
                curDataTable.Columns[4].AlignLeft = false;
                curDataRowIndexes = new int[deviceTags.Count];

                if (Locale.IsRussian)
                {
                    Row headerRow = curDataTable.Rows[0];
                    headerRow.Cells[0] = "Номер";
                    headerRow.Cells[1] = "Код";
                    headerRow.Cells[2] = "Наименование";
                    headerRow.Cells[3] = "Значение";
                    headerRow.Cells[4] = "Канал";
                }
                else
                {
                    Row headerRow = curDataTable.Rows[0];
                    headerRow.Cells[0] = "#";
                    headerRow.Cells[1] = "Code";
                    headerRow.Cells[2] = "Name";
                    headerRow.Cells[3] = "Value";
                    headerRow.Cells[4] = "Channel";
                }

                int rowIndex = 1;

                foreach (TagGroup tagGroup in deviceTags.TagGroups)
                {
                    if (tagGroup.Hidden)
                    {
                        foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
                        {
                            curDataRowIndexes[deviceTag.Index] = -1;
                        }
                    }
                    else
                    {
                        if (!deviceTags.FlattenGroups)
                        {
                            Row row = curDataTable.Rows[rowIndex++];
                            row.IsSubheader = true;
                            row.Cells[0] = tagGroup.Name;
                        }

                        foreach (DeviceTag deviceTag in tagGroup.DeviceTags)
                        {
                            curDataRowIndexes[deviceTag.Index] = rowIndex;
                            int dataLen = deviceTag.DataLength;
                            int cnlNum = deviceTag.Cnl == null ? 0 : deviceTag.Cnl.CnlNum;

                            Row row = curDataTable.Rows[rowIndex++];
                            row.Cells[0] = deviceTag.TagNum.ToString();
                            row.Cells[1] = deviceTag.Code;
                            row.Cells[2] = deviceTag.Name;

                            if (cnlNum > 0)
                            {
                                row.Cells[4] = dataLen > 1 ? 
                                    cnlNum + "-" + (cnlNum + dataLen - 1) : cnlNum.ToString();
                            }

                            if (deviceTag.IsNumericArray)
                            {
                                for (int i = 0, lastIdx = dataLen - 1; i <= lastIdx; i++)
                                {
                                    row = curDataTable.Rows[rowIndex++];
                                    row.Cells[2] = deviceTag.Name + "[" + i + "]";

                                    if (cnlNum > 0)
                                        row.Cells[4] = (cnlNum + i).ToString();
                                }
                            }
                        }
                    }
                }

                PadColumn(curDataTable, 0);
                PadColumn(curDataTable, 1);
                PadColumn(curDataTable, 2);
                PadColumn(curDataTable, 4);
            }
        }

        /// <summary>
        /// Sets the display value of the tag.
        /// </summary>
        public void SetDisplayValue(int tagIndex, int offset, string displayValue)
        {
            if (curDataTable != null)
            {
                int rowIndex = curDataRowIndexes[tagIndex] + offset;

                if (rowIndex >= 0)
                    curDataTable.Rows[rowIndex].Cells[3] = displayValue;
            }
        }

        /// <summary>
        /// Adds the archive slice.
        /// </summary>
        public void AddSlice(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            lock (slices)
            {
                while (slices.Count >= ItemCount)
                {
                    slices.Dequeue();
                }

                slices.Enqueue(deviceSlice);
                sliceTable.ResetColumnWidths();
            }
        }

        /// <summary>
        /// Adds the device event.
        /// </summary>
        public void AddEvent(DeviceEvent deviceEvent)
        {
            if (deviceEvent == null)
                throw new ArgumentNullException(nameof(deviceEvent));

            lock (events)
            {
                while (events.Count >= ItemCount)
                {
                    events.Dequeue();
                }

                events.Enqueue(deviceEvent);
                eventTable.ResetColumnWidths();
            }
        }

        /// <summary>
        /// Adds the telecontrol command.
        /// </summary>
        public void AddCommand(TeleCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            lock (commands)
            {
                while (commands.Count >= ItemCount)
                {
                    commands.Dequeue();
                }

                commands.Enqueue(cmd);
                commandTable.ResetColumnWidths();
            }
        }

        /// <summary>
        /// Appends a string representation of the current data to the string builder.
        /// </summary>
        public void AppendCurrentDataInfo(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

            if (curDataTable != null && curDataTable.Rows.Length > 1)
            {
                sb.AppendLine(Locale.IsRussian ?
                    "Текущие данные" :
                    "Current Data");
                AppendTable(sb, curDataTable);
            }
            else
            {
                sb.AppendLine(Locale.IsRussian ?
                    "Теги устройства отсутствуют" :
                    "No device tags");
            }
        }

        /// <summary>
        /// Appends a string representation of the historical data to the string builder.
        /// </summary>
        public void AppendHistoricalDataInfo(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

            if (slices.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Locale.IsRussian ?
                    "Недавние исторические данные" :
                    "Recent Historical Data");

                lock (slices)
                {
                    int rowIndex = 1;

                    foreach (DeviceSlice slice in slices)
                    {
                        Row row = sliceTable.Rows[rowIndex++];
                        row.IsEmpty = false;
                        row.Cells[0] = slice.Timestamp.ToLocalTime().ToLocalizedString();
                        row.Cells[1] = slice.Descr;
                    }

                    while (rowIndex < sliceTable.Rows.Length)
                    {
                        sliceTable.Rows[rowIndex++].IsEmpty = true;
                    }
                }

                AppendTable(sb, sliceTable);
            }
        }

        /// <summary>
        /// Appends a string representation of the events to the string builder.
        /// </summary>
        public void AppendEventInfo(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

            if (events.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Locale.IsRussian ?
                    "Недавние события" :
                    "Recent Events");

                lock (events)
                {
                    int rowIndex = 1;

                    foreach (DeviceEvent ev in events)
                    {
                        Row row = eventTable.Rows[rowIndex++];
                        row.IsEmpty = false;
                        row.Cells[0] = ev.Timestamp.ToLocalTime().ToLocalizedString();
                        row.Cells[1] = ev.DeviceTag?.ToString() ?? "";
                        row.Cells[2] = ev.Descr;
                    }

                    while (rowIndex < eventTable.Rows.Length)
                    {
                        eventTable.Rows[rowIndex++].IsEmpty = true;
                    }
                }

                AppendTable(sb, eventTable);
            }
        }

        /// <summary>
        /// Appends a string representation of the telecontrol commands to the string builder.
        /// </summary>
        public void AppendCommandInfo(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

            if (commands.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(Locale.IsRussian ?
                    "Недавние команды" :
                    "Recent Commands");

                lock (commands)
                {
                    int rowIndex = 1;

                    foreach (TeleCommand cmd in commands)
                    {
                        Row row = commandTable.Rows[rowIndex++];
                        row.IsEmpty = false;
                        row.Cells[0] = cmd.CreationTime.ToLocalTime().ToLocalizedString();
                        row.Cells[1] = GetCommandDescr(cmd);
                    }

                    while (rowIndex < commandTable.Rows.Length)
                    {
                        commandTable.Rows[rowIndex++].IsEmpty = true;
                    }
                }

                AppendTable(sb, commandTable);
            }
        }
    }
}
