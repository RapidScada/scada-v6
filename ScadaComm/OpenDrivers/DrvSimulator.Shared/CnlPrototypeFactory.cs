using Scada.Comm.Devices;
using Scada.Data.Const;
using System.Collections.Generic;

namespace Scada.Comm.Drivers.DrvSimulator
{
    internal static class CnlPrototypeFactory
    {
        /// <summary>
        /// The length of the array tag.
        /// </summary>
        public const int ArrayLength = 3;

        public static List<CnlPrototypeGroup> GetCnlPrototypeGroups()
        {
            List<CnlPrototypeGroup> groups = new List<CnlPrototypeGroup>();

            CnlPrototypeGroup group = new CnlPrototypeGroup("Inputs");
            group.AddCnlPrototype("Sin", "Sine");
            group.AddCnlPrototype("Sqr", "Square").SetFormat("OffOn"); // TODO: use constant
            group.AddCnlPrototype("Tr", "Triangle");
            groups.Add(group);

            group = new CnlPrototypeGroup("Outputs");
            group.AddCnlPrototype("DO", "Relay State").Setup(cnl =>
            {
                cnl.CnlTypeID = CnlTypeID.InputOutput;
                cnl.FormatCode = "OffOn"; // TODO: use constant
            });
            group.AddCnlPrototype("AO", "Analog Output").Setup(cnl =>
            {
                cnl.CnlTypeID = CnlTypeID.InputOutput;
            });
            groups.Add(group);

            group = new CnlPrototypeGroup("Random");
            group.AddCnlPrototype("RA", "Array").Setup(cnl => cnl.DataLen = ArrayLength);
            groups.Add(group);

            return groups;
        }

        public static List<CnlPrototype> GetCnlPrototypes()
        {
            List<CnlPrototype> cnlPrototypes = new List<CnlPrototype>();
            GetCnlPrototypeGroups().ForEach(group => cnlPrototypes.AddRange(group.CnlPrototypes));
            return cnlPrototypes;
        }
    }
}
