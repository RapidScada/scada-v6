// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Data.Const;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSms
{
    /// <summary>
    /// Creates channel prototypes for a device.
    /// <para>Создает прототипы каналов устройства.</para>
    /// </summary>
    internal static class CnlPrototypeFactory
    {
        /// <summary>
        /// Gets a group of channel prototypes.
        /// </summary>
        public static CnlPrototypeGroup GetGroup()
        {
            CnlPrototypeGroup group = new CnlPrototypeGroup();

            group.CnlPrototypes.Add(new CnlPrototype
            {
                Name = Locale.IsRussian ? "Сообщения" : "Messages",
                CnlTypeID = CnlTypeID.InputOutput,
                TagCode = TagCode.Msg,
                FormatCode = FormatCode.N0
            });

            group.CnlPrototypes.Add(new CnlPrototype
            {
                Name = Locale.IsRussian ? "AT-комманды" : "AT commands",
                CnlTypeID = CnlTypeID.InputOutput,
                TagCode = TagCode.AtCmd,
                FormatCode = FormatCode.N0
            });

            return group;
        }
    }
}
