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
 * Module   : ScadaServerEngine
 * Summary  : Holds archives classified by archive kinds
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Lang;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Holds archives classified by archive kinds.
    /// <para>Содержит архивы, классифицированные по видам.</para>
    /// </summary>
    internal class ArchiveHolder
    {
        private readonly ILog log;                                        // the application log
        private readonly List<ArchiveLogic> allArchives;                  // all the archives
        private readonly List<CurrentArchiveLogic> currentArchives;       // the current archives
        private readonly List<HistoricalArchiveLogic> historicalArchives; // the historical archives
        private readonly List<EventArchiveLogic> eventArchives;           // the event archives
        private readonly List<EventArchiveLogic> defaultEventArchives;    // the default event archives
        private readonly Dictionary<string, ArchiveLogic> archiveMap;     // the archives accessed by code
        private readonly ArchiveLogic[] arcByBit;                         // the archives accessed by bit number
        private int maxTitleLength;                                       // the maximum length of archive title


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            allArchives = new List<ArchiveLogic>();
            currentArchives = new List<CurrentArchiveLogic>();
            historicalArchives = new List<HistoricalArchiveLogic>();
            eventArchives = new List<EventArchiveLogic>();
            defaultEventArchives = new List<EventArchiveLogic>();
            archiveMap = new Dictionary<string, ArchiveLogic>();
            arcByBit = new ArchiveLogic[ServerUtils.MaxArchiveCount];
            maxTitleLength = 0;
            DefaultArchiveMask = 0;
        }


        /// <summary>
        /// Gets the archive mask that defines the default archives.
        /// </summary>
        public int DefaultArchiveMask { get; private set; }


        /// <summary>
        /// Sets the end time to the the current time if it is undefined.
        /// </summary>
        private void DefineEndTime(ref TimeRange timeRange)
        {
            if (timeRange.EndTime == DateTime.MinValue)
                timeRange.EndTime = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Checks if an archive with the specified code exists.
        /// </summary>
        public bool ArchiveExists(string code)
        {
            return archiveMap.ContainsKey(code);
        }

        /// <summary>
        /// Adds the specified archive to the lists.
        /// </summary>
        public void AddArchive(Archive archiveEntity, ArchiveLogic archiveLogic)
        {
            if (archiveEntity == null)
                throw new ArgumentNullException(nameof(archiveEntity));
            if (archiveLogic == null)
                throw new ArgumentNullException(nameof(archiveLogic));

            // add archive to the corresponding lists
            if (archiveMap.ContainsKey(archiveLogic.Code))
                throw new ScadaException("Archive already exists.");

            allArchives.Add(archiveLogic);
            archiveMap.Add(archiveLogic.Code, archiveLogic);

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

            // calculate maximum title length
            if (maxTitleLength < archiveLogic.Title.Length)
                maxTitleLength = archiveLogic.Title.Length;
        }

        /// <summary>
        /// Gets an archive by the specified bit number if the archive is ready for use.
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
        /// Appends information about the archives to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sb)
        {
            string header = Locale.IsRussian ?
                "Архивы (" + allArchives.Count + ")" :
                "Archives (" + allArchives.Count + ")";

            sb
                .AppendLine(header)
                .Append('-', header.Length).AppendLine();

            if (allArchives.Count > 0)
            {
                foreach (ArchiveLogic archiveLogic in allArchives)
                {
                    sb
                        .Append(archiveLogic.Title)
                        .Append(' ', maxTitleLength - archiveLogic.Title.Length)
                        .Append(" : ")
                        .AppendLine(archiveLogic.StatusText);
                }
            }
            else
            {
                sb.AppendLine(Locale.IsRussian ? "Архивов нет" : "No archives");
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
                    archiveLogic.MakeReady();
                    archiveLogic.IsReady = true;
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(MakeReady), archiveLogic.Code);
                }
            }
        }

        /// <summary>
        /// Calls the Close method of the archives.
        /// </summary>
        public void Close()
        {
            foreach (ArchiveLogic archiveLogic in allArchives)
            {
                try
                {
                    archiveLogic.Close();
                    archiveLogic.IsReady = false;
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(Close), archiveLogic.Code);
                }
            }
        }

        /// <summary>
        /// Gets the time (UTC) when the archive was last written to.
        /// </summary>
        public DateTime GetLastWriteTime(int archiveBit)
        {
            if (GetArchive(archiveBit, out ArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetLastWriteTime();
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetLastWriteTime), archiveLogic.Code);
                }
            }

            return DateTime.MinValue;
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
                        if (utcNow - archiveLogic.LastCleanupTime > archiveLogic.CleanupPeriod)
                        {
                            archiveLogic.LastCleanupTime = utcNow;
                            archiveLogic.DeleteOutdatedData();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInArchive,
                            nameof(DeleteOutdatedData), archiveLogic.Code);
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
                        archiveLogic.ReadData(curData, out bool completed);

                        if (completed)
                            return;
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(ReadCurrentData), archiveLogic.Code);
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
                        archiveLogic.WriteData(curData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(WriteCurrentData), archiveLogic.Code);
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
                        archiveLogic.ProcessData(curData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(ProcessData), archiveLogic.Code);
                    }
                }
            }

            foreach (HistoricalArchiveLogic archiveLogic in historicalArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.ProcessData(curData);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(ProcessData), archiveLogic.Code);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the EndUpdate method of the specified archive.
        /// </summary>
        public void EndUpdate(HistoricalArchiveLogic archiveLogic, UpdateContext updateContext)
        {
            try
            {
                updateContext.Stopwatch.Stop();
                archiveLogic.CurrentUpdateContext = null;
                archiveLogic.EndUpdate(updateContext);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(EndUpdate), archiveLogic.Code);
            }
        }

        /// <summary>
        /// Gets the trends of the specified channels.
        /// </summary>
        public TrendBundle GetTrends(int archiveBit, TimeRange timeRange, int[] cnlNums)
        {
            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));

            if (cnlNums.Length == 0)
                return new TrendBundle(cnlNums, 0);

            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    DefineEndTime(ref timeRange);
                    return archiveLogic.GetTrends(timeRange, cnlNums) ?? 
                        throw new ScadaException(ServerPhrases.NullResultNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetTrends), archiveLogic.Code);
                }
            }

            return new TrendBundle(cnlNums, 0);
        }

        /// <summary>
        /// Gets the trend of the specified channel.
        /// </summary>
        public Trend GetTrend(int archiveBit, TimeRange timeRange, int cnlNum)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    DefineEndTime(ref timeRange);
                    return archiveLogic.GetTrend(timeRange, cnlNum) ?? 
                        throw new ScadaException(ServerPhrases.NullResultNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetTrend), archiveLogic.Code);
                }
            }

            return new Trend(cnlNum, 0);
        }

        /// <summary>
        /// Gets the available timestamps.
        /// </summary>
        public List<DateTime> GetTimestamps(int archiveBit, TimeRange timeRange)
        {
            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    DefineEndTime(ref timeRange);
                    return archiveLogic.GetTimestamps(timeRange) ?? 
                        throw new ScadaException(ServerPhrases.NullResultNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetTimestamps), archiveLogic.Code);
                }
            }

            return new List<DateTime>();
        }

        /// <summary>
        /// Gets the slice of the specified channels at the timestamp.
        /// </summary>
        public Slice GetSlice(int archiveBit, DateTime timestamp, int[] cnlNums)
        {
            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));

            if (cnlNums.Length == 0)
                return new Slice(timestamp, cnlNums);

            if (GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetSlice(timestamp, cnlNums) ??
                        throw new ScadaException(ServerPhrases.NullResultNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetSlice), archiveLogic.Code);
                }
            }

            return new Slice(timestamp, cnlNums, new CnlData[cnlNums.Length]);
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public Event GetEventByID(int archiveBit, long eventID)
        {
            if (GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
            {
                try
                {
                    return archiveLogic.GetEventByID(eventID);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetEventByID), archiveLogic.Code);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public List<Event> GetEvents(int archiveBit, TimeRange timeRange, DataFilter filter)
        {
            if (GetArchive(archiveBit, out EventArchiveLogic archiveLogic))
            {
                try
                {
                    DefineEndTime(ref timeRange);
                    return archiveLogic.GetEvents(timeRange, filter) ?? 
                        throw new ScadaException(ServerPhrases.NullResultNotAllowed);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(GetEvents), archiveLogic.Code);
                }
            }

            return new List<Event>();
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(int archiveMask, Event ev)
        {
            void DoWriteEvent(EventArchiveLogic archiveLogic)
            {
                try
                {
                    archiveLogic.WriteEvent(ev);
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(WriteEvent), archiveLogic.Code);
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
        public void AckEvent(EventAck eventAck)
        {
            foreach (EventArchiveLogic archiveLogic in eventArchives)
            {
                if (archiveLogic.IsReady)
                {
                    try
                    {
                        archiveLogic.AckEvent(eventAck);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, ServerPhrases.ErrorInArchive, nameof(AckEvent), archiveLogic.Code);
                    }
                }
            }
        }
    }
}
