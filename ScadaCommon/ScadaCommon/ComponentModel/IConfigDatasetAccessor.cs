// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Models;

namespace Scada.ComponentModel
{
    /// <summary>
    /// Provides access to the configuration database.
    /// <para>Предоставляет доступ к базе конфигурации.</para>
    /// </summary>
    public interface IConfigDatasetAccessor
    {
        /// <summary>
        /// Gets the configuration database.
        /// </summary>
        ConfigDataset ConfigDataset { get; }
    }
}
