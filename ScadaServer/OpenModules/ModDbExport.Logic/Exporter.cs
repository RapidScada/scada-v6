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

namespace Scada.Server.Modules.ModDbExport.Logic
{
    /// <summary>
    /// Exports data to a one target.
    /// <para>Экспортирует данные в одну базу данных.</para>
    /// </summary>
    internal sealed class Exporter : IExporterContext
    {
        private readonly ExportTargetConfig exporterConfig;    // the exporter configuration
        private readonly IServerContext serverContext;         // the application context

        private readonly string exporterTitle;                 // the title of the exporter
        private readonly TimeSpan dataLifetime;                // the data lifetime in the queue
        private readonly CurDataExportOptions curDataExportOptions;   // the current data export options
        private readonly ArcReplicationOptions arcReplicationOptions; // the archive replication options

        private readonly DataSource dataSource;                // provides access to the DB
        private readonly ClassifiedQueries classifiedQueries;  // the queries grouped by classes
        private readonly DataQueue<Slice> curDataQueue;        // the current data queue
        private readonly DataQueue<Slice> histDataQueue;       // the historical data queue
        private readonly DataQueue<Event> eventQueue;          // the event queue
        private readonly DataQueue<EventAck> eventAckQueue;    // the event acknowledgement queue
        private readonly DataQueue<TeleCommand> cmdQueue;      // the command queue

        private readonly ILog exporterLog;                     // the exporter log
        private readonly string filePrefix;                    // the prefix of the exporter files
        private readonly string infoFileName;                  // the information file name
        private readonly Dictionary<int, CnlData> prevCnlData; // the previous channel data
        private readonly ArcReplicator arcReplicator;          // replicates archives

        private ICollection<int> calcCnlNums;  // the calculated channel numbers exported on change
        private ICollection<int> timerCnlNums; // the channel numbers exported on timer
        private DateTime allDataDT;            // the timestamp of exporting all channel data
        private Thread exporterThread;         // the working thread of the exporter
        private volatile bool terminated;      // necessary to stop the thread


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
            curDataQueue = new DataQueue<Slice>(classifiedQueries.CurDataQueries.IsNotEmpty(), maxQueueSize,
                Locale.IsRussian ? "Текущие данные" : "Current Data") { RemoveExceeded = true };
            histDataQueue = new DataQueue<Slice>(
                classifiedQueries.HistDataQueries.IsNotEmpty() || arcReplicationOptions.Enabled, maxQueueSize,
                Locale.IsRussian ? "Исторические данные" : "Historical Data");
            eventQueue = new DataQueue<Event>(
                classifiedQueries.EventQueries.IsNotEmpty() || arcReplicationOptions.Enabled, maxQueueSize,
                Locale.IsRussian ? "События" : "Events");
            eventAckQueue = new DataQueue<EventAck>(
                classifiedQueries.EventAckQueries.IsNotEmpty(), maxQueueSize,
                Locale.IsRussian ? "Квитирование событий" : "Event Acknowledgements");
            cmdQueue = new DataQueue<TeleCommand>(classifiedQueries.CmdQueries.IsNotEmpty(), maxQueueSize,
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

            calcCnlNums = null;
            timerCnlNums = null;
            allDataDT = DateTime.MinValue;
            exporterThread = null;
            terminated = false;
        }


        /// <summary>
        /// Starts the exporter.
        /// </summary>
        public void Start()
        {

        }

        /// <summary>
        /// Stops the exporter.
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// Enqueues the received current data to be exported.
        /// </summary>
        public void EnqueueCurrentData(Slice slice)
        {
            ArgumentNullException.ThrowIfNull(slice, nameof(slice));

        }

        /// <summary>
        /// Enqueues the calculated current data to be exported.
        /// </summary>
        public void EnqueueCalculatedData()
        {

        }

        /// <summary>
        /// Enqueues the historical data to be exported.
        /// </summary>
        public void EnqueueHistoricalData(Slice slice)
        {
            ArgumentNullException.ThrowIfNull(slice, nameof(slice));

        }

        /// <summary>
        /// Enqueues the event to be exported.
        /// </summary>
        public void EnqueueEvent(Event ev)
        {
            ArgumentNullException.ThrowIfNull(ev, nameof(ev));

        }

        /// <summary>
        /// Enqueues the event acknowledgement to be exported.
        /// </summary>
        public void EnqueueEventAck(EventAck eventAck)
        {
            ArgumentNullException.ThrowIfNull(eventAck, nameof(eventAck));

        }

        /// <summary>
        /// Enqueues the command to be exported or executed.
        /// </summary>
        public void EnqueueCommand(TeleCommand command, CommandResult commandResult)
        {
            ArgumentNullException.ThrowIfNull(command, nameof(command));
            ArgumentNullException.ThrowIfNull(commandResult, nameof(commandResult));

        }
    }
}
