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
 * Module   : ScadaServerEngine
 * Summary  : Encapsulates functionality required by the calculator
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Data.Models;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Encapsulates functionality required by the calculator.
    /// <para>Инкапсулирует функциональность, необходимую для калькулятора.</para>
    /// </summary>
    public interface ICalcContext
    {
        /// <summary>
        /// Gets the timestamp of the processed data (UTC).
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets a value indicating whether the processed data is current data.
        /// </summary>
        bool IsCurrent { get; }


        /// <summary>
        /// Gets the actual channel data.
        /// </summary>
        CnlData GetCnlData(int cnlNum);

        /// <summary>
        /// Gets the previous channel data, if applicable.
        /// </summary>
        CnlData GetPrevCnlData(int cnlNum);

        /// <summary>
        /// Gets the actual channel timestamp.
        /// </summary>
        DateTime GetCnlTime(int cnlNum);

        /// <summary>
        /// Gets the previous channel timestamp, if applicable.
        /// </summary>
        DateTime GetPrevCnlTime(int cnlNum);

        /// <summary>
        /// Sets the channel data.
        /// </summary>
        void SetCnlData(int cnlNum, CnlData cnlData);
    }
}
