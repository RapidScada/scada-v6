// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Data.Models;
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
        /// The minimum queue size.
        /// </summary>
        private const int MinQueueSize = 100;
        /// <summary>
        /// The number of queue items to send when performing a data transfer operation.
        /// </summary>
        private const int BundleSize = 10;
        /// <summary>
        /// The delay in case of a data transfer error, ms.
        /// </summary>
        private const int ErrorDelay = 1000;

        private readonly DriverConfig driverConfig; // the driver configuration
        private readonly ScadaServerDSO options;    // the data source options
        private readonly TimeSpan maxCurDataAge;    // determines sending current data as historical
        private readonly TimeSpan dataLifetime;     // the data lifetime in the queue
        private readonly HashSet<int> deviceFilter; // the device IDs to filter data
        private readonly ILog log;                  // the application log

        private readonly int maxQueueSize;                            // the maximum queue size
        private readonly Queue<QueueItem<DeviceSlice>> curDataQueue;  // the current data queue
        private readonly Queue<QueueItem<DeviceSlice>> histDataQueue; // the historical data queue
        private readonly Queue<QueueItem<DeviceEvent>> eventQueue;    // the event queue

        private ConnectionOptions connOptions; // the connection options
        private ScadaClient scadaClient;  // communicates with the server
        private Thread thread;            // the thread for communication with the server
        private volatile bool terminated; // necessary to stop the thread
        private int curDataSkipped;       // the number of skipped slices of current data 
        private int histDataSkipped;      // the number of skipped slices of historical data
        private int eventSkipped;         // the number of skipped events


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaServerDSL(ICommContext commContext, DataSourceConfig dataSourceConfig, DriverConfig driverConfig)
            : base(commContext, dataSourceConfig)
        {
            this.driverConfig = driverConfig ?? throw new ArgumentNullException(nameof(driverConfig));
            options = new ScadaServerDSO(dataSourceConfig.CustomOptions);
            maxCurDataAge = TimeSpan.FromSeconds(options.MaxCurDataAge);
            dataLifetime = TimeSpan.FromSeconds(options.DataLifetime);
            deviceFilter = options.DeviceFilter.Count > 0 ? new HashSet<int>(options.DeviceFilter) : null;
            log = commContext.Log;

            maxQueueSize = Math.Max(options.MaxQueueSize, MinQueueSize);
            curDataQueue = new Queue<QueueItem<DeviceSlice>>(maxQueueSize);
            histDataQueue = new Queue<QueueItem<DeviceSlice>>(maxQueueSize);
            eventQueue = new Queue<QueueItem<DeviceEvent>>(maxQueueSize);

            connOptions = null;
            scadaClient = null;
            thread = null;
            terminated = false;
            curDataSkipped = 0;
            histDataSkipped = 0;
            eventSkipped = 0;
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
                Capacity = CommContext.AppConfig.GeneralOptions.MaxLogSize
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
                const int MaxCommandCount = 100;
                int commandCount = 0;

                while (scadaClient.GetCommand() is TeleCommand cmd)
                {
                    CommContext.SendCommand(cmd, Code);

                    if (++commandCount == MaxCommandCount)
                        break;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при приёме команд ТУ" :
                    "Error receiving telecontrol commands");
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
            for (int i = 0; i < BundleSize; i++)
            {
                // get a slice from the queue without removing it
                QueueItem<DeviceSlice> queueItem;
                DeviceSlice deviceSlice;

                lock (curDataQueue)
                {
                    if (curDataQueue.Count > 0)
                    {
                        queueItem = curDataQueue.Peek();
                        deviceSlice = queueItem.Value;
                    }
                    else
                    {
                        break;
                    }
                }

                // export the slice
                DateTime utcNow = DateTime.UtcNow;

                if (utcNow - queueItem.CreationTime > dataLifetime)
                {
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Устаревшие текущие данные удалены из очереди" :
                        "Outdated current data removed from the queue");

                    curDataSkipped++;
                }
                else if (ConvertSlice(deviceSlice, out Slice slice))
                {
                    try
                    {
                        if (utcNow - queueItem.CreationTime > maxCurDataAge)
                        {
                            scadaClient.WriteHistoricalData(deviceSlice.ArchiveMask, slice, 
                                deviceSlice.DeviceNum, WriteFlags.EnableAll);
                        }
                        else
                        {
                            scadaClient.WriteCurrentData(slice.CnlNums, slice.CnlData, 
                                deviceSlice.DeviceNum, WriteFlags.EnableAll);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                            "Ошибка при передаче текущих данных" :
                            "Error transferring current data");

                        Thread.Sleep(ErrorDelay);
                        break; // the slice is not removed from the queue
                    }
                }

                // remove the slice from the queue
                lock (curDataQueue)
                {
                    if (curDataQueue.Count > 0 && curDataQueue.Peek() == queueItem)
                        curDataQueue.Dequeue();
                }
            }
        }

        /// <summary>
        /// Transfers historical data to the server.
        /// </summary>
        private void TransferHistoricalData()
        {
            for (int i = 0; i < BundleSize; i++)
            {
                // retrieve a slice from the queue
                QueueItem<DeviceSlice> queueItem;
                DeviceSlice deviceSlice;

                lock (histDataQueue)
                {
                    if (histDataQueue.Count > 0)
                    {
                        queueItem = histDataQueue.Dequeue();
                        deviceSlice = queueItem.Value;
                    }
                    else
                    {
                        break;
                    }
                }

                // export the slice
                if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                {
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Устаревшие архивные данные удалены из очереди" :
                        "Outdated historical data removed from the queue");

                    histDataSkipped++;
                    CallFailedToSend(deviceSlice);
                }
                else if (ConvertSlice(deviceSlice, out Slice slice))
                {
                    try
                    {
                        scadaClient.WriteHistoricalData(deviceSlice.ArchiveMask, slice,
                            deviceSlice.DeviceNum, WriteFlags.EnableAll);
                        CallDataSent(deviceSlice);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                            "Ошибка при передаче архивных данных" :
                            "Error transferring historical data");

                        // return the unsent slice to the queue
                        lock (histDataQueue)
                        {
                            histDataQueue.Enqueue(queueItem);
                        }

                        Thread.Sleep(ErrorDelay);
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
            for (int i = 0; i < BundleSize; i++)
            {
                // retrieve an event from the queue
                QueueItem<DeviceEvent> queueItem;
                DeviceEvent deviceEvent;

                lock (eventQueue)
                {
                    if (eventQueue.Count > 0)
                    {
                        queueItem = eventQueue.Dequeue();
                        deviceEvent = queueItem.Value;
                    }
                    else
                    {
                        break;
                    }
                }

                // export the event
                if (DateTime.UtcNow - queueItem.CreationTime > dataLifetime)
                {
                    log.WriteError(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                        "Устаревшее событие удалено из очереди" :
                        "Outdated event removed from the queue");

                    eventSkipped++;
                    CallFailedToSend(deviceEvent);
                }
                else if (deviceEvent.DeviceTag?.Cnl != null)
                {
                    try
                    {
                        deviceEvent.CnlNum = deviceEvent.DeviceTag.Cnl.CnlNum;
                        scadaClient.WriteEvent(deviceEvent.ArchiveMask, deviceEvent);
                        CallEventSent(deviceEvent);
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                            "Ошибка при передаче события" :
                            "Error transferring event");

                        // return the unsent event to the queue
                        lock (eventQueue)
                        {
                            eventQueue.Enqueue(queueItem);
                        }

                        Thread.Sleep(ErrorDelay);
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
                    destSlice = new Slice(srcSlice.Timestamp, cnlNums.ToArray(), srcSlice.CnlData);
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

                    destSlice = new Slice(srcSlice.Timestamp, cnlNums.ToArray(), destCnlData);
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
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            bool cmdEnabled = CommContext.AppConfig.GeneralOptions.EnableCommands;

            while (!terminated)
            {
                if (scadaClient.IsReady)
                {
                    if (cmdEnabled)
                        ReceiveCommands();

                    TransferData();
                }

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
            scadaClient = new ScadaClient(connOptions);

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

            scadaClient?.Close();
        }

        /// <summary>
        /// Reads the configuration database.
        /// </summary>
        public override bool ReadBase(out BaseDataSet baseDataSet)
        {
            // do not read the configuration database from the server that likely contains partial data
            if (deviceFilter != null)
            {
                baseDataSet = null;
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
                    baseDataSet = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex.BuildErrorMessage(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "Ошибка при проверке соединения с сервером" :
                    "Error checking server connection"));
                baseDataSet = null;
                return false;
            }

            // receive tables
            string tableName = CommonPhrases.UndefinedTable;

            try
            {

                baseDataSet = new BaseDataSet();

                foreach (IBaseTable baseTable in baseDataSet.AllTables)
                {
                    tableName = baseTable.Name;
                    localClient.DownloadBaseTable(baseTable);
                }

                log.WriteAction(CommPhrases.DataSourceMessage, Code, Locale.IsRussian ?
                    "База конфигурации получена успешно" :
                    "The configuration database has been received successfully");
                return true;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommPhrases.DataSourceMessage, Code, string.Format(Locale.IsRussian ?
                    "Ошибка при приёме базы конфигурации, таблица {0}" :
                    "Error receiving the configuration database, the {0} table", tableName));
                baseDataSet = null;
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
            {
                lock (curDataQueue)
                {
                    // remove current data from the beginning of the queue
                    while (curDataQueue.Count >= maxQueueSize)
                    {
                        curDataQueue.Dequeue();
                        curDataSkipped++;
                    }

                    // append current data
                    curDataQueue.Enqueue(new QueueItem<DeviceSlice>(deviceSlice.Timestamp, deviceSlice));
                }
            }
        }

        /// <summary>
        /// Writes the slice of historical data.
        /// </summary>
        public override void WriteHistoricalData(DeviceSlice deviceSlice)
        {
            if (CheckDeviceFilter(deviceSlice.DeviceNum))
            {
                lock (histDataQueue)
                {
                    if (histDataQueue.Count < maxQueueSize)
                    {
                        histDataQueue.Enqueue(new QueueItem<DeviceSlice>(DateTime.UtcNow, deviceSlice));
                    }
                    else
                    {
                        log.WriteError(CommPhrases.DataSourceMessage, Code, string.Format(Locale.IsRussian ?
                            "Невозможно добавить архивные данные в очередь. Максимальный размер очереди {0} превышен" :
                            "Unable to enqueue historical data. The maximum size of the queue {0} is exceeded",
                            maxQueueSize));

                        histDataSkipped++;
                        CallFailedToSend(deviceSlice);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public override void WriteEvent(DeviceEvent deviceEvent)
        {
            if (CheckDeviceFilter(deviceEvent.DeviceNum))
            {
                lock (eventQueue)
                {
                    if (eventQueue.Count < maxQueueSize)
                    {
                        eventQueue.Enqueue(new QueueItem<DeviceEvent>(DateTime.UtcNow, deviceEvent));
                    }
                    else
                    {
                        log.WriteError(CommPhrases.DataSourceMessage, Code, string.Format(Locale.IsRussian ?
                            "Невозможно добавить событие в очередь. Максимальный размер очереди {0} превышен" :
                            "Unable to enqueue an event. The maximum size of the queue {0} is exceeded",
                            maxQueueSize));

                        eventSkipped++;
                        CallFailedToSend(deviceEvent);
                    }
                }
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
                    sb.Append("Соединение              : не используется").AppendLine();
                else
                    sb.Append("Соединение              : ").AppendLine(scadaClient.ClientState.ToString(true));

                sb.Append("Очередь текущих данных  : ")
                    .Append(curDataQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(curDataSkipped).AppendLine();

                sb.Append("Очередь архивных данных : ")
                    .Append(histDataQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(histDataSkipped).AppendLine();

                sb.Append("Очередь событий         : ")
                    .Append(eventQueue.Count).Append(" из ").Append(maxQueueSize)
                    .Append(", пропущено ").Append(eventSkipped).AppendLine();
            }
            else
            {
                if (scadaClient == null)
                    sb.Append("Connection            : Not Used").AppendLine();
                else
                    sb.Append("Connection            : ").AppendLine(scadaClient.ClientState.ToString(false));

                sb.Append("Current data queue    : ")
                    .Append(curDataQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(curDataSkipped).AppendLine();

                sb.Append("Historical data queue : ")
                    .Append(histDataQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(histDataSkipped).AppendLine();

                sb.Append("Event queue           : ")
                    .Append(eventQueue.Count).Append(" of ").Append(maxQueueSize)
                    .Append(", skipped ").Append(eventSkipped).AppendLine();
            }
        }
    }
}
