// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSms.Logic.Messaging
{
    /// <summary>
    /// Creates events based on messages.
    /// <para>Создаёт события на основе сообщений.</para>
    /// </summary>
    public static class EventFactory
    {
        /// <summary>
        /// Creates an event based on the message.
        /// </summary>
        public static DeviceEvent CreateDeviceEvent(DeviceTag deviceTag, IMessageItem messageItem)
        {
            ArgumentNullException.ThrowIfNull(deviceTag);
            ArgumentNullException.ThrowIfNull(messageItem);

            return new DeviceEvent(deviceTag)
            {
                Timestamp = DateTime.UtcNow,
                CnlVal = 0.0,
                CnlStat = CnlStatusID.Defined, // has informational severity
                TextFormat = EventTextFormat.CustomText,
                Text = messageItem.Address + "; " + messageItem.Text,
                Descr = string.Format(Locale.IsRussian ?
                    "Сообщение от {0}" :
                    "Message from {0}", messageItem.Address)
            };
        }
    }
}
