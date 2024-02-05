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
 * Summary  : Represents an archive mask
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Const;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an archive mask.
    /// <para>Представляет маску архивов.</para>
    /// </summary>
    public struct ArchiveMask
    {
        /// <summary>
        /// The mask indicating that none of the archives is selected.
        /// </summary>
        public const int None = 0;
        /// <summary>
        /// The mask indicating to select the default archives.
        /// </summary>
        public const int Default = -1;
        /// <summary>
        /// The mask indicating to select all the archives.
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

        /// <summary>
        /// Gets or sets a value indicating whether the channel is stored in the current archive.
        /// </summary>
        public bool Current
        {
            get
            {
                return Value.BitIsSet(ArchiveBit.Current);
            }
            set
            {
                Value = Value.SetBit(ArchiveBit.Current, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the channel is stored in the minute archive.
        /// </summary>
        public bool Minute
        {
            get
            {
                return Value.BitIsSet(ArchiveBit.Minute);
            }
            set
            {
                Value = Value.SetBit(ArchiveBit.Minute, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the channel is stored in the hourly archive.
        /// </summary>
        public bool Hourly
        {
            get
            {
                return Value.BitIsSet(ArchiveBit.Hourly);
            }
            set
            {
                Value = Value.SetBit(ArchiveBit.Hourly, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the channel is stored in the daily archive.
        /// </summary>
        public bool Daily
        {
            get
            {
                return Value.BitIsSet(ArchiveBit.Daily);
            }
            set
            {
                Value = Value.SetBit(ArchiveBit.Daily, value);
            }
        }
    }
}
