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
    }
}
