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
 * Summary  : Specifies the client states
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

namespace Scada.Client
{
    /// <summary>
    /// Specifies the client communication states.
    /// <para>Задаёт состояния связи клиента.</para>
    /// </summary>
    public enum ClientState
    {
        /// <summary>
        /// Client is disconnected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// Client is just connected.
        /// </summary>
        Connected,

        /// <summary>
        /// Client is authenticated successfully.
        /// </summary>
        LoggedIn,

        /// <summary>
        /// Communication error has occurred. Client is still connected.
        /// </summary>
        Error
    }
}
