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
 * Summary  : The class provides helper methods for working with trends
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2020
 */

using Scada.Data.Models;
using System;
using System.Collections.Generic;

namespace Scada
{
    /// <summary>
    /// The class provides helper methods for working with trends.
    /// <para>Класс, предоставляющий вспомогательные методы для работы с трендами.</para>
    /// </summary>
    public static class TrendHelper
    {
        /// <summary>
        /// If the difference between the two timestamps is less than this number of milliseconds, the timestamps are equal.
        /// </summary>
        private const int TimeDiscreteness = 10;


        /// <summary>
        /// Creates a new trend bundle containing the specified trend.
        /// </summary>
        private static TrendBundle CreateTrendBundle(Trend trend)
        {
            TrendBundle trendBundle = new TrendBundle(new int[] { trend.CnlNum }, trend.Points.Count);
            TrendBundle.CnlDataList destTrend = trendBundle.Trends[0];

            foreach (TrendPoint trendPoint in trend.Points)
            {
                trendBundle.Timestamps.Add(trendPoint.Timestamp);
                destTrend.Add(new CnlData(trendPoint.Val, trendPoint.Stat));
            }

            return trendBundle;
        }

        /// <summary>
        /// Merges the trends providing a single timeline.
        /// </summary>
        public static TrendBundle MergeTrends(IList<Trend> trends)
        {
            if (trends == null)
                throw new ArgumentNullException(nameof(trends));

            // simple cases first
            int cnlCnt = trends.Count;

            if (cnlCnt == 0)
                return new TrendBundle(0, 0);

            if (cnlCnt == 1)
                return CreateTrendBundle(trends[0]);

            // full case
            int maxTrendCapacity = 0;
            int[] cnlNums = new int[cnlCnt];
            int[] trendPositions = new int[cnlCnt]; // trend reading positions

            for (int i = 0; i < cnlCnt; i++)
            {
                Trend trend = trends[i];
                cnlNums[i] = trend.CnlNum;
                trendPositions[i] = 0;

                if (maxTrendCapacity < trend.Points.Count)
                    maxTrendCapacity = trend.Points.Count;
            }

            const double CapacityRatio = 1.1;
            TrendBundle trendBundle = new TrendBundle(cnlNums, (int)(maxTrendCapacity * CapacityRatio));

            while (true)
            {
                // determine the minimum timestamp at trend reading positions
                DateTime minTimestamp = DateTime.MaxValue;
                bool endReached = true;

                for (int i = 0; i < cnlCnt; i++)
                {
                    List<TrendPoint> trendPoints = trends[i].Points;
                    int trendPos = trendPositions[i];

                    if (trendPos < trendPoints.Count)
                    {
                        endReached = false;
                        DateTime pointTimestamp = trendPoints[trendPos].Timestamp;

                        if (minTimestamp > pointTimestamp)
                            minTimestamp = pointTimestamp;
                    }
                }

                if (endReached)
                    break;

                // copy data with found timestamp
                trendBundle.Timestamps.Add(minTimestamp);

                for (int i = 0; i < cnlCnt; i++)
                {
                    Trend trend = trends[i];
                    int trendPos = trendPositions[i];
                    CnlData cnlData = CnlData.Empty;

                    if (trendPos < trend.Points.Count)
                    {
                        TrendPoint trendPoint = trend.Points[trendPos];

                        if ((trendPoint.Timestamp - minTimestamp).TotalMilliseconds < TimeDiscreteness)
                        {
                            cnlData = new CnlData(trendPoint.Val, trendPoint.Stat);
                            trendPositions[i]++;
                        }
                    }

                    trendBundle.Trends[i].Add(cnlData);
                }
            }

            return trendBundle;
        }
    }
}
