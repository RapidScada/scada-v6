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
 * Summary  : Implements the Communicator context interface for accessing the application environment
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Comm.Config;
using Scada.Comm.Drivers;
using Scada.Data.Models;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Engine
{
    /// <summary>
    /// Implements the Communicator context interface for accessing the application environment.
    /// <para>Реализует интерфейс контекста сервера для доступа к окружению приложения.</para>
    /// </summary>
    internal class CommContext : ICommContext
    {
        private readonly CoreLogic coreLogic; // the Communicator logic instance


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommContext(CoreLogic coreLogic)
        {
            this.coreLogic = coreLogic ?? throw new ArgumentNullException(nameof(coreLogic));
        }


        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public CommConfig AppConfig => coreLogic.Config;

        /// <summary>
        /// Gets the application directories.
        /// </summary>
        public CommDirs AppDirs => coreLogic.AppDirs;

        /// <summary>
        /// Gets the application log.
        /// </summary>
        public ILog Log => coreLogic.Log;

        /// <summary>
        /// Gets the configuration database cache.
        /// </summary>
        public BaseDataSet BaseDataSet => null;

        /// <summary>
        /// Gets the application level shared data.
        /// </summary>
        public IDictionary<string, object> SharedData => coreLogic.SharedData;


        /// <summary>
        /// Sends the telecontrol command to the current application.
        /// </summary>
        public void SendCommand(TeleCommand cmd)
        {
            // TODO: send command
        }
    }
}
