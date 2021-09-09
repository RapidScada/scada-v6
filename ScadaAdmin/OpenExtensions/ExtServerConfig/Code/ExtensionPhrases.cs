// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Admin.Extensions.ExtServerConfig.Code
{
    /// <summary>
    /// The phrases used by the extension.
    /// <para>Фразы, используемые расширением.</para>
    /// </summary>
    public class ExtensionPhrases
    {
        // Scada.Admin.Extensions.ExtServerConfig.ExtServerConfigLogic
        public static string GeneralOptionsNode { get; private set; }
        public static string ModulesNode { get; private set; }

        // Scada.Admin.Extensions.ExtServerConfig.Forms.FrmGeneralOptions
        public static string IncorrectSecretKey { get; private set; }

        // Scada.Admin.Extensions.ExtServerConfig.Forms.FrmModules
        public static string ModuleNotFound { get; private set; }


        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtServerConfig.ExtServerConfigLogic");
            GeneralOptionsNode = dict["GeneralOptionsNode"];
            ModulesNode = dict["ModulesNode"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtServerConfig.Forms.FrmGeneralOptions");
            IncorrectSecretKey = dict["IncorrectSecretKey"];

            dict = Locale.GetDictionary("Scada.Admin.Extensions.ExtServerConfig.Forms.FrmModules");
            ModuleNotFound = dict["ModuleNotFound"];
        }
    }
}
