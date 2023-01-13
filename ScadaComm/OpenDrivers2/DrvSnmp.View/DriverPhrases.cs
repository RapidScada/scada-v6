// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSnmp.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvSnmp.View.SnmpConfigProvider
        public static string FormTitle { get; private set; }
        public static string OptionsNode { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvSnmp.View.SnmpConfigProvider");
            FormTitle = dict[nameof(FormTitle)];
            OptionsNode = dict[nameof(OptionsNode)];
        }
    }
}
