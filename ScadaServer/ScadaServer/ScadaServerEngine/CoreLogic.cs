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
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Log;
using Scada.Server.Archives;
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
    internal class CoreLogic : ICnlDataChangeHandler
    {
        /// <summary>
        /// The service statuses.
        /// </summary>
        private enum ServiceStatus
        {
            Undefined = 0,
            Normal = 1,
            Error = 2,
            Terminated = 3
        }


        /// <summary>
        /// The waiting time to stop the thread, ms.
        /// </summary>
        private const int WaitForStop = 10000;
        /// <summary>
        /// The maximum number of input channels to process per iteration when checking activity.
        /// </summary>
        private const int MaxCnlCountToCheckActivity = 1000;
        /// <summary>
        /// The period of writing application info.
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);
        /// <summary>
        /// The service status names in English.
        /// </summary>
        private static readonly string[] StatusNamesEn = { "Undefined", "Normal", "Error", "Terminated" };
        /// <summary>
        /// The service status names in Russian.
        /// </summary>
        private static readonly string[] StatusNamesRu = { "не определено", "норма", "ошибка", "завершено" };

        private readonly ServerConfig config; // the server configuration
        private readonly ServerDirs appDirs;  // the application directories
        private readonly ILog log;            // the application log
        private readonly string infoFileName; // the full file name to write application information

        private Thread thread;                // the working thread of the logic
        private volatile bool terminated;     // necessary to stop the thread
        private DateTime utcStartDT;          // the UTC start time
        private DateTime startDT;             // the local start time
        private ServiceStatus serviceStatus;  // the current service status

        private ServerListener listener;      // the TCP listener
        private ModuleHolder moduleHolder;    // holds modules
        private ArchiveHolder archiveHolder;  // holds archives
        private BaseDataSet baseDataSet;      // the configuration database
        private Dictionary<int, CnlTag> cnlTags;       // the metadata about the input channels accessed by channel number
        private List<CnlTag> measCnlTags;              // the list of the input channel tags of the measured type
        private List<CnlTag> calcCnlTags;              // the list of the input channel tags of the calculated type
        private Dictionary<int, OutCnlTag> outCnlTags; // the metadata about the output channels accessed by channel number
        private Dictionary<string, User> users;        // the users accessed by name
        private ObjSecurity objSecurity;      // provides access control
        private Calculator calc;              // provides work with scripts and formulas
        private CurrentData curData;          // the current data of the input channels
        private Queue<Event> events;          // the just generated events
        private ServerCache serverCache;      // the server level cache


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
            serviceStatus = ServiceStatus.Undefined;

            listener = null;
            moduleHolder = null;
            archiveHolder = null;
            baseDataSet = null;
            cnlTags = null;
            measCnlTags = null;
            calcCnlTags = null;
            outCnlTags = null;
            users = null;
            objSecurity = null;
            calc = null;
            curData = null;
            events = null;
            serverCache = null;
        }


        /// <summary>
        /// Gets a value indicating whether the server is ready.
        /// </summary>
        public bool IsReady
        {
            get
            {
                return serviceStatus == ServiceStatus.Normal;
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
            serviceStatus = ServiceStatus.Undefined;
            WriteInfo();

            if (!config.PathOptions.CheckExistence(out errMsg))
                return false;

            InitModules();
            InitArchives();

            if (!ReadBase())
                return false;

            InitCnlTags();
            InitOutCnlTags();
            InitUsers();
            InitObjSecurity();

            if (!InitCalculator())
                return false;

            curData = new CurrentData(this, cnlTags);
            events = new Queue<Event>();
            serverCache = new ServerCache();
            listener = new ServerListener(this, archiveHolder, serverCache, config, log);
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
            string tableName = Locale.IsRussian ? "неопределена" : "undefined";

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
                    "Ошибка при чтении базы конфигурации, таблица {0}" :
                    "Error reading the configuration database, the {0} table", tableName);
                return false;
            }
        }

        /// <summary>
        /// Initializes metadata about the input channels.
        /// </summary>
        private void InitCnlTags()
        {
            // create channel tags
            cnlTags = new Dictionary<int, CnlTag>();
            measCnlTags = new List<CnlTag>();
            calcCnlTags = new List<CnlTag>();
            List<CnlTag> limTags = new List<CnlTag>();
            int index = 0;
            int cnlNum = 0;

            // add new channel tag to collections
            void AddCnlTag(InCnl inCnl, Lim lim)
            {
                CnlTag cnlTag = new CnlTag(index++, cnlNum, inCnl, lim);
                cnlTags.Add(cnlNum++, cnlTag);

                if (inCnl.CnlTypeID == CnlTypeID.Measured)
                    measCnlTags.Add(cnlTag);
                else if (inCnl.CnlTypeID == CnlTypeID.Calculated)
                    calcCnlTags.Add(cnlTag);

                if (lim != null && lim.IsBoundToCnl)
                    limTags.Add(cnlTag);
            }

            foreach (InCnl inCnl in baseDataSet.InCnlTable.EnumerateItems())
            {
                if (inCnl.Active && inCnl.CnlNum >= cnlNum)
                {
                    cnlNum = inCnl.CnlNum;
                    Lim lim = inCnl.LimID.HasValue ? baseDataSet.LimTable.GetItem(inCnl.LimID.Value) : null;
                    AddCnlTag(inCnl, lim);

                    // add channel tags if one channel row defines multiple channels
                    if (inCnl.DataLen > 1)
                    {
                        for (int i = 1, cnt = inCnl.DataLen.Value; i < cnt; i++)
                        {
                            AddCnlTag(inCnl, lim);
                        }
                    }
                }
            }

            // find channel indices for limits
            int FindIndex(double? n)
            {
                return n.HasValue && !double.IsNaN(n.Value) && cnlTags.TryGetValue((int)n, out CnlTag t) ?
                    t.Index : -1;
            }

            foreach (CnlTag cnlTag in limTags)
            {
                cnlTag.LimCnlIndex = new LimCnlIndex
                {
                    LoLo = FindIndex(cnlTag.Lim.LoLo),
                    Low = FindIndex(cnlTag.Lim.Low),
                    High = FindIndex(cnlTag.Lim.High),
                    HiHi = FindIndex(cnlTag.Lim.HiHi),
                };
            }

            log.WriteInfo(string.Format(Locale.IsRussian ?
                "Количество активных входных каналов: {0}" :
                "Number of active input channels: {0}", cnlTags.Count));
        }

        /// <summary>
        /// Initializes metadata about the output channels.
        /// </summary>
        private void InitOutCnlTags()
        {
            outCnlTags = new Dictionary<int, OutCnlTag>();

            foreach (OutCnl outCnl in baseDataSet.OutCnlTable.EnumerateItems())
            {
                if (outCnl.Active)
                    outCnlTags.Add(outCnl.OutCnlNum, new OutCnlTag(outCnl));
            }

            log.WriteInfo(string.Format(Locale.IsRussian ?
                "Количество активных каналов управления: {0}" :
                "Number of active output channels: {0}", outCnlTags.Count));
        }

        /// <summary>
        /// Initializes the user dictionary.
        /// </summary>
        private void InitUsers()
        {
            users = new Dictionary<string, User>(baseDataSet.UserTable.ItemCount);

            foreach (User user in baseDataSet.UserTable.EnumerateItems())
            {
                users[user.Name.ToLowerInvariant()] = user;
            }
        }

        /// <summary>
        /// Initializes the object security instance.
        /// </summary>
        private void InitObjSecurity()
        {
            objSecurity = new ObjSecurity();
            objSecurity.Init(baseDataSet);
        }

        /// <summary>
        /// Initializes the calculator.
        /// </summary>
        private bool InitCalculator()
        {
            calc = new Calculator(appDirs, log);
            return calc.CompileScripts(baseDataSet, cnlTags, outCnlTags);
        }

        /// <summary>
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                int checkActivityTagIndex = 0; // the index of the input channel tag which is checked for activity
                DateTime writeInfoDT = DateTime.MinValue; // the timestamp of writing application info

                moduleHolder.OnServiceStart();
                archiveHolder.ReadCurrentData(curData);
                calc.InitScripts();
                serviceStatus = ServiceStatus.Normal;

                while (!terminated)
                {
                    try
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        lock (curData)
                        {
                            // check activity of input channels
                            CheckActivity(ref checkActivityTagIndex, utcNow);

                            // calculate input channel data
                            CalcCnlData(utcNow);

                            // process data by archives
                            curData.PrepareIteration(utcNow);
                            archiveHolder.ProcessData(curData);
                        }

                        // process events without blocking current data
                        ProcessEvents();

                        // process modules and archives
                        moduleHolder.OnIteration();
                        archiveHolder.DeleteOutdatedData();

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
                calc.FinalizeScripts();
                moduleHolder.OnServiceStop();
                serviceStatus = ServiceStatus.Terminated;
                WriteInfo();
            }
        }

        /// <summary>
        /// Checks the activity of input channels and sets status of inactive input channels to unreliable.
        /// </summary>
        private void CheckActivity(ref int tagIndex, DateTime nowDT)
        {
            int unrelIfInactive = config.GeneralOptions.UnrelIfInactive;

            if (unrelIfInactive > 0)
            {
                for (int i = 0, cnt = measCnlTags.Count; i < MaxCnlCountToCheckActivity && tagIndex < cnt; i++)
                {
                    CnlTag cnlTag = measCnlTags[tagIndex++];

                    if (cnlTag.InCnl.CnlTypeID == CnlTypeID.Measured)
                    {
                        CnlData cnlData = curData.CnlData[cnlTag.Index];

                        if (cnlData.Stat > CnlStatusID.Undefined && cnlData.Stat != CnlStatusID.Unreliable &&
                            (nowDT - curData.Timestamps[cnlTag.Index]).TotalSeconds > unrelIfInactive)
                        {
                            curData.SetCurCnlData(cnlTag, new CnlData(cnlData.Val, CnlStatusID.Unreliable), nowDT);
                        }
                    }
                }

                if (tagIndex >= measCnlTags.Count)
                    tagIndex = 0;
            }
        }

        /// <summary>
        /// Calculates the input channels of the calculated type.
        /// </summary>
        private void CalcCnlData(DateTime nowDT)
        {
            try
            {
                calc.BeginCalculation(curData);

                foreach (CnlTag cnlTag in calcCnlTags)
                {
                    CnlData newCnlData = calc.CalcCnlData(cnlTag, curData.CnlData[cnlTag.Index]);
                    curData.SetCurCnlData(cnlTag, newCnlData, nowDT);
                }
            }
            finally
            {
                calc.EndCalculation();
            }
        }

        /// <summary>
        /// Processes the events from the queue.
        /// </summary>
        private void ProcessEvents()
        {
            lock (events)
            {
                while (events.Count > 0)
                {
                    Event ev = events.Dequeue();

                    log.WriteAction(string.Format(Locale.IsRussian ?
                        "Создано событие с ид. {0}, входным каналом {1} и выходным каналом {2}" :
                        "Event with ID {0}, input channel {1} and output channel {2} generated",
                        ev.EventID, ev.CnlNum, ev.OutCnlNum));

                    moduleHolder.OnEvent(ev);
                    archiveHolder.WriteEvent(ev, ArchiveMask.Default);
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
                        .Append("Статус       : ").AppendLine(StatusNamesRu[(int)serviceStatus])
                        .Append("Версия       : ").AppendLine(ServerUtils.AppVersion);
                }
                else
                {
                    sbInfo
                        .AppendLine("Server")
                        .AppendLine("------")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("Status         : ").AppendLine(StatusNamesEn[(int)serviceStatus])
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
        /// Updates the input channel status after formula calculation for the current data.
        /// </summary>
        private void UpdateCnlStatus(CnlTag cnlTag, ref CnlData cnlData, CnlData prevCnlData)
        {
            if (cnlData.Stat == CnlStatusID.Defined)
            {
                if (double.IsNaN(cnlData.Val))
                {
                    // set undefined status if value is not a number
                    cnlData.Stat = CnlStatusID.Undefined;
                }
                else if (cnlTag.Lim != null)
                {
                    // set status depending on channel limits
                    GetCnlLimits(cnlTag, out double lolo, out double low, out double high, out double hihi);
                    cnlData.Stat = GetCnlStatus(cnlData.Val, lolo, low, high, hihi);
                    int prevStat = prevCnlData.Stat;

                    if (cnlData.Stat == CnlStatusID.Normal && 
                        (prevStat == CnlStatusID.ExtremelyLow || prevStat == CnlStatusID.Low || 
                        prevStat == CnlStatusID.High || prevStat == CnlStatusID.ExtremelyHigh))
                    {
                        double deadband = cnlTag.Lim.Deadband ?? 0;
                        cnlData.Stat = GetCnlStatus(cnlData.Val,
                            lolo + deadband, low + deadband, 
                            high - deadband, hihi - deadband);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the input channel status after formula calculation for the archive at the timestamp.
        /// </summary>
        private void UpdateCnlStatus(HistoricalArchiveLogic archiveLogic, DateTime timestamp, 
            CnlTag cnlTag, ref CnlData cnlData)
        {
            if (cnlData.Stat == CnlStatusID.Defined)
            {
                if (double.IsNaN(cnlData.Val))
                {
                    cnlData.Stat = CnlStatusID.Undefined;
                }
                else if (cnlTag.Lim == null)
                {
                    if (cnlTag.InCnl.CnlTypeID == CnlTypeID.Measured)
                        cnlData.Stat = CnlStatusID.Archival;
                }
                else
                {
                    GetCnlLimits(archiveLogic, timestamp, cnlTag, 
                        out double lolo, out double low, out double high, out double hihi);
                    cnlData.Stat = GetCnlStatus(cnlData.Val, lolo, low, high, hihi);
                }
            }
        }

        /// <summary>
        /// Get the input channel limits for the current data.
        /// </summary>
        private void GetCnlLimits(CnlTag cnlTag, out double lolo, out double low, out double high, out double hihi)
        {
            double GetLimit(int cnlIndex)
            {
                CnlData limData = cnlIndex >= 0 ? curData.CnlData[cnlIndex] : CnlData.Empty;
                return limData.Stat > 0 ? limData.Val : double.NaN;
            }

            if (cnlTag.Lim.IsBoundToCnl)
            {
                LimCnlIndex limCnlIndex = cnlTag.LimCnlIndex;
                lolo = GetLimit(limCnlIndex.LoLo);
                low = GetLimit(limCnlIndex.Low);
                high = GetLimit(limCnlIndex.High);
                hihi = GetLimit(limCnlIndex.HiHi);
            }
            else
            {
                Lim lim = cnlTag.Lim;
                lolo = lim.LoLo ?? double.NaN;
                low = lim.Low ?? double.NaN;
                high = lim.High ?? double.NaN;
                hihi = lim.HiHi ?? double.NaN;
            }
        }

        /// <summary>
        /// Get the input channel limits for the archive at the timestamp.
        /// </summary>
        private void GetCnlLimits(HistoricalArchiveLogic archiveLogic, DateTime timestamp, CnlTag cnlTag, 
            out double lolo, out double low, out double high, out double hihi)
        {
            double GetLimit(double cnlNum)
            {
                CnlData limData = cnlNum > 0 ? archiveLogic.GetCnlData((int)cnlNum, timestamp) : CnlData.Empty;
                return limData.Stat > 0 ? limData.Val : double.NaN;
            }

            Lim lim = cnlTag.Lim;
            lolo = lim.LoLo ?? double.NaN;
            low = lim.Low ?? double.NaN;
            high = lim.High ?? double.NaN;
            hihi = lim.HiHi ?? double.NaN;

            if (cnlTag.Lim.IsBoundToCnl)
            {
                lolo = GetLimit(lolo);
                low = GetLimit(low);
                high = GetLimit(high);
                hihi = GetLimit(hihi);
            }
        }

        /// <summary>
        /// Get the input channel status depending on whether the channel value is within the limits.
        /// </summary>
        private int GetCnlStatus(double cnlVal, double lolo, double low, double high, double hihi)
        {
            if (cnlVal < lolo)
                return CnlStatusID.ExtremelyLow;
            else if (cnlVal < low)
                return CnlStatusID.Low;
            else if (cnlVal > hihi)
                return CnlStatusID.ExtremelyHigh;
            else if (cnlVal > high)
                return CnlStatusID.High;
            else
                return CnlStatusID.Normal;
        }

        /// <summary>
        /// Generates an event based on input channel data.
        /// </summary>
        private void GenerateEvent(CnlTag cnlTag, CnlData cnlData, CnlData prevCnlData)
        {
            InCnl inCnl = cnlTag.InCnl;
            EventMask eventMask = new EventMask(inCnl.EventMask);

            if (eventMask.Enabled)
            {
                int cnlStat = cnlData.Stat;
                int prevStat = prevCnlData.Stat;

                if (eventMask.CnlDataChange && cnlData != prevCnlData ||
                    eventMask.CnlStatusChange && cnlStat != prevStat ||
                    eventMask.CnlUndefined &&
                    (cnlStat == CnlStatusID.Undefined && prevStat > CnlStatusID.Undefined ||
                    cnlStat > CnlStatusID.Undefined && prevStat == CnlStatusID.Undefined))
                {
                    CnlStatus cnlStatus = baseDataSet.CnlStatusTable.GetItem(cnlData.Stat);

                    EnqueueEvent(new Event
                    {
                        EventID = ScadaUtils.GenerateUniqueID(),
                        Timestamp = DateTime.UtcNow,
                        CnlNum = cnlTag.CnlNum,
                        ObjNum = inCnl.ObjNum ?? 0,
                        DeviceNum = inCnl.DeviceNum ?? 0,
                        PrevCnlVal = prevCnlData.Val,
                        PrevCnlStat = prevStat,
                        CnlVal = cnlData.Val,
                        CnlStat = cnlStat,
                        Severity = cnlStatus?.Severity ?? 0,
                        AckRequired = cnlStatus?.AckRequired ?? false
                    });
                }
            }
        }

        /// <summary>
        /// Generates an event based on the command.
        /// </summary>
        private void GenerateEvent(OutCnlTag outCnlTag, TeleCommand command)
        {
            if (outCnlTag.OutCnl.EventEnabled)
            {
                EnqueueEvent(new Event
                {
                    EventID = ScadaUtils.GenerateUniqueID(),
                    Timestamp = DateTime.UtcNow,
                    OutCnlNum = command.OutCnlNum,
                    ObjNum = command.ObjNum,
                    DeviceNum = command.DeviceNum,
                    CnlVal = command.CmdVal,
                    Data = command.CmdData
                });
            }
        }

        /// <summary>
        /// Adds the event to the queue.
        /// </summary>
        private void EnqueueEvent(Event ev)
        {
            lock (events)
            {
                events.Enqueue(ev);
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
                serviceStatus = ServiceStatus.Error;
                WriteInfo();
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при остановке обработки логики" :
                    "Error stopping logic processing");
            }
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        public bool ValidateUser(string username, string password, out int userID, out int roleID, out string errMsg)
        {
            userID = 0;
            roleID = RoleID.Disabled;

            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    errMsg = Locale.IsRussian ?
                        "Имя пользователя или пароль не может быть пустым" :
                        "Username or password can not be empty";
                    return false;
                }

                // validate by modules
                bool userIsValid = moduleHolder.ValidateUser(username, password, 
                    out userID, out roleID, out errMsg, out bool handled);

                if (handled)
                    return userIsValid;

                // validate by the configuration database
                if (users.TryGetValue(username.ToLowerInvariant(), out User user) &&
                    user.Password == ScadaUtils.GetPasswordHash(user.UserID, password))
                {
                    userID = user.UserID;
                    roleID = user.RoleID;

                    if (roleID > RoleID.Disabled)
                    {
                        errMsg = "";
                        return true;
                    }
                    else
                    {
                        errMsg = Locale.IsRussian ?
                            "Пользователь отключен" :
                            "Account is disabled";
                        return false;
                    }
                }
                else
                {
                    errMsg = Locale.IsRussian ?
                        "Неверное имя пользователя или пароль" :
                        "Invalid username or password";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMsg = Locale.IsRussian ?
                    "Ошибка при проверке пользователя" :
                    "Error validating user";

                log.WriteException(ex, errMsg);
                return false;
            }
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");

            int cnlCnt = cnlNums.Length;
            CnlData[] cnlDataArr = new CnlData[cnlCnt];
            cnlListID = 0;

            try
            {
                CnlListItem cnlListItem = null;

                if (useCache)
                {
                    cnlListID = serverCache.GetNextID();
                    cnlListItem = new CnlListItem(cnlListID, cnlCnt);
                    serverCache.CnlListCache.Add(cnlListID, cnlListItem);
                }

                lock (curData)
                {
                    for (int i = 0; i < cnlCnt; i++)
                    {
                        if (cnlTags.TryGetValue(cnlNums[i], out CnlTag cnlTag))
                        {
                            cnlDataArr[i] = curData.CnlData[cnlTag.Index];
                            cnlListItem?.CnlTags.Add(cnlTag);
                        }
                        else
                        {
                            cnlDataArr[i] = CnlData.Empty;
                            cnlListItem?.CnlTags.Add(new CnlTag());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных" :
                    "Error getting current data");
            }

            return cnlDataArr;
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(long cnlListID)
        {
            CnlData[] cnlDataArr = null;

            try
            {
                CnlListItem cnlListItem = serverCache.CnlListCache.Get(cnlListID);

                if (cnlListItem != null)
                {
                    int cnlCnt = cnlListItem.CnlTags.Count;
                    cnlDataArr = new CnlData[cnlCnt];

                    for (int i = 0; i < cnlCnt; i++)
                    {
                        CnlTag cnlTag = cnlListItem.CnlTags[i];
                        cnlDataArr[i] = cnlTag.Index >= 0 ? curData.CnlData[cnlTag.Index] : CnlData.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных" :
                    "Error getting current data");
            }

            return cnlDataArr;
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public void WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData, bool applyFormulas)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");
            if (cnlData == null)
                throw new ArgumentNullException("cnlData");

            try
            {
                moduleHolder.OnCurrentDataProcessing(deviceNum, cnlNums, cnlData);
                calc.BeginCalculation(curData);

                lock (curData)
                {
                    DateTime utcNow = DateTime.UtcNow;

                    for (int i = 0, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
                    {
                        if (cnlTags.TryGetValue(cnlNums[i], out CnlTag cnlTag))
                        {
                            CnlData newCnlData = cnlData[i];

                            if (applyFormulas && cnlTag.InCnl.FormulaEnabled && 
                                cnlTag.InCnl.CnlTypeID == CnlTypeID.Measured)
                            {
                                newCnlData = calc.CalcCnlData(cnlTag, newCnlData);
                                cnlData[i] = newCnlData;
                            }

                            curData.SetCurCnlData(cnlTag, newCnlData, utcNow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи текущих данных" :
                    "Error writing current data");
            }
            finally
            {
                calc.EndCalculation();
                moduleHolder.OnCurrentDataProcessed(deviceNum, cnlNums, cnlData);
            }
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        public void WriteHistoricalData(int deviceNum, Slice slice, int archiveMask, bool applyFormulas)
        {
            if (slice == null)
                throw new ArgumentNullException("slice");

            try
            {
                moduleHolder.OnHistoricalDataProcessing(deviceNum, slice);
                DateTime timestamp = slice.Timestamp;

                // find input channel tags of the slice
                int cnlCnt = slice.CnlNums.Length;
                CnlTag[] sliceCnlTags = new CnlTag[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    cnlTags.TryGetValue(slice.CnlNums[i], out CnlTag cnlTag);
                    sliceCnlTags[i] = cnlTag;
                }

                for (int archiveBit = 0; archiveBit < ServerUtils.MaxArchiveCount; archiveBit++)
                {
                    if (archiveMask.BitIsSet(archiveBit) && 
                        archiveHolder.GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic) &&
                        archiveLogic.AcceptData(timestamp))
                    {
                        try
                        {
                            calc.BeginCalculation(new ArchiveCalcContext(archiveLogic, timestamp));
                            archiveLogic.BeginUpdate();

                            // calculate input channels which are written
                            for (int i = 0; i < cnlCnt; i++)
                            {
                                CnlTag cnlTag = sliceCnlTags[i];

                                if (cnlTag != null)
                                {
                                    CnlData newCnlData = slice.CnlData[i];

                                    if (applyFormulas && cnlTag.InCnl.FormulaEnabled &&
                                        cnlTag.InCnl.CnlTypeID == CnlTypeID.Measured)
                                    {
                                        newCnlData = calc.CalcCnlData(cnlTag, newCnlData);
                                        UpdateCnlStatus(archiveLogic, timestamp, cnlTag, ref newCnlData);
                                        slice.CnlData[i] = newCnlData;
                                    }

                                    archiveLogic.WriteCnlData(cnlTag.CnlNum, timestamp, newCnlData);
                                }
                            }

                            // calculate input channels of the calculated type
                            foreach (CnlTag cnlTag in calcCnlTags)
                            {
                                CnlData arcCnlData = archiveLogic.GetCnlData(cnlTag.CnlNum, timestamp);
                                CnlData newCnlData = calc.CalcCnlData(cnlTag, arcCnlData);
                                UpdateCnlStatus(archiveLogic, timestamp, cnlTag, ref newCnlData);

                                if (arcCnlData != newCnlData)
                                    archiveLogic.WriteCnlData(cnlTag.CnlNum, timestamp, newCnlData);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.WriteException(ex, string.Format(Locale.IsRussian ?
                                "Ошибка при записи исторических данных в архив {0}" :
                                "Error writing historical data to the {1} archive", archiveLogic.Code));
                        }
                        finally
                        {
                            archiveHolder.EndUpdate(archiveLogic);
                            calc.EndCalculation();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи исторических данных" :
                    "Error writing historical data");
            }
            finally
            {
                moduleHolder.OnHistoricalDataProcessed(deviceNum, slice);
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(Event ev, int archiveMask)
        {
            if (ev == null)
                throw new ArgumentNullException("ev");

            try
            {
                ev.EventID = ScadaUtils.GenerateUniqueID();

                if (ev.CnlNum > 0 && cnlTags.TryGetValue(ev.CnlNum, out CnlTag cnlTag))
                {
                    // set missing event properties
                    InCnl inCnl = cnlTag.InCnl;

                    if (ev.ObjNum <= 0)
                        ev.ObjNum = inCnl.ObjNum ?? 0;

                    if (ev.DeviceNum <= 0)
                        ev.DeviceNum = inCnl.DeviceNum ?? 0;

                    if (baseDataSet.CnlStatusTable.GetItem(ev.CnlStat) is CnlStatus cnlStatus)
                    {
                        if (ev.Severity <= 0)
                            ev.Severity = cnlStatus.Severity ?? 0;

                        if (!ev.AckRequired)
                            ev.AckRequired = cnlStatus.AckRequired;
                    }
                }
                else if (ev.OutCnlNum > 0 && outCnlTags.TryGetValue(ev.OutCnlNum, out OutCnlTag outCnlTag))
                {
                    // set missing event properties
                    OutCnl outCnl = outCnlTag.OutCnl;

                    if (ev.ObjNum <= 0)
                        ev.ObjNum = outCnl.ObjNum ?? 0;

                    if (ev.DeviceNum <= 0)
                        ev.DeviceNum = outCnl.DeviceNum ?? 0;
                }

                log.WriteAction(string.Format(Locale.IsRussian ?
                    "Получено событие с ид. {0}, входным каналом {1} и выходным каналом {2}" :
                    "Event with ID {0}, input channel {1} and output channel {2} received",
                    ev.EventID, ev.CnlNum, ev.OutCnlNum));

                moduleHolder.OnEvent(ev);
                archiveHolder.WriteEvent(ev, archiveMask);
            }
            catch (Exception ex)
            {
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи события" :
                    "Error writing event");
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public void AckEvent(long eventID, DateTime timestamp, int userID)
        {
            log.WriteAction(string.Format(Locale.IsRussian ?
                "Квитирование события с ид. {0}" :
                "Acknowledge event with ID {0}", eventID));
            moduleHolder.OnEventAck(eventID, timestamp, userID);
            archiveHolder.AckEvent(eventID, timestamp, userID);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(TeleCommand command, out CommandResult commandResult)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            commandResult = new CommandResult();

            try
            {
                int outCnlNum = command.OutCnlNum;
                int userID = command.UserID;
                log.WriteAction(string.Format(Locale.IsRussian ?
                    "Отправка команды на канал управления {0} пользователем с ид. {1}" :
                    "Send command to the output channel {0} by the user with ID {1}", outCnlNum, userID));

                if (!baseDataSet.UserTable.Items.TryGetValue(userID, out User user))
                {
                    commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Пользователь {0} не найден" :
                        "User {0} not found", userID);
                    log.WriteError(commandResult.ErrorMessage);
                }
                else if (!outCnlTags.TryGetValue(outCnlNum, out OutCnlTag outCnlTag))
                {
                    commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Канал управления {0} не найден" :
                        "Output channel {0} not found", outCnlNum);
                    log.WriteError(commandResult.ErrorMessage);
                }
                else
                {
                    OutCnl outCnl = outCnlTag.OutCnl;
                    int objNum = outCnlTag.OutCnl.ObjNum ?? 0;

                    if (!objSecurity.GetRights(user.RoleID, objNum).ControlRight)
                    {
                        commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                            "Недостаточно прав пользователя с ролью {0} на управление объектом {1}" :
                            "Insufficient rights of user with role {0} to control object {1}", user.RoleID, objNum);
                        log.WriteError(commandResult.ErrorMessage);
                    }
                    else
                    {
                        command.CommandID = ScadaUtils.GenerateUniqueID();
                        command.CreationTime = DateTime.UtcNow;
                        command.CmdTypeID = outCnl.CmdTypeID;
                        command.ObjNum = objNum;
                        command.DeviceNum = outCnl.DeviceNum ?? 0;
                        command.CmdNum = outCnl.CmdNum ?? 0;
                        command.CmdCode = outCnl.CmdCode;

                        double cmdVal;
                        byte[] cmdData;

                        try
                        {
                            calc.BeginCalculation(curData);
                            Monitor.Enter(curData);
                            commandResult.IsSuccessful = calc.CalcCmdData(outCnlTag, command.CmdVal, command.CmdData,
                                out cmdVal, out cmdData, out string errMsg);
                            commandResult.ErrorMessage = errMsg;
                        }
                        finally
                        {
                            Monitor.Exit(curData);
                            calc.EndCalculation();
                        }

                        moduleHolder.OnCommand(command, commandResult);

                        if (commandResult.IsSuccessful)
                        {
                            command.CmdVal = cmdVal;
                            command.CmdData = cmdData;
                            listener.EnqueueCommand(command);
                            GenerateEvent(outCnlTag, command);
                            log.WriteAction(Locale.IsRussian ?
                                "Команда поставлена в очередь на отправку клиентам" :
                                "Command is queued to be sent to clients");
                        }
                        else
                        {
                            log.WriteAction(string.Format(Locale.IsRussian ?
                                "Невозможно отправить команду: {0}" :
                                "Unable to send command: {0}", commandResult.ErrorMessage));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult.ErrorMessage = ex.Message;
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при отправке команды" :
                    "Error sending command");
            }
        }

        /// <summary>
        /// Handles the changes of the current input channel data.
        /// </summary>
        void ICnlDataChangeHandler.HandleCurDataChanged(CnlTag cnlTag, ref CnlData cnlData, CnlData prevCnlData,
            DateTime timestamp, DateTime prevTimestamp)
        {
            UpdateCnlStatus(cnlTag, ref cnlData, prevCnlData);
            GenerateEvent(cnlTag, cnlData, prevCnlData);
        }
    }
}
