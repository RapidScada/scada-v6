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
 * Module   : ScadaData
 * Summary  : Represents an output channel as an entity of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2018
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents an output channel as an entity of the configuration database.
    /// <para>Представляет канал управления как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class OutCnl
    {
        public int OutCnlNum { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public int CmdTypeID { get; set; }

        public int? ObjNum { get; set; }

        public int? DeviceNum { get; set; }

        public int? CmdNum { get; set; }

        public int? CmdValID { get; set; }

        public bool FormulaUsed { get; set; }

        public string Formula { get; set; }

        public bool EventEnabled { get; set; }
    }
}
