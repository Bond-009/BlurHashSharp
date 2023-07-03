using System;
using BenchmarkDotNet.Attributes;

namespace BlurHashSharp.Benches
{
    [MemoryDiagnoser]
    public class EncodeBase83Benches
    {
        private int _data;

        [Params(1, 2, 4)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _data = new Random(42).Next();
        }

        [Benchmark]
        public void EncodeBase83()
        {
            Span<char> dest = stackalloc char[4];
            CoreBlurHashEncoder.EncodeBase83(_data, N, dest);
        }
    }
}
