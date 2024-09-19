using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis;

[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1024",
    Justification = "See CompilationIndependentSymbolEqualityComparer")]
internal class DefaultModelBuilder(DocumentedSystem system) : ModelBuilder
{
    private readonly ConcurrentDictionary<Element, ElementInfo> _elements = new();
    private readonly ConcurrentDictionary<ISymbol, ConcurrentSet<Element>> _symbolToElements =
        new(CompilationIndependentSymbolEqualityComparer.Default);

    private readonly ConcurrentSet<Relation> _relations = new();
    private readonly ConcurrentSet<Func<ElementsProvider, IEnumerable<Relation>>> _relationFactories =
        new();

    private readonly ConcurrentSet<Trait> _traits = new();
    private readonly ConcurrentSet<Func<ElementsProvider, IEnumerable<Trait>>> _traitFactories = new();

    [PublicAPI]
    public void Add<TElement>(TElement element) where TElement : Element =>
        _elements.TryAdd(element, new ElementInfo<TElement>(element));

    [PublicAPI]
    public void Add<TElement>(TElement element, ISymbol symbol)
        where TElement : Element
    {
        _elements.AddOrUpdate(element,
            _ =>
            {
                var info = new ElementInfo<TElement>(element);
                info.Add(symbol);
                return info;
            },
            (_, info) =>
            {
                info.Add(symbol);
                return info;
            });
        _symbolToElements.AddOrUpdate(symbol,
            _ =>
            {
                var set = new ConcurrentSet<Element>();
                set.TryAdd(element);
                return set;
            },
            (_, elements) =>
            {
                elements.TryAdd(element);
                return elements;
            });
    }

    [PublicAPI]
    public void Add<TElement>(TElement element, DirectoryInfo directory)
        where TElement : Element
    {
        _elements.AddOrUpdate(element,
            _ =>
            {
                var info = new ElementInfo<TElement>(element);
                info.Add(directory);
                return info;
            },
            (_, info) =>
            {
                info.Add(directory);
                return info;
            });
    }

    [PublicAPI]
    public void Add(Relation relation) => _relations.TryAdd(relation);

    [PublicAPI]
    public void Add(Func<ElementsProvider, IEnumerable<Relation>> relationFactory) =>
        _relationFactories.TryAdd(relationFactory);

    [PublicAPI]
    public void Add(Trait trait) => _traits.TryAdd(trait);

    [PublicAPI]
    public void Add(Func<ElementsProvider, IEnumerable<Trait>> traitFactory) =>
        _traitFactories.TryAdd(traitFactory);

    IEnumerable<Element> ElementsProvider.For(ISymbol symbol) => _symbolToElements.TryGetValue(symbol, out var elements)
        ? elements
        : Enumerable.Empty<Element>();

    public IEnumerable<Element> Where(Func<ISymbol, bool> predicate) => _symbolToElements
        .Where(pair => predicate(pair.Key))
        .SelectMany(pair => pair.Value);

    IEnumerable<TElement> ElementsProvider.OfType<TElement>() => _elements.Keys.OfType<TElement>();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ElementInfo> GetEnumerator() => _elements.Values.GetEnumerator();

    public Model Build()
    {
        foreach (var relationsFactory in _relationFactories)
        foreach (var relation in relationsFactory(this))
            Add(relation);

        foreach (var traitFactory in _traitFactories)
        foreach (var trait in traitFactory(this))
            Add(trait);

        return new Model(system,
            [.._elements.Keys.OrderBy(e => e.GetType().FullName).ThenBy(e => e.Name)],
            [.._relations.OrderBy(r => r.GetType().Name)],
            [.._traits.OrderBy(t => t.GetType().Name)]);
    }
}