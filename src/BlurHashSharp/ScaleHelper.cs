using System;

namespace BlurHashSharp
{
    /// <summary>
    /// Class containing helper methods for scaling images.
    /// </summary>
    public static class ScaleHelper
    {
        /// <summary>
        /// Calculates the smallest scale factor between actual image dimensions and the maximum dimensions.
        /// </summary>
        /// <param name="width">The original width.</param>
        /// <param name="height">The original height.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns>The smallest scale factor.</returns>
        public static float GetScale(int width, int height, int maxWidth, int maxHeight)
        {
            var ratioX = (float)maxWidth / width;
            var ratioY = (float)maxHeight / height;

            return MathF.Min(ratioX, ratioY);
        }

        /// <summary>
        /// Calculates the scaled width and height based on a maximum size.
        /// </summary>
        /// <param name="width">The original width.</param>
        /// <param name="height">The original height.</param>
        /// <param name="scale">The scale.</param>
        /// <returns>The scaled down width and height.</returns>
        public static (int width, int height) GetScaleDimensions(int width, int height, float scale)
            => (Convert.ToInt32(width * scale), Convert.ToInt32(height * scale));

        /// <summary>
        /// Calculates the scaled width and height based on a maximum size.
        /// </summary>
        /// <param name="width">The original width.</param>
        /// <param name="height">The original height.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns>The scaled down width and height.</returns>
        public static (int width, int height) GetScaleDimensions(int width, int height, int maxWidth, int maxHeight)
            => GetScaleDimensions(width, height, GetScale(width, width, maxWidth, maxHeight));
    }
}
