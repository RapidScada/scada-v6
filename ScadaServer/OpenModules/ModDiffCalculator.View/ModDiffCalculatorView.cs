// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using Scada.Server.Modules.ModDiffCalculator.Config;

namespace Scada.Server.Modules.ModDiffCalculator.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModDiffCalculatorView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModDiffCalculatorView()
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
                return Locale.IsRussian ?
                    "Калькулятор разности" :
                    "Difference Calculator";
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Модуль рассчитывает изменения значений каналов с заданной периодичностью." :
                    "The module calculates changes of channel values with a specified period.";
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
            AttrTranslator.Translate(typeof(GeneralOptions));
            AttrTranslator.Translate(typeof(GroupConfig));
            AttrTranslator.Translate(typeof(ItemConfig));
            AttrTranslator.Translate(typeof(PeriodType));
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            ModuleUtils.ConfigDataset = ConfigDataset;
            return new FrmModuleConfig(new ModuleConfigProvider(AppDirs.ConfigDir))
                .ShowDialog() == DialogResult.OK;
        }
    }
}
