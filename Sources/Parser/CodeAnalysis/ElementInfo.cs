using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

public interface ElementInfo
{
    [PublicAPI]
    public ElementBase Element { get; }

    [PublicAPI]
    public IReadOnlySet<ISymbol> Symbols { get; }

    [PublicAPI]
    public IReadOnlySet<DirectoryInfo> Directories { get; }

    public void Add(ISymbol symbol);

    public void Add(DirectoryInfo directory);
}

[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1024:Symbole powinny być porównywane pod kątem równości")]
public class ElementInfo<TElement> : ElementInfo
    where TElement : ElementBase
{
    public TElement Element { get; }

    ElementBase ElementInfo.Element => Element;

    private readonly ConcurrentSet<ISymbol> _symbols = new(CompilationIndependentSymbolEqualityComparer.Default);
    public IReadOnlySet<ISymbol> Symbols => _symbols;

    private readonly ConcurrentSet<DirectoryInfo> _directories = new();
    public IReadOnlySet<DirectoryInfo> Directories => _directories;

    public ElementInfo(TElement element) => Element = element;

    public void Add(ISymbol symbol) => _symbols.TryAdd(symbol);

    public void Add(DirectoryInfo directory) => _directories.TryAdd(directory);
}