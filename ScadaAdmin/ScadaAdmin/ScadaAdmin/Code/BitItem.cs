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
 * Module   : Administrator
 * Summary  : Represents a bit mask item
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

namespace Scada.Admin.App.Code
{
    /// <summary>
    /// Represents a bit mask item.
    /// <para>Представляет элемент битовой маски.</para>
    /// </summary>
    internal class BitItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BitItem(int bit, string descr)
        {
            Bit = bit;
            Descr = descr;
        }


        /// <summary>
        /// Gets a bit number.
        /// </summary>
        public int Bit { get; }

        /// <summary>
        /// Gets a bit description.
        /// </summary>
        public string Descr { get; }


        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString() => Descr;
    }
}
