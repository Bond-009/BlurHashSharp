using BenchmarkDotNet.Attributes;

namespace BlurHashSharp.Benches;

public class EncodeACBenches
{
    [Benchmark]
    public int EncodeAC() => CoreBlurHashEncoder.EncodeAC(123f, 124f, 125f, 165f);
}
