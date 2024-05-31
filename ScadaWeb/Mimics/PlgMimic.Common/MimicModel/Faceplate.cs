// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Dynamic;

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a faceplate, user component.
    /// <para>Представляет фейсплейт, пользовательский компонент.</para>
    /// </summary>
    public class Faceplate
    {
        /// <summary>
        /// Gets the faceplate document that groups its properties.
        /// </summary>
        public ExpandoObject Document { get; } = new();

        /// <summary>
        /// Gets the components contained within the faceplate.
        /// </summary>
        public List<Component> Components { get; } = [];

        /// <summary>
        /// Gets the images used by the components.
        /// </summary>
        public Dictionary<string, Image> Images { get; } = [];


        /// <summary>
        /// Loads the faceplate.
        /// </summary>
        public void Load(Stream stream)
        {

        }

        /// <summary>
        /// Saves the faceplate.
        /// </summary>
        public void Save(Stream stream)
        {

        }
    }
}
