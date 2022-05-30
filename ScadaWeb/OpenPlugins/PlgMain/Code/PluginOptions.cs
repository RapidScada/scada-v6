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
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            RefreshRate = options.GetValueAsInt("RefreshRate", 1000);
            TableArchiveCode = options.GetValueAsString("TableArchiveCode");
            TablePeriod = options.GetValueAsInt("TablePeriod", 60);
            EventArchiveCode = options.GetValueAsString("EventArchiveCode");
            EventCount = options.GetValueAsInt("EventCount", 100);
            EventDepth = options.GetValueAsInt("EventDepth", 2);
            CommandPassword = options.GetValueAsBool("CommandPassword");
            AllowCommandApi = options.GetValueAsBool("AllowCommandApi");
            AllowAuthApi = options.GetValueAsBool("AllowAuthApi");
        }


        /// <summary>
        /// Gets or sets the data refresh rate in milliseconds.
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

        /// <summary>
        /// Gets or sets the archive code to get events.
        /// </summary>
        public string EventArchiveCode { get; set; }

        /// <summary>
        /// Gets or sets the number of events to display.
        /// </summary>
        public int EventCount { get; set; }

        /// <summary>
        /// Gets or sets the number of days to receive events.
        /// </summary>
        public int EventDepth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a password is required to send a command.
        /// </summary>
        public bool CommandPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a command can be sent using web API.
        /// </summary>
        public bool AllowCommandApi { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether login and logout is allowed using web API.
        /// </summary>
        public bool AllowAuthApi { get; set; }
    }
}
