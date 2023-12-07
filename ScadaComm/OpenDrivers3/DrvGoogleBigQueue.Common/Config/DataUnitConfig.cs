﻿// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvGoogleBigQueue.Protocol;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.Config
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
            Address = 0;
        }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the zero-based address of the start element.
        /// </summary>
        public int Address { get; set; }
    }
}
