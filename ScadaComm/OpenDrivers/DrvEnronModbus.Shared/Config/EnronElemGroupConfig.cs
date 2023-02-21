// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;
using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvEnronModbus.Config
{
    /// <summary>
    /// Represents an Enron Modbus element group configuration.
    /// <para>Представляет конфигурацию группы элементов Enron Modbus.</para>
    /// </summary>
    internal class EnronElemGroupConfig : ElemGroupConfig
    {
        /// <summary>
        /// Gets the default element type.
        /// </summary>
        public override ElemType DefaultElemType
        {
            get
            {
                return DataBlock == DataBlock.Custom
                    ? ElemType.Undefined
                    : DataBlock == DataBlock.DiscreteInputs || DataBlock == DataBlock.Coils
                        ? ElemType.Bool
                        : ElemType.Float;
            }
        }

        /// <summary>
        /// Creates a new element configuration.
        /// </summary>
        public override ElemConfig CreateElemConfig()
        {
            return new EnronElemConfig { ElemType = DefaultElemType };
        }
    }
}
