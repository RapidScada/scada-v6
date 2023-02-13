// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvEnronModbus.Logic.Protocol
{
    /// <summary>
    /// Represents a group of Enron Modbus elements.
    /// <para>Представляет группу элементов Enron Modbus.</para>
    /// </summary>
    internal class EnronElemGroup : ElemGroup
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnronElemGroup(DataBlock dataBlock)
            : base(dataBlock)
        {
        }

        /// <summary>
        /// Creates a new Modbus element.
        /// </summary>
        public override Elem CreateElem()
        {
            return new EnronElem();
        }
    }
}
