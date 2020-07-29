using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BlurHashSharp.Drawing
{
    /// <summary>
    /// The BlurHash encoder for use with the System.Drawing.Common library.
    /// </summary>
    public static class BlurHashEncoder
    {
        /// <summary>
        /// Encodes the BlurHash representation of the image.
        /// </summary>
        /// <param name="xComponent">The number x components.</param>
        /// <param name="yComponent">The number y components.</param>
        /// <param name="filename">The path to an encoded image on the file system.</param>
        /// <returns>BlurHash representation of the image.</returns>
        public static string Encode(int xComponent, int yComponent, string filename)
        {
            using (var bitmap = new Bitmap(filename))
            {
                if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                {
                    return EncodeInternal(xComponent, yComponent, bitmap, PixelFormat.BGR888);
                }
                else if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                {
                    return EncodeInternal(xComponent, yComponent, bitmap, PixelFormat.BGR888x);
                }

                using (var temporaryBitmap = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                {
                    using (var graphics = Graphics.FromImage(temporaryBitmap))
                    {
                        graphics.DrawImageUnscaled(bitmap, 0, 0);
                    }

                    return EncodeInternal(xComponent, yComponent, temporaryBitmap, PixelFormat.BGR888);
                }
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
            using (var bitmap = new Bitmap(filename))
            {
                var ratioX = (double)maxWidth / bitmap.Width;
                var ratioY = (double)maxHeight / bitmap.Height;
                var ratio = Math.Min(ratioX, ratioY);
                var scaledWidth = Convert.ToInt32(Math.Round(bitmap.Width * ratio));
                var scaledHeight = Convert.ToInt32(Math.Round(bitmap.Height * ratio));

                using (var scaledBitmap = new Bitmap(bitmap, new Size(scaledWidth, scaledHeight)))
                {
                    if (scaledBitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                    {
                        return EncodeInternal(xComponent, yComponent, scaledBitmap, PixelFormat.BGR888);
                    }
                    else if (scaledBitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                    {
                        return EncodeInternal(xComponent, yComponent, scaledBitmap, PixelFormat.BGR888x);
                    }

                    using (var temporaryBitmap = new Bitmap(scaledBitmap.Width, scaledBitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                    {
                        using (var graphics = Graphics.FromImage(temporaryBitmap))
                        {
                            graphics.DrawImageUnscaled(scaledBitmap, 0, 0);
                        }

                        return EncodeInternal(xComponent, yComponent, temporaryBitmap, PixelFormat.BGR888);
                    }
                }
            }
        }
        
        internal static unsafe string EncodeInternal(int xComponent, int yComponent, Bitmap bitmap, PixelFormat pixelFormat)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;

            // Lock the bitmap's bits.
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // Get the address of the first line.
            var ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            var bytes = Math.Abs(bmpData.Stride) * height;

            var hash = CoreBlurHashEncoder.Encode(xComponent, yComponent, width, height, new ReadOnlySpan<byte>(ptr.ToPointer(), bytes), bmpData.Stride, pixelFormat);

            bitmap.UnlockBits(bmpData);
            return hash;
        }
    }
}
