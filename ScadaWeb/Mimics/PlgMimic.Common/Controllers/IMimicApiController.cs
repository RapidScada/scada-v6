// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Api;
using Scada.Web.Plugins.PlgMimic.Models;

namespace Scada.Web.Plugins.PlgMimic.Controllers
{
    /// <summary>
    /// Defines functionality of a web API for loading mimic diagrams.
    /// <para>Определяет функциональность веб-API для загрузки мнемосхем.</para>
    /// </summary>
    public interface IMimicApiController
    {
        /// <summary>
        /// Gets the mimic properties.
        /// </summary>
        Dto<MimicPacket> GetMimicProperties(long key);

        /// <summary>
        /// Gets the faceplate.
        /// </summary>
        Dto<FaceplatePacket> GetFaceplate(long key, string typeName);

        /// <summary>
        /// Gets a range of components.
        /// </summary>
        Dto<ComponentPacket> GetComponents(long key, int index, int count);

        /// <summary>
        /// Gets a range of images.
        /// </summary>
        Dto<ImagePacket> GetImages(long key, int index, int count, int size);
    }
}
