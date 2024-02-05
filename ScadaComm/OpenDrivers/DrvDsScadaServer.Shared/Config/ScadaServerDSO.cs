﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvDsScadaServer.Config
{
    /// <summary>
    /// Represents data source options.
    /// <para>Представляет параметры источника данных.</para>
    /// </summary>
    internal class ScadaServerDSO
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ScadaServerDSO(OptionList options)
        {
            ReadConfigDb = options.GetValueAsBool("ReadConfigDb", true);
            UseDefaultConn = options.GetValueAsBool("UseDefaultConn", true);
            Connection = options.GetValueAsString("Connection");
            MaxQueueSize = options.GetValueAsInt("MaxQueueSize", 1000);
            DataLifetime = options.GetValueAsInt("DataLifetime", 3600);
            ClientLogEnabled = options.GetValueAsBool("ClientLogEnabled", false);
            DeviceFilter = new List<int>();
            DeviceFilter.AddRange(ScadaUtils.ParseRange(options.GetValueAsString("DeviceFilter"), true, true));
        }


        /// <summary>
        /// Gets or sets a value indicating whether the data source can read the configuration database.
        /// </summary>
        public bool ReadConfigDb { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a connection specified in the application configuration.
        /// </summary>
        public bool UseDefaultConn { get; set; }

        /// <summary>
        /// Gets or sets the connection name.
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Gets or sets the maximum queue size.
        /// </summary>
        public int MaxQueueSize { get; set; }

        /// <summary>
        /// Gets or sets the data lifetime in the queue, in seconds.
        /// </summary>
        public int DataLifetime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to write client communication log.
        /// </summary>
        public bool ClientLogEnabled { get; set; }

        /// <summary>
        /// Gets the device IDs that filter data sent to the server.
        /// </summary>
        public List<int> DeviceFilter { get; private set; }


        /// <summary>
        /// Adds the options to the list.
        /// </summary>
        public void AddToOptionList(OptionList options)
        {
            options.Clear();
            options["ReadConfigDb"] = ReadConfigDb.ToLowerString();
            options["UseDefaultConn"] = UseDefaultConn.ToLowerString();
            options["Connection"] = Connection;
            options["MaxQueueSize"] = MaxQueueSize.ToString();
            options["DataLifetime"] = DataLifetime.ToString();
            options["ClientLogEnabled"] = ClientLogEnabled.ToLowerString();
            options["DeviceFilter"] = DeviceFilter.ToRangeString();
        }
    }
}
