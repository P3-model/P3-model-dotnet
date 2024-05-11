using System.Collections.Immutable;

namespace P3Model.Parser.CodeAnalysis.Configuration.Analyzers;

public record NamespaceOptions(ImmutableArray<string> NamespacePartsToSkip)
{
    public static NamespaceOptions Default => new(ImmutableArray<string>.Empty);
}