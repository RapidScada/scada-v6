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
 * Summary  : Provides extensions for entities of the configuration database
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Data.Const;
using System;

namespace Scada.Data.Entities
{
    /// <summary>
    /// Provides extensions for entities of the configuration database.
    /// <para>Предоставляет расширения для сущностей базы конфигурации.</para>
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Determines whether the channel data type is numeric.
        /// </summary>
        public static bool IsNumeric(this Cnl cnl)
        {
            return DataTypeID.IsNumeric(cnl.DataTypeID);
        }

        /// <summary>
        /// Determines whether the channel data type is string.
        /// </summary>
        public static bool IsString(this Cnl cnl)
        {
            return DataTypeID.IsString(cnl.DataTypeID);
        }

        /// <summary>
        /// Determines whether the channel represents an array or string.
        /// </summary>
        public static bool IsArray(this Cnl cnl)
        {
            return cnl.DataLen > 1;
        }

        /// <summary>
        /// Determines whether the channel represents an array of numbers.
        /// </summary>
        public static bool IsNumericArray(this Cnl cnl)
        {
            return cnl.DataLen > 1 && DataTypeID.IsNumeric(cnl.DataTypeID);
        }

        /// <summary>
        /// Determines whether the channel type relates to an input channel (not calculated).
        /// </summary>
        public static bool IsInput(this Cnl cnl)
        {
            return CnlTypeID.IsInput(cnl.CnlTypeID);
        }

        /// <summary>
        /// Determines whether the channel type relates to an output channel.
        /// </summary>
        public static bool IsOutput(this Cnl cnl)
        {
            return CnlTypeID.IsOutput(cnl.CnlTypeID);
        }

        /// <summary>
        /// Determines whether the channel type relates to a calculated channel.
        /// </summary>
        public static bool IsCalculated(this Cnl cnl)
        {
            return CnlTypeID.IsCalculated(cnl.CnlTypeID);
        }

        /// <summary>
        /// Determines whether the channel can be written to an archive, depending on its type.
        /// </summary>
        public static bool IsArchivable(this Cnl cnl)
        {
            return CnlTypeID.IsArchivable(cnl.CnlTypeID);
        }

        /// <summary>
        /// Gets the normalized data length of the specified channel.
        /// </summary>
        public static int GetDataLength(this Cnl cnl)
        {
            return cnl.DataLen.HasValue ? Math.Max(cnl.DataLen.Value, 1) : 1;
        }

        /// <summary>
        /// Gets the number of channels that should be joined to display a string.
        /// </summary>
        public static int GetJoinLength(this Cnl cnl)
        {
            return cnl.IsString() ? cnl.GetDataLength() : 1;
        }
    }
}
