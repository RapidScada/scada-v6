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
 * Summary  : Represents an event mask of a channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using Scada.Data.Const;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents an event mask of a channel.
    /// <para>Представляет маску событий канала.</para>
    /// </summary>
    public struct EventMask
    {
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
                return Value.BitIsSet(EventBit.Enabled);
            }
            set
            {
                Value = Value.SetBit(EventBit.Enabled, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a client application should play a beep on event.
        /// </summary>
        public bool Beep
        {
            get
            {
                return Value.BitIsSet(EventBit.Beep);
            }
            set
            {
                Value = Value.SetBit(EventBit.Beep, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when channel data changes.
        /// </summary>
        public bool DataChange
        {
            get
            {
                return Value.BitIsSet(EventBit.DataChange);
            }
            set
            {
                Value = Value.SetBit(EventBit.DataChange, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when channel value changes.
        /// </summary>
        public bool ValueChange
        {
            get
            {
                return Value.BitIsSet(EventBit.ValueChange);
            }
            set
            {
                Value = Value.SetBit(EventBit.ValueChange, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised when channel status changes.
        /// </summary>
        public bool StatusChange
        {
            get
            {
                return Value.BitIsSet(EventBit.StatusChange);
            }
            set
            {
                Value = Value.SetBit(EventBit.StatusChange, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised
        /// when the channel becomes undefined, or vice versa.
        /// </summary>
        public bool CnlUndefined
        {
            get
            {
                return Value.BitIsSet(EventBit.CnlUndefined);
            }
            set
            {
                Value = Value.SetBit(EventBit.CnlUndefined, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an event should be raised
        /// when a command defined by the channel has been sent.
        /// </summary>
        public bool Command
        {
            get
            {
                return Value.BitIsSet(EventBit.Command);
            }
            set
            {
                Value = Value.SetBit(EventBit.Command, value);
            }
        }
    }
}
