using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace BlurHashSharp
{
    internal static class AbsMaxExtensions
    {
        public static float AbsMax(this ReadOnlySpan<float> array)
        {
            int len = array.Length;

            if (len >= 8 && Avx.IsSupported)
            {
                return array.AbsMaxAvx();
            }
            else if (len >= 4 && AdvSimd.IsSupported)
            {
                return array.AbsMaxAdvSimd();
            }

            return array.AbsMaxFallback();
        }

        internal static float AbsMaxFallback(this ReadOnlySpan<float> array)
        {
            int len = array.Length;
            float actualMaximumValue = MathF.Abs(array[0]);
            for (int i = 1; i < len; i++)
            {
                var cur = MathF.Abs(array[i]);
                if (cur > actualMaximumValue)
                {
                    actualMaximumValue = cur;
                }
            }

            return actualMaximumValue;
        }

        internal static unsafe float AbsMaxAvx(this ReadOnlySpan<float> array)
        {
            const int StepSize = 8; // Vector256<float>.Count;

            // Constant used to get the absolute value of a Vector<float>
            Vector256<float> neg = Vector256.Create(-0.0f);

            int len = array.Length;
            int rem = len % StepSize;
            int fit = len - rem;
            fixed (float* p = array)
            {
                Vector256<float> maxVec = Avx.AndNot(neg, Avx.LoadVector256(p));

                for (int i = StepSize; i < fit; i += StepSize)
                {
                    maxVec = Avx.Max(maxVec, Avx.AndNot(neg, Avx.LoadVector256(p + i)));
                }

                if (rem != 0)
                {
                    maxVec = Avx.Max(maxVec, Avx.AndNot(neg, Avx.LoadVector256(p + len - StepSize)));
                }

                Vector128<float> maxVec128 = Avx.Max(maxVec.GetLower(), maxVec.GetUpper());
                maxVec128 = Avx.Max(maxVec128, Avx.Permute(maxVec128, 0b00001110));
                maxVec128 = Avx.Max(maxVec128, Avx.Permute(maxVec128, 0b00000001));

                return maxVec128.GetElement(0);
            }
        }

        internal static unsafe float AbsMaxAdvSimd(this ReadOnlySpan<float> array)
        {
            const int StepSize = 4; // Vector128<float>.Count;

            // Constant used to get the absolute value of a Vector<float>
            Vector128<float> notNeg = Vector128.Create(0x7FFFFFFF).AsSingle();
            //AdvSimd.Not(Vector128.Create(-0.0f));

            int len = array.Length;
            int rem = len % StepSize;
            int fit = len - rem;
            fixed (float* p = array)
            {
                Vector128<float> maxVec = AdvSimd.And(notNeg, AdvSimd.LoadVector128(p));

                for (int i = StepSize; i < fit; i += StepSize)
                {
                    maxVec = AdvSimd.Max(maxVec, AdvSimd.And(notNeg, AdvSimd.LoadVector128(p + i)));
                }

                if (rem != 0)
                {
                    maxVec = AdvSimd.Max(maxVec, AdvSimd.And(notNeg, AdvSimd.LoadVector128(p + len - StepSize)));
                }

                Vector64<float> maxVec64 = AdvSimd.Max(maxVec.GetLower(), maxVec.GetUpper());

                return MathF.Max(maxVec64.GetElement(0), maxVec64.GetElement(1));
            }
        }
    }
}
