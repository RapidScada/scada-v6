/*
 * Copyright 2024 Rapid Software LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : Webstation Application
 * Summary  : Generates a captcha
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2021
 * Modified : 2021
 */

using SkiaSharp;
using System;
using System.IO;

namespace Scada.Web.Code
{
    /// <summary>
    /// Generates a captcha.
    /// <para>Генерирует защитное изображение.</para>
    /// </summary>
    public class Captcha
    {
        /// <summary>
        /// Represents captcha options.
        /// </summary>
        public class Options
        {
            public Options()
            {
                MinCodeValue = 100000;
                MaxCodeValue = MinCodeValue * 10;
                MaxRotationAngle = 45;
                SpacingFactor = 0.75f;
                ImageWidth = 500;
                ImageHeight = 100;
                Color = SKColors.DimGray;
                TextSize = ImageHeight;
                FontFamily = "Arial";
            }
            public int MinCodeValue { get; init; }
            public int MaxCodeValue { get; init; }
            public int MaxRotationAngle { get; init; }
            public float SpacingFactor { get; init; }
            public int ImageWidth { get; init; }
            public int ImageHeight { get; init; }
            public SKColor Color { get; init; }
            public int TextSize { get; init; }
            public string FontFamily { get; init; }
        }


        private static readonly Random RandomGenerator = new();
        private readonly Options options;


        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Captcha()
            : this(new Options())
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Captcha(Options options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }


        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        private static int NextRandom(int minValue, int maxValue)
        {
            lock (RandomGenerator)
            {
                return RandomGenerator.Next(minValue, maxValue);
            }
        }

        /// <summary>
        /// Writes the captcha image to the specified stream.
        /// </summary>
        private void WriteImage(Stream stream, string text)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            // create surface
            SKImageInfo imageInfo = new(options.ImageWidth, options.ImageHeight);
            using SKSurface surface = SKSurface.Create(imageInfo);

            // create paint
            using SKPaint paint = new()
            {
                Color = options.Color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                TextAlign = SKTextAlign.Left,
                TextSize = options.TextSize,
            };

            if (!string.IsNullOrEmpty(options.FontFamily))
                paint.Typeface = SKTypeface.FromFamilyName(options.FontFamily);

            // draw image
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            SKRect textBounds = new();
            paint.MeasureText(text, ref textBounds);
            float symbolX = 0;
            float symbolY = imageInfo.Rect.MidY - textBounds.MidY;

            for (int i = 0, len = text.Length; i < len; i++)
            {
                string symbol = text[i].ToString();
                float symbolWidth = paint.MeasureText(symbol);
                float angle = NextRandom(-options.MaxRotationAngle, options.MaxRotationAngle);

                canvas.ResetMatrix();
                canvas.RotateDegrees(angle, symbolX + symbolWidth / 2, options.ImageHeight / 2);
                canvas.DrawText(symbol, symbolX, symbolY, paint);
                symbolX += symbolWidth * options.SpacingFactor;
            }

            // save image
            using SKImage image = surface.Snapshot();
            using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(stream);
        }

        /// <summary>
        /// Generates a numeric captcha code.
        /// </summary>
        public string GenerateCode()
        {
            return NextRandom(options.MinCodeValue, options.MaxCodeValue).ToString();
        }

        /// <summary>
        /// Generates an image source to insert into HTML markup.
        /// </summary>
        public string GenerateImageSrc(string text)
        {
            using MemoryStream stream = new();
            WriteImage(stream, text);
            return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
        }
    }
}
