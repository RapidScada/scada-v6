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
 * Module   : ScadaCommEngine
 * Summary  : Represents a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2022
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a communication line.
    /// <para>Представляет линию связи.</para>
    /// </summary>
    internal class CommLine : ILineContext
    {
        /// <summary>
        /// The period of attempts to start the communication channel.
        /// </summary>
        private static readonly TimeSpan StartChannelPeriod = TimeSpan.FromSeconds(10);
        /// <summary>
        /// The minimum delay after polling cycle, ms.
        /// </summary>
        private const int MinCycleDelay = 50;
        /// <summary>
        /// The delay after empty polling cycle, ms.
        /// </summary>
        private const int EmptyCycleDelay = 200;

        private readonly CoreLogic coreLogic;         // the Communicator logic instance
        private readonly string infoFileName;         // the full file name to write communication line information
        private readonly List<DeviceWrapper> devices; // the devices to poll
        private readonly Dictionary<int, DeviceWrapper> deviceMap;        // the devices accessed by device number
        private readonly Dictionary<int, DeviceLogic> deviceByNumAddr;    // the devices accessed by numeric address
        private readonly Dictionary<string, DeviceLogic> deviceByStrAddr; // the devices accessed by string address
        private readonly Queue<TeleCommand> commands;       // the command queue
        private readonly Queue<DeviceWrapper> priorityPoll; // the priority poll queue

        private Thread thread;                     // the working thread of the communication line
        private volatile bool terminated;          // necessary to stop the thread
        private volatile ServiceStatus lineStatus; // the current communication line status
        private int lastInfoLength;                // the last info text length
        private int maxDeviceTitleLength;          // the maximum length of device title
        private ChannelWrapper channel;            // the communication channel


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CommLine(LineConfig lineConfig, CoreLogic coreLogic)
        {
            LineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            infoFileName = Path.Combine(coreLogic.AppDirs.LogDir, CommUtils.GetLineLogFileName(CommLineNum, ".txt"));
            devices = new List<DeviceWrapper>();
            deviceMap = new Dictionary<int, DeviceWrapper>();
            deviceByNumAddr = new Dictionary<int, DeviceLogic>();
            deviceByStrAddr = new Dictionary<string, DeviceLogic>();
            commands = new Queue<TeleCommand>();
            priorityPoll = new Queue<DeviceWrapper>();

            thread = null;
            terminated = false;
            lineStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;
            maxDeviceTitleLength = 0;
            channel = null;

            Title = CommUtils.GetLineTitle(lineConfig);
            SharedData = null;
            Log = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(coreLogic.AppDirs.LogDir, CommUtils.GetLineLogFileName(CommLineNum, ".log")),
                CapacityMB = coreLogic.AppConfig.GeneralOptions.MaxLogSize
            };
        }


        /// <summary>
        /// Gets the communication line configuration.
        /// </summary>
        public LineConfig LineConfig { get; }

        /// <summary>
        /// Gets the communication line number.
        /// </summary>
        public int CommLineNum
        {
            get
            {
                return LineConfig.CommLineNum;
            }
        }

        /// <summary>
        /// Gets the communication line title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Gets the shared data of the communication line.
        /// </summary>
        public IDictionary<string, object> SharedData { get; private set; }

        /// <summary>
        /// Get the communication channel.
        /// </summary>
        public ChannelLogic Channel
        {
            get
            {
                return channel?.ChannelLogic;
            }
        }

        /// <summary>
        /// Gets the current communication line status.
        /// </summary>
        public ServiceStatus LineStatus
        {
            get
            {
                return lineStatus;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the communication line is completely terminated.
        /// </summary>
        public bool IsTerminated
        {
            get
            {
                return lineStatus == ServiceStatus.Terminated;
            }
        }


        /// <summary>
        /// Adds the device to the communication line, eliminating duplication of devices.
        /// </summary>
        private void AddDevice(DeviceLogic deviceLogic)
        {
            if (!deviceMap.ContainsKey(deviceLogic.DeviceNum))
            {
                DeviceWrapper deviceWrapper = new DeviceWrapper(deviceLogic, Log)
                {
                    DeviceIndex = devices.Count,
                    InfoFileName = Path.Combine(coreLogic.AppDirs.LogDir,
                        CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, ".txt"))
                };

                devices.Add(deviceWrapper);
                deviceMap.Add(deviceLogic.DeviceNum, deviceWrapper);

                if (deviceLogic.NumAddress > 0)
                    deviceByNumAddr[deviceLogic.NumAddress] = deviceLogic;

                if (!string.IsNullOrEmpty(deviceLogic.StrAddress))
                    deviceByStrAddr[deviceLogic.StrAddress] = deviceLogic;

                if (maxDeviceTitleLength < deviceLogic.Title.Length)
                    maxDeviceTitleLength = deviceLogic.Title.Length;
            }
        }

        /// <summary>
        /// Prepares the communication line for start.
        /// </summary>
        private bool Prepare(out string errMsg)
        {
            terminated = false;
            lineStatus = ServiceStatus.Starting;
            SharedData = new ConcurrentDictionary<string, object>();
            WriteInfo();
            WriteDeviceInfo();

            if (devices.Count > 0)
            {
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = Locale.IsRussian ?
                    "Работа линии связи невозможна из-за отсутствия устройств" :
                    "Communication line execution is impossible due to lack of devices";
                return false;
            }
        }

        /// <summary>
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                StartChannel();

                foreach (DeviceWrapper deviceWrapper in devices)
                {
                    deviceWrapper.OnCommLineStart();
                    deviceWrapper.InitDeviceTags();

                    if (LineConfig.IsBound)
                        deviceWrapper.BindDeviceTags(coreLogic.BaseDataSet);

                    deviceWrapper.InitDeviceData();
                }

                if (!LineConfig.LineOptions.DetailedLog)
                {
                    Log.WriteInfo(Locale.IsRussian ?
                        "Детальный журнал отключен" :
                        "Detailed log is disabled");
                }

                lineStatus = ServiceStatus.Normal;
                LineCycle();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, CommonPhrases.ThreadFatalError);
            }
            finally
            {
                devices.ForEach(d => d.OnCommLineTerminate());
                channel.Stop();
                lineStatus = ServiceStatus.Terminated;
                WriteInfo();
                WriteDeviceInfo();

                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Линия связи {0} остановлена" :
                    "Communication line {0} is stopped", Title);
                Log.WriteBreak();
            }
        }

        /// <summary>
        /// Starts the communication channel.
        /// </summary>
        private void StartChannel()
        {
            DateTime attempDT = DateTime.MinValue;

            while (!terminated)
            {
                DateTime utcNow = DateTime.UtcNow;

                if (utcNow - attempDT >= StartChannelPeriod)
                {
                    attempDT = utcNow;
                    if (channel.Start())
                        break;
                }

                Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Work cycle of the communication line.
        /// </summary>
        private void LineCycle()
        {
            int cycleDelay = Math.Max(MinCycleDelay, LineConfig.LineOptions.CycleDelay);
            int deviceCount = devices.Count;
            int deviceIndex = 0;
            int requiredSessionCnt = 0;
            int actualSessionCnt = 0;
            bool skipUnableMsg = false;
            TimeSpan sendAllPeriod = TimeSpan.FromSeconds(coreLogic.AppConfig.GeneralOptions.SendAllDataPeriod);
            bool sendAllAlways = !coreLogic.AppConfig.GeneralOptions.SendModifiedData;
            bool sendAllWithPeriod = sendAllPeriod > TimeSpan.Zero;
            DateTime[] sendAllDT = new DateTime[deviceCount];
            DateTime writeInfoDT = DateTime.MinValue;

            while (!terminated)
            {
                try
                {
                    DateTime utcNow = DateTime.UtcNow;
                    DeviceWrapper deviceWrapper;
                    DeviceLogic deviceLogic;

                    // commands
                    while (!terminated && DequeueCommand(utcNow, out TeleCommand cmd, out deviceWrapper))
                    {
                        deviceLogic = deviceWrapper.DeviceLogic;
                        channel.BeforeSession(deviceLogic);

                        if (!deviceLogic.ConnectionRequired ||
                            deviceLogic.Connection != null && deviceLogic.Connection.Connected)
                        {
                            deviceWrapper.SendCommand(cmd);

                            if (LineConfig.LineOptions.PollAfterCmd)
                                PollWithPriority(deviceWrapper);
                        }
                        else
                        {
                            Log.WriteLine();
                            Log.WriteAction(string.Format(Locale.IsRussian ?
                                "Невозможно отправить команду устройству {0}, т.к. соединение не установлено" :
                                "Unable to send command to the device {0} because connection is not established",
                                deviceLogic.Title));
                        }

                        channel.AfterSession(deviceLogic);
                    }

                    // session
                    deviceWrapper = SelectDevice(ref deviceIndex, out bool hasPriority);
                    deviceLogic = deviceWrapper.DeviceLogic;
                    bool sendAll = false;

                    if (sendAllAlways)
                    {
                        sendAll = true;
                    }
                    else if (sendAllWithPeriod && (utcNow - sendAllDT[deviceWrapper.DeviceIndex] >= sendAllPeriod))
                    {
                        sendAll = true;
                        sendAllDT[deviceWrapper.DeviceIndex] = utcNow;
                    }

                    if (hasPriority || CheckSessionIsRequired(deviceLogic, utcNow))
                    {
                        requiredSessionCnt++;
                        channel.BeforeSession(deviceLogic);

                        if (!deviceLogic.ConnectionRequired ||
                            deviceLogic.Connection != null && deviceLogic.Connection.Connected)
                        {
                            deviceWrapper.Session();
                            actualSessionCnt++;
                        }
                        else
                        {
                            deviceWrapper.InvalidateData();

                            if (!skipUnableMsg)
                            {
                                Log.WriteLine();
                                Log.WriteAction(string.Format(Locale.IsRussian ?
                                    "Невозможно выполнить сеанс связи с устройством {0}, т.к. соединение не установлено" :
                                    "Unable to communicate with the device {0} because connection is not established",
                                    deviceLogic.Title));
                            }
                        }

                        channel.AfterSession(deviceLogic);
                        TransferDeviceData(deviceLogic, sendAll);
                    }
                    else if (sendAll)
                    {
                        TransferDeviceData(deviceLogic, true);
                    }

                    // write line and devices info
                    if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                    {
                        writeInfoDT = utcNow;
                        WriteInfo();
                        WriteDeviceInfo();
                    }

                    if (deviceIndex >= deviceCount)
                    {
                        // line cycle ended
                        if (actualSessionCnt > 0)
                        {
                            skipUnableMsg = false;
                            Thread.Sleep(cycleDelay);
                        }
                        else
                        {
                            if (requiredSessionCnt > 0)
                                skipUnableMsg = true;
                            Thread.Sleep(EmptyCycleDelay);
                        }

                        deviceIndex = 0;
                        requiredSessionCnt = 0;
                        actualSessionCnt = 0;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка в цикле работы линии связи" :
                        "Error in the communication line work cycle");
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }

        /// <summary>
        /// Removes, validates and returns the command at the beginning of the queue.
        /// </summary>
        private bool DequeueCommand(DateTime utcNow, out TeleCommand cmd, out DeviceWrapper deviceWrapper)
        {
            bool queueIsNotEmpty = true;
            bool commandIsValid = false;
            cmd = null;
            deviceWrapper = null;

            while (queueIsNotEmpty && !commandIsValid)
            {
                lock (commands)
                {
                    queueIsNotEmpty = commands.Count > 0;
                    cmd = queueIsNotEmpty ? commands.Dequeue() : null;
                }

                if (cmd != null)
                {
                    if (utcNow - cmd.CreationTime > ScadaUtils.CommandLifetime)
                    {
                        Log.WriteLine();
                        Log.WriteError(Locale.IsRussian ?
                            "Устаревшая команда для устройства {0} отклонена" :
                            "Outdated command to the device {0} is rejected", cmd.DeviceNum);
                    }
                    else if (!deviceMap.TryGetValue(cmd.DeviceNum, out deviceWrapper))
                    {
                        Log.WriteLine();
                        Log.WriteError(Locale.IsRussian ?
                            "Команда с недопустимым устройством {0}, отклонена" :
                            "Command with invalid device {0} is rejected", cmd.DeviceNum);
                    } 
                    else if (!deviceWrapper.DeviceLogic.CanSendCommands)
                    {
                        Log.WriteLine();
                        Log.WriteError(Locale.IsRussian ?
                            "Устройство {0} не поддерживает отправку команд" :
                            "The device {0} does not support sending commands", cmd.DeviceNum);
                    }
                    else
                    {
                        commandIsValid = true;
                    }
                }
            }

            if (commandIsValid)
            {
                return true;
            }
            else
            {
                cmd = null;
                deviceWrapper = null;
                return false;
            }
        }

        /// <summary>
        /// Schedules a priority poll the device.
        /// </summary>
        private void PollWithPriority(DeviceWrapper deviceWrapper)
        {
            lock (priorityPoll)
            {
                // check if the device poll is already enqueued
                int deviceNum = deviceWrapper.DeviceLogic.DeviceNum;
                bool deviceFound = false;

                foreach (DeviceWrapper d in priorityPoll)
                {
                    if (d.DeviceLogic.DeviceNum == deviceNum)
                    {
                        deviceFound = true;
                        break;
                    }
                }

                // enqueue a poll
                if (!deviceFound)
                    priorityPoll.Enqueue(deviceWrapper);
            }
        }

        /// <summary>
        /// Selects a device to poll.
        /// </summary>
        private DeviceWrapper SelectDevice(ref int deviceIndex, out bool hasPriority)
        {
            lock (priorityPoll)
            {
                hasPriority = priorityPoll.Count > 0;
                return hasPriority
                    ? priorityPoll.Dequeue()
                    : devices[deviceIndex++];
            }
        }

        /// <summary>
        /// Checks if a device polling session of the device is required.
        /// </summary>
        private bool CheckSessionIsRequired(DeviceLogic deviceLogic, DateTime utcNow)
        {
            PollingOptions pollingOptions = deviceLogic.PollingOptions;

            if (pollingOptions.PollOnCmd)
            {
                return false;
            }
            else if (pollingOptions.Time > TimeSpan.Zero || pollingOptions.Period > TimeSpan.Zero)
            {
                // local polling time is used
                DateTime localNow = utcNow.ToLocalTime();
                DateTime nowDate = localNow.Date;
                TimeSpan nowTime = localNow.TimeOfDay;
                DateTime localSessionDT = deviceLogic.LastSessionTime.ToLocalTime();
                DateTime sessionDate = localSessionDT.Date;
                TimeSpan sessionTime = localSessionDT.TimeOfDay;

                if (pollingOptions.Period > TimeSpan.Zero) // period is set
                {
                    // periodic polling
                    double timeInSec = pollingOptions.Time.TotalSeconds;
                    double periodInSec = pollingOptions.Period.TotalSeconds;
                    int sessionNum = (int)((nowTime.TotalSeconds - timeInSec) / periodInSec);

                    return pollingOptions.Time <= nowTime /*time to poll*/ &&
                        (sessionTime.TotalSeconds < sessionNum * periodInSec + timeInSec ||
                        sessionTime > nowTime /*session was yesterday*/);
                }
                else if (pollingOptions.Time > TimeSpan.Zero) // time is set
                {
                    // polling once a day at the specified time
                    return pollingOptions.Time <= nowTime /*time to poll*/ &&
                        (sessionDate < nowDate || sessionTime < pollingOptions.Time /*after an extra poll*/);
                }
            }

            // continuous cyclic polling
            return true;
        }

        /// <summary>
        /// Transfers data from the device to the server.
        /// </summary>
        private void TransferDeviceData(DeviceLogic deviceLogic, bool allData)
        {
            // current data
            DeviceSlice currentSlice = deviceLogic.GetCurrentData(allData);

            if (!currentSlice.IsEmpty)
                coreLogic.EnqueueCurrentData(currentSlice);

            // historical data
            while (deviceLogic.DeviceData.DequeueSlice(out DeviceSlice historicalSlice))
            {
                coreLogic.EnqueueHistoricalData(historicalSlice);
            }

            // events
            while (deviceLogic.DeviceData.DequeueEvent(out DeviceEvent deviceEvent))
            {
                coreLogic.EnqueueEvent(deviceEvent);
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
                sb.AppendLine(Title);
                sb.Append('-', Title.Length).AppendLine();
                string deviceHeader;

                if (Locale.IsRussian)
                {
                    sb.Append("Статус      : ").AppendLine(lineStatus.ToString(true));
                    sb.Append("Канал связи : ").AppendLine(channel.ChannelLogic.StatusText);
                    deviceHeader = $"Устройства ({devices.Count})";
                }
                else
                {
                    sb.Append("Status                : ").AppendLine(lineStatus.ToString(false));
                    sb.Append("Communication channel : ").AppendLine(channel.ChannelLogic.StatusText);
                    deviceHeader = $"Devices ({devices.Count})";
                }

                channel.ChannelLogic.AppendInfo(sb);

                if (SharedData != null && SharedData.Count > 0)
                    EngineUtils.AppendSharedData(sb, SharedData);

                sb
                    .AppendLine()
                    .AppendLine(deviceHeader)
                    .Append('-', deviceHeader.Length).AppendLine();

                if (devices.Count > 0)
                {
                    foreach (DeviceWrapper deviceWrapper in devices)
                    {
                        DeviceLogic deviceLogic = deviceWrapper.DeviceLogic;
                        sb
                            .Append(deviceLogic.Title)
                            .Append(' ', maxDeviceTitleLength - deviceLogic.Title.Length)
                            .Append(" : ")
                            .AppendLine(deviceLogic.StatusText);
                    }
                }
                else
                {
                    sb.AppendLine(Locale.IsRussian ? "Устройств нет" : "No devices");
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
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при записи в файл информации о работе линии связи" :
                    "Error writing communication line information to the file");
            }
        }

        /// <summary>
        /// Writes device information to appropriate files.
        /// </summary>
        private void WriteDeviceInfo()
        {
            devices.ForEach(d => d.WriteInfo());
        }


        /// <summary>
        /// Starts the communication line.
        /// </summary>
        public bool Start()
        {
            try
            {
                if (thread == null)
                {
                    Log.WriteBreak();
                    Log.WriteAction(Locale.IsRussian ? 
                        "Запуск линии связи {0}" :
                        "Start communication line {0}", Title);

                    if (Prepare(out string errMsg))
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
                    Log.WriteAction(Locale.IsRussian ?
                        "Линия связи {0} уже запущена" :
                        "Communication line {0} is already started", Title);
                }

                return thread != null;
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске линии связи {0}" :
                    "Error starting communication line {0}", Title);
                return false;
            }
            finally
            {
                if (thread == null)
                {
                    lineStatus = ServiceStatus.Error;
                    WriteInfo();
                }
            }
        }

        /// <summary>
        /// Begins termination process of the communication line.
        /// </summary>
        public void Terminate()
        {
            try
            {
                if (thread == null)
                {
                    lineStatus = ServiceStatus.Terminated;
                    WriteInfo();
                }
                else
                {
                    terminated = true;
                    lineStatus = ServiceStatus.Terminating;
                    devices.ForEach(d => d.DeviceLogic.Terminate());
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при завершении связи {0}" :
                    "Error terminating communication line {0}", Title);
            }
        }

        /// <summary>
        /// Adds the telecontrol command to the queue.
        /// </summary>
        public void EnqueueCommand(TeleCommand cmd)
        {
            if (LineConfig.LineOptions.CmdEnabled)
            {
                lock (commands)
                {
                    commands.Enqueue(cmd);
                }
            }
            else
            {
                Log.WriteLine();
                Log.WriteAction(Locale.IsRussian ?
                    "Выполнение команды не разрешено в конфигурации линии связи" :
                    "Command execution is disabled in communication line configuration");
            }
        }

        /// <summary>
        /// Schedules a priority poll the device.
        /// </summary>
        public void PollWithPriority(int deviceNum)
        {
            if (deviceMap.TryGetValue(deviceNum, out DeviceWrapper deviceWrapper))
                PollWithPriority(deviceWrapper);
        }

        /// <summary>
        /// Selects all devices on the communication line.
        /// </summary>
        public IEnumerable<DeviceLogic> SelectDevices()
        {
            return devices.Select(dw => dw.DeviceLogic);
        }

        /// <summary>
        /// Selects devices on the communication line that satisfy the condition.
        /// </summary>
        IEnumerable<DeviceLogic> ILineContext.SelectDevices(Func<DeviceLogic, bool> predicate)
        {
            return devices.Select(dw => dw.DeviceLogic).Where(predicate);
        }

        /// <summary>
        /// Gets the device by device number.
        /// </summary>
        bool ILineContext.GetDevice(int deviceNum, out DeviceLogic deviceLogic)
        {
            if (deviceMap.TryGetValue(deviceNum, out DeviceWrapper deviceWrapper))
            {
                deviceLogic = deviceWrapper.DeviceLogic;
                return true;
            }
            else
            {
                deviceLogic = null;
                return true;
            }
        }

        /// <summary>
        /// Gets the device by numeric address.
        /// </summary>
        bool ILineContext.GetDeviceByAddress(int numAddress, out DeviceLogic deviceLogic)
        {
            return deviceByNumAddr.TryGetValue(numAddress, out deviceLogic);
        }

        /// <summary>
        /// Gets the device by string address.
        /// </summary>
        bool ILineContext.GetDeviceByAddress(string strAddress, out DeviceLogic deviceLogic)
        {
            return deviceByStrAddr.TryGetValue(strAddress, out deviceLogic);
        }

        /// <summary>
        /// Creates a communication line, communication channel and devices.
        /// </summary>
        public static CommLine Create(LineConfig lineConfig, CoreLogic coreLogic, DriverHolder driverHolder)
        {
            // create communication line
            CommLine commLine = new CommLine(lineConfig, coreLogic);

            // create communication channel
            if (string.IsNullOrEmpty(lineConfig.Channel.Driver))
            {
                ChannelLogic channelLogic = new ChannelLogic(commLine, lineConfig.Channel); // stub
                commLine.channel = new ChannelWrapper(channelLogic, commLine.Log);
            }
            else if (driverHolder.GetDriver(lineConfig.Channel.Driver, out DriverLogic driverLogic))
            {
                ChannelLogic channelLogic = driverLogic.CreateChannel(commLine, lineConfig.Channel);
                commLine.channel = new ChannelWrapper(channelLogic, commLine.Log);
            }
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Драйвер канала связи {0} не найден." :
                    "Communication channel driver {0} not found.", lineConfig.Channel.Driver);
            }

            // create devices
            foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
            {
                if (deviceConfig.Active && !coreLogic.DeviceExists(deviceConfig.DeviceNum))
                {
                    if (driverHolder.GetDriver(deviceConfig.Driver, out DriverLogic driverLogic))
                    {
                        DeviceLogic deviceLogic = driverLogic.CreateDevice(commLine, deviceConfig);

                        if (deviceLogic == null)
                        {
                            throw new ScadaException(Locale.IsRussian ?
                                "Не удалось создать устройство {0}." :
                                "Unable to create device {0}.", CommUtils.GetDeviceTitle(deviceConfig));
                        }

                        commLine.AddDevice(deviceLogic);
                    }
                    else
                    {
                        throw new ScadaException(Locale.IsRussian ?
                            "Драйвер {0} для устройства {1} не найден." :
                            "Driver {0} for device {1} not found.",
                            deviceConfig.Driver, CommUtils.GetDeviceTitle(deviceConfig));
                    }
                }
            }

            // prepare channel after adding devices
            commLine.channel.ChannelLogic.MakeReady();

            return commLine;
        }
    }
}
