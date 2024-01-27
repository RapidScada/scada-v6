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
 * Summary  : Represents a table that contains trends
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
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
        public TrendTable()
        {
            IsReady = false;
            TableDate = DateTime.MinValue;
            WritingPeriod = 0;
            TableCapacity = 0;
            Metadata = null;
            Pages = null;
            CnlNumList = null;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TrendTable(DateTime tableDate, int writingPeriod)
            : this()
        {
            TableDate = tableDate;
            WritingPeriod = writingPeriod;
            TableCapacity = GetTableCapacity();
        }


        /// <summary>
        /// Gets or sets a value indicating whether the table is ready for reading and writing.
        /// </summary>
        /// <remarks>This means that the object in memory and the files on disk match.</remarks>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets the date of the data stored in the table.
        /// </summary>
        public DateTime TableDate { get; protected set; }

        /// <summary>
        /// Gets the writing period in seconds.
        /// </summary>
        public int WritingPeriod { get; protected set; }

        /// <summary>
        /// Gets or sets the table capacity.
        /// </summary>
        public int TableCapacity { get; set; }

        /// <summary>
        /// Gets the table metadata.
        /// </summary>
        public TrendTableMeta Metadata { get; protected set; }

        /// <summary>
        /// Gets the pages that store the table data.
        /// </summary>
        public List<TrendTablePage> Pages { get; protected set; }

        /// <summary>
        /// Gets or sets the list of channel numbers for newly allocated pages.
        /// </summary>
        public CnlNumList CnlNumList { get; set; }


        /// <summary>
        /// Gets the table capacity depending on the writing period.
        /// </summary>
        protected int GetTableCapacity()
        {
            if (WritingPeriod <= 0)
                throw new ScadaException("Writing period must be greater than zero.");

            return SecondsPerDay / WritingPeriod;
        }

        /// <summary>
        /// Gets the page capacity depending on the writing period.
        /// </summary>
        protected int GetPageCapacity()
        {
            if (WritingPeriod <= 0)
            {
                throw new ScadaException("Writing period must be greater than zero.");
            }
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
            int pageCount = Metadata != null && Metadata.PageCapacity > 0 ?
                (int)Math.Ceiling((double)TableCapacity / Metadata.PageCapacity) : 0;

            List<TrendTablePage> pages = new List<TrendTablePage>(pageCount);
            DateTime timestamp = TableDate;
            int capacityCounter = 0;

            for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
            {
                int pageCapacity = pageNumber < pageCount ? Metadata.PageCapacity : TableCapacity - capacityCounter;
                DateTime nextTimestamp = timestamp.AddSeconds(WritingPeriod * pageCapacity);

                pages.Add(new TrendTablePage(pageNumber, this,
                    new TrendTableMeta
                    {
                        MinTimestamp = timestamp,
                        MaxTimestamp = nextTimestamp,
                        WritingPeriod = WritingPeriod,
                        PageCapacity = pageCapacity
                    }));

                timestamp = nextTimestamp;
                capacityCounter += pageCapacity;
            }

            return pages;
        }

        /// <summary>
        /// Accepts or rejects data with the specified timestamp.
        /// </summary>
        protected bool AcceptData(DateTime timestamp, bool exactMatchRequired, out bool isExactMatch)
        {
            if (!(TableDate > DateTime.MinValue &&
                Metadata != null && Metadata.WritingPeriod > 0 && Metadata.PageCapacity > 0 &&
                Metadata.MinTimestamp <= timestamp && timestamp <= Metadata.MaxTimestamp))
            {
                isExactMatch = false;
                return false;
            }

            isExactMatch = (int)Math.Round(timestamp.TimeOfDay.TotalMilliseconds) % (WritingPeriod * 1000) == 0;
            return !exactMatchRequired || isExactMatch && timestamp < Metadata.MaxTimestamp;
        }

        /// <summary>
        /// Sets the table metadata based on the table date and writing period.
        /// </summary>
        public void SetDefaultMetadata()
        {
            Metadata = new TrendTableMeta
            {
                MinTimestamp = TableDate,
                MaxTimestamp = TableDate.AddDays(1.0),
                WritingPeriod = WritingPeriod,
                PageCapacity = GetPageCapacity()
            };

            Pages = CreatePages();
        }

        /// <summary>
        /// Sets the table metadata.
        /// </summary>
        public void SetMetadata(TrendTableMeta meta)
        {
            if (meta == null)
                throw new ArgumentNullException(nameof(meta));

            TableDate = meta.MinTimestamp.Date;
            WritingPeriod = meta.WritingPeriod;
            TableCapacity = GetTableCapacity();
            Metadata = meta;
            Pages = CreatePages();
        }

        /// <summary>
        /// Gets the data position depending on the timestamp.
        /// </summary>
        public bool GetDataPosition(DateTime timestamp, PositionKind positionKind,
            out TrendTablePage page, out int indexInPage)
        {
            if (AcceptData(timestamp, positionKind == PositionKind.Exact, out bool isExactMatch))
            {
                int indexWithinTable;
                double timeOfDay = timestamp < Metadata.MaxTimestamp ?
                    timestamp.TimeOfDay.TotalSeconds : SecondsPerDay;

                switch (positionKind)
                {
                    case PositionKind.Floor:
                        indexWithinTable = (int)(timeOfDay / WritingPeriod);
                        break;
                    case PositionKind.FloorExclusive:
                        indexWithinTable = (int)(timeOfDay / WritingPeriod);
                        if (isExactMatch)
                            indexWithinTable--;
                        break;
                    case PositionKind.Ceiling:
                        indexWithinTable = (int)Math.Ceiling(timeOfDay / WritingPeriod);
                        break;
                    default: // PositionKind.Exact:
                        indexWithinTable = (int)Math.Round(timeOfDay / WritingPeriod);
                        break;
                }

                if (indexWithinTable < 0)
                    indexWithinTable = 0;
                else if (indexWithinTable >= TableCapacity)
                    indexWithinTable = TableCapacity - 1;

                int pageIndex = indexWithinTable / Metadata.PageCapacity;

                if (0 <= pageIndex && pageIndex < Pages.Count)
                {
                    page = Pages[pageIndex];
                    indexInPage = indexWithinTable % Metadata.PageCapacity;
                    return true;
                }
            }

            page = null;
            indexInPage = -1;
            return false;
        }
    }
}
