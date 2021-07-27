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
 * Module   : ScadaServerCommon
 * Summary  : Defines functionality to access the server environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Config;
using System;
using System.Collections.Generic;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Defines functionality to access the server environment.
    /// <para>Определяет функциональность для доступа к окружению сервера.</para>
    /// </summary>
    public interface IServerContext
    {
        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        ServerConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        ServerDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the cached configuration database.
        /// </summary>
        BaseDataSet BaseDataSet { get; }

        /// <summary>
        /// Gets the active input channel numbers.
        /// </summary>
        int[] CnlNums { get; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        IDictionary<string, object> SharedData { get; }


        /// <summary>
        /// Gets the current data of the input channel.
        /// </summary>
        CnlData GetCurrentData(int cnlNum);

        /// <summary>
        /// Gets the current data of the input channels.
        /// </summary>
        CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID);

        /// <summary>
        /// Gets the current data of the cached input channel list.
        /// </summary>
        CnlData[] GetCurrentData(long cnlListID);

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        TrendBundle GetTrends(int archiveBit, TimeRange timeRange, int[] cnlNums);

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        Trend GetTrend(int archiveBit, TimeRange timeRange, int cnlNum);

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        List<DateTime> GetTimestamps(int archiveBit, TimeRange timeRange);

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        Slice GetSlice(int archiveBit, DateTime timestamp, int[] cnlNums);

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        Event GetEventByID(int archiveBit, long eventID);

        /// <summary>
        /// Gets the events.
        /// </summary>
        List<Event> GetEvents(int archiveBit, TimeRange timeRange, DataFilter filter);

        /// <summary>
        /// Writes the current data of the input channel.
        /// </summary>
        void WriteCurrentData(int cnlNum, CnlData cnlData);

        /// <summary>
        /// Writes the current data of the input channels.
        /// </summary>
        void WriteCurrentData(int[] cnlNums, CnlData[] cnlData, int deviceNum, bool applyFormulas);

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        void WriteHistoricalData(int archiveMask, Slice slice, int deviceNum, bool applyFormulas);

        /// <summary>
        /// Writes the event.
        /// </summary>
        void WriteEvent(int archiveMask, Event ev);

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        void SendCommand(TeleCommand command, out CommandResult commandResult);
    }
}
