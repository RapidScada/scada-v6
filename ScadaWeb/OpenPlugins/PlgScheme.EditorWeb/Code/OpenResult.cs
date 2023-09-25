// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgScheme.Editor.Code
{
    /// <summary>
    /// Represents the result of opening a scheme.
    /// <para>Представляет результат открытия схемы.</para>
    /// </summary>
    public class OpenResult
    {
        /// <summary>
        /// Gets a value indicating whether the scheme has been opened successfully.
        /// </summary>
        public bool IsSuccessful { get; init; }

        /// <summary>
        /// Gets the error occurred while opening the schema.
        /// </summary>
        public string ErrorMessage { get; init; }

        /// <summary>
        /// Gets the ID of the editor instance that works on the scheme.
        /// </summary>
        public long EditorID { get; init; }
    }
}
