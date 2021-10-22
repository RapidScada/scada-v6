// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents an element configuration.
    /// <para>Представляет конфигурацию элемента.</para>
    /// </summary>
    public class ElemConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ElemConfig()
        {
            Name = "";
            ElemType = ElemType.Undefined;
            ByteOrder = "";
        }


        /// <summary>
        /// Gets or sets the element name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the element type.
        /// </summary>
        public ElemType ElemType { get; set; }

        /// <summary>
        /// Gets or sets the byte order.
        /// </summary>
        public string ByteOrder { get; set; }

        /// <summary>
        /// Gets the quantity of addresses.
        /// </summary>
        public virtual int Quantity
        {
            get
            {
                return ModbusUtils.GetQuantity(ElemType);
            }
        }
    }
}
