namespace P3Model.Parser;

// ReSharper disable once NotAccessedPositionalProperty.Global
public readonly record struct TargetFramework(string Value)
{
    public static TargetFramework Unspecified => new("unspecified");
    public static TargetFramework Net60 => new("net6.0");
    public static TargetFramework Net70 => new("net7.0");
    public static TargetFramework Net80 => new("net8.0");
    public static IEnumerable<TargetFramework> All => [Net60, Net70, Net80];
}