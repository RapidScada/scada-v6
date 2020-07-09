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
 * Module   : ScadaData
 * Summary  : Represents a mechanism to read data from a source and write data to the source
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System.IO;

namespace Scada.Data.Adapters
{
    /// <summary>
    /// Represents a mechanism to read data from a source and write data to the source.
    /// <para>Представляет механизм для чтения данных из источника и записи данных в источник.</para>
    /// </summary>
    public abstract class Adapter
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Adapter()
        {
            FileName = "";
            Stream = null;
        }


        /// <summary>
        /// Gets or sets the table file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the input and output stream.
        /// </summary>
        public Stream Stream { get; set; }
    }
}
