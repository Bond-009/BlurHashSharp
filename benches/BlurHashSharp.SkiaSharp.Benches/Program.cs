using System;
using BenchmarkDotNet.Running;

namespace BlurHashSharp.SkiaSharp.Benches
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<EncodeBenches>();
            /*var t = new EncodeBenches()
            {
                Components = 4,
                FileName = "samples.ffmpeg.org/image-samples/fujifilm-finepix40i.jpg"
            };

            for (int i = 0; i < 1000; i++)
            {
                t.Encode();
            }*/
        }
    }
}
