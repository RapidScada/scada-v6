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
 * Summary  : Implements OPC UA server for Rapid SCADA
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
    /// Implements OPC UA server for Rapid SCADA.
    /// <para>Реализует сервер OPC UA для Rapid SCADA.</para>
    /// </summary>
    internal class CustomServer : StandardServer
    {
        /// <summary>
        /// Creates the master node manager for the server.
        /// </summary>
        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            return new MasterNodeManager(server, configuration, null,
                new INodeManager[] { new NodeManager(server, configuration) });
        }
    }
}
