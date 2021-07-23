// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;

namespace Scada.Web.Plugins.PlgChart.Code
{
    /// <summary>
    /// Represents plugin options.
    /// <para>Представляет параметры плагина.</para>
    /// </summary>
    public class PluginOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PluginOptions(OptionList options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            ChartArchiveCode = options.GetValueAsString("ChartArchiveCode");
            GapBetweenPoints = options.GetValueAsInt("GapBetweenPoints", 90);
        }


        /// <summary>
        /// Gets or sets the archive code to get data for charts.
        /// </summary>
        public string ChartArchiveCode { get; set; }

        /// <summary>
        /// Gets or sets the distance between points to make a gap, sec.
        /// </summary>
        public int GapBetweenPoints { get; set; }
    }
}
