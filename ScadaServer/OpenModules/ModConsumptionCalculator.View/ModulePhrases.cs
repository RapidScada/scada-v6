// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Lang;

namespace Scada.Server.Modules.ModConsumptionCalculator.View
{
    /// <summary>
    /// The phrases used by the module.
    /// <para>Фразы, используемые модулем.</para>
    /// </summary>
    internal class ModulePhrases
    {
        // Scada.Server.Modules.ModConsumptionCalculator.View.ModuleConfigProvider
        public static string GeneralOptionsNode { get; private set; }

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Modules.ModConsumptionCalculator.View.ModuleConfigProvider");
            GeneralOptionsNode = dict[nameof(GeneralOptionsNode)];
        }
    }
}
