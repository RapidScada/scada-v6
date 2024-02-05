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
 * Summary  : Implements the calculator context interface for calculating data in archives
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2023
 */

using Scada.Data.Models;
using Scada.Server.Archives;
using System;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Implements the calculator context interface for calculating data in archives.
    /// <para>Реализует интерфейс контекста калькулятора для расчета данных в архивах.</para>
    /// </summary>
    internal class ArchiveCalcContext : ICalcContext
    {
        private readonly HistoricalArchiveLogic archiveLogic; // the historical archive logic
        private readonly UpdateContext updateContext;         // the context of the current update operation


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveCalcContext(HistoricalArchiveLogic archiveLogic, UpdateContext updateContext)
        {
            this.archiveLogic = archiveLogic ?? throw new ArgumentNullException(nameof(archiveLogic));
            this.updateContext = updateContext ?? throw new ArgumentNullException(nameof(updateContext));
        }


        /// <summary>
        /// Gets the timestamp of the processed data (UTC).
        /// </summary>
        public DateTime Timestamp => updateContext.Timestamp;

        /// <summary>
        /// Gets a value indicating whether the processed data is current data.
        /// </summary>
        public bool IsCurrent => false;


        /// <summary>
        /// Gets the actual channel data.
        /// </summary>
        public CnlData GetCnlData(int cnlNum)
        {
            return archiveLogic.GetCnlData(Timestamp, cnlNum);
        }

        /// <summary>
        /// Gets the previous channel data, if applicable.
        /// </summary>
        public CnlData GetPrevCnlData(int cnlNum)
        {
            return CnlData.Empty;
        }

        /// <summary>
        /// Gets the actual channel timestamp.
        /// </summary>
        public DateTime GetCnlTime(int cnlNum)
        {
            return Timestamp;
        }

        /// <summary>
        /// Gets the previous channel timestamp, if applicable.
        /// </summary>
        public DateTime GetPrevCnlTime(int cnlNum)
        {
            return DateTime.MinValue;
        }

        /// <summary>
        /// Sets the channel data.
        /// </summary>
        public void SetCnlData(int cnlNum, CnlData cnlData)
        {
            archiveLogic.UpdateData(updateContext, cnlNum, cnlData);
        }
    }
}
