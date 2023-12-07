// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Comm.Config;
using Scada.Comm.Devices;
using Scada.Comm.Drivers.DrvGoogleBigQueue.View.Forms;
using Scada.Forms;
using Scada.Lang;

namespace Scada.Comm.Drivers.DrvGoogleBigQueue.View
{
    /// <summary>
    /// Implements the driver user interface.
    /// </summary>
    public class DrvGoogleBigQueueView : DriverView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DrvGoogleBigQueueView()
             : base()
        {
            CanShowProperties = true;
            CanCreateDevice = true;
        }


        /// <summary>
        /// Gets the driver name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Google Big Queue";
            }
        }

        /// <summary>
        /// Gets the driver description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return "Interacts with controllers via google big queue.";
            }
        }


        /// <summary>
        /// Gets a UI customization object.
        /// </summary>
        protected virtual CustomUi GetCustomUi()
        {
            return new CustomUi();
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "DrvGoogleBigQueue", out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            DriverPhrases.Init();
        }

        /// <summary>
        /// Shows a modal dialog box for editing driver properties.
        /// </summary>
        public override bool ShowProperties()
        {
            new FrmDeviceTemplate(AppDirs, GetCustomUi()).ShowDialog();
            return false;
        }

        /// <summary>
        /// Creates a new device user interface.
        /// </summary>
        public override DeviceView CreateDeviceView(LineConfig lineConfig, DeviceConfig deviceConfig)
        {
            return new DevGoogleBigQueueView(this, lineConfig, deviceConfig, GetCustomUi());
        }
    }
}
