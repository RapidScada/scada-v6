/*
 * Copyright 2022 Rapid Software LLC
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
 * Modified : 2022
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
    internal class CoreLogic : ICnlDataChangeHandler
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
            public OutCnlTag OutCnlTag { get; set; }
            public TeleCommand Command { get; set; }
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

        private Dictionary<int, CnlTag> cnlTags;       // the channel tags for archiving
        private Dictionary<int, OutCnlTag> outCnlTags; // the output channel tags for sending commands
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
        private Queue<EventItem> events;         // the just created events
        private Queue<CommandItem> commands;     // the preprocessed commands that have not yet been sent


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
            events = null;
            commands = null;
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
        /// Gets the active channel numbers for archiving.
        /// </summary>
        public int[] CnlNums => curData?.CnlNums;

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

            InitCnlTags();
            InitUsers();
            InitRightMatrix();

            if (!InitCalculator())
                return false;

            SharedData = new ConcurrentDictionary<string, object>();
            moduleHolder = new ModuleHolder(Log);
            archiveHolder = new ArchiveHolder(Log);
            serverCache = new ServerCache();
            listener = new ServerListener(this, archiveHolder, serverCache);
            curData = new CurrentData(this, cnlTags);
            events = new Queue<EventItem>();
            commands = new Queue<CommandItem>();

            InitModules();
            InitArchives();
            return true;
        }

        /// <summary>
        /// Reads the configuration database from the storage.
        /// </summary>
        private bool ReadConfigDatabase()
        {
            string tableName = Locale.IsRussian ? "неопределена" : "undefined";

            try
            {
                ConfigDatabase = new ConfigDatabase();

                foreach (IBaseTable baseTable in ConfigDatabase.AllTables)
                {
                    Storage.ReadBaseTable(baseTable);
                }

                Log.WriteAction(Locale.IsRussian ?
                    "База конфигурации считана успешно" :
                    "The configuration database has been read successfully");
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при чтении базы конфигурации, таблица {0}" :
                    "Error reading the configuration database, the {0} table", tableName);
                return false;
            }
        }

        /// <summary>
        /// Initializes metadata about the channels.
        /// </summary>
        private void InitCnlTags()
        {
            // create channel tags
            cnlTags = new Dictionary<int, CnlTag>();
            outCnlTags = new Dictionary<int, OutCnlTag>();
            measCnlTags = new List<CnlTag>();
            calcCnlTags = new List<CnlTag>();
            List<CnlTag> limTags = new List<CnlTag>();
            int index = 0;
            int cnlNum = 0;

            // add new channel tag to collections
            void AddCnlTag(Cnl cnl, Lim lim)
            {
                if (cnl.IsArchivable())
                {
                    CnlTag cnlTag = new CnlTag(index++, cnlNum, cnl, lim);
                    cnlTags.Add(cnlNum++, cnlTag);

                    if (cnl.IsInput())
                        measCnlTags.Add(cnlTag);
                    else if (cnl.IsCalculated())
                        calcCnlTags.Add(cnlTag);

                    if (lim != null && lim.IsBoundToCnl)
                        limTags.Add(cnlTag);
                }
            }

            // add new output channel tag
            void AddOutCnlTag(Cnl cnl)
            {
                if (cnl.IsOutput())
                    outCnlTags.Add(cnl.CnlNum, new OutCnlTag(cnl));
            }

            foreach (Cnl cnl in ConfigDatabase.CnlTable.EnumerateItems())
            {
                if (cnl.Active && cnl.CnlNum >= cnlNum)
                {
                    cnlNum = cnl.CnlNum;
                    Lim lim = cnl.LimID.HasValue ? ConfigDatabase.LimTable.GetItem(cnl.LimID.Value) : null;
                    AddCnlTag(cnl, lim);
                    AddOutCnlTag(cnl);

                    // add channel tags if one channel row defines multiple channels
                    if (cnl.IsArray())
                    {
                        for (int i = 1, len = cnl.DataLen.Value; i < len; i++)
                        {
                            AddCnlTag(cnl, lim);
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

            foreach (User user in ConfigDatabase.UserTable.EnumerateItems())
            {
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
            calc = new Calculator(AppDirs, Log);
            return calc.CompileScripts(ConfigDatabase, cnlTags, outCnlTags);
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

            foreach (Archive archive in ConfigDatabase.ArchiveTable.EnumerateItems())
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
        /// Gets the channel numbers processed by the archive.
        /// </summary>
        private int[] GetProcessedCnls(Archive archive)
        {
            List<int> cnlNums = new List<int>(cnlTags.Count);
            bool isDefault = archive.IsDefault;
            int archiveBit = archive.Bit;

            foreach (CnlTag cnlTag in cnlTags.Values)
            {
                int? archiveMask = cnlTag.Cnl.ArchiveMask;

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
                            curData.PrepareIteration(utcNow);

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
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex, CommonPhrases.LogicCycleError);
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
                        curData.SetCurCnlData(cnlTag, ref unrelCnlData, nowDT);
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
            try
            {
                calc.BeginCalculation(curData);

                foreach (CnlTag cnlTag in calcCnlTags)
                {
                    CnlData newCnlData = calc.CalcCnlData(cnlTag, CnlData.Zero);
                    curData.SetCurCnlData(cnlTag, ref newCnlData, nowDT);
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
            while (true)
            {
                EventItem eventItem;
                Event ev;

                lock (events)
                {
                    if (events.Count > 0)
                    {
                        eventItem = events.Dequeue();
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

                lock (commands)
                {
                    if (commands.Count > 0)
                        commandItem = commands.Dequeue();
                    else
                        break;
                }

                if (commandItem.Result.IsSuccessful)
                    moduleHolder.OnCommand(commandItem.Command, commandItem.Result);

                if (commandItem.Result.IsSuccessful)
                {
                    GenerateEvent(commandItem.OutCnlTag, commandItem.Command);

                    if (commandItem.Result.TransmitToClients)
                    {
                        listener.EnqueueCommand(commandItem.Command);
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
        /// Updates the channel status after formula calculation for the current data.
        /// </summary>
        private void UpdateCnlStatus(CnlTag cnlTag, ref CnlData cnlData, CnlData prevCnlData)
        {
            if (double.IsNaN(cnlData.Val))
            {
                // set undefined status if value is not a number
                cnlData = CnlData.Empty;
            }
            else if (cnlData.Stat == CnlStatusID.Defined && cnlTag.Lim != null)
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
        private void GenerateEvent(OutCnlTag outCnlTag, TeleCommand command)
        {
            EventMask eventMask = new EventMask(outCnlTag.Cnl.EventMask);

            if (eventMask.Enabled && eventMask.Command)
            {
                DateTime utcNow = DateTime.UtcNow;
                string userName = ConfigDatabase.UserTable.GetItem(command.UserID)?.Name;

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
                    Text = string.Format(ServerPhrases.CommandSentBy, userName),
                    Data = command.CmdData
                });
            }
        }

        /// <summary>
        /// Adds the event to the queue.
        /// </summary>
        private void EnqueueEvent(int archiveMask, Event ev)
        {
            lock (events)
            {
                events.Enqueue(new EventItem { ArchiveMask = archiveMask, Event = ev });
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

                    if (thread.Join(ScadaUtils.ThreadWait))
                    {
                        Log.WriteAction(CommonPhrases.LogicStopped);
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
                Log.WriteError(ex, CommonPhrases.StopLogicError);
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

                Log.WriteError(ex, errMsg);
                return false;
            }
        }

        /// <summary>
        /// Gets the current data of the specified channel.
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
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при получении текущих данных канала" :
                    "Error getting current data of the channel");
            }

            return CnlData.Empty;
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
        public void WriteCurrentData(int[] cnlNums, CnlData[] cnlData, int deviceNum, WriteFlags writeFlags)
        {
            if (cnlNums == null)
                throw new ArgumentNullException(nameof(cnlNums));
            if (cnlData == null)
                throw new ArgumentNullException(nameof(cnlData));

            try
            {
                moduleHolder.OnCurrentDataProcessing(cnlNums, cnlData, deviceNum);
                Monitor.Enter(curData);
                calc.BeginCalculation(curData);

                DateTime utcNow = DateTime.UtcNow;
                curData.Timestamp = utcNow;
                bool applyFormulas = writeFlags.HasFlag(WriteFlags.ApplyFormulas);
                bool enableEvents = writeFlags.HasFlag(WriteFlags.EnableEvents);

                for (int i = 0, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
                {
                    if (cnlTags.TryGetValue(cnlNums[i], out CnlTag cnlTag))
                    {
                        if (applyFormulas && cnlTag.Cnl.FormulaEnabled && cnlTag.Cnl.IsInput())
                        {
                            CnlData newCnlData = calc.CalcCnlData(cnlTag, cnlData[i]);
                            curData.SetCurCnlData(cnlTag, ref newCnlData, utcNow, enableEvents);
                            cnlData[i] = newCnlData;
                        }
                        else
                        {
                            curData.SetCurCnlData(cnlTag, ref cnlData[i], utcNow, enableEvents);
                        }
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
                calc.EndCalculation();
                Monitor.Exit(curData);
                moduleHolder.OnCurrentDataProcessed(cnlNums, cnlData, deviceNum);
            }
        }

        /// <summary>
        /// Writes the historical data.
        /// </summary>
        public void WriteHistoricalData(int archiveMask, Slice slice, int deviceNum, WriteFlags writeFlags)
        {
            if (slice == null)
                throw new ArgumentNullException(nameof(slice));

            try
            {
                moduleHolder.OnHistoricalDataProcessing(slice, deviceNum);
                DateTime timestamp = slice.Timestamp;

                if (archiveMask == ArchiveMask.Default)
                    archiveMask = archiveHolder.DefaultArchiveMask;

                // find channel tags of the slice
                int cnlCnt = slice.CnlNums.Length;
                CnlTag[] sliceCnlTags = new CnlTag[cnlCnt];

                for (int i = 0; i < cnlCnt; i++)
                {
                    cnlTags.TryGetValue(slice.CnlNums[i], out CnlTag cnlTag);
                    sliceCnlTags[i] = cnlTag;
                }

                // write to archives
                bool applyFormulas = writeFlags.HasFlag(WriteFlags.ApplyFormulas);

                for (int archiveBit = 0; archiveBit < ServerUtils.MaxArchiveCount; archiveBit++)
                {
                    if (archiveMask.BitIsSet(archiveBit) && 
                        archiveHolder.GetArchive(archiveBit, out HistoricalArchiveLogic archiveLogic) &&
                        archiveLogic.AcceptData(ref timestamp))
                    {
                        try
                        {
                            archiveLogic.Lock();
                            archiveLogic.BeginUpdate(timestamp, deviceNum);
                            calc.BeginCalculation(new ArchiveCalcContext(archiveLogic, timestamp));

                            // calculate written channels
                            for (int i = 0; i < cnlCnt; i++)
                            {
                                CnlTag cnlTag = sliceCnlTags[i];

                                if (cnlTag != null)
                                {
                                    CnlData newCnlData = slice.CnlData[i];

                                    if (applyFormulas && cnlTag.Cnl.FormulaEnabled && cnlTag.Cnl.IsInput())
                                    {
                                        newCnlData = calc.CalcCnlData(cnlTag, newCnlData);
                                        UpdateCnlStatus(archiveLogic, timestamp, cnlTag, ref newCnlData);
                                        slice.CnlData[i] = newCnlData;
                                    }

                                    archiveLogic.WriteCnlData(timestamp, cnlTag.CnlNum, newCnlData);
                                }
                            }

                            // calculate channels of the calculated type
                            foreach (CnlTag cnlTag in calcCnlTags)
                            {
                                CnlData arcCnlData = archiveLogic.GetCnlData(timestamp, cnlTag.CnlNum);
                                CnlData newCnlData = calc.CalcCnlData(cnlTag, arcCnlData);
                                UpdateCnlStatus(archiveLogic, timestamp, cnlTag, ref newCnlData);

                                if (arcCnlData != newCnlData)
                                    archiveLogic.WriteCnlData(timestamp, cnlTag.CnlNum, newCnlData);
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
                            calc.EndCalculation();
                            archiveHolder.EndUpdate(archiveLogic, timestamp, deviceNum);
                            archiveHolder.Unlock(archiveLogic);
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
                moduleHolder.OnHistoricalDataProcessed(slice, deviceNum);
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
                int cnlNum = command.CnlNum;
                int userID = command.UserID;
                Log.WriteAction(Locale.IsRussian ?
                    "Команда на канал {0} от пользователя с ид. {1}" :
                    "Command to channel {0} from user with ID {1}", cnlNum, userID);

                if (!ConfigDatabase.UserTable.Items.TryGetValue(userID, out User user))
                {
                    commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Пользователь {0} не найден" :
                        "User {0} not found", userID);
                    Log.WriteError(commandResult.ErrorMessage);
                }
                else if (!outCnlTags.TryGetValue(cnlNum, out OutCnlTag outCnlTag))
                {
                    commandResult.ErrorMessage = string.Format(Locale.IsRussian ?
                        "Канал {0} не найден среди выходных каналов" :
                        "Channel {0} not found among output channels", cnlNum);
                    Log.WriteError(commandResult.ErrorMessage);
                }
                else
                {
                    Cnl outCnl = outCnlTag.Cnl;
                    int objNum = outCnl.ObjNum ?? 0;

                    if (!rightMatrix.GetRight(user.RoleID, objNum).Control)
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
                        command.ObjNum = objNum;
                        command.DeviceNum = outCnl.DeviceNum ?? 0;
                        command.CmdNum = outCnl.TagNum ?? 0;
                        command.CmdCode = outCnl.TagCode;

                        try
                        {
                            Monitor.Enter(curData);
                            calc.BeginCalculation(curData);
                            curData.Timestamp = command.CreationTime;

                            if (calc.CalcCmdData(outCnlTag, command.CmdVal, command.CmdData, 
                                out double cmdVal, out byte[] cmdData, out string errMsg))
                            {
                                commandResult.IsSuccessful = true;
                                command.CmdVal = cmdVal;
                                command.CmdData = cmdData;
                            }
                            else
                            {
                                commandResult.ErrorMessage = errMsg;
                            }
                        }
                        finally
                        {
                            calc.EndCalculation();
                            Monitor.Exit(curData);
                        }

                        lock (commands)
                        {
                            commands.Enqueue(new CommandItem
                            {
                                OutCnlTag = outCnlTag,
                                Command = command,
                                Result = commandResult
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult.ErrorMessage = ex.Message;
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при отправке команды" :
                    "Error sending command");
            }
        }

        /// <summary>
        /// Handles the changes of the current channel data.
        /// </summary>
        void ICnlDataChangeHandler.HandleCurDataChanged(CnlTag cnlTag, ref CnlData cnlData, 
            CnlData prevCnlData, CnlData prevCnlDataDef, bool enableEvents)
        {
            UpdateCnlStatus(cnlTag, ref cnlData, prevCnlData);

            if (enableEvents)
                GenerateEvent(cnlTag, cnlData, prevCnlData, prevCnlDataDef);
        }
    }
}
