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
using Scada.Server.Modules;
using System;
using System.Collections.Concurrent;
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
        /// Represents an event to be written to archives.
        /// </summary>
        private class EventItem
        {
            public Event Event { get; set; }
            public int ArchiveMask { get; set; }
        }


        /// <summary>
        /// The maximum number of input channels to process per iteration when checking activity.
        /// </summary>
        private const int MaxCnlCountToCheckActivity = 1000;
        /// <summary>
        /// The period of writing application info.
        /// </summary>
        private static readonly TimeSpan WriteInfoPeriod = TimeSpan.FromSeconds(1);

        private readonly string infoFileName;    // the full file name to write application information

        private Thread thread;                   // the working thread of the logic
        private volatile bool terminated;        // necessary to stop the thread
        private DateTime utcStartDT;             // the UTC start time
        private DateTime startDT;                // the local start time
        private ServiceStatus serviceStatus;     // the current service status
        private int lastInfoLength;              // the last info text length

        private Dictionary<int, CnlTag> cnlTags; // the metadata about the input channels accessed by channel number
        private List<CnlTag> measCnlTags;        // the list of the input channel tags of the measured type
        private List<CnlTag> calcCnlTags;        // the list of the input channel tags of the calculated type
        private Dictionary<int, OutCnlTag> outCnlTags; // the metadata about the output channels accessed by channel number
        private Dictionary<string, User> users;  // the users accessed by name
        private ObjSecurity objSecurity;         // provides access control
        private Calculator calc;                 // provides work with scripts and formulas
        private ModuleHolder moduleHolder;       // holds modules
        private ArchiveHolder archiveHolder;     // holds archives
        private ServerCache serverCache;         // the server level cache
        private ServerListener listener;         // the TCP listener
        private CurrentData curData;             // the current data of the input channels
        private Queue<EventItem> events;         // the just generated events


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(ServerConfig config, ServerDirs appDirs, ILog log)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            AppDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            BaseDataSet = null;
            SharedData = null;

            infoFileName = Path.Combine(appDirs.LogDir, ServerUtils.InfoFileName);

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            serviceStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;

            cnlTags = null;
            measCnlTags = null;
            calcCnlTags = null;
            outCnlTags = null;
            users = null;
            objSecurity = null;
            moduleHolder = null;
            archiveHolder = null;
            calc = null;
            curData = null;
            events = null;
            serverCache = null;
            listener = null;
        }


        /// <summary>
        /// Gets the server configuration.
        /// </summary>
        public ServerConfig Config { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public ServerDirs AppDirs { get; }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        public BaseDataSet BaseDataSet { get; private set; }

        /// <summary>
        /// Gets the active input channel numbers.
        /// </summary>
        public int[] CnlNums => curData?.CnlNums;

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public ConcurrentDictionary<string, object> SharedData { get; private set; }

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
            terminated = false;
            utcStartDT = DateTime.UtcNow;
            startDT = utcStartDT.ToLocalTime();
            serviceStatus = ServiceStatus.Undefined;
            WriteInfo();

            if (!Config.PathOptions.CheckExistence(out errMsg))
                return false;

            if (!ReadBase())
                return false;

            InitCnlTags();
            InitOutCnlTags();
            InitUsers();
            InitObjSecurity();

            if (!InitCalculator())
                return false;

            SharedData = new ConcurrentDictionary<string, object>();
            moduleHolder = new ModuleHolder(Log);
            archiveHolder = new ArchiveHolder(Log);
            serverCache = new ServerCache();
            listener = new ServerListener(this, archiveHolder, serverCache);
            curData = new CurrentData(this, cnlTags);
            events = new Queue<EventItem>();

            InitModules();
            InitArchives();
            return true;
        }

        /// <summary>
        /// Reads the configuration database from files.
        /// </summary>
        private bool ReadBase()
        {
            string tableName = Locale.IsRussian ? "неопределена" : "undefined";

            try
            {
                BaseDataSet = new BaseDataSet();
                BaseTableAdapter adapter = new BaseTableAdapter();

                foreach (IBaseTable baseTable in BaseDataSet.AllTables)
                {
                    tableName = baseTable.Name;
                    adapter.FileName = Path.Combine(Config.PathOptions.BaseDir, baseTable.FileNameDat);
                    adapter.Fill(baseTable);
                }

                Log.WriteAction(Locale.IsRussian ?
                    "База конфигурации считана успешно" :
                    "The configuration database has been read successfully");

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
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

            foreach (InCnl inCnl in BaseDataSet.InCnlTable.EnumerateItems())
            {
                if (inCnl.Active && inCnl.CnlNum >= cnlNum)
                {
                    cnlNum = inCnl.CnlNum;
                    Lim lim = inCnl.LimID.HasValue ? BaseDataSet.LimTable.GetItem(inCnl.LimID.Value) : null;
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

            // find channel indexes for limits
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

            Log.WriteInfo(Locale.IsRussian ?
                "Количество активных входных каналов: {0}" :
                "Number of active input channels: {0}", cnlTags.Count);
        }

        /// <summary>
        /// Initializes metadata about the output channels.
        /// </summary>
        private void InitOutCnlTags()
        {
            outCnlTags = new Dictionary<int, OutCnlTag>();

            foreach (OutCnl outCnl in BaseDataSet.OutCnlTable.EnumerateItems())
            {
                if (outCnl.Active)
                    outCnlTags.Add(outCnl.OutCnlNum, new OutCnlTag(outCnl));
            }

            Log.WriteInfo(Locale.IsRussian ?
                "Количество активных каналов управления: {0}" :
                "Number of active output channels: {0}", outCnlTags.Count);
        }

        /// <summary>
        /// Initializes the user dictionary.
        /// </summary>
        private void InitUsers()
        {
            users = new Dictionary<string, User>(BaseDataSet.UserTable.ItemCount);

            foreach (User user in BaseDataSet.UserTable.EnumerateItems())
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
            objSecurity.Init(BaseDataSet);
        }

        /// <summary>
        /// Initializes the calculator.
        /// </summary>
        private bool InitCalculator()
        {
            calc = new Calculator(AppDirs, Log);
            return calc.CompileScripts(BaseDataSet, cnlTags, outCnlTags);
        }

        /// <summary>
        /// Initializes modules.
        /// </summary>
        private void InitModules()
        {
            ServerContext serverContext = new ServerContext(this, archiveHolder, listener);

            foreach (string moduleCode in Config.ModuleCodes)
            {
                if (ModuleFactory.GetModuleLogic(AppDirs.ModDir, moduleCode, serverContext,
                    out ModuleLogic moduleLogic, out string message))
                {
                    Log.WriteAction(message);
                    moduleHolder.AddModule(moduleLogic);
                }
                else
                {
                    Log.WriteError(message);
                }
            }
        }

        /// <summary>
        /// Initializes archives.
        /// </summary>
        private void InitArchives()
        {
            // create map of archives accessed by code
            Dictionary<string, Archive> arcByCode = new Dictionary<string, Archive>();

            foreach (Archive archive in BaseDataSet.ArchiveTable.EnumerateItems())
            {
                if (!string.IsNullOrEmpty(archive.Code))
                {
                    arcByCode[archive.Code] = archive;
                }
            }

            // create archives
            ArchiveContext archiveContext = new ArchiveContext(this);

            foreach (ArchiveConfig archiveConfig in Config.Archives)
            {
                if (archiveConfig.Active)
                {
                    try
                    {
                        if (!arcByCode.TryGetValue(archiveConfig.Code, out Archive archiveEntity))
                        {
                            Log.WriteError(Locale.IsRussian ?
                                "Архив {0} не найден в базе конфигурации" :
                                "Archive {0} not found in the configuration database", archiveConfig.Code);
                        }
                        else if (moduleHolder.GetModule(archiveConfig.Module, out ModuleLogic moduleLogic) &&
                            moduleLogic.ModulePurposes.HasFlag(ModulePurposes.Archive) &&
                            moduleLogic.CreateArchive(archiveContext, archiveConfig, GetProcessedCnls(archiveEntity)) is
                            ArchiveLogic archiveLogic)
                        {
                            archiveHolder.AddArchive(archiveEntity, archiveLogic);
                            Log.WriteAction(Locale.IsRussian ?
                                "Архив {0} инициализирован успешно" :
                                "Archive {0} initialized successfully", archiveConfig.Code);
                        }
                        else
                        {
                            Log.WriteError(Locale.IsRussian ?
                                "Не удалось создать архив {0} с помощью модуля {1}" :
                                "Unable to create archive {0} with the module {1}",
                                archiveConfig.Code, archiveConfig.Module);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, Locale.IsRussian ?
                            "Ошибка при создании архива {0} с помощью модуля {1}" :
                            "Error creating archive {0} with the module {1}",
                            archiveConfig.Code, archiveConfig.Module);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the input channel numbers processed by the archive.
        /// </summary>
        private int[] GetProcessedCnls(Archive archive)
        {
            List<int> cnlNums = new List<int>(cnlTags.Count);
            bool isDefault = archive.IsDefault;
            int archiveBit = archive.Bit;

            foreach (CnlTag cnlTag in cnlTags.Values)
            {
                int? archiveMask = cnlTag.InCnl.ArchiveMask;

                if (archiveMask.HasValue)
                {
                    if (archiveMask.Value.BitIsSet(archiveBit))
                        cnlNums.Add(cnlTag.CnlNum);
                }
                else if (isDefault)
                {
                    cnlNums.Add(cnlTag.CnlNum);
                }
            }

            return cnlNums.ToArray();
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
                archiveHolder.MakeReady();
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
                            // prepare current data for new iteration
                            curData.PrepareIteration(utcNow);

                            // check activity of input channels
                            CheckActivity(ref checkActivityTagIndex, utcNow);

                            // calculate input channel data
                            CalcCnlData(utcNow);

                            // process data by archives
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
                        Log.WriteException(ex, CommonPhrases.LogicCycleError);
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                }
            }
            finally
            {
                calc.FinalizeScripts();
                archiveHolder.WriteCurrentData(curData);
                archiveHolder.Close();
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
            int unrelIfInactive = Config.GeneralOptions.UnrelIfInactive;

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
                    EventItem eventItem = events.Dequeue();
                    Event ev = eventItem.Event;

                    Log.WriteAction(Locale.IsRussian ?
                        "Создано событие с ид. {0}, входным каналом {1} и выходным каналом {2}" :
                        "Generated event with ID {0}, input channel {1} and output channel {2}",
                        ev.EventID, ev.CnlNum, ev.OutCnlNum);

                    moduleHolder.OnEvent(ev);
                    archiveHolder.WriteEvent(ev, eventItem.ArchiveMask);
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
                        .AppendLine("Сервер")
                        .AppendLine("------")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workSpanStr)
                        .Append("Статус       : ").AppendLine(serviceStatus.ToString(true))
                        .Append("Версия       : ").AppendLine(ServerUtils.AppVersion);
                }
                else
                {
                    sb
                        .AppendLine("Server")
                        .AppendLine("------")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("Status         : ").AppendLine(serviceStatus.ToString(false))
                        .Append("Version        : ").AppendLine(ServerUtils.AppVersion);
                }

                if (archiveHolder != null)
                {
                    sb.AppendLine();
                    archiveHolder.AppendInfo(sb);
                }

                if (listener != null)
                {
                    sb.AppendLine();
                    listener.AppendClientInfo(sb);
                }

                lastInfoLength = sb.Length;

                // write to file
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                {
                    writer.Write(sb.ToString());
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, CommonPhrases.WriteInfoError);
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
                    CnlStatus cnlStatus = BaseDataSet.CnlStatusTable.GetItem(cnlData.Stat);
                    DateTime utcNow = DateTime.UtcNow;

                    EnqueueEvent(new Event
                    {
                        EventID = ScadaUtils.GenerateUniqueID(utcNow),
                        Timestamp = utcNow,
                        CnlNum = cnlTag.CnlNum,
                        ObjNum = inCnl.ObjNum ?? 0,
                        DeviceNum = inCnl.DeviceNum ?? 0,
                        PrevCnlVal = prevCnlData.Val,
                        PrevCnlStat = prevStat,
                        CnlVal = cnlData.Val,
                        CnlStat = cnlStat,
                        Severity = cnlStatus?.Severity ?? 0,
                        AckRequired = cnlStatus?.AckRequired ?? false
                    }, inCnl.ArchiveMask ?? ArchiveMask.Default);
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
                DateTime utcNow = DateTime.UtcNow;

                EnqueueEvent(new Event
                {
                    EventID = ScadaUtils.GenerateUniqueID(utcNow),
                    Timestamp = utcNow,
                    OutCnlNum = command.OutCnlNum,
                    ObjNum = command.ObjNum,
                    DeviceNum = command.DeviceNum,
                    CnlVal = command.CmdVal,
                    Data = command.CmdData
                }, ArchiveMask.Default);
            }
        }

        /// <summary>
        /// Adds the event to the queue.
        /// </summary>
        private void EnqueueEvent(Event ev, int archiveMask)
        {
            lock (events)
            {
                events.Enqueue(new EventItem { Event = ev, ArchiveMask = archiveMask });
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
                    
                    if (PrepareProcessing(out string errMsg) && listener.Start())
                    {
                        thread = new Thread(Execute);
                        thread.Start();
                    }
                    else if (!string.IsNullOrEmpty(errMsg))
                    {
                        Log.WriteError(errMsg);
                    }
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

                    if (thread.Join(ScadaUtils.ThreadWait))
                    {
                        Log.WriteAction(CommonPhrases.LogicIsStopped);
                    }
                    else if (ScadaUtils.IsRunningOnCore)
                    {
                        Log.WriteAction(CommonPhrases.UnableToStopLogic);
                    }
                    else
                    {
                        thread.Abort(); // not supported on .NET Core
                        Log.WriteAction(Locale.IsRussian ?
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
                Log.WriteException(ex, CommonPhrases.StopLogicError);
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

                    if (user.Enabled && roleID > RoleID.Disabled)
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

                Log.WriteException(ex, errMsg);
                return false;
            }
        }

        /// <summary>
        /// Gets the current data of the input channel.
        /// </summary>
        public CnlData GetCurrentData(int cnlNum)
        {
            try
            {
                lock (curData)
                {
                    if (cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag))
                        return curData.CnlData[cnlTag.Index];
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных входного канала" :
                    "Error getting current data of the input channel");
            }

            return CnlData.Empty;
        }

        /// <summary>
        /// Gets the current data of the input channels.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID)
        {
            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));

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
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных входных каналов" :
                    "Error getting current data of the input channels");
            }

            return cnlDataArr;
        }

        /// <summary>
        /// Gets the current data of the cached input channel list.
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
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных кэшированного списка входных каналов" :
                    "Error getting current data of the cached input channel list");
            }

            return cnlDataArr;
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public void WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData, bool applyFormulas)
        {
            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));
            if (cnlData == null)
                throw new ArgumentNullException(nameof(cnlData));

            try
            {
                moduleHolder.OnCurrentDataProcessing(deviceNum, cnlNums, cnlData);
                calc.BeginCalculation(curData);

                lock (curData)
                {
                    DateTime utcNow = DateTime.UtcNow;
                    curData.Timestamp = utcNow;

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
                Log.WriteException(ex, Locale.IsRussian ?
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
                throw new ArgumentNullException(nameof(slice));

            try
            {
                moduleHolder.OnHistoricalDataProcessing(deviceNum, slice);
                DateTime timestamp = slice.Timestamp;

                if (archiveMask == ArchiveMask.Default)
                    archiveMask = archiveHolder.DefaultArchiveMask;

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
                        archiveLogic.AcceptData(ref timestamp))
                    {
                        try
                        {
                            calc.BeginCalculation(new ArchiveCalcContext(archiveLogic, timestamp));
                            archiveLogic.Lock();
                            archiveLogic.BeginUpdate(deviceNum, timestamp);

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
                            Log.WriteException(ex, Locale.IsRussian ?
                                "Ошибка при записи исторических данных в архив {0}" :
                                "Error writing historical data to the {1} archive", archiveLogic.Code);
                        }
                        finally
                        {
                            archiveHolder.EndUpdate(archiveLogic, deviceNum, timestamp);
                            archiveHolder.Unlock(archiveLogic);
                            calc.EndCalculation();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
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
                throw new ArgumentNullException(nameof(ev));

            try
            {
                ev.EventID = ScadaUtils.GenerateUniqueID(ev.Timestamp);

                if (ev.CnlNum > 0 && cnlTags.TryGetValue(ev.CnlNum, out CnlTag cnlTag))
                {
                    // set missing event properties
                    InCnl inCnl = cnlTag.InCnl;

                    if (ev.ObjNum <= 0)
                        ev.ObjNum = inCnl.ObjNum ?? 0;

                    if (ev.DeviceNum <= 0)
                        ev.DeviceNum = inCnl.DeviceNum ?? 0;

                    if (BaseDataSet.CnlStatusTable.GetItem(ev.CnlStat) is CnlStatus cnlStatus)
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

                Log.WriteAction(Locale.IsRussian ?
                    "Получено событие с ид. {0}, входным каналом {1} и выходным каналом {2}" :
                    "Received event with ID {0}, input channel {1} and output channel {2}",
                    ev.EventID, ev.CnlNum, ev.OutCnlNum);

                moduleHolder.OnEvent(ev);
                archiveHolder.WriteEvent(ev, archiveMask);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи события" :
                    "Error writing event");
            }
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public void AckEvent(long eventID, DateTime timestamp, int userID)
        {
            Log.WriteAction(Locale.IsRussian ?
                "Квитирование события с ид. {0}" :
                "Acknowledge event with ID {0}", eventID);
            moduleHolder.OnEventAck(eventID, timestamp, userID);
            archiveHolder.AckEvent(eventID, timestamp, userID);
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(TeleCommand command, out CommandResult commandResult)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            commandResult = new CommandResult(false);

            try
            {
                int outCnlNum = command.OutCnlNum;
                int userID = command.UserID;
                Log.WriteAction(Locale.IsRussian ?
                    "Команда на канал управления {0} от пользователя с ид. {1}" :
                    "Command to the output channel {0} from the user with ID {1}", outCnlNum, userID);

                if (!BaseDataSet.UserTable.Items.TryGetValue(userID, out User user))
                {
                    commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Пользователь {0} не найден" :
                        "User {0} not found", userID);
                    Log.WriteError(commandResult.ErrorMessage);
                }
                else if (!outCnlTags.TryGetValue(outCnlNum, out OutCnlTag outCnlTag))
                {
                    commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Канал управления {0} не найден" :
                        "Output channel {0} not found", outCnlNum);
                    Log.WriteError(commandResult.ErrorMessage);
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
                        Log.WriteError(commandResult.ErrorMessage);
                    }
                    else
                    {
                        DateTime utcNow = DateTime.UtcNow;
                        command.CommandID = ScadaUtils.GenerateUniqueID(utcNow);
                        command.CreationTime = utcNow;
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
                            curData.Timestamp = command.CreationTime;
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
                            Log.WriteAction(Locale.IsRussian ?
                                "Команда поставлена в очередь на отправку клиентам" :
                                "Command is queued to be sent to clients");
                        }
                        else
                        {
                            Log.WriteAction(Locale.IsRussian ?
                                "Невозможно отправить команду: {0}" :
                                "Unable to send command: {0}", commandResult.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult.ErrorMessage = ex.Message;
                Log.WriteException(ex, Locale.IsRussian ?
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
