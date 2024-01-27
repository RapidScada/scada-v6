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
 * Summary  : Represents a set of limits as an entity of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Represents a set of limits as an entity of the configuration database.
    /// <para>Представляет границы как сущность базы конфигурации.</para>
    /// </summary>
    [Serializable]
    public class Lim
    {
        public int LimID { get; set; }

        public string Name { get; set; }

        public bool IsBoundToCnl { get; set; }

        public bool IsShared { get; set; }

        public double? LoLo { get; set; }

        public double? Low { get; set; }

        public double? High { get; set; }

        public double? HiHi { get; set; }

        public double? Deadband { get; set; }
    }
}
