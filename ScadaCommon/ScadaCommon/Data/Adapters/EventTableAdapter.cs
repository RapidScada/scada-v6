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
 * Module   : ScadaData
 * Summary  : Represents a mechanism to read and write event tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Data;
using System.IO;
using System.Text;
using static Scada.BinaryConverter;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write event tables.
    /// <para>Представляет механизм для чтения и записи таблиц событий.</para>
    /// </summary>
    public class EventTableAdapter : Adapter
    {
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 4;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 20;
        /// <summary>
        /// The event size in a file.
        /// </summary>
        protected const int EventSize = 102;
        /// <summary>
        /// The maximum size of the event text.
        /// </summary>
        protected const int MaxTextSize = 50;
        /// <summary>
        /// The maximum size of the event data.
        /// </summary>
        protected const int MaxDataSize = 50;
        /// <summary>
        /// Indicates the beginning of a new event text block in a file.
        /// </summary>
        protected const ushort TextMarker = 0x0E01;
        /// <summary>
        /// Indicates the beginning of a new event data block in a file.
        /// </summary>
        protected const ushort DataMarker = 0x0E02;


        /// <summary>
        /// Reads and validates the table header.
        /// </summary>
        protected bool ReadHeader(BinaryReader reader, byte[] buffer)
        {
            int bytesRead = reader.Read(buffer, 0, HeaderSize);

            if (bytesRead == 0) // table is empty
                return false;

            if (bytesRead < HeaderSize)
                throw new ScadaException("Unexpected end of stream.");

            if (BitConverter.ToUInt16(buffer, 0) != TableType.EventTable)
                throw new ScadaException("Invalid table type.");

            if (BitConverter.ToUInt16(buffer, 2) != MajorVersion)
                throw new ScadaException("Incompatible format version.");

            if (BitConverter.ToUInt16(buffer, 6) != EventSize)
                throw new ScadaException("Invalid event size.");

            return true;
        }

        /// <summary>
        /// Reads an event into the buffer and validates the event.
        /// </summary>
        protected bool ReadEvent(BinaryReader reader, byte[] buffer, out int index)
        {
            index = 0;

            if (reader.Read(buffer, 0, EventSize) == EventSize)
            {
                if (GetUInt16(buffer, ref index) != BlockMarker)
                    throw new ScadaException("Event marker not found.");

                if (ScadaUtils.CRC16(buffer, 0, EventSize - 2) != BitConverter.ToUInt16(buffer, EventSize - 2))
                    throw new ScadaException("Event CRC error.");

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reads event text.
        /// </summary>
        protected string ReadEventText(BinaryReader reader, byte[] buffer, int textSize)
        {
            if (textSize > 0)
            {
                ReadData(reader, buffer, 0, EventSize, true);

                if (BitConverter.ToUInt16(buffer, 0) != TextMarker)
                    throw new ScadaException("Event text marker not found.");

                return Encoding.Unicode.GetString(buffer, 2, textSize);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Reads event data.
        /// </summary>
        protected byte[] ReadEventData(BinaryReader reader, byte[] buffer, int dataSize)
        {
            if (dataSize > 0)
            {
                if (reader.ReadUInt16() != DataMarker)
                    throw new ScadaException("Event data marker not found.");

                byte[] eventData = new byte[dataSize];
                ReadData(reader, eventData, 0, dataSize, true);
                ReadData(reader, buffer, 0, EventSize - dataSize - 2, true);
                return eventData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Estimates the number of events, assuming 10% of events contain custom text or data.
        /// </summary>
        protected int EstimateCapacity(long streamLength)
        {
            return (int)((streamLength - HeaderSize) / EventSize * 1.1);
        }

        /// <summary>
        /// Reads the table header.
        /// </summary>
        protected void WriteHeader(BinaryWriter writer)
        {
            writer.Write(TableType.EventTable);
            writer.Write(MajorVersion);
            writer.Write(MinorVersion);
            writer.Write((ushort)EventSize);
            writer.Write(ReserveBuffer, 0, 12);
        }

        /// <summary>
        /// Copies the event to the buffer.
        /// </summary>
        protected void CopyEvent(Event ev, bool textExists, bool dataExists,
            byte[] buffer, out int textSize, out int dataSize)
        {
            textSize = ev.Text == null ? 0 : Math.Min(ev.Text.Length, MaxTextSize);
            dataSize = ev.Data == null ? 0 : Math.Min(ev.Data.Length, MaxDataSize);

            int index = 0;
            CopyUInt16(BlockMarker, buffer, ref index);
            CopyInt64(ev.EventID, buffer, ref index);
            CopyTime(ev.Timestamp, buffer, ref index);
            CopyBool(ev.Hidden, buffer, ref index);
            CopyInt32(ev.CnlNum, buffer, ref index);
            CopyInt32(ev.OutCnlNum, buffer, ref index);
            CopyInt32(ev.ObjNum, buffer, ref index);
            CopyInt32(ev.DeviceNum, buffer, ref index);
            CopyDouble(ev.PrevCnlVal, buffer, ref index);
            CopyUInt16((ushort)ev.PrevCnlStat, buffer, ref index);
            CopyDouble(ev.CnlVal, buffer, ref index);
            CopyUInt16((ushort)ev.CnlStat, buffer, ref index);
            CopyInt32(ev.Severity, buffer, ref index);
            CopyBool(ev.AckRequired, buffer, ref index);
            CopyBool(ev.Ack, buffer, ref index);
            CopyTime(ev.AckTimestamp, buffer, ref index);
            CopyInt32(ev.AckUserID, buffer, ref index);
            CopyByte((byte)ev.TextFormat, buffer, ref index);
            CopyBool(textExists, buffer, ref index);
            CopyByte((byte)textSize, buffer, ref index);
            CopyBool(dataExists, buffer, ref index);
            CopyByte((byte)dataSize, buffer, ref index);
            Array.Clear(buffer, index, EventSize - index - 2);
            ushort crc = ScadaUtils.CRC16(buffer, 0, EventSize - 2);
            CopyUInt16(crc, buffer, EventSize - 2);
        }

        /// <summary>
        /// Copies the event to the buffer.
        /// </summary>
        protected void CopyEvent(Event ev, byte[] buffer, out int textSize, out int dataSize)
        {
            CopyEvent(ev,
                ev.Text != null && ev.Text.Length > 0,
                ev.Data != null && ev.Data.Length > 0,
                buffer, out textSize, out dataSize);
        }

        /// <summary>
        /// Writes the event text.
        /// </summary>
        protected void WriteEventText(BinaryWriter writer, string text, int textSize, byte[] buffer)
        {
            int textDataSize = Encoding.Unicode.GetBytes(text ?? "", 0, textSize, buffer, 0);
            writer.Write(TextMarker);
            writer.Write(buffer, 0, textDataSize);
            writer.Write(EmptyBuffer, 0, EventSize - textDataSize - 2);
        }

        /// <summary>
        /// Writes the event text.
        /// </summary>
        protected void WriteEventData(BinaryWriter writer, byte[] data, int dataSize)
        {
            writer.Write(DataMarker);

            if (data == null)
            {
                writer.Write(EmptyBuffer, 0, EventSize - 2);
            }
            else
            {
                writer.Write(data, 0, dataSize);
                writer.Write(EmptyBuffer, 0, EventSize - dataSize - 2);
            }
        }


        /// <summary>
        /// Fills the specified table by reading data from a file or stream.
        /// </summary>
        public void Fill(EventTable eventTable)
        {
            if (eventTable == null)
                throw new ArgumentNullException(nameof(eventTable));

            Stream stream = null;
            BinaryReader reader = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // prepare table
                eventTable.BeginLoadData();
                eventTable.Events.Capacity = EstimateCapacity(stream.Length);

                // read header
                byte[] buffer = new byte[Math.Max(HeaderSize, EventSize)];
                if (!ReadHeader(reader, buffer))
                    return;

                // read events
                long eventPosition = stream.Position;

                while (ReadEvent(reader, buffer, out int index))
                {
                    Event ev = new Event
                    {
                        EventID = GetInt64(buffer, ref index),
                        Timestamp = GetTime(buffer, ref index),
                        Hidden = GetBool(buffer, ref index),
                        CnlNum = GetInt32(buffer, ref index),
                        OutCnlNum = GetInt32(buffer, ref index),
                        ObjNum = GetInt32(buffer, ref index),
                        DeviceNum = GetInt32(buffer, ref index),
                        PrevCnlVal = GetDouble(buffer, ref index),
                        PrevCnlStat = GetUInt16(buffer, ref index),
                        CnlVal = GetDouble(buffer, ref index),
                        CnlStat = GetUInt16(buffer, ref index),
                        Severity = GetUInt16(buffer, ref index),
                        AckRequired = GetBool(buffer, ref index),
                        Ack = GetBool(buffer, ref index),
                        AckTimestamp = GetTime(buffer, ref index),
                        AckUserID = GetInt32(buffer, ref index),
                        TextFormat = (EventTextFormat)GetByte(buffer, ref index),
                        Position = eventPosition
                    };

                    bool textExists = GetBool(buffer, ref index);
                    int textSize = GetByte(buffer, ref index);
                    bool dataExists = GetBool(buffer, ref index);
                    int dataSize = GetByte(buffer, ref index);

                    ev.Text = textExists ? ReadEventText(reader, buffer, textSize) : "";
                    ev.Data = dataExists ? ReadEventData(reader, buffer, dataSize) : null;

                    eventTable.Events.Add(ev);
                    eventPosition = stream.Position;
                }
            }
            finally
            {
                reader?.Close();
                eventTable.EndLoadData();
            }
        }

        /// <summary>
        /// Fills the specified table by reading data from a file or stream.
        /// </summary>
        public void Fill(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            Stream stream = null;
            BinaryReader reader = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // prepare table
                dataTable.Rows.Clear();
                dataTable.BeginLoadData();
                dataTable.DefaultView.Sort = "";

                // create table columns
                if (dataTable.Columns.Count == 0)
                {
                    dataTable.Columns.Add("EventID", typeof(long));
                    dataTable.Columns.Add("Timestamp", typeof(DateTime));
                    dataTable.Columns.Add("Hidden", typeof(bool));
                    dataTable.Columns.Add("CnlNum", typeof(int));
                    dataTable.Columns.Add("OutCnlNum", typeof(int));
                    dataTable.Columns.Add("ObjNum", typeof(int));
                    dataTable.Columns.Add("DeviceNum", typeof(int));
                    dataTable.Columns.Add("PrevCnlVal", typeof(double));
                    dataTable.Columns.Add("PrevCnlStat", typeof(int));
                    dataTable.Columns.Add("CnlVal", typeof(double));
                    dataTable.Columns.Add("CnlStat", typeof(int));
                    dataTable.Columns.Add("Severity", typeof(int));
                    dataTable.Columns.Add("AckRequired", typeof(bool));
                    dataTable.Columns.Add("Ack", typeof(bool));
                    dataTable.Columns.Add("AckTimestamp", typeof(DateTime));
                    dataTable.Columns.Add("AckUserID", typeof(int));
                    dataTable.Columns.Add("TextFormat", typeof(int));
                    dataTable.Columns.Add("Text", typeof(string));
                    dataTable.Columns.Add("Data", typeof(byte[]));
                    dataTable.Columns.Add("Position", typeof(long));
                }

                // read header
                byte[] buffer = new byte[Math.Max(HeaderSize, EventSize)];
                if (!ReadHeader(reader, buffer))
                    return;

                // read events
                long eventPosition = stream.Position;

                while (ReadEvent(reader, buffer, out int index))
                {
                    DataRow row = dataTable.NewRow();
                    row["EventID"] = GetInt64(buffer, ref index);
                    row["Timestamp"] = GetTime(buffer, ref index);
                    row["Hidden"] = GetBool(buffer, ref index);
                    row["CnlNum"] = GetInt32(buffer, ref index);
                    row["OutCnlNum"] = GetInt32(buffer, ref index);
                    row["ObjNum"] = GetInt32(buffer, ref index);
                    row["DeviceNum"] = GetInt32(buffer, ref index);
                    row["PrevCnlVal"] = GetDouble(buffer, ref index);
                    row["PrevCnlStat"] = GetUInt16(buffer, ref index);
                    row["CnlVal"] = GetDouble(buffer, ref index);
                    row["CnlStat"] = GetUInt16(buffer, ref index);
                    row["Severity"] = GetUInt16(buffer, ref index);
                    row["AckRequired"] = GetBool(buffer, ref index);
                    row["Ack"] = GetBool(buffer, ref index);
                    row["AckTimestamp"] = GetTime(buffer, ref index);
                    row["AckUserID"] = GetInt32(buffer, ref index);
                    row["TextFormat"] = GetByte(buffer, ref index);
                    row["Position"] = eventPosition;

                    bool textExists = GetBool(buffer, ref index);
                    int textSize = GetByte(buffer, ref index);
                    bool dataExists = GetBool(buffer, ref index);
                    int dataSize = GetByte(buffer, ref index);

                    row["Text"] = textExists ? ReadEventText(reader, buffer, textSize) : "";
                    row["Data"] = dataExists ? ReadEventData(reader, buffer, dataSize) : null;

                    dataTable.Rows.Add(row);
                    eventPosition = stream.Position;
                }
            }
            finally
            {
                reader?.Close();
                dataTable.EndLoadData();
                dataTable.AcceptChanges();

                if (dataTable.Columns.Count > 0)
                    dataTable.DefaultView.Sort = "Timestamp";
            }
        }

        /// <summary>
        /// Appends the specified event to a file or stream.
        /// </summary>
        public void AppendEvent(Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            Stream stream;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                long position = stream.Seek(0, SeekOrigin.End);

                if (position >= HeaderSize)
                {
                    // set the proper writing position
                    long eventIndex = (position - HeaderSize) / EventSize;
                    long offset = HeaderSize + eventIndex * EventSize;

                    if (position != offset)
                        position = stream.Seek(offset, SeekOrigin.Begin);
                }
                else
                {
                    // write header
                    if (position > 0)
                        stream.Seek(0, SeekOrigin.Begin);

                    WriteHeader(writer);
                    position = HeaderSize;
                }

                // write event
                byte[] buffer = new byte[EventSize];
                CopyEvent(ev, buffer, out int textSize, out int dataSize);
                writer.Write(buffer);
                ev.Position = position;

                if (textSize > 0)
                    WriteEventText(writer, ev.Text, textSize, buffer);

                if (dataSize > 0)
                    WriteEventData(writer, ev.Data, dataSize);
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        /// Updates the specified event in a file or stream.
        /// </summary>
        public void UpdateEvent(Event ev, bool updateText, bool updateData)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            if (ev.Position < 0)
                throw new ScadaException("Event position is undefined.");

            Stream stream = null;
            BinaryReader reader = null;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                // check if text and data blocks exist in the file and find their positions
                bool textExists = false;
                bool dataExists = false;
                long textPosition = -1;
                long dataPosition = -1;

                long streamLength = stream.Length;
                long blockPosition = ev.Position + EventSize;

                if (blockPosition < streamLength)
                {
                    stream.Seek(blockPosition, SeekOrigin.Begin);
                    ushort marker = reader.ReadUInt16();

                    if (marker == TextMarker)
                    {
                        textExists = true;
                        textPosition = blockPosition;
                        blockPosition += EventSize;

                        if (blockPosition < streamLength)
                        {
                            stream.Seek(blockPosition, SeekOrigin.Begin);

                            if (reader.ReadUInt16() == DataMarker)
                            {
                                dataExists = true;
                                dataPosition = blockPosition;
                            }
                        }
                    }
                    else if (marker == DataMarker)
                    {
                        dataExists = true;
                        dataPosition = blockPosition;
                    }
                }

                // write event
                byte[] buffer = new byte[EventSize];
                CopyEvent(ev, textExists, dataExists, buffer, out int textSize, out int dataSize);

                if (stream.Seek(ev.Position, SeekOrigin.Begin) == ev.Position)
                {
                    writer.Write(buffer);

                    if (updateText && textExists)
                    {
                        stream.Seek(textPosition, SeekOrigin.Begin);
                        WriteEventText(writer, ev.Text, textSize, buffer);
                    }

                    if (updateData && dataExists)
                    {
                        stream.Seek(dataPosition, SeekOrigin.Begin);
                        WriteEventData(writer, ev.Data, dataSize);
                    }
                }
            }
            finally
            {
                reader?.Close();
                writer?.Close();
            }
        }

        /// <summary>
        /// Writes the event acknowledgement information in a file or stream.
        /// </summary>
        public void WriteEventAck(Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            if (ev.Position < 0)
                throw new ScadaException("Event position is undefined.");

            Stream stream = null;
            BinaryReader reader = null;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                // read the existing event into the buffer
                byte[] buffer = new byte[EventSize];

                if (stream.Seek(ev.Position, SeekOrigin.Begin) == ev.Position &&
                    ReadEvent(reader, buffer, out int index) &&
                    GetInt64(buffer, ref index) == ev.EventID)
                {
                    // update the event in the buffer
                    index = 58;
                    CopyBool(ev.Ack, buffer, ref index);
                    CopyTime(ev.AckTimestamp, buffer, ref index);
                    CopyInt32(ev.AckUserID, buffer, ref index);
                    ushort crc = ScadaUtils.CRC16(buffer, 0, EventSize - 2);
                    CopyUInt16(crc, buffer, EventSize - 2);

                    // write the updated buffer
                    stream.Seek(ev.Position, SeekOrigin.Begin);
                    writer.Write(buffer);
                }
            }
            finally
            {
                reader?.Close();
                writer?.Close();
            }
        }

        /// <summary>
        /// Gets the table file name corresponding to the specified archive and date.
        /// </summary>
        public static string GetTableFileName(string archiveCode, DateTime tableDate)
        {
            return archiveCode.ToLowerInvariant() + tableDate.ToString("yyyyMMdd") + ".dat";
        }
    }
}
