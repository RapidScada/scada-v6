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
 * Module   : DrvCnlBasic
 * Summary  : Specifies the names of the channel types supported by the driver
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Specifies the names of the channel types supported by the driver.
    /// <para>Задает имена типов каналов, поддерживаемые драйвером.</para>
    /// </summary>
    internal static class ChannelTypeName
    {
        public const string Serial = "Serial";
        public const string TcpClient = "TcpClient";
        public const string TcpServer = "TcpServer";
        public const string Udp = "Udp";
    }
}
