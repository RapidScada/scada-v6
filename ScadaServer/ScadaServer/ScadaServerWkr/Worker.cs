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
 * Module   : Server Worker
 * Summary  : Implements the server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scada.Server.Engine;
using System.Threading;
using System.Threading.Tasks;

namespace Scada.Server.Wkr
{
    /// <summary>
    /// Implements the server service.
    /// <para>Реализует службу сервера.</para>
    /// </summary>
    public class Worker : BackgroundService
    {
        private const int TaskDelay = 1000;
        private readonly ILogger<Worker> logger;
        private readonly Manager manager;

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
            manager = new Manager();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (manager.StartService())
                logger.LogInformation("Server is started successfully");
            else
                logger.LogError("Server is started with errors");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TaskDelay, stoppingToken);
            }

            manager.StopService();
            logger.LogInformation("Server is stopped");
        }
    }
}
