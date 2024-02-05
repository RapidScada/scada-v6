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
 * Summary  : Represents channel data
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Const;
using System;

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents channel data.
    /// <para>Представляет данные канала.</para>
    /// </summary>
    [Serializable]
    public struct CnlData
    {
        /// <summary>
        /// Specifies an empty channel data.
        /// </summary>
        public static readonly CnlData Empty = new CnlData(0.0, 0);
        /// <summary>
        /// Specifies a zero value with the defined status.
        /// </summary>
        public static readonly CnlData Zero = new CnlData(0.0, 1);


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CnlData(double val)
        {
            if (double.IsNaN(val))
            {
                Val = 0.0;
                Stat = CnlStatusID.Undefined;
            }
            else
            {
                Val = val;
                Stat = CnlStatusID.Defined;
            }
        }

        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public CnlData(double val, int stat)
        {
            Val = val;
            Stat = stat;
        }


        /// <summary>
        /// Gets or sets the channel value.
        /// </summary>
        public double Val { get; set; }

        /// <summary>
        /// Gets or sets the channel status.
        /// </summary>
        public int Stat { get; set; }

        /// <summary>
        /// Gets a value indicating whether the channel data has one of the defined statuses.
        /// </summary>
        public bool IsDefined
        {
            get
            {
                return Stat > CnlStatusID.Undefined;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the channel data has the undefined status.
        /// </summary>
        public bool IsUndefined
        {
            get
            {
                return Stat <= CnlStatusID.Undefined;
            }
        }


        /// <summary>
        /// Converts the channel data to a double-precision floating-point number.
        /// Returns the channel value if the channel data is defined, otherwise returns NaN.
        /// </summary>
        public double ToDouble()
        {
            return IsDefined ? Val : double.NaN;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is CnlData cnlData && this == cnlData;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Tuple.Create(Val, Stat).GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether two specified inctances are equal.
        /// </summary>
        public static bool operator ==(CnlData x, CnlData y)
        {
            // double.Equals() is needed for comparing double.NaN
            return x.Val.Equals(y.Val) && x.Stat == y.Stat;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified inctances are not equal.
        /// </summary>
        public static bool operator !=(CnlData x, CnlData y)
        {
            return !(x == y);
        }
    }
}
