using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using Xunit;

namespace BlurHashSharp.Tests
{
    public class AbsMaxExtensionsTests
    {
        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(100)]
        [InlineData(128)]
        [InlineData(1000)]
        [InlineData(1024)]
        public void AbsMax_EqualsAbsFallback(int n)
        {
            // chosen by fair dice roll
            // guaranteed to be random
            Random _rng = new Random(4);
            Span<float> testData = new float[n];
            for (int i = 0; i < n; i++)
            {
                testData[i] = (float)_rng.NextDouble();
            }

            Assert.Equal(AbsMaxExtensions.AbsMaxFallback(testData), AbsMaxExtensions.AbsMax(testData));
        }

        public static IEnumerable<object[]> TestArrays()
        {
            yield return new object[] { new float[] { -0.9825f, 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.9825f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.9825f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.9825f, -0.28224f, -0.50914f, 0.00998f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.9825f, -0.50914f, 0.00998f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, -0.9825f, 0.00998f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, -0.9825f, -0.90476f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, -0.90476f, -0.9825f, 0.86733f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, 0.86733f, -0.90476f, -0.9825f, 0.08699f }, 0.9825f };
            yield return new object[] { new float[] { 0.19204f, -0.57923f, -0.26693f, -0.28224f, -0.50914f, 0.00998f, 0.86733f, -0.90476f, 0.08699f, -0.9825f }, 0.9825f };
        }

        [Theory]
        [MemberData(nameof(TestArrays))]
        public void AbsMax_Valid_AbsMax(float[] array, float expected)
        {
            Assert.Equal(expected, AbsMaxExtensions.AbsMax(array));
        }

        [Theory]
        [MemberData(nameof(TestArrays))]
        public void AbsMaxFallback_Valid_AbsMax(float[] array, float expected)
        {
            Assert.Equal(expected, AbsMaxExtensions.AbsMaxFallback(array));
        }

        [SkippableTheory]
        [MemberData(nameof(TestArrays))]
        public void AbsMaxAvx_Valid_AbsMax(float[] array, float expected)
        {
            Skip.IfNot(Avx.IsSupported);

            Assert.Equal(expected, AbsMaxExtensions.AbsMaxAvx(array));
        }

        [SkippableTheory]
        [MemberData(nameof(TestArrays))]
        public void AbsMaxSse_Valid_AbsMax(float[] array, float expected)
        {
            Skip.IfNot(Sse.IsSupported);

            Assert.Equal(expected, AbsMaxExtensions.AbsMaxSse(array));
        }

        [SkippableTheory]
        [MemberData(nameof(TestArrays))]
        public void AbsMaxAdvSimd_Valid_AbsMax(float[] array, float expected)
        {
            Skip.IfNot(AdvSimd.IsSupported);

            Assert.Equal(expected, AbsMaxExtensions.AbsMaxAdvSimd(array));
        }
    }
}
