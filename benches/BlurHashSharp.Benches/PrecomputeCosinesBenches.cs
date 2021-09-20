using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BlurHashSharp.Benches
{
    [MemoryDiagnoser]
    public class PrecomputeCosinesBenches
    {
        private float[] _data;

        [Params(8, 16, 64, 128, 512, 1000)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _data = new float[N];
        }

        [Benchmark]
        public void PrecomputeCosines() => CoreBlurHashEncoder.PrecomputeCosines(_data, N, MathF.PI / N);

        [Benchmark]
        public void PrecomputeCosinesZero() => CoreBlurHashEncoder.PrecomputeCosines(_data, N, 0f);
    }
}
