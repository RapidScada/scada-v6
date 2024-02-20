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
 * Module   : Agent Application
 * Summary  : The Agent cross-platform console application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2024
 */

using Scada.Agent.Engine;

namespace Scada.Agent.App
{
    /// <summary>
    /// The Agent cross-platform console application.
    /// <para>Кросс-платформенное консольное приложение Агента.</para>
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Agent...");
            Manager manager = new();

            if (manager.StartService())
                Console.WriteLine("Agent is started successfully");
            else
                Console.WriteLine("Agent is started with errors");

            // stop the service if 'x' is pressed or a stop file exists
            const int ThreadDelay = 500;
            Console.WriteLine("Press 'x' or create 'agentstop' file to stop Agent");
            string stopFileName = Path.Combine(manager.AppDirs.CmdDir, "agentstop");

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X || File.Exists(stopFileName)))
            {
                Thread.Sleep(ThreadDelay);
            }

            manager.StopService();
            try { File.Delete(stopFileName); } catch { }
            Console.WriteLine("Agent is stopped");
        }
    }
}
