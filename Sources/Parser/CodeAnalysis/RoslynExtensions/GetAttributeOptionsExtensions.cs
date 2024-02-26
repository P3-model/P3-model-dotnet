namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

public static class GetAttributeOptionsExtensions
{
    public static TypeAttributeSources Without(this TypeAttributeSources sources, TypeAttributeSources source) =>
        sources & ~source;
}