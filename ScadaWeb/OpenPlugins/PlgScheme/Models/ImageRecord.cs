// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;
using System;
using System.IO;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a record containing image data.
    /// <para>Представляет запись, содержащую данные изображения.</para>
    /// </summary>
    public class ImageRecord
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ImageRecord(Image image)
        {
            Name = image.Name ?? "";
            Data = Convert.ToBase64String(image.Data ?? Array.Empty<byte>(), Base64FormattingOptions.None);
            SetMediaType();
        }


        /// <summary>
        /// Получить наименование.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Получить медиа-тип.
        /// </summary>
        public string MediaType { get; private set; }

        /// <summary>
        /// Получить данные в формате base 64.
        /// </summary>
        public string Data { get; }


        /// <summary>
        /// Установить медиа-тип на основе наименования.
        /// </summary>
        private void SetMediaType()
        {
            string ext = Path.GetExtension(Name).ToLowerInvariant();
            if (ext == ".png")
                MediaType = "image/png";
            else if (ext == ".jpg")
                MediaType = "image/jpeg";
            else if (ext == ".gif")
                MediaType = "image/gif";
            else if (ext == ".svg")
                MediaType = "image/svg+xml";
            else
                MediaType = "";
        }
    }
}
