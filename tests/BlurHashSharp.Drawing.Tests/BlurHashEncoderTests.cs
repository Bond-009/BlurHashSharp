using System;
using System.IO;
using Xunit;

namespace BlurHashSharp.Drawing.Tests
{
    public class BlurHashEncoderTests
    {
        [Theory]
        [ClassData(typeof(TestData))]
        public void Encode_Success(string relPath, int xComponent, int yComponent, string expectedResult)
        {
            string absPath = Path.Join(Environment.GetEnvironmentVariable("FFMPEG_SAMPLES_PATH"), relPath);
            Assert.Equal(expectedResult, BlurHashEncoder.Encode(xComponent, yComponent, absPath));
        }
    }
}
