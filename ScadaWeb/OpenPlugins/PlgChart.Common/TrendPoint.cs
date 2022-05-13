// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;
using System.Collections;
using System.Globalization;
using System.Text;

namespace Scada.Web.Plugins.PlgChart
{
    /// <summary>
    /// Represents a trend point.
    /// <para>Представляет точку тренда.</para>
    /// </summary>
    /// <remarks>Each point is an array of [value, status, "text"].</remarks>
    public class TrendPoint : ArrayList
    {
        /// <summary>
        /// The number of array elements.
        /// </summary>
        private const int ElemCount = 3;
        

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        private TrendPoint()
            : base(ElemCount)
        {
        }


        /// <summary>
        /// Creates a new trend point.
        /// </summary>
        public static TrendPoint Create(CnlData cnlData, string text)
        {
            return new TrendPoint
            {
                double.IsNaN(cnlData.Val) ? 0.0 : cnlData.Val,
                cnlData.Stat,
                text
            };
        }

        /// <summary>
        /// Appends the trend point to the string builder.
        /// </summary>
        public static void Append(StringBuilder stringBuilder, CnlData cnlData, string text)
        {
            // HttpUtility.JavaScriptStringEncode() is skipped for performance
            stringBuilder
                .Append('[')
                .Append(double.IsNaN(cnlData.Val) ? "0" : cnlData.Val.ToString(CultureInfo.InvariantCulture))
                .Append(", ")
                .Append(cnlData.Stat).Append(", '")
                .Append(text).Append("'], ");
        }
    }
}
