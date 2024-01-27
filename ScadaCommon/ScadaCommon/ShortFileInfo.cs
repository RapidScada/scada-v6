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
 * Summary  : Represents information associated with a file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.IO;

namespace Scada
{
    /// <summary>
    /// Represents information associated with a file.
    /// <para>Представляет информацию, связанную с файлом.</para>
    /// </summary>
    public struct ShortFileInfo
    {
        /// <summary>
        /// Represents an information of nonexistent file.
        /// </summary>
        public static readonly ShortFileInfo FileNotExists = new ShortFileInfo
        {
            Exists = false,
            LastWriteTime = DateTime.MinValue,
            Length = 0
        };


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public ShortFileInfo(FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));

            Exists = fileInfo.Exists;
            LastWriteTime = fileInfo.Exists ? fileInfo.LastWriteTimeUtc : DateTime.MinValue;
            Length = fileInfo.Exists ? fileInfo.Length : 0;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the file exists.
        /// </summary>
        public bool Exists { get; set; }

        /// <summary>
        /// Gets or sets the time (UTC) when the file was last written to.
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public long Length { get; set; }
    }
}
