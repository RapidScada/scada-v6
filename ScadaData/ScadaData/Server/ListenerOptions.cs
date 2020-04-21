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
 * Summary  : Represents listener options
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

namespace Scada.Server
{
    /// <summary>
    /// Represents listener options.
    /// <para>Представляет параметры прослушивателя.</para>
    /// </summary>
    public class ListenerOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ListenerOptions()
        {
            Port = 10000;
            Timeout = 10000;
            SecretKey = null;
        }


        /// <summary>
        /// Gets or sets the TCP port number to listen on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the send and receive timeout.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the secret key for password encryption.
        /// </summary>
        public byte[] SecretKey { get; set; }
    }
}
