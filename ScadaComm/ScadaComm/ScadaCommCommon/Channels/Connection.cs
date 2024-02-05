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
 * Summary  : Represents the base class for device connection
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2023
 */

using Scada.Comm.Lang;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scada.Comm.Channels
{
    /// <summary>
    /// Represents the base class for device connection.
    /// <para>Представляет базовый класс соединения с устройством.</para>
    /// </summary>
    public abstract class Connection
    {
        /// <summary>
        /// The default timeout for read operations, ms.
        /// </summary>
        protected const int DefaultReadTimeout = 5000;
        /// <summary>
        /// The default timeout for write operations, ms.
        /// </summary>
        protected const int DefaultWriteTimeout = 5000;
        /// <summary>
        /// The timeout for reading one byte, ms.
        /// </summary>
        protected const int OneByteReadTimeout = 10;
        /// <summary>
        /// The delay for data accumulation in the internal connection buffer, ms.
        /// </summary>
        protected const int DataAccumDelay = 10;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Connection(ILog log)
        {
            Log = log;
            ProtocolFormat = ProtocolFormat.Hex;
            RemoteAddress = "";
            NewLine = Environment.NewLine;
            Encoding = Encoding.ASCII;
        }


        /// <summary>
        /// Gets an object that can be used to synchronize access to the connection.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the communication line log.
        /// </summary>
        protected ILog Log { get; }

        /// <summary>
        /// Gets or sets the format of communication protocol data packets.
        /// </summary>
        public ProtocolFormat ProtocolFormat { get; set; }
        
        /// <summary>
        /// Gets or sets the remote address of the connection.
        /// </summary>
        public string RemoteAddress { get; set; }

        /// <summary>
        /// Gets a value indicating whether the connection is established.
        /// </summary>
        public abstract bool Connected { get; }

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        public virtual Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the end of a line in text mode.
        /// </summary>
        public virtual string NewLine { get; set; }


        /// <summary>
        /// Reads data.
        /// </summary>
        /// <returns>The number of bytes read.</returns>
        public abstract int Read(byte[] buffer, int offset, int count, int timeout, 
            ProtocolFormat format, out string logText);

        /// <summary>
        /// Reads and logs data.
        /// </summary>
        public virtual int Read(byte[] buffer, int offset, int count, int timeout)
        {
            int readCnt = Read(buffer, offset, count, timeout, ProtocolFormat, out string logText);
            Log?.WriteLine(logText);
            return readCnt;
        }

        /// <summary>
        /// Reads data with the stop condition.
        /// </summary>
        public abstract int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived, ProtocolFormat format, out string logText);

        /// <summary>
        /// Reads and logs data with the stop condition.
        /// </summary>
        public virtual int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived)
        {
            int readCnt = Read(buffer, offset, maxCount, timeout, stopCond, 
                out stopReceived, ProtocolFormat, out string logText);
            Log?.WriteLine(logText);
            return readCnt;
        }

        /// <summary>
        /// Reads lines.
        /// </summary>
        public abstract List<string> ReadLines(int timeout, TextStopCondition stopCond, 
            out bool stopReceived, out string logText);

        /// <summary>
        /// Reads and logs lines.
        /// </summary>
        public virtual List<string> ReadLines(int timeout, TextStopCondition stopCond, out bool stopReceived)
        {
            List<string> lines = ReadLines(timeout, stopCond, out stopReceived, out string logText);
            Log?.WriteLine(logText);
            return lines;
        }

        /// <summary>
        /// Reads one line.
        /// </summary>
        public virtual string ReadLine(int timeout, out string logText)
        {
            List<string> lines = ReadLines(timeout, TextStopCondition.OneLine, out _, out logText);
            return lines.Count > 0 ? lines[0] : null;
        }

        /// <summary>
        /// Reads and logs one line.
        /// </summary>
        public virtual string ReadLine(int timeout)
        {
            List<string> lines = ReadLines(timeout, TextStopCondition.OneLine, out _, out string logText);
            Log?.WriteLine(logText);
            return lines.Count > 0 ? lines[0] : null;
        }

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        public abstract void Write(byte[] buffer, int offset, int count, ProtocolFormat format, out string logText);

        /// <summary>
        /// Writes and logs the specified data.
        /// </summary>
        public virtual void Write(byte[] buffer, int offset, int count)
        {
            Write(buffer, offset, count, ProtocolFormat, out string logText);
            Log?.WriteLine(logText);
        }

        /// <summary>
        /// Writes the specified line.
        /// </summary>
        public abstract void WriteLine(string text, out string logText);

        /// <summary>
        /// Writes and logs the specified line.
        /// </summary>
        public virtual void WriteLine(string text)
        {
            WriteLine(text, out string logText);
            Log?.WriteLine(logText);
        }

        /// <summary>
        /// Builds a log text about reading data.
        /// </summary>
        public static string BuildReadLogText(byte[] buffer, int offset, int count, int readCnt, ProtocolFormat format)
        {
            return $"{CommPhrases.ReceiveNotation} ({readCnt}/{count}): " +
                ScadaUtils.BytesToString(buffer, offset, readCnt, format == ProtocolFormat.Hex);
        }

        /// <summary>
        /// Builds a log text about reading data.
        /// </summary>
        public static string BuildReadLogText(byte[] buffer, int offset, int readCnt, ProtocolFormat format)
        {
            return $"{CommPhrases.ReceiveNotation} ({readCnt}): " +
                ScadaUtils.BytesToString(buffer, offset, readCnt, format == ProtocolFormat.Hex);
        }

        /// <summary>
        /// Builds a log text about reading lines.
        /// </summary>
        public static string BuildReadLinesLogText(List<string> lines)
        {
            StringBuilder sbLines = new StringBuilder(CommPhrases.ReceiveNotation);
            sbLines.Append(": ");
            int lineCnt = lines.Count;

            if (lineCnt > 0)
            {
                for (int i = 0; i < lineCnt; i++)
                {
                    if (i > 0)
                        sbLines.AppendLine();

                    sbLines.Append(lines[i].Trim(CommUtils.NewLineChars));
                }
            }
            else
            {
                sbLines.Append(Locale.IsRussian ? "нет данных" : "no data");
            }

            return sbLines.ToString();
        }

        /// <summary>
        /// Builds a log text about writing data.
        /// </summary>
        public static string BuildWriteLogText(byte[] buffer, int offset, int count, ProtocolFormat format)
        {
            return $"{CommPhrases.SendNotation} ({count}): " +
                ScadaUtils.BytesToString(buffer, offset, count, format == ProtocolFormat.Hex);
        }
    }
}
