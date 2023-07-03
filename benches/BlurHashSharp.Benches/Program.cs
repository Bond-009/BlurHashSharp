﻿using BenchmarkDotNet.Running;

namespace BlurHashSharp.Benches
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // _ = BenchmarkRunner.Run<MaxBenches>();
            // _ = BenchmarkRunner.Run<PrecomputeCosinesBenches>();
            _ = BenchmarkRunner.Run<EncodeBase83Benches>();
        }
    }
}
