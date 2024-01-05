/*
 * Copyright 2024 Rapid Software LLC
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
 * Summary  : Reads telecontrol commands from files
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using System;
using System.IO;
using System.Threading;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Reads telecontrol commands from files.
    /// <para>Считывает команды ТУ из файлов.</para>
    /// </summary>
    internal class CommandReader
    {
        /// <summary>
        /// The minimum file age to process a command.
        /// </summary>
        private static readonly TimeSpan MinFileAge = TimeSpan.FromMilliseconds(500);

        private readonly CoreLogic coreLogic; // the Communicator logic instance
        private readonly string cmdDir;       // the directory of commands
        private readonly ILog log;            // the application log

        private Thread thread;                // the working thread of the reader
        private volatile bool terminated;     // necessary to stop the thread

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandReader(CoreLogic coreLogic)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
            cmdDir = coreLogic.AppDirs.CmdDir;
            log = coreLogic.Log;

            thread = null;
            terminated = false;
        }


        /// <summary>
        /// Operating loop running in a separate thread.
        /// </summary>
        private void Execute()
        {
            DirectoryInfo cmdDirInfo = new DirectoryInfo(cmdDir);

            while (!terminated)
            {
                try
                {
                    if (cmdDirInfo.Exists)
                    {
                        DateTime utcNow = DateTime.UtcNow;

                        foreach (FileInfo fileInfo in 
                            cmdDirInfo.EnumerateFiles("*.dat", SearchOption.TopDirectoryOnly))
                        {
                            // skip potentially incomplete file
                            if (utcNow - fileInfo.LastWriteTimeUtc < MinFileAge)
                                continue;

                            // process file
                            TeleCommand cmd = new TeleCommand();
                            string fileName = fileInfo.FullName;

                            try
                            {
                                if (cmd.Load(fileName, out string errMsg))
                                {
                                    coreLogic.EnqueueCommand(cmd, fileName);
                                    File.Delete(fileName);
                                }
                                else
                                {
                                    log.WriteError(errMsg);
                                    File.Move(fileName, fileName + ".err"); // rename file for manual analysis
                                }
                            }
                            catch (Exception ex)
                            {
                                log.WriteError(ex, Locale.IsRussian ?
                                    "Ошибка при обработке файла {0}" :
                                    "Error processing the file {0}", fileName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.WriteError(ex, Locale.IsRussian ?
                        "Ошибка при чтении команд из файлов" :
                        "Error reading commands from files");
                }
                finally
                {
                    Thread.Sleep(ScadaUtils.ThreadDelay);
                }
            }
        }

        /// <summary>
        /// Starts reading commands.
        /// </summary>
        public void Start()
        {
            try
            {
                if (thread == null)
                {
                    log.WriteAction(Locale.IsRussian ? 
                        "Запуск чтения команд из файлов" :
                        "Start reading commands from files");
                    terminated = false;
                    thread = new Thread(Execute) { Priority = ThreadPriority.BelowNormal };
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске чтения команд" :
                    "Error starting reading commands");
            }
        }

        /// <summary>
        /// Stops reading commands.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (thread != null)
                {
                    terminated = true;
                    thread.Join();
                    thread = null;
                }
            }
            catch (Exception ex)
            {
                log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при остановке чтения команд" :
                    "Error stopping reading commands");
            }
        }
    }
}
