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
 * Module   : ScadaCommCommon
 * Summary  : Represents a device tag
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a device tag.
    /// <para>Представляет тег КП.</para>
    /// </summary>
    public class DeviceTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTag()
            : this("", "")
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DeviceTag(string code, string name)
        {
            Index = -1;
            TagNum = 0;
            Code = code;
            Name = name;
            CnlNum = 0;
            DataLen = 1;
            Aux = null;
        }


        /// <summary>
        /// Gets or sets the tag index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the tag number.
        /// </summary>
        public int TagNum { get; set; }

        /// <summary>
        /// Gets or sets the tag code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the input channel number bound to the tag.
        /// </summary>
        public int CnlNum { get; set; }

        /// <summary>
        /// Gets or sets the number of data elements stored in the tag value.
        /// </summary>
        public int DataLen { get; set; }

        /// <summary>
        /// Gets or sets the auxiliary object that contains data about the device tag.
        /// </summary>
        public object Aux { get; set; }
    }
}
