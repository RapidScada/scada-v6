// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Comm.Drivers.DrvMqttClient.View
{
    /// <summary>
    /// The phrases used by the driver.
    /// <para>Фразы, используемые драйвером.</para>
    /// </summary>
    public static class DriverPhrases
    {
        // Scada.Comm.Drivers.DrvMqttClient.View.MqttClientConfigProvider
        public static string FormTitle { get; private set; }
        public static string AddSubscriptionButton { get; private set; }
        public static string AddCommandButton { get; private set; }
        public static string OptionsNode { get; private set; }
        public static string SubscriptionsNode { get; private set; }
        public static string CommandsNode { get; private set; }
        public static string UnnamedSubscription { get; private set; }
        public static string UnnamedCommand { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Comm.Drivers.DrvMqttClient.View.MqttClientConfigProvider");
            FormTitle = dict[nameof(FormTitle)];
            AddSubscriptionButton = dict[nameof(AddSubscriptionButton)];
            AddCommandButton = dict[nameof(AddCommandButton)];
            OptionsNode = dict[nameof(OptionsNode)];
            SubscriptionsNode = dict[nameof(SubscriptionsNode)];
            CommandsNode = dict[nameof(CommandsNode)];
            UnnamedSubscription = dict[nameof(UnnamedSubscription)];
            UnnamedCommand = dict[nameof(UnnamedCommand)];
        }
    }
}
