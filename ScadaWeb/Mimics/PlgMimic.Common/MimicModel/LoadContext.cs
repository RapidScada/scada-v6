// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Encapsulates information about a loading operation.
    /// <para>Содержит информацию об операции загрузки.</para>
    /// </summary>
    public class LoadContext
    {
        /// <summary>
        /// Gets a value indicating whether a mimic is loading for editing.
        /// </summary>
        public required bool EditMode { get; init; }

        /// <summary>
        /// Gets the IDs of the loaded components for protection purposes.
        /// </summary>
        public HashSet<int> ComponentIDs { get; } = [];

        /// <summary>
        /// Gets the loading errors.
        /// </summary>
        public List<string> Errors { get; } = [];
    }
}
