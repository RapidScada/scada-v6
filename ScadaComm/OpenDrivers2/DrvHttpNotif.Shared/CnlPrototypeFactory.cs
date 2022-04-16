// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Data.Const;
using Scada.Lang;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvHttpNotif
{
    /// <summary>
    /// Creates channel prototypes for a device.
    /// <para>Создает прототипы каналов устройства.</para>
    /// </summary>
    internal static class CnlPrototypeFactory
    {
        /// <summary>
        /// Gets the grouped channel prototypes.
        /// </summary>
        public static CnlPrototypeGroup GetCnlPrototypeGroup()
        {
            CnlPrototypeGroup group = new CnlPrototypeGroup();
            
            group.CnlPrototypes.Add(new CnlPrototype
            {
                Name = Locale.IsRussian ? "Уведомления" : "Notifications",
                CnlTypeID = CnlTypeID.InputOutput,
                TagCode = TagCode.Notif,
                FormatCode = FormatCode.N0
            });

            group.CnlPrototypes.Add(new CnlPrototype
            {
                Name = Locale.IsRussian ? "Запросы" : "Requests",
                CnlTypeID = CnlTypeID.InputOutput,
                TagCode = TagCode.Request,
                FormatCode = FormatCode.N0
            });

            group.CnlPrototypes.Add(new CnlPrototype
            {
                Name = Locale.IsRussian ? "Статус ответа" : "Response status",
                CnlTypeID = CnlTypeID.Input,
                TagCode = TagCode.Response,
                FormatCode = FormatCode.N0
            });

            return group;
        }
    }
}
