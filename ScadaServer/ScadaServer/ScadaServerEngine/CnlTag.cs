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
 * Module   : ScadaServerEngine
 * Summary  :  Represents metadata about a channel
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2022
 */

using Scada.Data.Entities;
using Scada.Data.Models;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Represents metadata about a channel.
    /// <para>Представляет метаданные канала.</para>
    /// </summary>
    /// <remarks>
    /// Used for channels of the following types: Input, InputOutput, Calculated, CalculatedOutput.
    /// <para>Используется для каналов следующих типов: Input, InputOutput, Calculated, CalculatedOutput.</para>
    /// </remarks>
    internal class CnlTag
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlTag()
        {
            Index = -1;
            CnlNum = 0;
            Cnl = null;
            Lim = null;
            CalcEngine = null;
            CalcCnlDataFunc = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CnlTag(int index, int cnlNum, Cnl cnl, Lim lim)
        {
            Index = index;
            CnlNum = cnlNum;
            Cnl = cnl ?? throw new ArgumentNullException(nameof(cnl));
            Lim = lim;
            LimCnlIndex = null;
            CalcEngine = null;
            CalcCnlDataFunc = null;
        }


        /// <summary>
        /// Gets the index among the active channels for archiving.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the actual channel number. 
        /// It differs from Cnl.CnlNum if the channel data length is greater than 1.
        /// </summary>
        public int CnlNum { get; }

        /// <summary>
        /// Gets the channel entity.
        /// </summary>
        public Cnl Cnl { get; }

        /// <summary>
        /// Gets the index of the array element if the channel represents an array.
        /// </summary>
        public int ArrIdx => Cnl == null ? 0 : CnlNum - Cnl.CnlNum;

        /// <summary>
        /// Gets the channel limits.
        /// </summary>
        public Lim Lim { get; }

        /// <summary>
        /// Gets or sets the channel indexes that define limits of this channel.
        /// </summary>
        public LimCnlIndex LimCnlIndex { get; set; }

        /// <summary>
        /// Gets or sets the object that calculates channel data.
        /// </summary>
        public CalcEngine CalcEngine { get; set; }

        /// <summary>
        /// Gets or sets the function that calculates channel data.
        /// </summary>
        public Func<CnlData> CalcCnlDataFunc { get; set; }

        /// <summary>
        /// Gets a value indicating whether the input channel has an enabled and non-empty input formula.
        /// </summary>
        public bool InFormulaEnabled => CalcCnlDataFunc != null;
    }
}
