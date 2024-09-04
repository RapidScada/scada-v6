// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Admin.Extensions.ExtExternalTools
{
    /// <summary>
    /// Specifies the variables supported by arguments.
    /// </summary>
    internal class VarName
    {
        /// <summary>
        /// The file name of the open project.
        /// </summary>
        public const string ProjectFileName = nameof(ProjectFileName);

        /// <summary>
        /// The file name corresponding to the active window
        /// </summary>
        public const string ItemFileName = nameof(ItemFileName);
    }
}
