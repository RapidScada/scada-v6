// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Lang;
using Scada.Server.Modules.ModDbExport.Config;
using Scada.Server.Modules.ModDbExport.Logic.Queries;
using System.Text;

namespace Scada.Server.Modules.ModDbExport.Logic.Replication
{
    /// <summary>
    /// Replicates archives to a target database.
    /// <para>Реплицирует архивы на целевую базу данных.</para>
    /// </summary>
    internal class ArcReplicator
    {
        /// <summary>
        /// The size of the slices created by the replicator.
        /// </summary>
        private const int SliceSize = 1000;
        /// <summary>
        /// The maximum size of the task queue.
        /// </summary>
        public const int MaxTaskQueueSize = 10;

        private readonly IServerContext serverContext;     // the application context
        private readonly IExporterContext exporterContext; // the exporter context
        private readonly ArcReplicationOptions options;    // the replication options
        private readonly string stateFileName;             // the short name of the replication state file
        private readonly ArcExportTask autoTask;           // the task of automatic replication
        private readonly Queue<ArcExportTask> taskQueue;   // the task queue
        private readonly bool histDataEnabled;             // indicates if historical data is exported
        private readonly bool eventsEnabled;               // indicates if events are exported

        private bool replicatorIsReady;                    // indicates that the replicator is ready
        private bool histDataQueueIsReady;                 // indicates that the historical data queue is ready
        private bool eventQueueIsReady;                    // indicates that the event queue is ready
        private List<CnlNumGroup> cnlNumGroups;            // the groups of channel numbers
        private ArcExportTask displayTask;                 // the task those status is displayed


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcReplicator(IServerContext serverContext, IExporterContext exporterContext)
        {
            this.serverContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));
            this.exporterContext = exporterContext ?? throw new ArgumentNullException(nameof(exporterContext));
            options = exporterContext.ExporterConfig.ExportOptions.ArcReplicationOptions;
            stateFileName = exporterContext.FilePrefix + "_State.xml";
            autoTask = options.AutoExport ? CreateAutoTask() : null;
            taskQueue = new Queue<ArcExportTask>();
            histDataEnabled = exporterContext.ClassifiedQueries.HistDataQueries.Count > 0;
            eventsEnabled = exporterContext.ClassifiedQueries.EventQueries.Count > 0;

            replicatorIsReady = false;
            histDataQueueIsReady = true;
            eventQueueIsReady = true;
            cnlNumGroups = null;
            displayTask = null;

            exporterContext.HistDataQueue.Empty += HistDataQueue_Empty;
            exporterContext.EventQueue.Empty += EventQueue_Empty;
        }


        /// <summary>
        /// Creates an automatic replication task.
        /// </summary>
        private ArcExportTask CreateAutoTask()
        {
            DateTime utcNow = DateTime.UtcNow;

            return new ArcExportTask
            {
                Name = Locale.IsRussian ? "Авто репликация" : "Auto replication",
                StartDT = utcNow.AddSeconds(-options.MaxDepth),
                EndDT = utcNow.AddSeconds(-options.MinDepth)
            };
        }

        /// <summary>
        /// Validates the replication options.
        /// </summary>
        private bool ValidateOptions()
        {
            StringBuilder sbError = new();

            if (options.MinDepth <= 0)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Минимальная глубина должна быть положительной." :
                    "* Minimum depth must be positive.");
            }

            if (options.MaxDepth <= 0)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Максимальная глубина должна быть положительной." :
                    "* Maximum depth must be positive.");
            }

            if (options.MinDepth >= options.MaxDepth)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Минимальная глубина должна быть меньше максимальной." :
                    "* Minimum depth must be less than maximum depth.");
            }

            if (options.ReadingStep <= 0)
            {
                sbError.AppendLine(Locale.IsRussian ?
                    "* Шаг чтения должен быть положительным." :
                    "* Reading step must be positive.");
            }

            if (sbError.Length > 0)
            {
                exporterContext.ExporterLog.WriteError((Locale.IsRussian ?
                    "Некорректные параметры репликации:" :
                    "Invalid replication options:") + Environment.NewLine + sbError);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes the groups of channel numbers.
        /// </summary>
        private void InitCnlNumGroups()
        {
            cnlNumGroups = new List<CnlNumGroup>();
            IEnumerable<int> histCnlNums =
                exporterContext.ClassifiedQueries.HistDataQueries.All(q => q.CnlNumFilter.Count > 0)
                ? exporterContext.ClassifiedQueries.HistDataQueries.SelectMany(q => q.CnlNumFilter)
                .Distinct().OrderBy(n => n)
                : serverContext.Cnls.ArcCnls.Keys;

            // create groups for non-single queries
            CnlNumGroup cnlNumGroup = new(SliceSize) { SingleQuery = false };
            int cnlIdx = 0;

            foreach (int cnlNum in histCnlNums)
            {
                cnlNumGroup.CnlNums[cnlIdx] = cnlNum;
                cnlIdx++;

                if (cnlIdx == SliceSize)
                {
                    cnlIdx = 0;
                    cnlNumGroups.Add(cnlNumGroup);
                    cnlNumGroup = new CnlNumGroup(SliceSize) { SingleQuery = false };
                }
            }

            if (cnlIdx > 0)
            {
                cnlNumGroup.Resize(cnlIdx);
                cnlNumGroups.Add(cnlNumGroup);
            }

            // create groups for single queries
            foreach (DataQuery query in exporterContext.ClassifiedQueries.HistDataQueries
                .Where(q => q.Options.SingleQuery && q.Filter.CnlNums.Count > 0))
            {
                cnlNumGroup = new(query.Filter.CnlNums.Count) { QueryID = query.QueryID };
                query.Filter.CnlNums.CopyTo(cnlNumGroup.CnlNums);
                cnlNumGroups.Add(cnlNumGroup);
            }

            if (cnlNumGroups.Count == 0)
            {
                exporterContext.ExporterLog.WriteError(Locale.IsRussian ?
                    "Отсутствуют каналы для репликации архивов" :
                    "Channels for archive replication missing");
            }
        }

        /// <summary>
        /// Selects a task to execute.
        /// </summary>
        private ArcExportTask SelectTask()
        {
            // select a task from the queue
            if (autoTask == null || !autoTask.State.StepInProgress)
            {
                lock (taskQueue)
                {
                    if (taskQueue.Count > 0)
                        return taskQueue.Peek();
                }
            }

            if (autoTask == null)
                return null;

            // update the auto task
            if (!autoTask.State.StepInProgress)
            {
                DateTime utcNow = DateTime.UtcNow;
                autoTask.StartDT = utcNow.AddSeconds(-options.MaxDepth);
                autoTask.EndDT = utcNow.AddSeconds(-options.MinDepth);
            }

            return autoTask.State.PositionDT.AddSeconds(options.ReadingStep) < autoTask.EndDT ? autoTask : null;
        }

        /// <summary>
        /// Removes the task from the queue.
        /// </summary>
        private void CloseTask(ArcExportTask task)
        {
            lock (taskQueue)
            {
                if (taskQueue.Count > 0 && taskQueue.Peek() == task)
                    taskQueue.Dequeue();
            }
        }

        /// <summary>
        /// Saves the state of the auto task.
        /// </summary>
        private void SaveAutoTaskState()
        {
            if (autoTask != null && autoTask.State.Modified)
            {
                if (autoTask.State.Save(serverContext.Storage, stateFileName, out string errMsg))
                    autoTask.State.Modified = false;
                else
                    exporterContext.ExporterLog.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Exports historical data for the specified time range.
        /// </summary>
        private bool ExportHistoricalData(TimeRange timeRange, CnlNumGroup cnlNumGroup)
        {
            TrendBundle trendBundle = serverContext.GetTrends(options.HistArchiveBit, timeRange, cnlNumGroup.CnlNums);
            DateTime creationTime = DateTime.UtcNow;

            for (int tmIdx = 0, tmCnt = trendBundle.Timestamps.Count; tmIdx < tmCnt; tmIdx++)
            {
                int cnlCnt = cnlNumGroup.CnlNums.Length;
                CnlData[] cnlData = new CnlData[cnlCnt];
                Slice slice = new(trendBundle.Timestamps[tmIdx], cnlNumGroup.CnlNums, cnlData);
                SliceItem sliceItem = new(slice) 
                { 
                    QueryID = cnlNumGroup.QueryID, 
                    SingleQuery = cnlNumGroup.SingleQuery 
                };

                for (int cnlIdx = 0; cnlIdx < cnlCnt; cnlIdx++)
                {
                    cnlData[cnlIdx] = trendBundle.Trends[cnlIdx][tmIdx];
                }

                histDataQueueIsReady = false;

                if (!exporterContext.HistDataQueue.Enqueue(creationTime, sliceItem, out string errMsg))
                {
                    exporterContext.ExporterLog.WriteError(errMsg);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Exports events for the specified time range.
        /// </summary>
        private bool ExportEvents(TimeRange timeRange)
        {
            List<Event> events = serverContext.GetEvents(options.EventArchiveBit, timeRange, null);
            DateTime creationTime = DateTime.UtcNow;

            foreach (Event ev in events)
            {
                eventQueueIsReady = false;

                if (!exporterContext.EventQueue.Enqueue(creationTime, ev, out string errMsg))
                {
                    exporterContext.ExporterLog.WriteError(errMsg);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Corrects the replication position by updating the time as a multiple of the period.
        /// </summary>
        private static DateTime CorrectPosition(DateTime positionDT, int period)
        {
            return period > 0
                ? positionDT.Date.AddSeconds((int)positionDT.TimeOfDay.TotalSeconds / period * period)
                : positionDT;
        }

        /// <summary>
        /// Handles a queue empty event for the historical data queue.
        /// </summary>
        private void HistDataQueue_Empty(object sender, EventArgs e)
        {
            histDataQueueIsReady = true;

            if (eventQueueIsReady) // both queues are ready
                SaveAutoTaskState();
        }

        /// <summary>
        /// Handles a queue empty event for the event queue.
        /// </summary>
        private void EventQueue_Empty(object sender, EventArgs e)
        {
            eventQueueIsReady = true;

            if (histDataQueueIsReady)
                SaveAutoTaskState();
        }


        /// <summary>
        /// Initializes the replicator.
        /// </summary>
        public void Init()
        {
            if (ValidateOptions())
            {
                replicatorIsReady = true;

                if (histDataEnabled)
                    InitCnlNumGroups();

                if (autoTask != null)
                {
                    if (!autoTask.State.Load(serverContext.Storage, stateFileName, out string errMsg))
                        exporterContext.ExporterLog.WriteError(errMsg);

                    autoTask.State.PositionDT = autoTask.State.PositionDT < autoTask.StartDT
                        ? CorrectPosition(autoTask.StartDT, options.ReadingStep)
                        : CorrectPosition(autoTask.State.PositionDT, options.ReadingStep);
                }
            }
        }

        /// <summary>
        /// Replicates data to a target database.
        /// </summary>
        public void ReplicateData()
        {
            if (!replicatorIsReady)
                return;

            ArcExportTask task = null;
            displayTask = null;

            try
            {
                task = SelectTask();
                displayTask = task ?? autoTask;

                if (task != null)
                {
                    ArcReplicationState taskState = task.State;
                    bool transferOK = false;
                    bool transferCanceled = false;
                    bool stepCompleted = false;

                    // start new step
                    if (!taskState.StepInProgress)
                    {
                        taskState.StepInProgress = true;
                        taskState.StepTimeRange = new TimeRange(
                            taskState.PositionDT,
                            taskState.PositionDT.AddSeconds(options.ReadingStep), false);

                        if (cnlNumGroups.Count == 0)
                            taskState.ArchiveIndex = 1;
                    }

                    // export data
                    if (!histDataQueueIsReady || !eventQueueIsReady)
                    {
                        transferCanceled = true;
                    }
                    else if (taskState.ArchiveIndex == 0)
                    {
                        if (!histDataEnabled || 
                            ExportHistoricalData(taskState.StepTimeRange, cnlNumGroups[taskState.GroupIndex]))
                        {
                            transferOK = true;

                            if (++taskState.GroupIndex == cnlNumGroups.Count)
                            {
                                taskState.GroupIndex = 0;
                                taskState.ArchiveIndex = 1;
                            }
                        }
                    }
                    else if (!eventsEnabled || 
                        ExportEvents(taskState.StepTimeRange))
                    {
                        transferOK = true;
                        stepCompleted = true;
                        taskState.ArchiveIndex = 0;
                    }

                    // complete step
                    if (transferCanceled)
                    {
                        // do nothing
                    }
                    else if (transferOK)
                    {
                        taskState.HasError = false;

                        if (stepCompleted)
                        {
                            taskState.PositionDT = taskState.StepTimeRange.EndTime;
                            taskState.Modified = true;
                            taskState.StepInProgress = false;

                            if (task == autoTask)
                            {
                                SaveAutoTaskState();
                            }
                            else if (task.IsCompleted)
                            {
                                CloseTask(task);
                            }
                        }
                    }
                    else
                    {
                        taskState.HasError = true;
                    }
                }
            }
            catch (Exception ex)
            {
                exporterContext.ExporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при репликации данных" :
                    "Error replicating data");

                if (task != null)
                    task.State.HasError = true;
            }
        }

        /// <summary>
        /// Enqueues the export task.
        /// </summary>
        public void EnqueueTask(ArcExportTask task)
        {
            ArgumentNullException.ThrowIfNull(task, nameof(task));

            if (!replicatorIsReady)
            {
                exporterContext.ExporterLog.WriteError(Locale.IsRussian ?
                    "Невозможно добавить задачу в очередь. Репликация не готова к работе" :
                    "Unable to add task to the queue. Replication is not ready for operating");
            }
            else if (task.Validate(out string errMsg))
            {
                lock (taskQueue)
                {
                    if (taskQueue.Count >= MaxTaskQueueSize)
                    {
                        exporterContext.ExporterLog.WriteError(Locale.IsRussian ?
                            "Невозможно добавить задачу в очередь. Максимальный размер очереди превышен" :
                            "Unable to add task to the queue. The maximum size of the queue is exceeded");
                    }
                    else
                    {
                        task.State.PositionDT = CorrectPosition(task.StartDT, options.ReadingStep);
                        taskQueue.Enqueue(task);
                    }
                }
            }
            else
            {
                exporterContext.ExporterLog.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Clears the task queue.
        /// </summary>
        public void ClearTaskQueue()
        {
            lock (taskQueue)
            {
                taskQueue.Clear();
            }
        }

        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sbInfo)
        {
            ArgumentNullException.ThrowIfNull(sbInfo, nameof(sbInfo));

            if (Locale.IsRussian)
            {
                sbInfo
                    .AppendLine("Репликация архива")
                    .AppendLine("-----------------")
                    .Append("Готовность      : ").AppendLine(replicatorIsReady ? "да" : "нет")
                    .Append("Задач в очереди : ").Append(taskQueue.Count)
                    .Append(" из ").Append(MaxTaskQueueSize).AppendLine()
                    .Append("Задача          : ").AppendLine(displayTask == null ? "нет" : displayTask.Name);
            }
            else
            {
                sbInfo
                    .AppendLine("Archive Replication")
                    .AppendLine("-------------------")
                    .Append("Is ready       : ").AppendLine(replicatorIsReady ? "Yes" : "No")
                    .Append("Tasks in queue : ").Append(taskQueue.Count)
                    .Append(" of ").Append(MaxTaskQueueSize).AppendLine()
                    .Append("Task           : ").AppendLine(displayTask == null ? "None" : displayTask.Name);
            }

            if (displayTask != null)
                displayTask.State.AppendInfo(sbInfo);
        }
    }
}
