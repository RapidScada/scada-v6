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
 * Module   : ScadaServerEngine
 * Summary  : Holds archives classified by archive kinds
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Archives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Holds archives classified by archive kinds.
    /// <para>Содержит архивы, классифицированные по видам.</para>
    /// </summary>
    internal class ArchiveHolder
    {
        private static readonly string ErrorInArchive = Locale.IsRussian ?
            "Ошибка при вызове метода {0} архива {1}" :
            "Error calling the {0} method of the {1} archive";

        private readonly ILog log;                // the application log
        private readonly ArchiveLogic[] arcByBit; // the archives accessed by bit number


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            arcByBit = new ArchiveLogic[ServerUtils.MaxArchiveCount];

            CurrentArchives = new List<CurrentArchiveLogic>();
            HistoricalArchives = new List<HistoricalArchiveLogic>();
            EventArchives = new List<EventArchiveLogic>();
        }


        /// <summary>
        /// Gets the current archives.
        /// </summary>
        public List<CurrentArchiveLogic> CurrentArchives { get; }

        /// <summary>
        /// Gets the historical archives.
        /// </summary>
        public List<HistoricalArchiveLogic> HistoricalArchives { get; }

        /// <summary>
        /// Gets the event archives.
        /// </summary>
        public List<EventArchiveLogic> EventArchives { get; }


        /// <summary>
        /// Calls the ReadCurrentData method of the current archives until a successful result is obtained.
        /// </summary>
        public void ReadCurrentData(ICurrentData curData)
        {
            lock (CurrentArchives)
            {
                foreach (CurrentArchiveLogic archiveLogic in CurrentArchives)
                {
                    try
                    {
                        if (archiveLogic.ReadData(curData))
                            return;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "ReadCurrentData", archiveLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the ProcessData method of the current and historical archives.
        /// </summary>
        public void ProcessData(ICurrentData curData)
        {
            lock (CurrentArchives)
            {
                foreach (CurrentArchiveLogic archiveLogic in CurrentArchives)
                {
                    try
                    {
                        archiveLogic.ProcessData(curData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "ProcessData", archiveLogic.Code);
                    }
                }
            }

            lock (CurrentArchives)
            {
                foreach (HistoricalArchiveLogic archiveLogic in HistoricalArchives)
                {
                    try
                    {
                        archiveLogic.ProcessData(curData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "ProcessData", archiveLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the DeleteOutdatedData method of the archives.
        /// </summary>
        public void DeleteOutdatedData()
        {
            DateTime utcNow = DateTime.UtcNow;

            void DoCleanup(IList archives)
            {
                lock (archives)
                {
                    foreach (ArchiveLogic archiveLogic in archives)
                    {
                        try
                        {
                            if (utcNow - archiveLogic.LastCleanupTime > archiveLogic.CleanupPeriod)
                            {
                                archiveLogic.LastCleanupTime = utcNow;
                                archiveLogic.DeleteOutdatedData();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.WriteException(ex, ErrorInArchive, "DeleteOutdatedData", archiveLogic.Code);
                        }
                    }
                }
            }

            DoCleanup(CurrentArchives);
            DoCleanup(HistoricalArchives);
            DoCleanup(EventArchives);
        }

        /// <summary>
        /// Calls the EndUpdate method of the specified archive.
        /// </summary>
        public void EndUpdate(HistoricalArchiveLogic archiveLogic)
        {
            try
            {
                archiveLogic.EndUpdate();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, ErrorInArchive, "EndUpdate", archiveLogic.Code);
            }
        }

        /// <summary>
        /// Gets an archive by the specified bit number.
        /// </summary>
        public bool GetArchive<T>(int archiveBit, out T archiveLogic) where T : ArchiveLogic
        {
            archiveLogic = 0 <= archiveBit && archiveBit < ServerUtils.MaxArchiveCount ? 
                arcByBit[archiveBit] as T : null;
            return archiveLogic != null;
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        public TrendBundle GetTrends(int[] cnlNums, DateTime startTime, DateTime endTime, int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetTrends(cnlNums, startTime, endTime);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetTrends", archiveLogic.Code);
                }
            }

            return new TrendBundle(cnlNums, 0);
        }

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        public Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime, int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetTrend(cnlNum, startTime, endTime);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetTrend", archiveLogic.Code);
                }
            }

            return new Trend(cnlNum, 0);
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public List<DateTime> GetTimestamps(DateTime startTime, DateTime endTime, int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetTimestamps(startTime, endTime);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetTimestamps", archiveLogic.Code);
                }
            }

            return new List<DateTime>();
        }

        /// <summary>
        /// Gets the slice of the specified input channels at the timestamp.
        /// </summary>
        public Slice GetSlice(int[] cnlNums, DateTime timestamp, int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetSlice(cnlNums, timestamp);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetSlice", archiveLogic.Code);
                }
            }

            return new Slice(timestamp, cnlNums, new CnlData[cnlNums.Length]);
        }

        /// <summary>
        /// Gets the time (UTC) when when the archive was last written to.
        /// </summary>
        public DateTime GetLastWriteTime(int archiveBit)
        {
            return GetArchive(archiveBit, out ArchiveLogic archiveLogic) ?
                archiveLogic.LastWriteTime : DateTime.MinValue;
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public Event GetEventByID(long eventID, int archiveBit)
        {
            if (GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetEventByID(eventID);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetEventByID", archiveLogic.Code);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public List<Event> GetEvents(DateTime startTime, DateTime endTime, DataFilter filter, int archiveBit)
        {
            if (GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetEvents(startTime, endTime, filter);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetEvents", archiveLogic.Code);
                }
            }

            return new List<Event>();
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(Event ev, int archiveMask)
        {
            for (int archiveBit = 0; archiveBit < ServerUtils.MaxArchiveCount; archiveBit++)
            {
                if (archiveMask.BitIsSet(archiveBit) && GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
                {
                    try
                    {
                        archiveLogic.WriteEvent(ev);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "WriteEvent", archiveLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public void AckEvent(long eventID, DateTime timestamp, int userID)
        {
            for (int archiveBit = 0; archiveBit < ServerUtils.MaxArchiveCount; archiveBit++)
            {
                if (GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
                {
                    try
                    {
                        archiveLogic.AckEvent(eventID, timestamp, userID);
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "AckEvent", archiveLogic.Code);
                    }
                }
            }
        }
    }
}
