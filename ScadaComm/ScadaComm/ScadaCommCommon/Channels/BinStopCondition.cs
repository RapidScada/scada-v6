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
 * Module   : ScadaCommEngine
 * Summary  : Represents a condition to stop reading data in a binary format
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents a condition to stop reading data in a binary format.
    /// <para>Представляет условие остановки считывания данных в бинарном формате.</para>
    /// </summary>
    public class BinStopCondition
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected BinStopCondition()
        {
            StopCode = 0;
            StopSeq = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BinStopCondition(byte stopCode)
        {
            StopCode = stopCode;
            StopSeq = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public BinStopCondition(byte[] stopSeq)
        {
            StopCode = 0;
            StopSeq = stopSeq ?? throw new ArgumentNullException(nameof(stopSeq));
        }


        /// <summary>
        /// Gets the byte that stops reading.
        /// </summary>
        public byte StopCode { get; protected set; }

        /// <summary>
        /// Gets the sequence of bytes that stops reading.
        /// </summary>
        public byte[] StopSeq { get; protected set; }


        /// <summary>
        /// Checks if the stop condition is satisfied.
        /// </summary>
        public virtual bool CheckCondition(byte[] buffer, int index)
        {
            if (StopSeq == null)
            {
                return buffer[index] == StopCode;
            }
            else
            {
                for (int i = index, j = StopSeq.Length - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (buffer[i] != StopSeq[j])
                        return false;
                }

                return true;
            }
        }
    }
}
