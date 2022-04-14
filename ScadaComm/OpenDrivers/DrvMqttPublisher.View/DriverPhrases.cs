// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvMqttPublisher.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvMqttClient.View.MqttClientConfigProvider
        public static string FormTitle { get; private set; }
        public static string AddItemButton { get; private set; }
        public static string OptionsNode { get; private set; }
        public static string ItemsNode { get; private set; }
        public static string ItemNodeFormat { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvMqttClient.View.MqttClientConfigProvider");
            FormTitle = dict[nameof(FormTitle)];
            AddItemButton = dict[nameof(AddItemButton)];
            OptionsNode = dict[nameof(OptionsNode)];
            ItemsNode = dict[nameof(ItemsNode)];
            ItemNodeFormat = dict[nameof(ItemNodeFormat)];
        }
    }
}
