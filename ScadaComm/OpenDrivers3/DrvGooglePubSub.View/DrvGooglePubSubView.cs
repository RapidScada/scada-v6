// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvGooglePubSub.Config;
using Scada.ComponentModel;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvGooglePubSub.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// <para>Реализует пользовательский интерфейс драйвера.</para>
    /// </summary>
    public class DrvGooglePubSubView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvGooglePubSubView()
            : base()
        {
            CanCreateDevice = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Google Cloud Pub/Sub";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return "Receives device data from Google Cloud subscribe.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, DriverUtils.DriverCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
            AttrTranslator.Translate(typeof(DeviceOptions));
            AttrTranslator.Translate(typeof(BaseItemConfig));
            AttrTranslator.Translate(typeof(SubscriptionConfig));
            AttrTranslator.Translate(typeof(CommandConfig));
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevGooglePubSubView(this, lineConfig, deviceConfig);
        }
    }
}
