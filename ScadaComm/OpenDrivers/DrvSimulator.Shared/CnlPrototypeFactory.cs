// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Devices;
using Scada.Data.Const;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvSimulator
{
    /// <summary>
    /// Creates channel prototypes for a simulator device.
    /// <para>Создает прототипы каналов для симулятора устройства.</para>
    /// </summary>
    internal static class CnlPrototypeFactory
    {
        /// <summary>
        /// The length of the array tag.
        /// </summary>
        private const int ArrayLength = 3;

        /// <summary>
        /// Gets the grouped channel prototypes.
        /// </summary>
        public static List<CnlPrototypeGroup> GetCnlPrototypeGroups()
        {
            List<CnlPrototypeGroup> groups = new List<CnlPrototypeGroup>();

            CnlPrototypeGroup group = new CnlPrototypeGroup("Inputs");
            group.AddCnlPrototype(TagCode.Sin, "Sine");
            group.AddCnlPrototype(TagCode.Sqr, "Square").SetFormat(FormatCode.OffOn);
            group.AddCnlPrototype(TagCode.Tri, "Triangle");
            groups.Add(group);

            group = new CnlPrototypeGroup("Outputs");
            group.AddCnlPrototype(TagCode.DO, "Relay State").Configure(cnl =>
            {
                cnl.CnlTypeID = CnlTypeID.InputOutput;
                cnl.FormatCode = FormatCode.OffOn;
                cnl.QuantityCode = QuantityCode.RelayState;
            });
            group.AddCnlPrototype(TagCode.AO, "Analog Output").Configure(cnl =>
            {
                cnl.CnlTypeID = CnlTypeID.InputOutput;
                cnl.QuantityCode = QuantityCode.Current;
                cnl.UnitCode = UnitCode.Milliampere;
            });
            groups.Add(group);

            group = new CnlPrototypeGroup("Random");
            group.AddCnlPrototype(TagCode.RA, "Array").Configure(cnl => cnl.DataLen = ArrayLength);
            groups.Add(group);

            return groups;
        }
    }
}
