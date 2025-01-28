﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Protocol;

namespace Scada.Comm.Drivers.DrvModbus.Config
{
    /// <summary>
    /// Represents a data unit configuration.
    /// <para>Представляет конфигурацию блока данных.</para>
    /// </summary>
    public abstract class DataUnitConfig
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DataUnitConfig()
        {
            Name = "";
            DataBlock = DataBlock.DiscreteInputs;
            Address = 0;
        }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data block.
        /// </summary>
        public DataBlock DataBlock { get; set; }

        /// <summary>
        /// Gets or sets the zero-based address of the start element.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets the maximum number of elements.
        /// </summary>
        public int MaxElemCnt
        {
            get
            {
                return GetMaxElemCnt(DataBlock);
            }
        }

        /// <summary>
        /// Gets the default element type.
        /// </summary>
        public virtual ElemType DefaultElemType
        {
            get
            {
                return DataBlock == DataBlock.Custom
                    ? ElemType.Undefined
                    : DataBlock == DataBlock.DiscreteInputs || DataBlock == DataBlock.Coils
                        ? ElemType.Bool
                        : ElemType.UShort;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the data type selection is applicable for the data unit.
        /// </summary>
        public virtual bool ElemTypeEnabled
        {
            get
            {
                return DataBlock == DataBlock.InputRegisters || DataBlock == DataBlock.HoldingRegisters;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the byte order is applicable for the data unit.
        /// </summary>
        public virtual bool ByteOrderEnabled
        {
            get
            {
                return DataBlock == DataBlock.InputRegisters || DataBlock == DataBlock.HoldingRegisters;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a scaling is applicable for the elements.
        /// </summary>
        public virtual bool ScalingEnabled
        {
            get
            {
                return DataBlock == DataBlock.InputRegisters || DataBlock == DataBlock.HoldingRegisters;
            }
        }


        /// <summary>
        /// Gets the maximum number of elements depending on the data block.
        /// </summary>
        public virtual int GetMaxElemCnt(DataBlock dataBlock)
        {
            return dataBlock == DataBlock.DiscreteInputs || dataBlock == DataBlock.Coils
                ? 2000
                : 125;
        }
    }
}
