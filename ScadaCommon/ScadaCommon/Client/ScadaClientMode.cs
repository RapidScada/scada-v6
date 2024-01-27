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
 * Summary  : Represents a client mode for interacting with the Server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2022
 * Modified : 2022
 */

namespace Scada.Client
{
    /// <summary>
    /// Represents a client mode for interacting with the Server service.
    /// <para>Представляет режим клиента для взаимодействия со службой Сервера.</para>
    /// </summary>
    public struct ScadaClientMode
    {
        /// <summary>
        /// The bit to enable incoming commands.
        /// </summary>
        private const int EnableIncomingCommandsBit = 0;

        /// <summary>
        /// The default client mode.
        /// </summary>
        public const int Default = 0;
        /// <summary>
        /// The client mode with enabled incoming commands.
        /// </summary>
        public const int IncomingCommands = 0x0000_0001;


        /// <summary>
        /// Initializes a new instance of the structure.
        /// </summary>
        public ScadaClientMode(int value)
        {
            Value = value;
        }


        /// <summary>
        /// Gets the client mode value.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether a client can receive commands from the server.
        /// </summary>
        public bool EnableIncomingCommands
        {
            get
            {
                return Value.BitIsSet(EnableIncomingCommandsBit);
            }
            set
            {
                Value = Value.SetBit(EnableIncomingCommandsBit, value);
            }
        }
    }
}
