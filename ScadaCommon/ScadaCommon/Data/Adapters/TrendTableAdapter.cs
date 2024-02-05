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
 * Summary  : Represents a mechanism to read and write trend tables
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Scada.BinaryConverter;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read and write trend tables.
    /// <para>Представляет механизм для чтения и записи таблиц трендов.</para>
    /// </summary>
    public class TrendTableAdapter : Adapter
    {
        /// <summary>
        /// The major version number.
        /// </summary>
        protected const ushort MajorVersion = 1;
        /// <summary>
        /// The minor version number.
        /// </summary>
        protected const ushort MinorVersion = 0;
        /// <summary>
        /// The header size in a file.
        /// </summary>
        protected const int HeaderSize = 50;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendTableAdapter()
            : base()
        {
            ParentDirectory = null;
            ArchiveCode = "";
            CnlNumCache = null;
        }


        /// <summary>
        /// Hides the FileName property.
        /// </summary>
        protected new string FileName { get; set; }

        /// <summary>
        /// Hides the Stream property.
        /// </summary>
        protected new Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the parent directory of the table.
        /// </summary>
        public string ParentDirectory { get; set; }

        /// <summary>
        /// Gets or sets the archive code.
        /// </summary>
        public string ArchiveCode { get; set; }

        /// <summary>
        /// Gets or sets the cache containing lists of channel numbers accessed by list ID.
        /// </summary>
        public MemoryCache<long, CnlNumList> CnlNumCache { get; set; }


        /// <summary>
        /// Gets the file position of the data availability flags.
        /// </summary>
        protected long GetFlagsPosition(int cnlCnt)
        {
            return HeaderSize + cnlCnt * 4 + 16;
        }

        /// <summary>
        /// Gets the file position of the data availability flag.
        /// </summary>
        protected long GetFlagPosition(int cnlCnt, int indexInPage)
        {
            return HeaderSize + cnlCnt * 4 + 16 + indexInPage;
        }

        /// <summary>
        /// Gets the file position of the trend data.
        /// </summary>
        protected long GetTrendPosition(int cnlCnt, int pageCapacity, int cnlIndex)
        {
            return GetFlagsPosition(cnlCnt) + pageCapacity + 10 * pageCapacity * cnlIndex;
        }

        /// <summary>
        /// Gets the file position of the channel data.
        /// </summary>
        protected long GetDataPosition(int cnlCnt, int pageCapacity, int cnlIndex, int indexInPage)
        {
            return GetFlagsPosition(cnlCnt) + pageCapacity + 10 * (pageCapacity * cnlIndex + indexInPage);
        }

        /// <summary>
        /// Gets the page size on disk.
        /// </summary>
        protected long GetPageSize(int cnlCnt, int pageCapacity)
        {
            return GetFlagsPosition(cnlCnt) + pageCapacity * (cnlCnt * 10 + 1);
        }

        /// <summary>
        /// Gets the data position of the specified period within the table.
        /// </summary>
        protected bool GetDataPosition(TrendTable trendTable, TimeRange timeRange,
            out int startPageIndex, out int startIndexInPage, out int endPageIndex, out int endIndexInPage,
            out int pointCount)
        {
            TrendTableMeta tableMeta = trendTable.Metadata;
            DateTime startTime = timeRange.StartTime < tableMeta.MinTimestamp ?
                tableMeta.MinTimestamp : timeRange.StartTime;
            DateTime endTime = timeRange.EndTime > tableMeta.MaxTimestamp ?
                tableMeta.MaxTimestamp : timeRange.EndTime;

            if (startTime <= endTime &&
                trendTable.GetDataPosition(startTime, PositionKind.Ceiling,
                    out TrendTablePage startPage, out startIndexInPage) &&
                trendTable.GetDataPosition(endTime,
                    timeRange.EndInclusive ? PositionKind.Floor : PositionKind.FloorExclusive,
                    out TrendTablePage endPage, out endIndexInPage))
            {
                startPageIndex = startPage.PageIndex;
                endPageIndex = endPage.PageIndex;
                pointCount = tableMeta.PageCapacity * (endPageIndex - startPageIndex) +
                    endIndexInPage - startIndexInPage + 1;
                return true;
            }

            startPageIndex = -1;
            startIndexInPage = -1;
            endPageIndex = -1;
            endIndexInPage = -1;
            pointCount = 0;
            return false;
        }

        /// <summary>
        /// Reads metadata using the specified reader.
        /// </summary>
        protected TrendTableMeta ReadMetadata(BinaryReader reader)
        {
            byte[] buffer = new byte[HeaderSize];
            int bytesRead = reader.Read(buffer, 0, HeaderSize);

            if (bytesRead == 0) // table is empty
            {
                return null;
            }
            else
            {
                if (bytesRead < HeaderSize)
                    throw new ScadaException("Unexpected end of stream.");

                int index = 0;
                if (GetUInt16(buffer, ref index) != TableType.TrendTable)
                    throw new ScadaException("Invalid table type.");

                if (GetUInt16(buffer, ref index) != MajorVersion)
                    throw new ScadaException("Incompatible format version.");

                index += 2; // skip minor version
                return new TrendTableMeta
                {
                    MinTimestamp = GetTime(buffer, ref index),
                    MaxTimestamp = GetTime(buffer, ref index),
                    WritingPeriod = GetInt32(buffer, ref index),
                    PageCapacity = GetInt32(buffer, ref index)
                };
            }
        }

        /// <summary>
        /// Writes the metadata using the specified writer.
        /// </summary>
        protected void WriteMetadata(BinaryWriter writer, TrendTableMeta meta)
        {
            writer.Write(TableType.TrendTable);
            writer.Write(MajorVersion);
            writer.Write(MinorVersion);
            writer.Write(meta.MinTimestamp.Ticks);
            writer.Write(meta.MaxTimestamp.Ticks);
            writer.Write(meta.WritingPeriod);
            writer.Write(meta.PageCapacity);
            writer.Write(ReserveBuffer, 0, 20);
        }

        /// <summary>
        /// Reads channel numbers.
        /// </summary>
        protected CnlNumList ReadCnlNums(BinaryReader reader, bool useCache)
        {
            long listID = reader.ReadInt64();
            CnlNumList cnlNumList = useCache && CnlNumCache != null ? CnlNumCache.Get(listID) : null;

            if (cnlNumList == null)
            {
                int cnlCnt = reader.ReadInt32();
                int dataLength = cnlCnt * 4;
                byte[] buffer = new byte[dataLength + 4];
                ReadData(reader, buffer, 0, buffer.Length, true);
                uint crc = BitConverter.ToUInt32(buffer, dataLength);

                if (ScadaUtils.CRC32(buffer, 0, dataLength) != crc)
                    throw new ScadaException("Channel list CRC error.");

                int[] cnlNums = new int[cnlCnt];
                Buffer.BlockCopy(buffer, 0, cnlNums, 0, dataLength);
                cnlNumList = new CnlNumList(listID, cnlNums, crc);

                if (useCache && CnlNumCache != null)
                    CnlNumCache.Add(listID, cnlNumList);
            }

            return cnlNumList;
        }

        /// <summary>
        /// Writes the channel numbers.
        /// </summary>
        protected void WriteCnlNums(BinaryWriter writer, CnlNumList cnlNumList)
        {
            int bufferLength = cnlNumList.CnlNums.Length * 4 + 16;
            byte[] buffer = new byte[bufferLength];

            int index = 0;
            CopyInt64(cnlNumList.ListID, buffer, ref index);
            CopyIntArray(cnlNumList.CnlNums, buffer, ref index);
            CopyInt32((int)cnlNumList.CRC, buffer, ref index);
            writer.Write(buffer, 0, bufferLength);
        }

        /// <summary>
        /// Reads the channel data from the current stream position.
        /// </summary>
        protected CnlData ReadCnlData(BinaryReader reader)
        {
            double val = reader.ReadDouble();
            int stat = reader.ReadUInt16();
            return new CnlData(val, stat);
        }

        /// <summary>
        /// Writes the channel data to the current stream position.
        /// </summary>
        protected void WriteCnlData(BinaryWriter writer, CnlData cnlData)
        {
            writer.Write(cnlData.Val);
            writer.Write((ushort)cnlData.Stat);
        }

        /// <summary>
        /// Makes the table ready for reading and writing.
        /// </summary>
        protected bool MakeTableReady(TrendTable trendTable, bool allowNewMetadata)
        {
            if (trendTable.IsReady)
            {
                return true;
            }
            else
            {
                string metaFileName = GetMetaPath(trendTable);
                TrendTableMeta tableMeta = ReadMetadata(metaFileName);

                if (tableMeta == null)
                {
                    if (allowNewMetadata)
                    {
                        trendTable.SetDefaultMetadata();
                        WriteMetadata(metaFileName, trendTable.Metadata);
                        trendTable.IsReady = true;
                    }
                }
                else
                {
                    if (trendTable.Metadata == null)
                        trendTable.SetMetadata(tableMeta);
                    else if (!trendTable.Metadata.Equals(tableMeta))
                        throw new ScadaException("Invalid table metadata.");

                    trendTable.IsReady = true;
                }

                return trendTable.IsReady;
            }
        }

        /// <summary>
        /// Makes the page ready for reading and writing.
        /// </summary>
        protected bool MakePageReady(TrendTablePage page, bool allowNewPage, BinaryReader reader, BinaryWriter writer)
        {
            if (page.IsReady)
            {
                return true;
            }
            else
            {
                TrendTableMeta pageMeta = ReadMetadata(reader);

                if (pageMeta == null)
                {
                    if (allowNewPage)
                    {
                        AllocateNewPage(writer, page);
                        page.IsReady = true;
                    }
                }
                else
                {
                    if (page.Metadata != null)
                    {
                        if (page.Metadata.Equals(pageMeta))
                            page.CnlNumList = ReadCnlNums(reader, true);
                        else
                            throw new ScadaException("Invalid page metadata.");
                    }

                    ValidatePage(page);
                    page.IsReady = true;
                }

                return page.IsReady;
            }
        }

        /// <summary>
        /// Validates that the table properties are initialized.
        /// </summary>
        protected void ValidateTable(TrendTable trendTable)
        {
            if (trendTable.Metadata == null)
                throw new ScadaException("Table metadata must not be null.");

            if (trendTable.CnlNumList == null)
                throw new ScadaException("Table channel numbers must not be null.");
        }

        /// <summary>
        /// Validates that the page properties are initialized.
        /// </summary>
        protected void ValidatePage(TrendTablePage page)
        {
            if (page.Metadata == null)
                throw new ScadaException("Page metadata must not be null.");

            if (page.CnlNumList == null)
                throw new ScadaException("Page channel numbers must not be null.");
        }

        /// <summary>
        /// Allocates a new page on disk.
        /// </summary>
        protected void AllocateNewPage(BinaryWriter writer, TrendTablePage page)
        {
            if (page.CnlNumList == null)
                page.CnlNumList = page.TrendTable.CnlNumList;

            ValidatePage(page);
            int pageCapacity = page.Metadata.PageCapacity;
            int cnlCnt = page.CnlNumList.CnlNums.Length;
            writer.BaseStream.SetLength(GetPageSize(cnlCnt, pageCapacity));
            WriteMetadata(writer, page.Metadata);
            WriteCnlNums(writer, page.CnlNumList);
            writer.Write(new byte[pageCapacity], 0, pageCapacity); // data availability flags
        }

        /// <summary>
        /// Allocates a new page on disk.
        /// </summary>
        protected void AllocateNewPage(string fileName, TrendTablePage page)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    AllocateNewPage(writer, page);
                }
            }
        }


        /// <summary>
        /// Reads metadata from the file.
        /// </summary>
        public TrendTableMeta ReadMetadata(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    return ReadMetadata(reader);
                }
            }
        }

        /// <summary>
        /// Writes the metadata to the file.
        /// </summary>
        public void WriteMetadata(string fileName, TrendTableMeta meta)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    WriteMetadata(writer, meta);
                }
            }
        }

        /// <summary>
        /// Reads channel numbers.
        /// </summary>
        public CnlNumList ReadCnlNums(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    stream.Position = HeaderSize;
                    return ReadCnlNums(reader, false);
                }
            }
        }

        /// <summary>
        /// Reads the trends of the specified channels from the trend table.
        /// </summary>
        public TrendBundle ReadTrends(TrendTable trendTable, TimeRange timeRange, int[] cnlNums)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));

            if (MakeTableReady(trendTable, false) && GetDataPosition(trendTable, timeRange,
                out int startPageIndex, out int startIndexInPage, out int endPageIndex, out int endIndexInPage,
                out int pointCount))
            {
                TrendBundle trendBundle = new TrendBundle(cnlNums, pointCount);
                byte[] flagBuffer = new byte[trendTable.Metadata.PageCapacity];
                byte[] dataBuffer = new byte[trendTable.Metadata.PageCapacity * 10];

                for (int pageIndex = startPageIndex; pageIndex <= endPageIndex; pageIndex++)
                {
                    TrendTablePage page = trendTable.Pages[pageIndex];
                    string pageFileName = GetPagePath(page);

                    if (!File.Exists(pageFileName))
                        continue;

                    // read data from the page
                    Stream stream;
                    BinaryReader reader = null;

                    try
                    {
                        stream = new FileStream(pageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = new BinaryReader(stream, Encoding.UTF8, false);

                        if (MakePageReady(page, false, reader, null))
                        {
                            int pageCapacity = page.Metadata.PageCapacity;
                            int pageCnlCnt = page.CnlNumList.CnlNums.Length;
                            int startPointIndex = pageIndex == startPageIndex ? startIndexInPage : 0;
                            int endPointIndex = pageIndex == endPageIndex ? endIndexInPage : pageCapacity - 1;
                            int pointsToRead = endPointIndex - startPointIndex + 1;

                            // read data availability flags
                            stream.Position = GetFlagPosition(pageCnlCnt, startPointIndex);
                            ReadData(reader, flagBuffer, 0, pointsToRead, true);

                            // create timestamps
                            int writingPeriod = page.Metadata.WritingPeriod;
                            DateTime timestamp = page.Metadata.MinTimestamp.AddSeconds(startPointIndex * writingPeriod);

                            for (int i = 0; i < pointsToRead; i++)
                            {
                                if (flagBuffer[i] > 0)
                                    trendBundle.Timestamps.Add(timestamp);

                                timestamp = timestamp.AddSeconds(writingPeriod);
                            }

                            // read trends
                            for (int trendIndex = 0, cnlCnt = cnlNums.Length; trendIndex < cnlCnt; trendIndex++)
                            {
                                TrendBundle.CnlDataList trend = trendBundle.Trends[trendIndex];

                                if (page.GetCnlIndex(cnlNums[trendIndex], out int cnlIndex))
                                {
                                    stream.Position = GetDataPosition(pageCnlCnt, pageCapacity, cnlIndex, startPointIndex);
                                    ReadData(reader, dataBuffer, 0, pointsToRead * 10, true);
                                    int bufferIndex = 0;

                                    for (int n = 0; n < pointsToRead; n++)
                                    {
                                        if (flagBuffer[n] > 0)
                                            trend.Add(GetCnlData(dataBuffer, ref bufferIndex));
                                        else
                                            bufferIndex += 10;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < pointsToRead; i++)
                                    {
                                        if (flagBuffer[i] > 0)
                                            trend.Add(CnlData.Empty);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        reader?.Close();
                    }
                }

                return trendBundle;
            }
            else
            {
                return new TrendBundle(cnlNums, 0); // empty trends
            }
        }

        /// <summary>
        /// Reads the trend of the specified channel from the trend table.
        /// </summary>
        public Trend ReadTrend(TrendTable trendTable, TimeRange timeRange, int cnlNum)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (MakeTableReady(trendTable, false) && GetDataPosition(trendTable, timeRange,
                out int startPageIndex, out int startIndexInPage, out int endPageIndex, out int endIndexInPage,
                out int pointCount))
            {
                Trend trend = new Trend(cnlNum, pointCount);
                byte[] flagBuffer = new byte[trendTable.Metadata.PageCapacity];
                byte[] dataBuffer = new byte[trendTable.Metadata.PageCapacity * 10];

                for (int pageIndex = startPageIndex; pageIndex <= endPageIndex; pageIndex++)
                {
                    TrendTablePage page = trendTable.Pages[pageIndex];
                    string pageFileName = GetPagePath(page);

                    if (!File.Exists(pageFileName))
                        continue;

                    // read data from the page
                    Stream stream;
                    BinaryReader reader = null;

                    try
                    {
                        stream = new FileStream(pageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = new BinaryReader(stream, Encoding.UTF8, false);

                        if (MakePageReady(page, false, reader, null))
                        {
                            int pageCapacity = page.Metadata.PageCapacity;
                            int pageCnlCnt = page.CnlNumList.CnlNums.Length;
                            int startPointIndex = pageIndex == startPageIndex ? startIndexInPage : 0;
                            int endPointIndex = pageIndex == endPageIndex ? endIndexInPage : pageCapacity - 1;
                            int pointsToRead = endPointIndex - startPointIndex + 1;

                            // read data availability flags
                            stream.Position = GetFlagPosition(pageCnlCnt, startPointIndex);
                            ReadData(reader, flagBuffer, 0, pointsToRead, true);

                            // read trend
                            int writingPeriod = page.Metadata.WritingPeriod;
                            DateTime timestamp = page.Metadata.MinTimestamp.AddSeconds(startPointIndex * writingPeriod);

                            if (page.GetCnlIndex(cnlNum, out int cnlIndex))
                            {
                                stream.Position = GetDataPosition(pageCnlCnt, pageCapacity, cnlIndex, startPointIndex);
                                ReadData(reader, dataBuffer, 0, pointsToRead * 10, true);
                                int bufferIndex = 0;

                                for (int i = 0; i < pointsToRead; i++)
                                {
                                    if (flagBuffer[i] > 0)
                                    {
                                        trend.Points.Add(new TrendPoint(timestamp,
                                            BitConverter.ToDouble(dataBuffer, bufferIndex),
                                            BitConverter.ToUInt16(dataBuffer, bufferIndex + 8)));
                                    }

                                    timestamp = timestamp.AddSeconds(writingPeriod);
                                    bufferIndex += 10;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < pointsToRead; i++)
                                {
                                    if (flagBuffer[i] > 0)
                                        trend.Points.Add(new TrendPoint(timestamp));

                                    timestamp = timestamp.AddSeconds(writingPeriod);
                                }
                            }
                        }
                    }
                    finally
                    {
                        reader?.Close();
                    }
                }

                return trend;
            }
            else
            {
                return new Trend(cnlNum, 0); // empty trend
            }
        }

        /// <summary>
        /// Reads the available timestamps from the trend table.
        /// </summary>
        public List<DateTime> ReadTimestamps(TrendTable trendTable, TimeRange timeRange)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (MakeTableReady(trendTable, false) && GetDataPosition(trendTable, timeRange,
                out int startPageIndex, out int startIndexInPage, out int endPageIndex, out int endIndexInPage,
                out int pointCount))
            {
                List<DateTime> timestamps = new List<DateTime>(pointCount);
                byte[] flagBuffer = new byte[trendTable.Metadata.PageCapacity];

                for (int pageIndex = startPageIndex; pageIndex <= endPageIndex; pageIndex++)
                {
                    TrendTablePage page = trendTable.Pages[pageIndex];
                    string pageFileName = GetPagePath(page);

                    if (!File.Exists(pageFileName))
                        continue;

                    // read data from the page
                    Stream stream;
                    BinaryReader reader = null;

                    try
                    {
                        stream = new FileStream(pageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = new BinaryReader(stream, Encoding.UTF8, false);

                        if (MakePageReady(page, false, reader, null))
                        {
                            int pageCapacity = page.Metadata.PageCapacity;
                            int pageCnlCnt = page.CnlNumList.CnlNums.Length;
                            int startPointIndex = pageIndex == startPageIndex ? startIndexInPage : 0;
                            int endPointIndex = pageIndex == endPageIndex ? endIndexInPage : pageCapacity - 1;
                            int pointsToRead = endPointIndex - startPointIndex + 1;

                            // read data availability flags
                            stream.Position = GetFlagPosition(pageCnlCnt, startPointIndex);
                            ReadData(reader, flagBuffer, 0, pointsToRead, true);

                            // create timestamps
                            int writingPeriod = page.Metadata.WritingPeriod;
                            DateTime timestamp = page.Metadata.MinTimestamp.AddSeconds(startPointIndex * writingPeriod);

                            for (int i = 0; i < pointsToRead; i++)
                            {
                                if (flagBuffer[i] > 0)
                                    timestamps.Add(timestamp);

                                timestamp = timestamp.AddSeconds(writingPeriod);
                            }
                        }
                    }
                    finally
                    {
                        reader?.Close();
                    }
                }

                return timestamps;
            }
            else
            {
                return new List<DateTime>(); // no timestamps
            }
        }

        /// <summary>
        /// Reads the slice of the specified channels from the trend table.
        /// </summary>
        public Slice ReadSlice(TrendTable trendTable, DateTime timestamp, int[] cnlNums)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));

            if (MakeTableReady(trendTable, false) &&
                trendTable.GetDataPosition(timestamp, PositionKind.Exact, out TrendTablePage page, out int indexInPage))
            {
                Slice slice = new Slice(timestamp, cnlNums);
                string pageFileName = GetPagePath(page);

                if (File.Exists(pageFileName))
                {
                    // read data from the page
                    Stream stream;
                    BinaryReader reader = null;

                    try
                    {
                        stream = new FileStream(pageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = new BinaryReader(stream, Encoding.UTF8, false);

                        if (MakePageReady(page, false, reader, null))
                        {
                            // read data availability flag
                            int pageCnlCnt = page.CnlNumList.CnlNums.Length;
                            stream.Position = GetFlagPosition(pageCnlCnt, indexInPage);

                            if (reader.ReadBoolean())
                            {
                                // read channel data
                                int pageCapacity = page.Metadata.PageCapacity;

                                for (int i = 0, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
                                {
                                    if (page.GetCnlIndex(cnlNums[i], out int cnlIndex))
                                    {
                                        stream.Position = GetDataPosition(pageCnlCnt, pageCapacity, cnlIndex, indexInPage);
                                        slice.CnlData[i] = ReadCnlData(reader);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        reader?.Close();
                    }
                }

                return slice;
            }
            else
            {
                return new Slice(timestamp, cnlNums); // empty slice
            }
        }

        /// <summary>
        /// Reads the channel data from the trend table.
        /// </summary>
        public CnlData ReadCnlData(TrendTable trendTable, DateTime timestamp, int cnlNum)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (MakeTableReady(trendTable, false) &&
                trendTable.GetDataPosition(timestamp, PositionKind.Exact, out TrendTablePage page, out int indexInPage))
            {
                string pageFileName = GetPagePath(page);

                if (File.Exists(pageFileName))
                {
                    Stream stream;
                    BinaryReader reader = null;

                    try
                    {
                        stream = new FileStream(pageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = new BinaryReader(stream, Encoding.UTF8, false);

                        if (MakePageReady(page, false, reader, null) &&
                            page.GetCnlIndex(cnlNum, out int cnlIndex))
                        {
                            stream.Position = GetDataPosition(page.CnlNumList.CnlNums.Length,
                                page.Metadata.PageCapacity, cnlIndex, indexInPage);
                            return ReadCnlData(reader);
                        }
                    }
                    finally
                    {
                        reader?.Close();
                    }
                }
            }

            return CnlData.Empty;
        }

        /// <summary>
        /// Writes the specified slice to the trend table.
        /// </summary>
        public void WriteSlice(TrendTable trendTable, Slice slice)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (slice == null)
                throw new ArgumentNullException(nameof(slice));

            if (MakeTableReady(trendTable, true) &&
                trendTable.GetDataPosition(slice.Timestamp, PositionKind.Exact,
                out TrendTablePage page, out int indexInPage))
            {
                Stream stream;
                BinaryReader reader = null;
                BinaryWriter writer = null;

                try
                {
                    string pageFileName = GetPagePath(page);
                    stream = new FileStream(pageFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    reader = new BinaryReader(stream, Encoding.UTF8, false);
                    writer = new BinaryWriter(stream, Encoding.UTF8, false);

                    // make the page ready
                    MakePageReady(page, true, reader, writer);

                    // write data availability flag
                    int pageCnlCnt = page.CnlNumList.CnlNums.Length;
                    stream.Position = GetFlagPosition(pageCnlCnt, indexInPage);
                    writer.Write(true);

                    // write channel data
                    int pageCapacity = page.Metadata.PageCapacity;

                    if (slice.CnlNums == page.CnlNumList.CnlNums)
                    {
                        for (int i = 0, cnlCnt = slice.Length; i < cnlCnt; i++)
                        {
                            stream.Position = GetDataPosition(pageCnlCnt, pageCapacity, i, indexInPage);
                            WriteCnlData(writer, slice.CnlData[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0, cnlCnt = slice.Length; i < cnlCnt; i++)
                        {
                            if (page.GetCnlIndex(slice.CnlNums[i], out int cnlIndex))
                            {
                                stream.Position = GetDataPosition(pageCnlCnt, pageCapacity, cnlIndex, indexInPage);
                                WriteCnlData(writer, slice.CnlData[i]);
                            }
                        }
                    }
                }
                finally
                {
                    reader?.Close();
                    writer?.Close();
                }
            }
            else
            {
                throw new ScadaException("Failed to write slice.");
            }
        }

        /// <summary>
        /// Writes the channel data to the trend table.
        /// </summary>
        public void WriteCnlData(TrendTable trendTable, DateTime timestamp, int cnlNum, CnlData cnlData)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (MakeTableReady(trendTable, true) &&
                trendTable.GetDataPosition(timestamp, PositionKind.Exact, out TrendTablePage page, out int indexInPage))
            {
                Stream stream;
                BinaryReader reader = null;
                BinaryWriter writer = null;

                try
                {
                    string pageFileName = GetPagePath(page);
                    stream = new FileStream(pageFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    reader = new BinaryReader(stream, Encoding.UTF8, false);
                    writer = new BinaryWriter(stream, Encoding.UTF8, false);

                    // make the page ready
                    MakePageReady(page, true, reader, writer);

                    // write data availability flag
                    int pageCnlCnt = page.CnlNumList.CnlNums.Length;
                    stream.Position = GetFlagPosition(pageCnlCnt, indexInPage);
                    writer.Write(true);

                    // write channel data
                    if (page.GetCnlIndex(cnlNum, out int cnlIndex))
                    {
                        stream.Position = GetDataPosition(pageCnlCnt,
                            page.Metadata.PageCapacity, cnlIndex, indexInPage);
                        WriteCnlData(writer, cnlData);
                    }
                }
                finally
                {
                    reader?.Close();
                    writer?.Close();
                }
            }
            else
            {
                throw new ScadaException("Failed to write channel data.");
            }
        }

        /// <summary>
        /// Updates the table structure on disk according to the specified table and copies the existing table data.
        /// </summary>
        public void UpdateTableStructure(TrendTable trendTable, TrendTableMeta srcTableMeta)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (srcTableMeta == null)
                throw new ArgumentNullException(nameof(srcTableMeta));

            ValidateTable(trendTable);
            string tableDir = GetTablePath(trendTable);

            if (Directory.Exists(tableDir))
            {
                string srcTableDir = tableDir + ".bak";
                Directory.Move(tableDir, srcTableDir);
                WriteMetadata(GetMetaPath(trendTable), trendTable.Metadata);

                // read data from the source table
                TrendTable srcTable = new TrendTable();
                srcTable.SetMetadata(srcTableMeta);

                int srcPageCapacity = srcTableMeta.PageCapacity;
                byte[] buffer = new byte[srcPageCapacity * 10];

                DateTime[] timestamps = new DateTime[srcPageCapacity];
                bool[] dataAvailable = new bool[srcPageCapacity];
                CnlData[] cnlDataArr = new CnlData[srcPageCapacity];

                CnlNumList destCnlNums = trendTable.CnlNumList;
                int destCnlCnt = destCnlNums.CnlNums.Length;

                foreach (TrendTablePage srcPage in srcTable.Pages)
                {
                    string srcPageFileName = Path.Combine(srcTableDir,
                        GetPageFileName(ArchiveCode, srcTable.TableDate, srcPage.PageNumber));

                    if (!File.Exists(srcPageFileName))
                        continue;

                    Stream inStream = null;
                    Stream outStream = null;
                    BinaryReader reader = null;
                    BinaryWriter writer = null;

                    void OpenOrCreatePage(TrendTablePage destPage)
                    {
                        writer?.Close();
                        outStream = new FileStream(GetPagePath(destPage),
                            FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                        writer = new BinaryWriter(outStream, Encoding.UTF8, false);

                        if (!destPage.IsReady)
                        {
                            AllocateNewPage(writer, destPage);
                            destPage.IsReady = true;
                        }
                    }

                    try
                    {
                        inStream = new FileStream(srcPageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        reader = new BinaryReader(inStream, Encoding.UTF8, false);

                        TrendTableMeta srcPageMeta = ReadMetadata(reader);
                        CnlNumList srcCnlNums = ReadCnlNums(reader, true);

                        if (!srcPage.Metadata.Equals(srcPageMeta))
                            throw new ScadaException("Invalid page metadata.");

                        // get timestamps
                        srcPageCapacity = srcPageMeta.PageCapacity;
                        int srcWritingPeriod = srcPageMeta.WritingPeriod;
                        DateTime timestamp = srcPageMeta.MinTimestamp;

                        for (int i = 0; i < srcPageCapacity; i++)
                        {
                            timestamps[i] = timestamp;
                            timestamp = timestamp.AddSeconds(srcWritingPeriod);
                        }

                        // copy trends
                        Array.Clear(dataAvailable, 0, srcPageCapacity);
                        int srcCnlCnt = srcCnlNums.CnlNums.Length;
                        int trendDataSize = srcPageCapacity * 10;

                        for (int destCnlIndex = 0; destCnlIndex < destCnlCnt; destCnlIndex++)
                        {
                            if (srcCnlNums.CnlIndexes.TryGetValue(destCnlNums.CnlNums[destCnlIndex],
                                out int srcCnlIndex))
                            {
                                // read a trend from the source page
                                inStream.Position = GetTrendPosition(srcCnlCnt, srcPageCapacity, srcCnlIndex);
                                ReadData(reader, buffer, 0, trendDataSize, true);
                                int bufferIndex = 0;

                                for (int i = 0; i < srcPageCapacity; i++)
                                {
                                    CnlData cnlData = GetCnlData(buffer, ref bufferIndex);
                                    dataAvailable[i] = dataAvailable[i] || cnlData.Stat > 0;
                                    cnlDataArr[i] = cnlData;
                                }

                                // write the trend to the destination table
                                for (int i = 0, prevPageNumber = 0; i < srcPageCapacity; i++)
                                {
                                    if (trendTable.GetDataPosition(timestamps[i], PositionKind.Exact,
                                        out TrendTablePage destPage, out int indexInPage))
                                    {
                                        if (destPage.PageNumber > prevPageNumber)
                                        {
                                            prevPageNumber = destPage.PageNumber;
                                            OpenOrCreatePage(destPage);
                                        }

                                        outStream.Position = GetDataPosition(destCnlCnt,
                                            destPage.Metadata.PageCapacity, destCnlIndex, indexInPage);
                                        WriteCnlData(writer, cnlDataArr[i]);
                                    }
                                }
                            }
                        }

                        // write data availability flags
                        for (int i = 0, prevPageNumber = 0; i < srcPageCapacity; i++)
                        {
                            if (trendTable.GetDataPosition(timestamps[i], PositionKind.Exact,
                                out TrendTablePage destPage, out int indexInPage))
                            {
                                if (destPage.PageNumber > prevPageNumber)
                                {
                                    prevPageNumber = destPage.PageNumber;
                                    OpenOrCreatePage(destPage);
                                }

                                outStream.Position = GetFlagPosition(destCnlCnt, indexInPage);
                                writer.Write(dataAvailable[i]);
                            }
                        }
                    }
                    finally
                    {
                        reader?.Close();
                        writer?.Close();
                    }
                }

                Directory.Delete(srcTableDir);
            }
            else
            {
                WriteMetadata(GetMetaPath(trendTable), trendTable.Metadata);
            }

            trendTable.IsReady = true;
        }

        /// <summary>
        /// Updates the page channel numbers on disk and copies the existing page data.
        /// </summary>
        public void UpdatePageChannels(TrendTablePage page, CnlNumList srcCnlNums)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (srcCnlNums == null)
                throw new ArgumentNullException(nameof(srcCnlNums));

            string pageFileName = GetPagePath(page);

            if (File.Exists(pageFileName))
            {
                string srcPageFileName = pageFileName + ".bak";
                File.Move(pageFileName, srcPageFileName);
                AllocateNewPage(pageFileName, page);

                Stream inStream = null;
                Stream outStream = null;
                BinaryReader reader = null;
                BinaryWriter writer = null;

                try
                {
                    inStream = new FileStream(srcPageFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    outStream = new FileStream(pageFileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    reader = new BinaryReader(inStream, Encoding.UTF8, false);
                    writer = new BinaryWriter(outStream, Encoding.UTF8, false);

                    int pageCapacity = page.Metadata.PageCapacity;
                    int trendDataSize = pageCapacity * 10;
                    byte[] buffer = new byte[trendDataSize];

                    // copy data availability flags
                    int srcCnlCnt = srcCnlNums.CnlNums.Length;
                    inStream.Position = GetFlagsPosition(srcCnlCnt);
                    ReadData(reader, buffer, 0, pageCapacity, true);

                    CnlNumList destCnlNums = page.CnlNumList;
                    int destCnlCnt = destCnlNums.CnlNums.Length;
                    outStream.Position = GetFlagsPosition(destCnlCnt);
                    writer.Write(buffer, 0, pageCapacity);

                    // copy trends
                    for (int destCnlIndex = 0; destCnlIndex < destCnlCnt; destCnlIndex++)
                    {
                        if (srcCnlNums.CnlIndexes.TryGetValue(destCnlNums.CnlNums[destCnlIndex], out int srcCnlIndex))
                        {
                            inStream.Position = GetTrendPosition(srcCnlCnt, pageCapacity, srcCnlIndex);
                            ReadData(reader, buffer, 0, trendDataSize, true);

                            outStream.Position = GetTrendPosition(destCnlCnt, pageCapacity, destCnlIndex);
                            writer.Write(buffer, 0, trendDataSize);
                        }
                    }
                }
                finally
                {
                    reader?.Close();
                    writer?.Close();
                }

                File.Delete(srcPageFileName);
            }
            else
            {
                AllocateNewPage(pageFileName, page);
            }

            page.IsReady = true;
        }

        /// <summary>
        /// Creates a backup of the table and deletes the original table directory.
        /// </summary>
        public void BackupTable(TrendTable trendTable)
        {
            const int AttemptCount = 100;
            string origTableDir = GetTablePath(trendTable);
            string backupTableDir = "";

            for (int copyNumber = 1; copyNumber <= AttemptCount; copyNumber++)
            {
                string s = origTableDir + ".copy" + copyNumber.ToString("D3");

                if (!Directory.Exists(s))
                {
                    backupTableDir = s;
                    break;
                }
            }

            if (backupTableDir == "")
                throw new ScadaException("Failed to backup table.");
            else
                Directory.Move(origTableDir, backupTableDir);
        }

        /// <summary>
        /// Gets the full path of the trend table.
        /// </summary>
        public string GetTablePath(TrendTable trendTable)
        {
            if (trendTable == null)
                throw new ArgumentNullException(nameof(trendTable));

            if (trendTable.TableDate == DateTime.MinValue)
                throw new ScadaException("Table date must be defined.");

            if (string.IsNullOrEmpty(ParentDirectory))
                throw new ScadaException("Parent directory must not be empty.");

            if (string.IsNullOrEmpty(ArchiveCode))
                throw new ScadaException("Archive code must not be empty.");

            return Path.Combine(ParentDirectory, GetTableDirectory(ArchiveCode, trendTable.TableDate));
        }

        /// <summary>
        /// Gets the full path of the metadata file.
        /// </summary>
        public string GetMetaPath(TrendTable trendTable)
        {
            return Path.Combine(GetTablePath(trendTable), GetMetaFileName(ArchiveCode, trendTable.TableDate));
        }

        /// <summary>
        /// Gets the full path of the page file.
        /// </summary>
        public string GetPagePath(TrendTablePage page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (string.IsNullOrEmpty(page.Path))
            {
                page.Path = Path.Combine(
                    GetTablePath(page.TrendTable),
                    GetPageFileName(ArchiveCode, page.TrendTable.TableDate, page.PageNumber));
            }

            return page.Path;
        }

        /// <summary>
        /// Gets the trend table directory corresponding to the specified archive and date.
        /// </summary>
        public static string GetTableDirectory(string archiveCode, DateTime tableDate)
        {
            return archiveCode + tableDate.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Gets the metadata file name corresponding to the specified archive and date.
        /// </summary>
        public static string GetMetaFileName(string archiveCode, DateTime tableDate)
        {
            return archiveCode.ToLowerInvariant() + tableDate.ToString("yyyyMMdd") + "_meta.dat";
        }

        /// <summary>
        /// Gets the page file name corresponding to the specified archive, date and page number.
        /// </summary>
        public static string GetPageFileName(string archiveCode, DateTime tableDate, int pageNumber)
        {
            return archiveCode.ToLowerInvariant() + tableDate.ToString("yyyyMMdd") +
                "_" + pageNumber.ToString("D4") + ".dat";
        }
    }
}
