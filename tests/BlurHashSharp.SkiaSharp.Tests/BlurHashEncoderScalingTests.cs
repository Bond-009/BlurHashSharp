using System;
using System.IO;
using SkiaSharp;
using Xunit;

namespace BlurHashSharp.SkiaSharp.Tests
{
    public class BlurHashEncoderScalingTests : IDisposable
    {
        private readonly string _directory;

        public BlurHashEncoderScalingTests()
        {
            _directory = Path.Combine(Path.GetTempPath(), "BlurHashSharp." + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_directory);
        }

        public void Dispose()
        {
            Directory.Delete(_directory, true);
            GC.SuppressFinalize(this);
        }

        [Theory]
        [InlineData(4319, 481)]
        [InlineData(481, 4319)]
        [InlineData(10601, 1848)]
        [InlineData(9148, 2250)]
        public void Encode_ExtremeAspectRatioWebp_ReturnsHash(int width, int height)
        {
            var bytes = CreateImage(width, height, SKEncodedImageFormat.Webp);
            var path = WriteImage(bytes, width, height, "webp");

            var fromPath = BlurHashEncoder.Encode(4, 3, path, 128, 128);
            string fromStream;
            using (var stream = new MemoryStream(bytes))
            {
                fromStream = BlurHashEncoder.Encode(4, 3, stream, 128, 128);
            }

            Assert.NotEmpty(fromPath);
            Assert.Equal(fromPath, fromStream);
        }

        [Theory]
        [InlineData(SKEncodedImageFormat.Webp)]
        [InlineData(SKEncodedImageFormat.Png)]
        [InlineData(SKEncodedImageFormat.Jpeg)]
        public void DecodeScaled_ShrinksExactlyOnce(SKEncodedImageFormat format)
        {
            var path = WriteImage(CreateImage(4319, 481, format), 4319, 481, format.ToString().ToLowerInvariant());

            using (SKCodec codec = SKCodec.Create(path))
            using (SKBitmap bitmap = BlurHashEncoder.DecodeScaled(codec, 128, 128))
            {
                Assert.Equal(128, bitmap.Width);
                Assert.Equal(14, bitmap.Height);
            }
        }

        [Theory]
        [InlineData(SKEncodedImageFormat.Webp, 10000, 1, 128, 1)]
        [InlineData(SKEncodedImageFormat.Webp, 1, 10000, 1, 128)]
        [InlineData(SKEncodedImageFormat.Png, 10000, 1, 128, 1)]
        [InlineData(SKEncodedImageFormat.Png, 1, 10000, 1, 128)]
        [InlineData(SKEncodedImageFormat.Jpeg, 10000, 1, 128, 1)]
        [InlineData(SKEncodedImageFormat.Jpeg, 1, 10000, 1, 128)]
        public void DecodeScaled_ClampsZeroSideToOne(SKEncodedImageFormat format, int width, int height, int expectedWidth, int expectedHeight)
        {
            var path = WriteImage(CreateImage(width, height, format), width, height, format.ToString().ToLowerInvariant());

            using (SKCodec codec = SKCodec.Create(path))
            using (SKBitmap bitmap = BlurHashEncoder.DecodeScaled(codec, 128, 128))
            {
                Assert.Equal(expectedWidth, bitmap.Width);
                Assert.Equal(expectedHeight, bitmap.Height);
            }
        }

        [Fact]
        public void Encode_WithinLimit_MatchesUnscaledOverload()
        {
            var path = WriteImage(CreateImage(100, 50, SKEncodedImageFormat.Png), 100, 50, "png");

            Assert.Equal(BlurHashEncoder.Encode(4, 3, path), BlurHashEncoder.Encode(4, 3, path, 128, 128));
        }

        private string WriteImage(byte[] bytes, int width, int height, string extension)
        {
            var path = Path.Combine(_directory, $"{width}x{height}.{extension}");
            File.WriteAllBytes(path, bytes);
            return path;
        }

        private static byte[] CreateImage(int width, int height, SKEncodedImageFormat format)
        {
            using (var bitmap = new SKBitmap(width, height))
            using (var canvas = new SKCanvas(bitmap))
            using (var gradient = new SKPaint())
            using (var circle = new SKPaint())
            {
                gradient.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(width, height),
                    new[] { SKColors.DarkBlue, SKColors.Orange },
                    SKShaderTileMode.Clamp);
                canvas.DrawRect(new SKRect(0, 0, width, height), gradient);

                circle.Color = SKColors.White;
                circle.IsAntialias = true;
                canvas.DrawCircle(width / 2f, height / 2f, Math.Min(width, height) / 3f, circle);

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(format, 90))
                {
                    return data.ToArray();
                }
            }
        }
    }
}
