// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvEnronModbus.Logic.Protocol
{
    /// <summary>
    /// Represents an Enron Modbus element.
    /// <para>Представляет элемент Enron Modbus.</para>
    /// </summary>
    internal class EnronElem : Elem
    {
        /// <summary>
        /// Gets the quantity of addresses.
        /// </summary>
        public override int Quantity
        {
            get
            {
                return 1;
            }
        }
    }
}
