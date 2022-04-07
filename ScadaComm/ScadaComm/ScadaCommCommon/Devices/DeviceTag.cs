/*
 * Copyright 2022 Rapid Software LLC
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
 * Modified : 2022
 */

using Scada.Data.Entities;
using System;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a device tag.
    /// <para>Представляет тег устройства.</para>
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
            Code = code;
            Name = name;
            DataType = TagDataType.Double;
            DataLen = 1;
            DataIndex = -1;
            Format = null;
            Cnl = null;
            Aux = null;
        }


        /// <summary>
        /// Gets or sets the tag index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets the tag number.
        /// </summary>
        public int TagNum
        {
            get
            {
                return Index + 1;
            }
        }

        /// <summary>
        /// Gets or sets the tag code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tag data type.
        /// </summary>
        public TagDataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the number of data elements stored in the tag value.
        /// </summary>
        public int DataLen { get; set; }

        /// <summary>
        /// Gets the normalized data length.
        /// </summary>
        public int DataLength
        {
            get
            {
                return Math.Max(DataLen, 1);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tag represents an array of numbers.
        /// </summary>
        public bool IsNumericArray
        {
            get
            {
                return DataLen > 1 && (DataType == TagDataType.Double || DataType == TagDataType.Int64);
            }
        }

        /// <summary>
        /// Gets or sets the starting index of the raw tag data within a DeviceData instance.
        /// </summary>
        public int DataIndex { get; set; }

        /// <summary>
        /// Gets or sets the display format of the tag.
        /// </summary>
        public TagFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the channel bound to the tag.
        /// </summary>
        public Cnl Cnl { get; set; }

        /// <summary>
        /// Gets or sets the auxiliary object that contains data about the device tag.
        /// </summary>
        public object Aux { get; set; }


        /// <summary>
        /// Sets the tag data type.
        /// </summary>
        public DeviceTag SetDataType(TagDataType dataType)
        {
            DataType = dataType;
            return this;
        }

        /// <summary>
        /// Sets the tag data length.
        /// </summary>
        public DeviceTag SetDataLen(int dataLen)
        {
            DataLen = dataLen;
            return this;
        }

        /// <summary>
        /// Sets the tag format.
        /// </summary>
        public DeviceTag SetFormat(TagFormat format)
        {
            Format = format;
            return this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Code) ? TagNum.ToString() : Code;
        }
    }
}
