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

        private readonly CoreLogic coreLogic;         // the Communicator logic instance
        private readonly string infoFileName;         // the full file name to write communication line information
        private readonly List<DeviceWrapper> devices; // the devices to poll

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
                devices.ForEach(d => WriteDeviceInfo(d));
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

                if (coreLogic.BaseDataSet != null)
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
        /// Working cycle of the communication line.
        /// </summary>
        private void LineCycle()
        {
            while (!terminated)
            {
                Thread.Sleep(ScadaUtils.ThreadDelay);
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
        /// Writes device information to the file.
        /// </summary>
        private void WriteDeviceInfo(DeviceWrapper deviceWrapper)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(deviceWrapper.InfoFileName, false, Encoding.UTF8))
                {
                    writer.Write(deviceWrapper.DeviceLogic.GetInfo());
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при записи в файл информации о работе КП {0}" :
                    "Error writing device {0} information to the file", 
                    deviceWrapper.DeviceLogic.Title);
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
            foreach (DeviceWrapper deviceWrapper in devices)
            {
                deviceWrapper.DeviceLogic.Terminate();
            }

            terminated = true;
        }

        /// <summary>
        /// Sends the telecontrol command to the current communication line.
        /// </summary>
        public void SendCommand(TeleCommand cmd)
        {

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
