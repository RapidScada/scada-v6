// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Lang;
using Scada.Log;
using Scada.MultiDb;
using Scada.Server.Modules.ModDbExport.Config;
using Scada.Server.Modules.ModDbExport.Logic.Queries;
using Scada.Server.Modules.ModDbExport.Logic.Replication;
using System.Data.Common;
using System.Text;

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Exports data to a one target database.
    /// <para>Экспортирует данные в одну целевую базу данных.</para>
    /// </summary>
    internal sealed class Exporter : IExporterContext
    {
        /// <summary>
        /// The number of queue items to export in a single loop iteration.
        /// </summary>
        private const int BundleSize = 100;
        /// <summary>
        /// The size of the slices created by the exporter.
        /// </summary>
        private const int SliceSize = 1000;

        private readonly ExportTargetConfig exporterConfig;    // the exporter configuration
        private readonly IServerContext serverContext;         // the application context

        private readonly string exporterTitle;                 // the title of the exporter
        private readonly TimeSpan dataLifetime;                // the data lifetime in the queue
        private readonly CurDataExportOptions curDataExportOptions;   // the current data export options
        private readonly ArcReplicationOptions arcReplicationOptions; // the archive replication options

        private readonly DataSource dataSource;                // provides access to the DB
        private readonly ClassifiedQueries classifiedQueries;  // the queries grouped by classes
        private readonly DataQueue<SliceItem> curDataQueue;    // the current data queue
        private readonly DataQueue<SliceItem> histDataQueue;   // the historical data queue
        private readonly DataQueue<Event> eventQueue;          // the event queue
        private readonly DataQueue<EventAck> eventAckQueue;    // the event acknowledgement queue
        private readonly DataQueue<TeleCommand> cmdQueue;      // the command queue

        private readonly ILog exporterLog;                     // the exporter log
        private readonly string filePrefix;                    // the prefix of the exporter files
        private readonly string infoFileName;                  // the information file name
        private readonly Dictionary<int, CnlData> prevCnlData; // the previous channel data
        private readonly ArcReplicator arcReplicator;          // replicates archives

        private HashSet<int> cnlNumFilter;     // the incoming current data filter
        private ICollection<int> calcCnlNums;  // the calculated channel numbers exported on change
        private ICollection<int> timerCnlNums; // the channel numbers exported on timer
        private DateTime allDataDT;            // the timestamp of exporting all channel data
        private Thread exporterThread;         // the working thread of the exporter
        private volatile bool terminated;      // necessary to stop the thread
        private ConnectionStatus connStatus;   // the status of the DB connection 


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Exporter(ExportTargetConfig exporterConfig, IServerContext serverContext)
        {
            this.exporterConfig = exporterConfig ?? throw new ArgumentNullException(nameof(exporterConfig));
            this.serverContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));

            // get options
            GeneralOptions generalOptions = exporterConfig.GeneralOptions;
            exporterTitle = generalOptions.Title;
            dataLifetime = TimeSpan.FromSeconds(generalOptions.DataLifetime);
            curDataExportOptions = exporterConfig.ExportOptions.CurDataExportOptions;
            arcReplicationOptions = exporterConfig.ExportOptions.ArcReplicationOptions;

            // create data source and queries
            dataSource = DataSourceFactory.GetDataSource(exporterConfig.ConnectionOptions);
            classifiedQueries = new ClassifiedQueries();
            classifiedQueries.CreateQueries(exporterConfig.Queries, dataSource);

            // create queues
            int maxQueueSize = exporterConfig.GeneralOptions.MaxQueueSize;
            curDataQueue = new DataQueue<SliceItem>(classifiedQueries.CurDataQueries.Count > 0, maxQueueSize,
                Locale.IsRussian ? "Текущие данные" : "Current Data") { RemoveExceeded = true };
            histDataQueue = new DataQueue<SliceItem>(
                classifiedQueries.HistDataQueries.Count > 0 || arcReplicationOptions.Enabled, maxQueueSize,
                Locale.IsRussian ? "Исторические данные" : "Historical Data");
            eventQueue = new DataQueue<Event>(
                classifiedQueries.EventQueries.Count > 0 || arcReplicationOptions.Enabled, maxQueueSize,
                Locale.IsRussian ? "События" : "Events");
            eventAckQueue = new DataQueue<EventAck>(
                classifiedQueries.EventAckQueries.Count > 0, maxQueueSize,
                Locale.IsRussian ? "Квитирование событий" : "Event Acknowledgements");
            cmdQueue = new DataQueue<TeleCommand>(classifiedQueries.CmdQueries.Count > 0, maxQueueSize,
                Locale.IsRussian ? "Команды" : "Commands");

            // initialize other fields
            filePrefix = ModuleUtils.ModuleCode + "_" + generalOptions.ID.ToString("D3");
            exporterLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, filePrefix + ".log"),
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
            infoFileName = Path.Combine(serverContext.AppDirs.LogDir, filePrefix + ".txt");
            prevCnlData = curDataQueue.Enabled ? new Dictionary<int, CnlData>() : null;
            arcReplicator = arcReplicationOptions.Enabled ? new ArcReplicator(serverContext, this) : null;

            cnlNumFilter = null;
            calcCnlNums = null;
            timerCnlNums = null;
            allDataDT = DateTime.MinValue;
            exporterThread = null;
            terminated = false;
            connStatus = ConnectionStatus.Undefined;
        }


        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            DateTime utcNow = DateTime.UtcNow;
            bool timerMode = curDataExportOptions.Trigger == ExportTrigger.OnTimer;
            DateTime timerDT = LogicUtils.CalcNextTimer(utcNow, curDataExportOptions.TimerPeriod);
            allDataDT = utcNow.AddSeconds(curDataExportOptions.AllDataPeriod);

            DateTime writeInfoDT = utcNow;
            WriteInfo();
            InitExporter();

            while (!terminated)
            {
                // export to database
                try
                {
                    if (Connect())
                    {
                        ExportCurrentData();
                        ExportHistoricalData();
                        ExportEvents();
                        ExportEventAcks();
                        ExportCommands();
                    }
                }
                finally
                {
                    Disconnect();
                }

                // make slices on timer
                if (timerMode)
                {
                    utcNow = DateTime.UtcNow;
                    if (timerDT <= utcNow)
                    {
                        timerDT = LogicUtils.CalcNextTimer(utcNow, curDataExportOptions.TimerPeriod);
                        EnqueueCurrentDataOnTimer(utcNow);
                    }
                }

                // export archives
                //if (arcReplicationOptions.Enabled && scadaClient.IsReady)
                //    arcReplicator.ReplicateData();

                // write exporter info
                utcNow = DateTime.UtcNow;
                if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                {
                    writeInfoDT = utcNow;
                    WriteInfo();
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Initializes the exporter.
        /// </summary>
        private void InitExporter()
        {
            try
            {
                InitCnlNums();

                //if (arcReplicationOptions.Enabled)
                //    arcReplicator.Init();
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при инициализации шлюза" :
                    "Error initializing gate");
            }
        }

        /// <summary>
        /// Initializes the channel numbers of the exporter.
        /// </summary>
        private void InitCnlNums()
        {
            if (!curDataQueue.Enabled) 
                return;

            IEnumerable<int> cnlNumRange = 
                classifiedQueries.CurDataQueries.All(q => q.Filter.CnlNums.Count > 0)
                ? classifiedQueries.CurDataQueries.SelectMany(q => q.Filter.CnlNums).Distinct().OrderBy(n => n)
                : null;

            if (curDataExportOptions.Trigger == ExportTrigger.OnReceive)
            {
                cnlNumFilter = cnlNumRange == null ? null : new HashSet<int>(cnlNumRange);

                if (curDataExportOptions.IncludeCalculated)
                {
                    calcCnlNums = cnlNumRange == null
                        ? serverContext.Cnls.CalcCnls.Keys
                        : cnlNumRange.Where(n => serverContext.Cnls.CalcCnls.ContainsKey(n)).ToArray();
                }
            }
            else // ExportTrigger.OnTimer
            {
                if (curDataExportOptions.IncludeCalculated)
                {
                    timerCnlNums = cnlNumRange == null
                        ? serverContext.Cnls.ArcCnls.Keys
                        : cnlNumRange.ToArray();
                }
                else
                {
                    timerCnlNums = cnlNumRange == null 
                        ? serverContext.Cnls.MeasCnls.Keys
                        : cnlNumRange.Where(n => serverContext.Cnls.MeasCnls.ContainsKey(n)).ToArray();
                }
            }
        }

        /// <summary>
        /// Connects to the database.
        /// </summary>
        private bool Connect()
        {
            try
            {
                dataSource.Connect();
                connStatus = ConnectionStatus.Normal;
                return true;
            }
            catch (Exception ex)
            {
                connStatus = ConnectionStatus.Error;
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при соединении с БД" :
                    "Error connecting to DB");
                Thread.Sleep(ScadaUtils.ErrorDelay);
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the database.
        /// </summary>
        private void Disconnect()
        {
            try
            {
                dataSource.Disconnect();
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при разъединении с БД" :
                    "Error disconnecting from DB");
            }
        }

        /// <summary>
        /// Exports current data to the database.
        /// </summary>
        private void ExportCurrentData()
        {
            if (!curDataQueue.Enabled)
                return;

        }

        /// <summary>
        /// Exports historical data to the database.
        /// </summary>
        private void ExportHistoricalData()
        {
            if (!histDataQueue.Enabled)
                return;

        }

        /// <summary>
        /// Exports events to the database.
        /// </summary>
        private void ExportEvents()
        {
            if (!eventQueue.Enabled)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve an item from the queue
                    if (!eventQueue.TryDequeue(out QueueItem<Event> queueItem))
                        break;

                    // export the event
                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        exporterLog.WriteError(Locale.IsRussian ?
                            "Устаревшее событие удалено из очереди" :
                            "Outdated event removed from the queue");

                        eventQueue.Stats.SkippedItems++;
                    }
                    else if (ExportEvent(queueItem.Value, trans))
                    {
                        eventQueue.Stats.ExportedItems++;
                        eventQueue.Stats.HasError = false;
                        eventQueue.CheckEmpty();
                    }
                    else
                    {
                        // return the unsent item to the queue
                        eventQueue.ReturnItem(queueItem);
                        eventQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                dataSource.SilentRollback(trans);
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте событий" :
                    "Error export events");
            }
        }

        /// <summary>
        /// Exports event acknowledgements to the database.
        /// </summary>
        private void ExportEventAcks()
        {
            if (!eventAckQueue.Enabled)
                return;

        }

        /// <summary>
        /// Exports commands to the database.
        /// </summary>
        private void ExportCommands()
        {
            if (!cmdQueue.Enabled)
                return;

        }

        /// <summary>
        /// Exports the specified event.
        /// </summary>
        private bool ExportEvent(Event ev, DbTransaction trans)
        {
            Query currentQuery = null;

            try
            {
                foreach (EventQuery query in classifiedQueries.EventQueries)
                {
                    currentQuery = query;

                    if (query.Filter.Match(ev))
                    {
                        query.Parameters.EventID.Value = ev.EventID;
                        query.Parameters.Timestamp.Value = ev.Timestamp;
                        query.Parameters.Hidden.Value = ev.Hidden;
                        query.Parameters.CnlNum.Value = ev.CnlNum;
                        query.Parameters.ObjNum.Value = ev.ObjNum;
                        query.Parameters.DeviceNum.Value = ev.DeviceNum;
                        query.Parameters.PrevCnlVal.Value = ev.PrevCnlVal;
                        query.Parameters.PrevCnlStat.Value = ev.PrevCnlStat;
                        query.Parameters.CnlVal.Value = ev.CnlVal;
                        query.Parameters.CnlStat.Value = ev.CnlStat;
                        query.Parameters.Severity.Value = ev.Severity;
                        query.Parameters.AckRequired.Value = ev.AckRequired;
                        query.Parameters.Ack.Value = ev.Ack;
                        query.Parameters.AckTimestamp.Value = ev.AckTimestamp;
                        query.Parameters.AckUserID.Value = ev.AckUserID;
                        query.Parameters.TextFormat.Value = (int)ev.TextFormat;
                        query.Parameters.Text.Value = (object)ev.Text ?? DBNull.Value;
                        query.Parameters.Data.Value = (object)ev.Data ?? DBNull.Value;

                        query.Command.Transaction = trans;
                        query.Command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте события по запросу \"{0}\"" :
                    "Error exporting event by the query \"{0}\"", 
                    currentQuery?.Name);
                return false;
            }
        }

        /// <summary>
        /// Filters the incoming current data.
        /// </summary>
        private SliceItem FilterIncomingData(Slice slice)
        {
            if (cnlNumFilter == null && !curDataExportOptions.SkipUnchanged)
            {
                return new SliceItem(slice);
            }
            else
            {
                int srcCnlCnt = slice.CnlNums.Length;
                List<int> destCnlNums = new(srcCnlCnt);
                List<CnlData> destCnlData = new(srcCnlCnt);

                for (int i = 0; i < srcCnlCnt; i++)
                {
                    int cnlNum = slice.CnlNums[i];

                    if (cnlNumFilter == null || cnlNumFilter.Contains(cnlNum))
                    {
                        CnlData cnlDataItem = slice.CnlData[i];

                        if (!curDataExportOptions.SkipUnchanged || ChannelDataChanged(cnlNum, cnlDataItem))
                        {
                            destCnlNums.Add(cnlNum);
                            destCnlData.Add(cnlDataItem);
                        }
                    }
                }

                if (destCnlNums.Count > 0)
                {
                    return new SliceItem(new Slice(
                        slice.Timestamp, 
                        destCnlNums.ToArray(), 
                        destCnlData.ToArray()));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the slices of the current data of the specified channels.
        /// </summary>
        private List<SliceItem> SliceCurrentData(ICollection<int> cnlNums, bool skipUnchanged)
        {
            if (cnlNums == null)
                return null;

            List<SliceItem> sliceItems = null;
            List<int> sliceCnlNums = null;
            List<CnlData> sliceCnlData = null;
            DateTime utcNow = DateTime.UtcNow;

            void AddSlice()
            {
                sliceItems ??= new List<SliceItem>();
                sliceItems.Add(new SliceItem(new Slice(
                    utcNow,
                    sliceCnlNums.ToArray(),
                    sliceCnlData.ToArray())));
                sliceCnlNums = null;
                sliceCnlData = null;
            }

            foreach (int cnlNum in cnlNums)
            {
                CnlData curCnlData = serverContext.GetCurrentData(cnlNum);

                if (!skipUnchanged || ChannelDataChanged(cnlNum, curCnlData))
                {
                    if (sliceCnlNums == null)
                    {
                        sliceCnlNums = new List<int>(SliceSize);
                        sliceCnlData = new List<CnlData>(SliceSize);
                    }

                    sliceCnlNums.Add(cnlNum);
                    sliceCnlData.Add(curCnlData);

                    if (sliceCnlNums.Count == SliceSize)
                        AddSlice();
                }
            }

            if (sliceCnlNums != null && sliceCnlNums.Count > 0)
                AddSlice();

            return sliceItems;
        }

        /// <summary>
        /// Checks if the channel data has changed and saves the previous data.
        /// </summary>
        private bool ChannelDataChanged(int cnlNum, CnlData cnlData)
        {
            if (prevCnlData == null)
            {
                return false;
            }
            else if (prevCnlData.TryGetValue(cnlNum, out CnlData prevCnlDataItem) && prevCnlDataItem == cnlData)
            {
                return false;
            }
            else
            {
                prevCnlData[cnlNum] = cnlData;
                return true;
            }
        }

        /// <summary>
        /// Enqueues the current data on timer.
        /// </summary>
        private void EnqueueCurrentDataOnTimer(DateTime nowDT)
        {
            try
            {
                if (curDataQueue.Enabled)
                {
                    // enqueue slices for timer channels
                    bool skipUnchanged = curDataExportOptions.SkipUnchanged;

                    if (skipUnchanged && curDataExportOptions.AllDataPeriod > 0 && allDataDT <= nowDT)
                    {
                        allDataDT = nowDT.AddSeconds(curDataExportOptions.AllDataPeriod);
                        skipUnchanged = false;
                    }

                    if (SliceCurrentData(timerCnlNums, skipUnchanged) is List<SliceItem> sliceItems)
                        curDataQueue.Enqueue(DateTime.UtcNow, sliceItems, out _);

                    // enqueue slices for single queries
                    foreach (DataQuery dataQuery in classifiedQueries.CurDataQueries
                        .Where(q => q.Options.SingleQuery && q.Options.Filter.CnlNums.Count > 0))
                    {
                        int cnlCnt = dataQuery.Options.Filter.CnlNums.Count;
                        Slice slice = new(nowDT, cnlCnt);

                        for (int i = 0; i < cnlCnt; i++)
                        {
                            int cnlNum = dataQuery.Options.Filter.CnlNums[i];
                            slice.CnlNums[i] = cnlNum;
                            slice.CnlData[i] = serverContext.GetCurrentData(cnlNum);
                        }

                        curDataQueue.Enqueue(DateTime.UtcNow, 
                            new SliceItem(slice) { QueryID = dataQuery.QueryID }, out _);
                    }
                }
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных по таймеру" :
                    "Error getting current data on timer");
            }
        }

        /// <summary>
        /// Writes the exporter information to the file.
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // prepare information
                StringBuilder sbInfo = new();

                if (Locale.IsRussian)
                {
                    sbInfo
                        .AppendLine("Состояние экспортёра")
                        .AppendLine("--------------------")
                        .Append("Наименование : ").AppendLine(exporterTitle)
                        .Append("Сервер БД    : ").AppendLine(exporterConfig.ConnectionOptions.Server)
                        .Append("Соединение   : ").AppendLine(connStatus.ToString(true));
                }
                else
                {
                    sbInfo
                        .AppendLine("Exporter State")
                        .AppendLine("--------------")
                        .Append("Name       : ").AppendLine(exporterTitle)
                        .Append("DB server  : ").AppendLine(exporterConfig.ConnectionOptions.Server)
                        .Append("Connection : ").AppendLine(connStatus.ToString(false));
                }

                sbInfo.AppendLine();
                curDataQueue.AppendInfo(sbInfo);
                histDataQueue.AppendInfo(sbInfo);
                eventQueue.AppendInfo(sbInfo);
                eventAckQueue.AppendInfo(sbInfo);
                cmdQueue.AppendInfo(sbInfo);
                //arcReplicator?.AppendInfo(sbInfo);

                // write to file
                using StreamWriter writer = new(infoFileName, false, Encoding.UTF8);
                writer.Write(sbInfo.ToString());
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при записи в файл информации о работе экспортёра" :
                    "Error writing exporter information to the file");
            }
        }


        /// <summary>
        /// Starts the exporter.
        /// </summary>
        public void Start()
        {
            try
            {
                if (exporterThread == null)
                {
                    exporterLog.WriteBreak();
                    exporterLog.WriteAction(Locale.IsRussian ?
                        "Экспортёр \"{0}\" запущен" :
                        "Exporter \"{0}\" started", exporterTitle);

                    terminated = false;
                    exporterThread = new Thread(Execute);
                    exporterThread.Start();
                }
                else
                {
                    exporterLog.WriteError(Locale.IsRussian ?
                        "Экспортёр уже запущен" :
                        "Exporter is already running");
                }
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске экспортёра" :
                    "Error starting exporter");
            }
        }

        /// <summary>
        /// Stops the exporter.
        /// </summary>
        public void Stop()
        {
            try
            {
                // stop exporter thread
                if (exporterThread != null)
                {
                    terminated = true;
                    exporterThread.Join();
                    exporterThread = null;
                }

                exporterLog.WriteAction(Locale.IsRussian ?
                    "Экспортёр \"{0}\" остановлен" :
                    "Exporter \"{0}\" is stopped", exporterTitle);
                exporterLog.WriteBreak();
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при остановке экспортёра" :
                    "Error stopping exporter");
            }
        }

        /// <summary>
        /// Enqueues the received current data to be exported.
        /// </summary>
        public void EnqueueCurrentData(Slice slice)
        {
            ArgumentNullException.ThrowIfNull(slice, nameof(slice));

            if (curDataQueue.Enabled &&
                curDataExportOptions.Trigger == ExportTrigger.OnReceive &&
                FilterIncomingData(slice) is SliceItem sliceItem)
            {
                curDataQueue.Enqueue(slice.Timestamp, sliceItem, out _);
            }
        }

        /// <summary>
        /// Enqueues the calculated current data to be exported.
        /// </summary>
        public void EnqueueCalculatedData()
        {
            if (curDataQueue.Enabled &&
                curDataExportOptions.Trigger == ExportTrigger.OnReceive &&
                curDataExportOptions.IncludeCalculated &&
                SliceCurrentData(calcCnlNums, true) is List<SliceItem> sliceItems)
            {
                curDataQueue.Enqueue(DateTime.UtcNow, sliceItems, out _);
            }
        }

        /// <summary>
        /// Enqueues the historical data to be exported.
        /// </summary>
        public void EnqueueHistoricalData(Slice slice)
        {
            ArgumentNullException.ThrowIfNull(slice, nameof(slice));

            if (histDataQueue.Enabled && !arcReplicationOptions.Enabled)
            {
                if (!histDataQueue.Enqueue(DateTime.UtcNow, new SliceItem(slice), out string errMsg))
                    exporterLog.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Enqueues the event to be exported.
        /// </summary>
        public void EnqueueEvent(Event ev)
        {
            ArgumentNullException.ThrowIfNull(ev, nameof(ev));

            if (eventQueue.Enabled && !arcReplicationOptions.Enabled)
            {
                if (!eventQueue.Enqueue(DateTime.UtcNow, ev, out string errMsg))
                    exporterLog.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Enqueues the event acknowledgement to be exported.
        /// </summary>
        public void EnqueueEventAck(EventAck eventAck)
        {
            ArgumentNullException.ThrowIfNull(eventAck, nameof(eventAck));

            if (eventAckQueue.Enabled)
            {
                if (!eventAckQueue.Enqueue(DateTime.UtcNow, eventAck, out string errMsg))
                    exporterLog.WriteError(errMsg);
            }
        }

        /// <summary>
        /// Enqueues the command to be exported or executed.
        /// </summary>
        public void EnqueueCommand(TeleCommand command, CommandResult commandResult)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(commandResult, nameof(commandResult));

            if (command.CmdCode == exporterConfig.GeneralOptions.CmdCode)
            {
                // handle exporter command
                commandResult.TransmitToClients = false;
                IDictionary<string, string> cmdArgs = command.GetCmdDataArgs();

                if (Enum.TryParse(cmdArgs.GetValueAsString("cmd"), out ExporterCommand exporterCommand))
                {
                    exporterLog.WriteAction(Locale.IsRussian ?
                        "Получена команда экспортёра: {0}" :
                        "Exporter command received: {0}", exporterCommand);

                    if (arcReplicationOptions.Enabled)
                    {
                        // TODO: exporter tasks
                        //if (exporterCommand == ExporterCommand.ClearTaskQueue)
                        //    arcReplicator.ClearTaskQueue();
                        //else if (ArcReplicationTask.CreateTask(exporterCommand, cmdArgs) is ArcReplicationTask task)
                        //    arcReplicator.EnqueueTask(task);
                    }
                }
                else
                {
                    exporterLog.WriteError(Locale.IsRussian ?
                        "Неизвестная команда экспортёра" :
                        "Unknown exporter command");
                }
            }
            else if (cmdQueue.Enabled)
            {
                // transfer command
                if (!cmdQueue.Enqueue(DateTime.UtcNow, command, out string errMsg))
                    exporterLog.WriteError(errMsg);
            }
        }
    }
}
