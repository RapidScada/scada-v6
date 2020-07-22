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
 * Summary  : Represents a data packet of the application protocol
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Protocol
{
    /// <summary>
    /// Represents a data packet of the application protocol.
    /// <para>Представляет пакет данных протокола приложения.</para>
    /// </summary>
    public class DataPacket
    {
        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        public ushort TransactionID { get; set; }

        /// <summary>
        /// Gets or sets the length of following bytes.
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// Gets or sets the length of the function arguments.
        /// </summary>
        public int ArgumentLength
        {
            get
            {
                return DataLength - 10;
            }
            set
            {
                DataLength = value + 10;
            }
        }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        public long SessionID { get; set; }

        /// <summary>
        /// Gets or sets the function ID.
        /// </summary>
        public ushort FunctionID { get; set; }

        /// <summary>
        /// Gets or sets the data buffer.
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// Gets or sets the used buffer length.
        /// </summary>
        public int BufferLength
        {
            get
            {
                return DataLength + 6;
            }
            set
            {
                DataLength = value - 6;
            }
        }


        /// <summary>
        /// Encodes the data packet properties and writes them to the buffer.
        /// </summary>
        public virtual void Encode()
        {
            if (Buffer != null)
            {
                ProtocolUtils.CopyUInt16(TransactionID, Buffer, 0, out int index);
                ProtocolUtils.CopyInt32(DataLength, Buffer, index, out index);
                ProtocolUtils.CopyInt64(SessionID, Buffer, index, out index);
                ProtocolUtils.CopyUInt16(FunctionID, Buffer, index, out index);
            }
        }
    }
}
