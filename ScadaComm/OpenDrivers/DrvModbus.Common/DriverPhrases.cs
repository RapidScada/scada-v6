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

        // Scada.Comm.Drivers.DrvModbus.View.Controls
        public static string AddressHint { get; private set; }
        public static string ElemRemoveWarning { get; private set; }

        // Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceProps
        public static string ConfigDirRequired { get; private set; }
        public static string TemplateNotExists { get; private set; }

        // Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceTemplate
        public static string TemplateTitle { get; private set; }
        public static string ElemGroupsNode { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string UnnamedElemGroup { get; private set; }
        public static string UnnamedElem { get; private set; }
        public static string UnnamedCommand { get; private set; }
        public static string SaveTemplateConfirm { get; private set; }
        public static string ElemCntExceeded { get; private set; }
        public static string DuplicatedCodes { get; private set; }
        public static string DuplicatedCmdNums { get; private set; }
        public static string EmptyTagCodes { get; private set; }
        public static string EmptyCmdCodes { get; private set; }
        public static string VerificationPassed { get; private set; }
        public static string CloneElemConfigConfirm { get; private set; }

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

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceTemplate");
            TemplateTitle = dict["TemplateTitle"];
            ElemGroupsNode = dict["ElemGroupsNode"];
            CommandsNode = dict["CommandsNode"];
            UnnamedElemGroup = dict["UnnamedElemGroup"];
            UnnamedElem = dict["UnnamedElem"];
            UnnamedCommand = dict["UnnamedCommand"];
            SaveTemplateConfirm = dict["SaveTemplateConfirm"];
            ElemCntExceeded = dict["ElemCntExceeded"];
            DuplicatedCodes = dict["DuplicatedCodes"];
            DuplicatedCmdNums = dict["DuplicatedCmdNums"];
            EmptyTagCodes = dict["EmptyTagCodes"];
            EmptyCmdCodes = dict["EmptyCmdCodes"];
            VerificationPassed = dict["VerificationPassed"];
            CloneElemConfigConfirm = dict["CloneElemConfigConfirm"];
        }
    }
}
