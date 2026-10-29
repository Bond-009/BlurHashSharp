using System;
using System.IO;
using SkiaSharp;

namespace BlurHashSharp.SkiaSharp
{
    /// <summary>
    /// The BlurHash encoder for use with the SkiaSharp image library.
    /// </summary>
    public static class BlurHashEncoder
    {
        /// <summary>
        /// Encodes the BlurHash representation of the image.
        /// </summary>
        /// <param name="xComponent">The number x components.</param>
        /// <param name="yComponent">The number y components.</param>
        /// <param name="stream">The IO stream of an encoded image.</param>
        /// <returns>BlurHash representation of the image.</returns>
        public static string Encode(int xComponent, int yComponent, Stream stream)
        {
            using (SKCodec codec = SKCodec.Create(stream))
            {
                return Encode(xComponent, yComponent, codec);
            }
        }

        /// <summary>
        /// Encodes the BlurHash representation of the image.
        /// </summary>
        /// <param name="xComponent">The number x components.</param>
        /// <param name="yComponent">The number y components.</param>
        /// <param name="filename">The path to an encoded image on the file system.</param>
        /// <returns>BlurHash representation of the image.</returns>
        public static string Encode(int xComponent, int yComponent, string filename)
        {
            using (SKCodec codec = SKCodec.Create(filename))
            {
                return Encode(xComponent, yComponent, codec);
            }
        }

        internal static string Encode(int xComponent, int yComponent, SKCodec codec)
        {
            var newInfo = new SKImageInfo()
            {
                Width = codec.Info.Width,
                Height = codec.Info.Height,
                ColorType = SKColorType.Rgba8888,
                AlphaType = SKAlphaType.Unpremul,
                ColorSpace = SKColorSpace.CreateSrgb()
            };

            using (SKBitmap bitmap = SKBitmap.Decode(codec, newInfo))
            {
                return EncodeInternal(xComponent, yComponent, bitmap);
            }
        }

        /// <summary>
        /// Resizes the image and encodes the BlurHash representation of the image.
        /// </summary>
        /// <param name="xComponent">The number x components.</param>
        /// <param name="yComponent">The number y components.</param>
        /// <param name="stream">The IO stream of an encoded image.</param>
        /// <param name="maxWidth">The maximum width to resize the image to.</param>
        /// <param name="maxHeight">The maximum height to resize the image to.</param>
        /// <returns>BlurHash representation of the image.</returns>
        public static string Encode(int xComponent, int yComponent, Stream stream, int maxWidth, int maxHeight)
        {
            using (SKCodec codec = SKCodec.Create(stream))
            {
                return Encode(xComponent, yComponent, codec, maxWidth, maxHeight);
            }
        }

        /// <summary>
        /// Resizes the image and encodes the BlurHash representation of the image.
        /// </summary>
        /// <param name="xComponent">The number x components.</param>
        /// <param name="yComponent">The number y components.</param>
        /// <param name="filename">The path to an encoded image on the file system.</param>
        /// <param name="maxWidth">The maximum width to resize the image to.</param>
        /// <param name="maxHeight">The maximum height to resize the image to.</param>
        /// <returns>BlurHash representation of the image.</returns>
        public static string Encode(int xComponent, int yComponent, string filename, int maxWidth, int maxHeight)
        {
            using (SKCodec codec = SKCodec.Create(filename))
            {
                return Encode(xComponent, yComponent, codec, maxWidth, maxHeight);
            }
        }

        internal static string Encode(int xComponent, int yComponent, SKCodec codec, int maxWidth, int maxHeight)
        {
            using (SKBitmap bitmap = DecodeScaled(codec, maxWidth, maxHeight))
            {
                return EncodeInternal(xComponent, yComponent, bitmap);
            }
        }

        internal static SKBitmap DecodeScaled(SKCodec codec, int maxWidth, int maxHeight)
        {
            var width = codec.Info.Width;
            var height = codec.Info.Height;

            var info = new SKImageInfo()
            {
                Width = width,
                Height = height,
                ColorType = SKColorType.Rgba8888,
                AlphaType = SKAlphaType.Unpremul,
                ColorSpace = SKColorSpace.CreateSrgb()
            };

            if (width <= maxWidth && height <= maxHeight)
            {
                return SKBitmap.Decode(codec, info);
            }

            float scaleFactor = ScaleHelper.GetScale(width, height, maxWidth, maxHeight);

            // The codec may already shrink the image while decoding, so the target size has to come
            // from the original dimensions or the image gets scaled down twice.
            var (targetWidth, targetHeight) = ScaleHelper.GetScaleDimensions(width, height, scaleFactor);
            targetWidth = Math.Max(targetWidth, 1);
            targetHeight = Math.Max(targetHeight, 1);

            SKSizeI supportedScale = codec.GetScaledDimensions(scaleFactor);
            SKBitmap bitmap = SKBitmap.Decode(codec, info.WithSize(supportedScale.Width, supportedScale.Height));
            if (bitmap.Width == targetWidth && bitmap.Height == targetHeight)
            {
                return bitmap;
            }

            using (bitmap)
            {
                return bitmap.Resize(info.WithSize(targetWidth, targetHeight), SKSamplingOptions.Default);
            }
        }

        internal static string EncodeInternal(int xComponent, int yComponent, SKBitmap bitmap)
            => CoreBlurHashEncoder.Encode(xComponent, yComponent, bitmap.Width, bitmap.Height, bitmap.GetPixelSpan(), bitmap.RowBytes, PixelFormat.RGB888x);
    }
}
