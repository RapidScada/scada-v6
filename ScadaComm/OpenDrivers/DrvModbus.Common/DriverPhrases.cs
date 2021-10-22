// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvModbus
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvModbus.Config.DeviceTemplate
        public static string LoadTemplateError { get; private set; }
        public static string SaveTemplateError { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.Config.DeviceTemplate");
            LoadTemplateError = dict["LoadTemplateError"];
            SaveTemplateError = dict["SaveTemplateError"];
        }
    }
}
