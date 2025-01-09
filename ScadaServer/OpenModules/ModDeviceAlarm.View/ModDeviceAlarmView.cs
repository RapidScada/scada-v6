// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Lang;
using Scada.Server.Modules.ModDeviceAlarm.View.Forms;

namespace Scada.Server.Modules.ModDeviceAlarm.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// </summary>
    public class ModDeviceAlarmView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDeviceAlarmView()
        {
            CanShowProperties = true;
        }


        /// <summary>
        /// Gets the module name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Device alarm";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return "The module send mail to someone when device alarm. " + 
                    "Supports a channel status is 0,  group of cnl val not change for interval.";
            }
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, ModuleUtils.ModuleCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);
            
            ModulePhrases.Init();
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmModuleConfig(ConfigDataset, AppDirs).ShowDialog() == DialogResult.OK;
        }
    }
}
 