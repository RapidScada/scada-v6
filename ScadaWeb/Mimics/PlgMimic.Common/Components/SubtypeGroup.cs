// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.Components
{
    /// <summary>
    /// Represents a group of subtypes.
    /// <para>Представляет группу подтипов.</para>
    /// </summary>
    public class SubtypeGroup
    {
        /// <summary>
        /// Gets the dictionary prefix for translating subtype fields.
        /// </summary>
        public string DictionaryPrefix { get; init; } = "";

        /// <summary>
        /// Gets the subtype names.
        /// </summary>
        public List<string> Names { get; } = [];
    }
}
