// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Devices;
using Scada.Comm.Lang;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements the TCP connection.
    /// <para>Реализует соединение по протоколу TCP.</para>
    /// </summary>
    public class TcpConnection : Connection
    {
        /// <summary>
        /// The maximum number of characters per line.
        /// </summary>
        protected const int MaxLineLength = 1024;

        protected readonly List<DeviceLogic> boundDevices; // the devices bound to the connection
        protected DeviceLogic[] devicesCopy; // the shallow copy of the bound devices
        protected DateTime connFailDT;       // the time of unsuccessful attempt to connect


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TcpConnection(ILog log, TcpClient tcpClient)
            : base(log)
        {
            boundDevices = new List<DeviceLogic>();
            devicesCopy = null;
            connFailDT = DateTime.MinValue;

            InternalInit(tcpClient);
            ReconnectAfter = 0;
            JustConnected = true;
            CloseMark = false;
        }


        /// <summary>
        /// Gets the TCP client.
        /// </summary>
        public TcpClient TcpClient { get; protected set; }

        /// <summary>
        /// Gets the data stream of the TCP client.
        /// </summary>
        public NetworkStream NetStream { get; protected set; }

        /// <summary>
        /// Gets the remote TCP port.
        /// </summary>
        public int RemotePort { get; protected set; }

        /// <summary>
        /// Gets the time of the last activity (UTC).
        /// </summary>
        public DateTime ActivityTime { get; protected set; }

        /// <summary>
        /// Gets or sets the reconnect interval in seconds.
        /// </summary>
        public int ReconnectAfter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connection has just been established and no data has been exchanged.
        /// </summary>
        public bool JustConnected { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connection should be closed.
        /// </summary>
        public bool CloseMark { get; set; }

        /// <summary>
        /// Gets the devices bound to the connection.
        /// </summary>
        public IReadOnlyCollection<DeviceLogic> BoundDevices
        {
            get
            {
                lock (boundDevices)
                {
                    if (devicesCopy == null)
                        devicesCopy = boundDevices.ToArray();

                    return devicesCopy;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the connection is established.
        /// </summary>
        public override bool Connected
        {
            get
            {
                return TcpClient.Connected;
            }
        }


        /// <summary>
        /// Initializes the connection.
        /// </summary>
        protected void InternalInit(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            DefineNetStream();
            DefineRemoteAddress();
            ActivityTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Defines the data stream of the TCP client.
        /// </summary>
        protected void DefineNetStream()
        {
            if (TcpClient.Connected)
            {
                NetStream = TcpClient.GetStream();
                NetStream.ReadTimeout = DefaultReadTimeout;
                NetStream.WriteTimeout = DefaultWriteTimeout;
            }
            else
            {
                NetStream = null;
            }
        }

        /// <summary>
        /// Defines the remote address of the connection.
        /// </summary>
        protected void DefineRemoteAddress()
        {
            try
            {
                IPEndPoint endPoint = (IPEndPoint)TcpClient.Client.RemoteEndPoint;
                RemoteAddress = endPoint.Address.ToString();
                RemotePort = endPoint.Port;
            }
            catch
            {
                RemoteAddress = "";
                RemotePort = 0;
            }
        }

        /// <summary>
        /// Updates the time of the last activity.
        /// </summary>
        protected void UpdateActivityTime()
        {
            ActivityTime = DateTime.UtcNow;
            JustConnected = false;
        }


        /// <summary>
        /// Establishes a TCP connection.
        /// </summary>
        public void Open(string host, int port)
        {
            if ((DateTime.UtcNow - connFailDT).TotalSeconds >= ReconnectAfter)
            {
                try
                {
                    if (IPAddress.TryParse(host, out IPAddress addr))
                        TcpClient.Connect(addr, port);
                    else
                        TcpClient.Connect(host, port);

                    DefineNetStream();
                    DefineRemoteAddress();
                    connFailDT = DateTime.MinValue;
                }
                catch (Exception ex)
                {
                    // get the time again, because the Connect method can take a long time
                    connFailDT = DateTime.UtcNow;
                    throw new ScadaException(string.Format(Locale.IsRussian ?
                        "Ошибка при установке TCP-соединения: " :
                        "Error establishing TCP connection: ") + ex.Message, ex);
                }
            }
            else
            {
                throw new ScadaException(Locale.IsRussian ?
                    "Попытка установки TCP-соединения может быть не ранее, чем через {0} с после предыдущей." :
                    "An attempt to establish TCP connection can not be earlier than {0} seconds after the previous one.",
                    ReconnectAfter);
            }
        }

        /// <summary>
        /// Disconnects the client and resets connections of the bound devices.
        /// </summary>
        public void Close()
        {
            foreach (DeviceLogic deviceLogic in BoundDevices)
            {
                deviceLogic.Connection = null;
            }

            Disconnect();
        }

        /// <summary>
        /// Disconnects the client only.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                NetStream?.Close();
                TcpClient?.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при отключении" :
                    "Error disconnecting");
            }
        }

        /// <summary>
        /// Creates a new or repairs the connection.
        /// </summary>
        public void Renew()
        {
            Disconnect();
            InternalInit(new TcpClient());
        }

        /// <summary>
        /// Reads the currently available data.
        /// </summary>
        public int ReadAvailable(byte[] buffer, int offset, int maxCount, ProtocolFormat format, out string logText)
        {
            try
            {
                int count = Math.Min(TcpClient.Available, maxCount);
                NetStream.ReadTimeout = DefaultReadTimeout;
                int readCnt = NetStream.DataAvailable ? NetStream.Read(buffer, offset, count) : 0;
                logText = BuildReadLogText(buffer, offset, count, readCnt, format);

                if (readCnt > 0)
                    UpdateActivityTime();

                return readCnt;
            }
            catch (Exception ex)
            {
                throw new ScadaException((Locale.IsRussian ?
                    "Ошибка при считывании доступных данных: " :
                    "Error reading available data: ") + ex.Message, ex);
            }
        }

        /// <summary>
        /// Clears the data stream of the TCP client.
        /// </summary>
        public void ClearNetStream(byte[] buffer)
        {
            try
            {
                if (NetStream.DataAvailable)
                    NetStream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                throw new ScadaException((Locale.IsRussian ?
                    "Ошибка при очистке потока данных: " :
                    "Error clearing data stream: ") + ex.Message, ex);
            }
        }

        /// <summary>
        /// Binds the device to the connection.
        /// </summary>
        public void BindDevice(DeviceLogic deviceLogic)
        {
            if (deviceLogic != null)
            {
                deviceLogic.Connection = this;

                lock (boundDevices)
                {
                    boundDevices.Add(deviceLogic);
                    devicesCopy = null;
                }
            }
        }

        /// <summary>
        /// Clears the devices bound to the connection.
        /// </summary>
        public void ClearBoundDevices()
        {
            lock (boundDevices)
            {
                boundDevices.Clear();
                devicesCopy = null;
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
                int readCnt = 0;
                NetStream.ReadTimeout = timeout; // timeout is not maintained if all available data has been read
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (readCnt < count && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    // read data
                    try
                    {
                        if (NetStream.DataAvailable) // checking DataAvailable is critical on Linux
                            readCnt += NetStream.Read(buffer, readCnt + offset, count - readCnt);
                    }
                    catch (IOException) { }

                    // accumulate data in the internal connection buffer
                    if (readCnt < count)
                        Thread.Sleep(DataAccumDelay);
                }

                logText = BuildReadLogText(buffer, offset, count, readCnt, format);

                if (readCnt > 0)
                    UpdateActivityTime();

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
                int curOffset = offset;
                stopReceived = false;
                NetStream.ReadTimeout = OneByteReadTimeout;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (readCnt < maxCount && !stopReceived && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    // read one byte
                    bool readOk;
                    try { readOk = NetStream.DataAvailable && NetStream.Read(buffer, curOffset, 1) > 0; }
                    catch (IOException) { readOk = false; }

                    if (readOk)
                    {
                        stopReceived = stopCond.CheckCondition(buffer, curOffset);
                        curOffset++;
                        readCnt++;
                    }
                    else
                    {
                        Thread.Sleep(DataAccumDelay);
                    }
                }

                logText = BuildReadLogText(buffer, offset, readCnt, format);

                if (readCnt > 0)
                    UpdateActivityTime();

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
                NetStream.ReadTimeout = OneByteReadTimeout;
                Stopwatch stopwatch = Stopwatch.StartNew();

                StringBuilder sbLine = new StringBuilder(MaxLineLength);
                byte[] buffer = new byte[1];

                while (!stopReceived && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    // read one byte
                    bool readOk;
                    try { readOk = NetStream.DataAvailable && NetStream.Read(buffer, 0, 1) > 0; }
                    catch (IOException) { readOk = false; }

                    if (readOk)
                    {
                        sbLine.Append(Encoding.GetChars(buffer));
                    }
                    else
                    {
                        Thread.Sleep(DataAccumDelay);
                    }

                    bool newLineFound = sbLine.EndsWith(NewLine);

                    if (newLineFound || sbLine.Length == MaxLineLength)
                    {
                        string line = newLineFound ?
                            sbLine.ToString(0, sbLine.Length - NewLine.Length) :
                            sbLine.ToString();
                        lines.Add(line);
                        sbLine.Clear();
                        stopReceived = stopCond.CheckCondition(lines, line);
                    }
                }

                logText = BuildReadLinesLogText(lines);

                if (lines.Count > 0)
                    UpdateActivityTime();

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
                NetStream.Write(buffer, offset, count);
                logText = BuildWriteLogText(buffer, offset, count, format);
            }
            catch (IOException ex)
            {
                logText = CommPhrases.WriteDataError + ": " + ex.Message;
            }
            catch (Exception ex)
            {
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
                byte[] buffer = Encoding.GetBytes(text + NewLine);
                Write(buffer, 0, buffer.Length, ProtocolFormat.String, out logText);
            }
            catch (Exception ex)
            {
                throw new ScadaException(CommPhrases.WriteLineError + ": " + ex.Message, ex);
            }
        }
    }
}
