using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1024:Symbole powinny być porównywane pod kątem równości")]
internal class ElementInfo<TElement>(TElement element) : ElementInfo
    where TElement : Element
{
    public TElement Element { get; } = element;

    ElementId ElementInfo.ElementId => Element.Id;

    private readonly ConcurrentSet<ISymbol> _symbols = new(CompilationIndependentSymbolEqualityComparer.Default);
    public IReadOnlySet<ISymbol> Symbols => _symbols;

    private readonly ConcurrentSet<DirectoryInfo> _directories = new();
    public IReadOnlySet<DirectoryInfo> Directories => _directories;

    public void Add(ISymbol symbol) => _symbols.TryAdd(symbol);

    public void Add(DirectoryInfo directory) => _directories.TryAdd(directory);
}