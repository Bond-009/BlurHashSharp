using Xunit;

namespace BlurHashSharp.Tests
{
    public class ScaleHelpersTests
    {
        [Theory]
        [InlineData(3840, 2160, 128, 128, 128, 72)]
        public void Scale_Success(int width, int height, int maxWidth, int maxHeight, int scaledWidth, int scaledHeight)
        {
            var (w, h) = ScaleHelper.GetScaleDimensions(width, height, maxWidth, maxHeight);
            Assert.Equal(w, scaledWidth);
            Assert.Equal(h, scaledHeight);
        }
    }
}
