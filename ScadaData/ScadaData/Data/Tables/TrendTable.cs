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
 * Summary  : Represents a table that contains trends
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using System;
using System.Collections.Generic;

namespace Scada.Data.Tables
{
    /// <summary>
    /// Represents a table that contains trends.
    /// <para>Представляет таблицу, которая содержит тренды.</para>
    /// </summary>
    public class TrendTable
    {
        /// <summary>
        /// The number of seconds in a day.
        /// </summary>
        private const int SecondsPerDay = 86400;
        /// <summary>
        /// The maximum page capacity.
        /// </summary>
        private const int MaxPageCapacity = 100;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendTable(DateTime tableDate, int writingPeriod)
        {
            IsReady = false;
            TableDate = tableDate;
            WritingPeriod = writingPeriod;
            PageCapacity = GetPageCapacity();
            Pages = CreatePages();
            CnlNumList = null;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the table is ready for reading and writing.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets the date of the data stored in the table.
        /// </summary>
        public DateTime TableDate { get; }

        /// <summary>
        /// Gets the writing period in seconds.
        /// </summary>
        public int WritingPeriod { get; }

        /// <summary>
        /// Gets the page capacity.
        /// </summary>
        public int PageCapacity { get; }

        /// <summary>
        /// Gets the pages that store the table data.
        /// </summary>
        public List<TrendTablePage> Pages { get; }

        /// <summary>
        /// Gets or sets the list of input channel numbers for newly added pages.
        /// </summary>
        public CnlNumList CnlNumList { get; set; }


        /// <summary>
        /// Gets the page capacity depending on the writing period.
        /// </summary>
        protected int GetPageCapacity()
        {
            if (WritingPeriod >= SecondsPerDay)
            {
                return 1;
            }
            else if (SecondsPerDay % WritingPeriod == 0)
            {
                int periodsPerDay = SecondsPerDay / WritingPeriod;
                return periodsPerDay <= MaxPageCapacity ? periodsPerDay : 60;
            }
            else
            {
                return MaxPageCapacity;
            }
        }

        /// <summary>
        /// Creates the pages.
        /// </summary>
        protected List<TrendTablePage> CreatePages()
        {
            int periodsPerDay = SecondsPerDay / WritingPeriod;
            int pageCount = (int)Math.Ceiling((double)periodsPerDay / PageCapacity);
            List<TrendTablePage> pages = new List<TrendTablePage>(pageCount);
            DateTime timestamp = TableDate;
            int tableCapacity = 0;

            for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
            {
                DateTime nextTimestamp = timestamp.AddSeconds(WritingPeriod);
                int pageCapacity = Math.Min(PageCapacity, periodsPerDay - tableCapacity);

                Pages.Add(new TrendTablePage(this)
                {
                    PageNumber = pageNumber,
                    MinTimestamp = timestamp,
                    MaxTimestamp = nextTimestamp,
                    WritingPeriod = WritingPeriod,
                    PageCapacity = pageCapacity
                });

                timestamp = nextTimestamp;
                tableCapacity += pageCapacity;
            }

            return Pages;
        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        protected bool AcceptData(DateTime timestamp)
        {
            return timestamp.Date == TableDate && 
                (int)Math.Round(timestamp.TimeOfDay.TotalMilliseconds) % (WritingPeriod * 1000) == 0;
        }

        /// <summary>
        /// Gets the data position depending on the timestamp.
        /// </summary>
        public void GetDataPosition(DateTime timestamp, out int pageNumber, out int indexWithinPage)
        {
            if (AcceptData(timestamp))
            {
                int indexWithinTable = (int)Math.Round(timestamp.TimeOfDay.TotalSeconds) / WritingPeriod;
                pageNumber = indexWithinTable / PageCapacity + 1;
                indexWithinPage = indexWithinTable % PageCapacity;
            }
            else
            {
                pageNumber = 0;
                indexWithinPage = 0;
            }
        }
    }
}
