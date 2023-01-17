// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvSnmp.Config;
using Scada.Comm.Lang;

namespace Scada.Comm.Drivers.DrvSnmp.Logic
{
    /// <summary>
    /// Implements the device logic.
    /// <para>Реализует логику устройства.</para>
    /// </summary>
    internal class DevSnmpLogic : DeviceLogic
    {
        private readonly SnmpDeviceConfig config; // the device configuration
        private bool configError;                 // indicates that that device configuration is not loaded


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevSnmpLogic(ICommContext commContext, ILineContext lineContext, DeviceConfig deviceConfig)
            : base(commContext, lineContext, deviceConfig)
        {
            CanSendCommands = true;
            ConnectionRequired = false;

            config = new SnmpDeviceConfig();
            configError = false;
        }


        /// <summary>
        /// Performs actions when starting a communication line.
        /// </summary>
        public override void OnCommLineStart()
        {
            if (!config.Load(Storage, SnmpDeviceConfig.GetFileName(DeviceNum), out string errMsg))
            {
                Log.WriteLine(CommPhrases.DeviceMessage, Title, errMsg);
                configError = true;
            }
        }

        /// <summary>
        /// Initializes the device tags.
        /// </summary>
        public override void InitDeviceTags()
        {
            if (configError)
                return;

            foreach (VarGroupConfig varGroupConfig in config.VarGroups)
            {
                TagGroup tagGroup = new(varGroupConfig.Name);

                foreach (VariableConfig variableConfig in varGroupConfig.Variables)
                {
                    DeviceTag deviceTag = tagGroup.AddTag(variableConfig.Name /*TagCode*/, variableConfig.Name); // TODO: add tag code?
                    deviceTag.DataType = variableConfig.DataType;
                    deviceTag.DataLen = DeviceTag.CalcDataLength(variableConfig.DataLen, variableConfig.DataType);
                    deviceTag.Format = TagFormat.GetDefault(variableConfig.DataType);
                }

                DeviceTags.AddGroup(tagGroup);
            }
        }
    }
}
