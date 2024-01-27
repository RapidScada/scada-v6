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
 * Module   : ScadaAdminCommon
 * Summary  : Specifies the messages sent and received by the Administrator application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Specifies the known messages sent and received by extensions.
    /// <para>Задает известные сообщения, отправляемые и принимаемые расширениями.</para>
    /// </summary>
    public static class KnownExtensionMessage
    {
        /// <summary>
        /// Calls the Project Tools extension to generate a channel map.
        /// </summary>
        public const string GenerateChannelMap = "ExtProjectTools.GenerateChannelMap";

        /// <summary>
        /// Calls the Project Tools extension to generate a device map.
        /// </summary>
        public const string GenerateDeviceMap = "ExtProjectTools.GenerateDeviceMap";

        /// <summary>
        /// Calls the Communicator Configurator extension to update the communication line node.
        /// </summary>
        public const string UpdateLineNode = "ExtCommConfig.UpdateLineNode";
    }
}
