using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

public interface ModelBuilder : ElementsProvider
{
    [PublicAPI]
    public void Add<TElement>(TElement element) where TElement : Element;

    [PublicAPI]
    public void Add<TElement>(TElement element, ISymbol symbol) where TElement : Element;

    [PublicAPI]
    public void Add<TElement>(TElement element, DirectoryInfo directory) where TElement : Element;

    [PublicAPI]
    public void Add(Relation relation);

    [PublicAPI]
    public void Add(Func<ElementsProvider, IEnumerable<Relation>> relationFactory);

    [PublicAPI]
    public void Add(Trait trait);

    [PublicAPI]
    public void Add(Func<ElementsProvider, IEnumerable<Trait>> traitFactory);
}