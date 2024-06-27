using System;
using Xunit;

namespace BlurHashSharp.Tests;

public class PrecomputeCosinesTests
{
    [Theory]
    [InlineData(0, 32)]
    [InlineData(1, 32)]
    [InlineData(2, 32)]
    [InlineData(3, 32)]
    [InlineData(4, 32)]
    [InlineData(5, 32)]
    [InlineData(6, 32)]
    [InlineData(7, 32)]
    [InlineData(8, 32)]
    [InlineData(0, 36)]
    [InlineData(1, 36)]
    [InlineData(2, 36)]
    [InlineData(3, 36)]
    [InlineData(4, 36)]
    [InlineData(5, 36)]
    [InlineData(6, 36)]
    [InlineData(7, 36)]
    [InlineData(8, 36)]
    [InlineData(9, 36)]
    [InlineData(0, 97)]
    [InlineData(1, 97)]
    [InlineData(2, 97)]
    [InlineData(3, 97)]
    [InlineData(4, 97)]
    [InlineData(5, 97)]
    [InlineData(6, 97)]
    [InlineData(7, 97)]
    [InlineData(8, 97)]
    [InlineData(9, 97)]
    [InlineData(0, 99)]
    [InlineData(1, 99)]
    [InlineData(2, 99)]
    [InlineData(3, 99)]
    [InlineData(4, 99)]
    [InlineData(5, 99)]
    [InlineData(6, 99)]
    [InlineData(7, 99)]
    [InlineData(8, 99)]
    [InlineData(9, 99)]
    [InlineData(0, 128)]
    [InlineData(1, 128)]
    [InlineData(2, 128)]
    [InlineData(3, 128)]
    [InlineData(4, 128)]
    [InlineData(5, 128)]
    [InlineData(6, 128)]
    [InlineData(7, 128)]
    [InlineData(8, 128)]
    [InlineData(9, 128)]

    public void PrecomputeCosines_Success(int c, int length)
    {
        float[] table = new float[length];
        CoreBlurHashEncoder.PrecomputeCosines(table, c);
        Assert.All(table, (x, i) => Assert.Equal(Math.Cos(Math.PI * c * i / length), x, 3));
    }
}
