# BlurHashSharp

![.NET][github-actions-badge] [![NuGet][nuget-badge]][nuget-page]

C# BlurHash encoder library with packages for ImageSharp and SkiaSharp.

## Usage

```cs
string blurhash = BlurHashEncoder.Encode(xComponent: 4, yComponent: 4, filename: "/path/to/your/image.jpeg");
```

[github-actions-badge]: https://github.com/Bond-009/BlurHashSharp/workflows/.NET/badge.svg
[nuget-badge]: https://img.shields.io/nuget/v/BlurHashSharp
[nuget-page]: https://www.nuget.org/packages/BlurHashSharp/
