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
 * Module   : ScadaCommEngine
 * Summary  : Implements the core Communicator logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Comm.Config;
using Scada.Comm.DataSources;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Storages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Implements the core Communicator logic.
    /// <para>Реализует основную логику Коммуникатора.</para>
    /// </summary>
    internal class CoreLogic : ICommContext
    {
        /// <summary>
        /// Represents information associated with a device.
        /// </summary>
        private class DeviceItem
        {
            public DeviceItem(DeviceLogic deviceLogic, CommLine commLine)
            {
                Device = deviceLogic;
                Line = commLine;
            }
            public DeviceLogic Device { get; set; }
            public CommLine Line { get; set; }
        }

        /// <summary>
        /// Specifies the operating steps.
        /// </summary>
        private enum OperatingStep { ReadBase, StartLines, MainWork, FatalError }

        /// <summary>
        /// The period of attempts to read the configuration database.
        /// </summary>
        private static readonly TimeSpan ReadBasePeriod = TimeSpan.FromSeconds(10);

        private readonly string infoFileName; // the full file name to write application information
        private readonly object commLineLock; // syncronizes access to communication lines

        private Thread thread;                // the working thread of the logic
        private volatile bool terminated;     // necessary to stop the thread
        private DateTime utcStartDT;          // the UTC start time
        private DateTime startDT;             // the local start time
        private ServiceStatus serviceStatus;  // the current service status
        private int lastInfoLength;           // the last info text length
        private int maxLineTitleLength;       // the maximum length of communication line title

        private List<CommLine> commLines;              // the active communication lines
        private Dictionary<int, CommLine> commLineMap; // the communication lines accessed by line number
        private Dictionary<int, DeviceItem> deviceMap; // the devices accessed by device number
        private Queue<TeleCommand> appCmdQueue;        // the queue containing application commands
        private CommandReader commandReader;           // reads telecontrol commands from files
        private DriverHolder driverHolder;             // holds drivers
        private DataSourceHolder dataSourceHolder;     // holds data sources


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(CommConfig appConfig, CommDirs appDirs, IStorage storage, ILog log)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            AppDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            ConfigDatabase = null;
            SharedData = null;

            infoFileName = Path.Combine(appDirs.LogDir, CommUtils.InfoFileName);
            commLineLock = new object();

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            serviceStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;
            maxLineTitleLength = -1;

            commLines = null;
            commLineMap = null;
            deviceMap = null;
            appCmdQueue = null;
            commandReader = null;
            driverHolder = null;
            dataSourceHolder = null;
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public CommConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the application storage.
        /// </summary>
        public IStorage Storage { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        public ConfigDatabase ConfigDatabase { get; private set; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public IDictionary<string, object> SharedData { get; private set; }


        /// <summary>
        /// Prepares the logic processing.
        /// </summary>
        private void PrepareProcessing()
        {
            terminated = false;
            utcStartDT = DateTime.UtcNow;
            startDT = utcStartDT.ToLocalTime();
            serviceStatus = ServiceStatus.Starting;
            WriteInfo();

            ConfigDatabase = null;
            SharedData = new ConcurrentDictionary<string, object>();

            commLines = new List<CommLine>(AppConfig.Lines.Count);
            commLineMap = new Dictionary<int, CommLine>(AppConfig.Lines.Count);
            deviceMap = new Dictionary<int, DeviceItem>();

            if (AppConfig.GeneralOptions.EnableCommands)
            {
                appCmdQueue = new Queue<TeleCommand>();
                commandReader = AppConfig.GeneralOptions.EnableFileCommands ? new CommandReader(this) : null;
            }

            InitDrivers();
            InitDataSources();
        }
        
        /// <summary>
        /// Initializes the drivers used in the application configuration.
        /// </summary>
        private void InitDrivers()
        {
            InitDrivers(AppConfig.GetDriverCodes(), out _);
        }

        /// <summary>
        /// Initializes the specified drivers.
        /// </summary>
        private void InitDrivers(List<string> driverCodes, out List<DriverWrapper> addedDrivers)
        {
            if (driverHolder == null)
                driverHolder = new DriverHolder(Log);

            addedDrivers = new List<DriverWrapper>();

            foreach (string driverCode in driverCodes)
            {
                if (!driverHolder.DriverExists(driverCode))
                {
                    if (DriverFactory.GetDriverLogic(AppDirs.DrvDir, driverCode, this,
                        out DriverLogic driverLogic, out string message))
                    {
                        Log.WriteAction(message);
                        addedDrivers.Add(driverHolder.AddDriver(driverLogic));
                    }
                    else
                    {
                        Log.WriteError(message);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes data sources.
        /// </summary>
        private void InitDataSources()
        {
            if (dataSourceHolder == null)
                dataSourceHolder = new DataSourceHolder(Log);

            foreach (DataSourceConfig dataSourceConfig in AppConfig.DataSources)
            {
                if (dataSourceConfig.Active)
                {
                    if (dataSourceHolder.DataSourceExists(dataSourceConfig.Code))
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Источник данных {0} дублируется" :
                            "Data source {0} is duplicated", dataSourceConfig.Code);
                    }
                    else if (driverHolder.GetDriver(dataSourceConfig.Driver, out DriverWrapper driverWrapper) &&
                        driverWrapper.CreateDataSource(this, dataSourceConfig, out DataSourceLogic dataSourceLogic))
                    {
                        dataSourceHolder.AddDataSource(dataSourceLogic);
                        Log.WriteAction(Locale.IsRussian ?
                            "Источник данных {0} инициализирован успешно" :
                            "Data source {0} initialized successfully", dataSourceLogic.Code);
                    }
                    else
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Не удалось создать источник данных {0} с помощью драйвера {1}" :
                            "Unable to create data source {0} with the driver {1}",
                            dataSourceConfig.Code, dataSourceConfig.Driver);
                    }
                }
            }
        }

        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                OperatingStep operatingStep = AppConfig.GeneralOptions.IsBound ?
                    OperatingStep.ReadBase : OperatingStep.StartLines;
                DateTime readBaseDT = DateTime.MinValue;
                DateTime writeInfoDT = DateTime.MinValue;

                driverHolder.OnServiceStart();
                dataSourceHolder.MakeReady();
                serviceStatus = ServiceStatus.Normal;

                while (!terminated)
                {
                    try
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        switch (operatingStep)
                        {
                            case OperatingStep.MainWork:
                                // process application commands
                                if (appCmdQueue != null)
                                {
                                    TeleCommand appCmd = null;

                                    lock (appCmdQueue)
                                    {
                                        if (appCmdQueue.Count > 0)
                                            appCmd = appCmdQueue.Dequeue();
                                    }

                                    if (appCmd != null)
                                        ProcessAppCommand(appCmd);
                                }
                                break;

                            case OperatingStep.ReadBase:
                                if (dataSourceHolder.Count == 0)
                                {
                                    Log.WriteError(Locale.IsRussian ?
                                        "Работа невозможна, потому что отсутствуют активные источники данных" :
                                        "Execution is impossible because active data sources are missed");
                                    operatingStep = OperatingStep.FatalError;
                                    serviceStatus = ServiceStatus.Error;
                                }
                                else if (utcNow - readBaseDT >= ReadBasePeriod)
                                {
                                    readBaseDT = utcNow;

                                    if (dataSourceHolder.ReadConfigDatabase(out ConfigDatabase configDatabase))
                                    {
                                        ConfigDatabase = configDatabase;
                                        operatingStep = OperatingStep.StartLines;
                                        serviceStatus = ServiceStatus.Normal;
                                    }
                                    else
                                    {
                                        Log.WriteError(Locale.IsRussian ?
                                            "Невозможно запустить линии связи, потому что база конфигурации не получена" :
                                            "Unable to start communication lines because the configuration database is not received");
                                        serviceStatus = ServiceStatus.Error;
                                    }
                                }
                                break;

                            case OperatingStep.StartLines:
                                if (AppConfig.GeneralOptions.StartLinesOnCommand)
                                {
                                    Log.WriteAction(Locale.IsRussian ?
                                        "Ожидание команды на запуск линий связи" :
                                        "Waiting for command to start communication lines");
                                }
                                else
                                {
                                    StartLines();
                                }

                                dataSourceHolder.Start();
                                commandReader?.Start();
                                operatingStep = OperatingStep.MainWork;
                                break;

                            case OperatingStep.FatalError:
                                // do nothing
                                break;
                        }

                        // write application info
                        if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                        {
                            writeInfoDT = utcNow;
                            WriteInfo();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex, CommonPhrases.LogicLoopError);
                    }
                    finally
                    {
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, CommonPhrases.ThreadFatalError);
            }
            finally
            {
                WriteInfo();
                commandReader?.Stop();
                StopLines();
                dataSourceHolder.Close();
                driverHolder.OnServiceStop();
                serviceStatus = ServiceStatus.Terminated;
                WriteInfo();
            }
        }

        /// <summary>
        /// Processes the application command.
        /// </summary>
        private void ProcessAppCommand(TeleCommand cmd)
        {
            switch (cmd.CmdCode)
            {
                case CommCmdCode.StartLine:
                    StartLine((int)cmd.CmdVal);
                    dataSourceHolder.Refresh();
                    break;

                case CommCmdCode.StopLine:
                    StopLine((int)cmd.CmdVal);
                    dataSourceHolder.Refresh();
                    break;

                case CommCmdCode.RestartLine:
                    RestartLine((int)cmd.CmdVal);
                    dataSourceHolder.Refresh();
                    break;

                case CommCmdCode.StartAllLines:
                    StartLines();
                    dataSourceHolder.Refresh();
                    break;

                case CommCmdCode.StopAllLines:
                    StopLines();
                    dataSourceHolder.Refresh();
                    break;

                case CommCmdCode.PollDevice:
                    if (GetDeviceLine(cmd.DeviceNum, out CommLine commLine))
                        commLine.PollWithPriority(cmd.DeviceNum);
                    break;

                default:
                    Log.WriteError(Locale.IsRussian ?
                        "Неизвестная команда" :
                        "Unknown command");
                    break;
            }
        }

        /// <summary>
        /// Creates a communication line if it does not exists, and adds it to the lists.
        /// </summary>
        private bool CreateLine(LineConfig lineConfig, out CommLine commLine)
        {
            lock (commLineLock)
            {
                if (commLineMap.ContainsKey(lineConfig.CommLineNum))
                {
                    Log.WriteError(Locale.IsRussian ?
                        "Линия связи {0} уже создана" :
                        "Communication line {0} already created", lineConfig.Title);
                    commLine = null;
                    return false;
                }
                else if (CommLineFactory.GetCommLine(lineConfig, this, driverHolder, out commLine, out string errMsg))
                {
                    commLine.Terminated += CommLine_Terminated;
                    commLines.Add(commLine);
                    commLineMap.Add(lineConfig.CommLineNum, commLine);

                    foreach (DeviceLogic deviceLogic in commLine.SelectDevices())
                    {
                        // only one device instance is possible
                        deviceMap.Add(deviceLogic.DeviceNum, new DeviceItem(deviceLogic, commLine));
                    }

                    maxLineTitleLength = -1; // reset max length
                    return true;
                }
                else
                {
                    Log.WriteError(errMsg);
                    return false;
                }
            }
        }

        /// <summary>
        /// Handles a communication line termination event.
        /// </summary>
        private void CommLine_Terminated(object sender, EventArgs e)
        {
            if (sender is CommLine commLine)
            {
                // remove the line and its devices from the lists
                lock (commLineLock)
                {
                    maxLineTitleLength = -1; // reset max length
                    commLines.Remove(commLine);
                    commLineMap.Remove(commLine.CommLineNum);

                    foreach (DeviceLogic deviceLogic in commLine.SelectDevices())
                    {
                        deviceMap.Remove(deviceLogic.DeviceNum);
                    }
                }
            }
        }

        /// <summary>
        /// Creates and starts communication lines.
        /// </summary>
        private void StartLines()
        {
            try
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Запуск линий связи" :
                    "Start communication lines");

                foreach (LineConfig lineConfig in AppConfig.Lines)
                {
                    if (lineConfig.Active && 
                        CreateLine(lineConfig, out CommLine commLine) && 
                        !commLine.Start())
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Не удалось запустить линию связи {0}" :
                            "Failed to start communication line {0}", commLine.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске линий связи" :
                    "Error starting communication lines");
            }
        }

        /// <summary>
        /// Stops communication lines.
        /// </summary>
        private void StopLines()
        {
            try
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Остановка линий связи" :
                    "Stop communication lines");
                CommLine[] commLineArr;

                lock (commLineLock)
                {
                    commLineArr = commLines.ToArray(); // copy line list
                }

                foreach (CommLine commLine in commLineArr)
                {
                    commLine.Terminate();
                }

                // waiting for all lines to terminate
                Stopwatch stopwatch = Stopwatch.StartNew();
                bool linesTerminated;

                do
                {
                    linesTerminated = true;

                    foreach (CommLine commLine in commLineArr)
                    {
                        if (!commLine.IsTerminated)
                        {
                            linesTerminated = false;
                            Thread.Sleep(ScadaUtils.ThreadDelay);
                            break;
                        }
                    }
                } while (!linesTerminated && stopwatch.ElapsedMilliseconds <= ScadaUtils.ThreadWait);

                if (linesTerminated)
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Все линии связи остановлены" :
                        "All communication lines are stopped");
                }
                else
                {
                    Log.WriteWarning(Locale.IsRussian ?
                        "Некоторые линии связи всё ещё работают" :
                        "Some communication lines are still working");
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при остановке линий связи" :
                    "Error stopping communication lines");
            }
        }

        /// <summary>
        /// Starts the specified communication line.
        /// </summary>
        private void StartLine(int commLineNum)
        {
            try
            {
                lock (commLineLock)
                {
                    if (commLineMap.ContainsKey(commLineNum))
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Линия связи {0} уже создана" :
                            "Communication line {0} already created", commLineNum);
                        return;
                    }
                }

                if (CommConfig.LoadLineConfig(Storage, CommConfig.DefaultFileName, commLineNum, 
                    out LineConfig lineConfig, out string errMsg))
                {
                    InitDrivers(lineConfig.GetDriverCodes(), out List<DriverWrapper> addedDrivers);
                    addedDrivers.ForEach(d => d.OnServiceStart());

                    if (CreateLine(lineConfig, out CommLine commLine))
                    {
                        Log.WriteAction(Locale.IsRussian ?
                            "Запуск линии связи {0}" :
                            "Start communication line {0}", commLine.Title);

                        if (dataSourceHolder.ReadConfigDatabase(out ConfigDatabase configDatabase))
                            ConfigDatabase = configDatabase;

                        if (!commLine.Start())
                        {
                            Log.WriteError(Locale.IsRussian ?
                                "Не удалось запустить линию связи {0}" :
                                "Failed to start communication line {0}", commLine.Title);
                        }
                    }
                }
                else
                {
                    Log.WriteError(errMsg);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске линии связи {0}" :
                    "Error starting communication line {0}", commLineNum);
            }
        }

        /// <summary>
        /// Stops the specified communication line.
        /// </summary>
        private void StopLine(int commLineNum)
        {
            try
            {
                CommLine commLine;

                lock (commLineLock)
                {
                    if (!commLineMap.TryGetValue(commLineNum, out commLine))
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Линия связи {0} не найдена" :
                            "Communication line {0} not found", commLineNum);
                        return;
                    }
                }

                if (commLine.LineStatus == ServiceStatus.Normal ||
                    commLine.LineStatus == ServiceStatus.Error)
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Остановка линии связи {0}" :
                        "Stop communication line {0}", commLine.Title);

                    commLine.Terminate();
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    while (!commLine.IsTerminated && stopwatch.ElapsedMilliseconds <= ScadaUtils.ThreadWait)
                    {
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }

                    if (commLine.IsTerminated)
                    {
                        Log.WriteAction(Locale.IsRussian ?
                            "Линия связи {0} остановлена" :
                            "Communication line {0} is stopped", commLine.Title);
                    }
                    else
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Не удалось остановить линию связи {0}" :
                            "Failed to stop communication line {0}", commLine.Title);
                    }
                }
                else
                {
                    Log.WriteError(Locale.IsRussian ?
                        "Невозможно остановить линию связи {0}, потому что её состояние {1}" :
                        "Unable to stop communication line {0} because its state is {1}",
                        commLine.Title, commLine.LineStatus.ToString(Locale.IsRussian));
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при остановке линии связи {0}" :
                    "Error stopping communication line {0}", commLineNum);
            }
        }

        /// <summary>
        /// Restarts the specified communication line.
        /// </summary>
        private void RestartLine(int commLineNum)
        {
            try
            {
                bool lineExists;
                CommLine commLine;

                lock (commLineLock)
                {
                    lineExists = commLineMap.TryGetValue(commLineNum, out commLine);
                }

                if (!lineExists ||
                    commLine.LineStatus == ServiceStatus.Normal ||
                    commLine.LineStatus == ServiceStatus.Error)
                {
                    if (lineExists)
                        StopLine(commLineNum);

                    if (!lineExists || commLine.IsTerminated)
                        StartLine(commLineNum);
                }
                else
                {
                    Log.WriteError(Locale.IsRussian ?
                        "Невозможно перезапустить линию связи {0}, потому что её состояние {1}" :
                        "Unable to restart communication line {0} because its state is {1}",
                        commLine.Title, commLine.LineStatus.ToString(Locale.IsRussian));
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при перезапуске линии связи {0}" :
                    "Error restarting communication line {0}", commLineNum);
            }
        }

        /// <summary>
        /// Gets the communication line of the specified device.
        /// </summary>
        private bool GetDeviceLine(int deviceNum, out CommLine commLine)
        {
            lock (commLineLock)
            {
                if (deviceMap.TryGetValue(deviceNum, out DeviceItem deviceItem))
                {
                    commLine = deviceItem.Line;
                    return true;
                }
                else
                {
                    commLine = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Writes application information to the file.
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // prepare information
                StringBuilder sb = new StringBuilder((int)(lastInfoLength * 1.1));
                TimeSpan workSpan = DateTime.UtcNow - utcStartDT;
                string workSpanStr = workSpan.Days > 0 ?
                    workSpan.ToString(@"d\.hh\:mm\:ss") :
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Locale.IsRussian)
                {
                    sb
                        .AppendLine("Коммуникатор")
                        .AppendLine("------------")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workSpanStr)
                        .Append("Статус       : ").AppendLine(serviceStatus.ToString(true))
                        .Append("Версия       : ").AppendLine(EngineUtils.AppVersion);
                }
                else
                {
                    sb
                        .AppendLine("Communicator")
                        .AppendLine("------------")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("Status         : ").AppendLine(serviceStatus.ToString(false))
                        .Append("Version        : ").AppendLine(EngineUtils.AppVersion);
                }

                dataSourceHolder?.AppendInfo(sb);
                SharedData?.AppendInfo(sb);

                if (commLines != null)
                {
                    lock (commLineLock)
                    {
                        string header = Locale.IsRussian ?
                            $"Линии связи ({commLines.Count})" :
                            $"Communication Lines ({commLines.Count})";

                        sb
                            .AppendLine()
                            .AppendLine(header)
                            .Append('-', header.Length).AppendLine();

                        if (commLines.Count > 0)
                        {
                            if (maxLineTitleLength < 0)
                            {
                                maxLineTitleLength = 0;
                                commLines.ForEach(l => maxLineTitleLength = Math.Max(maxLineTitleLength, l.Title.Length));
                            }

                            foreach (CommLine commLine in commLines)
                            {
                                sb
                                    .Append(commLine.Title)
                                    .Append(' ', maxLineTitleLength - commLine.Title.Length)
                                    .Append(" : ")
                                    .AppendLine(commLine.LineStatus.ToString(Locale.IsRussian));
                            }
                        }
                        else
                        {
                            sb.AppendLine(Locale.IsRussian ? "Линий нет" : "No lines");
                        }
                    }
                }

                lastInfoLength = sb.Length;

                // write to file
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                {
                    writer.Write(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, CommonPhrases.WriteInfoError);
            }
        }


        /// <summary>
        /// Starts processing logic.
        /// </summary>
        public bool StartProcessing()
        {
            try
            {
                if (thread == null)
                {
                    Log.WriteAction(CommonPhrases.StartLogic);
                    PrepareProcessing();
                    thread = new Thread(Execute);
                    thread.Start();
                }
                else
                {
                    Log.WriteAction(CommonPhrases.LogicAlreadyStarted);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, CommonPhrases.StartLogicError);
                return false;
            }
            finally
            {
                if (thread == null)
                {
                    serviceStatus = ServiceStatus.Error;
                    WriteInfo();
                }
            }
        }

        /// <summary>
        /// Stops processing logic.
        /// </summary>
        public void StopProcessing()
        {
            try
            {
                if (thread != null)
                {
                    terminated = true;
                    serviceStatus = ServiceStatus.Terminating;

                    if (thread.Join(TimeSpan.FromSeconds(AppConfig.GeneralOptions.StopWait)))
                        Log.WriteAction(CommonPhrases.LogicStopped);
                    else
                        Log.WriteAction(CommonPhrases.UnableToStopLogic);

                    thread = null;
                }
            }
            catch (Exception ex)
            {
                serviceStatus = ServiceStatus.Error;
                WriteInfo();
                Log.WriteError(ex, CommonPhrases.StopLogicError);
            }
        }

        /// <summary>
        /// Adds the slice of the current data to the queue for transfer to the server.
        /// </summary>
        public void EnqueueCurrentData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            dataSourceHolder.WriteCurrentData(deviceSlice);
        }

        /// <summary>
        /// Adds the slice of historical data to the queue for transfer to the server.
        /// </summary>
        public void EnqueueHistoricalData(DeviceSlice deviceSlice)
        {
            if (deviceSlice == null)
                throw new ArgumentNullException(nameof(deviceSlice));

            dataSourceHolder.WriteHistoricalData(deviceSlice);
        }

        /// <summary>
        /// Adds the event to the queue for transfer to the server.
        /// </summary>
        public void EnqueueEvent(DeviceEvent deviceEvent)
        {
            if (deviceEvent == null)
                throw new ArgumentNullException(nameof(deviceEvent));

            dataSourceHolder.WriteEvent(deviceEvent);
        }

        /// <summary>
        /// Adds the telecontrol command to the queue for further execution.
        /// </summary>
        public void EnqueueCommand(TeleCommand cmd, string source)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            try
            {
                if (!AppConfig.GeneralOptions.EnableCommands)
                {
                    Log.WriteError(Locale.IsRussian ?
                        "Невозможно обработать команду, потому что команды отключены" :
                        "Unable to process command because commands are disabled");
                }
                else if (DateTime.UtcNow - cmd.CreationTime > ScadaUtils.CommandLifetime)
                {
                    Log.WriteError(Locale.IsRussian ?
                        "Устаревшая команда с ид. {0} от {1} отклонена" :
                        "Outdated command with ID {0} from {1} is rejected",
                        cmd.CommandID, source);
                }
                else if (cmd.IsAddressedToApp)
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Команда приложению {0} с ид. {1} от {2}" :
                        "Application command {0} with ID {1} from {2}",
                        cmd.CmdCode, cmd.CommandID, source);

                    if (CommCmdCode.AddressedToComm(cmd.CmdCode))
                    {
                        lock (appCmdQueue)
                        {
                            appCmdQueue.Enqueue(cmd);
                        }
                    }
                    else
                    {
                        Log.WriteAction(Locale.IsRussian ?
                            "Команда игнорирована" :
                            "Command ignored");
                    }
                }
                else
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Команда с ид. {0} на устройство {1} от {2}" :
                        "Command with ID {0} to the device {1} from {2}",
                        cmd.CommandID, cmd.DeviceNum, source);

                    if (GetDeviceLine(cmd.DeviceNum, out CommLine commLine))
                        commLine.EnqueueCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при добавлении в очередь команды ТУ" :
                    "Error enqueuing telecontrol command");
            }
        }

        /// <summary>
        /// Checks if a device with the specified number exists.
        /// </summary>
        public bool DeviceExists(int deviceNum)
        {
            return deviceMap.ContainsKey(deviceNum);
        }

        /// <summary>
        /// Gets all communication lines.
        /// </summary>
        ILineContext[] ICommContext.GetCommLines()
        {
            lock (commLineLock)
            {
                return commLines.ToArray();
            }
        }

        /// <summary>
        /// Gets the communication line by line number.
        /// </summary>
        bool ICommContext.GetCommLine(int commLineNum, out ILineContext lineContext)
        {
            lock (commLineLock)
            {
                if (commLineMap.TryGetValue(commLineNum, out CommLine commLine))
                {
                    lineContext = commLine;
                    return true;
                }
                else
                {
                    lineContext = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the device by device number.
        /// </summary>
        bool ICommContext.GetDevice(int deviceNum, out DeviceLogic deviceLogic)
        {
            lock (commLineLock)
            {
                if (deviceMap.TryGetValue(deviceNum, out DeviceItem deviceItem))
                {
                    deviceLogic = deviceItem.Device;
                    return true;
                }
                else
                {
                    deviceLogic = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Sends the telecontrol command to the current application.
        /// </summary>
        void ICommContext.SendCommand(TeleCommand cmd, string source)
        {
            EnqueueCommand(cmd, source);
        }
    }
}
