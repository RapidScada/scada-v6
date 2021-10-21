// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Lang;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements the serial port connection.
    /// <para>Реализует соединение через последовательный порт.</para>
    /// </summary>
    internal class SerialPortConnection : Connection
    {
        /// <summary>
        /// The port reopening interval.
        /// </summary>
        protected static readonly TimeSpan ReopenAfter = TimeSpan.FromSeconds(5);

        protected DateTime openFailDT; // the time of unsuccessful attempt to open port


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SerialPortConnection(ILog log, SerialPort serialPort)
            : base(log)
        {
            openFailDT = DateTime.MinValue;

            SerialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
            SerialPort.WriteTimeout = DefaultWriteTimeout;
            WriteError = false;
        }


        /// <summary>
        /// Gets the serial port.
        /// </summary>
        public SerialPort SerialPort { get; }

        /// <summary>
        /// Gets a value indicating whether a port write error has occurred.
        /// </summary>
        public bool WriteError { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the connection is established.
        /// </summary>
        public override bool Connected
        {
            get
            {
                return SerialPort.IsOpen;
            }
        }

        /// <summary>
        /// Gets or sets the end of a line in text mode.
        /// </summary>
        public override string NewLine
        {
            get
            {
                return SerialPort == null ? Environment.NewLine : SerialPort.NewLine;
            }
            set
            {
                if (SerialPort != null)
                    SerialPort.NewLine = value;
            }
        }

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return SerialPort == null ? Encoding.ASCII : SerialPort.Encoding;
            }
            set
            {
                if (SerialPort != null)
                    SerialPort.Encoding = value;
            }
        }


        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void Open()
        {
            WriteError = false;

            if (DateTime.UtcNow - openFailDT >= ReopenAfter)
            {
                try
                {
                    SerialPort.Open();
                    openFailDT = DateTime.MinValue;
                }
                catch (Exception ex)
                {
                    // get the time again, because the Open method can take a long time
                    openFailDT = DateTime.UtcNow;
                    throw new ScadaException((Locale.IsRussian ?
                        "Ошибка при открытии последовательного порта: " :
                        "Error opening serial port: ") + ex.Message, ex);
                }
            }
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Попытка открытия последовательного порта может быть не ранее, чем через {0} с после предыдущей." :
                    "An attempt to open serial port can not be earlier than {0} seconds after the previous one.",
                    ReopenAfter);
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close()
        {
            try
            {
                SerialPort.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при закрытии последовательного порта" :
                    "Error closing serial port");
            }
        }

        /// <summary>
        /// Discards data from the serial driver's receive buffer.
        /// </summary>
        public void DiscardInBuffer()
        {
            try
            {
                SerialPort.DiscardInBuffer();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при сбросе входного буфера" :
                    "Error discarding input buffer");
            }
        }

        /// <summary>
        /// Reads data.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count, int timeout,
            ProtocolFormat format, out string logText)
        {
            try
            {
                // this read method avoids ObjectDisposedException when the thread is aborted
                int readCnt = 0;
                SerialPort.ReadTimeout = 0;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (readCnt < count && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    try { readCnt += SerialPort.Read(buffer, offset + readCnt, count - readCnt); }
                    catch (TimeoutException) { }

                    // accumulate data in the internal port buffer
                    if (readCnt < count)
                        Thread.Sleep(DataAccumDelay);
                }

                logText = BuildReadLogText(buffer, offset, count, readCnt, format);
                return readCnt;
            }
            catch (Exception ex)
            {
                throw new ScadaException(CommPhrases.ReadDataError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads data with the stop condition.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int maxCount, int timeout, BinStopCondition stopCond,
            out bool stopReceived, ProtocolFormat format, out string logText)
        {
            try
            {
                int readCnt = 0;
                int curInd = offset;
                stopReceived = false;
                SerialPort.ReadTimeout = 0;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (readCnt < maxCount && !stopReceived && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    bool readOk;
                    try { readOk = SerialPort.Read(buffer, curInd, 1) > 0; }
                    catch (TimeoutException) { readOk = false; }

                    if (readOk)
                    {
                        stopReceived = stopCond.CheckCondition(buffer, curInd);
                        curInd++;
                        readCnt++;
                    }
                    else
                    {
                        Thread.Sleep(DataAccumDelay);
                    }
                }

                logText = BuildReadLogText(buffer, offset, readCnt, format);
                return readCnt;
            }
            catch (Exception ex)
            {
                throw new ScadaException(CommPhrases.ReadDataStopCondError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads lines.
        /// </summary>
        public override List<string> ReadLines(int timeout, TextStopCondition stopCond,
            out bool stopReceived, out string logText)
        {
            try
            {
                List<string> lines = new List<string>();
                stopReceived = false;
                SerialPort.ReadTimeout = ScadaUtils.IsRunningOnWin ? 0 : timeout; // TODO: check on Linux
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (!stopReceived && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    string line;
                    try { line = SerialPort.ReadLine().Trim(); }
                    catch (TimeoutException) { line = ""; }

                    if (line != "")
                    {
                        lines.Add(line);
                        stopReceived = stopCond.CheckCondition(lines, line);
                    }

                    if (!stopReceived)
                        Thread.Sleep(DataAccumDelay);
                }

                logText = BuildReadLinesLogText(lines);
                return lines;
            }
            catch (Exception ex)
            {
                throw new ScadaException(CommPhrases.ReadLinesError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Writes the specified data.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count, ProtocolFormat format, out string logText)
        {
            try
            {
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();

                try
                {
                    SerialPort.Write(buffer, offset, count);
                    logText = BuildWriteLogText(buffer, offset, count, format);
                    WriteError = false;
                }
                catch (TimeoutException ex)
                {
                    logText = CommPhrases.WriteDataError + ": " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                WriteError = true;
                throw new ScadaException(CommPhrases.WriteDataError + ": " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Writes the specified line.
        /// </summary>
        public override void WriteLine(string text, out string logText)
        {
            try
            {
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();

                try
                {
                    SerialPort.WriteLine(text);
                    logText = CommPhrases.SendNotation + ": " + text;
                    WriteError = false;
                }
                catch (TimeoutException ex)
                {
                    logText = CommPhrases.WriteDataError + ": " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                WriteError = true;
                throw new ScadaException(CommPhrases.WriteLineError + ": " + ex.Message, ex);
            }
        }
    }
}
