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
        public static string AddVarGroupButton { get; private set; }
        public static string AddVariableButton { get; private set; }
        public static string OptionsNode { get; private set; }
        public static string VarGroupsNode { get; private set; }
        public static string UnnamedGroup { get; private set; }
        public static string UnnamedVariable { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvSnmp.View.SnmpConfigProvider");
            FormTitle = dict[nameof(FormTitle)];
            AddVarGroupButton = dict[nameof(AddVarGroupButton)];
            AddVariableButton = dict[nameof(AddVariableButton)];
            OptionsNode = dict[nameof(OptionsNode)];
            VarGroupsNode = dict[nameof(VarGroupsNode)];
            UnnamedGroup = dict[nameof(UnnamedGroup)];
            UnnamedVariable = dict[nameof(UnnamedVariable)];
        }
    }
}
