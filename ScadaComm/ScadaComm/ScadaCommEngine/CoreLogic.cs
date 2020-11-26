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
 * Module   : ScadaCommEngine
 * Summary  : Implements of the core Communicator logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Client;
using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
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
    /// Implements of the core Communicator logic.
    /// <para>Реализует основную логику Коммуникатора.</para>
    /// </summary>
    internal class CoreLogic
    {
        /// <summary>
        /// Specifies the execution steps.
        /// </summary>
        private enum ExecutionStep { ReceiveBase, StartLines, MainWork }

        /// <summary>
        /// The period of attempts to receive the configuration database.
        /// </summary>
        private static readonly TimeSpan ReceiveBasePeriod = TimeSpan.FromSeconds(10);
        /// <summary>
        /// The period of writing application info.
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);

        private readonly string infoFileName; // the full file name to write application information

        private Thread thread;                // the working thread of the logic
        private volatile bool terminated;     // necessary to stop the thread
        private DateTime utcStartDT;          // the UTC start time
        private DateTime startDT;             // the local start time
        private ServiceStatus serviceStatus;  // the current service status
        private int lastInfoLength;           // the last info text length
        private int maxLineTitleLength;       // the maximum length of communication line title

        private ScadaClient scadaClient;      // communicates with the Server service
        private List<CommLine> commLines;     // the active communication lines
        private DriverHolder driverHolder;    // holds drivers


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(CommConfig config, CommDirs appDirs, ILog log)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            AppDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            BaseDataSet = null;
            SharedData = null;

            infoFileName = Path.Combine(appDirs.LogDir, CommUtils.InfoFileName);

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            serviceStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;
            maxLineTitleLength = 0;

            scadaClient = null;
            commLines = null;
            driverHolder = null;
        }


        /// <summary>
        /// Gets the Communicator configuration.
        /// </summary>
        public CommConfig Config { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public CommDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        public BaseDataSet BaseDataSet { get; private set; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public ConcurrentDictionary<string, object> SharedData { get; private set; }


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

            BaseDataSet = null;
            SharedData = new ConcurrentDictionary<string, object>();
            scadaClient = Config.GeneralOptions.InteractWithServer ? 
                new ScadaClient(Config.ConnectionOptions) { CommLog = CreateClientLog() } : null;
            commLines = new List<CommLine>(Config.Lines.Count);
            InitDrivers();
        }

        /// <summary>
        /// Creates a client communication log file.
        /// </summary>
        private ILog CreateClientLog()
        {
            return Config.GeneralOptions.ClientLogEnabled ?
                new LogFile(LogFormat.Simple)
                {
                    FileName = Path.Combine(AppDirs.LogDir, CommUtils.ClientLogFileName),
                    TimestampFormat = LogFile.DefaultTimestampFormat + "'.'ff"
                } : 
                null;
        }

        /// <summary>
        /// Initializes drivers.
        /// </summary>
        private void InitDrivers()
        {
            driverHolder = new DriverHolder(Log);
            CommContext commContext = new CommContext(this);

            foreach (string driverCode in Config.DriverCodes)
            {
                if (DriverFactory.GetDriverLogic(AppDirs.DrvDir, driverCode, commContext,
                    out DriverLogic driverLogic, out string message))
                {
                    Log.WriteAction(message);
                    driverHolder.AddDriver(driverLogic);
                }
                else
                {
                    Log.WriteError(message);
                }
            }
        }

        /// <summary>
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                ExecutionStep executionStep = Config.GeneralOptions.InteractWithServer ? 
                    ExecutionStep.ReceiveBase : ExecutionStep.StartLines;
                DateTime receiveBaseDT = DateTime.MinValue;
                DateTime writeInfoDT = DateTime.MinValue;

                driverHolder.OnServiceStart();
                serviceStatus = ServiceStatus.Normal;

                while (!terminated)
                {
                    try
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        switch (executionStep)
                        {
                            case ExecutionStep.MainWork:
                                break;

                            case ExecutionStep.ReceiveBase:
                                if (utcNow - receiveBaseDT >= ReceiveBasePeriod)
                                {
                                    receiveBaseDT = utcNow;

                                    if (ReceiveBase())
                                    {
                                        executionStep = ExecutionStep.StartLines;
                                        serviceStatus = ServiceStatus.Normal;
                                    }
                                    else
                                    {
                                        serviceStatus = ServiceStatus.Error;
                                    }
                                }
                                break;

                            case ExecutionStep.StartLines:
                                CreateLines();
                                StartLines();
                                executionStep = ExecutionStep.MainWork;
                                break;
                        }

                        // write application info
                        if (utcNow - writeInfoDT >= WriteInfoPeriod)
                        {
                            writeInfoDT = utcNow;
                            WriteInfo();
                        }

                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, CommonPhrases.LogicCycleError);
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                }
            }
            finally
            {
                StopLines();
                driverHolder.OnServiceStop();
                serviceStatus = ServiceStatus.Terminated;
                WriteInfo();
            }
        }

        /// <summary>
        /// Receives the configuration database.
        /// </summary>
        private bool ReceiveBase()
        {
            string tableName = Locale.IsRussian ? "неопределена" : "undefined";

            try
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Приём базы конфигурации" :
                    "Receive the configuration database");

                BaseDataSet = new BaseDataSet();

                foreach (IBaseTable baseTable in BaseDataSet.AllTables)
                {
                    tableName = baseTable.Name;
                    scadaClient.DownloadBaseTable(baseTable);
                }

                Log.WriteAction(Locale.IsRussian ?
                    "База конфигурации получена успешно" :
                    "The configuration database has been received successfully");
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при приёме базы конфигурации, таблица {0}" :
                    "Error receiving the configuration database, the {0} table", tableName);
                return false;
            }
        }

        /// <summary>
        /// Creates communication lines.
        /// </summary>
        private void CreateLines()
        {
            maxLineTitleLength = 0;

            foreach (LineConfig lineConfig in Config.Lines)
            {
                try
                {
                    if (lineConfig.Active)
                    {
                        CommLine commLine = CommLine.Create(lineConfig, this, driverHolder);
                        commLines.Add(commLine);

                        if (maxLineTitleLength < commLine.Title.Length)
                            maxLineTitleLength = commLine.Title.Length;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка при создании линии связи {0}" :
                        "Error creating communication line {0}", lineConfig.Title);
                }
            }
        }

        /// <summary>
        /// Starts communication lines.
        /// </summary>
        private void StartLines()
        {
            try
            {
                Log.WriteAction(Locale.IsRussian ?
                    "Запуск линий связи" :
                    "Start communication lines");

                foreach (CommLine commLine in commLines)
                {
                    if (!commLine.Start())
                    {
                        Log.WriteError(Locale.IsRussian ?
                            "Не удалось запустить линию связи {0}" :
                            "Failed to start communication line {0}", commLine.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
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

                foreach (CommLine commLine in commLines)
                {
                    commLine.Terminate();
                }

                // waiting for all lines to terminate
                Stopwatch stopwatch = Stopwatch.StartNew();
                bool linesTerminated;

                do
                {
                    linesTerminated = true;

                    foreach (CommLine commLine in commLines)
                    {
                        if (!commLine.IsTerminated)
                        {
                            linesTerminated = false;
                            Thread.Sleep(ScadaUtils.ThreadDelay);
                            break;
                        }
                    }
                } while (!linesTerminated && stopwatch.ElapsedMilliseconds <= ScadaUtils.ThreadWait);

                if (!linesTerminated)
                {
                    Log.WriteWarning(Locale.IsRussian ?
                        "Некоторые линии связи всё ещё работают" :
                        "Some communication lines are still working");
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при остановке линий связи" :
                    "Error stopping communication lines");
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
                        .Append("Версия       : ").AppendLine(CommUtils.AppVersion);
                }
                else
                {
                    sb
                        .AppendLine("Communicator")
                        .AppendLine("------------")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("Status         : ").AppendLine(serviceStatus.ToString(false))
                        .Append("Version        : ").AppendLine(CommUtils.AppVersion);
                }

                if (commLines != null)
                {
                    lock (commLines)
                    {
                        string header = Locale.IsRussian ?
                            "Линии связи (" + commLines.Count + ")" :
                            "Communication Lines (" + commLines.Count + ")";

                        sb
                            .AppendLine()
                            .AppendLine(header)
                            .Append('-', header.Length).AppendLine();

                        if (commLines.Count > 0)
                        {
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
                Log.WriteException(ex, CommonPhrases.WriteInfoError);
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
                    Log.WriteAction(CommonPhrases.LogicIsAlreadyStarted);
                }

                return thread != null;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, CommonPhrases.StartLogicError);
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

                    if (thread.Join(ScadaUtils.ThreadWait))
                        Log.WriteAction(CommonPhrases.LogicIsStopped);
                    else
                        Log.WriteAction(CommonPhrases.UnableToStopLogic);

                    thread = null;
                }
            }
            catch (Exception ex)
            {
                serviceStatus = ServiceStatus.Error;
                WriteInfo();
                Log.WriteException(ex, CommonPhrases.StopLogicError);
            }
        }
    }
}
