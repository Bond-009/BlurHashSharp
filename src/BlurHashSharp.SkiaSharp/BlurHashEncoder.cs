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
                var newInfo = codec.Info.WithAlphaType(SKAlphaType.Unpremul).WithColorType(SKColorType.Rgba8888).WithColorSpace(SKColorSpace.CreateSrgb());
                using (SKBitmap bitmap = SKBitmap.Decode(codec, newInfo))
                {
                    return EncodeInternal(xComponent, yComponent, bitmap);
                }
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
                var newInfo = codec.Info.WithAlphaType(SKAlphaType.Unpremul).WithColorType(SKColorType.Rgba8888).WithColorSpace(SKColorSpace.CreateSrgb());
                using (SKBitmap bitmap = SKBitmap.Decode(codec, newInfo))
                {
                    return EncodeInternal(xComponent, yComponent, bitmap);
                }
            }
        }

        internal static string EncodeInternal(int xComponent, int yComponent, SKBitmap bitmap)
            => CoreBlurHashEncoder.Encode(xComponent, yComponent, bitmap.Width, bitmap.Height, bitmap.GetPixelSpan(), bitmap.RowBytes, PixelFormat.RGB888x);
    }
}
