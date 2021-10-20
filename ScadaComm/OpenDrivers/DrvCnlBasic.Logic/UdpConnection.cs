// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Comm.Lang;
using Scada.Lang;
using Scada.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Scada.Comm.Drivers.DrvCnlBasic.Logic
{
    /// <summary>
    /// Implements UDP connection.
    /// <para>Реализует соединение по протоколу UDP.</para>
    /// </summary>
    public class UdpConnection : Connection
    {
        /// <summary>
        /// The datagram receive taimeout, ms.
        /// </summary>
        protected const int DatagramReceiveTimeout = 10;

        protected bool connected;     // indicates that the connection is established
        protected byte[] datagramBuf; // contains an incompletely read datagram
        protected int bufReadPos;     // the datagram buffer read position


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UdpConnection(ILog log, UdpClient udpClient, int remotePort)
            : base(log)
        {
            InternalInit(udpClient);
            RemotePort = remotePort;
        }


        /// <summary>
        /// Gets the UDP client.
        /// </summary>
        public UdpClient UdpClient { get; protected set; }

        /// <summary>
        /// Gets or sets the remote UDP port.
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// Gets a value indicating whether the connection is established.
        /// </summary>
        public override bool Connected
        {
            get
            {
                return connected;
            }
        }


        /// <summary>
        /// Initializes the connection.
        /// </summary>
        protected void InternalInit(UdpClient udpClient)
        {
            connected = true;
            datagramBuf = null;
            bufReadPos = 0;

            UdpClient = udpClient ?? throw new ArgumentNullException(nameof(udpClient));
            UdpClient.Client.ReceiveTimeout = DefaultReadTimeout;
            UdpClient.Client.SendTimeout = DefaultWriteTimeout;
        }

        /// <summary>
        /// Creates an endpoint of the remote host.
        /// </summary>
        protected IPEndPoint CreateRemoteEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(RemoteAddress), RemotePort);
        }

        /// <summary>
        /// Receives an UDP datagram.
        /// </summary>
        protected byte[] ReceiveDatagram(ref IPEndPoint endPoint, out int readPos, out bool isNew)
        {
            if (datagramBuf == null)
            {
                // receive new datagram
                readPos = 0;
                isNew = true;
                try { return UdpClient.Receive(ref endPoint); }
                catch (SocketException) { return null; }
            }
            else
            {
                // return the incompletely read datagram
                byte[] datagram = datagramBuf;
                readPos = bufReadPos;
                isNew = false;

                datagramBuf = null;
                bufReadPos = 0;

                return datagram;
            }
        }

        /// <summary>
        /// Stores the incompletely read datagram.
        /// </summary>
        protected void StoreDatagram(byte[] datagram, int readPos)
        {
            if (datagram != null && readPos < datagram.Length)
            {
                datagramBuf = datagram;
                bufReadPos = readPos;
            }
        }


        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close()
        {
            try
            {
                UdpClient.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при отключении" :
                    "Error disconnecting");
            }
            finally
            {
                connected = false;
            }
        }

        /// <summary>
        /// Clears the buffer from an incompletely read datagram.
        /// </summary>
        public void ClearDatagramBuffer()
        {
            datagramBuf = null;
            bufReadPos = 0;
        }

        /// <summary>
        /// Receives a datagram from a remote host asynchronously.
        /// </summary>
        public void BeginReceive(Action<IAsyncResult> callback)
        {
            try
            {
                UdpClient.Client.ReceiveTimeout = DatagramReceiveTimeout;
                UdpClient.BeginReceive(new AsyncCallback(callback), null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, Locale.IsRussian ?
                    "Ошибка при запуске приёма данных" :
                    "Error starting receiving data");
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
                IPEndPoint endPoint = CreateRemoteEndPoint();
                UdpClient.Client.ReceiveTimeout = DatagramReceiveTimeout;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (readCnt < count && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    // read data
                    byte[] datagram = ReceiveDatagram(ref endPoint, out int readPos, out bool isNew);

                    // copy received data to the buffer
                    if (datagram != null && datagram.Length > 0)
                    {
                        int requiredCnt = count - readCnt; // left to read
                        int copyCnt = Math.Min(datagram.Length - readPos, requiredCnt);
                        Buffer.BlockCopy(datagram, readPos, buffer, readCnt + offset, copyCnt);
                        readCnt += copyCnt;
                        readPos += copyCnt;
                    }

                    // accumulate data in the internal connection buffer
                    if (readCnt < count && isNew)
                        Thread.Sleep(DataAccumDelay);

                    StoreDatagram(datagram, readPos);
                }

                logText = BuildReadLogText(buffer, offset, count, readCnt, format);
                return readCnt;
            }
            catch (SocketException ex)
            {
                logText = CommPhrases.ReadDataError + ": " + ex.Message;
                return 0;
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
                IPEndPoint endPoint = CreateRemoteEndPoint();
                stopReceived = false;
                UdpClient.Client.ReceiveTimeout = DatagramReceiveTimeout;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (readCnt < maxCount && !stopReceived && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    // read data
                    byte[] datagram = ReceiveDatagram(ref endPoint, out int readPos, out bool isNew);

                    if (datagram != null && datagram.Length > 0)
                    {
                        // search for stop code
                        int stopCodeInd = -1;

                        for (int i = readPos, len = datagram.Length; i < len && !stopReceived; i++)
                        {
                            if (stopCond.CheckCondition(datagram, i))
                            {
                                stopCodeInd = i;
                                stopReceived = true;
                            }
                        }

                        // copy received data to the buffer
                        int requiredCnt = stopReceived ? stopCodeInd - readCnt + 1 : maxCount - readCnt;
                        int copyCnt = Math.Min(datagram.Length - readPos, requiredCnt);
                        Buffer.BlockCopy(datagram, readPos, buffer, readCnt + offset, copyCnt);
                        readCnt += copyCnt;
                        readPos += copyCnt;
                    }

                    // accumulate data in the internal connection buffer
                    if (readCnt < maxCount && !stopReceived && isNew)
                        Thread.Sleep(DataAccumDelay);

                    StoreDatagram(datagram, readPos);
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
                IPEndPoint endPoint = CreateRemoteEndPoint();
                UdpClient.Client.ReceiveTimeout = DatagramReceiveTimeout;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (!stopReceived && stopwatch.ElapsedMilliseconds <= timeout)
                {
                    // read data
                    byte[] datagram = ReceiveDatagram(ref endPoint, out int readPos, out bool isNew);

                    if (datagram != null && datagram.Length > 0)
                    {
                        // get lines from the received data
                        int datagramLen = datagram.Length;
                        StringBuilder sbLine = new StringBuilder(datagramLen);

                        while (readPos < datagramLen && !stopReceived)
                        {
                            sbLine.Append(Encoding.GetChars(datagram, readPos, 1));
                            readPos++;
                            bool newLineFound = sbLine.EndsWith(NewLine);

                            if (newLineFound || readPos == datagramLen)
                            {
                                string line = newLineFound ?
                                    sbLine.ToString(0, sbLine.Length - NewLine.Length) :
                                    sbLine.ToString();
                                lines.Add(line);
                                sbLine.Clear();
                                stopReceived = stopCond.CheckCondition(lines, line);
                            }
                        }
                    }

                    // accumulate data in the internal connection buffer
                    if (!stopReceived && isNew)
                        Thread.Sleep(DataAccumDelay);

                    StoreDatagram(datagram, readPos);
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
                if (string.IsNullOrEmpty(RemoteAddress))
                    throw new InvalidOperationException("Remote address is undefined.");
                if (RemotePort <= 0)
                    throw new InvalidOperationException("Remote port is undefined.");

                byte[] datagram;

                if (offset > 0)
                {
                    datagram = new byte[count];
                    Array.Copy(buffer, offset, datagram, 0, count);
                }
                else
                {
                    datagram = buffer;
                }

                try
                {
                    int sentCnt = UdpClient.Send(datagram, count, RemoteAddress, RemotePort);
                    logText = BuildWriteLogText(datagram, 0, count, format);
                }
                catch (SocketException ex)
                {
                    logText = CommPhrases.WriteDataError + ": " + ex.Message;
                }
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
                throw new InvalidOperationException(CommPhrases.WriteLineError + ": " + ex.Message, ex);
            }
        }
    }
}
