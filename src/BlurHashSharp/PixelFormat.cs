namespace BlurHashSharp
{
    /// <summary>
    /// Describes how to interpret the components of a pixel.
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// Represents a 24-bit color with the format RGB, with 8 bits per color component.
        /// </summary>
        RGB888,

        /// <summary>
        /// Represents a 24-bit color with the format BGR, with 8 bits per color component.
        /// </summary>
        BGR888,

        /// <summary>
        /// Represents a 32-bit color with the format RGB, with 8 bits per color component.
        /// </summary>
        RGB888x,

        /// <summary>
        /// Represents a 32-bit color with the format BGR, with 8 bits per color component.
        /// </summary>
        BGR888x,
    }
}
