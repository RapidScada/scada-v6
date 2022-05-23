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
 * Module   : ScadaServerEngine
 * Summary  : Implements the server context interface for accessing the application environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Config;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Protocol;
using Scada.Server.Config;
using Scada.Server.Modules;
using Scada.Storages;
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
        /// Gets the instance configuration.
        /// </summary>
        public InstanceConfig InstanceConfig => coreLogic.InstanceConfig;

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public ServerConfig AppConfig => coreLogic.AppConfig;

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public ServerDirs AppDirs => coreLogic.AppDirs;

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        public IStorage Storage => coreLogic.Storage;

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log => coreLogic.Log;

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        public ConfigDatabase ConfigDatabase => coreLogic.ConfigDatabase;

        /// <summary>
        /// Gets the active channel numbers for archiving.
        /// </summary>
        public int[] CnlNums => coreLogic.CnlNums;

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public IDictionary<string, object> SharedData => coreLogic.SharedData;


        /// <summary>
        /// Gets the current data of the specified channel.
        /// </summary>
        public CnlData GetCurrentData(int cnlNum)
        {
            return coreLogic.GetCurrentData(cnlNum);
        }

        /// <summary>
        /// Gets the current data of the specified channels.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID)
        {
            return coreLogic.GetCurrentData(cnlNums, useCache, out cnlListID);
        }

        /// <summary>
        /// Gets the current data of the cached channel list.
        /// </summary>
        public CnlData[] GetCurrentData(long cnlListID)
        {
            return coreLogic.GetCurrentData(cnlListID);
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public TrendBundle GetTrends(int archiveBit, TimeRange timeRange, int[] cnlNums)
        {
            return archiveHolder.GetTrends(archiveBit, timeRange, cnlNums);
        }

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public Trend GetTrend(int archiveBit, TimeRange timeRange, int cnlNum)
        {
            return archiveHolder.GetTrend(archiveBit, timeRange, cnlNum);
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public List<DateTime> GetTimestamps(int archiveBit, TimeRange timeRange)
        {
            return archiveHolder.GetTimestamps(archiveBit, timeRange);
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public Slice GetSlice(int archiveBit, DateTime timestamp, int[] cnlNums)
        {
            return archiveHolder.GetSlice(archiveBit, timestamp, cnlNums);
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public Event GetEventByID(int archiveBit, long eventID)
        {
            return archiveHolder.GetEventByID(archiveBit, eventID);
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public List<Event> GetEvents(int archiveBit, TimeRange timeRange, DataFilter filter)
        {
            return archiveHolder.GetEvents(archiveBit, timeRange, filter);
        }

        /// <summary>
        /// Writes the current data of the specified channel.
        /// </summary>
        public void WriteCurrentData(int cnlNum, CnlData cnlData)
        {
            coreLogic.WriteCurrentData(
                new int[] { cnlNum }, 
                new CnlData[] { cnlData }, 
                0, WriteFlags.EnableAll);
        }

        /// <summary>
        /// Writes the current data of the specified channels.
        /// </summary>
        public void WriteCurrentData(int[] cnlNums, CnlData[] cnlData, int deviceNum, WriteFlags writeFlags)
        {
            coreLogic.WriteCurrentData(cnlNums, cnlData, deviceNum, writeFlags);
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        public void WriteHistoricalData(int archiveMask, Slice slice, int deviceNum, WriteFlags writeFlags)
        {
            coreLogic.WriteHistoricalData(archiveMask, slice, deviceNum, writeFlags);
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(int archiveMask, Event ev)
        {
            coreLogic.WriteEvent(archiveMask, ev);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(TeleCommand command, out CommandResult commandResult)
        {
            if (command.CnlNum > 0)
            {
                // validate and send command
                coreLogic.SendCommand(command, out commandResult);
            }
            else
            {
                // pass command directly to clients
                DateTime utcNow = DateTime.UtcNow;
                command.CommandID = ScadaUtils.GenerateUniqueID(utcNow);
                command.CreationTime = utcNow;
                listener.EnqueueCommand(command);
                commandResult = new CommandResult(true);
            }
        }
    }
}
