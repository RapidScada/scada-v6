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
 * Summary  : Represents errors that occur during Rapid SCADA execution
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2020
 */

using System;

namespace Scada
{
    /// <summary>
    /// Represents errors that occur during Rapid SCADA execution.
    /// <para>Представляет ошибки, возникающие во время выполнения Rapid SCADA.</para>
    /// </summary>
    public class ScadaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }


        /// <summary>
        /// Gets or sets a value indicating whether an error message can be displayed to a user.
        /// </summary>
        public bool MessageIsPublic { get; set; } = false;
    }
}
