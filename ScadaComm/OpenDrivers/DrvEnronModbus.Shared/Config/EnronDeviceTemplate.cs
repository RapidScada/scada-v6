// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Drivers.DrvModbus.Config;

namespace Scada.Comm.Drivers.DrvEnronModbus.Config
{
    /// <summary>
    /// Represents an Enron Modbus device template.
    /// <para>Представляет шаблон устройства Enron Modbus.</para>
    /// </summary>
    internal class EnronDeviceTemplate : DeviceTemplate
    {
        /// <summary>
        /// Gets the file name for a newly created device template.
        /// </summary>
        public override string NewTemplateFileName => "DrvEnronModbus_NewTemplate.xml";


        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected override void SetToDefault()
        {
            base.SetToDefault();
            Options.ZeroAddr = true;
        }

        /// <summary>
        /// Creates a new element group configuration.
        /// </summary>
        public override ElemGroupConfig CreateElemGroupConfig()
        {
            return new EnronElemGroupConfig();
        }

        /// <summary>
        /// Creates a new command configuration.
        /// </summary>
        public override CmdConfig CreateCmdConfig()
        {
            return new EnronCmdConfig();
        }
    }
}
