// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Code
{
    /// <summary>
    /// Represents a mimic diagram in runtime mode.
    /// <para>Представляет мнемосхему в режиме выполнения.</para>
    /// </summary>
    public class MimicView : ViewBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public MimicView(View viewEntity)
            : base(viewEntity)
        {
            Mimic = new Mimic();
        }


        /// <summary>
        /// Gets the mimic diagram.
        /// </summary>
        public Mimic Mimic { get; }


        /// <summary>
        /// Gets a resource path relative to the view directory.
        /// </summary>
        private static string GetResourcePath(string mimicDir, string faceplatePath)
        {
            return mimicDir + '\\' + faceplatePath;
        }

        /// <summary>
        /// Loads the view from the specified stream.
        /// </summary>
        public override void LoadView(Stream stream)
        {
            Mimic.Load(stream);
            string mimicDir = Path.GetDirectoryName(ViewEntity.Path);

            foreach (FaceplateMeta faceplateMeta in Mimic.Dependencies)
            {
                Resources.Add(new ViewResource
                {
                    TypeCode = faceplateMeta.TypeName,
                    Path = GetResourcePath(mimicDir, faceplateMeta.Path)
                });
            }
        }

        /// <summary>
        /// Loads the view resource from the specified stream.
        /// </summary>
        public override void LoadResource(ViewResource resource, Stream stream)
        {
            if (!string.IsNullOrEmpty(resource.TypeCode) &&
                !Mimic.Faceplates.ContainsKey(resource.TypeCode))
            {
                Faceplate faceplate = new();
                faceplate.Load(stream);
                Mimic.Faceplates.Add(resource.TypeCode, faceplate);
            }
        }

        /// <summary>
        /// Binds the view to the configuration database.
        /// </summary>
        public override void Bind(ConfigDataset configDataset)
        {

        }
    }
}
