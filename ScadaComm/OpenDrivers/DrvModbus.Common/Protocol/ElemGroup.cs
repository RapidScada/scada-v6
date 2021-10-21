// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Xml;

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Represents a group of Modbus elements.
    /// <para>Представляет группу элементов Modbus.</para>
    /// </summary>
    public class ElemGroup : DataUnit
    {
        private string reqDescr; // the request description


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ElemGroup(DataBlock dataBlock)
            : base(dataBlock)
        {
            reqDescr = "";
            Active = true;
            Elems = new List<Elem>();
            ElemData = null;
            StartTagIdx = -1;
            StartTagNum = 0;

            // define function codes
            UpdateFuncCode();
            ExcFuncCode = (byte)(FuncCode | 0x80);
        }


        /// <summary>
        /// Gets or sets a value indicating whether the element group is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets the elements of the group.
        /// </summary>
        public List<Elem> Elems { get; private set; }

        /// <summary>
        /// Gets the raw data of the group elements.
        /// </summary>
        public byte[][] ElemData { get; private set; }

        /// <summary>
        /// Gets a description of the request that represents the element group.
        /// </summary>
        public override string ReqDescr
        {
            get
            {
                if (string.IsNullOrEmpty(reqDescr))
                    reqDescr = string.Format(ModbusPhrases.Request, Name);
                return reqDescr;
            }
        }

        /// <summary>
        /// Gets or sets the device tag index that corresponds to the start element.
        /// </summary>
        public int StartTagIdx { get; set; }

        /// <summary>
        /// Gets or sets the device tag number that corresponds to the start element.
        /// </summary>
        public int StartTagNum { get; set; }


        /// <summary>
        /// Initializes the request PDU and calculates the response length.
        /// </summary>
        public override void InitReqPDU()
        {
            // get total request amount
            int totalQuantity = 0;
            int totalDataLength = 0;

            foreach (Elem elem in Elems)
            {
                totalQuantity += elem.Quantity;
                totalDataLength += elem.DataLength;
            }

            // build PDU
            ReqPDU = new byte[5];
            ReqPDU[0] = FuncCode;
            ReqPDU[1] = (byte)(Address / 256);
            ReqPDU[2] = (byte)(Address % 256);
            ReqPDU[3] = (byte)(totalQuantity / 256);
            ReqPDU[4] = (byte)(totalQuantity % 256);

            // calculate response length
            if (DataBlock == DataBlock.DiscreteInputs || DataBlock == DataBlock.Coils)
            {
                int n = totalQuantity / 8;
                if ((totalQuantity % 8) > 0)
                    n++;
                RespPduLen = 2 + n;
                RespByteCnt = (byte)n;
            }
            else
            {
                RespPduLen = 2 + totalDataLength;
                RespByteCnt = (byte)totalDataLength;
            }

            // initialize array of element values
            int elemCnt = Elems.Count;
            ElemData = new byte[elemCnt][];

            for (int i = 0; i < elemCnt; i++)
            {
                Elem elem = Elems[i];
                byte[] elemVal = new byte[elem.DataLength];
                Array.Clear(elemVal, 0, elemVal.Length);
                ElemData[i] = elemVal;
            }
        }

        /// <summary>
        /// Decodes the response PDU.
        /// </summary>
        public override bool DecodeRespPDU(byte[] buffer, int offset, int length, out string errMsg)
        {
            if (base.DecodeRespPDU(buffer, offset, length, out errMsg))
            {
                if (buffer[offset + 1] == RespByteCnt)
                {
                    int len = ElemData.Length;
                    int byteNum = offset + 2;

                    if (DataBlock == DataBlock.DiscreteInputs || DataBlock == DataBlock.Coils)
                    {
                        int bitNum = 0;
                        for (int elemInd = 0; elemInd < len; elemInd++)
                        {
                            ElemData[elemInd][0] = ((buffer[byteNum] >> bitNum) & 0x01) > 0 ? (byte)1 : (byte)0;

                            if (++bitNum == 8)
                            {
                                bitNum = 0;
                                byteNum++;
                            }
                        }
                    }
                    else
                    {
                        for (int elemInd = 0; elemInd < len; elemInd++)
                        {
                            byte[] elemVal = ElemData[elemInd];
                            int elemDataLen = Elems[elemInd].DataLength;

                            // copy the read bytes in reverse order
                            for (int i = elemDataLen - 1, j = byteNum; i >= 0; i--, j++)
                            {
                                elemVal[i] = buffer[j];
                            }

                            byteNum += elemDataLen;
                        }
                    }

                    return true;
                }
                else
                {
                    errMsg = ModbusPhrases.IncorrectPduData;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Loads the group from the XML node.
        /// </summary>
        public virtual void LoadFromXml(XmlElement groupElem)
        {
            if (groupElem == null)
                throw new ArgumentNullException(nameof(groupElem));

            Name = groupElem.GetAttribute("name");
            Address = (ushort)groupElem.GetAttrAsInt("address");
            Active = groupElem.GetAttrAsBool("active", true);

            XmlNodeList elemNodes = groupElem.SelectNodes("Elem");
            int maxElemCnt = MaxElemCnt;
            ElemType defElemType = DefElemType;

            foreach (XmlElement elemElem in elemNodes)
            {
                if (Elems.Count >= maxElemCnt)
                    break;

                Elem elem = CreateElem();
                elem.Name = elemElem.GetAttribute("name");
                elem.ElemType = elemElem.GetAttrAsEnum("type", defElemType);

                if (ByteOrderEnabled)
                {
                    elem.ByteOrderStr = elemElem.GetAttribute("byteOrder");
                    elem.ByteOrder = ModbusUtils.ParseByteOrder(elem.ByteOrderStr);
                }

                Elems.Add(elem);
            }
        }

        /// <summary>
        /// Saves the group into the XML node.
        /// </summary>
        public virtual void SaveToXml(XmlElement groupElem)
        {
            if (groupElem == null)
                throw new ArgumentNullException(nameof(groupElem));

            groupElem.SetAttribute("active", Active);
            groupElem.SetAttribute("dataBlock", DataBlock);
            groupElem.SetAttribute("address", Address);
            groupElem.SetAttribute("name", Name);

            foreach (Elem elem in Elems)
            {
                XmlElement elemElem = groupElem.AppendElem("Elem");
                elemElem.SetAttribute("name", elem.Name);

                if (ElemTypeEnabled)
                    elemElem.SetAttribute("type", elem.ElemType.ToString().ToLowerInvariant());

                if (ByteOrderEnabled)
                    elemElem.SetAttribute("byteOrder", elem.ByteOrderStr);
            }
        }

        /// <summary>
        /// Copies the group properties from the source group.
        /// </summary>
        public virtual void CopyFrom(ElemGroup srcGroup)
        {
            if (srcGroup == null)
                throw new ArgumentNullException(nameof(srcGroup));

            Name = srcGroup.Name;
            Address = srcGroup.Address;
            Active = srcGroup.Active;
            StartTagIdx = srcGroup.StartTagIdx;
            StartTagNum = srcGroup.StartTagNum;
            Elems.Clear();

            foreach (Elem srcElem in srcGroup.Elems)
            {
                Elem destElem = CreateElem();
                destElem.Name = srcElem.Name;
                destElem.ElemType = srcElem.ElemType;
                destElem.ByteOrder = srcElem.ByteOrder; // copy the array reference
                destElem.ByteOrderStr = srcElem.ByteOrderStr;
                Elems.Add(destElem);
            }
        }

        /// <summary>
        /// Creates a new Modbus element.
        /// </summary>
        public virtual Elem CreateElem()
        {
            return new Elem();
        }

        /// <summary>
        /// Updates the function code according to the data block.
        /// </summary>
        public void UpdateFuncCode()
        {
            switch (DataBlock)
            {
                case DataBlock.DiscreteInputs:
                    FuncCode = FuncCodes.ReadDiscreteInputs;
                    break;
                case DataBlock.Coils:
                    FuncCode = FuncCodes.ReadCoils;
                    break;
                case DataBlock.InputRegisters:
                    FuncCode = FuncCodes.ReadInputRegisters;
                    break;
                default: // DataBlock.HoldingRegisters:
                    FuncCode = FuncCodes.ReadHoldingRegisters;
                    break;
            }
        }

        /// <summary>
        /// Gets the element value according to its type, converted to double.
        /// </summary>
        public double GetElemVal(int elemInd)
        {
            Elem elem = Elems[elemInd];
            byte[] elemData = ElemData[elemInd];
            byte[] buf;

            // order bytes if needed
            if (elem.ByteOrder == null)
            {
                buf = elemData;
            }
            else
            {
                buf = new byte[elemData.Length];
                ModbusUtils.ApplyByteOrder(elemData, buf, elem.ByteOrder);
            }

            // calculate value
            switch (elem.ElemType)
            {
                case ElemType.UShort:
                    return BitConverter.ToUInt16(buf, 0);
                case ElemType.Short:
                    return BitConverter.ToInt16(buf, 0);
                case ElemType.UInt:
                    return BitConverter.ToUInt32(buf, 0);
                case ElemType.Int:
                    return BitConverter.ToInt32(buf, 0);
                case ElemType.ULong: // possible data loss
                    return BitConverter.ToUInt64(buf, 0);
                case ElemType.Long:  // possible data loss
                    return BitConverter.ToInt64(buf, 0);
                case ElemType.Float:
                    return BitConverter.ToSingle(buf, 0);
                case ElemType.Double:
                    return BitConverter.ToDouble(buf, 0);
                case ElemType.Bool:
                    return buf[0] > 0 ? 1.0 : 0.0;
                default:
                    return 0.0;
            }
        }
    }
}
