// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvModbus.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
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
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.View.Controls");
            AddressHint = dict[nameof(AddressHint)];
            ElemRemoveWarning = dict[nameof(ElemRemoveWarning)];

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceProps");
            ConfigDirRequired = dict[nameof(ConfigDirRequired)];
            TemplateNotExists = dict[nameof(TemplateNotExists)];

            dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvModbus.View.Forms.FrmDeviceTemplate");
            TemplateTitle = dict[nameof(TemplateTitle)];
            ElemGroupsNode = dict[nameof(ElemGroupsNode)];
            CommandsNode = dict[nameof(CommandsNode)];
            UnnamedElemGroup = dict[nameof(UnnamedElemGroup)];
            UnnamedElem = dict[nameof(UnnamedElem)];
            UnnamedCommand = dict[nameof(UnnamedCommand)];
            SaveTemplateConfirm = dict[nameof(SaveTemplateConfirm)];
            ElemCntExceeded = dict[nameof(ElemCntExceeded)];
            DuplicatedCodes = dict[nameof(DuplicatedCodes)];
            DuplicatedCmdNums = dict[nameof(DuplicatedCmdNums)];
            EmptyTagCodes = dict[nameof(EmptyTagCodes)];
            EmptyCmdCodes = dict[nameof(EmptyCmdCodes)];
            VerificationPassed = dict[nameof(VerificationPassed)];
            CloneElemConfigConfirm = dict[nameof(CloneElemConfigConfirm)];
        }
    }
}
