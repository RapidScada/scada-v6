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
 * Module   : ModArcBasic
 * Summary  : Implements the event archive logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Modules.ModArcBasic.Logic.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Scada.Server.Modules.ModArcBasic.Logic
{
    /// <summary>
    /// Implements the event archive logic.
    /// <para>Реализует логику архива событий.</para>
    /// </summary>
    internal class BasicEAL : EventArchiveLogic
    {
        private readonly BasicEAO archiveOptions;   // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly EventTableAdapter adapter; // reads and writes events
        private readonly string archivePath;        // the full path of the archive
        private readonly MemoryCache<DateTime, EventTable> tableCache; // the cache containing event tables

        private EventTable currentTable; // the today's event table
        private EventTable lastTable;    // the last accessed event table


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicEAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums)
            : base(archiveContext, archiveConfig, cnlNums)
        {
            archiveOptions = new BasicEAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = archiveOptions.LogEnabled ? CreateLog(ModUtils.ModCode) : null;
            stopwatch = new Stopwatch();
            adapter = new EventTableAdapter();
            archivePath = Path.Combine(archiveContext.AppConfig.PathOptions.GetArcDir(archiveOptions.IsCopy), Code);
            tableCache = new MemoryCache<DateTime, EventTable>(ModUtils.CacheExpiration, ModUtils.CacheCapacity);

            currentTable = null;
            lastTable = null;
        }


        /// <summary>
        /// Gets the event table from the cache, creating a table if necessary.
        /// </summary>
        private EventTable GetEventTable(DateTime timestamp)
        {
            DateTime tableDate = timestamp.Date;

            if (currentTable != null && currentTable.TableDate == tableDate)
            {
                RefreshEvents(currentTable);
                return currentTable;
            }
            else if (lastTable != null && lastTable.TableDate == tableDate)
            {
                RefreshEvents(lastTable);
                return lastTable;
            }
            else
            {
                EventTable eventTable = tableCache.Get(tableDate);

                if (eventTable == null)
                {
                    eventTable = new EventTable(tableDate);
                    tableCache.Add(tableDate, eventTable);

                    if (tableDate == DateTime.UtcNow.Date)
                        currentTable = eventTable;
                }

                lastTable = eventTable;
                RefreshEvents(eventTable);
                return eventTable;
            }
        }

        /// <summary>
        /// Loads new events from a file if the file has changed.
        /// </summary>
        private void RefreshEvents(EventTable eventTable)
        {
            if (string.IsNullOrEmpty(eventTable.FileName))
            {
                eventTable.FileName = Path.Combine(archivePath, 
                    EventTableAdapter.GetTableFileName(Code, eventTable.TableDate));
            }

            DateTime fileAge = File.Exists(eventTable.FileName) ? 
                File.GetLastWriteTimeUtc(eventTable.FileName) : DateTime.MinValue;

            if (fileAge > DateTime.MinValue && fileAge != eventTable.FileAge)
            {
                stopwatch.Restart();
                adapter.FileName = eventTable.FileName;
                adapter.Fill(eventTable);
                eventTable.FileAge = fileAge;

                stopwatch.Stop();
                arcLog?.WriteAction(Locale.IsRussian ?
                    "Чтение таблицы событий длины {0} успешно завершено за {1} мс" :
                    "Reading an event table of length {0} completed successfully in {1} ms",
                    eventTable.Events.Count, stopwatch.ElapsedMilliseconds);
            }
        }

        
        /// <summary>
        /// Makes the archive ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            Directory.CreateDirectory(archivePath);
            GetEventTable(DateTime.UtcNow); // preload the current table
        }

        /// <summary>
        /// Deletes the outdated data from the archive.
        /// </summary>
        public override void DeleteOutdatedData()
        {
            DirectoryInfo arcDirInfo = new DirectoryInfo(archivePath);

            if (arcDirInfo.Exists)
            {
                DateTime minDT = DateTime.UtcNow.AddDays(-archiveOptions.StoragePeriod);
                string minFileName = EventTableAdapter.GetTableFileName(Code, minDT);

                appLog.WriteAction(Locale.IsRussian ?
                    "Удаление устаревших событий из архива {0}, которые старше {1}" :
                    "Delete outdated events from the {0} archive older than {1}",
                    Code, minDT.ToLocalizedDateString());

                foreach (FileInfo fileInfo in
                    arcDirInfo.EnumerateFiles(Code + "*", SearchOption.TopDirectoryOnly))
                {
                    if (string.CompareOrdinal(fileInfo.Name, minFileName) < 0)
                        fileInfo.Delete();
                }
            }
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public override Event GetEventByID(long eventID)
        {
            EventTable eventTable = GetEventTable(ScadaUtils.RetrieveTimeFromID(eventID));
            return eventTable.GetEventByID(eventID);
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public override List<Event> GetEvents(DateTime startTime, DateTime endTime, bool endInclusive, 
            DataFilter filter)
        {
            // simple cases
            List<DateTime> dates = new List<DateTime>(EnumerateDates(startTime, endTime, endInclusive));

            if (dates.Count == 0)
                return new List<Event>();

            if (dates.Count == 1)
            {
                EventTable eventTable = GetEventTable(dates[0]);
                return new List<Event>(eventTable.SelectEvents(startTime, endTime, endInclusive, filter));
            }

            // full case
            List<Event> events = new List<Event>();

            if (filter == null)
            {
                foreach (DateTime date in dates)
                {
                    EventTable eventTable = GetEventTable(date);
                    events.AddRange(eventTable.SelectEvents(startTime, endTime, endInclusive, null));
                }
            }
            else
            {
                int limit = filter.Limit > 0 ? filter.Limit : int.MaxValue;
                int selectedCount = 0;
                int addedCount = 0;

                DataFilter filterCopy = filter.ShallowCopy();
                filterCopy.Limit = 0;
                filterCopy.Offset = 0;

                if (!filter.OriginBegin)
                    dates.Reverse();

                foreach (DateTime date in dates)
                {
                    EventTable eventTable = GetEventTable(date);

                    foreach (Event ev in eventTable.SelectEvents(startTime, endTime, endInclusive, filterCopy))
                    {
                        if (++selectedCount > filter.Offset)
                        {
                            events.Add(ev);
                            addedCount++;

                            if (addedCount >= limit)
                                break;
                        }
                    }

                    if (addedCount >= limit)
                        break;
                }

                if (!filter.OriginBegin)
                    events.Reverse();
            }

            return events;
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public override void WriteEvent(Event ev)
        {
            EventTable eventTable = GetEventTable(ev.Timestamp);

            if (eventTable.AddEvent(ev))
            {
                stopwatch.Restart();
                adapter.FileName = eventTable.FileName;
                adapter.AppendEvent(ev);
                eventTable.FileAge = File.GetLastWriteTimeUtc(eventTable.FileName);

                stopwatch.Stop();
                arcLog?.WriteAction(Locale.IsRussian ?
                    "Запись события успешно завершена за {0} мс" :
                    "Event writing completed successfully in {0} ms",
                    stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public override void AckEvent(long eventID, DateTime timestamp, int userID)
        {
            EventTable eventTable = GetEventTable(ScadaUtils.RetrieveTimeFromID(eventID));
            Event ev = eventTable.GetEventByID(eventID);

            if (ev != null)
            {
                stopwatch.Restart();
                ev.Ack = true;
                ev.AckTimestamp = timestamp;
                ev.AckUserID = userID;

                adapter.FileName = eventTable.FileName;
                adapter.WriteEventAck(ev);
                eventTable.FileAge = File.GetLastWriteTimeUtc(eventTable.FileName);

                stopwatch.Stop();
                arcLog?.WriteAction(Locale.IsRussian ?
                    "Квитирование события успешно завершено за {0} мс" :
                    "Event acknowledgment completed successfully in {0} ms",
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
