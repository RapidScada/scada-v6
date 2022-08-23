// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvCnlBasic
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvCnlBasic.View.BasicChannelView
        public static string ChannelTypeNotFound { get; private set; }

        // Scada.Comm.Drivers.DrvCnlBasic.View.Forms.FrmTcpClientChannelOptions
        public static string HostRequired { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvCnlBasic.View.BasicChannelView");
            ChannelTypeNotFound = dict["ChannelTypeNotFound"];
            
            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvCnlBasic.View.Forms.FrmTcpClientChannelOptions");
            HostRequired = dict["HostRequired"];
        }
    }
}
