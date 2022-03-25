// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvMqtt
{
    /// <summary>
    /// The class provides helper methods for MQTT drivers.
    /// <para>Класс, предоставляющий вспомогательные методы для драйверов MQTT.</para>
    /// </summary>
    public static class MqttUtils
    {
        /// <summary>
        /// The number of characters to preview message.
        /// </summary>
        public const int MessagePreviewLength = 20;

        /// <summary>
        /// Formats the specified value and status to publish.
        /// </summary>
        public static string FormatPayload(string format, string value, int status)
        {
            return format
                .Replace(MessageVar.Value, value, StringComparison.Ordinal)
                .Replace(MessageVar.Status, status.ToString(), StringComparison.Ordinal);
        }
    }
}
