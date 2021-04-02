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
 * Module   : ScadaCommon
 * Summary  : Represents an archive mask
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an archive mask.
    /// <para>Представляет маску архивов.</para>
    /// </summary>
    public struct ArchiveMask
    {
        /// <summary>
        /// The bit that identifies the current data archive.
        /// </summary>
        public const int CurrentArchiveBit = 0;
        /// <summary>
        /// The bit that identifies the minute data archive.
        /// </summary>
        public const int MinuteArchiveBit = 1;
        /// <summary>
        /// The bit that identifies the hourly data archive.
        /// </summary>
        public const int HourlyArchiveBit = 2;
        /// <summary>
        /// The bit that identifies the daily data archive.
        /// </summary>
        public const int DailyArchiveBit = 3;
        /// <summary>
        /// The bit that identifies the monthly data archive.
        /// </summary>
        public const int MonthlyArchiveBit = 4;
        /// <summary>
        /// The bit that identifies the event archive.
        /// </summary>
        public const int EventArchiveBit = 5;

        /// <summary>
        /// The mask indicating to select default archives.
        /// </summary>
        public const int Default = -1;
        /// <summary>
        /// The mask indicating to select all archives.
        /// </summary>
        public const int All = 0x7FFF_FFFF;


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public ArchiveMask(int? value)
        {
            Value = value ?? 0;
        }


        /// <summary>
        /// Gets the mask value.
        /// </summary>
        public int Value { get; private set; }
    }
}
