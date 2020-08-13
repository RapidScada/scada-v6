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

        private ServerListener listener;         // the TCP listener
        private ModuleHolder moduleHolder;       // holds modules
        private ArchiveHolder archiveHolder;     // holds archives
        private BaseDataSet baseDataSet;         // the configuration database
        private Dictionary<int, CnlTag> cnlTags; // the metadata about the input channels accessed by channel numbers
        private Dictionary<string, User> users;  // the users accessed by names
        private Calculator calc;                 // provides work with scripts and formulas
        private CurrentData curData;             // the current data of the input channels
        private ServerCache serverCache;         // the server level cache


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
            users = null;
            calc = null;
            curData = null;
            serverCache = null;
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
            InitUsers();

            if (!InitCalculator())
                return false;

            curData = new CurrentData(this, cnlTags);
            serverCache = new ServerCache();
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
            List<CnlTag> limTags = new List<CnlTag>();
            int cnlNum = 0;
            int index = 0;

            foreach (InCnl inCnl in baseDataSet.InCnlTable.EnumerateItems())
            {
                if (inCnl.Active && inCnl.CnlNum >= cnlNum)
                {
                    cnlNum = inCnl.CnlNum;
                    Lim lim = inCnl.LimID.HasValue ? baseDataSet.LimTable.GetItem(inCnl.LimID.Value) : null;
                    CnlTag cnlTag = new CnlTag(index++, cnlNum, inCnl, lim);
                    cnlTags.Add(cnlNum++, cnlTag);

                    if (lim != null && lim.IsBoundToCnl)
                        limTags.Add(cnlTag);

                    // add channel tags if one channel row defines multiple channels
                    if (inCnl.DataLen > 1)
                    {
                        for (int i = 1, cnt = inCnl.DataLen.Value; i < cnt; i++)
                        {
                            cnlTag = new CnlTag(index++, cnlNum, inCnl, lim);
                            cnlTags.Add(cnlNum++, cnlTag);

                            if (lim != null && lim.IsBoundToCnl)
                                limTags.Add(cnlTag);
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
        /// Initializes the calculator.
        /// </summary>
        private bool InitCalculator()
        {
            calc = new Calculator(appDirs, log);
            return calc.CompileScripts(baseDataSet, cnlTags);
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
                calc.InitScripts();

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
                calc.FinalizeScripts();
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
        /// Updates the input channel status after formula calculation.
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
        /// Get the input channel limits.
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
        /// Generates an event and adds it to the event queue.
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

                    Event ev = new Event
                    {
                        EventID = ScadaUtils.GenerateUniqueID(),
                        Timestamp = DateTime.UtcNow,
                        Hidden = false,
                        CnlNum = cnlTag.CnlNum,
                        OutCnlNum = 0,
                        ObjNum = inCnl.ObjNum ?? 0,
                        DeviceNum = inCnl.DeviceNum ?? 0,
                        PrevCnlVal = prevCnlData.Val,
                        PrevCnlStat = prevStat,
                        CnlVal = cnlData.Val,
                        CnlStat = cnlStat,
                        Severity = cnlStatus?.Severity ?? 0,
                        AckRequired = cnlStatus?.AckRequired ?? false,
                        Ack = false,
                        AckTimestamp = DateTime.MinValue,
                        AckUserID = 0,
                        TextFormat = EventTextFormat.Full,
                        Text = "",
                        Data = null
                    };

                    // TODO: write event to archives
                    log.WriteAction(string.Format("Created event with ID = {0}, CnlNum = {1}", ev.EventID, ev.CnlNum));
                }
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
        /// Validates the username and password.
        /// </summary>
        public bool ValidateUser(string username, string password, out int roleID, out string errMsg)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    roleID = RoleID.Disabled;
                    errMsg = Locale.IsRussian ?
                        "Имя пользователя или пароль не может быть пустым" :
                        "Username or password can not be empty";
                    return false;
                }

                if (users.TryGetValue(username.ToLowerInvariant(), out User user) &&
                    user.Password == ScadaUtils.GetPasswordHash(user.UserID, password))
                {
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
                    roleID = RoleID.Disabled;
                    errMsg = Locale.IsRussian ?
                        "Неверное имя пользователя или пароль" :
                        "Invalid username or password";
                    return false;
                }
            }
            catch (Exception ex)
            {
                roleID = RoleID.Disabled;
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
                    cnlListID = serverCache.GetNextCnlListID();
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
                calc.BeginCalculation(curData);

                lock (curData)
                {
                    DateTime utcNow = DateTime.UtcNow;

                    for (int i = 0, cnlCnt = cnlNums.Length; i < cnlCnt; i++)
                    {
                        if (cnlTags.TryGetValue(cnlNums[i], out CnlTag cnlTag))
                        {
                            CnlData newCnlData = calc.CalcCnlData(cnlTag, cnlData[i]);
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
            }
        }

        /// <summary>
        /// Sends the telecontrol command.
        /// </summary>
        public void SendCommand(TeleCommand command, out CommandResult commandResult)
        {
            commandResult = new CommandResult();
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
