using System;

namespace BlurHashSharp
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Calculates the scaled width and height based on a maximum size.
        /// </summary>
        /// <param name="width">The original width.</param>
        /// <param name="height">The original height.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns>The scaled down width and height.</returns>
        public static (int width, int height) Scale(int width, int height, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / width;
            var ratioY = (double)maxHeight / height;
            var ratio = Math.Min(ratioX, ratioY);
            var scaledWidth = Convert.ToInt32(Math.Round(width * ratio));
            var scaledHeight = Convert.ToInt32(Math.Round(height * ratio));

            return (scaledWidth, scaledHeight);
        }
        
        /// <summary>
        /// Calculates the smallest scale factor between actual image dimensions and the maximum dimensions.
        /// </summary>
        /// <param name="width">The original width.</param>
        /// <param name="height">The original height.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns>The smallest scale factor.</returns>
        public static float GetScaleFactor(int width, int height, int maxWidth, int maxHeight)
        {
            var ratioX = (float)maxWidth / width;
            var ratioY = (float)maxHeight / height;

            return Math.Min(ratioX, ratioY);
        }
    }
}