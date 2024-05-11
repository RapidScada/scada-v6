// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a faceplate, user component.
    /// <para>Представляет фейсплейт, пользовательский компонент.</para>
    /// </summary>
    public class Faceplate
    {
        /// <summary>
        /// Gets the main panel, which is the root component of the faceplate.
        /// </summary>
        public Panel MainPanel { get; } = new();

        /// <summary>
        /// Gets the images used by the components.
        /// </summary>
        public Dictionary<string, Image> Images { get; } = [];
    }
}
