﻿/*
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
 * Module   : ScadaServerEngine
 * Summary  : Implements the server context interface for accessing the application environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Config;
using Scada.Server.Modules;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements the server context interface for accessing the application environment.
    /// <para>Реализует интерфейс контекста сервера для доступа к окружению приложения.</para>
    /// </summary>
    internal class ServerContext : IServerContext
    {
        private readonly CoreLogic coreLogic;         // the server logic instance
        private readonly ArchiveHolder archiveHolder; // holds archives
        private readonly ServerListener listener;     // the TCP listener


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerContext(CoreLogic coreLogic, ArchiveHolder archiveHolder, ServerListener listener)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            this.archiveHolder = archiveHolder ?? throw new ArgumentNullException(nameof(archiveHolder));
            this.listener = listener ?? throw new ArgumentNullException(nameof(listener));
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public ServerConfig AppConfig => coreLogic.Config;

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public ServerDirs AppDirs => coreLogic.AppDirs;

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log => coreLogic.Log;

        /// <summary>
        /// Gets the configuration database cache.
        /// </summary>
        public BaseDataSet BaseDataSet => coreLogic.BaseDataSet;

        /// <summary>
        /// Gets the active input channel numbers.
        /// </summary>
        public int[] CnlNums => coreLogic.CnlNums;

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public IDictionary<string, object> SharedData => coreLogic.SharedData;


        /// <summary>
        /// Gets the current data of the input channel.
        /// </summary>
        public CnlData GetCurrentData(int cnlNum)
        {
            return coreLogic.GetCurrentData(cnlNum);
        }

        /// <summary>
        /// Gets the current data of the input channels.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID)
        {
            return coreLogic.GetCurrentData(cnlNums, useCache, out cnlListID);
        }

        /// <summary>
        /// Gets the current data of the cached input channel list.
        /// </summary>
        public CnlData[] GetCurrentData(long cnlListID)
        {
            return coreLogic.GetCurrentData(cnlListID);
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        public TrendBundle GetTrends(int[] cnlNums, TimeRange timeRange, int archiveBit)
        {
            return archiveHolder.GetTrends(cnlNums, timeRange, archiveBit);
        }

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        public Trend GetTrend(int cnlNum, TimeRange timeRange, int archiveBit)
        {
            return archiveHolder.GetTrend(cnlNum, timeRange, archiveBit);
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public List<DateTime> GetTimestamps(TimeRange timeRange, int archiveBit)
        {
            return archiveHolder.GetTimestamps(timeRange, archiveBit);
        }

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        public Slice GetSlice(int[] cnlNums, DateTime timestamp, int archiveBit)
        {
            return archiveHolder.GetSlice(cnlNums, timestamp, archiveBit);
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public Event GetEventByID(long eventID, int archiveBit)
        {
            return archiveHolder.GetEventByID(eventID, archiveBit);
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public List<Event> GetEvents(TimeRange timeRange, DataFilter filter, int archiveBit)
        {
            return archiveHolder.GetEvents(timeRange, filter, archiveBit);
        }

        /// <summary>
        /// Writes the current data of the input channel.
        /// </summary>
        public void WriteCurrentData(int cnlNum, CnlData cnlData)
        {
            coreLogic.WriteCurrentData(0, new int[] { cnlNum }, new CnlData[] { cnlData }, true);
        }

        /// <summary>
        /// Writes the current data of the input channels.
        /// </summary>
        public void WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData, bool applyFormulas)
        {
            coreLogic.WriteCurrentData(deviceNum, cnlNums, cnlData, applyFormulas);
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        public void WriteHistoricalData(int deviceNum, Slice slice, int archiveMask, bool applyFormulas)
        {
            coreLogic.WriteHistoricalData(deviceNum, slice, archiveMask, applyFormulas);
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(Event ev, int archiveMask)
        {
            coreLogic.WriteEvent(ev, archiveMask);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(TeleCommand command, out CommandResult commandResult)
        {
            if (command.OutCnlNum > 0)
            {
                // validate and send command
                coreLogic.SendCommand(command, out commandResult);
            }
            else
            {
                // pass command directly to clients
                listener.EnqueueCommand(command);
                commandResult = new CommandResult(true);
            }
        }
    }
}
