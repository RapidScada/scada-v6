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
 * Summary  : Specifies the primary applications that run as services
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2021
 */

namespace Scada
{
    /// <summary>
    /// Specifies the primary applications that run as services.
    /// <para>Задаёт основные приложения, которые работают как службы.</para>
    /// </summary>
    public enum ServiceApp : byte
    {
        /// <summary>
        /// Unknown application.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The Server application.
        /// </summary>
        Server = 1,

        /// <summary>
        /// The Communicator application.
        /// </summary>
        Comm = 2,

        /// <summary>
        /// The Webstation application.
        /// </summary>
        Web = 3
    }
}
