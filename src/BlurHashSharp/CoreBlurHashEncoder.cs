using System;
using System.Buffers;
using System.Diagnostics;

namespace BlurHashSharp
{
    /// <summary>
    /// The core BlurHash encoder.
    /// </summary>
    public static class CoreBlurHashEncoder
    {
        internal static readonly float[] _sRGBToLinearLookup;

        static CoreBlurHashEncoder()
        {
            _sRGBToLinearLookup = new float[byte.MaxValue + 1];
            for (int i = byte.MinValue; i <= byte.MaxValue; i++)
            {
                _sRGBToLinearLookup[i] = sRGBToLinear(i);
            }
        }

        /// <summary>
        /// Encodes the given <code>pixels</code> into a BlurHash.
        /// </summary>
        /// <param name="xComponents">The number x components.</param>
        /// <param name="yComponents">The number y components.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="pixels">The pixels to encode.</param>
        /// <param name="bytesPerRow">The number of bytes in 1 row of the image (stride).</param>
        /// <param name="pixelFormat">The format in witch the <code>pixels</code> are stored.</param>
        /// <returns>BlurHash representation of the image.</returns>
        public static string Encode(
            int xComponents,
            int yComponents,
            int width,
            int height,
            ReadOnlySpan<byte> pixels,
            int bytesPerRow,
            PixelFormat pixelFormat)
        {
            static int ThrowPixelFormatArgumentException()
                => throw new ArgumentException("Invalid pixel format.", nameof(pixelFormat));

            int totalComponents = xComponents * yComponents;
            int factorsLen = totalComponents * 3;

            int bytesPerPixel = pixelFormat switch
            {
                PixelFormat.RGB888 => 3,
                PixelFormat.BGR888 => 3,
                PixelFormat.RGB888x => 4,
                PixelFormat.BGR888x => 4,
                _ => ThrowPixelFormatArgumentException()
            };

            float[] rented = ArrayPool<float>.Shared.Rent(factorsLen + height + width);
            try
            {
                Memory<float> factors = rented.AsMemory(0, factorsLen);
                Span<float> factorsSpan = factors.Span;
                Span<float> cosYLookup = rented.AsSpan(factorsLen, height);
                Span<float> cosXLookup = rented.AsSpan(factorsLen + height, width);
                ReadOnlySpan<float> sRGBToLinearLookup = _sRGBToLinearLookup;

                int totalPixels = width * height;
                float dcScale = 1f / totalPixels;
                float acScale = 2f / totalPixels;

                for (int yC = 0; yC < yComponents; yC++)
                {
                    PrecomputeCosines(cosYLookup, MathF.PI * yC / height);

                    for (int xC = 0; xC < xComponents; xC++)
                    {
                        PrecomputeCosines(cosXLookup, MathF.PI * xC / width);

                        float c1 = 0;
                        float c2 = 0;
                        float c3 = 0;

                        for (int y = 0; y < height; y++)
                        {
                            float yBasis = cosYLookup[y];
                            int offset = y * bytesPerRow;
                            for (int x = 0; x < width; x++)
                            {
                                float basis = cosXLookup[x] * yBasis;
                                c1 += basis * sRGBToLinearLookup[pixels[offset]];
                                c2 += basis * sRGBToLinearLookup[pixels[offset + 1]];
                                c3 += basis * sRGBToLinearLookup[pixels[offset + 2]];

                                offset += bytesPerPixel;
                            }
                        }

                        int factorOffset = ((yC * xComponents) + xC) * 3;
                        float scale = (xC == 0 && yC == 0) ? dcScale : acScale;
                        factorsSpan[factorOffset] = c1 * scale;
                        factorsSpan[factorOffset + 1] = c2 * scale;
                        factorsSpan[factorOffset + 2] = c3 * scale;
                    }
                }

                int acCount = totalComponents - 1;
                int hashLen = 1 + 1 + 4 + (acCount * 2);

                return string.Create(
                    hashLen,
                    (pixelFormat, acCount),
                    (hash, state) =>
                {
                    ReadOnlySpan<float> dc = factors.Slice(0, 3).Span;
                    ReadOnlySpan<float> ac = factors.Slice(3).Span;
                    int acLen = ac.Length;

                    int hashPos = EncodeBase83(xComponents - 1 + ((yComponents - 1) * 9), 1, hash);
                    float maximumValue;
                    if (state.acCount > 0)
                    {
                        float actualMaximumValue = ac.AbsMax();

                        int quantisedMaximumValue = Math.Clamp((int)((actualMaximumValue * 166f) - 0.5f), 0, 82);
                        maximumValue = (quantisedMaximumValue + 1) / 166f;
                        hashPos += EncodeBase83(quantisedMaximumValue, 1, hash.Slice(hashPos));
                    }
                    else
                    {
                        maximumValue = 1;
                        hashPos += EncodeBase83(0, 1, hash.Slice(hashPos));
                    }

                    switch (state.pixelFormat)
                    {
                        case PixelFormat.BGR888:
                        case PixelFormat.BGR888x:
                            hashPos += EncodeBase83(EncodeDC(dc[2], dc[1], dc[0]), 4, hash.Slice(hashPos));

                            for (int i = 0; i < acLen; i += 3)
                            {
                                hashPos += EncodeBase83(EncodeAC(ac[i + 2], ac[i + 1], ac[i], maximumValue), 2, hash.Slice(hashPos));
                            }

                            break;

                        case PixelFormat.RGB888:
                        case PixelFormat.RGB888x:
                            hashPos += EncodeBase83(EncodeDC(dc[0], dc[1], dc[2]), 4, hash.Slice(hashPos));

                            for (int i = 0; i < acLen; i += 3)
                            {
                                hashPos += EncodeBase83(EncodeAC(ac[i], ac[i + 1], ac[i + 2], maximumValue), 2, hash.Slice(hashPos));
                            }

                            break;
                    }
                });
            }
            finally
            {
                ArrayPool<float>.Shared.Return(rented);
            }
        }

        internal static void PrecomputeCosines(Span<float> table, float offset)
        {
            if (offset == 0f)
            {
                table.Fill(1f);
                return;
            }

            float coef = 0f;
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = MathF.Cos(coef);
                coef += offset;
            }
        }

        private static float sRGBToLinear(int value)
        {
            if (value < 11)
            {
                return (float)(value / 3294.6);
            }

            return (float)Math.Pow((value + 14.025) / 269.025, 2.4);
        }

        internal static int LinearTosRGB(float value)
        {
            float v = Math.Clamp(value, 0, 1);
            if (v <= 0.0031308f)
            {
                return (int)((3294.6f * v) + 0.5f);
            }

            return (int)((269.025f * MathF.Pow(v, 1 / 2.4f)) - 13.525f);
        }

        internal static int EncodeDC(float r, float g, float b)
        {
            int roundedR = LinearTosRGB(r);
            int roundedG = LinearTosRGB(g);
            int roundedB = LinearTosRGB(b);
            return (roundedR << 16) + (roundedG << 8) + roundedB;
        }

        internal static int EncodeAC(float r, float g, float b, float maximumValue)
        {
            int quantR = Math.Clamp((int)((9 * SignSqrtF(r / maximumValue)) + 9.5f), 0, 18);
            int quantG = Math.Clamp((int)((9 * SignSqrtF(g / maximumValue)) + 9.5f), 0, 18);
            int quantB = Math.Clamp((int)((9 * SignSqrtF(b / maximumValue)) + 9.5f), 0, 18);

            return (19 * 19 * quantR) + (19 * quantG) + quantB;
        }

        internal static float SignSqrtF(float value)
            => MathF.CopySign(MathF.Sqrt(MathF.Abs(value)), value);

        internal static int EncodeBase83(int value, int length, Span<char> destination)
        {
            const string Characters = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#$%*+,-.:;=?@[]^_{|}~";

            Debug.Assert(length <= destination.Length);

            destination[length - 1] = Characters[value % Characters.Length];

            int tmpValue = value;
            for (int i = length - 2; i >= 0; i--)
            {
                tmpValue /= Characters.Length;
                destination[i] = Characters[tmpValue % Characters.Length];
            }

            return length;
        }
    }
}
