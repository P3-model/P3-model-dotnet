﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.ModelSyntax;

[SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1024",
    Justification = "See CompilationIndependentSymbolEqualityComparer")]
public class ModelBuilder : ElementsProvider
{
    private readonly DocumentedSystem _system;

    private readonly ConcurrentDictionary<Element, ElementInfo> _elements = new();
    private readonly ConcurrentDictionary<ISymbol, ConcurrentSet<Element>> _symbolToElements =
        new(CompilationIndependentSymbolEqualityComparer.Default);

    private readonly ConcurrentSet<Relation> _relations = new();
    private readonly ConcurrentSet<Func<ElementsProvider, IEnumerable<Relation>>> _relationFactories =
        new();

    private readonly ConcurrentSet<Trait> _traits = new();
    private readonly ConcurrentSet<Func<ElementsProvider, IEnumerable<Trait>>> _traitFactories = new();

    public ModelBuilder(DocumentedSystem system) => _system = system;

    [PublicAPI]
    public void Add<TElement>(TElement element) where TElement : class, Element =>
        _elements.TryAdd(element, new ElementInfo<TElement>(element));

    [PublicAPI]
    public void Add<TElement>(TElement element, ISymbol symbol)
        where TElement : class, Element
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
        where TElement : class, Element
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

        return new Model(_system,
            _elements.Keys.ToImmutableArray(),
            _relations.ToImmutableArray(),
            _traits.ToImmutableArray());
    }
}