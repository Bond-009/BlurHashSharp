using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BlurHashSharp.SkiaSharp.Benches
{
    [MemoryDiagnoser]
    public class EncodeBenches
    {
        private readonly string _samplesPath;
        private string _fileName;

        public EncodeBenches()
        {
            _samplesPath = Environment.GetEnvironmentVariable("FFMPEG_SAMPLES_PATH");
        }

        [Params(1, 4)]
        public int Components { get; set; }

        [Params("samples.ffmpeg.org/image-samples/professional_splash.png", "samples.ffmpeg.org/image-samples/fujifilm-finepix40i.jpg")]
        public string FileName
        {
            get => _fileName;
            set => _fileName = Path.Join(_samplesPath, value);
        }

        [Benchmark]
        public string Encode() => BlurHashEncoder.Encode(Components, Components, _fileName);
    }
}
