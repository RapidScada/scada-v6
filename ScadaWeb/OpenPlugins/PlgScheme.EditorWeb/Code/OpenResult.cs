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
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }
    }
}
