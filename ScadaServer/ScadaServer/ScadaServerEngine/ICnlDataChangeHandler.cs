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
 * Module   : ScadaServerEngine
 * Summary  : Defines functionality to handle channel data changes
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
    /// Defines functionality to handle channel data changes.
    /// <para>Определяет функциональность для обработки изменений данных каналов.</para>
    /// </summary>
    internal interface ICnlDataChangeHandler
    {
        /// <summary>
        /// Handles the changes of the current channel data.
        /// </summary>
        void HandleCurDataChanged(CnlTag cnlTag, ref CnlData cnlData, CnlData prevCnlData, 
            CnlData prevCnlDataDef, DateTime timestamp, DateTime prevTimestamp);
    }
}
