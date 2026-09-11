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
        /// <exception cref="InvalidDataException">The image is in an unsupported format or could not be decoded.</exception>
        public static string Encode(int xComponent, int yComponent, Stream stream)
        {
            using (SKCodec codec = CreateCodec(stream))
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
        /// <exception cref="FileNotFoundException">The file does not exist.</exception>
        /// <exception cref="InvalidDataException">The image is in an unsupported format or could not be decoded.</exception>
        public static string Encode(int xComponent, int yComponent, string filename)
        {
            using (SKCodec codec = CreateCodec(filename))
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

            using (SKBitmap bitmap = DecodeBitmap(codec, newInfo))
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
        /// <exception cref="InvalidDataException">The image is in an unsupported format or could not be decoded.</exception>
        public static string Encode(int xComponent, int yComponent, Stream stream, int maxWidth, int maxHeight)
        {
            using (SKCodec codec = CreateCodec(stream))
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
        /// <exception cref="FileNotFoundException">The file does not exist.</exception>
        /// <exception cref="InvalidDataException">The image is in an unsupported format or could not be decoded.</exception>
        public static string Encode(int xComponent, int yComponent, string filename, int maxWidth, int maxHeight)
        {
            using (SKCodec codec = CreateCodec(filename))
            {
                return Encode(xComponent, yComponent, codec, maxWidth, maxHeight);
            }
        }

        internal static string Encode(int xComponent, int yComponent, SKCodec codec, int maxWidth, int maxHeight)
        {
            var width = codec.Info.Width;
            var height = codec.Info.Height;
            float scaleFactor = 0;
            if (width > maxWidth || height > maxHeight)
            {
                scaleFactor = ScaleHelper.GetScale(width, height, maxWidth, maxHeight);
                SKSizeI supportedScale = codec.GetScaledDimensions(scaleFactor);
                width = supportedScale.Width;
                height = supportedScale.Height;
            }

            var newInfo = new SKImageInfo()
            {
                Width = width,
                Height = height,
                ColorType = SKColorType.Rgba8888,
                AlphaType = SKAlphaType.Unpremul,
                ColorSpace = SKColorSpace.CreateSrgb()
            };

            using (SKBitmap bitmap = DecodeBitmap(codec, newInfo))
            {
                if (scaleFactor == 0f)
                {
                    return EncodeInternal(xComponent, yComponent, bitmap);
                }

                var (scaledWidth, scaledHeight) = ScaleHelper.GetScaleDimensions(bitmap.Width, bitmap.Height, scaleFactor);

                newInfo = newInfo.WithSize(scaledWidth, scaledHeight);

                using (SKBitmap scaledBitmap = bitmap.Resize(newInfo, SKSamplingOptions.Default))
                {
                    return EncodeInternal(xComponent, yComponent, scaledBitmap);
                }
            }
        }

        private static SKCodec CreateCodec(Stream stream)
            => SKCodec.Create(stream)
                ?? throw new InvalidDataException("The stream does not contain an image in a format supported by SkiaSharp.");

        private static SKCodec CreateCodec(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("The image file could not be found.", filename);
            }

            return SKCodec.Create(filename)
                ?? throw new InvalidDataException($"The file '{filename}' does not contain an image in a format supported by SkiaSharp.");
        }

        private static SKBitmap DecodeBitmap(SKCodec codec, SKImageInfo info)
            => SKBitmap.Decode(codec, info)
                ?? throw new InvalidDataException("The image could not be decoded; the data is corrupt or truncated.");

        internal static string EncodeInternal(int xComponent, int yComponent, SKBitmap bitmap)
            => CoreBlurHashEncoder.Encode(xComponent, yComponent, bitmap.Width, bitmap.Height, bitmap.GetPixelSpan(), bitmap.RowBytes, PixelFormat.RGB888x);
    }
}
