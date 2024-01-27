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
 * Summary  : Provides data for the Progress event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;

namespace Scada.Client
{
    /// <summary>
    /// Provides data for the Progress event.
    /// <para>Предоставляет данные для события Progress.</para>
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProgressEventArgs(int blockNumber, int blockCount)
        {
            BlockNumber = blockNumber;
            BlockCount = blockCount;
        }


        /// <summary>
        /// Gets the block number.
        /// </summary>
        public int BlockNumber { get; protected set; }

        /// <summary>
        /// Gets the block count.
        /// </summary>
        public int BlockCount { get; protected set; }

        /// <summary>
        /// Gets the percentage of completion.
        /// </summary>
        public double Progress
        {
            get
            {
                return BlockCount > 0 ? 100.0 * BlockNumber / BlockCount : 0.0;
            }
        }
    }
}
