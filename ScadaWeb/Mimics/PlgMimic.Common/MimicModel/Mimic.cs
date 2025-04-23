// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Scada.Web.Plugins.PlgMimic.MimicModel
{
    /// <summary>
    /// Represents a mimic diagram.
    /// <para>Представляет мнемосхему.</para>
    /// </summary>
    /// <remarks>It could also be a faceplate being edited.</remarks>
    public class Mimic : MimicBase
    {
        /// <summary>
        /// Gets all mimic components accessible by ID.
        /// </summary>
        public Dictionary<int, Component> ComponentMap { get; } = [];

        /// <summary>
        /// Gets the images accessible by name.
        /// </summary>
        public Dictionary<string, Image> ImageMap { get; } = [];

        /// <summary>
        /// Gets the faceplates accessible by type name.
        /// </summary>
        public Dictionary<string, Faceplate> FaceplateMap { get; } = [];

        /// <summary>
        /// Gets an object that can be used to synchronize access to the mimic.
        /// </summary>
        public object SyncRoot => this;


        /// <summary>
        /// Loads the mimic diagram.
        /// </summary>
        public override void Load(Stream stream)
        {
            base.Load(stream);

            // map components
            foreach (Component component in EnumerateComponents())
            {
                ComponentMap.Add(component.ID, component);
            }

            // map images
            foreach (Image image in Images)
            {
                ImageMap.Add(image.Name, image);
            }
        }

        /// <summary>
        /// Enumerates the components recursively.
        /// </summary>
        public IEnumerable<Component> EnumerateComponents()
        {
            foreach (Component component in Components)
            {
                yield return component;

                foreach (Component childComponent in component.GetAllChildren())
                {
                    yield return childComponent;
                }
            }
        }
    }
}
