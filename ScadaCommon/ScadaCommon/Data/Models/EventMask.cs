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
 * Summary  : Represents an event mask of an input channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an event mask of an input channel.
    /// <para>Представляет маску событий входного канала.</para>
    /// </summary>
    public struct EventMask
    {
        /// <summary>
        /// The bit indicating whether events are enabled for the channel.
        /// </summary>
        public const int EnabledBit = 0;
        /// <summary>
        /// The bit indicating whether a client application should play a beep on event.
        /// </summary>
        public const int BeepBit = 1;
        /// <summary>
        /// The bit indicating whether an event should be raised when channel data changes.
        /// </summary>
        public const int DataChangeBit = 2;
        /// <summary>
        /// The bit indicating whether an event should be raised when channel value changes.
        /// </summary>
        public const int ValueChangeBit = 3;
        /// <summary>
        /// The bit indicating whether an event should be raised when channel status changes.
        /// </summary>
        public const int StatusChangeBit = 4;
        /// <summary>
        /// The bit indicating whether an event should be raised when the channel becomes undefined, or vice versa.
        /// </summary>
        public const int CnlUndefinedBit = 5;


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public EventMask(int? value)
        {
            Value = value ?? 0;
        }


        /// <summary>
        /// Gets the mask value.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether events are enabled for the channel.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return Value.BitIsSet(EnabledBit);
            }
            set
            {
                Value = Value.SetBit(EnabledBit, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a client application should play a beep on event.
        /// </summary>
        public bool Beep
        {
            get
            {
                return Value.BitIsSet(BeepBit);
            }
            set
            {
                Value = Value.SetBit(BeepBit, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when channel data changes.
        /// </summary>
        public bool DataChange
        {
            get
            {
                return Value.BitIsSet(DataChangeBit);
            }
            set
            {
                Value = Value.SetBit(DataChangeBit, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when channel value changes.
        /// </summary>
        public bool ValueChange
        {
            get
            {
                return Value.BitIsSet(ValueChangeBit);
            }
            set
            {
                Value = Value.SetBit(ValueChangeBit, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when channel status changes.
        /// </summary>
        public bool StatusChange
        {
            get
            {
                return Value.BitIsSet(StatusChangeBit);
            }
            set
            {
                Value = Value.SetBit(StatusChangeBit, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when the channel becomes undefined, or vice versa.
        /// </summary>
        public bool CnlUndefined
        {
            get
            {
                return Value.BitIsSet(CnlUndefinedBit);
            }
            set
            {
                Value = Value.SetBit(CnlUndefinedBit, value);
            }
        }
    }
}
