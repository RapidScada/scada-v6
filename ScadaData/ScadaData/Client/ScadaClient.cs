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
using static Scada.BinaryConverter;
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
            CopyInt64(cnlListID, outBuf, ArgumentIndex);
            request.ArgumentLength = 8;
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            int index = ArgumentIndex;
            cnlListID = GetInt64(inBuf, ref index);
            return cnlListID > 0 ? GetCnlDataArray(inBuf, ref index) : null;
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
            int index = ArgumentIndex;
            CopyInt64(0, outBuf, ref index);
            CopyIntArray(cnlNums, outBuf, ref index);
            CopyBool(useCache, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);

            DataPacket response = ReceiveResponse(request);
            index = ArgumentIndex;
            cnlListID = GetInt64(inBuf, ref index);
            CnlData[] cnlData = GetCnlDataArray(inBuf, ref index);

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
        public void WriteCurrentData(int deviceNum, int[] cnlNums, CnlData[] cnlData, bool applyFormulas)
        {
            if (cnlNums == null)
                throw new ArgumentNullException("cnlNums");
            if (cnlData == null)
                throw new ArgumentNullException("cnlData");

            RestoreConnection();

            DataPacket request = CreateRequest(FunctionID.WriteCurrentData);
            int index = ArgumentIndex;
            CopyInt32(deviceNum, outBuf, ref index);

            int cnlCnt = cnlNums.Length;
            CopyInt32(cnlCnt, outBuf, ref index);

            for (int i = 0, idx1 = index, idx2 = index + cnlCnt * 4; i < cnlCnt; i++)
            {
                CnlData cnlDataElem = cnlData[i];
                CopyInt32(cnlNums[i], outBuf, ref idx1);
                CopyDouble(cnlDataElem.Val, outBuf, ref idx2);
                CopyInt32(cnlDataElem.Stat, outBuf, ref idx2);
            }

            index += cnlCnt * 16;
            CopyBool(applyFormulas, outBuf, ref index);
            request.BufferLength = index;
            SendRequest(request);
            ReceiveResponse(request);
        }
    }
}
