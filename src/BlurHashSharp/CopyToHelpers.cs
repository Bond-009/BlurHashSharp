using System;
using System.Diagnostics;

namespace BlurHashSharp;

internal static class CopyToHelpers
{
    public static void ReverseCopyTo(ReadOnlySpan<float> src, Span<float> dst)
    {
        Debug.Assert(src.Length == dst.Length, "src and dst length don't match");

        var l = src.Length;
        for (int i = 0; i < l; i++)
        {
            dst[l - i - 1] = src[i];
        }
    }

    public static unsafe void ReverseInverseSignCopyTo(ReadOnlySpan<float> src, Span<float> dst)
    {
        Debug.Assert(src.Length == dst.Length, "src and dst length don't match");

        var l = src.Length;
        for (int i = 0; i < l; i++)
        {
            dst[l - i - 1] = -src[i];
        }
    }

    public static void InverseSignCopyTo(ReadOnlySpan<float> src, Span<float> dst)
    {
        Debug.Assert(src.Length <= dst.Length, "src is larger than dst");

        for (int i = 0; i < src.Length; i++)
        {
            dst[i] = -src[i];
        }
    }
}
