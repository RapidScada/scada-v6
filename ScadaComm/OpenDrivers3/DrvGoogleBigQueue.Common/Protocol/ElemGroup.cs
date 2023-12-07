// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol
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
        public ElemGroup()
            : base()
        {
            reqDescr = "";
            Elems = new List<Elem>();
            ElemData = null;
            StartTagIdx = -1;
            DataTimeCache = new ConcurrentDictionary<string, DateTime>();
        }


        /// <summary>
        /// big queue sql
        /// </summary>
        public string QuerySql { get; set; }

        /// <summary>
        /// 时间缓存，记录是否最新数据
        /// </summary>
        public ConcurrentDictionary<string, DateTime> DataTimeCache = null;

        /// <summary>
        /// 最新记录时间
        /// </summary>
        public DateTime LastRecordTime;

        /// <summary>
        /// Gets a description of the request that reads the element group.
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
        /// Gets the elements of the group.
        /// </summary>
        public List<Elem> Elems { get; private set; }

        /// <summary>
        /// Gets the raw data of the group elements.
        /// </summary>
        public byte[][] ElemData { get; private set; }

        /// <summary>
        /// Gets or sets the device tag index that corresponds to the start element.
        /// </summary>
        public int StartTagIdx { get; set; }


        /// <summary>
        /// Initializes the request PDU and calculates the response length.
        /// </summary>
        public override void InitReqPDU()
        {
        }

        /// <summary>
        /// Decodes the response PDU.
        /// </summary>
        public override bool DecodeRespPDU(byte[] buffer, int offset, int length, out string errMsg)
        {
            errMsg = string.Empty;
            return false;
        }

        /// <summary>
        /// Creates a new Modbus element.
        /// </summary>
        public virtual Elem CreateElem()
        {
            return new Elem();
        }

        /// <summary>
        /// Gets the element value according to its type, converted to double.
        /// </summary>
        public double GetElemVal(int elemIdx)
        {
            Elem elem = Elems[elemIdx];
            byte[] elemData = ElemData[elemIdx];
            byte[] buf;

            // order bytes if needed
            if (elem.ByteOrder == null)
            {
                buf = elemData;
            }
            else
            {
                buf = new byte[elemData.Length];
                BigQueueUtils.ApplyByteOrder(elemData, buf, elem.ByteOrder);
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
