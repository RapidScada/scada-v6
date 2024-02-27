// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.Server.Modules.ModDiffCalculator
{
    /// <summary>
    /// The class provides helper methods for the module.
    /// <para>Класс, предоставляющий вспомогательные методы для модуля.</para>
    /// </summary>
    internal static class ModuleUtils
    {
        /// <summary>
        /// The module code.
        /// </summary>
        public const string ModuleCode = "ModDiffCalculator";

        /// <summary>
        /// Gets or sets the configuration database required for editing the module configuration.
        /// </summary>
        public static ConfigDataset ConfigDataset { get; set; } = null;
    }
}
