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
 * Summary  : Specifies the results of reading from a file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the results of reading from a file.
    /// <para>Задаёт результаты чтения из файла.</para>
    /// </summary>
    public enum FileReadingResult : byte
    {
        /// <summary>
        /// Reading completed successfully.
        /// </summary>
        Completed = 0,

        /// <summary>
        /// Data block has been read successfully.
        /// </summary>
        BlockRead = 1,

        /// <summary>
        /// Requested file not found.
        /// </summary>
        FileNotFound = 2,

        /// <summary>
        /// File write time is less than requested.
        /// </summary>
        FileOutdated = 3
    }
}
