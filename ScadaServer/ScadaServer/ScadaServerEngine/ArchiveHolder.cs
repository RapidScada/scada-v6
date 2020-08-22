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
 * Module   : ScadaServerEngine
 * Summary  : Holds archives classified by archive kinds
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Log;
using Scada.Server.Archives;
using System;
using System.Collections.Generic;

namespace Scada.Server.Engine
{
    /// <summary>
    /// Holds archives classified by archive kinds.
    /// <para>Содержит архивы, классифицированные по видам.</para>
    /// </summary>
    internal class ArchiveHolder
    {
        private readonly ILog log; // the application log


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArchiveHolder(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException("log");
            CurrentArchives = new List<CurrentArchiveLogic>();
            HistoricalArchives = new List<HistoricalArchiveLogic>();
            EventArchives = new List<EventArchiveLogic>();
        }


        /// <summary>
        /// Gets the current archives.
        /// </summary>
        public List<CurrentArchiveLogic> CurrentArchives { get; }

        /// <summary>
        /// Gets the historical archives.
        /// </summary>
        public List<HistoricalArchiveLogic> HistoricalArchives { get; }

        /// <summary>
        /// Gets the event archives.
        /// </summary>
        public List<EventArchiveLogic> EventArchives { get; }


        /// <summary>
        /// Calls the ReadCurrentData method of the current archives until a successful result is obtained.
        /// </summary>
        public void ReadCurrentData(ICurrentData curData)
        {

        }

        /// <summary>
        /// Calls the ProcessData method of the current and historical archives.
        /// </summary>
        public void ProcessData(ICurrentData curData)
        {

        }

        /// <summary>
        /// Writes the slice of input channels to the historical archives, performing calculations.
        /// </summary>
        public void WriteSlice(Slice slice, int archiveMask, bool applyFormulas)
        {
            // TODO: write slice to archives
            Calculator calc = null;
            Dictionary<int, CnlTag> cnlTags = null;
            List<CnlTag> calcCnlTags = null;
            HistoricalArchiveLogic archiveLogic = null;

            DateTime timestamp = slice.Timestamp;
            calc.BeginCalculation(new ArchiveCalcContext(archiveLogic, timestamp));
            archiveLogic.BeginUpdate();

            // calculate input channels which are written
            for (int i = 0, cnlCnt = slice.CnlNums.Length; i < cnlCnt; i++)
            {
                int cnlNum = slice.CnlNums[i];

                if (cnlTags.TryGetValue(cnlNum, out CnlTag cnlTag))
                {
                    CnlData newCnlData = slice.CnlData[i];

                    if (applyFormulas && cnlTag.InCnl.FormulaEnabled &&
                        cnlTag.InCnl.CnlTypeID == CnlTypeID.Measured)
                    {
                        newCnlData = calc.CalcCnlData(cnlTag, newCnlData);
                    }

                    archiveLogic.WriteCnlData(cnlNum, timestamp, newCnlData);
                }
            }

            // calculate input channels of the calculated type
            foreach (CnlTag cnlTag in calcCnlTags)
            {
                CnlData arcCnlData = archiveLogic.GetCnlData(cnlTag.CnlNum, timestamp);
                CnlData newCnlData = calc.CalcCnlData(cnlTag, arcCnlData);

                if (arcCnlData != newCnlData)
                    archiveLogic.WriteCnlData(cnlTag.CnlNum, timestamp, newCnlData);
            }

            // update channel status according to limits and other conditions, similar to CoreLogic.UpdateCnlStatus
            //if (newCnlData.Stat == CnlStatusID.Defined)
            //    newCnlData.Stat = CnlStatusID.Archival;

            archiveLogic.EndUpdate();
            calc.EndCalculation();
        }

        /// <summary>
        /// Calls the WriteEvent method of the event archives.
        /// </summary>
        public void WriteEvent(Event ev, int archiveMask)
        {
            log.WriteAction(string.Format("Created event with ID = {0}, CnlNum = {1}, OutCnlNum = {2}", 
                ev.EventID, ev.CnlNum, ev.OutCnlNum));
        }

        /// <summary>
        /// Calls the AckEvent method of the event archives.
        /// </summary>
        public void AckEvent(long eventID, DateTime timestamp, int userID)
        {

        }

        /// <summary>
        /// Calls the DeleteOutdatedData method of the archives.
        /// </summary>
        public virtual void DeleteOutdatedData()
        {
        }
    }
}
