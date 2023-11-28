// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvGooglePubSub.Config;
using Scada.Data.Const;
using Scada.Data.Models;
using Scada.Forms.Forms;

namespace Scada.Comm.Drivers.DrvGooglePubSub.View
{
    /// <summary>
    /// Implements the device user interface.
    /// <para>Реализует пользовательский интерфейс устройства.</para>
    /// </summary>
    internal class DevGooglePubSubView : DeviceView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DevGooglePubSubView(DriverView parentView, LineConfig lineConfig, DeviceConfig deviceConfig)
            : base(parentView, lineConfig, deviceConfig)
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Shows a modal dialog box for editing device properties.
        /// </summary>
        public override bool ShowProperties()
        {
            new FrmModuleConfig(new GooglePubSubConfigProvider(AppDirs.ConfigDir, DeviceNum)).ShowDialog();
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
            GooglePubSubDeviceConfig config = new();

            if (!config.Load(Path.Combine(AppDirs.ConfigDir, GooglePubSubDeviceConfig.GetFileName(DeviceNum)),
                out string errMsg))
            {
                throw new ScadaException(errMsg);
            }

            // create channels for subscriptions
            List<CnlPrototype> cnlPrototypes = new();
            int eventMask = new EventMask { Enabled = true, StatusChange = true, Command = true }.Value;
            int timeEventMask = new EventMask { Enabled = false, StatusChange = false, Command = false }.Value;
            //int cmdEventMask = new EventMask { Enabled = true, Command = true }.Value;

            foreach (SubscriptionConfig subscriptionConfig in config.Subscriptions)
            {
                if (subscriptionConfig.SubItems.Count > 0)
                {
                    for (int i = 0, cnt = subscriptionConfig.SubItems.Count; i < cnt; i++)
                    {
                        string suffix = "." + subscriptionConfig.SubItems[i];
                        cnlPrototypes.Add(new CnlPrototype
                        {
                            Name = subscriptionConfig.DisplayName + suffix,
                            CnlTypeID = CnlTypeID.Input,
                            TagCode = CalcTagCode(subscriptionConfig.TagCode, subscriptionConfig.SubItems[i]),
                            EventMask = eventMask
                        });
                        cnlPrototypes.Add(new CnlPrototype
                        {
                            Name = subscriptionConfig.DisplayName + suffix + ".time",
                            CnlTypeID = CnlTypeID.Input,
                            DataTypeID = DataTypeID.Int64,
                            FormatCode = "DateTime",
                            TagCode = DriverUtils.GetPointTime(CalcTagCode(subscriptionConfig.TagCode, subscriptionConfig.SubItems[i])),
                            EventMask = timeEventMask
                        });
                    }
                }
                else
                {
                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Name = subscriptionConfig.DisplayName,
                        CnlTypeID = CnlTypeID.Input,
                        TagCode = subscriptionConfig.TagCode,
                        EventMask = eventMask
                    }); 
                    cnlPrototypes.Add(new CnlPrototype
                    {
                        Name = subscriptionConfig.DisplayName + ".time",
                        CnlTypeID = CnlTypeID.Input,
                        DataTypeID = DataTypeID.Int64,
                        FormatCode = "DateTime",
                        TagCode = DriverUtils.GetPointTime(subscriptionConfig.TagCode),
                        EventMask = timeEventMask
                    });
                }
            }

            return cnlPrototypes;
        }

        private string CalcTagCode(string tagCode, string suffix)
        {
            if (string.IsNullOrEmpty(tagCode)) return suffix;
            return tagCode + "." + suffix;
        }
    }
}
