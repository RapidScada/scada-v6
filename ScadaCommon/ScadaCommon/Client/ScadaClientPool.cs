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
 * Module   : ScadaCommon
 * Summary  : Represents a pool of clients that interact with the Server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Scada.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scada.Client
{
    /// <summary>
    /// Represents a pool of clients that interact with the Server service.
    /// <para>Представляет пул клиентов, которые взаимодействуют со службой Сервера.</para>
    /// </summary>
    public class ScadaClientPool
    {
        /// <summary>
        /// Gets the number of clients that currently exist.
        /// </summary>
        public int ClientCount
        {
            get
            {
                return 0;
            }
        }


        /// <summary>
        /// Gets a client from the pool if one is available, otherwise creates one.
        /// </summary>
        public ScadaClient GetClient(ConnectionOptions connectionOptions)
        {
            return null;
        }

        /// <summary>
        /// Return the client to the pool.
        /// </summary>
        public void ReturnClient(ScadaClient client)
        {

        }
    }
}
