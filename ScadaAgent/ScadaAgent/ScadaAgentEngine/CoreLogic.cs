/*
 * Copyright 2021 Rapid Software LLC
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
 * Module   : ScadaAgentEngine
 * Summary  : Implements the core Agent logic
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Agent.Config;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace Scada.Agent.Engine
{
    /// <summary>
    /// Implements the core Agent logic.
    /// <para>Реализует основную логику Агента.</para>
    /// </summary>
    internal class CoreLogic
    {
        private readonly AgentConfig appConfig; // the application configuration
        private readonly AppDirs appDirs;       // the application directories
        private readonly ILog log;              // the application log
        private readonly string infoFileName;   // the full file name to write application information

        private Thread thread;                  // the working thread of the logic
        private volatile bool terminated;       // necessary to stop the thread
        private DateTime utcStartDT;            // the UTC start time
        private DateTime startDT;               // the local start time
        private ServiceStatus serviceStatus;    // the current service status
        private int lastInfoLength;             // the last info text length

        private AgentListener listener;                      // the TCP listener
        private Dictionary<string, ScadaInstance> instances; // the instances accessed by name


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(AgentConfig appConfig, AppDirs appDirs, ILog log)
        {
            this.appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            infoFileName = Path.Combine(appDirs.LogDir, EngineUtils.InfoFileName);

            thread = null;
            terminated = false;
            utcStartDT = DateTime.MinValue;
            startDT = DateTime.MinValue;
            serviceStatus = ServiceStatus.Undefined;
            lastInfoLength = 0;

            listener = null;
            instances = null;
        }


        /// <summary>
        /// Gets a value indicating whether the Agent service is ready.
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
        private void PrepareProcessing()
        {
            terminated = false;
            utcStartDT = DateTime.UtcNow;
            startDT = utcStartDT.ToLocalTime();
            serviceStatus = ServiceStatus.Starting;
            WriteInfo();

            listener = new AgentListener(this, appConfig.ListenerOptions, appConfig.ReverseConnectionOptions, log);
            instances = new Dictionary<string, ScadaInstance>(appConfig.Instances.Count);

            foreach (InstanceOptions instanceOptions in appConfig.Instances)
            {
                if (instanceOptions.Active && !string.IsNullOrEmpty(instanceOptions.Name))
                    instances.Add(instanceOptions.Name, new ScadaInstance(log, instanceOptions));
            }
        }

        /// <summary>
        /// Operating cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                DateTime writeInfoDT = DateTime.MinValue; // the timestamp of writing application info
                serviceStatus = ServiceStatus.Normal;

                while (!terminated)
                {
                    try
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        // write application info
                        if (utcNow - writeInfoDT >= ScadaUtils.WriteInfoPeriod)
                        {
                            writeInfoDT = utcNow;
                            WriteInfo();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WriteError(ex, CommonPhrases.LogicCycleError);
                    }
                    finally
                    {
                        Thread.Sleep(ScadaUtils.ThreadDelay);
                    }
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommonPhrases.ThreadFatalError);
            }
            finally
            {
                serviceStatus = ServiceStatus.Terminated;
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
                StringBuilder sb = new StringBuilder((int)(lastInfoLength * 1.1));
                TimeSpan workSpan = DateTime.UtcNow - utcStartDT;
                string workSpanStr = workSpan.Days > 0 ?
                    workSpan.ToString(@"d\.hh\:mm\:ss") :
                    workSpan.ToString(@"hh\:mm\:ss");

                if (Locale.IsRussian)
                {
                    sb
                        .AppendLine("Агент")
                        .AppendLine("-----")
                        .Append("Запуск       : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Время работы : ").AppendLine(workSpanStr)
                        .Append("Статус       : ").AppendLine(serviceStatus.ToString(true))
                        .Append("Версия       : ").AppendLine(EngineUtils.AppVersion);
                }
                else
                {
                    sb
                        .AppendLine("Agent")
                        .AppendLine("-----")
                        .Append("Started        : ").AppendLine(startDT.ToLocalizedString())
                        .Append("Execution time : ").AppendLine(workSpanStr)
                        .Append("Status         : ").AppendLine(serviceStatus.ToString(false))
                        .Append("Version        : ").AppendLine(EngineUtils.AppVersion);
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
                log.WriteError(ex, CommonPhrases.WriteInfoError);
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
                    log.WriteAction(CommonPhrases.StartLogic);
                    PrepareProcessing();

                    if (listener.Start())
                    {
                        thread = new Thread(Execute);
                        thread.Start();
                    }
                }
                else
                {
                    log.WriteAction(CommonPhrases.LogicAlreadyStarted);
                }

                return thread != null;
            }
            catch (Exception ex)
            {
                log.WriteError(ex, CommonPhrases.StartLogicError);
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
                    serviceStatus = ServiceStatus.Terminating;

                    if (thread.Join(ScadaUtils.ThreadWait))
                        log.WriteAction(CommonPhrases.LogicStopped);
                    else
                        log.WriteAction(CommonPhrases.UnableToStopLogic);

                    thread = null;
                }
            }
            catch (Exception ex)
            {
                serviceStatus = ServiceStatus.Error;
                WriteInfo();
                log.WriteError(ex, CommonPhrases.StopLogicError);
            }
        }

        /// <summary>
        /// Gets the names of the proxy instances.
        /// </summary>
        public IEnumerable<string> GetProxyInstanceNames()
        {
            return from i in instances.Values
                   where i.ProxyMode
                   select i.Name;
        }

        /// <summary>
        /// Gets the instance by name.
        /// </summary>
        public bool GetInstance(string name, out ScadaInstance scadaInstance)
        {
            if (instances == null)
            {
                scadaInstance = null;
                return false;
            }
            else
            {
                return instances.TryGetValue(name, out scadaInstance);
            }
        }
    }
}
