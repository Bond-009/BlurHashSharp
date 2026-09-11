using System;
using System.IO;
using Xunit;

namespace BlurHashSharp.SkiaSharp.Tests
{
    public class BlurHashEncoderInvalidInputTests
    {
        // A 64x96 progressive JPEG cut off half way through its first scan: Skia reads the header
        // and creates a codec, but decoding the pixels fails with InvalidInput.
        private static readonly string TruncatedImagePath = Path.Join(AppContext.BaseDirectory, "TestData", "truncated-progressive.jpg");

        private static byte[] UnrecognizedData()
        {
            var data = new byte[2048];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            return data;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Encode_UnrecognizedStream_ThrowsInvalidDataException(bool resize)
        {
            using var stream = new MemoryStream(UnrecognizedData());

            Assert.Throws<InvalidDataException>(() => resize
                ? BlurHashEncoder.Encode(4, 3, stream, 128, 128)
                : BlurHashEncoder.Encode(4, 3, stream));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Encode_UnrecognizedFile_ThrowsInvalidDataException(bool resize)
        {
            var path = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".jpg");
            File.WriteAllBytes(path, UnrecognizedData());
            try
            {
                Assert.Throws<InvalidDataException>(() => resize
                    ? BlurHashEncoder.Encode(4, 3, path, 128, 128)
                    : BlurHashEncoder.Encode(4, 3, path));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Encode_TruncatedStream_ThrowsInvalidDataException(bool resize)
        {
            using var stream = File.OpenRead(TruncatedImagePath);

            Assert.Throws<InvalidDataException>(() => resize
                ? BlurHashEncoder.Encode(4, 3, stream, 32, 32)
                : BlurHashEncoder.Encode(4, 3, stream));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Encode_TruncatedFile_ThrowsInvalidDataException(bool resize)
        {
            Assert.Throws<InvalidDataException>(() => resize
                ? BlurHashEncoder.Encode(4, 3, TruncatedImagePath, 32, 32)
                : BlurHashEncoder.Encode(4, 3, TruncatedImagePath));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Encode_MissingFile_ThrowsFileNotFoundException(bool resize)
        {
            var path = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".png");

            Assert.Throws<FileNotFoundException>(() => resize
                ? BlurHashEncoder.Encode(4, 3, path, 128, 128)
                : BlurHashEncoder.Encode(4, 3, path));
        }
    }
}
