using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BlurHashSharp.Benches
{
    [MemoryDiagnoser]
    public class MaxBenches
    {
        private float[] _data;

        [Params(8, 9, 15, 16, 17, 45, 47, 100, 1000)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _data = new float[N];
            var rand = new Random(42);
            for (int i = 0; i < N; i++)
            {
                _data[i] = (float)rand.NextDouble();
            }
        }

        [Benchmark]
        public float MaxAbs() => AbsMaxExtensions.AbsMax(_data);

        [Benchmark]
        public float MaxFallback() => AbsMaxExtensions.AbsMaxFallback(_data);

        [Benchmark]
        public float MaxLinq() => _data.Select(MathF.Abs).Max();
    }
}
