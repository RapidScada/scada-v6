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
 * Summary  : Represents a mechanism to read and write slice tables
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
    /// Represents a mechanism to read and write slice tables.
    /// <para>Представляет механизм для чтения и записи таблиц срезов.</para>
    /// </summary>
    public class SliceTableAdapter : Adapter
    {
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 3;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 20;

        /// <summary>
        /// The buffer for writing slices.
        /// </summary>
        protected byte[] sliceBuffer = null;


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

            if (BitConverter.ToUInt16(buffer, 0) != TableType.SliceTable)
                throw new ScadaException("Invalid table type.");

            if (BitConverter.ToUInt16(buffer, 2) != MajorVersion)
                throw new ScadaException("Incompatible format version.");

            return true;
        }

        /// <summary>
        /// Fills the specified table by reading data from a file or stream.
        /// </summary>
        public void Fill(DataTable dataTable)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            Stream stream;
            BinaryReader reader = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // prepare the data table
                dataTable.Rows.Clear();
                dataTable.BeginLoadData();
                dataTable.DefaultView.Sort = "";

                // create table columns
                if (dataTable.Columns.Count == 0)
                {
                    dataTable.Columns.Add("DateTime", typeof(DateTime));
                    dataTable.Columns.Add("CnlNum", typeof(int));
                    dataTable.Columns.Add("Val", typeof(double));
                    dataTable.Columns.Add("Stat", typeof(int));
                }

                // read header
                byte[] buffer = new byte[HeaderSize];
                if (!ReadHeader(reader, buffer))
                    return;

                // read slices
                int[] cnlNums = null;

                while (true)
                {
                    if (reader.ReadUInt16() != BlockMarker)
                        throw new ScadaException("Slice marker not found.");

                    DateTime timestamp = ScadaUtils.TicksToTime(reader.ReadInt64());
                    int cnlCnt = reader.ReadInt32();

                    if (cnlCnt != 0)
                    {
                        int cnlDataSize = cnlCnt * 10;
                        int cnlNumsSize = cnlCnt * 4;
                        ResizeBuffer(ref buffer, cnlDataSize);

                        // read channel numbers
                        if (cnlCnt > 0)
                        {
                            ReadData(reader, buffer, 0, cnlNumsSize + 4, true);
                            cnlNums = new int[cnlCnt];
                            Buffer.BlockCopy(buffer, 0, cnlNums, 0, cnlNumsSize);

                            if (ScadaUtils.CRC32(buffer, 0, cnlNumsSize) != BitConverter.ToUInt32(buffer, cnlNumsSize))
                                throw new ScadaException("CRC error.");
                        }
                        else // cnlCnt < 0, channel numbers are the same
                        {
                            cnlCnt = ~cnlCnt;

                            if (cnlNums == null)
                                throw new ScadaException("Channel numbers are undefined.");

                            if (cnlNums.Length != cnlCnt)
                                throw new ScadaException("Invalid channel count.");
                        }

                        // read channel data
                        ReadData(reader, buffer, 0, cnlDataSize, true);

                        for (int i = 0, index = 0; i < cnlCnt; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            row["DateTime"] = timestamp;
                            row["CnlNum"] = cnlNums[i];
                            row["Val"] = GetDouble(buffer, ref index);
                            row["Stat"] = GetUInt16(buffer, ref index);
                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (EndOfStreamException)
            {
                // normal file ending case
            }
            finally
            {
                reader?.Close();
                dataTable.EndLoadData();
                dataTable.AcceptChanges();

                if (dataTable.Columns.Count > 0)
                    dataTable.DefaultView.Sort = "DateTime, CnlNum";
            }
        }

        /// <summary>
        /// Reads a single slice from a file or stream.
        /// </summary>
        public Slice ReadSingleSlice()
        {
            Stream stream;
            BinaryReader reader = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reader = new BinaryReader(stream, Encoding.UTF8, Stream != null);

                // read header
                byte[] buffer = new byte[HeaderSize];
                if (!ReadHeader(reader, buffer))
                    return null;

                // read the first slice
                if (reader.ReadUInt16() != BlockMarker)
                    throw new ScadaException("Slice marker not found.");

                DateTime timestamp = ScadaUtils.TicksToTime(reader.ReadInt64());
                int cnlCnt = reader.ReadInt32();

                if (cnlCnt > 0)
                {
                    // read channel numbers
                    int cnlDataSize = cnlCnt * 10;
                    int cnlNumsSize = cnlCnt * 4;
                    buffer = new byte[cnlDataSize];
                    ReadData(reader, buffer, 0, cnlNumsSize + 4, true);

                    int[] cnlNums = new int[cnlCnt];
                    Buffer.BlockCopy(buffer, 0, cnlNums, 0, cnlNumsSize);

                    if (ScadaUtils.CRC32(buffer, 0, cnlNumsSize) != BitConverter.ToUInt32(buffer, cnlNumsSize))
                        throw new ScadaException("CRC error.");

                    // read channel data
                    ReadData(reader, buffer, 0, cnlDataSize, true);
                    CnlData[] cnlData = new CnlData[cnlCnt];

                    for (int i = 0, index = 0; i < cnlCnt; i++)
                    {
                        cnlData[i] = GetCnlData(buffer, ref index);
                    }

                    return new Slice(timestamp, cnlNums, cnlData);
                }
                else if (cnlCnt == 0)
                {
                    return new Slice(timestamp, 0);
                }
                else
                {
                    throw new ScadaException("Invalid channel count.");
                }
            }
            catch (EndOfStreamException)
            {
                // unable to read slice
                return null;
            }
            finally
            {
                reader?.Close();
            }
        }

        /// <summary>
        /// Writes the single slice to a file or stream.
        /// </summary>
        public void WriteSingleSlice(Slice slice)
        {
            if (slice == null)
                throw new ArgumentNullException(nameof(slice));

            Stream stream;
            BinaryWriter writer = null;

            try
            {
                stream = Stream ?? new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                writer = new BinaryWriter(stream, Encoding.UTF8, Stream != null);

                // write header
                writer.Write(TableType.SliceTable);
                writer.Write(MajorVersion);
                writer.Write(MinorVersion);
                writer.Write(ReserveBuffer, 0, 14);

                // write slice
                int cnlCnt = slice.Length;
                int sliceSize = cnlCnt > 0 ? cnlCnt * 14 + 18 : 14;
                ResizeBuffer(ref sliceBuffer, sliceSize);
                byte[] buffer = sliceBuffer;

                int index = 0;
                CopyUInt16(BlockMarker, buffer, ref index);
                CopyTime(slice.Timestamp, buffer, ref index);
                CopyInt32(cnlCnt, buffer, ref index);

                if (cnlCnt > 0)
                {
                    int dataLength = cnlCnt * 4;
                    Buffer.BlockCopy(slice.CnlNums, 0, buffer, index, dataLength);
                    uint crc = ScadaUtils.CRC32(buffer, index, dataLength);
                    index += dataLength;
                    CopyInt32((int)crc, buffer, ref index);

                    foreach (CnlData cnlData in slice.CnlData)
                    {
                        CopyCnlData(cnlData, buffer, ref index);
                    }
                }

                writer.Write(buffer, 0, index);
            }
            finally
            {
                writer?.Close();
            }
        }
    }
}
