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

using Scada.Log;
using Scada.Server.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements of the core server logic.
    /// <para>Реализует основную логику сервера.</para>
    /// </summary>
    internal class CoreLogic
    {
        /// <summary>
        /// The waiting time to stop the thread, ms.
        /// </summary>
        private const int WaitForStop = 10000;

        private readonly ServerConfig config; // the server configuration
        private readonly AppDirs appDirs;     // the application directories
        private readonly ILog log;            // the application log

        private Thread thread;                // the working thread of the logic
        private volatile bool terminated;     // necessary to stop the thread


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CoreLogic(ServerConfig config, AppDirs appDirs, ILog log)
        {
            this.config = config ?? throw new ArgumentNullException("config");
            this.appDirs = appDirs ?? throw new ArgumentNullException("appDirs");
            this.log = log ?? throw new ArgumentNullException("log");

            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Prepares the logic processing.
        /// </summary>
        private void PrepareProcessing()
        {
            terminated = false;
            //utcStartDT = DateTime.UtcNow;
            //startDT = utcStartDT.ToLocalTime();
            //workState = WorkState.Normal;
            //WriteInfo();
        }

        /// <summary>
        /// Work cycle running in a separate thread.
        /// </summary>
        private void Execute()
        {
            try
            {
                while (!terminated)
                {
                    try
                    {
                        log.WriteAction("Iteration...");
                        Thread.Sleep(1000);
                        //Thread.Sleep(ScadaUtils.ThreadDelay);
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
                //workState = WorkState.Terminated;
                //WriteInfo();
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
                    PrepareProcessing();
                    thread = new Thread(new ThreadStart(Execute));
                    thread.Start();
                }
                else
                {
                    log.WriteAction(Locale.IsRussian ?
                        "Обработка логики уже запущена" :
                        "Logic processing is already started");
                }

                return true;
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
                //if (thread == null)
                //{
                //    workState = WorkState.Error;
                //    WriteInfo();
                //}
            }
        }

        /// <summary>
        /// Stops processing logic.
        /// </summary>
        public void StopProcessing()
        {
            try
            {
                if (thread != null)
                {
                    terminated = true;

                    if (thread.Join(WaitForStop))
                    {
                        log.WriteAction(Locale.IsRussian ?
                            "Обработка логики остановлена" :
                            "Logic processing is stopped");
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
                //workState = WorkState.Error;
                //WriteInfo();
                log.WriteException(ex, Locale.IsRussian ?
                    "Ошибка при остановке обработки логики" :
                    "Error stopping logic processing");
            }
        }
    }
}
