// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a mimic diagram.
    /// <para>Представляет мнемосхему.</para>
    /// </summary>
    public class Mimic
    {
        /// <summary>
        /// Gets the dependencies on the faceplates.
        /// </summary>
        public List<FaceplateMeta> Dependencies { get; } = [];

        /// <summary>
        /// Gets the components contained within the mimic.
        /// </summary>
        public List<Component> Components { get; } = [];

        /// <summary>
        /// Gets the images accessed by name.
        /// </summary>
        public Dictionary<string, Image> Images { get; } = [];

        /// <summary>
        /// Gets the faceplates accessed by type name.
        /// </summary>
        public Dictionary<string, Faceplate> Faceplates { get; } = [];


        /// <summary>
        /// Loads the mimic diagram.
        /// </summary>
        public void Load(Stream stream)
        {

        }

        /// <summary>
        /// Saves the mimic diagram.
        /// </summary>
        public void Save(Stream stream)
        {

        }
    }
}
