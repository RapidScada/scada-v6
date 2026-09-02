// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSmsParser.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    internal static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvSmsParser.View.Forms.FrmDeviceProperties
        public static string ConfigDirRequired { get; private set; }
        public static string TemplateNotExists { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvSmsParser.View.Forms.FrmDeviceProperties");
            ConfigDirRequired = dict[nameof(ConfigDirRequired)];
            TemplateNotExists = dict[nameof(TemplateNotExists)];
        }
    }
}
