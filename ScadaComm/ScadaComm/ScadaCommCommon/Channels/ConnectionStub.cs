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
 * Module   : ScadaCommCommon
 * Summary  : Represents a connection stub
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System.Collections.Generic;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents a connection stub.
    /// <para>Представляет заглушку соединения.</para>
    /// </summary>
    public class ConnectionStub : Connection
    {
        /// <summary>
        /// The connection stub instance.
        /// </summary>
        public static readonly ConnectionStub Instance = new ConnectionStub();


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ConnectionStub()
            : base(null)
        {
        }


        /// <summary>
        /// Gets a value indicating whether the connection is established.
        /// </summary>
        public override bool Connected
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Reads data.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count, int timeout, 
            ProtocolFormat format, out string logText)
        {
            logText = "";
            return 0;
        }

        /// <summary>
        /// Reads data with the stop condition.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived, ProtocolFormat format, out string logText)
        {
            stopReceived = false;
            logText = "";
            return 0;
        }

        /// <summary>
        /// Reads lines.
        /// </summary>
        public override List<string> ReadLines(int timeout, TextStopCondition stopCond,
            out bool stopReceived, out string logText)
        {
            stopReceived = false;
            logText = "";
            return new List<string>();
        }

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count, ProtocolFormat format, out string logText)
        {
            logText = "";
        }

        /// <summary>
        /// Writes the specified line.
        /// </summary>
        public override void WriteLine(string text, out string logText)
        {
            logText = "";
        }
    }
}
