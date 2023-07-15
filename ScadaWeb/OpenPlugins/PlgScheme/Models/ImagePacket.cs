// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgScheme.Model;

namespace Scada.Web.Plugins.PlgScheme.Models
{
    /// <summary>
    /// Represents a package containing scheme images.
    /// <para>Представляет пакет, содержащий изображения схемы.</para>
    /// </summary>
    public class ImagePacket : SchemePacket
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ImagePacket(long viewStamp)
        {
            ViewStamp = viewStamp.ToString();
            EndOfImages = false;
            Images = new List<ImageRecord>();
        }


        /// <summary>
        /// Получить признак, что считаны все изображения схемы.
        /// </summary>
        public bool EndOfImages { get; private set; }

        /// <summary>
        /// Получить изображения схемы.
        /// </summary>
        public List<ImageRecord> Images { get; }


        /// <summary>
        /// Копировать заданные изображения в объект для передачи данных.
        /// </summary>
        public void CopyImages(ICollection<Image> srcImages, int startIndex, int totalDataSize)
        {
            int i = 0;
            int size = 0;

            foreach (Image image in srcImages)
            {
                if (i >= startIndex)
                {
                    Images.Add(new ImageRecord(image));
                    if (image.Data != null)
                        size += image.Data.Length;
                }

                if (size >= totalDataSize)
                    break;

                i++;
            }

            EndOfImages = i == srcImages.Count;
        }
    }
}
