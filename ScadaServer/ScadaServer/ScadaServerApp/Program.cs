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
 * Module   : Server Application
 * Summary  : The Server cross-platform console application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Server.Engine;

namespace Scada.Server.App
{
    /// <summary>
    /// The Server cross-platform console application.
    /// <para>Кросс-платформенное консольное приложение Сервера.</para>
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // start the service
            Console.WriteLine("Starting Server...");
            Manager manager = new();

            if (manager.StartService())
                Console.WriteLine("Server is started successfully");
            else
                Console.WriteLine("Server is started with errors");

            // stop the service if 'x' is pressed or a stop file exists
            const int ThreadDelay = 500;
            Console.WriteLine("Press 'x' or create 'serverstop' file to stop Server");
            string stopFileName = Path.Combine(manager.AppDirs.CmdDir, "serverstop");

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X || File.Exists(stopFileName)))
            {
                Thread.Sleep(ThreadDelay);
            }

            manager.StopService();
            try { File.Delete(stopFileName); } catch { }
            Console.WriteLine("Server is stopped");
        }
    }
}
