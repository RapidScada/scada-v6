// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Config;

namespace Scada.Comm.Drivers.DrvSmsParser.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevSmsParserLogic : DeviceLogic
    {
        private TimeSpan dataLifetime;       // specifies when tag values should be invalidated
        private bool useDataLifetime;        // indicates that lifetime is used
        private DateTime[] updateTimestamps; // the update timestamps by device tag


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSmsParserLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            dataLifetime = TimeSpan.Zero;
            useDataLifetime = false;
            updateTimestamps = null;

            ConnectionRequired = false;
        }

        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            dataLifetime = TimeSpan.FromSeconds(LineContext.LineConfig.CustomOptions.GetValueAsInt("DataLifetime"));
            useDataLifetime = dataLifetime > TimeSpan.Zero;
        }
        
        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {

        }

        /// <summary>
        /// Performs a communication session.
        /// </summary>
        public override void Session()
        {

        }
    }
}
