// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvMqttClient.Config;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Forms.Forms;

namespace Scada.Comm.Drivers.DrvMqttClient.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevMqttClientView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevMqttClientView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            new FrmModuleConfig(new MqttClientConfigProvider(AppDirs.ConfigDir, DeviceNum)).ShowDialog();
            return false;
        }

        /// <summary>
        /// Gets the default polling options for the device.
        /// </summary>
        public override PollingOptions GetPollingOptions()
        {
            return PollingOptions.CreateWithDefaultDelay();
        }

        /// <summary>
        /// Gets the channel prototypes for the device.
        /// </summary>
        public override ICollection<CnlPrototype> GetCnlPrototypes()
        {
            MqttClientDeviceConfig config = new();

            if (!config.Load(Path.Combine(AppDirs.ConfigDir, MqttClientDeviceConfig.GetFileName(DeviceNum)),
                out string errMsg))
            {
                throw new ScadaException(errMsg);
            }

            // create channels for subscriptions
            List<CnlPrototype> cnlPrototypes = new();
            int eventMask = new EventMask { Enabled = true, StatusChange = true, Command = true }.Value;
            int cmdEventMask = new EventMask { Enabled = true, Command = true }.Value;

            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                if (subscriptionConfig.JsEnabled && subscriptionConfig.SubItems.Count > 0)
                {
                    foreach (string subItem in subscriptionConfig.SubItems)
                    {
                        cnlPrototypes.Add(new CnlPrototype
                        {
                            Name = subscriptionConfig.DisplayName + "." + subItem,
                            CnlTypeID = CnlTypeID.Input,
                            TagCode = subscriptionConfig.TagCode + "." + subItem,
                            EventMask = eventMask
                        });
                    }
                }
                else
                {
                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Name = subscriptionConfig.DisplayName,
                        CnlTypeID = subscriptionConfig.ReadOnly ? CnlTypeID.Input : CnlTypeID.InputOutput,
                        TagCode = subscriptionConfig.TagCode,
                        EventMask = eventMask
                    });
                }
            }

            // create channels for commands
            foreach (CommandConfig commandConfig in config.Commands)
            {
                cnlPrototypes.Add(new CnlPrototype
                {
                    Name = commandConfig.DisplayName,
                    CnlTypeID = CnlTypeID.Output,
                    TagCode = commandConfig.CmdCode,
                    EventMask = cmdEventMask
                });
            }

            return cnlPrototypes;
        }
    }
}
