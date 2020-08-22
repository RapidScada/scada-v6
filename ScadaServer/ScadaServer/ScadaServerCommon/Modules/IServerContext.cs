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
 * Module   : ScadaServerCommon
 * Summary  : Defines functionality to access the server features
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Config;
using System;
using System.Collections.Generic;

namespace Scada.Server.Modules
{
    /// <summary>
    /// Defines functionality to access the server features.
    /// <para>Определяет функциональность для доступа к функциям сервера.</para>
    /// </summary>
    public interface IServerContext
    {
        /// <summary>
        /// Gets the configuration database cache.
        /// </summary>
        BaseDataSet BaseDataSet { get; }

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
        /// Gets the application level shared data.
        /// </summary>
        IDictionary<string, object> SharedData { get; }


        /// <summary>
        /// Gets the active channel numbers.
        /// </summary>
        int[] GetCnlNums();

        /// <summary>
        /// Gets the current data of the input channel.
        /// </summary>
        CnlData GetCurrentData(int cnlNum);

        /// <summary>
        /// Gets the current data of the input channels.
        /// </summary>
        Slice GetCurrentData(int[] cnlNums);

        /// <summary>
        /// Gets a trend of the input channel from the specified archive.
        /// </summary>
        Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime, int archiveBit);

        /// <summary>
        /// Gets a slice of the input channels from the specified archive.
        /// </summary>
        Slice GetSlice(int[] cnlNums, DateTime timestamp, int archiveBit);

        /// <summary>
        /// Gets the timestamps available in the specified archive.
        /// </summary>
        DateTime[] GetTimestamps(DateTime startTime, DateTime endTime, int archiveBit);

        /// <summary>
        /// Writes the current data of the input channel.
        /// </summary>
        bool WriteCurrentData(int cnlNum, CnlData cnlData);

        /// <summary>
        /// Writes the current data of the input channels.
        /// </summary>
        bool WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData, bool applyFormulas);

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        bool WriteHistoricalData(int deviceNum, Slice slice, int archiveMask, bool applyFormulas);

        /// <summary>
        /// Writes the event.
        /// </summary>
        bool WriteEvent(Event ev, int archiveMask);

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        void SendCommand(TeleCommand command, out CommandResult commandResult);
    }
}
