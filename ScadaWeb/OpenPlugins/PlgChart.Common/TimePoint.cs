// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Text;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// Represents a time point.
    /// <para>Представляет временную точку.</para>
    /// </summary>
    /// <remarks>Each point is an array of [UTC, "local"].</remarks>
    public class TimePoint : ArrayList
    {
        /// <summary>
        /// The number of array elements.
        /// </summary>
        private const int ElemCount = 2;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private TimePoint()
            : base(ElemCount)
        {
        }


        /// <summary>
        /// Creates a new time point.
        /// </summary>
        public static TimePoint Create(long utc, string local)
        {
            return new TimePoint { utc, local };
        }

        /// <summary>
        /// Appends the time point to the string builder.
        /// </summary>
        public static void Append(StringBuilder stringBuilder, long utc, string local)
        {
            stringBuilder
                .Append('[')
                .Append(utc).Append(", \'")
                .Append(local)
                .Append("\'], ");
        }
    }
}
