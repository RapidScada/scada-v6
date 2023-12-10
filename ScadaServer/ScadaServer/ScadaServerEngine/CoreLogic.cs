/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : Implements the core Server logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Config;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using Scada.Log;
using Scada.Protocol;
using Scada.Server.Archives;
using Scada.Server.Config;
using Scada.Server.Lang;
using Scada.Server.Modules;
using Scada.Storages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements the core Server logic.
    /// <para>Реализует основную логику Сервера.</para>
    /// </summary>
    internal class CoreLogic
    {
        /// <summary>
        /// Represents an event to be written to archives.
        /// </summary>
        private class EventItem
        {
            public int ArchiveMask { get; set; }
            public Event Event { get; set; }
        }

        /// <summary>
        /// Represents a preprocessed command to send.
        /// </summary>
        private class CommandItem
        {
            public User User { get; set; }
            public OutCnlTag OutCnlTag { get; set; }
            public TeleCommand Command { get; set; }
            public WriteCommandFlags Flags { get; set; }
            public CommandResult Result { get; set; }
        }


        /// <summary>
        /// The maximum number of channels to process per iteration when checking activity.
        /// </summary>
        private const int MaxCnlCountToCheckActivity = 1000;

        private readonly string infoFileName;    // the full file name to write application information

        private Thread thread;                   // the working thread of the logic
        private volatile bool terminated;        // necessary to stop the thread
        private DateTime utcStartDT;             // the UTC start time
        private DateTime startDT;                // the local start time
        private ServiceStatus serviceStatus;     // the current service status
        private int lastInfoLength;              // the last info text length

        private Dictionary<int, CnlTag> cnlTags;       // the channel tags for archiving, not sorted
        private Dictionary<int, OutCnlTag> outCnlTags; // the channel tags for sending commands, not sorted
        private List<CnlTag> measCnlTags;        // the channel tags measured by devices
        private List<CnlTag> calcCnlTags;        // the channel tags of the calculated type
        private Dictionary<string, User> users;  // the users accessed by name
        private ServerRightMatrix rightMatrix;   // provides access control
        private Calculator calc;                 // provides work with scripts and formulas
        private ModuleHolder moduleHolder;       // holds modules
        private ArchiveHolder archiveHolder;     // holds archives
        private ServerCache serverCache;         // the server level cache
        private ServerListener listener;         // the TCP listener
        private CurrentData curData;             // the current channel data
        private Queue<EventItem> eventQueue;     // the just created events
        private Queue<CommandItem> commandQueue; // the preprocessed commands that have not yet been sent


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(InstanceConfig instanceConfig, ServerConfig appConfig, ServerDirs appDirs, 
            IStorage storage, ILog log)
        {
            InstanceConfig = instanceConfig ?? throw new ArgumentNullException(nameof(instanceConfig));
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            AppDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            ConfigDatabase = null;
            Cnls = null;
            SharedData = null;

            infoFileName = Path.Combine(appDirs.LogDir, ServerUtils.InfoFileName);

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            serviceStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;

            cnlTags = null;
            outCnlTags = null;
            measCnlTags = null;
            calcCnlTags = null;
            users = null;
            rightMatrix = null;
            calc = null;
            moduleHolder = null;
            archiveHolder = null;
            serverCache = null;
            listener = null;
            curData = null;
            eventQueue = null;
            commandQueue = null;
        }


        /// <summary>
        /// Gets the instance configuration.
        /// </summary>
        public InstanceConfig InstanceConfig { get; }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public ServerConfig AppConfig { get; }

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public ServerDirs AppDirs { get; }

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
        /// Gets the channels organized in categories.
        /// </summary>
        public ClassifiedChannels Cnls { get; private set; }

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public ConcurrentDictionary<string, object> SharedData { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Server service is ready.
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
        private bool PrepareProcessing()
        {
            terminated = false;
            utcStartDT = DateTime.UtcNow;
            startDT = utcStartDT.ToLocalTime();
            serviceStatus = ServiceStatus.Starting;
            WriteInfo();

            if (!ReadConfigDatabase())
                return false;

            InitChannels();
            InitUsers();
            InitRightMatrix();

            if (!InitCalculator())
                return false;

            SharedData = new ConcurrentDictionary<string, object>();
            moduleHolder = new ModuleHolder(Log);
            archiveHolder = new ArchiveHolder(Log);
            serverCache = new ServerCache();
            listener = new ServerListener(this, archiveHolder, serverCache);
            curData = new CurrentData(cnlTags, HandleCurDataChanging);
            eventQueue = new Queue<EventItem>();
            commandQueue = new Queue<CommandItem>();

            InitModules();
            InitArchives();
            return true;
        }

        /// <summary>
        /// Reads the configuration database from the storage.
        /// </summary>
        private bool ReadConfigDatabase()
        {
            string tableTitle = CommonPhrases.UndefinedTable;

            try
            {
                ConfigDatabase = new ConfigDatabase();

                foreach (IBaseTable baseTable in ConfigDatabase.AllTables)
                {
                    tableTitle = baseTable.Title;
                    Storage.ReadBaseTable(baseTable);
                }

                tableTitle = CommonPhrases.UndefinedTable;
                ConfigDatabase.Init();

                Log.WriteAction(Locale.IsRussian ?
                    "База конфигурации считана успешно" :
                    "The configuration database has been read successfully");
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при чтении базы конфигурации, таблица {0}" :
                    "Error reading the configuration database, the {0} table", tableTitle);
                return false;
            }
        }

        /// <summary>
        /// Initializes metadata about the channels.
        /// </summary>
        private void InitChannels()
        {
            // create collections of channels and channel tags
            Cnls = new ClassifiedChannels();
            cnlTags = new Dictionary<int, CnlTag>();
            outCnlTags = new Dictionary<int, OutCnlTag>();
            measCnlTags = new List<CnlTag>();
            calcCnlTags = new List<CnlTag>();
            List<CnlTag> limTags = new List<CnlTag>();
            int cnlIdx = 0;
            int cnlNum = 0;

            // add archivable channel to collections
            void AddCnl(Cnl cnl, Lim lim)
            {
                if (cnl.IsArchivable())
                {
                    Cnls.ArcCnls.Add(cnlNum, cnl);
                    CnlTag cnlTag = new CnlTag(cnlIdx, cnlNum, cnl, lim);
                    cnlTags.Add(cnlNum, cnlTag);

                    if (cnl.IsInput())
                    {
                        Cnls.MeasCnls.Add(cnlNum, cnl);
                        measCnlTags.Add(cnlTag);
                    }
                    else if (cnl.IsCalculated())
                    {
                        Cnls.CalcCnls.Add(cnlNum, cnl);
                        calcCnlTags.Add(cnlTag);
                    }

                    if (lim != null && lim.IsBoundToCnl)
                        limTags.Add(cnlTag);

                    cnlIdx++;
                    cnlNum++;
                }
            }

            // add output channel to collections
            void AddOutCnl(Cnl cnl)
            {
                if (cnl.IsOutput())
                {
                    Cnls.OutCnls.Add(cnl.CnlNum, cnl);
                    outCnlTags.Add(cnl.CnlNum, new OutCnlTag(cnl));
                }
            }

            foreach (Cnl cnl in ConfigDatabase.CnlTable)
            {
                if (cnl.Active && cnl.CnlNum >= cnlNum)
                {
                    cnlNum = cnl.CnlNum;
                    Lim lim = cnl.LimID.HasValue ? ConfigDatabase.LimTable.GetItem(cnl.LimID.Value) : null;
                    AddCnl(cnl, lim);
                    AddOutCnl(cnl);

                    // add channel tags if one channel row defines multiple channels
                    if (cnl.IsArray())
                    {
                        for (int i = 1, len = cnl.DataLen.Value; i < len; i++)
                        {
                            AddCnl(cnl, lim);
                        }
                    }
                }
            }

            // find channel indexes for limits
            int FindIndex(double? n)
            {
                return n.HasValue && cnlTags.TryGetValue((int)n, out CnlTag t) ? t.Index : -1;
            }

            foreach (CnlTag cnlTag in limTags)
            {
                cnlTag.LimCnlIndex = new LimCnlIndex
                {
                    LoLo = FindIndex(cnlTag.Lim.LoLo),
                    Low = FindIndex(cnlTag.Lim.Low),
                    High = FindIndex(cnlTag.Lim.High),
                    HiHi = FindIndex(cnlTag.Lim.HiHi),
                    Deadband = FindIndex(cnlTag.Lim.Deadband)
                };
            }

            Log.WriteInfo(Locale.IsRussian ?
                "Количество активных каналов для архивирования: {0}" :
                "Number of active channels for archiving: {0}", cnlTags.Count);
            Log.WriteInfo(Locale.IsRussian ?
                "Количество активных каналов для отправки команд: {0}" :
                "Number of active channels for sending commands: {0}", outCnlTags.Count);
        }

        /// <summary>
        /// Initializes the user dictionary.
        /// </summary>
        private void InitUsers()
        {
            users = new Dictionary<string, User>(ConfigDatabase.UserTable.ItemCount);

            foreach (User user in ConfigDatabase.UserTable)
            {
                if (!string.IsNullOrEmpty(user.Name))
                    users[user.Name.ToLowerInvariant()] = user;
            }
        }

        /// <summary>
        /// Initializes the right matrix.
        /// </summary>
        private void InitRightMatrix()
        {
            rightMatrix = new ServerRightMatrix();
            rightMatrix.Init(ConfigDatabase);
        }

        /// <summary>
        /// Initializes the calculator.
        /// </summary>
        private bool InitCalculator()
        {
            HashSet<int> enableFormulasObjNums = AppConfig.GeneralOptions.DisableFormulas ?
                new HashSet<int>(AppConfig.GeneralOptions.EnableFormulasObjNums) : null;
            calc = new Calculator(AppDirs, Log);
            return calc.CompileScripts(ConfigDatabase, enableFormulasObjNums, cnlTags, outCnlTags);
        }

        /// <summary>
        /// Initializes modules.
        /// </summary>
        private void InitModules()
        {
            ServerContext serverContext = new ServerContext(this, archiveHolder, listener);

            foreach (string moduleCode in AppConfig.ModuleCodes)
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

            foreach (Archive archive in ConfigDatabase.ArchiveTable)
            {
                if (!string.IsNullOrEmpty(archive.Code))
                {
                    arcByCode[archive.Code] = archive;
                }
            }

            // create archives
            ArchiveContext archiveContext = new ArchiveContext(this);

            foreach (ArchiveConfig archiveConfig in AppConfig.Archives)
            {
                if (archiveConfig.Active)
                {
                    try
                    {
                        if (archiveHolder.ArchiveExists(archiveConfig.Code))
                        {
                            Log.WriteError(Locale.IsRussian ?
                                "Архив {0} дублируется" :
                                "Archive {0} is duplicated", archiveConfig.Code);
                        }
                        else if (!arcByCode.TryGetValue(archiveConfig.Code, out Archive archiveEntity))
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
                        Log.WriteError(ex, Locale.IsRussian ?
                            "Ошибка при создании архива {0} с помощью модуля {1}" :
                            "Error creating archive {0} with the module {1}",
                            archiveConfig.Code, archiveConfig.Module);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the sorted channel numbers processed by the archive.
        /// </summary>
        private int[] GetProcessedCnls(Archive archive)
        {
            List<int> cnlNums = new List<int>();

            foreach (KeyValuePair<int, Cnl> pair in Cnls.ArcCnls)
            {
                int? archiveMask = pair.Value.ArchiveMask;

                if (archiveMask.HasValue)
                {
                    if (archiveMask.Value.BitIsSet(archive.Bit))
                        cnlNums.Add(pair.Key);
                }
                else if (archive.IsDefault)
                {
                    cnlNums.Add(pair.Key);
                }
            }

            return cnlNums.ToArray();
        }

        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                int checkActivityTagIndex = 0; // the channel tag index which is checked for activity
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
                            curData.Timestamp = utcNow;

                            // check channel activity
                            CheckActivity(ref checkActivityTagIndex, utcNow);

                            // calculate channel data
                            CalcCnlData(utcNow);

                            // process data by archives
                            archiveHolder.ProcessData(curData);
                        }

                        // process events and commands without blocking current data
                        ProcessEvents();
                        ProcessCommands();

                        // process modules and archives
                        moduleHolder.OnIteration();
                        archiveHolder.DeleteOutdatedData();

                        // write application info
                        if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                        {
                            writeInfoDT = utcNow;
                            WriteInfo();
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        // do nothing
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
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, CommonPhrases.ThreadFatalError);
            }
            finally
            {
                WriteInfo();
                calc.FinalizeScripts();
                curData.Timestamp = DateTime.UtcNow;
                archiveHolder.WriteCurrentData(curData);
                archiveHolder.Close();
                moduleHolder.OnServiceStop();
                serviceStatus = ServiceStatus.Terminated;
                WriteInfo();
            }
        }

        /// <summary>
        /// Checks if the channels are active and sets the status of inactive channels as unreliable.
        /// </summary>
        private void CheckActivity(ref int tagIndex, DateTime nowDT)
        {
            int unrelIfInactive = AppConfig.GeneralOptions.UnrelIfInactive;

            if (unrelIfInactive > 0)
            {
                for (int i = 0, cnt = measCnlTags.Count; i < MaxCnlCountToCheckActivity && tagIndex < cnt; i++)
                {
                    CnlTag cnlTag = measCnlTags[tagIndex++];
                    CnlData cnlData = curData.CnlData[cnlTag.Index];

                    if (cnlData.IsDefined && cnlData.Stat != CnlStatusID.Unreliable &&
                        (nowDT - curData.Timestamps[cnlTag.Index]).TotalSeconds > unrelIfInactive)
                    {
                        CnlData unrelCnlData = new CnlData(cnlData.Val, CnlStatusID.Unreliable);
                        curData.SetCurrentData(cnlTag, ref unrelCnlData, nowDT);
                    }
                }

                if (tagIndex >= measCnlTags.Count)
                    tagIndex = 0;
            }
        }

        /// <summary>
        /// Calculates the channels of the calculated type.
        /// </summary>
        private void CalcCnlData(DateTime nowDT)
        {
            foreach (CnlTag cnlTag in calcCnlTags)
            {
                if (cnlTag.InFormulaEnabled)
                {
                    CnlData newCnlData = calc.CalcCnlData(curData, cnlTag, CnlData.Zero);
                    curData.SetCurrentData(cnlTag, ref newCnlData, nowDT);
                }
            }
        }

        /// <summary>
        /// Processes the events from the queue.
        /// </summary>
        private void ProcessEvents()
        {
            while (true)
            {
                EventItem eventItem;
                Event ev;

                lock (eventQueue)
                {
                    if (eventQueue.Count > 0)
                    {
                        eventItem = eventQueue.Dequeue();
                        ev = eventItem.Event;
                    }
                    else
                    {
                        break;
                    }
                }

                Log.WriteAction(Locale.IsRussian ?
                    "Создано событие с ид. {0} и каналом {1}" :
                    "Created event with ID {0} and channel {1}", ev.EventID, ev.CnlNum);

                moduleHolder.OnEvent(ev);
                archiveHolder.WriteEvent(eventItem.ArchiveMask, ev);
            }
        }

        /// <summary>
        /// Processes the commands from the queue.
        /// </summary>
        private void ProcessCommands()
        {
            while (true)
            {
                CommandItem commandItem;

                lock (commandQueue)
                {
                    if (commandQueue.Count > 0)
                        commandItem = commandQueue.Dequeue();
                    else
                        break;
                }

                if (commandItem.Result.IsSuccessful)
                    moduleHolder.OnCommand(commandItem.Command, commandItem.Result);

                if (commandItem.Result.IsSuccessful)
                {
                    if (commandItem.Flags.HasFlag(WriteCommandFlags.EnableEvents))
                        GenerateEvent(commandItem);

                    if (commandItem.Result.TransmitToClients)
                    {
                        listener.EnqueueCommand(commandItem.Command, 
                            commandItem.Flags.HasFlag(WriteCommandFlags.ReturnToSender));
                        Log.WriteAction(Locale.IsRussian ?
                            "Команда поставлена в очередь на отправку клиентам" :
                            "Command is queued to be sent to clients");
                    }
                    else
                    {
                        Log.WriteAction(Locale.IsRussian ?
                            "Отправка команды клиентам отменена" :
                            "Sending command to clients canceled");
                    }
                }
                else
                {
                    Log.WriteAction(Locale.IsRussian ?
                        "Невозможно отправить команду: {0}" :
                        "Unable to send command: {0}", commandItem.Result.ErrorMessage);
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
                        .Append("Версия       : ").AppendLine(EngineUtils.AppVersion);
                }
                else
                {
                    sb
                        .AppendLine("Server")
                        .AppendLine("------")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("Status         : ").AppendLine(serviceStatus.ToString(false))
                        .Append("Version        : ").AppendLine(EngineUtils.AppVersion);
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
                Log.WriteError(ex, CommonPhrases.WriteInfoError);
            }
        }

        /// <summary>
        /// Handles a change of the current channel data.
        /// </summary>
        private void HandleCurDataChanging(CnlTag cnlTag, ref CnlData cnlData,
            CnlData prevCnlData, CnlData prevCnlDataDef, bool enableEvents)
        {
            UpdateCnlStatus(cnlTag, ref cnlData, prevCnlData);

            if (enableEvents)
                GenerateEvent(cnlTag, cnlData, prevCnlData, prevCnlDataDef);
        }

        /// <summary>
        /// Updates the channel status after formula calculation for the current data.
        /// </summary>
        private void UpdateCnlStatus(CnlTag cnlTag, ref CnlData cnlData, CnlData prevCnlData)
        {
            // use double.IsFinite on .NET Core
            if (double.IsNaN(cnlData.Val) ||
                double.IsInfinity(cnlData.Val))
            {
                // set undefined status if value is not a number
                cnlData = CnlData.Empty;
            }
            else if (cnlData.Stat == CnlStatusID.Defined && cnlTag.Lim != null)
            {
                // set status depending on channel limits
                GetCnlLimits(cnlTag, out double lolo, out double low, 
                    out double high, out double hihi, out double deadband);
                int newStat = GetCnlStatus(cnlData.Val, lolo, low, high, hihi);
                int prevStat = prevCnlData.Stat;

                // take deadband into account
                if (deadband != 0.0 && (
                    prevStat != CnlStatusID.Normal && newStat == CnlStatusID.Normal ||
                    prevStat == CnlStatusID.HiHi && newStat == CnlStatusID.High ||
                    prevStat == CnlStatusID.LoLo && newStat == CnlStatusID.Low))
                {
                    newStat = GetCnlStatus(cnlData.Val,
                        lolo + deadband, low + deadband,
                        high - deadband, hihi - deadband);
                }

                cnlData.Stat = newStat;
            }
        }

        /// <summary>
        /// Updates the channel status after formula calculation for the archive at the timestamp.
        /// </summary>
        private void UpdateCnlStatus(HistoricalArchiveLogic archiveLogic, DateTime timestamp, 
            CnlTag cnlTag, ref CnlData cnlData)
        {
            if (double.IsNaN(cnlData.Val))
            {
                cnlData = CnlData.Empty;
            }
            else if (cnlData.Stat == CnlStatusID.Defined)
            {
                if (cnlTag.Lim == null)
                {
                    if (AppConfig.GeneralOptions.UseArchivalStatus)
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
        /// Get the channel limits for the current data.
        /// </summary>
        private void GetCnlLimits(CnlTag cnlTag, out double lolo, out double low, 
            out double high, out double hihi, out double deadband)
        {
            double GetLimit(int cnlIndex, double defaultVal)
            {
                CnlData limData = cnlIndex >= 0 ? curData.CnlData[cnlIndex] : CnlData.Empty;
                return limData.Stat > 0 ? limData.Val : defaultVal;
            }

            if (cnlTag.Lim.IsBoundToCnl)
            {
                LimCnlIndex limCnlIndex = cnlTag.LimCnlIndex;
                lolo = GetLimit(limCnlIndex.LoLo, double.NaN);
                low = GetLimit(limCnlIndex.Low, double.NaN);
                high = GetLimit(limCnlIndex.High, double.NaN);
                hihi = GetLimit(limCnlIndex.HiHi, double.NaN);
                deadband = GetLimit(limCnlIndex.Deadband, 0);
            }
            else
            {
                Lim lim = cnlTag.Lim;
                lolo = lim.LoLo ?? double.NaN;
                low = lim.Low ?? double.NaN;
                high = lim.High ?? double.NaN;
                hihi = lim.HiHi ?? double.NaN;
                deadband = lim.Deadband ?? 0.0;
            }
        }

        /// <summary>
        /// Get the channel limits for the archive at the timestamp.
        /// </summary>
        private void GetCnlLimits(HistoricalArchiveLogic archiveLogic, DateTime timestamp, CnlTag cnlTag, 
            out double lolo, out double low, out double high, out double hihi)
        {
            double GetLimit(double cnlNum)
            {
                CnlData limData = cnlNum > 0 ? archiveLogic.GetCnlData(timestamp, (int)cnlNum) : CnlData.Empty;
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
        /// Get the channel status depending on whether the channel value is within the limits.
        /// </summary>
        private int GetCnlStatus(double cnlVal, double lolo, double low, double high, double hihi)
        {
            if (cnlVal < lolo)
                return CnlStatusID.LoLo;
            else if (cnlVal < low)
                return CnlStatusID.Low;
            else if (cnlVal > hihi)
                return CnlStatusID.HiHi;
            else if (cnlVal > high)
                return CnlStatusID.High;
            else
                return CnlStatusID.Normal;
        }

        /// <summary>
        /// Generates an event based on channel data.
        /// </summary>
        private void GenerateEvent(CnlTag cnlTag, CnlData cnlData, CnlData prevCnlData, CnlData prevCnlDataDef)
        {
            Cnl cnl = cnlTag.Cnl;
            EventMask eventMask = new EventMask(cnl.EventMask);

            bool DataChanged() => cnlData.IsDefined && prevCnlDataDef.IsDefined && 
                cnlData != prevCnlDataDef && cnl.IsNumeric();
            bool ValueChanged() => cnlData.IsDefined && prevCnlDataDef.IsDefined && 
                !cnlData.Val.Equals(prevCnlDataDef.Val) && cnl.IsNumeric(); // NaN == NaN
            bool StatusChanged() => cnlData.IsDefined && prevCnlDataDef.IsDefined &&
                cnlData.Stat != prevCnlDataDef.Stat && (cnl.IsNumeric() || cnlTag.ArrIdx == 0);
            bool UndefinedChanged() => cnlData.IsUndefined != prevCnlData.IsUndefined && 
                (cnl.IsNumeric() || cnlTag.ArrIdx == 0);

            if (eventMask.Enabled)
            {
                if (eventMask.DataChange && DataChanged() ||
                    eventMask.ValueChange && ValueChanged() ||
                    eventMask.StatusChange && StatusChanged() ||
                    eventMask.CnlUndefined && UndefinedChanged())
                {
                    CnlStatus cnlStatus = ConfigDatabase.CnlStatusTable.GetItem(cnlData.Stat);
                    DateTime utcNow = DateTime.UtcNow;

                    EnqueueEvent(cnl.ArchiveMask ?? ArchiveMask.Default, new Event
                    {
                        EventID = ScadaUtils.GenerateUniqueID(utcNow),
                        Timestamp = utcNow,
                        CnlNum = cnlTag.CnlNum,
                        ObjNum = cnl.ObjNum ?? 0,
                        DeviceNum = cnl.DeviceNum ?? 0,
                        PrevCnlVal = prevCnlData.Val,
                        PrevCnlStat = prevCnlData.Stat,
                        CnlVal = cnlData.Val,
                        CnlStat = cnlData.Stat,
                        Severity = cnlStatus?.Severity ?? 0,
                        AckRequired = cnlStatus?.AckRequired ?? false
                    });
                }
            }
        }

        /// <summary>
        /// Generates an event based on the command.
        /// </summary>
        private void GenerateEvent(CommandItem commandItem)
        {
            EventMask eventMask = new EventMask(commandItem.OutCnlTag.Cnl.EventMask);

            if (eventMask.Enabled && eventMask.Command)
            {
                DateTime utcNow = DateTime.UtcNow;
                TeleCommand command = commandItem.Command;

                EnqueueEvent(ArchiveMask.Default, new Event
                {
                    EventID = ScadaUtils.GenerateUniqueID(utcNow),
                    Timestamp = utcNow,
                    CnlNum = command.CnlNum,
                    ObjNum = command.ObjNum,
                    DeviceNum = command.DeviceNum,
                    CnlVal = double.IsNaN(command.CmdVal) ? 0.0 : command.CmdVal,
                    CnlStat = double.IsNaN(command.CmdVal) ? CnlStatusID.Undefined : CnlStatusID.Defined,
                    TextFormat = EventTextFormat.Command,
                    Text = string.Format(ServerPhrases.CommandSentBy, commandItem.User.Name),
                    Data = command.CmdData
                });
            }
        }

        /// <summary>
        /// Adds the command to the queue.
        /// </summary>
        private void EnqueueCommand(User user, OutCnlTag outCnlTag, 
            TeleCommand command, WriteCommandFlags flags, CommandResult result)
        {
            lock (commandQueue)
            {
                commandQueue.Enqueue(new CommandItem
                {
                    User = user,
                    OutCnlTag = outCnlTag,
                    Command = command,
                    Flags = flags,
                    Result = result
                });
            }
        }

        /// <summary>
        /// Adds the event to the queue.
        /// </summary>
        private void EnqueueEvent(int archiveMask, Event ev)
        {
            lock (eventQueue)
            {
                eventQueue.Enqueue(new EventItem 
                {
                    ArchiveMask = archiveMask, 
                    Event = ev 
                });
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
                    
                    if (PrepareProcessing() && listener.Start())
                    {
                        thread = new Thread(Execute);
                        thread.Start();
                    }
                }
                else
                {
                    Log.WriteAction(CommonPhrases.LogicAlreadyStarted);
                }

                return thread != null;
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
                // stop listener
                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }

                // stop logic processing
                if (thread == null)
                {
                    serviceStatus = ServiceStatus.Terminated;
                    WriteInfo();
                }
                else
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
        /// Validates the username and password.
        /// </summary>
        public UserValidationResult ValidateUser(string username, string password)
        {
            try
            {
                // ignore leading and trailing white-space when searching for a user
                username = username?.Trim();

                // prohibit empty username or password
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    return UserValidationResult.Fail(ServerPhrases.EmptyCredentials);

                // validate by the configuration database
                if (users.TryGetValue(username.ToLowerInvariant(), out User user) &&
                    !string.IsNullOrEmpty(user.Password))
                {
                    if (user.Password == ScadaUtils.GetPasswordHash(user.UserID, password))
                    {
                        UserValidationResult result = new UserValidationResult
                        {
                            UserID = user.UserID,
                            RoleID = user.RoleID,
                        };

                        if (user.Enabled && user.RoleID > RoleID.Disabled)
                            result.IsValid = true;
                        else
                            result.ErrorMessage = ServerPhrases.AccountDisabled;

                        return result;
                    }
                    else
                    {
                        return UserValidationResult.Fail(ServerPhrases.InvalidCredentials);
                    }
                }

                // validate by modules
                UserValidationResult moduleResult = moduleHolder.ValidateUser(username, password);

                if (moduleResult.Handled)
                    return moduleResult;
                else
                    return UserValidationResult.Fail(ServerPhrases.InvalidCredentials);
            }
            catch (Exception ex)
            {
                string errMsg = Locale.IsRussian ?
                    "Ошибка при проверке пользователя" :
                    "Error validating user";
                Log.WriteError(ex, errMsg);
                return UserValidationResult.Fail(errMsg);
            }
        }

        /// <summary>
        /// Finds a user by ID.
        /// </summary>
        public User FindUser(int userID)
        {
            return ConfigDatabase.UserTable.GetItem(userID) ?? moduleHolder.FindUser(userID);
        }

        /// <summary>
        /// Gets the current data of the specified channel of the certain kind.
        /// </summary>
        public CnlData GetCurrentData(int cnlNum, CurrentDataKind kind)
        {
            try
            {
                lock (curData)
                {
                    return curData.GetCurrentData(cnlNum, kind);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных канала" :
                    "Error getting current data of the channel");
                return CnlData.Empty;
            }
        }

        /// <summary>
        /// Gets the current data of the specified channels.
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
                    cnlListID = ScadaUtils.GenerateUniqueID();
                    cnlListItem = new CnlListItem(cnlListID, cnlCnt);

                    if (!serverCache.CnlListCache.Add(cnlListID, cnlListItem))
                        cnlListID = 0;
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
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных каналов" :
                    "Error getting current data of the channels");
            }

            return cnlDataArr;
        }

        /// <summary>
        /// Gets the current data of the cached channel list.
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
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных кэшированного списка каналов" :
                    "Error getting current data of the cached channel list");
            }

            return cnlDataArr;
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public void WriteCurrentData(Slice slice, WriteDataFlags flags)
        {
            if (slice == null)
                throw new ArgumentNullException(nameof(slice));

            try
            {
                moduleHolder.OnCurrentDataProcessing(slice);
                Monitor.Enter(curData);

                if (slice.Timestamp == DateTime.MinValue)
                    slice.Timestamp = DateTime.UtcNow;

                curData.Timestamp = slice.Timestamp;
                bool applyFormulas = flags.HasFlag(WriteDataFlags.ApplyFormulas);
                bool enableEvents = flags.HasFlag(WriteDataFlags.EnableEvents);

                for (int i = 0, cnlCnt = slice.Length; i < cnlCnt; i++)
                {
                    if (cnlTags.TryGetValue(slice.CnlNums[i], out CnlTag cnlTag))
                    {
                        CnlData cnlData = applyFormulas && cnlTag.InFormulaEnabled && cnlTag.Cnl.IsInput()
                            ? calc.CalcCnlData(curData, cnlTag, slice.CnlData[i])
                            : slice.CnlData[i];
                        curData.SetCurrentData(cnlTag, ref cnlData, slice.Timestamp, enableEvents);
                        slice.CnlData[i] = cnlData;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при записи текущих данных" :
                    "Error writing current data");
            }
            finally
            {
                Monitor.Exit(curData);
                moduleHolder.OnCurrentDataProcessed(slice);
            }
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        public void WriteHistoricalData(int archiveMask, Slice slice, WriteDataFlags flags)
        {
            if (slice == null)
                throw new ArgumentNullException(nameof(slice));

            try
            {
                moduleHolder.OnHistoricalDataProcessing(slice);
                DateTime timestamp = slice.Timestamp;

                if (archiveMask == ArchiveMask.Default)
                    archiveMask = archiveHolder.DefaultArchiveMask;

                // find channel tags of the slice
                int cnlCnt = slice.Length;
                CnlTag[] sliceCnlTags = new CnlTag[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    cnlTags.TryGetValue(slice.CnlNums[i], out CnlTag cnlTag);
                    sliceCnlTags[i] = cnlTag;
                }

                // write to archives
                bool applyFormulas = flags.HasFlag(WriteDataFlags.ApplyFormulas);
                CnlData[] cnlDataCopy = slice.CnlData.DeepClone();

                for (int archiveBit = 0; archiveBit < ServerUtils.MaxArchiveCount; archiveBit++)
                {
                    if (archiveMask.BitIsSet(archiveBit) && 
                        archiveHolder.GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic) &&
                        archiveLogic.AcceptData(ref timestamp))
                    {
                        UpdateContext updateContext = new UpdateContext(timestamp, slice.DeviceNum);

                        try
                        {
                            archiveLogic.BeginUpdate(updateContext);
                            archiveLogic.CurrentUpdateContext = updateContext;
                            ICalcContext calcContext = new ArchiveCalcContext(archiveLogic, updateContext);

                            // calculate written channels
                            for (int i = 0; i < cnlCnt; i++)
                            {
                                if (sliceCnlTags[i] is CnlTag cnlTag)
                                {
                                    CnlData cnlData = applyFormulas && cnlTag.InFormulaEnabled && cnlTag.Cnl.IsInput()
                                        ? calc.CalcCnlData(calcContext, cnlTag, cnlDataCopy[i])
                                        : cnlDataCopy[i];
                                    UpdateCnlStatus(archiveLogic, timestamp, cnlTag, ref cnlData);
                                    slice.CnlData[i] = cnlData;
                                    archiveLogic.UpdateData(updateContext, cnlTag.CnlNum, cnlData);
                                }
                            }

                            // calculate channels of the calculated type
                            foreach (CnlTag cnlTag in calcCnlTags)
                            {
                                if (cnlTag.InFormulaEnabled)
                                {
                                    CnlData arcCnlData = archiveLogic.GetCnlData(timestamp, cnlTag.CnlNum);
                                    CnlData newCnlData = calc.CalcCnlData(calcContext, cnlTag, arcCnlData);
                                    UpdateCnlStatus(archiveLogic, timestamp, cnlTag, ref newCnlData);

                                    if (arcCnlData != newCnlData)
                                        archiveLogic.UpdateData(updateContext, cnlTag.CnlNum, newCnlData);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex, Locale.IsRussian ?
                                "Ошибка при записи исторических данных в архив {0}" :
                                "Error writing historical data to the {1} archive", archiveLogic.Code);
                        }
                        finally
                        {
                            archiveHolder.EndUpdate(archiveLogic, updateContext);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при записи исторических данных" :
                    "Error writing historical data");
            }
            finally
            {
                // in case of archive error, slice data may be inconsistent
                moduleHolder.OnHistoricalDataProcessed(slice);
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        public void WriteEvent(int archiveMask, Event ev)
        {
            if (ev == null)
                throw new ArgumentNullException(nameof(ev));

            try
            {
                if (ev.EventID <= 0)
                    ev.EventID = ScadaUtils.GenerateUniqueID(ev.Timestamp);

                Cnl cnl = null;

                if (ev.CnlNum > 0)
                {
                    if (cnlTags.TryGetValue(ev.CnlNum, out CnlTag cnlTag))
                        cnl = cnlTag.Cnl;
                    else if (outCnlTags.TryGetValue(ev.CnlNum, out OutCnlTag outCnlTag))
                        cnl = outCnlTag.Cnl;
                }

                // set missing event properties
                if (cnl != null)
                {
                    if (ev.ObjNum <= 0)
                        ev.ObjNum = cnl.ObjNum ?? 0;

                    if (ev.DeviceNum <= 0)
                        ev.DeviceNum = cnl.DeviceNum ?? 0;

                    if (ConfigDatabase.CnlStatusTable.GetItem(ev.CnlStat) is CnlStatus cnlStatus)
                    {
                        if (ev.Severity <= 0)
                            ev.Severity = cnlStatus.Severity ?? 0;

                        if (!ev.AckRequired)
                            ev.AckRequired = cnlStatus.AckRequired;
                    }
                }

                // fix channel values
                if (double.IsNaN(ev.PrevCnlVal) || double.IsInfinity(ev.PrevCnlVal))
                {
                    ev.PrevCnlVal = 0.0;
                    ev.PrevCnlStat = 0;
                }

                if (double.IsNaN(ev.CnlVal) || double.IsInfinity(ev.CnlVal))
                {
                    ev.CnlVal = 0.0;
                    ev.CnlStat = 0;
                }

                EnqueueEvent(archiveMask, ev);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при записи события" :
                    "Error writing event");
            }
        }

        /// <summary>
        /// Acknowledges the event using the default arguments.
        /// </summary>
        public void AckEvent(EventAck eventAck)
        {
            AckEvent(eventAck, true, AppConfig.GeneralOptions.GenerateAckCmd);
        }

        /// <summary>
        /// Acknowledges the event.
        /// </summary>
        public void AckEvent(EventAck eventAck, bool useModules, bool generateAckCmd)
        {
            if (eventAck == null)
                throw new ArgumentNullException(nameof(eventAck));

            Log.WriteAction(Locale.IsRussian ?
                "Квитирование события с ид. {0}" :
                "Acknowledge event with ID {0}", eventAck.EventID);

            if (useModules)
                moduleHolder.OnEventAck(eventAck);

            archiveHolder.AckEvent(eventAck);

            if (generateAckCmd)
            {
                listener.EnqueueCommand(new TeleCommand
                {
                    CommandID = ScadaUtils.GenerateUniqueID(eventAck.Timestamp),
                    CreationTime = eventAck.Timestamp,
                    UserID = eventAck.UserID,
                    CmdCode = ServerCmdCode.AckEvent,
                    CmdVal = BitConverter.Int64BitsToDouble(eventAck.EventID)
                }, true);
            }
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public CommandResult SendCommand(TeleCommand command, WriteCommandFlags flags)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            CommandResult result = new CommandResult(false);

            try
            {
                int cnlNum = command.CnlNum;
                int userID = command.UserID;
                Log.WriteAction(Locale.IsRussian ?
                    "Команда на канал {0} от пользователя с ид. {1}" :
                    "Command to channel {0} from user with ID {1}", cnlNum, userID);
                User user = FindUser(userID);

                if (user == null)
                {
                    result.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Пользователь {0} не найден" :
                        "User {0} not found", userID);
                    Log.WriteError(result.ErrorMessage);
                }
                else if (!user.Enabled)
                {
                    result.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Пользователь {0} отключен" :
                        "User {0} is disabled", userID);
                    Log.WriteError(result.ErrorMessage);
                }
                else if (!outCnlTags.TryGetValue(cnlNum, out OutCnlTag outCnlTag))
                {
                    result.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Канал {0} не найден среди выходных каналов" :
                        "Channel {0} not found among output channels", cnlNum);
                    Log.WriteError(result.ErrorMessage);
                }
                else
                {
                    Cnl outCnl = outCnlTag.Cnl;
                    int objNum = outCnl.ObjNum ?? 0;

                    if (!rightMatrix.GetRight(user.RoleID, objNum).Control)
                    {
                        result.ErrorMessage = string.Format(Locale.IsRussian ?
                            "Недостаточно прав пользователя с ролью {0} на управление объектом {1}" :
                            "Insufficient rights of user with role {0} to control object {1}", user.RoleID, objNum);
                        Log.WriteError(result.ErrorMessage);
                    }
                    else
                    {
                        DateTime utcNow = DateTime.UtcNow;
                        command.CommandID = ScadaUtils.GenerateUniqueID(utcNow);
                        command.CreationTime = utcNow;
                        command.ObjNum = objNum;
                        command.DeviceNum = outCnl.DeviceNum ?? 0;
                        command.CmdNum = outCnl.TagNum ?? 0;
                        command.CmdCode = outCnl.TagCode;

                        lock (curData)
                        {
                            curData.Timestamp = command.CreationTime;

                            if (!flags.HasFlag(WriteCommandFlags.ApplyFormulas))
                            {
                                result.IsSuccessful = true;
                            }
                            else if (calc.CalcCmdData(curData, outCnlTag, command.CmdVal, command.CmdData,
                                out double cmdVal, out byte[] cmdData, out string errMsg))
                            {
                                result.IsSuccessful = true;
                                command.CmdVal = cmdVal;
                                command.CmdData = cmdData;
                            }
                            else
                            {
                                result.ErrorMessage = errMsg;
                            }
                        }

                        EnqueueCommand(user, outCnlTag, command, flags, result);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при отправке команды" :
                    "Error sending command");
            }

            return result;
        }
    }
}
