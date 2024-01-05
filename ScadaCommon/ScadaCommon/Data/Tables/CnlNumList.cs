/*
 * Copyright 2024 Rapid Software LLC
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
 * Module   : ScadaCommon
 * Summary  : Represents a list of channel numbers
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a list of channel numbers.
    /// <para>Представляет список номеров каналов.</para>
    /// </summary>
    public class CnlNumList
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlNumList(int[] cnlNums)
            : this(ScadaUtils.GenerateUniqueID(), cnlNums)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlNumList(long listID, int[] cnlNums)
        {
            ListID = listID;
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            CnlIndexes = CreateCnlIndexes();
            CRC = CalculateCRC();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlNumList(long listID, int[] cnlNums, uint crc)
        {
            ListID = listID;
            CnlNums = cnlNums ?? throw new ArgumentNullException(nameof(cnlNums));
            CnlIndexes = CreateCnlIndexes();
            CRC = crc;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlNumList(long listID, CnlNumList sourceList)
        {
            if (sourceList == null)
                throw new ArgumentNullException(nameof(sourceList));

            ListID = listID;
            CnlNums = sourceList.CnlNums;
            CnlIndexes = sourceList.CnlIndexes;
            CRC = sourceList.CRC;
        }


        /// <summary>
        /// Gets the list ID.
        /// </summary>
        public long ListID { get; }

        /// <summary>
        /// Gets the channel numbers.
        /// </summary>
        public int[] CnlNums { get; }

        /// <summary>
        /// Gets the channel indexes accessed by channel number.
        /// </summary>
        public Dictionary<int, int> CnlIndexes { get; }

        /// <summary>
        /// Gets the 32-bit CRC of the channel numbers.
        /// </summary>
        public uint CRC { get; }


        /// <summary>
        /// Creates the channel indexes.
        /// </summary>
        protected Dictionary<int, int> CreateCnlIndexes()
        {
            int cnlCnt = CnlNums.Length;
            Dictionary<int, int> cnlIndexes = new Dictionary<int, int>(cnlCnt);

            for (int i = 0; i < cnlCnt; i++)
            {
                cnlIndexes[CnlNums[i]] = i;
            }

            return cnlIndexes;
        }

        /// <summary>
        /// Calculates the CRC of the channel numbers.
        /// </summary>
        protected uint CalculateCRC()
        {
            int dataLength = CnlNums.Length * 4;
            byte[] buffer = new byte[dataLength];
            Buffer.BlockCopy(CnlNums, 0, buffer, 0, dataLength);
            return ScadaUtils.CRC32(buffer, 0, dataLength);
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        public bool Equals(CnlNumList list)
        {
            if (list == null)
                return false;
            else if (list == this || list.ListID == ListID || list.CnlNums == CnlNums)
                return true;
            else if (list.CRC == CRC)
                return list.CnlNums.SequenceEqual(CnlNums);
            else
                return false;
        }
    }
}
