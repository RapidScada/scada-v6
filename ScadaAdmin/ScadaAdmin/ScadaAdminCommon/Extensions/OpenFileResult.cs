// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions
{
    /// <summary>
    /// Represents a result of opening a file with an extension.
    /// <para>Представляет результат открытия файла расширением.</para>
    /// </summary>
    public class OpenFileResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was handled.
        /// </summary>
        public bool Handled { get; init; }

        /// <summary>
        /// Gets the form to edit the open file.
        /// </summary>
        public Form EditorForm { get; init; }
    }
}
