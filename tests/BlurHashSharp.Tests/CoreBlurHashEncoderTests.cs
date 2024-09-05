using System;
using Xunit;

namespace BlurHashSharp.Tests;

public sealed class CoreBlurHashEncoderTests
{
    public static TheoryData<float> LookupTable
        => new TheoryData<float>(CoreBlurHashEncoder._sRGBToLinearLookup);

    [Theory]
    [MemberData(nameof(LookupTable))]
    public void LinearTosRGB_RoundTrip_Success(float value)
    {
        int i = CoreBlurHashEncoder.LinearTosRGB(value);
        var roundtrip = CoreBlurHashEncoder._sRGBToLinearLookup[i];
        Assert.Equal((double)value, roundtrip, 9);
    }

    [Theory]
    [InlineData(float.Epsilon, float.Epsilon)]
    [InlineData(0f, 0f)]
    [InlineData(1f, 1f)]
    [InlineData(-1f, -1f)]
    [InlineData(3f, 1.732050808f)]
    [InlineData(-3f, -1.732050808f)]
    public void SignSqrtF_Success(float value, float excpected)
    {
        Assert.Equal((double)excpected, CoreBlurHashEncoder.SignSqrtF(value), 9);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 10)]
    [InlineData(10, 1)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    public void Ctor_InvalidNumberOfComponents_ThrowArgumentOutOfRangeException(int xComponents, int yComponents)
        => Assert.Throws<ArgumentOutOfRangeException>(() => CoreBlurHashEncoder.Encode(xComponents, yComponents, 128, 128, Array.Empty<byte>(), 3 * 128, PixelFormat.RGB888x));
}
