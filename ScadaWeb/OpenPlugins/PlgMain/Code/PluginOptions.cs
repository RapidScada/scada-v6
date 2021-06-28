// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;

namespace Scada.Web.Plugins.PlgMain.Code
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

            RefreshRate = options.GetValueAsInt("RefreshRate", 1000);
            TableArchiveCode = options.GetValueAsString("TableArchiveCode");
            TablePeriod = options.GetValueAsInt("TablePeriod", 60);
        }


        /// <summary>
        /// Gets or sets the data refresh rate.
        /// </summary>
        public int RefreshRate { get; set; }

        /// <summary>
        /// Gets or sets the archive code to get data for table views.
        /// </summary>
        public string TableArchiveCode { get; set; }

        /// <summary>
        /// Gets or sets the time period of table view columns.
        /// </summary>
        public int TablePeriod { get; set; }
    }
}
