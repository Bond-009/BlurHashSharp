using System;
using BenchmarkDotNet.Attributes;

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
        public void PrecomputeCosines() => CoreBlurHashEncoder.PrecomputeCosines(_data, 4);
    }
}
