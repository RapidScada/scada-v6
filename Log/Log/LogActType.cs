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
 * Module   : Log
 * Summary  : Specifies the log action types
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Log
{
    /// <summary>
    /// Specifies the log action types.
    /// <para>Задает типы действий журнала.</para>
    /// </summary>
    public enum LogActType
    {
        /// <summary>
        /// Informational message.
        /// </summary>
        Info,

        /// <summary>
        /// Action performed.
        /// </summary>
        Action,

        /// <summary>
        /// Error occurred.
        /// </summary>
        Error,

        /// <summary>
        /// Exception occurred.
        /// </summary>
        Exception,
    }
}
