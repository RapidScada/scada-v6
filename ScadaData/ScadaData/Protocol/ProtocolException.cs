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
 * Summary  : Represents errors that occur during communication between applications
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
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
            : base()
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
    }
}
