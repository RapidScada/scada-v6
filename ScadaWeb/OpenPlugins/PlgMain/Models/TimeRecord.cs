// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Scada.Web.Plugins.PlgMain.Models
{
    /// <summary>
    /// Represents a record containing a timestamp.
    /// <para>Представляет запись, содержащую метку времени.</para>
    /// </summary>
    public struct TimeRecord
    {
        /// <summary>
        /// Specified how local time is formatted.
        /// </summary>
        private const string LocalFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK";


        /// <summary>
        /// Gets or sets the Unix time in milliseconds (UTC).
        /// </summary>
        public long Ut { get; set; }

        /// <summary>
        /// Gets or sets the local time of a user. 
        /// Example: 2021-12-31T23:00:00.000+03:00
        /// </summary>
        public string Lt { get; set; }


        /// <summary>
        /// Creates a new time record.
        /// </summary>
        public static TimeRecord Create(DateTime dateTime, TimeZoneInfo timeZone)
        {           
            if (dateTime.Kind == DateTimeKind.Utc && !timeZone.Equals(TimeZoneInfo.Utc))
                dateTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, timeZone);
            else if (dateTime.Kind == DateTimeKind.Local && !timeZone.Equals(TimeZoneInfo.Local))
                dateTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, timeZone);

            DateTimeOffset dateTimeOffset = new(dateTime, timeZone.GetUtcOffset(dateTime));

            return new TimeRecord
            {
                Ut = dateTimeOffset.ToUnixTimeMilliseconds(),
                Lt = dateTimeOffset.ToString(LocalFormat)
            };
        }
    }
}
