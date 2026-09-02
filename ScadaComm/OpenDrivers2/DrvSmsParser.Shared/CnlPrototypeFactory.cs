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
        /// Creates a channel prototype from the full tag name.
        /// </summary>
        private static CnlPrototype ParseTag(string fullTagName)
        {
            CnlPrototype proto = new()
            {
                CnlTypeID = CnlTypeID.Input
            };

            if (!string.IsNullOrEmpty(fullTagName))
            {
                int idx1 = fullTagName.IndexOf('[');
                int idx2 = fullTagName.IndexOf(']');

                if (idx1 >= 0 && idx1 < idx2)
                {
                    proto.TagCode = fullTagName[(idx1 + 1)..idx2].Trim();
                    proto.Name = fullTagName[(idx2 + 1)..].Trim();
                }
                else
                {
                    proto.TagCode = proto.Name = fullTagName.Trim();
                }
            }

            return proto;
        }

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
                    group.CnlPrototypes.Add(ParseTag(tag));
                }
            }

            return group;
        }
    }
}
