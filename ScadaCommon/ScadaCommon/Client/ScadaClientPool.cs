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
 * Module   : ScadaCommon
 * Summary  : Represents a pool of clients that interact with the Server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2022
 */

using Scada.Lang;
using System;
using System.Collections.Generic;
using System.Text;

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
            private readonly ConnectionOptions connectionOptions;
            private readonly int capacity;
            private readonly int clientMode;
            private readonly List<ScadaClient> availableClients;
            private readonly Dictionary<long, ScadaClient> usedClients;


            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
            public ClientGroup(ConnectionOptions connectionOptions, int capacity, int clientMode)
            {
                this.connectionOptions = connectionOptions ?? throw new ArgumentNullException(nameof(connectionOptions));
                this.capacity = capacity;
                this.clientMode = clientMode;
                availableClients = new List<ScadaClient>();
                usedClients = new Dictionary<long, ScadaClient>();
                LastAccessTime = DateTime.UtcNow;
            }


            /// <summary>
            /// Gets the time (UTC) when the group was last accessed.
            /// </summary>
            public DateTime LastAccessTime { get; private set; }


            /// <summary>
            /// Creates a new client or raises an exception if the capacity is reached.
            /// </summary>
            private ScadaClient CreateClient()
            {
                if (availableClients.Count + usedClients.Count < capacity)
                {
                    return new ScadaClient(connectionOptions) { ClientMode = clientMode };
                }
                else
                {
                    throw new ScadaException(Locale.IsRussian ?
                        "Достигнута максимальная вместимость пула клиентов." :
                        "The maximum capacity of the client pool has been reached.");
                }
            }

            /// <summary>
            /// Removes and returns the client at the end of the client list.
            /// </summary>
            private ScadaClient PopClient()
            {
                int lastIndex = availableClients.Count - 1;
                ScadaClient scadaClient = availableClients[lastIndex];
                availableClients.RemoveAt(lastIndex);
                return scadaClient;
            }

            /// <summary>
            /// Gets a client from the group if one is available, otherwise creates one.
            /// </summary>
            public ScadaClient GetClient(DateTime nowDT)
            {
                lock (this)
                {
                    LastAccessTime = nowDT;
                    ScadaClient scadaClient = availableClients.Count > 0
                        ? PopClient()
                        : CreateClient();

                    usedClients[scadaClient.ClientID] = scadaClient;
                    return scadaClient;
                }
            }

            /// <summary>
            /// Returns the client to the group.
            /// </summary>
            public void ReturnClient(ScadaClient client, DateTime nowDT)
            {
                lock (this)
                {
                    LastAccessTime = nowDT;

                    if (usedClients.Remove(client.ClientID))
                        availableClients.Add(client);
                }
            }

            /// <summary>
            /// Removes the outdated clients.
            /// </summary>
            public void RemoveOutdatedClients(DateTime nowDT, TimeSpan cleanupPeriod)
            {
                lock (this)
                {
                    int endIndex = -1;

                    for (int i = 0, cnt = availableClients.Count; i < cnt; i++)
                    {
                        if (nowDT - availableClients[i].LastActivityTime > cleanupPeriod)
                            endIndex = i;
                        else
                            break;
                    }

                    if (endIndex >= 0)
                        availableClients.RemoveRange(0, endIndex + 1);
                }
            }

            /// <summary>
            /// Gets information about the group.
            /// </summary>
            public ClientGroupInfo GetInfo()
            {
                lock (this)
                {
                    return new ClientGroupInfo
                    {
                        ConnectionOptions = connectionOptions,
                        LastAccessTime = LastAccessTime,
                        Capacity = capacity,
                        AvailableClientCount = availableClients.Count,
                        UsedClientCount = usedClients.Count
                    };
                }
            }
        }

        /// <summary>
        /// Contains information about a group of clients.
        /// </summary>
        public struct ClientGroupInfo
        {
            /// <summary>
            /// Gets or sets the connection options.
            /// </summary>
            public ConnectionOptions ConnectionOptions { get; set; }
            /// <summary>
            /// Gets or sets the time (UTC) when the group was last accessed.
            /// </summary>
            public DateTime LastAccessTime { get; set; }
            /// <summary>
            /// Gets the maximum number of clients that can be stored in the group.
            /// </summary>
            public int Capacity { get; set; }
            /// <summary>
            /// Gets or sets the number of available clients.
            /// </summary>
            public int AvailableClientCount { get; set; }
            /// <summary>
            /// Gets or sets the number of clients in use.
            /// </summary>
            public int UsedClientCount { get; set; }
            /// <summary>
            /// Gets or sets the total number of clients.
            /// </summary>
            public int TotalClientCount => AvailableClientCount + UsedClientCount;
        }


        /// <summary>
        /// The default pool capacity.
        /// </summary>
        protected const int DefaultCapacity = 1000;
        /// <summary>
        /// The period of cleaning unused clients.
        /// </summary>
        protected static readonly TimeSpan CleanupPeriod = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The client groups accessed by connection key.
        /// </summary>
        protected readonly Dictionary<string, ClientGroup> clientGroups;
        /// <summary>
        /// The time (UTC) when the outdated clients were last removed.
        /// </summary>
        protected DateTime lastCleanupTime;


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
            lastCleanupTime = DateTime.UtcNow;
            Capacity = capacity;
            ClientMode = 0;
        }


        /// <summary>
        /// Gets the maximum number of clients, having the same connection options, that can be stored in the pool.
        /// </summary>
        public int Capacity { get; }
        
        /// <summary>
        /// Gets or sets the client mode for all clients in the pool.
        /// </summary>
        public int ClientMode { get; set; }

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
        protected string GetOptionsKey(ConnectionOptions connectionOptions)
        {
            if (string.IsNullOrEmpty(connectionOptions.AccessKey))
            {
                connectionOptions.AccessKey = new StringBuilder()
                    .Append(connectionOptions.Name).Append(';')
                    .Append(connectionOptions.Host).Append(';')
                    .Append(connectionOptions.Port).Append(';')
                    .Append(connectionOptions.Username).Append(';')
                    .Append(connectionOptions.Password).Append(';')
                    .Append(connectionOptions.Instance).Append(';')
                    .Append(connectionOptions.Timeout).Append(';')
                    .Append(ScadaUtils.BytesToHex(connectionOptions.SecretKey))
                    .ToString();
            }

            return connectionOptions.AccessKey;
        }

        /// <summary>
        /// Removes the outdated clients.
        /// </summary>
        protected void RemoveOutdatedClients(DateTime nowDT)
        {
            lock (clientGroups)
            {
                List<string> keysToRemove = new List<string>();

                foreach (KeyValuePair<string, ClientGroup> pair in clientGroups)
                {
                    if (nowDT - pair.Value.LastAccessTime > CleanupPeriod)
                        keysToRemove.Add(pair.Key);
                    else
                        pair.Value.RemoveOutdatedClients(nowDT, CleanupPeriod);
                }

                foreach (string key in keysToRemove)
                {
                    clientGroups.Remove(key);
                }
            }
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
                    clientGroup = new ClientGroup(connectionOptions, Capacity, ClientMode);
                    clientGroups.Add(optionsKey, clientGroup);
                }
            }

            DateTime utcNow = DateTime.UtcNow;
            ScadaClient scadaClient = clientGroup.GetClient(utcNow);

            // cleanup the pool
            if (utcNow - lastCleanupTime > CleanupPeriod)
                RemoveOutdatedClients(utcNow);

            return scadaClient;
        }

        /// <summary>
        /// Returns the client to the pool.
        /// </summary>
        public void ReturnClient(ScadaClient client)
        {
            if (client != null)
            {
                ClientGroup clientGroup;

                lock (clientGroups)
                {
                    clientGroups.TryGetValue(GetOptionsKey(client.ConnectionOptions), out clientGroup);
                }

                clientGroup?.ReturnClient(client, DateTime.UtcNow);
            }
        }

        /// <summary>
        /// Gets information about the pool.
        /// </summary>
        public ClientGroupInfo[] GetInfo()
        {
            lock (clientGroups)
            {
                int idx = 0;
                int cnt = clientGroups.Count;
                ClientGroupInfo[] info = new ClientGroupInfo[cnt];

                foreach (ClientGroup clientGroup in clientGroups.Values)
                {
                    info[idx++] = clientGroup.GetInfo();
                }

                return info;
            }
        }
    }
}
