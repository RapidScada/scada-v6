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
 * Summary  : Represents errors that occur during communication between applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;

namespace Scada.Protocol
{
    /// <summary>
    /// Represents errors that occur during communication between applications.
    /// <para>Представляет ошибки, возникающие во время обмена данными между приложениями.</para>
    /// </summary>
    public class ProtocolException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProtocolException(ErrorCode errorCode)
            : base(GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProtocolException(ErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }


        /// <summary>
        /// Gets the error code.
        /// </summary>
        public ErrorCode ErrorCode { get; protected set; }


        /// <summary>
        /// Gets an error message according to the specified error code.
        /// </summary>
        private static string GetErrorMessage(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.IllegalFunction:
                    return errorCode + ": Function not supported.";

                case ErrorCode.IllegalFunctionArguments:
                    return errorCode + ": Some function arguments are not valid.";

                case ErrorCode.AccessDenied:
                    return errorCode + ": Function is not available due to security reasons.";

                case ErrorCode.InvalidOperation:
                    return errorCode + ": Function call is invalid for the current state of the client or server.";

                case ErrorCode.OperationCanceled:
                    return errorCode + ": Operation has been canceled.";

                case ErrorCode.ProxyError:
                    return errorCode + ": Error processing request by a proxy.";

                case ErrorCode.InternalServerError:
                    return errorCode + ": Error while processing the operation.";

                default:
                    return errorCode.ToString();
            }
        }
    }
}
