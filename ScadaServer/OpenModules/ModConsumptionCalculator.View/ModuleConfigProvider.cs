// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Forms;
using Scada.Server.Modules.ModConsumptionCalculator.Config;

namespace Scada.Server.Modules.ModConsumptionCalculator.View
{
    /// <summary>
    /// Represents an intermediary between a module configuration and a configuration form.
    /// <para>Представляет посредника между конфигурацией модуля и формой конфигурации.</para>
    /// </summary>
    internal class ModuleConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModuleConfigProvider(string configDir)
            : base()
        {
            ConfigFileName = Path.Combine(configDir, ModuleConfig.DefaultFileName);
            Config = new ModuleConfig();
        }
    }
}
