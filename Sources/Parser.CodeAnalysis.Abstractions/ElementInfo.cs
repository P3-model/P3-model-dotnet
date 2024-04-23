using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

public interface ElementInfo
{
    [PublicAPI]
    public ElementId ElementId { get; }

    [PublicAPI]
    public IReadOnlySet<ISymbol> Symbols { get; }

    [PublicAPI]
    public IReadOnlySet<DirectoryInfo> Directories { get; }

    public void Add(ISymbol symbol);

    public void Add(DirectoryInfo directory);
}