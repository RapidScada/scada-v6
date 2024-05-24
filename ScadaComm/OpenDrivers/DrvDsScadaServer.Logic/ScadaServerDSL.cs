// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvDsScadaServer.Config;
using Scada.Comm.Lang;
using Scada.Data.Models;
using Scada.Data.Queues;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Scada.Comm.Drivers.DrvDsScadaServer.Logic
{
    /// <summary>
    /// Implements the data source logic.
    /// <para>Реализует логику источника данных.</para>
    /// </summary>
    internal class ScadaServerDSL : DataSourceLogic
    {
        /// <summary>
        /// The number of channels to send when writing current data.
        /// </summary>
        private const int ChannelBatchSize = 5000;
        /// <summary>
        /// The number of write requests sent in one iteration.
        /// </summary>
        private const int WritesPerIteration = 10;
        /// <summary>
        /// The maximum number of commands received in one iteration.
        /// </summary>
        private const int CommandsPerIteration = 100;

        private readonly DriverConfig driverConfig; // the driver configuration
        private readonly ScadaServerDSO options;    // the data source options
        private readonly TimeSpan dataLifetime;     // the data lifetime in the queue
        private readonly HashSet<int> deviceFilter; // the device IDs to filter data
        private readonly ILog log;                  // the application log

        private readonly DataQueue<DeviceSlice> curDataQueue;  // the current data queue
        private readonly DataQueue<DeviceSlice> histDataQueue; // the historical data queue
        private readonly DataQueue<DeviceEvent> eventQueue;    // the event queue
        private readonly List<Slice> slicesToSend;             // the slices of current data dequeued for sending

        private ConnectionOptions connOptions; // the connection options
        private ScadaClient scadaClient;       // communicates with the server
        private Thread thread;                 // the thread for communication with the server
        private volatile bool terminated;      // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaServerDSL(ICommContext commContext, DataSourceConfig dataSourceConfig, DriverConfig driverConfig)
            : base(commContext, dataSourceConfig)
        {
            this.driverConfig = driverConfig ?? throw new ArgumentNullException(nameof(driverConfig));
            options = new ScadaServerDSO(dataSourceConfig.CustomOptions);
            dataLifetime = TimeSpan.FromSeconds(options.DataLifetime);
            deviceFilter = options.DeviceFilter.Count > 0 ? new HashSet<int>(options.DeviceFilter) : null;
            log = commContext.Log;

            curDataQueue = new DataQueue<DeviceSlice>(true, options.MaxQueueSize,
                Locale.IsRussian ? "Очередь текущих данных" : "Current data queue") { RemoveExceeded = true };
            histDataQueue = new DataQueue<DeviceSlice>(true, options.MaxQueueSize,
                Locale.IsRussian ? "Очередь исторических данных" : "Historical data queue");
            eventQueue = new DataQueue<DeviceEvent>(true, options.MaxQueueSize,
                Locale.IsRussian ? "Очередь событий" : "Event queue");
            slicesToSend = new List<Slice>();

            connOptions = null;
            scadaClient = null;
            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Creates a client communication log file.
        /// </summary>
        private ILog CreateClientLog()
        {
            return new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(CommContext.AppDirs.LogDir, DriverUtils.DriverCode + "_" + Code + "_Client.log"),
                TimestampFormat = LogFile.DefaultTimestampFormat + "'.'ff",
                CapacityMB = CommContext.AppConfig.GeneralOptions.MaxLogSize
            };
        }

        /// <summary>
        /// Checks if the device filter allows the specified device number.
        /// </summary>
        private bool CheckDeviceFilter(int deviceNum)
        {
            return deviceFilter == null || deviceFilter.Contains(deviceNum);
        }

        /// <summary>
        /// Receives telecontrol commands from the server.
        /// </summary>
        private void ReceiveCommands()
        {
            try
            {
                int cmdCnt = 0;

                while (scadaClient.GetCommand() is TeleCommand cmd)
                {
                    CommContext.SendCommand(cmd, Code);

                    if (++cmdCnt == CommandsPerIteration)
                        break;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex.BuildErrorMessage(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при приёме команд ТУ" :
                    "Error receiving telecontrol commands"));
            }
        }

        /// <summary>
        /// Transfers data to the server.
        /// </summary>
        private void TransferData()
        {
            try
            {
                TransferCurrentData();
                TransferHistoricalData();
                TransferEvents();
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при передаче данных" :
                    "Error transferring data");
            }
        }

        /// <summary>
        /// Transfers current data to the server.
        /// </summary>
        private void TransferCurrentData()
        {
            for (int i = 0; i < WritesPerIteration; i++)
            {
                DateTime utcNow = DateTime.UtcNow;

                // check the age of unsent slices
                if (slicesToSend.Count > 0 && utcNow - slicesToSend[0].Timestamp > dataLifetime)
                {
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Неотправленные текущие данные удалены" :
                        "Unsent current data removed");

                    curDataQueue.Stats.SkippedItems += slicesToSend.Count;
                    slicesToSend.Clear();
                }

                // dequeue slices for sending
                if (slicesToSend.Count == 0)
                {
                    int totalCnlCnt = 0;

                    while (totalCnlCnt < ChannelBatchSize &&
                        curDataQueue.TryDequeue(out QueueItem<DeviceSlice> queueItem))
                    {
                        if (utcNow - queueItem.CreationTime > dataLifetime)
                        {
                            log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                                "Устаревшие текущие данные удалены из очереди" :
                                "Outdated current data removed from the queue");

                            curDataQueue.Stats.SkippedItems++;
                        }
                        else if (ConvertSlice(queueItem.Value, out Slice slice))
                        {
                            slicesToSend.Add(slice);
                            totalCnlCnt += slice.Length;
                        }
                    }

                    if (slicesToSend.Count == 0)
                        break;
                }

                // send the slices
                try
                {
                    scadaClient.WriteCurrentData(slicesToSend, WriteDataFlags.Default);
                    curDataQueue.Stats.ExportedItems += slicesToSend.Count;
                    curDataQueue.Stats.HasError = false;
                    slicesToSend.Clear();
                }
                catch (Exception ex)
                {
                    log.WriteError(ex.BuildErrorMessage(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Ошибка при передаче текущих данных" :
                        "Error transferring current data"));

                    curDataQueue.Stats.HasError = true;
                    Thread.Sleep(ScadaUtils.ErrorDelay);
                    break;
                }
            }
        }

        /// <summary>
        /// Transfers historical data to the server.
        /// </summary>
        private void TransferHistoricalData()
        {
            for (int i = 0; i < WritesPerIteration; i++)
            {
                // retrieve an item from the queue
                if (!histDataQueue.TryDequeue(out QueueItem<DeviceSlice> queueItem))
                    break;

                DeviceSlice deviceSlice = queueItem.Value;

                if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                {
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Устаревшие исторические данные удалены из очереди" :
                        "Outdated historical data removed from the queue");

                    histDataQueue.Stats.SkippedItems++;
                    CallFailedToSend(deviceSlice);
                }
                else if (ConvertSlice(deviceSlice, out Slice slice))
                {
                    try
                    {
                        // send the slice
                        scadaClient.WriteHistoricalData(deviceSlice.ArchiveMask, slice, WriteDataFlags.Default);
                        histDataQueue.Stats.ExportedItems++;
                        histDataQueue.Stats.HasError = false;
                        CallDataSent(deviceSlice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex.BuildErrorMessage(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                            "Ошибка при передаче исторических данных" :
                            "Error transferring historical data"));

                        // return the unsent item to the queue
                        histDataQueue.ReturnItem(queueItem);
                        histDataQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Transfers events to the server.
        /// </summary>
        private void TransferEvents()
        {
            for (int i = 0; i < WritesPerIteration; i++)
            {
                // retrieve an item from the queue
                if (!eventQueue.TryDequeue(out QueueItem<DeviceEvent> queueItem))
                    break;

                DeviceEvent deviceEvent = queueItem.Value;

                if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                {
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Устаревшее событие удалено из очереди" :
                        "Outdated event removed from the queue");

                    eventQueue.Stats.SkippedItems++;
                    CallFailedToSend(deviceEvent);
                }
                else if (deviceEvent.DeviceTag?.Cnl != null)
                {
                    try
                    {
                        // send the event
                        deviceEvent.CnlNum = deviceEvent.DeviceTag.Cnl.CnlNum;
                        scadaClient.WriteEvent(deviceEvent.ArchiveMask, deviceEvent);
                        eventQueue.Stats.ExportedItems++;
                        eventQueue.Stats.HasError = false;
                        CallEventSent(deviceEvent);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex.BuildErrorMessage(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                            "Ошибка при передаче события" :
                            "Error transferring event"));

                        // return the unsent item to the queue
                        eventQueue.ReturnItem(queueItem);
                        eventQueue.Stats.HasError = true;
                        Thread.Sleep(ScadaUtils.ErrorDelay);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Converts the device slice to a general purpose slice.
        /// </summary>
        private bool ConvertSlice(DeviceSlice srcSlice, out Slice destSlice)
        {
            try
            {
                int srcDataLength = srcSlice.CnlData.Length;
                int destDataLength = 0;
                List<int> cnlNums = new List<int>(srcDataLength);

                foreach (DeviceTag deviceTag in srcSlice.DeviceTags)
                {
                    if (deviceTag == null)
                    {
                        throw new ScadaException(Locale.IsRussian ?
                            "Неопределенные теги в срезе не допускаются." :
                            "Undefined tags are not allowed in a slice.");
                    }

                    if (deviceTag.Cnl != null)
                    {
                        int tagDataLength = deviceTag.DataLength;

                        for (int i = 0; i < tagDataLength; i++)
                        {
                            cnlNums.Add(deviceTag.Cnl.CnlNum + i);
                        }

                        destDataLength += tagDataLength;
                    }
                }

                if (destDataLength == 0)
                {
                    destSlice = null;
                    return false;
                }
                else if (destDataLength == srcDataLength)
                {
                    destSlice = new Slice(srcSlice.Timestamp, cnlNums.ToArray(), srcSlice.CnlData)
                    {
                        DeviceNum = srcSlice.DeviceNum
                    };
                    return true;
                }
                else
                {
                    CnlData[] destCnlData = new CnlData[destDataLength];
                    int srcDataIndex = 0;
                    int destDataIndex = 0;

                    foreach (DeviceTag deviceTag in srcSlice.DeviceTags)
                    {
                        int tagDataLength = deviceTag.DataLength;

                        if (deviceTag.Cnl != null)
                        {
                            Array.Copy(srcSlice.CnlData, srcDataIndex, destCnlData, destDataIndex, tagDataLength);
                            destDataIndex += tagDataLength;
                        }

                        srcDataIndex += tagDataLength;
                    }

                    destSlice = new Slice(srcSlice.Timestamp, cnlNums.ToArray(), destCnlData)
                    {
                        DeviceNum = srcSlice.DeviceNum
                    };
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, string.Format(Locale.IsRussian ?
                    "Ошибка при конвертировании среза от устройства {0}" :
                    "Error converting slice from the device {0}", srcSlice.DeviceNum));
                destSlice = null;
                return false;
            }
        }

        /// <summary>
        /// Calls the DataSentCallback method of the device slice.
        /// </summary>
        private void CallDataSent(DeviceSlice deviceSlice)
        {
            try
            {
                deviceSlice.DataSentCallback?.Invoke(deviceSlice, Code);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при вызове метода среза DataSentCallback" :
                    "Error calling the DataSentCallback method of the slice");
            }
        }

        /// <summary>
        /// Calls the DataSentCallback method of the device event.
        /// </summary>
        private void CallEventSent(DeviceEvent deviceEvent)
        {
            try
            {
                deviceEvent.EventSentCallback?.Invoke(deviceEvent, Code);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при вызове метода события EventSentCallback" :
                    "Error calling the EventSentCallback method of the event");
            }
        }

        /// <summary>
        /// Calls the FailedToSendCallback method of the device slice.
        /// </summary>
        private void CallFailedToSend(DeviceSlice deviceSlice)
        {
            try
            {
                deviceSlice.FailedToSendCallback?.Invoke(deviceSlice, Code);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при вызове метода среза FailedToSendCallback" :
                    "Error calling the FailedToSendCallback method of the slice");
            }
        }

        /// <summary>
        /// Calls the FailedToSendCallback method of the device event.
        /// </summary>
        private void CallFailedToSend(DeviceEvent deviceEvent)
        {
            try
            {
                deviceEvent.FailedToSendCallback?.Invoke(deviceEvent, Code);
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при вызове метода события FailedToSendCallback" :
                    "Error calling the FailedToSendCallback method of the event");
            }
        }

        /// <summary>
        /// Handles a ClientStateChanged event.
        /// </summary>
        private void ClientStateChanged(object sender, EventArgs e)
        {
            log.WriteAction(CommPhrases.DataSourceMessage, Code, string.Format(Locale.IsRussian ?
                "Состояние соединения: {0}" :
                "Connection state is {0}",
                scadaClient.ClientState.ToString(Locale.IsRussian)));
        }

        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            bool cmdEnabled = CommContext.AppConfig.GeneralOptions.EnableCommands;

            while (!terminated)
            {
                if (scadaClient.IsReady && cmdEnabled)
                    ReceiveCommands();

                if (scadaClient.IsReady)
                    TransferData();

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }


        /// <summary>
        /// Makes the data source ready for operating.
        /// </summary>
        public override void MakeReady()
        {
            if (options.UseDefaultConn)
                connOptions = CommContext.AppConfig.ConnectionOptions;
            else if (!driverConfig.Connections.TryGetValue(options.Connection, out connOptions))
                throw new ScadaException(CommonPhrases.ConnectionNotFound, options.Connection);
        }

        /// <summary>
        /// Starts the data source.
        /// </summary>
        public override void Start()
        {
            scadaClient = new ScadaClient(connOptions) { ClientMode = ScadaClientMode.IncomingCommands };
            scadaClient.ClientStateChanged += ClientStateChanged;

            if (options.ClientLogEnabled)
                scadaClient.CommLog = CreateClientLog();

            terminated = false;
            thread = new Thread(Execute);
            thread.Start();
        }

        /// <summary>
        /// Closes the data source.
        /// </summary>
        public override void Close()
        {
            if (thread != null)
            {
                terminated = true;
                thread.Join();
                thread = null;
            }

            if (scadaClient != null)
            {
                scadaClient.ClientStateChanged -= ClientStateChanged;
                scadaClient?.Close();
            }
        }

        /// <summary>
        /// Reads the configuration database.
        /// </summary>
        public override bool ReadConfigDatabase(out ConfigDatabase configDatabase)
        {
            if (!options.ReadConfigDb)
            {
                configDatabase = null;
                return false;
            }

            log.WriteAction(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                "Приём базы конфигурации" :
                "Receive the configuration database");

            // check connection
            ScadaClient localClient = new ScadaClient(connOptions);

            try
            {
                localClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                {
                    localClient.TerminateSession();
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Сервер не готов" :
                        "Server is not ready");
                    configDatabase = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex.BuildErrorMessage(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при проверке соединения с сервером" :
                    "Error checking server connection"));
                configDatabase = null;
                return false;
            }

            // receive tables
            string tableTitle = CommonPhrases.UndefinedTable;

            try
            {
                configDatabase = new ConfigDatabase();

                foreach (IBaseTable baseTable in configDatabase.AllTables)
                {
                    tableTitle = baseTable.Title;
                    localClient.DownloadBaseTable(baseTable);
                }

                tableTitle = CommonPhrases.UndefinedTable;
                configDatabase.Init();

                log.WriteAction(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "База конфигурации получена успешно" :
                    "The configuration database has been received successfully");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, string.Format(Locale.IsRussian ?
                    "Ошибка при приёме базы конфигурации, таблица {0}" :
                    "Error receiving the configuration database, the {0} table", tableTitle));
                configDatabase = null;
                return false;
            }
            finally
            {
                localClient.TerminateSession();
            }
        }

        /// <summary>
        /// Writes the slice of the current data.
        /// </summary>
        public override void WriteCurrentData(DeviceSlice deviceSlice)
        {
            if (CheckDeviceFilter(deviceSlice.DeviceNum))
                curDataQueue.Enqueue(deviceSlice.Timestamp, deviceSlice, out _);
        }

        /// <summary>
        /// Writes the slice of historical data.
        /// </summary>
        public override void WriteHistoricalData(DeviceSlice deviceSlice)
        {
            if (CheckDeviceFilter(deviceSlice.DeviceNum) &&
                !histDataQueue.Enqueue(DateTime.UtcNow, deviceSlice, out string errMsg))
            {
                log.WriteError(CommPhrases.DataSourceMessage, Code, errMsg);
                CallFailedToSend(deviceSlice);
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public override void WriteEvent(DeviceEvent deviceEvent)
        {
            if (CheckDeviceFilter(deviceEvent.DeviceNum) &&
                !eventQueue.Enqueue(DateTime.UtcNow, deviceEvent, out string errMsg))
            {
                log.WriteError(CommPhrases.DataSourceMessage, Code, errMsg);
                CallFailedToSend(deviceEvent);
            }
        }

        /// <summary>
        /// Appends information about the data source to the string builder.
        /// </summary>
        public override void AppendInfo(StringBuilder sb)
        {
            sb
                .AppendLine()
                .AppendLine(DataSourceConfig.Name)
                .Append('-', DataSourceConfig.Name.Length).AppendLine();

            if (Locale.IsRussian)
            {
                if (scadaClient == null)
                    sb.Append("Соединение                  : не используется").AppendLine();
                else
                    sb.Append("Соединение                  : ").AppendLine(scadaClient.ClientState.ToString(true));

                curDataQueue.AppendShortInfo(sb, 28);
                histDataQueue.AppendShortInfo(sb, 28);
                eventQueue.AppendShortInfo(sb, 28);
            }
            else
            {
                if (scadaClient == null)
                    sb.Append("Connection            : Not Used").AppendLine();
                else
                    sb.Append("Connection            : ").AppendLine(scadaClient.ClientState.ToString(false));

                curDataQueue.AppendShortInfo(sb, 22);
                histDataQueue.AppendShortInfo(sb, 22);
                eventQueue.AppendShortInfo(sb, 22);
            }
        }
    }
}
