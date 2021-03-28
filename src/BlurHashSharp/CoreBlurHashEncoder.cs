using System;
using System.Runtime.CompilerServices;

namespace BlurHashSharp
{
    /// <summary>
    /// The core BlurHash encoder.
    /// </summary>
    public static class CoreBlurHashEncoder
    {
        internal static float[] _sRGBToLinearLookup = new float[]
        {
            0f, 0.000303527f, 0.000607054f, 0.000910581f,
            0.001214108f, 0.001517635f, 0.001821162f, 0.0021246888f,
            0.002428216f, 0.002731743f, 0.00303527f, 0.0033465356f,
            0.003676507f, 0.004024717f, 0.004391442f, 0.0047769533f,
            0.005181517f, 0.0056053917f, 0.0060488326f, 0.006512091f,
            0.00699541f, 0.0074990317f, 0.008023192f, 0.008568125f,
            0.009134057f, 0.009721218f, 0.010329823f, 0.010960094f,
            0.011612245f, 0.012286487f, 0.012983031f, 0.013702081f,
            0.014443844f, 0.015208514f, 0.015996292f, 0.016807375f,
            0.017641952f, 0.018500218f, 0.019382361f, 0.020288562f,
            0.02121901f, 0.022173883f, 0.023153365f, 0.02415763f,
            0.025186857f, 0.026241222f, 0.027320892f, 0.028426038f,
            0.029556833f, 0.03071344f, 0.03189603f, 0.033104762f,
            0.034339808f, 0.035601314f, 0.036889445f, 0.038204364f,
            0.039546236f, 0.0409152f, 0.04231141f, 0.043735027f,
            0.045186203f, 0.046665084f, 0.048171822f, 0.049706563f,
            0.051269468f, 0.052860655f, 0.05448028f, 0.056128494f,
            0.057805434f, 0.05951124f, 0.06124607f, 0.06301003f,
            0.06480328f, 0.06662595f, 0.06847818f, 0.07036011f,
            0.07227186f, 0.07421358f, 0.07618539f, 0.07818743f,
            0.08021983f, 0.082282715f, 0.084376216f, 0.086500466f,
            0.088655606f, 0.09084173f, 0.09305898f, 0.095307484f,
            0.09758736f, 0.09989874f, 0.10224175f, 0.10461649f,
            0.10702311f, 0.10946172f, 0.111932434f, 0.11443538f,
            0.11697067f, 0.119538434f, 0.1221388f, 0.12477184f,
            0.1274377f, 0.13013649f, 0.13286833f, 0.13563335f,
            0.13843162f, 0.1412633f, 0.14412849f, 0.14702728f,
            0.1499598f, 0.15292616f, 0.15592647f, 0.15896086f,
            0.1620294f, 0.16513222f, 0.1682694f, 0.1714411f,
            0.17464739f, 0.17788841f, 0.18116423f, 0.18447499f,
            0.18782076f, 0.19120167f, 0.19461781f, 0.1980693f,
            0.20155624f, 0.2050787f, 0.20863685f, 0.21223073f,
            0.21586053f, 0.21952623f, 0.22322798f, 0.22696589f,
            0.23074007f, 0.23455065f, 0.23839766f, 0.2422812f,
            0.2462014f, 0.25015837f, 0.25415218f, 0.2581829f,
            0.26225072f, 0.26635566f, 0.27049786f, 0.27467737f,
            0.27889434f, 0.2831488f, 0.2874409f, 0.2917707f,
            0.29613832f, 0.30054384f, 0.30498737f, 0.30946895f,
            0.31398875f, 0.31854683f, 0.32314324f, 0.32777813f,
            0.33245158f, 0.33716366f, 0.34191445f, 0.3467041f,
            0.3515327f, 0.35640025f, 0.36130688f, 0.3662527f,
            0.37123778f, 0.37626222f, 0.3813261f, 0.38642952f,
            0.39157256f, 0.3967553f, 0.40197787f, 0.4072403f,
            0.4125427f, 0.41788515f, 0.42326775f, 0.42869055f,
            0.4341537f, 0.43965724f, 0.44520125f, 0.45078585f,
            0.45641106f, 0.46207705f, 0.46778384f, 0.47353154f,
            0.47932023f, 0.48514998f, 0.4910209f, 0.49693304f,
            0.5028866f, 0.50888145f, 0.5149178f, 0.5209957f,
            0.5271152f, 0.5332765f, 0.5394796f, 0.5457246f,
            0.5520115f, 0.5583405f, 0.56471163f, 0.5711249f,
            0.5775805f, 0.5840785f, 0.5906189f, 0.5972019f,
            0.6038274f, 0.6104956f, 0.61720663f, 0.62396044f,
            0.6307572f, 0.63759696f, 0.64447975f, 0.6514057f,
            0.65837485f, 0.66538733f, 0.6724432f, 0.67954254f,
            0.68668544f, 0.6938719f, 0.701102f, 0.70837593f,
            0.71569365f, 0.72305524f, 0.7304609f, 0.73791057f,
            0.74540436f, 0.7529423f, 0.76052463f, 0.7681513f,
            0.77582234f, 0.7835379f, 0.79129803f, 0.79910284f,
            0.80695236f, 0.8148467f, 0.82278585f, 0.83076996f,
            0.8387991f, 0.8468733f, 0.8549927f, 0.8631573f,
            0.8713672f, 0.87962234f, 0.8879232f, 0.8962694f,
            0.90466136f, 0.9130987f, 0.92158204f, 0.9301109f,
            0.9386859f, 0.9473066f, 0.9559735f, 0.9646863f,
            0.9734455f, 0.9822506f, 0.9911022f, 1f
        };

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
            float[] factors = new float[factorsLen];

            int bytesPerPixel = pixelFormat switch
            {
                PixelFormat.RGB888 => 3,
                PixelFormat.BGR888 => 3,
                PixelFormat.RGB888x => 4,
                PixelFormat.BGR888x => 4,
                _ => ThrowPixelFormatArgumentException()
            };

            ReadOnlySpan<float> lookUp = _sRGBToLinearLookup;

            int totalPixels = width * height;
            float dcScale = 1f / totalPixels;
            float acScale = 2f / totalPixels;

            float piDivH = MathF.PI / height;
            float piDivW = MathF.PI / width;

            // pi / width * yC
            float yCxPiDivH = 0f;
            for (int yC = 0; yC < yComponents; yC++, yCxPiDivH += piDivH)
            {
                // pi / height * xC
                float xCxPiDivW = 0f;
                for (int xC = 0; xC < xComponents; xC++, xCxPiDivW += piDivW)
                {
                    float c1 = 0;
                    float c2 = 0;
                    float c3 = 0;

                    // pi / width * yC * y
                    float yCoef = 0;
                    for (int y = 0, yOffset = 0; y < height; y++, yOffset += bytesPerRow, yCoef += yCxPiDivH)
                    {
                        float yBasis = MathF.Cos(yCoef);

                        // pi / height * xC * x
                        float xCoef = 0;
                        for (int x = 0, offset = yOffset; x < width; x++, offset += bytesPerPixel, xCoef += xCxPiDivW)
                        {
                            float basis = MathF.Cos(xCoef) * yBasis;
                            c1 += basis * lookUp[pixels[offset]];
                            c2 += basis * lookUp[pixels[offset + 1]];
                            c3 += basis * lookUp[pixels[offset + 2]];
                        }
                    }

                    int factorOffset = ((yC * xComponents) + xC) * 3;
                    float scale = (xC == 0 && yC == 0) ? dcScale : acScale;
                    factors[factorOffset] = c1 * scale;
                    factors[factorOffset + 1] = c2 * scale;
                    factors[factorOffset + 2] = c3 * scale;
                }
            }

            int acCount = totalComponents - 1;
            int hashLen = 1 + 1 + 4 + (acCount * 2);

            return string.Create(
                hashLen,
                (pixelFormat, factors, acCount),
                (hash, state) =>
            {
                ReadOnlySpan<float> dc = factors.AsSpan(0, 3);
                ReadOnlySpan<float> ac = factors.AsSpan(3);
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

        internal static int LinearTosRGB(float value)
        {
            float v = Math.Clamp(value, 0, 1);
            if (v <= 0.0031308f)
            {
                return (int)((12.92f * 255 * v) + 0.5f);
            }
            else
            {
                return (int)((((1.055f * MathF.Pow(v, 1 / 2.4f)) - 0.055f) * 255) + 0.5f);
            }
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
            => MathF.Sign(value) * MathF.Sqrt(MathF.Abs(value));

        internal static int EncodeBase83(int value, int length, Span<char> destination)
        {
            const string Characters = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#$%*+,-.:;=?@[]^_{|}~";
            int divisor = 1;

            int i = 0;
            for (; i < length - 1; i++)
            {
                divisor *= 83;
            }

            for (i = 0; i < length; i++)
            {
                int digit = (value / divisor) % 83;
                divisor /= 83;
                destination[i] = Characters[digit];
            }

            return length;
        }
    }
}
