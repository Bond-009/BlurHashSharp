using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace BlurHashSharp.Tests
{
    public class CoreBlurHashEncoderTests
    {
        public class LinearTosRGBRoundTripTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (float v in CoreBlurHashEncoder._sRGBToLinearLookup)
                {
                    yield return new object[] { v };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(LinearTosRGBRoundTripTestData))]
        public void LinearTosRGB_RoundTrip_Success(float value)
        {
            int i = CoreBlurHashEncoder.LinearTosRGB(value);
            var roundtrip = CoreBlurHashEncoder._sRGBToLinearLookup[i];
            Assert.Equal(value, roundtrip, 7);
        }

        [Theory]
        [InlineData(float.Epsilon, float.Epsilon)]
        [InlineData(0f, 0f)]
        [InlineData(-0f, -0f)]
        [InlineData(1f, 1f)]
        [InlineData(-1f, -1f)]
        [InlineData(3f, 1.732050808f)]
        [InlineData(-3f, -1.732050808f)]
        public void SignSqrtF_Success(float value, float excpected)
        {
            Assert.Equal(excpected, CoreBlurHashEncoder.SignSqrtF(value), 7);
        }
    }
}
