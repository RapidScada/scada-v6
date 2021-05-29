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

namespace Scada.Client
{
    /// <summary>
    /// Represents a pool of clients that interact with the Server service.
    /// <para>Представляет пул клиентов, которые взаимодействуют со службой Сервера.</para>
    /// </summary>
    public class ScadaClientPool
    {
        /// <summary>
        /// Represents a group of clients that have the same connection options.
        /// </summary>
        protected class ClientGroup
        {
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public ClientGroup(ConnectionOptions connectionOptions)
            {
                ConnectionOptions = connectionOptions;
            }

            /// <summary>
            /// Gets the connection options, the same for the group.
            /// </summary>
            public ConnectionOptions ConnectionOptions { get; }

            /// <summary>
            /// Gets a client from the group if one is available, otherwise creates one.
            /// </summary>
            public ScadaClient GetClient()
            {
                return new ScadaClient(ConnectionOptions);
            }

            /// <summary>
            /// Return the client to the group.
            /// </summary>
            public void ReturnClient(ScadaClient client)
            {

            }
        }


        /// <summary>
        /// The default pool capacity.
        /// </summary>
        public const int DefaultCapacity = 1000;


        /// <summary>
        /// The client groups accessed by connection key.
        /// </summary>
        protected readonly Dictionary<string, ClientGroup> clientGroups;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaClientPool()
            : this(DefaultCapacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaClientPool(int capacity)
        {
            clientGroups = new Dictionary<string, ClientGroup>();
            Capacity = capacity;
        }


        /// <summary>
        /// Gets the maximum number of clients that can be stored in the pool.
        /// </summary>
        public int Capacity { get; }

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
        /// Gets a key that identifies the connection options.
        /// </summary>
        private string GetOptionsKey(ConnectionOptions connectionOptions)
        {
            return "";
        }

        /// <summary>
        /// Gets a client from the pool if one is available, otherwise creates one.
        /// </summary>
        /// <remarks>If capacity is reached, an exception is raised.</remarks>
        public ScadaClient GetClient(ConnectionOptions connectionOptions)
        {
            if (connectionOptions == null)
                throw new ArgumentNullException(nameof(connectionOptions));

            string optionsKey = GetOptionsKey(connectionOptions);
            ClientGroup clientGroup;

            lock (clientGroups)
            {
                if (!clientGroups.TryGetValue(optionsKey, out clientGroup))
                {
                    clientGroup = new ClientGroup(connectionOptions);
                    clientGroups.Add(optionsKey, clientGroup);
                }
            }

            return clientGroup.GetClient();
        }

        /// <summary>
        /// Return the client to the pool.
        /// </summary>
        public void ReturnClient(ScadaClient client)
        {
            /*lock (clientGroups)
            {
                if (client != null && clientGroups.TryGetValue(GetOptionsKey(client.ConnectionOptions),
                out ClientGroup clientGroup))
                {
                    clientGroup.ReturnClient(client);
                }
            }*/
        }
    }
}
