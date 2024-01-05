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
 * Summary  : Specifies the channel type IDs
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

namespace Scada.Data.Const
{
    /// <summary>
    /// Specifies the channel type IDs.
    /// <para>Задаёт идентификаторы типов каналов.</para>
    /// </summary>
    public static class CnlTypeID
    {
        /// <summary>
        /// Channel values are read from a device.
        /// </summary>
        public const int Input = 1;

        /// <summary>
        /// Channel values can be read and written.
        /// </summary>
        public const int InputOutput = 2;

        /// <summary>
        /// Channel values are calculated using a formula.
        /// </summary>
        public const int Calculated = 3;

        /// <summary>
        /// Channel values are calculated and can be written.
        /// </summary>
        public const int CalculatedOutput = 4;

        /// <summary>
        /// Channel defines a telecontrol command.
        /// </summary>
        public const int Output = 5;


        /// <summary>
        /// Determines whether the channel type relates to an input channel (not calculated).
        /// </summary>
        public static bool IsInput(int? cnlTypeID)
        {
            return cnlTypeID.HasValue && (cnlTypeID == Input || cnlTypeID == InputOutput);
        }

        /// <summary>
        /// Determines whether the channel type relates to an output channel.
        /// </summary>
        public static bool IsOutput(int? cnlTypeID)
        {
            return cnlTypeID.HasValue && 
                (cnlTypeID == InputOutput || cnlTypeID == CalculatedOutput || cnlTypeID == Output);
        }

        /// <summary>
        /// Determines whether the channel type relates to a calculated channel.
        /// </summary>
        public static bool IsCalculated(int? cnlTypeID)
        {
            return cnlTypeID.HasValue && (cnlTypeID == Calculated || cnlTypeID == CalculatedOutput);
        }

        /// <summary>
        /// Determines whether channels of the specified type can be written to an archive.
        /// </summary>
        public static bool IsArchivable(int? cnlTypeID)
        {
            return cnlTypeID.HasValue && (cnlTypeID == Input || cnlTypeID == InputOutput || 
                cnlTypeID == Calculated || cnlTypeID == CalculatedOutput);
        }

        /// <summary>
        /// Changes the channel type by removing the output function.
        /// </summary>
        public static int UnsetOutput(int cnlTypeID)
        {
            switch (cnlTypeID)
            {
                case Input:
                case InputOutput:
                    return Input;

                case Calculated:
                case CalculatedOutput:
                    return Calculated;

                default:
                    throw new ScadaException("Unable to change channel type.");
            }
        }
    }
}
