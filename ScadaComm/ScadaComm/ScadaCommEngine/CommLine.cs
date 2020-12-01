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
 * Summary  : Represents a communication line
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2006
 * Modified : 2020
 */

using Scada.Comm.Channels;
using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
        private readonly Queue<TeleCommand> commands; // the command queue

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
            commands = new Queue<TeleCommand>();

            thread = null;
            terminated = false;
            lineStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;
            maxDeviceTitleLength = 0;
            channel = null;

            Title = CommUtils.GetLineTitle(CommLineNum, lineConfig.Name);
            SharedData = null;
            Log = new LogFile(LogFormat.Full)
            {
                FileName = Path.Combine(coreLogic.AppDirs.LogDir, CommUtils.GetLineLogFileName(CommLineNum, ".log")),
                Capacity = coreLogic.Config.GeneralOptions.MaxLogSize
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
        /// Prepares the communication line for start.
        /// </summary>
        private bool Prepare(out string errMsg)
        {
            terminated = false;
            lineStatus = ServiceStatus.Starting;
            SharedData = new ConcurrentDictionary<string, object>();
            WriteInfo();

            if (devices.Count > 0)
            {
                devices.ForEach(d => d.WriteInfo());
                errMsg = "";
                return true;
            }
            else
            {
                errMsg = Locale.IsRussian ?
                    "Работа линии связи невозможна из-за отсутствия КП" :
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
                devices.ForEach(d => d.OnCommLineStart());

                if (LineConfig.IsBound && coreLogic.BaseDataSet != null)
                    devices.ForEach(d => d.Bind(coreLogic.BaseDataSet));

                lineStatus = ServiceStatus.Normal;
                LineCycle();
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, CommonPhrases.ThreadFatalError);
            }
            finally
            {
                devices.ForEach(d => d.OnCommLineTerminate());
                channel.Stop();
                lineStatus = ServiceStatus.Terminated;
                WriteInfo();

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
            int cycleDelay = Math.Min(MinCycleDelay, LineConfig.LineOptions.CycleDelay);
            int deviceCnt = devices.Count;
            int deviceIndex = 0;
            int requiredSessionCnt = 0;
            int actualSessionCnt = 0;
            bool skipUnableMsg = false;

            while (!terminated)
            {
                try
                {
                    DateTime utcNow = DateTime.UtcNow;

                    // commands
                    while (DequeueCommand(out TeleCommand cmd))
                    {

                    }

                    // session
                    DeviceWrapper deviceWrapper = devices[deviceIndex];
                    DeviceLogic deviceLogic = deviceWrapper.DeviceLogic;

                    if (CheckSessionIsRequired(deviceLogic, utcNow))
                    {
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
                                    "Невозможно выполнить сеанс связи с {0}, т.к. соединение не установлено" :
                                    "Unable to communicate with {0} because connection is not established",
                                    deviceLogic.Title));
                            }
                        }

                        channel.AfterSession(deviceLogic);
                        requiredSessionCnt++;
                    }

                    if (++deviceIndex >= deviceCnt)
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
                    Log.WriteException(ex, Locale.IsRussian ?
                        "Ошибка в цикле работы линии связи" :
                        "Error in the communication line work cycle");
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }

        /// <summary>
        /// Removes, validates and returns the command at the beginning of the queue.
        /// </summary>
        private bool DequeueCommand(out TeleCommand cmd)
        {
            cmd = null;
            return false;
        }

        /// <summary>
        /// Checks if a device polling session of the device is required.
        /// </summary>
        private bool CheckSessionIsRequired(DeviceLogic deviceLogic, DateTime utcNow)
        {
            PollingOptions pollingOptions = deviceLogic.PollingOptions;
            bool timeIsSet = pollingOptions.Time > TimeSpan.Zero;
            bool periodIsSet = pollingOptions.Period > TimeSpan.Zero;

            if (timeIsSet || periodIsSet)
            {
                DateTime localNow = utcNow.ToLocalTime();
                DateTime nowDate = localNow.Date;
                TimeSpan nowTime = localNow.TimeOfDay;
                DateTime lastSessionDate = deviceLogic.LastSessionTime.Date;
                TimeSpan lastSessionTime = deviceLogic.LastSessionTime.TimeOfDay;

                if (periodIsSet)
                {
                    // periodic polling
                    double timeInSec = pollingOptions.Time.TotalSeconds;
                    double periodInSec = pollingOptions.Period.TotalSeconds;
                    int n = (int)((nowTime.TotalSeconds - timeInSec) / periodInSec) + 1;

                    return pollingOptions.Time <= nowTime /*time to poll*/ &&
                        (lastSessionTime.TotalSeconds <= n * periodInSec + timeInSec ||
                        lastSessionTime > nowTime /*session was yesterday*/);
                }
                else if (timeIsSet)
                {
                    // polling once a day at a specified time
                    return pollingOptions.Time <= nowTime /*time to poll*/ &&
                        (lastSessionDate < nowDate || lastSessionTime < pollingOptions.Time /*after an extra poll*/);
                }
            }

            // continuous cyclic polling
            return true;
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
                sb
                    .AppendLine(Title)
                    .Append('-', Title.Length).AppendLine();

                if (Locale.IsRussian)
                {
                    sb.Append("Статус      : ").AppendLine(lineStatus.ToString(true));
                    sb.Append("Канал связи : ").AppendLine(channel.ChannelLogic.StatusText);
                }
                else
                {
                    sb.Append("Status                : ").AppendLine(lineStatus.ToString(false));
                    sb.Append("Communication channel : ").AppendLine(channel.ChannelLogic.StatusText);
                }

                string header = Locale.IsRussian ?
                    "КП (" + devices.Count + ")" :
                    "Devices (" + devices.Count + ")";

                sb
                    .AppendLine()
                    .AppendLine(header)
                    .Append('-', header.Length).AppendLine();

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
                    sb.AppendLine(Locale.IsRussian ? "КП нет" : "No devices");
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
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи в файл информации о работе линии связи" :
                    "Error writing communication line information to the file");
            }
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
                Log.WriteException(ex, Locale.IsRussian ?
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
            devices.ForEach(d => d.DeviceLogic.Terminate());
            terminated = true;
        }

        /// <summary>
        /// Enqueues the telecontrol command.
        /// </summary>
        public void EnqueueCommand(TeleCommand cmd)
        {
            try
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
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при постановке команды в очередь" :
                    "Error enqueuing command");
            }
        }

        /// <summary>
        /// Creates a communication line, communication channel and devices.
        /// </summary>
        public static CommLine Create(LineConfig lineConfig, CoreLogic coreLogic, DriverHolder driverHolder)
        {
            // create communication line
            CommLine commLine = new CommLine(lineConfig, coreLogic);

            // create communication channel
            if (string.IsNullOrEmpty(lineConfig.Channel.TypeName))
            {
                ChannelLogic channelLogic = new ChannelLogic(commLine, lineConfig.Channel);
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
                    "Драйвер для создания канала связи не найден." :
                    "Driver for creating communication channel not found.");
            }

            // create devices
            foreach (DeviceConfig deviceConfig in lineConfig.DevicePolling)
            {
                if (driverHolder.GetDriver(deviceConfig.Driver, out DriverLogic driverLogic))
                {
                    DeviceLogic deviceLogic = driverLogic.CreateDevice(commLine, deviceConfig);

                    commLine.devices.Add(new DeviceWrapper(deviceLogic, commLine.Log)
                    {
                        InfoFileName = Path.Combine(coreLogic.AppDirs.LogDir, 
                            CommUtils.GetDeviceLogFileName(deviceLogic.DeviceNum, ".txt"))
                    });

                    if (commLine.maxDeviceTitleLength < deviceLogic.Title.Length)
                        commLine.maxDeviceTitleLength = deviceLogic.Title.Length;
                }
            }

            return commLine;
        }
    }
}
