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
 * Module   : ScadaData
 * Summary  : Represents a TCP client which interacts with the Server service
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using Scada.Protocol;
using System;
using static Scada.Protocol.ProtocolUtils;

namespace Scada.Client
{
    /// <summary>
    /// Represents a TCP client which interacts with the Server service.
    /// <para>Представляет TCP-клиента, который взаимодействует со службой Сервера.</para>
    /// </summary>
    public class ScadaClient : BaseClient
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaClient(ConnectionOptions connectionOptions)
            : base(connectionOptions)
        {
        }


        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(ref long cnlListID)
        {
            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetCurrentData);
            BitConverter.GetBytes(cnlListID).CopyTo(outBuf, ArgumentIndex);
            request.ArgumentLength = 8;
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            cnlListID = BitConverter.ToInt64(inBuf, ArgumentIndex);
            return cnlListID > 0 ? GetCnlDataArray(inBuf, ArgumentIndex + 8, out int index) : null;
        }

        /// <summary>
        /// Gets the current data.
        /// </summary>
        public CnlData[] GetCurrentData(int[] cnlNums, bool useCache, out long cnlListID)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.GetCurrentData);
            BitConverter.GetBytes((long)0).CopyTo(outBuf, ArgumentIndex);
            CopyIntArray(cnlNums, outBuf, ArgumentIndex + 8, out int index);
            outBuf[index++] = (byte)(useCache ? 1 : 0);
            request.BufferLength = index;
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            cnlListID = BitConverter.ToInt64(inBuf, ArgumentIndex);
            CnlData[] cnlData = GetCnlDataArray(inBuf, ArgumentIndex + 8, out index);

            if (cnlData.Length != cnlNums.Length)
            {
                throw new ProtocolException(ErrorCode.IllegalFunctionArguments, Locale.IsRussian ?
                    "Неверный размер данных." :
                    "Invalid data size.");
            }

            return cnlData;
        }

        /// <summary>
        /// Writes the current data.
        /// </summary>
        public void WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");
            if (cnlData == null)
                throw new ArgumentNullException("cnlData");

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.WriteCurrentData);
            int index = ArgumentIndex;
            BitConverter.GetBytes(deviceNum).CopyTo(outBuf, index);

            int cnlCnt = cnlNums.Length;
            BitConverter.GetBytes(cnlCnt).CopyTo(outBuf, index + 4);

            for (int i = 0, idx1 = index + 8, idx2 = idx1 + cnlCnt * 4; i < cnlCnt; i++, idx1 += 4, idx2 += 12)
            {
                CnlData cnlDataElem = cnlData[i];
                BitConverter.GetBytes(cnlNums[i]).CopyTo(outBuf, idx1);
                BitConverter.GetBytes(cnlDataElem.Val).CopyTo(outBuf, idx2);
                BitConverter.GetBytes(cnlDataElem.Stat).CopyTo(outBuf, idx2 + 8);
            }

            request.ArgumentLength = cnlCnt * 16 + 8;
            SendRequest(request);
            ReceiveResponse(request);
        }
    }
}
