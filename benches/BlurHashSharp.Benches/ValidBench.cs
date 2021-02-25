using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BlurHashSharp.Benches
{
    [MemoryDiagnoser]
    public class ValidBench
    {
        [ParamsSource(nameof(Values))]
        public string Data;

        public IEnumerable<string> Values => new[] { "ValidFileName", "AC/DC\0KD/A" };

        [Benchmark]
        public string GetValidFilenameBench() => GetValidFilename(Data);

        [Benchmark]
        public string GetValidFilenameOldBench() => GetValidFilenameOld(Data);

        [Benchmark]
        public string GetValidFilenameWinBench() => GetValidFilenameWin(Data);

        [Benchmark]
        public string GetValidFilenameOldWinBench() => GetValidFilenameOldWin(Data);

        public string GetValidFilename(string filename)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var first = filename.IndexOfAny(invalid);
            if (first == -1)
            {
                // Fast path for clean strings
                return filename;
            }

            return string.Create(
                filename.Length,
                (filename, invalid, first),
                (chars, state) =>
                {
                    state.filename.AsSpan().CopyTo(chars);

                    chars[state.first++] = ' ';

                    var len = chars.Length;
                    foreach (var c in state.invalid)
                    {
                        for (int i = state.first; i < len; i++)
                        {
                            if (chars[i] == c)
                            {
                                chars[i] = ' ';
                            }
                        }
                    }
                });
        }

        public string GetValidFilenameOld(string filename)
        {
            var builder = new StringBuilder(filename);
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var c in invalid)
            {
                builder = builder.Replace(c, ' ');
            }

            return builder.ToString();
        }

        public string GetValidFilenameWin(string filename)
        {
            var invalid = new char[]
        {
            '\"', '<', '>', '|', '\0',
            (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
            (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
            (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
            (char)31, ':', '*', '?', '\\', '/'
        };
            var first = filename.IndexOfAny(invalid);
            if (first == -1)
            {
                // Fast path for clean strings
                return filename;
            }

            return string.Create(
                filename.Length,
                (filename, invalid, first),
                (chars, state) =>
                {
                    state.filename.AsSpan().CopyTo(chars);

                    chars[state.first++] = ' ';

                    var len = chars.Length;
                    foreach (var c in state.invalid)
                    {
                        for (int i = state.first; i < len; i++)
                        {
                            if (chars[i] == c)
                            {
                                chars[i] = ' ';
                            }
                        }
                    }
                });
        }

        public string GetValidFilenameOldWin(string filename)
        {
            var builder = new StringBuilder(filename);
            var invalid = new char[]
        {
            '\"', '<', '>', '|', '\0',
            (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
            (char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
            (char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
            (char)31, ':', '*', '?', '\\', '/'
        };
            foreach (var c in invalid)
            {
                builder = builder.Replace(c, ' ');
            }

            return builder.ToString();
        }
    }
}
