// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol
{
    /// <summary>
    /// Represents a Modbus data unit.
    /// <para>Представляет блок данных Modbus.</para>
    /// </summary>
    public abstract class DataUnit
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataUnit()
        {
            Name = "";
            Address = 0;
            Aux = null;

            FuncCode = 0;
            ExcFuncCode = 0;
            ReqPDU = null;
            RespPduLen = 0;
            ReqADU = null;
            ReqStr = "";
            RespByteCnt = 0;
        }


        /// <summary>
        /// Gets or sets the data unit name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the data unit name.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets the data block.
        /// </summary>
        public DataBlock DataBlock { get; }

        /// <summary>
        /// Gets or sets the zero-based address of the start element.
        /// </summary>
        public ushort Address { get; set; }
        
        /// <summary>
        /// Gets or sets the auxiliary object.
        /// </summary>
        public object Aux { get; set; }

        /// <summary>
        /// Gets a description of the request that reads or writes the data unit.
        /// </summary>
        public abstract string ReqDescr { get; }


        /// <summary>
        /// Gets the request function code.
        /// </summary>
        public byte FuncCode { get; protected set; }

        /// <summary>
        /// Gets the exception function code.
        /// </summary>
        public byte ExcFuncCode { get; protected set; }

        /// <summary>
        /// Gets the request PDU.
        /// </summary>
        public byte[] ReqPDU { get; protected set; }

        /// <summary>
        /// Gets the length of the response PDU.
        /// </summary>
        public int RespPduLen { get; protected set; }

        /// <summary>
        /// Gets the request ADU.
        /// </summary>
        public byte[] ReqADU { get; protected set; }

        /// <summary>
        /// Gets the request string in the ASCII mode.
        /// </summary>
        public string ReqStr { get; protected set; }

        /// <summary>
        /// Gets the length of the response ADU.
        /// </summary>
        public int RespAduLen { get; protected set; }

        /// <summary>
        /// Gets the number of bytes specified in the response packet.
        /// </summary>
        public byte RespByteCnt { get; protected set; }


        /// <summary>
        /// Sets the request and exception function codes.
        /// </summary>
        public void SetFuncCode(byte funcCode)
        {
            FuncCode = funcCode;
            ExcFuncCode = (byte)(funcCode | 0x80);
        }

        /// <summary>
        /// Initializes the request PDU and calculates the response length.
        /// </summary>
        public abstract void InitReqPDU();

        /// <summary>
        /// Initializes the request ADU and calculates the response length.
        /// </summary>
        public virtual void InitReqADU(byte devAddr, TransMode transMode)
        {
            if (ReqPDU == null)
                return;

            int pduLen = ReqPDU.Length;

            switch (transMode)
            {
                case TransMode.RTU:
                    ReqADU = new byte[pduLen + 3];
                    ReqADU[0] = devAddr;
                    Buffer.BlockCopy(ReqPDU, 0, ReqADU, 1, ReqPDU.Length);
                    ushort crc = BigQueueUtils.CRC16(ReqADU, 0, pduLen + 1);
                    ReqADU[pduLen + 1] = (byte)(crc % 256);
                    ReqADU[pduLen + 2] = (byte)(crc / 256);
                    RespAduLen = RespPduLen + 3;
                    break;

                case TransMode.ASCII:
                    byte[] aduBuf = new byte[pduLen + 2];
                    aduBuf[0] = devAddr;
                    Buffer.BlockCopy(ReqPDU, 0, aduBuf, 1, ReqPDU.Length);
                    aduBuf[pduLen + 1] = BigQueueUtils.LRC(aduBuf, 0, pduLen + 1);

                    StringBuilder sbADU = new StringBuilder();
                    foreach (byte b in aduBuf)
                    {
                        sbADU.Append(b.ToString("X2"));
                    }

                    ReqADU = Encoding.ASCII.GetBytes(sbADU.ToString());
                    ReqStr = BigQueueUtils.Colon + sbADU;
                    RespAduLen = RespPduLen + 2;
                    break;

                default: // TransModes.TCP
                    ReqADU = new byte[pduLen + 7];
                    ReqADU[0] = 0;
                    ReqADU[1] = 0;
                    ReqADU[2] = 0;
                    ReqADU[3] = 0;
                    ReqADU[4] = (byte)((pduLen + 1) / 256);
                    ReqADU[5] = (byte)((pduLen + 1) % 256);
                    ReqADU[6] = devAddr;
                    Buffer.BlockCopy(ReqPDU, 0, ReqADU, 7, ReqPDU.Length);
                    RespAduLen = RespPduLen + 7;
                    break;
            }
        }

        /// <summary>
        /// Decodes the response PDU.
        /// </summary>
        public virtual bool DecodeRespPDU(byte[] buffer, int offset, int length, out string errMsg)
        {
            errMsg = "";
            bool result = false;
            byte respFuncCode = buffer[offset];

            if (respFuncCode == FuncCode)
            {
                if (length == RespPduLen)
                    result = true;
                else
                    errMsg = ModbusPhrases.InvalidPduLength;
            }
            else if (respFuncCode == ExcFuncCode)
            {
                errMsg = length == 2 ? 
                    ModbusPhrases.DeviceError + ": " + BigQueueUtils.GetExcDescr(buffer[offset + 1]) :
                    ModbusPhrases.InvalidPduLength;
            }
            else
            {
                errMsg = ModbusPhrases.InvalidPduFuncCode;
            }

            return result;
        }
    }
}
