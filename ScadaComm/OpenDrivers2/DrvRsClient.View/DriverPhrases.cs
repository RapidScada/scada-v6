// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvRsClient.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvRsClient.View.RsClientConfigProvider
        public static string FormTitle { get; private set; }
        public static string AddItemGroupButton { get; private set; }
        public static string AddItemButton { get; private set; }
        public static string ItemGroupsNode { get; private set; }
        public static string UnnamedGroup { get; private set; }
        public static string UnnamedItem { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvRsClient.View.RsClientConfigProvider");
            FormTitle = dict[nameof(FormTitle)];
            AddItemGroupButton = dict[nameof(AddItemGroupButton)];
            AddItemButton = dict[nameof(AddItemButton)];
            ItemGroupsNode = dict[nameof(ItemGroupsNode)];
            UnnamedGroup = dict[nameof(UnnamedGroup)];
            UnnamedItem = dict[nameof(UnnamedItem)];
        }
    }
}
