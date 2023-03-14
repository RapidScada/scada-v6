// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Adapters;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules.ModArcBasic.Config;
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
        private readonly ModuleConfig moduleConfig; // the module configuration
        private readonly BasicEAO options;          // the archive options
        private readonly ILog appLog;               // the application log
        private readonly ILog arcLog;               // the archive log
        private readonly Stopwatch stopwatch;       // measures the time of operations
        private readonly EventTableAdapter adapter; // reads and writes events
        private readonly MemoryCache<DateTime, EventTable> tableCache; // the cache containing event tables
        private readonly object archiveLock;        // synchronizes access to the archive

        private string archivePath;      // the full path of the archive
        private EventTable currentTable; // the today's event table
        private EventTable lastTable;    // the last accessed event table


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BasicEAL(IArchiveContext archiveContext, ArchiveConfig archiveConfig, int[] cnlNums, 
            ModuleConfig moduleConfig) : base(archiveContext, archiveConfig, cnlNums)
        {
            this.moduleConfig = moduleConfig ?? throw new ArgumentNullException(nameof(moduleConfig));
            options = new BasicEAO(archiveConfig.CustomOptions);
            appLog = archiveContext.Log;
            arcLog = options.LogEnabled ? CreateLog(ModuleUtils.ModuleCode) : null;
            stopwatch = new Stopwatch();
            adapter = new EventTableAdapter();
            tableCache = new MemoryCache<DateTime, EventTable>(ModuleUtils.CacheExpiration, ModuleUtils.CacheCapacity);
            archiveLock = new object();

            archivePath = "";
            currentTable = null;
            lastTable = null;
        }


        /// <summary>
        /// Gets the archive options.
        /// </summary>
        protected override EventArchiveOptions ArchiveOptions => options;


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

            DateTime lastWriteTime = File.Exists(eventTable.FileName) ? 
                File.GetLastWriteTimeUtc(eventTable.FileName) : DateTime.MinValue;

            if (lastWriteTime > DateTime.MinValue && lastWriteTime != eventTable.LastWriteTime)
            {
                stopwatch.Restart();
                adapter.FileName = eventTable.FileName;
                adapter.Fill(eventTable);
                eventTable.LastWriteTime = lastWriteTime;

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
            archivePath = Path.Combine(moduleConfig.SelectArcDir(options.UseCopyDir), Code);
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
                lock (archiveLock)
                {
                    DateTime minDT = DateTime.UtcNow.AddDays(-options.Retention);
                    string minFileName = EventTableAdapter.GetTableFileName(Code, minDT);
                    appLog.WriteAction(ServerPhrases.DeleteOutdatedData, Code, minDT.ToLocalizedDateString());

                    foreach (FileInfo fileInfo in
                        arcDirInfo.EnumerateFiles(Code + "*", SearchOption.TopDirectoryOnly))
                    {
                        if (string.CompareOrdinal(fileInfo.Name, minFileName) < 0)
                            fileInfo.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the event by ID.
        /// </summary>
        public override Event GetEventByID(long eventID)
        {
            lock (archiveLock)
            {
                stopwatch.Restart();
                EventTable eventTable = GetEventTable(ScadaUtils.RetrieveTimeFromID(eventID));
                Event ev = eventTable.GetEventByID(eventID);
                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingEventCompleted, stopwatch.ElapsedMilliseconds);
                return ev;
            }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public override List<Event> GetEvents(TimeRange timeRange, DataFilter filter)
        {
            lock (archiveLock)
            {
                stopwatch.Restart();
                List<Event> events;
                List<DateTime> dates = new List<DateTime>(EnumerateDates(timeRange));

                // simple cases first
                if (dates.Count == 0)
                {
                    events = new List<Event>();
                }
                else if (dates.Count == 1)
                {
                    EventTable eventTable = GetEventTable(dates[0]);
                    events = new List<Event>(eventTable.SelectEvents(timeRange, filter));
                }
                else
                {
                    // full case
                    events = new List<Event>();

                    if (filter == null)
                    {
                        foreach (DateTime date in dates)
                        {
                            EventTable eventTable = GetEventTable(date);
                            events.AddRange(eventTable.SelectEvents(timeRange, null));
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

                            foreach (Event ev in eventTable.SelectEvents(timeRange, filterCopy))
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
                }

                stopwatch.Stop();
                arcLog?.WriteAction(ServerPhrases.ReadingEventsCompleted, events.Count, stopwatch.ElapsedMilliseconds);
                return events;
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public override void WriteEvent(Event ev)
        {
            if (TimeInsideRetention(ev.Timestamp, DateTime.UtcNow))
            {
                lock (archiveLock)
                {
                    EventTable eventTable = GetEventTable(ev.Timestamp);
                    stopwatch.Restart();
                    adapter.FileName = eventTable.FileName;

                    if (eventTable.AddEvent(ev))
                        adapter.AppendEvent(ev); // write new event
                    else if (ev.Ack)
                        adapter.WriteEventAck(ev); // update acknowledgement

                    eventTable.LastWriteTime = File.GetLastWriteTimeUtc(eventTable.FileName);
                    LastWriteTime = eventTable.LastWriteTime;
                    stopwatch.Stop();
                    arcLog?.WriteAction(ServerPhrases.WritingEventCompleted, stopwatch.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public override void AckEvent(EventAck eventAck)
        {
            lock (archiveLock)
            {
                EventTable eventTable = GetEventTable(ScadaUtils.RetrieveTimeFromID(eventAck.EventID));
                Event ev = eventTable.GetEventByID(eventAck.EventID);

                if (ev != null)
                {
                    stopwatch.Restart();
                    ev.Ack = true;
                    ev.AckTimestamp = eventAck.Timestamp;
                    ev.AckUserID = eventAck.UserID;

                    adapter.FileName = eventTable.FileName;
                    adapter.WriteEventAck(ev);
                    eventTable.LastWriteTime = File.GetLastWriteTimeUtc(eventTable.FileName);
                    LastWriteTime = eventTable.LastWriteTime;

                    stopwatch.Stop();
                    arcLog?.WriteAction(ServerPhrases.AckEventCompleted, stopwatch.ElapsedMilliseconds);
                }
            }
        }
    }
}
