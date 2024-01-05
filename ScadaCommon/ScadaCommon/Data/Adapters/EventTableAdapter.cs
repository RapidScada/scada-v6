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
 * Summary  : Represents a mechanism to read and write event tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
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
        /// The maximum number of characters in a text block.
        /// </summary>
        protected const int TextBlockCapacity = 50;
        /// <summary>
        /// The maximum data size in a data block.
        /// </summary>
        protected const int DataBlockCapacity = 100;
        /// <summary>
        /// The maximum number of text blocks.
        /// </summary>
        protected const int MaxTextBlockCount = 2;
        /// <summary>
        /// The maximum number of data blocks.
        /// </summary>
        protected const int MaxDataBlockCount = 2;
        /// <summary>
        /// Indicates the beginning of a new event text block in a file.
        /// </summary>
        protected const ushort TextMarker = 0x0E01;
        /// <summary>
        /// Indicates the beginning of a new event data block in a file.
        /// </summary>
        protected const ushort DataMarker = 0x0E02;
        /// <summary>
        /// The event acknowledged field index.
        /// </summary>
        protected const int AckIndex = 54;
        /// <summary>
        /// The text block count field index.
        /// </summary>
        protected const int TextBlockCountIndex = 68;
        /// <summary>
        /// The data block count field index.
        /// </summary>
        protected const int DataBlockCountIndex = 71;


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
        protected string ReadEventText(BinaryReader reader, byte[] buffer, int textBlockCnt, int textSize)
        {
            if (textBlockCnt <= 0)
                return "";

            if (textBlockCnt > MaxTextBlockCount)
                throw new ScadaException("Number of text blocks exceeded.");

            StringBuilder sbEventText = new StringBuilder(textSize);

            for (int i = 0; i < textBlockCnt; i++)
            {
                ReadData(reader, buffer, 0, EventSize, true);

                if (BitConverter.ToUInt16(buffer, 0) != TextMarker)
                    throw new ScadaException("Text marker not found.");

                if (sbEventText.Length < textSize)
                {
                    int charCnt = Math.Min(textSize - sbEventText.Length, TextBlockCapacity);
                    sbEventText.Append(Encoding.Unicode.GetString(buffer, 2, charCnt * 2));
                }
            }

            return sbEventText.ToString();
        }

        /// <summary>
        /// Reads event data.
        /// </summary>
        protected byte[] ReadEventData(BinaryReader reader, byte[] buffer, int dataBlockCnt, int dataSize)
        {
            if (dataBlockCnt <= 0)
                return null;

            if (dataBlockCnt > MaxDataBlockCount)
                throw new ScadaException("Number of data blocks exceeded.");

            byte[] eventData = new byte[dataSize];
            int dataIndex = 0;

            for (int i = 0; i < dataBlockCnt; i++)
            {
                ReadData(reader, buffer, 0, EventSize, true);

                if (BitConverter.ToUInt16(buffer, 0) != DataMarker)
                    throw new ScadaException("Data marker not found.");

                if (dataIndex < dataSize)
                {
                    int byteCnt = Math.Min(dataSize - dataIndex, DataBlockCapacity);
                    Buffer.BlockCopy(buffer, 2, eventData, dataIndex, byteCnt);
                    dataIndex += byteCnt;
                }
            }

            return eventData;
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
        /// Copies the event to the buffer with the specified block count.
        /// </summary>
        protected void CopyEvent(Event ev, byte[] buffer, int textBlockCnt, int dataBlockCnt,
            out int textSize, out int dataSize)
        {
            textSize = ev.Text == null ? 0 : Math.Min(ev.Text.Length, textBlockCnt * TextBlockCapacity);
            dataSize = ev.Data == null ? 0 : Math.Min(ev.Data.Length, dataBlockCnt * DataBlockCapacity);

            int index = 0;
            CopyUInt16(BlockMarker, buffer, ref index);
            CopyInt64(ev.EventID, buffer, ref index);
            CopyTime(ev.Timestamp, buffer, ref index);
            CopyBool(ev.Hidden, buffer, ref index);
            CopyInt32(ev.CnlNum, buffer, ref index);
            CopyInt32(ev.ObjNum, buffer, ref index);
            CopyInt32(ev.DeviceNum, buffer, ref index);
            CopyDouble(ev.PrevCnlVal, buffer, ref index);
            CopyUInt16((ushort)ev.PrevCnlStat, buffer, ref index);
            CopyDouble(ev.CnlVal, buffer, ref index);
            CopyUInt16((ushort)ev.CnlStat, buffer, ref index);
            CopyUInt16((ushort)ev.Severity, buffer, ref index);
            CopyBool(ev.AckRequired, buffer, ref index);
            CopyBool(ev.Ack, buffer, ref index);
            CopyTime(ev.AckTimestamp, buffer, ref index);
            CopyInt32(ev.AckUserID, buffer, ref index);
            CopyByte((byte)ev.TextFormat, buffer, ref index);
            CopyByte((byte)textBlockCnt, buffer, ref index);
            CopyUInt16((ushort)textSize, buffer, ref index);
            CopyByte((byte)dataBlockCnt, buffer, ref index);
            CopyUInt16((ushort)dataSize, buffer, ref index);
            Array.Clear(buffer, index, EventSize - index - 2);
            ushort crc = ScadaUtils.CRC16(buffer, 0, EventSize - 2);
            CopyUInt16(crc, buffer, EventSize - 2);
        }

        /// <summary>
        /// Copies the event to the buffer and calculates block count.
        /// </summary>
        protected void CopyEvent(Event ev, byte[] buffer, out int textBlockCnt, out int dataBlockCnt, 
            out int textSize, out int dataSize)
        {
            int GetBlockCount(int fullSize, int blockCapacity, int maxBlockCount)
            {
                return Math.Min(
                    fullSize / blockCapacity + (fullSize % blockCapacity > 0 ? 1 : 0),
                    maxBlockCount);
            }

            int fullTextSize = ev.Text == null ? 0 : ev.Text.Length;
            int fullDataSize = ev.Data == null ? 0 : ev.Data.Length;
            textBlockCnt = GetBlockCount(fullTextSize, TextBlockCapacity, MaxTextBlockCount);
            dataBlockCnt = GetBlockCount(fullDataSize, DataBlockCapacity, MaxDataBlockCount);

            CopyEvent(ev, buffer, textBlockCnt, dataBlockCnt, out textSize, out dataSize);
        }

        /// <summary>
        /// Writes the event text.
        /// </summary>
        protected void WriteEventText(BinaryWriter writer, byte[] buffer, string text, int textBlockCnt, int textSize)
        {
            int textIndex = 0;

            for (int i = 0; i < textBlockCnt; i++)
            {
                int charCnt = Math.Min(textSize - textIndex, TextBlockCapacity);

                if (charCnt > 0)
                {
                    int textDataSize = Encoding.Unicode.GetBytes(text, textIndex, charCnt, buffer, 0);
                    textIndex += charCnt;

                    writer.Write(TextMarker);
                    writer.Write(buffer, 0, textDataSize);
                    writer.Write(ReserveBuffer, 0, EventSize - textDataSize - 2);
                }
                else
                {
                    writer.Write(TextMarker);
                    writer.Write(ReserveBuffer, 0, EventSize - 2);
                }
            }
        }

        /// <summary>
        /// Writes the event text.
        /// </summary>
        protected void WriteEventData(BinaryWriter writer, byte[] data, int dataBlockCnt, int dataSize)
        {
            int dataIndex = 0;

            for (int i = 0; i < dataBlockCnt; i++)
            {
                int byteCnt = Math.Min(dataSize - dataIndex, DataBlockCapacity);

                if (byteCnt > 0)
                {
                    writer.Write(DataMarker);
                    writer.Write(data, dataIndex, byteCnt);
                    writer.Write(ReserveBuffer, 0, EventSize - byteCnt - 2);
                    dataIndex += byteCnt;
                }
                else
                {
                    writer.Write(DataMarker);
                    writer.Write(ReserveBuffer, 0, EventSize - 2);
                }
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

                    int textBlockCnt = GetByte(buffer, ref index);
                    int textSize = GetUInt16(buffer, ref index);
                    int dataBlockCnt = GetByte(buffer, ref index);
                    int dataSize = GetUInt16(buffer, ref index);

                    ev.Text = ReadEventText(reader, buffer, textBlockCnt, textSize);
                    ev.Data = ReadEventData(reader, buffer, dataBlockCnt, dataSize);

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

                    int textBlockCnt = GetByte(buffer, ref index);
                    int textSize = GetUInt16(buffer, ref index);
                    int dataBlockCnt = GetByte(buffer, ref index);
                    int dataSize = GetUInt16(buffer, ref index);

                    row["Text"] = ReadEventText(reader, buffer, textBlockCnt, textSize);
                    row["Data"] = ReadEventData(reader, buffer, dataBlockCnt, dataSize);

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
                CopyEvent(ev, buffer, out int textBlockCnt, out int dataBlockCnt, out int textSize, out int dataSize);
                writer.Write(buffer);
                ev.Position = position;

                if (textBlockCnt > 0)
                    WriteEventText(writer, buffer, ev.Text, textBlockCnt, textSize);

                if (dataBlockCnt > 0)
                    WriteEventData(writer, ev.Data, dataBlockCnt, dataSize);
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

                // read text and data block count
                byte[] buffer = new byte[EventSize];
                long eventPosition = ev.Position;
                int textBlockCnt = 0;
                int dataBlockCnt = 0;

                if (stream.Seek(eventPosition, SeekOrigin.Begin) == eventPosition &&
                    ReadEvent(reader, buffer, out int index))
                {
                    textBlockCnt = BitConverter.ToUInt16(buffer, TextBlockCountIndex);
                    dataBlockCnt = BitConverter.ToUInt16(buffer, DataBlockCountIndex);
                }
                else
                {
                    throw new ScadaException("Unable to read event before update.");
                }

                // write event
                CopyEvent(ev, buffer, textBlockCnt, dataBlockCnt, out int textSize, out int dataSize);

                if (stream.Seek(eventPosition, SeekOrigin.Begin) == eventPosition)
                {
                    writer.Write(buffer);

                    if (updateText && textBlockCnt > 0)
                    {
                        long textPosition = eventPosition + EventSize;
                        if (stream.Seek(textPosition, SeekOrigin.Begin) == textPosition)
                            WriteEventText(writer, buffer, ev.Text, textBlockCnt, textSize);
                    }

                    if (updateData && dataBlockCnt > 0)
                    {
                        long dataPosition = eventPosition + EventSize + EventSize * textBlockCnt;
                        if (stream.Seek(dataPosition, SeekOrigin.Begin) == dataPosition)
                            WriteEventData(writer, ev.Data, dataBlockCnt, dataSize);
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
                    index = AckIndex;
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
