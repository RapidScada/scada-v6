/*
 * Copyright 2023 Rapid Software LLC
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
 * Summary  : Specifies the data type IDs
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace Scada.Data.Const
{
    /// <summary>
    /// Specifies the channel status IDs.
    /// <para>Задаёт идентификаторы статусов каналов.</para>
    /// </summary>
    public static class CnlStatusID
    {
        public const int Undefined = 0;
        public const int Defined = 1;
        public const int Archival = 2;
        public const int FormulaError = 3;
        public const int Unreliable = 4;
        public const int LoLo = 11;
        public const int Low = 12;
        public const int Normal = 13;
        public const int High = 14;
        public const int HiHi = 15;
        public const int Good = 21;
        public const int Warning = 22;
        public const int Error = 23;
    }
}
