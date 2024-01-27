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
 * Module   : Server Worker
 * Summary  : The Server service for Windows and Linux
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2024
 */

namespace Scada.Server.Wkr
{
    /// <summary>
    /// The Server service for Windows and Linux.
    /// <para>Служба Сервера для Windows и Linux.</para>
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services
                .AddWindowsService()
                .AddSystemd()
                .AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}
