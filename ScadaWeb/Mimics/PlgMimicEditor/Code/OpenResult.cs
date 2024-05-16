// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimicEditor.Code
{
    /// <summary>
    /// Represents a result of opening a mimic diagram.
    /// <para>Представляет результат открытия мнемосхемы.</para>
    /// </summary>
    public class OpenResult
    {
        /// <summary>
        /// Gets a value indicating whether the mimic has been opened successfully.
        /// </summary>
        public bool IsSuccessful { get; init; }

        /// <summary>
        /// Gets the error occurred while opening the mimic.
        /// </summary>
        public string ErrorMessage { get; init; }

        /// <summary>
        /// Gets the key provided by the editor to access the mimic.
        /// </summary>
        public long EditorKey { get; init; }
    }
}
