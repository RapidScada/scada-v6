// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer
{
    /// <summary>
    /// Represents data source options.
    /// <para>Представляет параметры источника данных.</para>
    /// </summary>
    public class OpcUaServerDSO
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public OpcUaServerDSO(OptionList options)
        {
            AutoAccept = options.GetValueAsBool("AutoAccept");
            Username = options.GetValueAsString("Username");
            Password = ScadaUtils.Decrypt(options.GetValueAsString("Password"));
            ConfigFileName = options.GetValueAsString("ConfigFileName");
            DeviceFilter = new List<int>();
            DeviceFilter.AddRange(ScadaUtils.ParseRange(options.GetValueAsString("DeviceFilter"), true, true));
        }


        /// <summary>
        /// Gets or sets a value indicating whether to automatically accept client certificates.
        /// </summary>
        public bool AutoAccept { get; set; }

        /// <summary>
        /// Gets or sets the server username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the server password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the server configuration filename.
        /// </summary>
        public string ConfigFileName { get; set; }

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
            options["AutoAccept"] = AutoAccept.ToLowerString();
            options["Username"] = Username;
            options["Password"] = ScadaUtils.Encrypt(Password);
            options["ConfigFileName"] = ConfigFileName;
            options["DeviceFilter"] = DeviceFilter.ToRangeString();
        }
    }
}
