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
        // TODO: dictionaries
        // Scada.Comm.Devices.Modbus.UI.FrmDevTemplate
        public static string TemplFormTitle { get; private set; }
        public static string GrsNode { get; private set; }
        public static string CmdsNode { get; private set; }
        public static string DefGrName { get; private set; }
        public static string DefElemName { get; private set; }
        public static string DefCmdName { get; private set; }
        public static string SaveTemplateConfirm { get; private set; }
        public static string ElemCntExceeded { get; private set; }



        // Scada.Comm.Drivers.DrvModbus.Config.DeviceTemplate
        public static string LoadTemplateError { get; private set; }
        public static string SaveTemplateError { get; private set; }

        // Scada.Comm.Drivers.DrvModbus.View.Controls
        public static string AddressHint { get; private set; }
        public static string ElemRemoveWarning { get; private set; }

        // Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceProps
        public static string ConfigDirRequired { get; private set; }
        public static string TemplateNotExists { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.Config.DeviceTemplate");
            LoadTemplateError = dict["LoadTemplateError"];
            SaveTemplateError = dict["SaveTemplateError"];

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.View.Controls");
            AddressHint = dict["AddressHint"];
            ElemRemoveWarning = dict["ElemRemoveWarning"];

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceProps");
            ConfigDirRequired = dict["ConfigDirRequired"];
            TemplateNotExists = dict["TemplateNotExists"];

            dict = Locale.GetDictionary("Temp");
            TemplFormTitle = dict["TemplFormTitle"];
            GrsNode = dict["GrsNode"];
            CmdsNode = dict["CmdsNode"];
            DefGrName = dict["DefGrName"];
            DefElemName = dict["DefElemName"];
            DefCmdName = dict["DefCmdName"];
            SaveTemplateConfirm = dict["SaveTemplateConfirm"];
            ElemCntExceeded = dict["ElemCntExceeded"];
        }
    }
}
