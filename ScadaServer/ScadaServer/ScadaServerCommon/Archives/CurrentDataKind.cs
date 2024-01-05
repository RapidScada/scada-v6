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
 * Module   : ScadaServerCommon
 * Summary  : Specifies the kinds of current data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

namespace Scada.Server.Archives
{
    /// <summary>
    /// Specifies the kinds of current data.
    /// <para>Задаёт виды текущих данных.</para>
    /// </summary>
    public enum CurrentDataKind
    {
        /// <summary>
        /// Actual current data.
        /// </summary>
        Current,

        /// <summary>
        /// Previous current data.
        /// </summary>
        Previous,

        /// <summary>
        /// Previous defined current data.
        /// </summary>
        PreviousDefined
    }
}
