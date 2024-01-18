namespace P3Model.Parser.CodeAnalysis.RoslynExtensions;

public static class GetAttributeOptionsExtensions
{
    public static GetAttributeOptions Without(this GetAttributeOptions options, GetAttributeOptions option) =>
        options & ~option;
}