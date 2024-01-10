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
 * Module   : ScadaAdminCommon
 * Summary  : Provides data for an extension message event
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using System;
using System.Collections.Generic;

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Provides data for an extension message event.
    /// <para>Предоставляет данные для события сообщения расширению.</para>
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MessageEventArgs()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MessageEventArgs(string message)
            : this(null, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MessageEventArgs(object source, string message)
            : this(source, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MessageEventArgs(object source, string message, IDictionary<string, object> arguments)
            : base()
        {
            Source = source;
            Message = message;
            Arguments = arguments;
            Outputs = null;
            Result = null;
        }


        /// <summary>
        /// Gets the object initiated the event.
        /// </summary>
        public object Source { get; init; }

        /// <summary>
        /// Gets the message text or action name.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Gets the message arguments.
        /// </summary>
        public IDictionary<string, object> Arguments { get; init; }

        /// <summary>
        /// Gets or sets the output parameters set by a recipient.
        /// </summary>
        public IDictionary<string, object> Outputs { get; set; }

        /// <summary>
        /// Gets or sets the result set by a recipient.
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Gets the result as a boolean value.
        /// </summary>
        public bool BoolResult => Result is bool boolResult ? boolResult : Result != null;
    }
}
