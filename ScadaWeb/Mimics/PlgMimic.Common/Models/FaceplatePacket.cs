// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;
using System.Dynamic;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet containing an entire faceplate.
    /// <para>Представляет пакет, содержащий фейсплейт целиком.</para>
    /// </summary>
    public class FaceplatePacket : PacketBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FaceplatePacket(long mimicKey, Faceplate faceplate)
            : base(mimicKey)
        {
            ArgumentNullException.ThrowIfNull(faceplate, nameof(faceplate));
            Dependencies = faceplate.Dependencies;
            Document = faceplate.Document;
            Components = faceplate.Components;
            Images = faceplate.Images;
        }


        /// <summary>
        /// Gets the faceplate dependencies.
        /// </summary>
        public List<FaceplateMeta> Dependencies { get; }

        /// <summary>
        /// Gets the faceplate document.
        /// </summary>
        public ExpandoObject Document { get; }

        /// <summary>
        /// Gets the components.
        /// </summary>
        public IList<Component> Components { get; }

        /// <summary>
        /// Gets the images.
        /// </summary>
        public IList<Image> Images { get; }
    }
}
