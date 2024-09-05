using System;
using System.IO;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace BlurHashSharp.ImageSharp;

/// <summary>
/// The BlurHash encoder for use with the SkiaSharp image library.
/// </summary>
public static class BlurHashEncoder
{
    private static readonly Lazy<Configuration> Lazy = new(() =>
    {
        var config = Configuration.Default.Clone();
        config.PreferContiguousImageBuffers = true;
        return config;
    });

    private static Configuration Configuration => Lazy.Value;

    private static DecoderOptions DecoderOptions =>
        new DecoderOptions()
        {
            Configuration = Configuration
        };

    /// <summary>
    /// Encodes the BlurHash representation of the image.
    /// </summary>
    /// <param name="xComponent">The number x components.</param>
    /// <param name="yComponent">The number y components.</param>
    /// <param name="filename">The path to an encoded image on the file system.</param>
    /// <returns>BlurHash representation of the image.</returns>
    public static string Encode(int xComponent, int yComponent, string filename)
    {
        using var image = Image.Load<Rgb24>(DecoderOptions, filename);
        return EncodeInternal(xComponent, yComponent, image);
    }

    /// <summary>
    /// Encodes the BlurHash representation of the image.
    /// </summary>
    /// <param name="xComponent">The number x components.</param>
    /// <param name="yComponent">The number y components.</param>
    /// <param name="stream">The IO stream of an encoded image.</param>
    /// <returns>BlurHash representation of the image.</returns>
    public static string Encode(int xComponent, int yComponent, Stream stream)
    {
        using var image = Image.Load<Rgb24>(DecoderOptions, stream);
        return EncodeInternal(xComponent, yComponent, image);
    }

    private static string EncodeInternal(int xComponent, int yComponent, Image<Rgb24> image)
    {
        var bytesPerRow = image.Width * (image.PixelType.BitsPerPixel / 8);
        Span<byte> buffer;
        if (image.DangerousTryGetSinglePixelMemory(out Memory<Rgb24> memory))
        {
            buffer = MemoryMarshal.AsBytes(memory.Span);
        }
        else
        {
            buffer = new byte[image.Height * bytesPerRow];
            image.CopyPixelDataTo(buffer);
        }

        return CoreBlurHashEncoder.Encode(xComponent, yComponent, image.Width, image.Height, buffer, bytesPerRow, PixelFormat.RGB888);
    }
}
