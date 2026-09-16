// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Data.Const;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvSmsParser
{
    /// <summary>
    /// Creates prototypes of SMS Parser channels.
    /// <para>Создает прототипы каналов Парсера SMS.</para>
    /// </summary>
    internal static class CnlPrototypeFactory
    {
        /// <summary>
        /// Gets a general channel prototype group.
        /// </summary>
        public static CnlPrototypeGroup GetGeneralGroup()
        {
            CnlPrototypeGroup group = new(Locale.IsRussian ? "Основные" : "General");
            
            group.CnlPrototypes.Add(new CnlPrototype
            {
                Name = Locale.IsRussian ? "Сообщения" : "Messages",
                CnlTypeID = CnlTypeID.Input,
                TagCode = TagCode.Msg,
                FormatCode = FormatCode.N0
            });

            return group;
        }

        /// <summary>
        /// Gets a custom channel prototype group.
        /// </summary>
        public static CnlPrototypeGroup GetCustomGroup(DeviceTemplate deviceTemplate)
        {
            CnlPrototypeGroup group = new(Locale.IsRussian ? "Пользовательские" : "Custom");

            if (deviceTemplate != null)
            {
                foreach (string tag in deviceTemplate.Tags)
                {
                    TagParser.Parse(tag, out string code, out string name);
                    group.CnlPrototypes.Add(new CnlPrototype
                    {
                        CnlTypeID = CnlTypeID.Input,
                        TagCode = code,
                        Name = name
                    });
                }
            }

            return group;
        }
    }
}
