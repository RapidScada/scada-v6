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
        /// Gets the dependencies accessible by type name.
        /// </summary>
        public Dictionary<string, FaceplateMeta> DependencyMap { get; } = [];

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
        public override void Load(Stream stream, LoadContext loadContext)
        {
            base.Load(stream, loadContext);

            // map dependencies
            foreach (FaceplateMeta faceplateMeta in Dependencies)
            {
                DependencyMap.Add(faceplateMeta.TypeName, faceplateMeta);
            }

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
        /// Loads the mimic diagram from file.
        /// </summary>
        public void Load(string fileName, LoadContext loadContext)
        {
            using FileStream mimicStream = new(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Load(mimicStream, loadContext);
        }

        /// <summary>
        /// Loads the faceplates specified in dependencies.
        /// </summary>
        public void LoadFaceplates(string viewDir, bool continueOnError, LoadContext loadContext)
        {
            ArgumentNullException.ThrowIfNull(loadContext, nameof(loadContext));
            int dependencyIndex = 0;

            while (dependencyIndex < Dependencies.Count)
            {
                FaceplateMeta faceplateMeta = Dependencies[dependencyIndex];
                faceplateMeta.HasError = false;
                dependencyIndex++;

                if (!string.IsNullOrEmpty(faceplateMeta.TypeName) &&
                    !FaceplateMap.ContainsKey(faceplateMeta.TypeName))
                {
                    try
                    {
                        string faceplateFileName = Path.Combine(viewDir,
                            ScadaUtils.NormalPathSeparators(faceplateMeta.Path));

                        using FileStream faceplateStream =
                            new(faceplateFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        Faceplate faceplate = new();
                        faceplate.Load(faceplateStream, loadContext);
                        FaceplateMap.Add(faceplateMeta.TypeName, faceplate);
                        faceplate.Dependencies.ForEach(d => Dependencies.Add(d.Transit()));
                    }
                    catch (Exception ex)
                    {
                        if (continueOnError)
                        {
                            faceplateMeta.HasError = true;
                            loadContext.Errors.Add(ex.Message);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reloads faceplates.
        /// </summary>
        public void ReloadFaceplates(string viewDir, LoadContext loadContext)
        {
            ArgumentNullException.ThrowIfNull(loadContext, nameof(loadContext));

            // clean up dependencies
            Dependencies.RemoveAll(d => d.IsTransitive);
            DependencyMap.Clear();
            Dependencies.ForEach(d => DependencyMap.Add(d.TypeName, d));

            // load faceplates
            FaceplateMap.Clear();
            LoadFaceplates(viewDir, true, loadContext);
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
