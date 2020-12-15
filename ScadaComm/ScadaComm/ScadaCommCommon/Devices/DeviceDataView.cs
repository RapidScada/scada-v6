/*
 * Copyright 2020 Mikhail Shiryaev
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
 * Modified : 2020
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents device data for display.
    /// <para>Представляет данные КП для отображения.</para>
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
                IsEmpty = true;
                Cells = new string[cellCount];
            }
            public bool IsEmpty { get; set; }
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
                Width = -1;
            }
            public bool AlignLeft { get; set; }
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

            if (cmd.CmdNum > 0)
                sb.Append("Num=").Append(cmd.CmdNum).Append(", ");

            if (!string.IsNullOrEmpty(cmd.CmdCode))
                sb.Append("Code=").Append(cmd.CmdCode).Append(", ");

            if (cmd.CmdData == null)
            {
                sb.Append("Val=").Append(cmd.CmdVal.ToString("N3", Locale.Culture));
            }
            else
            {
                const int MaxDisplayLength = 10;
                int displayLength = Math.Min(cmd.CmdData.Length, MaxDisplayLength);
                sb.Append("Data=").Append(ScadaUtils.BytesToHex(cmd.CmdData, 0, displayLength));

                if (displayLength < cmd.CmdData.Length)
                    sb.Append("...");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Appends the table to the string builder.
        /// </summary>
        private void AppendTable(StringBuilder sb, Table table)
        {

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
            }
        }

        /// <summary>
        /// Appends a string representation of the current data to the string builder.
        /// </summary>
        public void AppendCurrentDataInfo(StringBuilder sb)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

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

                lock (slices)
                {
                    int rowIndex = 1;

                    foreach (DeviceSlice slice in slices)
                    {
                        Row row = sliceTable.Rows[rowIndex++];
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

                lock (events)
                {
                    int rowIndex = 1;

                    foreach (DeviceEvent ev in events)
                    {
                        Row row = eventTable.Rows[rowIndex++];
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

                lock (commands)
                {
                    int rowIndex = 1;

                    foreach (TeleCommand cmd in commands)
                    {
                        Row row = commandTable.Rows[rowIndex++];
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
