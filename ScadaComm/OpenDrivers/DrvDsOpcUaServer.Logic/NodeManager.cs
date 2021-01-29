/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : DrvDsOpcUaServer
 * Summary  : Represents nodes of OPC UA server
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer.Logic
{
    /// <summary>
    /// Represents nodes of OPC UA server.
    /// <para>Представляет узлы сервера OPC UA.</para>
    /// </summary>
    internal class NodeManager : CustomNodeManager2
    {
        /// <summary>
        /// The namespace for the nodes provided by the server.
        /// </summary>
        private const string NamespaceUri = "http://rapidscada.org/RapidScada/DrvDsOpcUaServer";

        public NodeManager(IServerInternal server, ApplicationConfiguration configuration)
            : base(server, configuration, NamespaceUri)
        {

        }
    }
}
