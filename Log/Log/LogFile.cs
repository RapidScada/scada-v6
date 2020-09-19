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
 * Module   : Log
 * Summary  : Represents a log file
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2020
 */

using System;
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
        /// The default log file capacity, 1 MB.
        /// </summary>
        public const int DefaultCapacity = 1048576;
        /// <summary>
        /// The timestamp format.
        /// </summary>
        public const string DefaultTimestampFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

        /// <summary>
        /// The log divider.
        /// </summary>
        private static readonly string Divider = new string('-', 80);
        /// <summary>
        /// The action type names.
        /// </summary>
        private static readonly string[] ActTypeNames = { "INF", "ACT", "ERR", "EXC" };

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
        /// Gets or sets the log capacity.
        /// </summary>
        public int Capacity { get; set; }

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
        /// Writes action of the specified type to the log.
        /// </summary>
        public void WriteAction(string text, LogActType actType)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToString(TimestampFormat));

            if (logFormat == LogFormat.Simple)
            {
                WriteLine(sb.Append(" ").Append(text).ToString());
            }
            else
            {
                WriteLine(sb.Append(" <")
                    .Append(CompName).Append("><")
                    .Append(Username).Append("><")
                    .Append(ActTypeNames[(int)actType]).Append("> ")
                    .Append(text).ToString());
            }
        }

        /// <summary>
        /// Writes the informational message to the log.
        /// </summary>
        public void WriteInfo(string text)
        {
            WriteAction(text, LogActType.Info);
        }

        /// <summary>
        /// Writes the action to the log.
        /// </summary>
        public void WriteAction(string text)
        {
            WriteAction(text, LogActType.Action);
        }

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        public void WriteError(string text)
        {
            WriteAction(text, LogActType.Error);
        }

        /// <summary>
        /// Writes the exception to the log.
        /// </summary>
        public void WriteException(Exception ex, string errMsg = "", params object[] args)
        {
            if (ex == null)
            {
                WriteAction(args == null || args.Length == 0 ? errMsg : string.Format(errMsg, args), 
                    LogActType.Exception);
            }
            else if (string.IsNullOrEmpty(errMsg))
            {
                WriteAction(ex.ToString(), LogActType.Exception);
            }
            else
            {
                WriteAction(new StringBuilder()
                    .Append(args == null || args.Length == 0 ? errMsg : string.Format(errMsg, args))
                    .AppendLine(":")
                    .Append(ex.ToString()).ToString(),
                    LogActType.Exception);
            }
        }

        /// <summary>
        /// Writes the specified line to the log.
        /// </summary>
        public void WriteLine(string text = "")
        {
            try
            {
                lock (writeLock)
                {
                    // check file size
                    FileInfo fileInfo = new FileInfo(FileName);

                    if (fileInfo.Exists && fileInfo.Length > Capacity)
                    {
                        // rename the file
                        string bakFileName = FileName + ".bak";
                        File.Delete(bakFileName);
                        File.Move(FileName, bakFileName);
                    }

                    // write text
                    using (StreamWriter writer = new StreamWriter(FileName, true, Encoding))
                    {
                        writer.WriteLine(text);
                        writer.Flush();
                    }
                }
            }
            catch
            {
                // do nothing
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
