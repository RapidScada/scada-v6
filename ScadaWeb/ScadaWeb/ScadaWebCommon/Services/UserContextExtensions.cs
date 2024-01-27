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
 * Module   : ScadaWebCommon
 * Summary  : Provides extensions to the IUserContext interface
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2023
 */

using Scada.Data.Models;

namespace Scada.Web.Services
{
    /// <summary>
    /// Provides extensions to the IUserContext interface.
    /// <para>Предоставляет расширения интерфейса IUserContext.</para>
    /// </summary>
    public static class UserContextExtensions
    {
        /// <summary>
        /// Converts a universal time (UTC) to the time in the user's time zone.
        /// </summary>
        public static DateTime ConvertTimeFromUtc(this IUserContext userContext, DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, userContext.TimeZone);
        }

        /// <summary>
        /// Converts the time in the user's time zone to a universal time (UTC).
        /// </summary>
        public static DateTime ConvertTimeToUtc(this IUserContext userContext, DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : TimeZoneInfo.ConvertTimeToUtc(dateTime, userContext.TimeZone);
        }

        /// <summary>
        /// Creates a time range with UTC timestamps.
        /// </summary>
        public static TimeRange CreateTimeRangeUtc(this IUserContext userContext, 
            DateTime startTime, DateTime endTime, bool endInclusive)
        {
            return new TimeRange(
                userContext.ConvertTimeToUtc(startTime),
                userContext.ConvertTimeToUtc(endTime),
                endInclusive);
        }
    }
}
