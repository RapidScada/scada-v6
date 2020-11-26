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

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Represents a communication line.
    /// <para>Представляет линию связи.</para>
    /// </summary>
    internal class CommLine : ILineContext
    {
        private readonly CoreLogic coreLogic; // the Communicator logic instance
        private readonly string infoFileName; // the full file name to write communication line information

        private Thread thread;                // the working thread of the communication line
        private volatile bool terminated;     // necessary to stop the thread
        private volatile CommLineStatus lineStatus; // the current communication line status
        private List<DeviceLogic> devices;    // the list of devices


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private CommLine(LineConfig lineConfig, CoreLogic coreLogic)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            infoFileName = Path.Combine(coreLogic.AppDirs.LogDir, CommUtils.GetLineLogFileName(CommLineNum, ".txt"));

            thread = null;
            terminated = false;
            lineStatus = CommLineStatus.Undefined;
            devices = new List<DeviceLogic>();

            LineConfig = lineConfig ?? throw new ArgumentNullException(nameof(lineConfig));
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
        public string Title
        {
            get
            {
                return CommUtils.GetLineTitle(LineConfig.CommLineNum, LineConfig.Name);
            }
        }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        public ILog Log { get; }

        /// <summary>
        /// Gets the shared data of the communication line.
        /// </summary>
        public IDictionary<string, object> SharedData { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the communication line is terminated.
        /// </summary>
        public bool IsTerminated
        {
            get
            {
                return lineStatus == CommLineStatus.Terminated;
            }
        }


        /// <summary>
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {

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
                    Log.WriteAction(Locale.IsRussian ? 
                        "Запуск линии связи {0}" :
                        "Start communication line {0}", Title);
                    //PrepareProcessing();
                    thread = new Thread(Execute);
                    thread.Start();
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
                    lineStatus = CommLineStatus.Error;
                    //WriteInfo();
                }
            }
        }

        /// <summary>
        /// Begins termination process of the communication line.
        /// </summary>
        public void Terminate()
        {
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
            CommLine commLine = new CommLine(lineConfig, coreLogic);
            return commLine;
        }
    }
}
