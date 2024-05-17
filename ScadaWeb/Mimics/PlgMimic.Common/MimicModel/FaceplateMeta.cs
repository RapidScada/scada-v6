// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents information about a faceplate.
    /// <para>Представляет информацию о фейсплейте.</para>
    /// </summary>
    public class FaceplateMeta
    {
        /// <summary>
        /// Gets or sets the name of the faceplate type.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the file path relative to the mimic file.
        /// </summary>
        public string RelativePath { get; set; }
    }
}
