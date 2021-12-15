/*
 * Copyright 2021 Rapid Software LLC
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

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
        private static readonly Random RandomGenerator = new();


        public Captcha()
        {
        }


        public string GenerateCode()
        {
            lock (RandomGenerator)
            {
                return RandomGenerator.Next(100000, 999999).ToString();
            }
        }

        public string GenerateImageSrc(string text)
        {
            using MemoryStream stream = new();
            Write(stream, text);
            return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
        }

        public void Write(Stream stream, string text)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            using (Image image = new Image<Rgba32>(500, 100))
            {
                Font font = SystemFonts.Find("Arial").CreateFont(15, FontStyle.Regular);
                image.Mutate(x => x.DrawText(text, font, Color.Black, new PointF(10, 10)));
                image.Save(stream, PngFormat.Instance);
            }

            /*
            // crate a surface
            var info = new SKImageInfo(256, 256);
            using (var surface = SKSurface.Create(info))
            {
                // the the canvas and properties
                var canvas = surface.Canvas;

                // make sure the canvas is blank
                canvas.Clear(SKColors.White);

                // draw some text
                var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    TextAlign = SKTextAlign.Center,
                    TextSize = 24
                };
                var coord = new SKPoint(info.Width / 2, (info.Height + paint.TextSize) / 2);
                canvas.DrawText(text, coord, paint);

                // save the file
                using var image = surface.Snapshot();
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                data.SaveTo(stream);
            }*/

            /*const int CaptchaWidth = 200;
            const int CaptchaHeight = 30;
            const int FontSize = CaptchaHeight;
            const int MaxAngle = 45;
            const float SymbolStep = 0.5f;

            Bitmap bitmap = null;
            Graphics graphics = null;

            Font font = null;
            Brush brush = Brushes.DimGray;

            try
            {
                bitmap = new Bitmap(CaptchaWidth, CaptchaHeight);
                graphics = Graphics.FromImage(bitmap);
                font = new Font("Arial", FontSize, GraphicsUnit.Pixel);

                float symbolX = 0;
                graphics.Clear(Color.White);
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                for (int i = 0, len = text.Length; i < len; i++)
                {
                    string symbol = text[i].ToString();
                    float symbolWidth = graphics.MeasureString(symbol, font).Width;
                    float angle = RandomGenerator.Next(-MaxAngle, MaxAngle);

                    graphics.ResetTransform();
                    graphics.TranslateTransform(symbolX + symbolWidth / 2, CaptchaHeight / 2);
                    graphics.RotateTransform(angle);

                    graphics.DrawString(symbol, font, brush, -symbolWidth / 2, -FontSize / 2);
                    symbolX += symbolWidth * SymbolStep;
                }

                bitmap.Save(stream, ImageFormat.Jpeg);
            }
            finally
            {
                bitmap?.Dispose();
                graphics?.Dispose();
                font?.Dispose();
            }*/
        }
    }
}
