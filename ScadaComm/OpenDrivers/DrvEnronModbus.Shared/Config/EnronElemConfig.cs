// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;

namespace Scada.Comm.Drivers.DrvEnronModbus.Config
{
    /// <summary>
    /// Represents an Enron Modbus element configuration.
    /// <para>Представляет конфигурацию элемента Enron Modbus.</para>
    /// </summary>
    internal class EnronElemConfig : ElemConfig
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
