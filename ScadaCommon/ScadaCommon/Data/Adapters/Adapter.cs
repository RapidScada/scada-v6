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
        /// Indicates the beginning of a new block in a file.
        /// </summary>
        protected const ushort BlockMarker = 0x0E0E;
        /// <summary>
        /// The maximum size of a reserve area in a file.
        /// </summary>
        protected const int MaxReserveSize = 100;
        /// <summary>
        /// The empty buffer.
        /// </summary>
        protected static byte[] EmptyBuffer = new byte[0];
        /// <summary>
        /// The buffer containing empty data.
        /// </summary>
        protected static byte[] ReserveBuffer = new byte[MaxReserveSize];


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


        /// <summary>
        /// Sets the buffer size to the specified value multiplied by the factor.
        /// </summary>
        protected void ResizeBuffer(ref byte[] buffer, int newSize, int factor = 1)
        {
            if (buffer == null || buffer.Length < newSize)
                buffer = new byte[newSize * factor];
        }

        /// <summary>
        /// Reads the specified number of bytes from the stream.
        /// </summary>
        public static int ReadData(BinaryReader reader, byte[] buffer, int index, int count, bool throwOnFail)
        {
            int bytesRead = reader.Read(buffer, index, count);

            if (throwOnFail && bytesRead < count)
                throw new ScadaException("Unexpected end of stream.");

            return bytesRead;
        }
    }
}
