// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvDsOpcUaServer
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    internal static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvDsOpcUaServer.View.Forms.FrmOpcUaServerDSO
        public static string ConfigDirRequired { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvDsOpcUaServer.View.Forms.FrmOpcUaServerDSO");
            ConfigDirRequired = dict["ConfigDirRequired"];
        }
    }
}
