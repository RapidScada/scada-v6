﻿/*
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
 * Module   : Communicator Application
 * Summary  : The Communicator cross-platform console application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada;
using Scada.Comm.Engine;
using System;
using System.IO;
using System.Threading;

namespace Scada.Comm.App
{
    /// <summary>
    /// The Communicator cross-platform console application.
    /// <para>Кросс-платформенное консольное приложение Коммуникатора.</para>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // start the service
            Console.WriteLine("Starting Communicator...");
            Manager manager = new Manager();

            if (manager.StartService())
                Console.WriteLine("Communicator is started successfully");
            else
                Console.WriteLine("Communicator is started with errors");

            // stop the service if 'x' is pressed or a stop file exists
            Console.WriteLine("Press 'x' or create 'commstop' file to stop Communicator");
            FileListener stopFileListener = new FileListener(Path.Combine(manager.AppDirs.CmdDir, "commstop"));

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X || stopFileListener.FileFound))
            {
                Thread.Sleep(ScadaUtils.ThreadDelay);
            }

            manager.StopService();
            stopFileListener.Terminate();
            stopFileListener.DeleteFile();
            Console.WriteLine("Communicator is stopped");
        }
    }
}
