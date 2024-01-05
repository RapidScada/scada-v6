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
 * Summary  : Specifies the kinds of data positions
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Data.Tables
{
    /// <summary>
    /// Specifies the kinds of data positions.
    /// <para>Задаёт виды позиций данных.</para>
    /// </summary>
    public enum PositionKind
    {
        /// <summary>
        /// The exact data position.
        /// </summary>
        Exact,

        /// <summary>
        /// The closest data position that is less than or equal to the actual position.
        /// </summary>
        Floor,

        /// <summary>
        /// The exclusive closest data position that is less than the actual position.
        /// </summary>
        FloorExclusive,

        /// <summary>
        /// The closest data position that is greater than or equal to the actual position.
        /// </summary>
        Ceiling
    }
}
