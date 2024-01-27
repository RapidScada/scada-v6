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
 * Module   : ScadaCommon.Log
 * Summary  : Represents a log file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2021
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Scada.Log
{
    /// <summary>
    /// Represents a log file.
    /// <para>Представляет файл журнала.</para>
    /// </summary>
    public class LogFile : ILog
    {
        /// <summary>
        /// The number of bytes in a megabyte.
        /// </summary>
        private const int BytesInMegabyte = 1048576;
        /// <summary>
        /// The default log file capacity in megabytes.
        /// </summary>
        public const int DefaultCapacityMB = 1;
        /// <summary>
        /// The default log file capacity in bytes.
        /// </summary>
        public const int DefaultCapacity = DefaultCapacityMB * BytesInMegabyte;
        /// <summary>
        /// The timestamp format.
        /// </summary>
        public const string DefaultTimestampFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

        /// <summary>
        /// The log divider.
        /// </summary>
        private static readonly string Divider = new string('-', 80);
        /// <summary>
        /// The message type names.
        /// </summary>
        private static readonly string[] MessageTypeNames = { "INF", "ACT", "WARN", "ERR" };

        private readonly LogFormat logFormat; // the log format
        private readonly object writeLock;    // synchronizes writing to the log file


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LogFile(LogFormat logFormat, string fileName = "")
        {
            this.logFormat = logFormat;
            writeLock = new object();

            FileName = fileName;
            Encoding = Encoding.UTF8;
            Capacity = DefaultCapacity;
            TimestampFormat = DefaultTimestampFormat;
            CompName = Environment.MachineName;
            Username = Environment.UserName;
        }


        /// <summary>
        /// Gets or sets the log file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the log encoding.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the log capacity in bytes.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets the log capacity in megabytes.
        /// </summary>
        public int CapacityMB
        {
            get
            {
                return Capacity / BytesInMegabyte;
            }
            set
            {
                Capacity = value * BytesInMegabyte;
            }
        }

        /// <summary>
        /// Gets or sets the timestamp format.
        /// </summary>
        public string TimestampFormat { get; set; }

        /// <summary>
        /// Gets the computer name.
        /// </summary>
        public string CompName { get; private set; }

        /// <summary>
        /// Gets the current user name.
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Formats the specified text with the specified arguments.
        /// </summary>
        private string FormatText(string text, object[] args)
        {
            return args == null || args.Length == 0 ? text : string.Format(text, args);
        }

        /// <summary>
        /// Writes the message of the specified type to the log.
        /// </summary>
        public void WriteMessage(string text, LogMessageType messageType)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToString(TimestampFormat));

            if (logFormat == LogFormat.Simple)
            {
                WriteLine(sb.Append(" ").Append(text).ToString());
            }
            else
            {
                WriteLine(sb.Append(" [")
                    .Append(CompName).Append("][")
                    .Append(Username).Append("][")
                    .Append(MessageTypeNames[(int)messageType]).Append("] ")
                    .Append(text).ToString());
            }
        }

        /// <summary>
        /// Writes the informational message to the log.
        /// </summary>
        public void WriteInfo(string text, params object[] args)
        {
            WriteMessage(FormatText(text, args), LogMessageType.Info);
        }

        /// <summary>
        /// Writes the action to the log.
        /// </summary>
        public void WriteAction(string text, params object[] args)
        {
            WriteMessage(FormatText(text, args), LogMessageType.Action);
        }

        /// <summary>
        /// Writes the warning message to the log.
        /// </summary>
        public void WriteWarning(string text, params object[] args)
        {
            WriteMessage(FormatText(text, args), LogMessageType.Warning);
        }

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        public void WriteError(string text, params object[] args)
        {
            WriteMessage(FormatText(text, args), LogMessageType.Error);
        }

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        public void WriteError(Exception ex, string text = "", params object[] args)
        {
            if (ex == null)
            {
                WriteError(text, args);
            }
            else if (string.IsNullOrEmpty(text))
            {
                WriteMessage(ex.ToString(), LogMessageType.Error);
            }
            else
            {
                WriteMessage(FormatText(text, args) + ":" + Environment.NewLine + ex, LogMessageType.Error);
            }
        }

        /// <summary>
        /// Writes the specified line to the log.
        /// </summary>
        public void WriteLine(string text = "", params object[] args)
        {
            try
            {
                lock (writeLock)
                {
                    // rotate log file
                    if (Capacity > 0)
                    {
                        FileInfo fileInfo = new FileInfo(FileName);

                        if (fileInfo.Exists && fileInfo.Length > Capacity)
                        {
                            string bakFileName = FileName + ".bak";
                            File.Delete(bakFileName);
                            File.Move(FileName, bakFileName);
                        }
                    }

                    // write text
                    using (StreamWriter writer = new StreamWriter(FileName, true, Encoding))
                    {
                        writer.WriteLine(FormatText(text, args));
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Writes a divider to the log.
        /// </summary>
        public void WriteBreak()
        {
            WriteLine(Divider);
        }
    }
}
