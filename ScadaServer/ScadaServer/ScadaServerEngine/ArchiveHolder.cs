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

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Archives;
using System;
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

        private static readonly string NullNotAllowed = Locale.IsRussian ?
            "Результат метода не может быть null." :
            "Method result must not be null.";

        private readonly ILog log;                                        // the application log
        private readonly List<ArchiveLogic> allArchives;                  // the all archives
        private readonly List<CurrentArchiveLogic> currentArchives;       // the current archives
        private readonly List<HistoricalArchiveLogic> historicalArchives; // the historical archives
        private readonly List<EventArchiveLogic> eventArchives;           // the event archives
        private readonly List<EventArchiveLogic> defaultEventArchives;    // the default event archives
        private readonly ArchiveLogic[] arcByBit;                         // the archives accessed by bit number


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            allArchives = new List<ArchiveLogic>();
            currentArchives = new List<CurrentArchiveLogic>();
            historicalArchives = new List<HistoricalArchiveLogic>();
            eventArchives = new List<EventArchiveLogic>();
            defaultEventArchives = new List<EventArchiveLogic>();
            arcByBit = new ArchiveLogic[ServerUtils.MaxArchiveCount];
            DefaultArchiveMask = 0;
        }


        /// <summary>
        /// Gets the archive mask that defines the default archives.
        /// </summary>
        public int DefaultArchiveMask { get; private set; }


        /// <summary>
        /// Returns the specified time itself or the current time if the specified time is undefined.
        /// </summary>
        private DateTime DefineTime(DateTime dateTime)
        {
            return dateTime == DateTime.MinValue ? DateTime.UtcNow : dateTime;
        }

        /// <summary>
        /// Adds the specified archive to the lists.
        /// </summary>
        public void AddArchive(Archive archiveEntity, ArchiveLogic archiveLogic)
        {
            if (archiveEntity == null)
                throw new ArgumentNullException("archiveEntity");
            if (archiveLogic == null)
                throw new ArgumentNullException("archiveLogic");

            // add archive to the corresponding list
            allArchives.Add(archiveLogic);

            if (archiveLogic is CurrentArchiveLogic currentArchiveLogic)
            {
                currentArchives.Add(currentArchiveLogic);
            }
            else if (archiveLogic is HistoricalArchiveLogic historicalArchiveLogic)
            {
                historicalArchives.Add(historicalArchiveLogic);
            }
            else if (archiveLogic is EventArchiveLogic eventArchiveLogic)
            {
                eventArchives.Add(eventArchiveLogic);

                if (archiveEntity.IsDefault)
                    defaultEventArchives.Add(eventArchiveLogic);
            }

            // add archive to array by bit number
            int archiveBit = archiveEntity.Bit;

            if (0 <= archiveBit && archiveBit < ServerUtils.MaxArchiveCount)
                arcByBit[archiveBit] = archiveLogic;
            else
                throw new ScadaException("Archive bit is out of range.");

            // build default archive mask
            if (archiveEntity.IsDefault)
                DefaultArchiveMask = DefaultArchiveMask.SetBit(archiveEntity.Bit, true);
        }

        /// <summary>
        /// Gets an archive by the specified bit number.
        /// </summary>
        public bool GetArchive<T>(int archiveBit, out T archiveLogic) where T : ArchiveLogic
        {
            T archive = 0 <= archiveBit && archiveBit < ServerUtils.MaxArchiveCount ?
                arcByBit[archiveBit] as T : null;

            if (archive != null && archive.IsReady)
            {
                archiveLogic = archive;
                return true;
            }
            else
            {
                archiveLogic = null;
                return false;
            }
        }

        /// <summary>
        /// Calls the MakeReady method of the archives.
        /// </summary>
        public void MakeReady()
        {
            foreach (ArchiveLogic archiveLogic in allArchives)
            {
                try
                {
                    archiveLogic.Lock();
                    archiveLogic.MakeReady();
                    archiveLogic.IsReady = true;
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "MakeReady", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }
        }

        /// <summary>
        /// Calls the DeleteOutdatedData method of the archives.
        /// </summary>
        public void DeleteOutdatedData()
        {
            DateTime utcNow = DateTime.UtcNow;

            foreach (ArchiveLogic archiveLogic in allArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.Lock();

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
                    finally
                    {
                        Unlock(archiveLogic);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the ReadData method of the current archives.
        /// </summary>
        public void ReadCurrentData(ICurrentData curData)
        {
            foreach (CurrentArchiveLogic archiveLogic in currentArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.Lock();
                        archiveLogic.ReadData(curData, out bool completed);

                        if (completed)
                            return;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "ReadData", archiveLogic.Code);
                    }
                    finally
                    {
                        Unlock(archiveLogic);
                    }
                }
            }

            log.WriteError(Locale.IsRussian ?
                "Не удалось считать текущие данные" :
                "Unable to read current data");
        }

        /// <summary>
        /// Calls the WriteData method of the current archives.
        /// </summary>
        public void WriteCurrentData(ICurrentData curData)
        {
            foreach (CurrentArchiveLogic archiveLogic in currentArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.Lock();
                        archiveLogic.WriteData(curData);
                        archiveLogic.LastWriteTime = curData.Timestamp;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "WriteData", archiveLogic.Code);
                    }
                    finally
                    {
                        Unlock(archiveLogic);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the ProcessData method of the current and historical archives.
        /// </summary>
        public void ProcessData(ICurrentData curData)
        {
            foreach (CurrentArchiveLogic archiveLogic in currentArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.Lock();
                        if (archiveLogic.ProcessData(curData))
                            archiveLogic.LastWriteTime = curData.Timestamp;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "ProcessData", archiveLogic.Code);
                    }
                    finally
                    {
                        Unlock(archiveLogic);
                    }
                }
            }

            foreach (HistoricalArchiveLogic archiveLogic in historicalArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.Lock();
                        if (archiveLogic.ProcessData(curData))
                            archiveLogic.LastWriteTime = curData.Timestamp;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "ProcessData", archiveLogic.Code);
                    }
                    finally
                    {
                        Unlock(archiveLogic);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the EndUpdate method of the specified archive.
        /// </summary>
        public void EndUpdate(HistoricalArchiveLogic archiveLogic, int deviceNum, DateTime timestamp)
        {
            try
            {
                archiveLogic.Lock();
                archiveLogic.EndUpdate(deviceNum, timestamp);
                archiveLogic.LastWriteTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, ErrorInArchive, "EndUpdate", archiveLogic.Code);
            }
            finally
            {
                Unlock(archiveLogic);
            }
        }

        /// <summary>
        /// Calls the Unlock method of the specified archive.
        /// </summary>
        public void Unlock(ArchiveLogic archiveLogic)
        {
            try
            {
                archiveLogic.Unlock();
            }
            catch (Exception ex)
            {
                log.WriteException(ex, ErrorInArchive, "Unlock", archiveLogic.Code);
            }
        }

        /// <summary>
        /// Gets the trends of the specified input channels.
        /// </summary>
        public TrendBundle GetTrends(int[] cnlNums, DateTime startTime, DateTime endTime, bool endInclusive,
            int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    archiveLogic.Lock();
                    return archiveLogic.GetTrends(cnlNums, startTime, DefineTime(endTime), endInclusive) ??
                        throw new ScadaException(NullNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetTrends", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }

            return new TrendBundle(cnlNums, 0);
        }

        /// <summary>
        /// Gets the trend of the specified input channel.
        /// </summary>
        public Trend GetTrend(int cnlNum, DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    archiveLogic.Lock();
                    return archiveLogic.GetTrend(cnlNum, startTime, endTime, endInclusive) ??
                        throw new ScadaException(NullNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetTrend", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }

            return new Trend(cnlNum, 0);
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public List<DateTime> GetTimestamps(DateTime startTime, DateTime endTime, bool endInclusive, int archiveBit)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    archiveLogic.Lock();
                    return archiveLogic.GetTimestamps(startTime, DefineTime(endTime), endInclusive) ??
                        throw new ScadaException(NullNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetTimestamps", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
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
                    archiveLogic.Lock();
                    return archiveLogic.GetSlice(cnlNums, timestamp) ??
                        throw new ScadaException(NullNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetSlice", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }

            return new Slice(timestamp, cnlNums, new CnlData[cnlNums.Length]);
        }

        /// <summary>
        /// Gets the time (UTC) when when the archive was last written to.
        /// </summary>
        public DateTime GetLastWriteTime(int archiveBit)
        {
            if (GetArchive(archiveBit, out ArchiveLogic archiveLogic))
            {
                try
                {
                    archiveLogic.Lock();
                    return archiveLogic.LastWriteTime;
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }
            else
            {
                return DateTime.MinValue;
            }
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
                    archiveLogic.Lock();
                    return archiveLogic.GetEventByID(eventID);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetEventByID", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public List<Event> GetEvents(DateTime startTime, DateTime endTime, bool endInclusive,
            DataFilter filter, int archiveBit)
        {
            if (GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
            {
                try
                {
                    archiveLogic.Lock();
                    return archiveLogic.GetEvents(startTime, DefineTime(endTime), endInclusive, filter) ??
                        throw new ScadaException(NullNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "GetEvents", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }

            return new List<Event>();
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(Event ev, int archiveMask)
        {
            void DoWriteEvent(EventArchiveLogic archiveLogic)
            {
                try
                {
                    archiveLogic.Lock();
                    archiveLogic.WriteEvent(ev);
                    archiveLogic.LastWriteTime = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    log.WriteException(ex, ErrorInArchive, "WriteEvent", archiveLogic.Code);
                }
                finally
                {
                    Unlock(archiveLogic);
                }
            }

            if (archiveMask == ArchiveMask.Default)
            {
                foreach (EventArchiveLogic archiveLogic in defaultEventArchives)
                {
                    if (archiveLogic.IsReady)
                        DoWriteEvent(archiveLogic);
                }
            }
            else
            {
                for (int archiveBit = 0; archiveBit < ServerUtils.MaxArchiveCount; archiveBit++)
                {
                    if (archiveMask.BitIsSet(archiveBit) &&
                        GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
                    {
                        DoWriteEvent(archiveLogic);
                    }
                }
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public void AckEvent(long eventID, DateTime timestamp, int userID)
        {
            foreach (EventArchiveLogic archiveLogic in eventArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.Lock();
                        archiveLogic.AckEvent(eventID, timestamp, userID);
                        archiveLogic.LastWriteTime = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, ErrorInArchive, "AckEvent", archiveLogic.Code);
                    }
                    finally
                    {
                        Unlock(archiveLogic);
                    }
                }
            }
        }
    }
}
