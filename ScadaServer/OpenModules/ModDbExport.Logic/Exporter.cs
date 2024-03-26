// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Dbms;
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
        private readonly HistDataExportOptions histDataExportOptions; // the historical data export options
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
        private readonly Dictionary<DateTime, DateTime> histTimestamps; // the timestamps of the historical data
        private readonly ArcReplicator arcReplicator;          // replicates archives

        private HashSet<int> cnlNumFilter;        // the incoming current data filter
        private ICollection<int> curCalcCnlNums;  // the calculated channel numbers for exporting current data
        private ICollection<int> histCalcCnlNums; // the calculated channel numbers for exporting historical data
        private ICollection<int> timerCnlNums;    // the channel numbers for exporting current data on timer
        private DateTime allDataDT;               // the timestamp of exporting all channel data
        private Thread exporterThread;            // the working thread of the exporter
        private volatile bool terminated;         // necessary to stop the thread
        private ConnectionStatus connStatus;      // the status of the DB connection 


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
            histDataExportOptions = exporterConfig.ExportOptions.HistDataExportOptions;
            arcReplicationOptions = exporterConfig.ExportOptions.ArcReplicationOptions;

            // create data source and queries
            dataSource = DataSourceFactory.GetDataSource(exporterConfig.ConnectionOptions);
            classifiedQueries = new ClassifiedQueries();
            classifiedQueries.CreateQueries(exporterConfig.Queries, dataSource);

            // create queues
            int maxQueueSize = exporterConfig.GeneralOptions.MaxQueueSize;
            curDataQueue = new DataQueue<SliceItem>(classifiedQueries.CurDataQueries.Count > 0, maxQueueSize,
                Locale.IsRussian ? "Текущие данные" : "Current Data") { RemoveExceeded = true };
            histDataQueue = new DataQueue<SliceItem>(classifiedQueries.HistDataQueries.Count > 0, maxQueueSize,
                Locale.IsRussian ? "Исторические данные" : "Historical Data");
            eventQueue = new DataQueue<Event>(classifiedQueries.EventQueries.Count > 0, maxQueueSize,
                Locale.IsRussian ? "События" : "Events");
            eventAckQueue = new DataQueue<EventAck>(classifiedQueries.EventAckQueries.Count > 0, maxQueueSize,
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
            prevCnlData = curDataQueue.Enabled ?
                new Dictionary<int, CnlData>() : null;
            histTimestamps = histDataQueue.Enabled && histDataExportOptions.IncludeCalculated ?
                new Dictionary<DateTime, DateTime>() : null;
            arcReplicator = arcReplicationOptions.Enabled ?
                new ArcReplicator(serverContext, this) : null;

            cnlNumFilter = null;
            curCalcCnlNums = null;
            histCalcCnlNums = null;
            timerCnlNums = null;
            allDataDT = DateTime.MinValue;
            exporterThread = null;
            terminated = false;
            connStatus = ConnectionStatus.Undefined;
        }


        /// <summary>
        /// Gets or sets the status of the DB connection.
        /// </summary>
        private ConnectionStatus ConnStatus
        {
            get
            {
                return connStatus;
            }
            set
            {
                connStatus = value;

                // write status to channel
                if (exporterConfig.GeneralOptions.StatusCnlNum > 0)
                {
                    serverContext.WriteCurrentData(
                        exporterConfig.GeneralOptions.StatusCnlNum, 
                        connStatus switch
                        {
                            ConnectionStatus.Normal => new CnlData(0, CnlStatusID.Defined),
                            ConnectionStatus.Error => new CnlData(1, CnlStatusID.Defined),
                            _ => CnlData.Empty
                        });
                }
            }
        }


        /// <summary>
        /// Gets the gate configuration.
        /// </summary>
        ExportTargetConfig IExporterContext.ExporterConfig => exporterConfig;

        /// <summary>
        /// Gets the gate log.
        /// </summary>
        ILog IExporterContext.ExporterLog => exporterLog;

        /// <summary>
        /// Gets the prefix of the gate files.
        /// </summary>
        string IExporterContext.FilePrefix => filePrefix;

        /// <summary>
        /// Gets the queries grouped by classes.
        /// </summary>
        ClassifiedQueries IExporterContext.ClassifiedQueries => classifiedQueries;

        /// <summary>
        /// Gets the historical data queue.
        /// </summary>
        IDataQueue<SliceItem> IExporterContext.HistDataQueue => histDataQueue;

        /// <summary>
        /// Gets the event queue.
        /// </summary>
        IDataQueue<Event> IExporterContext.EventQueue => eventQueue;


        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            DateTime utcNow = DateTime.UtcNow;
            bool timerMode = curDataExportOptions.Trigger == ExportTrigger.OnTimer;
            DateTime timerDT = LogicUtils.CalcNextTimer(utcNow, curDataExportOptions.TimerPeriod);
            allDataDT = LogicUtils.CalcNextTimer(utcNow, curDataExportOptions.AllDataPeriod);

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

                // make slices of current data on timer
                if (timerMode)
                {
                    utcNow = DateTime.UtcNow;
                    if (timerDT <= utcNow)
                    {
                        timerDT = LogicUtils.CalcNextTimer(utcNow, curDataExportOptions.TimerPeriod);
                        EnqueueCurrentDataOnTimer(utcNow);
                    }
                }

                // make slices of historical data
                EnqueueCalculatedHistoricalData();

                // export archives
                if (arcReplicationOptions.Enabled && ConnStatus == ConnectionStatus.Normal)
                    arcReplicator.ReplicateData();

                // write exporter info
                utcNow = DateTime.UtcNow;
                if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                {
                    writeInfoDT = utcNow;
                    WriteInfo();
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }

            connStatus = ConnectionStatus.Undefined; // do not write status to channel
            WriteInfo();
        }

        /// <summary>
        /// Initializes the exporter.
        /// </summary>
        private void InitExporter()
        {
            try
            {
                ConnStatus = ConnectionStatus.Undefined;

                if (classifiedQueries.IsEmpty)
                {
                    exporterLog.WriteError(Locale.IsRussian ?
                        "Отсутствуют активные запросы." :
                        "Active queries missing.");
                }
                else
                {
                    InitCnlNums();

                    classifiedQueries.CurDataQueries.ForEach(q => q.FillCnlNumFilter(serverContext.ConfigDatabase));
                    classifiedQueries.HistDataQueries.ForEach(q => q.FillCnlNumFilter(serverContext.ConfigDatabase));

                    if (arcReplicationOptions.Enabled)
                        arcReplicator.Init();
                }
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
            // define channel numbers for exporting current data
            if (curDataQueue.Enabled)
            {
                // get all channel numbers specified in the filters of the current data queries
                IEnumerable<int> cnlNumRange = classifiedQueries.CurDataQueries.All(q => q.CnlNumFilter.Count > 0)
                    ? classifiedQueries.CurDataQueries.SelectMany(q => q.CnlNumFilter).Distinct().OrderBy(n => n)
                    : null;

                if (curDataExportOptions.Trigger == ExportTrigger.OnReceive)
                {
                    cnlNumFilter = cnlNumRange == null ? null : new HashSet<int>(cnlNumRange);

                    if (curDataExportOptions.IncludeCalculated)
                    {
                        curCalcCnlNums = cnlNumRange == null
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

            // define channel numbers for exporting historical data
            if (histDataQueue.Enabled && histDataExportOptions.IncludeCalculated)
            {
                histCalcCnlNums = classifiedQueries.HistDataQueries.All(q => q.CnlNumFilter.Count > 0)
                    ? classifiedQueries.HistDataQueries.SelectMany(q => q.CnlNumFilter).Distinct().OrderBy(n => n).ToArray()
                    : serverContext.Cnls.CalcCnls.Keys;
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
                ConnStatus = ConnectionStatus.Normal;
                return true;
            }
            catch (Exception ex)
            {
                ConnStatus = ConnectionStatus.Error;
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
            if (!curDataQueue.Enabled || curDataQueue.IsEmpty)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // get an item slice from the queue without removing it
                    if (!curDataQueue.TryPeek(out QueueItem<SliceItem> queueItem))
                        break;

                    DateTime utcNow = DateTime.UtcNow;

                    if (utcNow - queueItem.CreationTime > dataLifetime)
                    {
                        exporterLog.WriteError(Locale.IsRussian ?
                            "Устаревшие текущие данные удалены из очереди" :
                            "Outdated current data removed from the queue");

                        curDataQueue.Stats.SkippedItems++;
                    }
                    else if (ExportSlice(queueItem.Value, trans, classifiedQueries.CurDataQueries))
                    {
                        curDataQueue.Stats.ExportedItems++;
                        curDataQueue.Stats.HasError = false;
                    }
                    else
                    {
                        curDataQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break; // the item is not removed from the queue
                    }

                    // remove the item from the queue
                    curDataQueue.RemoveItem(queueItem);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans?.SilentRollback();
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте текущих данных" :
                    "Error exporting current data");
            }
        }

        /// <summary>
        /// Exports historical data to the database.
        /// </summary>
        private void ExportHistoricalData()
        {
            if (!histDataQueue.Enabled || histDataQueue.IsEmpty)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve an item from the queue
                    if (!histDataQueue.TryDequeue(out QueueItem<SliceItem> queueItem))
                        break;

                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        exporterLog.WriteError(Locale.IsRussian ?
                            "Устаревшие исторические данные удалены из очереди" :
                            "Outdated historical data removed from the queue");

                        histDataQueue.Stats.SkippedItems++;
                    }
                    else if (ExportSlice(queueItem.Value, trans, classifiedQueries.HistDataQueries))
                    {
                        histDataQueue.Stats.ExportedItems++;
                        histDataQueue.Stats.HasError = false;
                        histDataQueue.CheckEmpty();
                    }
                    else
                    {
                        // return the unsent item to the queue
                        histDataQueue.ReturnItem(queueItem);
                        histDataQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans?.SilentRollback();
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте исторических данных" :
                    "Error exporting historical data");
            }
        }

        /// <summary>
        /// Exports events to the database.
        /// </summary>
        private void ExportEvents()
        {
            if (!eventQueue.Enabled || eventQueue.IsEmpty)
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
                trans?.SilentRollback();
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте событий" :
                    "Error exporting events");
            }
        }

        /// <summary>
        /// Exports event acknowledgements to the database.
        /// </summary>
        private void ExportEventAcks()
        {
            if (!eventAckQueue.Enabled || eventAckQueue.IsEmpty)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve an item from the queue
                    if (!eventAckQueue.TryDequeue(out QueueItem<EventAck> queueItem))
                        break;

                    // export the acknowledgement
                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        exporterLog.WriteError(Locale.IsRussian ?
                            "Устаревшее квитирование удалено из очереди" :
                            "Outdated acknowledgement removed from the queue");

                        eventAckQueue.Stats.SkippedItems++;
                    }
                    else if (ExportEventAck(queueItem.Value, trans))
                    {
                        eventAckQueue.Stats.ExportedItems++;
                        eventAckQueue.Stats.HasError = false;
                        eventAckQueue.CheckEmpty();
                    }
                    else
                    {
                        // return the unsent item to the queue
                        eventAckQueue.ReturnItem(queueItem);
                        eventAckQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans?.SilentRollback();
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте квитирований" :
                    "Error exporting acknowledgements");
            }
        }

        /// <summary>
        /// Exports commands to the database.
        /// </summary>
        private void ExportCommands()
        {
            if (!cmdQueue.Enabled || cmdQueue.IsEmpty)
                return;

            DbTransaction trans = null;

            try
            {
                trans = dataSource.Connection.BeginTransaction();

                for (int i = 0; i < BundleSize; i++)
                {
                    // retrieve an item from the queue
                    if (!cmdQueue.TryDequeue(out QueueItem<TeleCommand> queueItem))
                        break;

                    // export the command
                    if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                    {
                        exporterLog.WriteError(Locale.IsRussian ?
                            "Устаревшая команда удалена из очереди" :
                            "Outdated command removed from the queue");

                        cmdQueue.Stats.SkippedItems++;
                    }
                    else if (ExportCommand(queueItem.Value, trans))
                    {
                        cmdQueue.Stats.ExportedItems++;
                        cmdQueue.Stats.HasError = false;
                        cmdQueue.CheckEmpty();
                    }
                    else
                    {
                        // return the unsent item to the queue
                        cmdQueue.ReturnItem(queueItem);
                        cmdQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break;
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans?.SilentRollback();
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при экспорте команд" :
                    "Error exporting commands");
            }
        }

        /// <summary>
        /// Exports the specified slice.
        /// </summary>
        private bool ExportSlice(SliceItem sliceItem, DbTransaction trans, List<DataQuery> queries)
        {
            Query currentQuery = null;

            try
            {
                foreach (DataQuery query in queries)
                {
                    currentQuery = query;
                    Slice slice = sliceItem.Slice;

                    if ((sliceItem.QueryID <= 0 || sliceItem.QueryID == query.QueryID) &&
                        (sliceItem.SingleQuery == null || sliceItem.SingleQuery == query.Options.SingleQuery) &&
                        (query.CnlNumFilter.Count == 0 || query.CnlNumFilter.Overlaps(slice.CnlNums)))
                    {
                        query.Command.Transaction = trans;
                        query.Parameters.Timestamp.Value = slice.Timestamp;

                        if (query.Options.SingleQuery)
                        {
                            List<int> queryCnlNums = query.Options.Filter.CnlNums;

                            if (queryCnlNums.Count > 0)
                            {
                                foreach (int cnlNum in queryCnlNums)
                                {
                                    query.SetCnlDataParams(cnlNum, CnlData.Empty);
                                }

                                for (int i = 0, cnlCnt = slice.Length; i < cnlCnt; i++)
                                {
                                    int cnlNum = slice.CnlNums[i];
                                    if (query.CnlNumFilter.Contains(cnlNum))
                                        query.SetCnlDataParams(cnlNum, slice.CnlData[i]);
                                }

                                if (query.CnlPropsRequired)
                                {
                                    Cnl cnl = serverContext.ConfigDatabase.CnlTable.GetItem(queryCnlNums[0]);
                                    query.Parameters.ObjNum.Value = cnl?.ObjNum ?? 0;
                                    query.Parameters.DeviceNum.Value = cnl?.DeviceNum ?? 0;
                                }

                                query.Command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            void ExportDataPoint(int cnlNum, CnlData cnlData)
                            {
                                if (query.CnlPropsRequired)
                                {
                                    Cnl cnl = serverContext.ConfigDatabase.CnlTable.GetItem(cnlNum);
                                    query.Parameters.ObjNum.Value = cnl?.ObjNum ?? 0;
                                    query.Parameters.DeviceNum.Value = cnl?.DeviceNum ?? 0;
                                }

                                query.Parameters.CnlNum.Value = cnlNum;
                                query.Parameters.Val.Value = cnlData.Val;
                                query.Parameters.Stat.Value = cnlData.Stat;
                                query.Command.ExecuteNonQuery();
                            }

                            if (query.CnlNumFilter.Count > 0)
                            {
                                for (int i = 0, cnlCnt = slice.Length; i < cnlCnt; i++)
                                {
                                    int cnlNum = slice.CnlNums[i];
                                    if (query.CnlNumFilter.Contains(cnlNum))
                                        ExportDataPoint(cnlNum, slice.CnlData[i]);
                                }
                            }
                            else
                            {
                                for (int i = 0, cnlCnt = slice.Length; i < cnlCnt; i++)
                                {
                                    ExportDataPoint(slice.CnlNums[i], slice.CnlData[i]);
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(Locale.IsRussian ?
                    "Ошибка при экспорте среза по запросу \"{0}\": {1}" :
                    "Error exporting slice by the query \"{0}\": {1}",
                    currentQuery?.Name, ex.Message);
                return false;
            }
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
                        query.SetParameters(ev);
                        query.Command.Transaction = trans;
                        query.Command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(Locale.IsRussian ?
                    "Ошибка при экспорте события по запросу \"{0}\": {1}" :
                    "Error exporting event by the query \"{0}\": {1}", 
                    currentQuery?.Name, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Exports the specified event acknowledgement.
        /// </summary>
        private bool ExportEventAck(EventAck evAck, DbTransaction trans)
        {
            Query currentQuery = null;

            try
            {
                foreach (EventAckQuery query in classifiedQueries.EventAckQueries)
                {
                    currentQuery = query;
                    query.SetParameters(evAck);
                    query.Command.Transaction = trans;
                    query.Command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(Locale.IsRussian ?
                    "Ошибка при экспорте квитирования по запросу \"{0}\": {1}" :
                    "Error exporting acknowledgement by the query \"{0}\": {1}",
                    currentQuery?.Name, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Exports the specified command.
        /// </summary>
        private bool ExportCommand(TeleCommand command, DbTransaction trans)
        {
            Query currentQuery = null;

            try
            {
                foreach (CmdQuery query in classifiedQueries.CmdQueries)
                {
                    currentQuery = query;

                    if (query.Filter.Match(command))
                    {
                        query.SetParameters(command);
                        query.Command.Transaction = trans;
                        query.Command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(Locale.IsRussian ?
                    "Ошибка при экспорте команды по запросу \"{0}\": {1}" :
                    "Error exporting command by the query \"{0}\": {1}",
                    currentQuery?.Name, ex.Message);
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
                int srcCnlCnt = slice.Length;
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
                        allDataDT = LogicUtils.CalcNextTimer(nowDT, curDataExportOptions.AllDataPeriod);
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
        /// Enqueues the historical data of the calculated channels.
        /// </summary>
        private void EnqueueCalculatedHistoricalData()
        {
            if (histTimestamps == null || histCalcCnlNums == null)
                return;

            try
            {
                lock (histTimestamps)
                {
                    DateTime utcNow = DateTime.UtcNow;

                    foreach (var (sliceTime, receiveTime) in histTimestamps.ToArray())
                    {
                        if ((utcNow - receiveTime).TotalSeconds >= histDataExportOptions.ExportCalculatedDelay)
                        {
                            histTimestamps.Remove(sliceTime);
                            List<SliceItem> sliceItems = SliceHistoricalData(sliceTime, histCalcCnlNums);

                            if (sliceItems != null && !histDataQueue.Enqueue(utcNow, sliceItems, out string errMsg))
                                exporterLog.WriteError(errMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exporterLog.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при получении расчётных исторических данных" :
                    "Error getting calculated historical data");
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
                Slice slice = new(utcNow, sliceCnlNums.ToArray(), sliceCnlData.ToArray());
                sliceItems ??= new List<SliceItem>();
                sliceItems.Add(new SliceItem(slice) { SingleQuery = false });
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
        /// Gets the slices of the historical data of the specified channels.
        /// </summary>
        private List<SliceItem> SliceHistoricalData(DateTime timestamp, ICollection<int> cnlNums)
        {
            if (cnlNums == null)
                return null;

            List<SliceItem> sliceItems = null;
            List<int> sliceCnlNums = null;

            void AddSlice()
            {
                Slice slice = serverContext.GetSlice(
                    histDataExportOptions.HistArchiveBit, timestamp, sliceCnlNums.ToArray());
                sliceItems ??= new List<SliceItem>();
                sliceItems.Add(new SliceItem(slice) { SingleQuery = false });
                sliceCnlNums = null;
            }

            foreach (int cnlNum in cnlNums)
            {
                sliceCnlNums ??= new List<int>(SliceSize);
                sliceCnlNums.Add(cnlNum);

                if (sliceCnlNums.Count == SliceSize)
                    AddSlice();
            }

            if (sliceCnlNums != null && sliceCnlNums.Count > 0)
                AddSlice();

            return sliceItems;
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
                string dbName = ScadaUtils.FirstNonEmpty(
                    exporterConfig.ConnectionOptions.Server, 
                    exporterConfig.ConnectionOptions.Name);

                if (Locale.IsRussian)
                {
                    sbInfo
                        .AppendLine("Состояние экспортёра")
                        .AppendLine("--------------------")
                        .Append("Наименование : ").AppendLine(exporterTitle)
                        .Append("Сервер БД    : ").AppendLine(dbName)
                        .Append("Соединение   : ").AppendLine(ConnStatus.ToString(true));
                }
                else
                {
                    sbInfo
                        .AppendLine("Exporter State")
                        .AppendLine("--------------")
                        .Append("Name       : ").AppendLine(exporterTitle)
                        .Append("DB server  : ").AppendLine(dbName)
                        .Append("Connection : ").AppendLine(ConnStatus.ToString(false));
                }

                sbInfo.AppendLine();
                curDataQueue.AppendInfo(sbInfo);
                histDataQueue.AppendInfo(sbInfo);
                eventQueue.AppendInfo(sbInfo);
                eventAckQueue.AppendInfo(sbInfo);
                cmdQueue.AppendInfo(sbInfo);
                arcReplicator?.AppendInfo(sbInfo);

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
                SliceCurrentData(curCalcCnlNums, true) is List<SliceItem> sliceItems)
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

            if (histDataQueue.Enabled && !(arcReplicationOptions.Enabled && arcReplicationOptions.AutoExport))
            {
                DateTime utcNow = DateTime.UtcNow;

                if (!histDataQueue.Enqueue(utcNow, new SliceItem(slice), out string errMsg))
                    exporterLog.WriteError(errMsg);

                if (histDataExportOptions.IncludeCalculated)
                {
                    lock (histTimestamps)
                    {
                        histTimestamps[slice.Timestamp] = utcNow;
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues the event to be exported.
        /// </summary>
        public void EnqueueEvent(Event ev)
        {
            ArgumentNullException.ThrowIfNull(ev, nameof(ev));

            if (eventQueue.Enabled && !(arcReplicationOptions.Enabled && arcReplicationOptions.AutoExport))
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

            // handle exporter command
            if (!string.IsNullOrEmpty(command.CmdCode) &&
                command.CmdCode == exporterConfig.GeneralOptions.CmdCode)
            {
                commandResult.TransmitToClients = false;
                IDictionary<string, string> cmdArgs = command.GetCmdDataArgs();

                if (Enum.TryParse(cmdArgs.GetValueAsString("cmd"), out ExporterCommand exporterCommand))
                {
                    exporterLog.WriteAction(Locale.IsRussian ?
                        "Получена команда экспортёра: {0}" :
                        "Exporter command received: {0}", exporterCommand);

                    if (arcReplicationOptions.Enabled)
                    {
                        if (exporterCommand == ExporterCommand.ClearTaskQueue)
                            arcReplicator.ClearTaskQueue();
                        else if (ArcExportTask.CreateTask(exporterCommand, cmdArgs) is ArcExportTask task)
                            arcReplicator.EnqueueTask(task);
                    }
                }
                else
                {
                    exporterLog.WriteError(Locale.IsRussian ?
                        "Неизвестная команда экспортёра" :
                        "Unknown exporter command");
                }
            }

            // transfer command
            if (cmdQueue.Enabled)
            {
                if (!cmdQueue.Enqueue(DateTime.UtcNow, command, out string errMsg))
                    exporterLog.WriteError(errMsg);
            }
        }
    }
}
