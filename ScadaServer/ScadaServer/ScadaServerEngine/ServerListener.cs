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
 * Summary  : Represents a TCP listener which waits for client connections
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Log;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents a TCP listener which waits for client connections.
    /// <para>Представляет TCP-прослушиватель, который ожидает подключения клиентов.</para>
    /// </summary>
    internal class ServerListener : BaseListener
    {
        private readonly CoreLogic coreLogic; // the server logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ServerListener(CoreLogic coreLogic, ListenerOptions listenerOptions, ILog log)
            : base(listenerOptions, log)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException("coreLogic");
        }


        /// <summary>
        /// Gets the server name and version.
        /// </summary>
        protected override string GetServerName()
        {
            return Locale.IsRussian ?
                "Сервер " + ServerUtils.AppVersion :
                "Server " + ServerUtils.AppVersion;
        }

        /// <summary>
        /// Gets the directory name by ID.
        /// </summary>
        protected override string GetDirectory(ushort directoryID)
        {
            return @"C:\SCADA\";
        }
    }
}
