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
 * Summary  : Represents a TCP client which interacts with the Server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using static Scada.BinaryConverter;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Client
{
    /// <summary>
    /// Represents a TCP client which interacts with the Server service.
    /// <para>Представляет TCP-клиента, который взаимодействует со службой Сервера.</para>
    /// </summary>
    /// <remarks>The class is not thread-safe.</remarks>
    public class ScadaClient : ClientBase
    {
        /// <summary>
        /// The ID of the last command received.
        /// </summary>
        protected long lastCommandID;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaClient(ConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
            lastCommandID = 0;
        }


        /// <summary>
        /// Writes the channel data.
        /// </summary>
        protected void WriteChannelData(int archiveMask, ICollection<Slice> slices, WriteDataFlags flags)
        {
            if (slices == null)
                throw new ArgumentNullException(nameof(slices));

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.WriteChannelData);
            int index = ArgumentIndex;
            CopyByte((byte)flags, outBuf, ref index);
            CopyInt32(archiveMask, outBuf, ref index);
            CopyInt32(slices.Count, outBuf, ref index);

            foreach (Slice slice in slices)
            {
                CopySlice(slice, outBuf, ref index);
            }

            request.BufferLength = index;
            SendRequest(request);
            ReceiveResponse(request);
        }

        /// <summary>
        /// Receives requested events.
        /// </summary>
        protected List<Event> ReceiveEvents(DataPacket request, out long filterID)
        {
            List<Event> events = null;
            filterID = 0;

            int prevBlockNumber = 0;
            int blockNumber = 0;
            int blockCount = int.MaxValue;
            int totalEventCount = 0;

            while (blockNumber < blockCount)
            {
                ReceiveResponse(request);
                int index = ArgumentIndex;
                blockNumber = GetInt32(inBuf, ref index);
                blockCount = GetInt32(inBuf, ref index);
                filterID = GetInt64(inBuf, ref index);
                totalEventCount = GetInt32(inBuf, ref index);
                int eventCount = GetInt32(inBuf, ref index);

                if (blockNumber != prevBlockNumber + 1)
                    ThrowBlockNumberException();

                if (events == null)
                    events = new List<Event>(totalEventCount);

                for (int i = 0; i < eventCount; i++)
                {
                    events.Add(GetEvent(inBuf, ref index));
                }

                prevBlockNumber = blockNumber;
                OnProgress(blockNumber, blockCount);
            }

            if (events.Count != totalEventCount)
                ThrowDataSizeException();

            return events;
        }


        /// <summary>
        /// Validates the username and password.
        /// </summary>
        public UserValidationResult ValidateUser(string username, string password)
        {
            RestoreConnection();
            return ClientState == ClientState.LoggedIn
                ? Login(username, password)
                : UserValidationResult.Fail(Locale.IsRussian ?
                    "Сервер недоступен" :
                    "Server unavailable");
        }

        /// <summary>
        /// Downloads the table of the configuration database.
        /// </summary>
        public bool DownloadBaseTable(IBaseTable baseTable)
        {
            if (baseTable == null)
                throw new ArgumentNullException(nameof(baseTable));

            DownloadFile(
                new RelativePath(TopFolder.Base, AppFolder.Root, baseTable.FileNameDat), 
                0, 0, false, DateTime.MinValue, false, () => { return new MemoryStream(); }, 
                out _, out FileReadingResult readingResult, out Stream stream);

            try
            {
                if (readingResult == FileReadingResult.Completed && stream != null)
                {
                    stream.Position = 0;
                    BaseTableAdapter adapter = new BaseTableAdapter { Stream = stream };
                    adapter.Fill(baseTable);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID)
        {
            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetCurrentData);
            int index = ArgumentIndex;
            CopyInt64(0, outBuf, ref index);
            CopyBool(useCache, outBuf, ref index);
            CopyIntArray(cnlNums, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            index = ArgumentIndex;
            cnlListID = GetInt64(inBuf, ref index);
            CnlData[] cnlData = GetCnlDataArray(inBuf, ref index);

            if (cnlData.Length != cnlNums.Length)
                ThrowDataSizeException();

            return cnlData;
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(ref long cnlListID)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetCurrentData);
            CopyInt64(cnlListID, outBuf, ArgumentIndex);
            request.ArgumentLength = 8;
            SendRequest(request);

            ReceiveResponse(request);
            int index = ArgumentIndex;
            cnlListID = GetInt64(inBuf, ref index);
            return cnlListID > 0 ? GetCnlDataArray(inBuf, ref index) : Array.Empty<CnlData>();
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public TrendBundle GetTrends(int archiveBit, TimeRange timeRange, int[] cnlNums)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetTrends);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyTimeRange(timeRange, outBuf, ref index);
            CopyIntArray(cnlNums, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            TrendBundle trendBundle = null;
            int prevBlockNumber = 0;
            int blockNumber = 0;
            int blockCount = int.MaxValue;
            int totalPointCount = 0;

            while (blockNumber < blockCount)
            {
                ReceiveResponse(request);
                index = ArgumentIndex;
                blockNumber = GetInt32(inBuf, ref index);
                blockCount = GetInt32(inBuf, ref index);
                totalPointCount = GetInt32(inBuf, ref index);
                int pointCount = GetInt32(inBuf, ref index);

                if (blockNumber != prevBlockNumber + 1)
                    ThrowBlockNumberException();

                if (trendBundle == null)
                    trendBundle = new TrendBundle(cnlNums, totalPointCount);

                for (int i = 0; i < pointCount; i++)
                {
                    trendBundle.Timestamps.Add(GetTime(inBuf, ref index));
                }

                foreach (TrendBundle.CnlDataList trend in trendBundle.Trends)
                {
                    for (int i = 0; i < pointCount; i++)
                    {
                        trend.Add(new CnlData(
                            GetDouble(inBuf, ref index),
                            GetUInt16(inBuf, ref index)));
                    }
                }

                prevBlockNumber = blockNumber;
                OnProgress(blockNumber, blockCount);
            }

            if (trendBundle.Timestamps.Count != totalPointCount)
                ThrowDataSizeException();

            return trendBundle;
        }

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public Trend GetTrend(int archiveBit, TimeRange timeRange, int cnlNum)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetTrends);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyTimeRange(timeRange, outBuf, ref index);
            CopyInt32(1, outBuf, ref index);
            CopyInt32(cnlNum, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            Trend trend = null;
            int prevBlockNumber = 0;
            int blockNumber = 0;
            int blockCount = int.MaxValue;
            int totalPointCount = 0;

            while (blockNumber < blockCount)
            {
                ReceiveResponse(request);
                index = ArgumentIndex;
                blockNumber = GetInt32(inBuf, ref index);
                blockCount = GetInt32(inBuf, ref index);
                totalPointCount = GetInt32(inBuf, ref index);
                int pointCount = GetInt32(inBuf, ref index);

                if (blockNumber != prevBlockNumber + 1)
                    ThrowBlockNumberException();

                if (trend == null)
                    trend = new Trend(cnlNum, totalPointCount);

                for (int i = 0, idx1 = index, idx2 = idx1 + pointCount * 8; i < pointCount; i++)
                {
                    trend.Points.Add(new TrendPoint(
                        GetTime(inBuf, ref idx1),
                        GetDouble(inBuf, ref idx2),
                        GetUInt16(inBuf, ref idx2)));
                }

                prevBlockNumber = blockNumber;
                OnProgress(blockNumber, blockCount);
            }

            if (trend.Points.Count != totalPointCount)
                ThrowDataSizeException();

            return trend;
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public List<DateTime> GetTimestamps(int archiveBit, TimeRange timeRange)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetTimestamps);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyTimeRange(timeRange, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            index = ArgumentIndex;
            int timestampCount = GetInt32(inBuf, ref index);
            List<DateTime> timestamps = new List<DateTime>(timestampCount);

            for (int i = 0; i < timestampCount; i++)
            {
                timestamps.Add(GetTime(inBuf, ref index));
            }

            return timestamps;
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public Slice GetSlice(int archiveBit, DateTime timestamp, int[] cnlNums)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetSlice);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyTime(timestamp, outBuf, ref index);
            CopyIntArray(cnlNums, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            index = ArgumentIndex;
            CnlData[] cnlData = GetCnlDataArray(inBuf, ref index);

            if (cnlData.Length != cnlNums.Length)
                ThrowDataSizeException();

            return new Slice(timestamp, cnlNums, cnlData);
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public DateTime GetLastWriteTime(int archiveBit)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetLastWriteTime);
            outBuf[ArgumentIndex] = (byte)archiveBit;
            request.ArgumentLength = 1;
            SendRequest(request);

            ReceiveResponse(request);
            return GetTime(inBuf, ArgumentIndex);
        }

        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public void WriteCurrentData(Slice slice, WriteDataFlags flags)
        {
            flags |= WriteDataFlags.IsCurrent;
            WriteChannelData(ArchiveMask.Default, new Slice[] { slice }, flags);
        }

        /// <summary>
        /// Writes the multiple slices of the current data.
        /// </summary>
        public void WriteCurrentData(ICollection<Slice> slices, WriteDataFlags flags)
        {
            flags |= WriteDataFlags.IsCurrent;
            WriteChannelData(ArchiveMask.Default, slices, flags);
        }

        /// <summary>
        /// Writes the slice of historical data.
        /// </summary>
        public void WriteHistoricalData(int archiveMask, Slice slice, WriteDataFlags flags)
        {
            flags &= ~WriteDataFlags.IsCurrent;
            WriteChannelData(archiveMask, new Slice[] { slice }, flags);
        }

        /// <summary>
        /// Writes the multiple slices of historical data.
        /// </summary>
        public void WriteHistoricalData(int archiveMask, ICollection<Slice> slices, WriteDataFlags flags)
        {
            flags &= ~WriteDataFlags.IsCurrent;
            WriteChannelData(archiveMask, slices, flags);
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public Event GetEventByID(int archiveBit, long eventID)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetEventByID);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyInt64(eventID, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            index = ArgumentIndex;
            return GetBool(inBuf, ref index) ? GetEvent(inBuf, ref index) : null;
        }

        /// <summary>
        /// Gets the events ordered by timestamp.
        /// </summary>
        public List<Event> GetEvents(int archiveBit, TimeRange timeRange, DataFilter filter,
            bool useCache, out long filterID)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetEvents);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyTimeRange(timeRange, outBuf, ref index);
            CopyInt64(0, outBuf, ref index);
            CopyBool(useCache, outBuf, ref index);
            CopyDataFilter(filter, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            return ReceiveEvents(request, out filterID);
        }

        /// <summary>
        /// Gets the events ordered by timestamp.
        /// </summary>
        public List<Event> GetEvents(int archiveBit, TimeRange timeRange, ref long filterID)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetEvents);
            int index = ArgumentIndex;
            CopyByte((byte)archiveBit, outBuf, ref index);
            CopyTimeRange(timeRange, outBuf, ref index);
            CopyInt64(filterID, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            return ReceiveEvents(request, out filterID);
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(int archiveMask, Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.WriteEvent);
            int index = ArgumentIndex;
            CopyInt32(archiveMask, outBuf, ref index);
            CopyEvent(ev, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            long eventID = BitConverter.ToInt64(inBuf, ArgumentIndex);

            if (eventID > 0)
                ev.EventID = eventID;
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public void AckEvent(EventAck eventAck)
        {
            if (eventAck == null)
                throw new ArgumentNullException(nameof(eventAck));

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.AckEvent);
            int index = ArgumentIndex;
            CopyInt64(eventAck.EventID, outBuf, ref index);
            CopyTime(eventAck.Timestamp, outBuf, ref index);
            CopyInt32(eventAck.UserID, outBuf, ref index);
            request.BufferLength = index;

            SendRequest(request);
            ReceiveResponse(request);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public CommandResult SendCommand(TeleCommand command, WriteCommandFlags flags)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.SendCommand);
            int index = ArgumentIndex;
            CopyInt32(command.UserID, outBuf, ref index);
            CopyInt32(command.CnlNum, outBuf, ref index);
            CopyDouble(command.CmdVal, outBuf, ref index);
            CopyByteArray(command.CmdData, outBuf, ref index);
            CopyByte((byte)flags, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            ReceiveResponse(request);
            index = ArgumentIndex;
            long commandID = GetInt64(inBuf, ref index);

            CommandResult result = new CommandResult
            {
                IsSuccessful = GetBool(inBuf, ref index),
                TransmitToClients = GetBool(inBuf, ref index),
                ErrorMessage = GetString(inBuf, ref index)
            };

            if (commandID > 0 && result.IsSuccessful)
                command.CommandID = commandID;

            return result;
        }

        /// <summary>
        /// Gets a telecontrol command from the server queue.
        /// </summary>
        public TeleCommand GetCommand()
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetCommand);
            CopyInt64(lastCommandID, outBuf, ArgumentIndex);
            request.ArgumentLength = 8;
            SendRequest(request);

            ReceiveResponse(request);
            int index = ArgumentIndex;
            lastCommandID = GetInt64(inBuf, ref index);

            if (lastCommandID > 0)
            {
                return new TeleCommand
                {
                    CommandID = lastCommandID,
                    CreationTime = GetTime(inBuf, ref index),
                    UserID = GetInt32(inBuf, ref index),
                    CnlNum = GetInt32(inBuf, ref index),
                    ObjNum = GetInt32(inBuf, ref index),
                    DeviceNum = GetInt32(inBuf, ref index),
                    CmdNum = GetInt32(inBuf, ref index),
                    CmdCode = GetString(inBuf, ref index),
                    CmdVal = GetDouble(inBuf, ref index),
                    CmdData = GetByteArray(inBuf, ref index)
                };
            }
            else
            {
                return null;
            }
        }
    }
}
