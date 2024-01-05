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
 * Summary  : Represents a response packet of the application protocol
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;

namespace Scada.Protocol
{
    /// <summary>
    /// Represents a response packet of the application protocol.
    /// <para>Представляет пакет ответа протокола приложения.</para>
    /// </summary>
    public class ResponsePacket : DataPacket
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponsePacket(DataPacket request, byte[] buffer)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            TransactionID = request.TransactionID;
            DataLength = 10; // no arguments
            SessionID = request.SessionID;
            FunctionID = request.FunctionID;
            Buffer = buffer;
            ErrorCode = ErrorCode.NoError;
        }


        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public ErrorCode ErrorCode { get; set; }


        /// <summary>
        /// Sets the error code and updates the buffer data.
        /// </summary>
        public void SetError(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
            ArgumentLength = 1;
            Encode();
        }

        /// <summary>
        /// Encodes the response properties and writes them to the buffer.
        /// </summary>
        public override void Encode()
        {
            base.Encode();

            if (Buffer != null && ErrorCode > ErrorCode.NoError)
            {
                BinaryConverter.CopyInt32(FunctionID | 0x8000, Buffer, 14);
                Buffer[ProtocolUtils.ArgumentIndex] = (byte)ErrorCode;
            }
        }
    }
}
