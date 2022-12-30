// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.ComponentModel;
using Scada.Forms;
using Scada.Forms.Forms;
using Scada.Lang;
using Scada.Server.Modules.ModActiveDirectory.Config;

namespace Scada.Server.Modules.ModActiveDirectory.View
{
    /// <summary>
    /// Implements the server module user interface.
    /// <para>Реализует пользовательский интерфейс серверного модуля.</para>
    /// </summary>
    public class ModActiveDirectoryView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModActiveDirectoryView()
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
                    "Аутентификация Active Directory" :
                    "Active Directory Authentication";
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
                    "Модуль проверяет имя и пароль пользователя, используя Active Directory." :
                    "The module validates username and password using Active Directory.";
            }
        }


        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, ModuleUtils.ModuleCode, out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            AttrTranslator.Translate(typeof(ModuleConfig));
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmOptions
            { 
                Config = new ModuleConfig(),
                ConfigFileName = Path.Combine(AppDirs.ConfigDir, ModuleConfig.DefaultFileName)
            }
            .ShowDialog() == DialogResult.OK;
        }
    }
}
