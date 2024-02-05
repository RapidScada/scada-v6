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
 * Module   : ScadaCommCommon
 * Summary  : Represents a channel prototype
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Entities;
using System;

namespace Scada.Comm.Devices
{
    /// <summary>
    /// Represents a channel prototype.
    /// <para>Представляет прототип канала.</para>
    /// </summary>
    public class CnlPrototype : Cnl
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlPrototype()
        {
            Active = true;
            CnlTypeID = Data.Const.CnlTypeID.Input;
        }


        /// <summary>
        /// Gets or sets the channel format code.
        /// </summary>
        public string FormatCode { get; set; }

        /// <summary>
        /// Gets or sets the channel format code for commands.
        /// </summary>
        public string OutFormatCode { get; set; }

        /// <summary>
        /// Gets or sets the channel quantity code.
        /// </summary>
        public string QuantityCode { get; set; }

        /// <summary>
        /// Gets or sets the channel unit code.
        /// </summary>
        public string UnitCode { get; set; }


        /// <summary>
        /// Gets a tag format by the format code of the channel prototype.
        /// </summary>
        private TagFormat GetTagFormat()
        {
            if (string.IsNullOrEmpty(FormatCode))
                return null;

            switch (FormatCode)
            {
                case Data.Const.FormatCode.N0:
                    return TagFormat.IntNumber;

                case Data.Const.FormatCode.X:
                case Data.Const.FormatCode.X2:
                case Data.Const.FormatCode.X4:
                case Data.Const.FormatCode.X8:
                    return TagFormat.HexNumber;

                case Data.Const.FormatCode.DateTime:
                case Data.Const.FormatCode.Date:
                case Data.Const.FormatCode.Time:
                    return TagFormat.DateTime;

                case Data.Const.FormatCode.String:
                    return TagFormat.String;

                case Data.Const.FormatCode.OffOn:
                    return TagFormat.OffOn;

                default: 
                    return null;
            }
        }

        /// <summary>
        /// Sets the channel format code.
        /// </summary>
        public CnlPrototype SetFormat(string formatCode)
        {
            FormatCode = formatCode;
            return this;
        }

        /// <summary>
        /// Executes the specified action that configures the channel prototype properties.
        /// </summary>
        public CnlPrototype Configure(Action<CnlPrototype> action)
        {
            action?.Invoke(this);
            return this;
        }

        /// <summary>
        /// Converts the channel prototype to a device tag.
        /// </summary>
        public DeviceTag ToDeviceTag()
        {
            return new DeviceTag(TagCode, Name)
            {
                DataType = (TagDataType)(DataTypeID ?? 0),
                DataLen = DataLen ?? 1,
                Format = GetTagFormat()
            };
        }
    }
}
