// Copyright (c) Rapid Software LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Scada.Web.Plugins.PlgMimic.MimicModel;

namespace Scada.Web.Plugins.PlgMimic.Models
{
    /// <summary>
    /// Represents a packet containing mimic images.
    /// <para>Представляет пакет, содержащий изображения схемы.</para>
    /// </summary>
    public class ImagePacket : PacketBase
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ImagePacket(long mimicKey, Mimic mimic, int index, int count, int size)
            : base(mimicKey)
        {
            ArgumentNullException.ThrowIfNull(mimic, nameof(mimic));

            Images = [];
            int currentIndex = index;
            int imageCount = mimic.Images.Count;
            int counter = 0;
            int totalSize = 0;

            while (currentIndex < imageCount && counter < count)
            {
                if (currentIndex >= 0)
                {
                    Image image = mimic.Images.GetValueAtIndex(currentIndex);

                    if (Images.Count == 0 || totalSize + image.DataSize <= size)
                    {
                        Images.Add(image);
                        totalSize += image.DataSize;
                    }
                    else
                    {
                        break;
                    }
                }

                currentIndex++;
                counter++;
            }

            EndOfImages = currentIndex >= imageCount;
        }


        /// <summary>
        /// Gets a value indicating whether all images have been read.
        /// </summary>
        public bool EndOfImages { get; }

        /// <summary>
        /// Gets the images.
        /// </summary>
        public List<Image> Images { get; }
    }
}
