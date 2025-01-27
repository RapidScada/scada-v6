// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Comm.Drivers.DrvModbus.Protocol
{
    /// <summary>
    /// Represents a Modbus element (register).
    /// <para>Представляет элемент (регистр) Modbus.</para>
    /// </summary>
    public class Elem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Elem()
        {
            Name = "";
            ElemType = ElemType.Undefined;
            ByteOrder = null;
            Scaling = "";
            Aux = null;
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
        public int[] ByteOrder { get; set; }

        /// <summary>
        /// Gets or sets the scaling double array.
        /// </summary>
        public string Scaling { get; set; }

        /// <summary>
        /// Gets or sets the auxiliary object.
        /// </summary>
        public object Aux { get; set; }

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

        /// <summary>
        /// Gets the element data length.
        /// </summary>
        public virtual int DataLength
        {
            get
            {
                return ModbusUtils.GetDataLength(ElemType);
            }
        }
    }
}
