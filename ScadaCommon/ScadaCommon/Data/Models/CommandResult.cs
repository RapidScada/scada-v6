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
 * Summary  : Represents a telecontrol command result
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

namespace Scada.Data.Models
{
    /// <summary>
    /// Represents a telecontrol command result.
    /// <para>Представляет результат команды телеуправления.</para>
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandResult()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CommandResult(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
            TransmitToClients = true;
            ErrorMessage = "";
        }


        /// <summary>
        /// Gets or sets a value indicating whether the command is processed successfully.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to transmit the command to the connected clients.
        /// </summary>
        public bool TransmitToClients { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
