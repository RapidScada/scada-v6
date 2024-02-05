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
 * Summary  : Represents channel indexes that define limits of another channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents channel indexes that define limits of another channel.
    /// <para>Представляет индексы каналов, которые определяют границы другого канала.</para>
    /// </summary>
    internal class LimCnlIndex
    {
        public int LoLo { get; set; }

        public int Low { get; set; }

        public int High { get; set; }

        public int HiHi { get; set; }

        public int Deadband { get; set; }
    }
}
