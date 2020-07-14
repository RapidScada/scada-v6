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
 * Module   : ScadaServerEngine
 * Summary  : Implements of the core server logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Adapters;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements of the core server logic.
    /// <para>Реализует основную логику сервера.</para>
    /// </summary>
    internal class CoreLogic
    {
        /// <summary>
        /// The application work states.
        /// </summary>
        private enum WorkState
        {
            Undefined = 0,
            Normal = 1,
            Error = 2,
            Terminated = 3
        }

        /// <summary>
        /// Represents metadata about an input channel.
        /// </summary>
        private class InCnlTag
        {
            public int Index { get; set; }
            public InCnl InCnl { get; set; }
        }


        /// <summary>
        /// The waiting time to stop the thread, ms.
        /// </summary>
        private const int WaitForStop = 10000;
        /// <summary>
        /// The period of writing application info.
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);
        /// <summary>
        /// The work state names in English.
        /// </summary>
        private static readonly string[] WorkStateNamesEn = { "Undefined", "Normal", "Error", "Terminated" };
        /// <summary>
        /// The work state names in Russian.
        /// </summary>
        private static readonly string[] WorkStateNamesRu = { "не определено", "норма", "ошибка", "завершено" };

        private readonly ServerConfig config; // the server configuration
        private readonly ServerDirs appDirs;  // the application directories
        private readonly ILog log;            // the application log
        private readonly string infoFileName; // the full file name to write application information

        private Thread thread;                // the working thread of the logic
        private volatile bool terminated;     // necessary to stop the thread
        private DateTime utcStartDT;          // the UTC start time
        private DateTime startDT;             // the local start time
        private WorkState workState;          // the work state

        private ServerListener listener;           // the TCP listener
        private ModuleHolder moduleHolder;         // holds modules
        private ArchiveHolder archiveHolder;       // holds archives
        private BaseDataSet baseDataSet;           // the configuration database
        private Dictionary<int, InCnlTag> cnlTags; // the metadata about the input channels accessed by channel numbers
        private CurrentData curData;               // the current data of the input channels


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(ServerConfig config, ServerDirs appDirs, ILog log)
        {
            this.config = config ?? throw new ArgumentNullException("config");
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.log = log ?? throw new ArgumentNullException("log");
            infoFileName = appDirs.LogDir + ServerUtils.InfoFileName;

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            workState = WorkState.Undefined;

            listener = null;
            moduleHolder = null;
            archiveHolder = null;
            baseDataSet = null;
            cnlTags = null;
            curData = null;
        }


        /// <summary>
        /// Gets a value indicating whether the server is ready.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return thread != null;
            }
        }


        /// <summary>
        /// Prepares the logic processing.
        /// </summary>
        private bool PrepareProcessing(out string errMsg)
        {
            errMsg = "";
            terminated = false;
            utcStartDT = DateTime.UtcNow;
            startDT = utcStartDT.ToLocalTime();
            workState = WorkState.Normal;
            WriteInfo();

            if (!config.PathOptions.CheckExistence(out errMsg))
                return false;

            InitModules();
            InitArchives();

            if (!ReadBase())
                return false;

            InitCnlTags();
            curData = new CurrentData(cnlTags.Count);

            listener = new ServerListener(this, config.ListenerOptions, log);
            return true;
        }

        /// <summary>
        /// Initializes modules.
        /// </summary>
        private void InitModules()
        {
            moduleHolder = new ModuleHolder(log);
        }

        /// <summary>
        /// Initializes archives.
        /// </summary>
        private void InitArchives()
        {
            archiveHolder = new ArchiveHolder(log);
        }

        /// <summary>
        /// Reads the configuration database from files.
        /// </summary>
        private bool ReadBase()
        {
            string tableName = Locale.IsRussian ? "неопределено" : "undefined";

            try
            {
                baseDataSet = new BaseDataSet();
                BaseTableAdapter adapter = new BaseTableAdapter();

                foreach (IBaseTable baseTable in baseDataSet.AllTables)
                {
                    tableName = baseTable.Name;
                    adapter.FileName = Path.Combine(config.PathOptions.BaseDir, tableName.ToLowerInvariant() + ".dat");
                    adapter.Fill(baseTable);
                }

                log.WriteAction(Locale.IsRussian ?
                    "База конфигурации считана успешно" :
                    "The configuration database has been read successfully");

                return true;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Error reading the configuration database. Table name is {0}" :
                    "Ошибка при чтении базы конфигурации. Имя таблицы {0}", tableName);
                return false;
            }
        }

        /// <summary>
        /// Initializes metadata about the input channels.
        /// </summary>
        private void InitCnlTags()
        {
            cnlTags = new Dictionary<int, InCnlTag>();
            int index = 0;

            foreach (InCnl inCnl in baseDataSet.InCnlTable.EnumerateItems())
            {
                if (inCnl.Active)
                {
                    cnlTags.Add(inCnl.CnlNum, new InCnlTag
                    {
                        Index = index++,
                        InCnl = inCnl
                    });
                }
            }

            log.WriteInfo(string.Format(Locale.IsRussian ?
                "Количество активных входных каналов: {0}" :
                "Number of active input channels: {0}", cnlTags.Count));
        }

        /// <summary>
        /// Work cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                DateTime writeInfoDT = DateTime.MinValue; // the timestamp of writing application info
                moduleHolder.CallOnServiceStart();

                while (!terminated)
                {
                    try
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        // write application info
                        if (utcNow - writeInfoDT >= WriteInfoPeriod)
                        {
                            writeInfoDT = utcNow;
                            WriteInfo();
                        }

                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception ex)
                    {
                        log.WriteException(ex, Locale.IsRussian ?
                            "Ошибка в цикле работы приложения" :
                            "Error in the application work cycle");
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                }
            }
            finally
            {
                moduleHolder.CallOnServiceStop();
                workState = WorkState.Terminated;
                WriteInfo();
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
                StringBuilder sbInfo = new StringBuilder();
                TimeSpan workSpan = DateTime.UtcNow - utcStartDT;
                string workSpanStr = workSpan.Days > 0 ?
                    workSpan.ToString(@"d\.hh\:mm\:ss") :
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Locale.IsRussian)
                {
                    sbInfo
                        .AppendLine("Сервер")
                        .AppendLine("------")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workSpanStr)
                        .Append("Состояние    : ").AppendLine(WorkStateNamesRu[(int)workState])
                        .Append("Версия       : ").AppendLine(ServerUtils.AppVersion);
                }
                else
                {
                    sbInfo
                        .AppendLine("Server")
                        .AppendLine("------")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("State          : ").AppendLine(WorkStateNamesEn[(int)workState])
                        .Append("Version        : ").AppendLine(ServerUtils.AppVersion);
                }

                // write to file
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                {
                    writer.Write(sbInfo.ToString());
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи в файл информации о работе приложения" :
                    "Error writing application information to the file");
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
                    log.WriteAction(Locale.IsRussian ?
                        "Запуск обработки логики" :
                        "Start logic processing");
                    
                    if (PrepareProcessing(out string errMsg) && 
                        listener.Start())
                    {
                        thread = new Thread(Execute);
                        thread.Start();
                    }
                    else if (!string.IsNullOrEmpty(errMsg))
                    {
                        log.WriteError(errMsg);
                    }
                }
                else
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Обработка логики уже запущена" :
                        "Logic processing is already started");
                }

                return thread != null;
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при запуске обработки логики" :
                    "Error starting logic processing");
                return false;
            }
            finally
            {
                if (thread == null)
                {
                    workState = WorkState.Error;
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
                // stop listener
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }

                // stop logic processing
                if (thread != null)
                {
                    terminated = true;

                    if (thread.Join(WaitForStop))
                    {
                        log.WriteAction(Locale.IsRussian ?
                            "Обработка логики остановлена" :
                            "Logic processing is stopped");
                    }
                    else if (ScadaUtils.IsRunningOnCore)
                    {
                        log.WriteAction(Locale.IsRussian ?
                            "Не удалось остановить обработку логики за установленное время" :
                            "Unable to stop logic processing for a specified time");
                    }
                    else
                    {
                        thread.Abort(); // not supported on .NET Core
                        log.WriteAction(Locale.IsRussian ?
                            "Обработка логики прервана" :
                            "Logic processing is aborted");
                    }

                    thread = null;
                }
            }
            catch (Exception ex)
            {
                workState = WorkState.Error;
                WriteInfo();
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при остановке обработки логики" :
                    "Error stopping logic processing");
            }
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(int cnlSetID, out bool cnlSetFound)
        {
            cnlSetFound = false;
            return new CnlData[0];
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out int cnlSetID)
        {
            cnlSetID = 0;
            return new CnlData[0];
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public void WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");
            if (cnlData == null)
                throw new ArgumentNullException("cnlData");

            try
            {
                lock (curData)
                {
                    DateTime utcNow = DateTime.UtcNow;

                    for (int i = 0, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
                    {
                        if (cnlTags.TryGetValue(cnlNums[i], out InCnlTag cnlTag))
                        {
                            curData.SetData(utcNow, cnlTag.Index, cnlData[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи текущих данных" :
                    "Error writing the current data");
            }
        }
    }
}
