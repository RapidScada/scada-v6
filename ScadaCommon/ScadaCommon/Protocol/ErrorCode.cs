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
 * Summary  : Specifies the error codes
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Protocol
{
    /// <summary>
    /// Specifies the error codes.
    /// <para>Задаёт коды ошибок.</para>
    /// </summary>
    public enum ErrorCode : byte
    {
        /// <summary>
        /// No error occured.
        /// </summary>
        NoError = 0x00,

        /// <summary>
        /// Function not supported.
        /// </summary>
        IllegalFunction = 0x01,

        /// <summary>
        /// Some function arguments are not valid.
        /// </summary>
        IllegalFunctionArguments = 0x02,

        /// <summary>
        /// Function is not available due to security reasons.
        /// </summary>
        AccessDenied = 0x03,

        /// <summary>
        /// Function call is invalid for the current state of the client or server.
        /// </summary>
        InvalidOperation = 0x04,

        /// <summary>
        /// Operation has been canceled.
        /// </summary>
        OperationCanceled = 0x05,

        /// <summary>
        /// Error processing request by a proxy.
        /// </summary>
        ProxyError = 0x06,

        /// <summary>
        /// Error while processing the operation.
        /// </summary>
        InternalServerError = 0x07
    }
}
