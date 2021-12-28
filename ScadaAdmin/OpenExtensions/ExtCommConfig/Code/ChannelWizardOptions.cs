// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Config;
using System;

namespace Scada.Admin.Extensions.ExtCommConfig.Code
{
    /// <summary>
    /// Represents the channel wizard options.
    /// <para>Представляет параметры мастера каналов.</para>
    /// </summary>
    public class ChannelWizardOptions
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ChannelWizardOptions(OptionList options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            Multiplicity = options.GetValueAsInt("Multiplicity", 100);
            Shift = options.GetValueAsInt("Shift", 1);
            Gap = options.GetValueAsInt("Gap", 10);
            PrependDeviceName = options.GetValueAsBool("PrependDeviceName", true);
        }


        /// <summary>
        /// Gets or sets the multiplicity of the first channel of a device.
        /// </summary>
        public int Multiplicity { get; set; }

        /// <summary>
        /// Gets or sets the shift of the first channel of a device.
        /// </summary>
        public int Shift { get; set; }

        /// <summary>
        /// Gets or sets the gap between channel numbers of different devices.
        /// </summary>
        public int Gap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prepend a device name in channel names.
        /// </summary>
        public bool PrependDeviceName { get; set; }
    }
}
