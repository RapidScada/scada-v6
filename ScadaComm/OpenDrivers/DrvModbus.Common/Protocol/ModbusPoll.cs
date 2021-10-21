// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Channels;
using Scada.Log;
using System.Globalization;

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Polls devices using Modbus protocol.
    /// <para>Опрос устройств по протоколу Modbus.</para>
    /// </summary>
    public class ModbusPoll
    {
        /// <summary>
        /// Represetns a request method.
        /// </summary>
        public delegate bool RequestDelegate(DataUnit dataUnit);

        /// <summary>
        /// The default input buffer size in bytes.
        /// </summary>
        private const int DefInBufSize = 300;

        private Connection connection; // the device connection
        private ILog log;              // the communication line log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModbusPoll()
            : this(DefInBufSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModbusPoll(int inBufSize)
        {
            connection = ConnectionStub.Instance;
            log = LogStub.Instance;

            InBuf = new byte[inBufSize];
            TransactionID = 0;
            Timeout = 0;
        }


        /// <summary>
        /// Gets the input buffer.
        /// </summary>
        public byte[] InBuf { get; protected set; }

        /// <summary>
        /// Gets the current transaction ID in the TCP mode.
        /// </summary>
        public ushort TransactionID { get; protected set; }

        /// <summary>
        /// Gets or sets the reading timeout, ms.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value ?? ConnectionStub.Instance;
            }
        }

        /// <summary>
        /// Gets or sets the communication line log.
        /// </summary>
        public ILog Log
        {
            get
            {
                return log;
            }
            set
            {
                log = value ?? LogStub.Instance;
            }
        }


        /// <summary>
        /// Performs a request in the PDU mode.
        /// </summary>
        public bool RtuRequest(DataUnit dataUnit)
        {
            bool result = false;

            // send request
            log.WriteLine(dataUnit.ReqDescr);
            Connection.Write(dataUnit.ReqADU, 0, dataUnit.ReqADU.Length, ProtocolFormat.Hex, out string logText);
            log.WriteLine(logText);

            // receive response
            // partial read to calculate PDU length
            int readCnt = Connection.Read(InBuf, 0, 5, Timeout, ProtocolFormat.Hex, out logText);
            log.WriteLine(logText);

            if (readCnt == 5)
            {
                int pduLen;
                int count;

                if (InBuf[0] != dataUnit.ReqADU[0]) // validate device address
                {
                    log.WriteLine(ModbusPhrases.IncorrectDevAddr);
                }
                else if (!(InBuf[1] == dataUnit.FuncCode || InBuf[1] == dataUnit.ExcFuncCode))
                {
                    log.WriteLine(ModbusPhrases.IncorrectPduFuncCode);
                }
                else
                {
                    if (InBuf[1] == dataUnit.FuncCode)
                    {
                        // read end of response
                        pduLen = dataUnit.RespPduLen;
                        count = dataUnit.RespAduLen - 5;

                        readCnt = Connection.Read(InBuf, 5, count, Timeout, ProtocolFormat.Hex, out logText);
                        log.WriteLine(logText);
                    }
                    else // exception received
                    {
                        pduLen = 2;
                        count = 0;
                        readCnt = 0;
                    }

                    if (readCnt == count)
                    {
                        if (InBuf[pduLen + 1] + InBuf[pduLen + 2] * 256 == ModbusUtils.CRC16(InBuf, 0, pduLen + 1))
                        {
                            // decode response
                            if (dataUnit.DecodeRespPDU(InBuf, 1, pduLen, out string errMsg))
                            {
                                log.WriteLine(ModbusPhrases.OK);
                                result = true;
                            }
                            else
                            {
                                log.WriteLine(errMsg + "!");
                            }
                        }
                        else
                        {
                            log.WriteLine(ModbusPhrases.CrcError);
                        }
                    }
                    else
                    {
                        log.WriteLine(ModbusPhrases.CommErrorWithExclamation);
                    }
                }
            }
            else
            {
                log.WriteLine(ModbusPhrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Performs a request in the ASCII mode.
        /// </summary>
        public bool AsciiRequest(DataUnit dataUnit)
        {
            bool result = false;

            // send request
            log.WriteLine(dataUnit.ReqDescr);
            Connection.WriteLine(dataUnit.ReqStr, out string logText);
            log.WriteLine(logText);

            // receive response
            string line = Connection.ReadLine(Timeout, out logText);
            log.WriteLine(logText);
            int lineLen = line == null ? 0 : line.Length;

            if (lineLen >= 3)
            {
                int aduLen = (lineLen - 1) / 2;

                if (aduLen == dataUnit.RespAduLen && lineLen % 2 == 1)
                {
                    // receive response ADU
                    byte[] aduBuf = new byte[aduLen];
                    bool parseOK = true;

                    for (int i = 0, j = 1; i < aduLen && parseOK; i++, j += 2)
                    {
                        try
                        {
                            aduBuf[i] = byte.Parse(line.Substring(j, 2), NumberStyles.HexNumber);
                        }
                        catch
                        {
                            log.WriteLine(ModbusPhrases.IncorrectSymbol);
                            parseOK = false;
                        }
                    }

                    if (parseOK)
                    {
                        if (aduBuf[aduLen - 1] == ModbusUtils.LRC(aduBuf, 0, aduLen - 1))
                        {
                            // decode response
                            if (dataUnit.DecodeRespPDU(aduBuf, 1, aduLen - 2, out string errMsg))
                            {
                                log.WriteLine(ModbusPhrases.OK);
                                result = true;
                            }
                            else
                            {
                                log.WriteLine(errMsg + "!");
                            }
                        }
                        else
                        {
                            log.WriteLine(ModbusPhrases.LrcError);
                        }
                    }
                }
                else
                {
                    log.WriteLine(ModbusPhrases.IncorrectAduLength);
                }
            }
            else
            {
                log.WriteLine(ModbusPhrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Performs a request in the TCP mode.
        /// </summary>
        public bool TcpRequest(DataUnit dataUnit)
        {
            bool result = false;

            // specify transaction ID
            if (++TransactionID == 0)
                TransactionID = 1;

            dataUnit.ReqADU[0] = (byte)(TransactionID / 256);
            dataUnit.ReqADU[1] = (byte)(TransactionID % 256);

            // send request
            log.WriteLine(dataUnit.ReqDescr);
            Connection.Write(dataUnit.ReqADU, 0, dataUnit.ReqADU.Length, ProtocolFormat.Hex, out string logText);
            log.WriteLine(logText);

            // receive response
            // read MBAP header
            int readCnt = Connection.Read(InBuf, 0, 7, Timeout, ProtocolFormat.Hex, out logText);
            log.WriteLine(logText);

            if (readCnt == 7)
            {
                int pduLen = InBuf[4] * 256 + InBuf[5] - 1;

                if (InBuf[0] == dataUnit.ReqADU[0] && InBuf[1] == dataUnit.ReqADU[1] && 
                    InBuf[2] == 0 && InBuf[3] == 0 && pduLen > 0 &&
                    InBuf[6] == dataUnit.ReqADU[6])
                {
                    // read PDU
                    readCnt = Connection.Read(InBuf, 7, pduLen, Timeout, ProtocolFormat.Hex, out logText);
                    log.WriteLine(logText);

                    if (readCnt == pduLen)
                    {
                        // decode response
                        if (dataUnit.DecodeRespPDU(InBuf, 7, pduLen, out string errMsg))
                        {
                            log.WriteLine(ModbusPhrases.OK);
                            result = true;
                        }
                        else
                        {
                            log.WriteLine(errMsg + "!");
                        }
                    }
                    else
                    {
                        log.WriteLine(ModbusPhrases.CommErrorWithExclamation);
                    }
                }
                else
                {
                    log.WriteLine(ModbusPhrases.IncorrectMbap);
                }
            }
            else
            {
                log.WriteLine(ModbusPhrases.CommErrorWithExclamation);
            }

            return result;
        }

        /// <summary>
        /// Gets the response method according to the data transfer mode.
        /// </summary>
        public RequestDelegate GetRequestMethod(TransMode transMode)
        {
            switch (transMode)
            {
                case TransMode.RTU:
                    return RtuRequest;
                case TransMode.ASCII:
                    return AsciiRequest;
                default: // TransMode.TCP
                    return TcpRequest;
            }
        }
    }
}
