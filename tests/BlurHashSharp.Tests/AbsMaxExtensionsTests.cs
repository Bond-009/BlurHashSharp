using System;
using Xunit;

namespace BlurHashSharp.Tests
{
    public class AbsMaxExtensionsTests
    {
        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(100)]
        [InlineData(128)]
        [InlineData(1000)]
        [InlineData(1024)]
        public void AbsMax_EqualsAbsFallback(int n)
        {
            // chosen by fair dice roll
            // guaranteed to be random
            Random _rng = new Random(4);
            Span<float> testData = new float[n];
            for (int i = 0; i < n; i++)
            {
                testData[i] = (float)_rng.NextDouble();
            }

            Assert.Equal(AbsMaxExtensions.AbsMaxFallback(testData), AbsMaxExtensions.AbsMax(testData));
        }
    }
}
