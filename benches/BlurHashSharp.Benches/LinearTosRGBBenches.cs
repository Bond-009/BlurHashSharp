using BenchmarkDotNet.Attributes;

namespace BlurHashSharp.Benches;

[RPlotExporter]
public class LinearTosRGBBenches
{
    [Params(0f, 0.5f, 1f)]
    public float X { get; set; }

    [Benchmark]
    public int LinearTosRGB() => CoreBlurHashEncoder.LinearTosRGB(X);
}
