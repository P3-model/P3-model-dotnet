﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.ModelSyntax;

public class ModelBuilder : ElementsProvider
{
    private readonly ConcurrentDictionary<Element, HashSet<ISymbol>> _elementToSymbols = new();
    private readonly ConcurrentDictionary<ISymbol, HashSet<Element>> _symbolToElements =
        new(SymbolEqualityComparer.Default);
    private readonly ConcurrentDictionary<Relation, byte> _relations = new();
    private readonly ConcurrentDictionary<Func<ElementsProvider, IEnumerable<Relation>>, byte> _relationFactories = new();
    private readonly ConcurrentDictionary<Trait, byte> _traits = new();
    private readonly ConcurrentDictionary<Func<ElementsProvider, IEnumerable<Trait>>, byte> _traitFactories = new();

    public void Add(Element element, ISymbol symbol)
    {
        _elementToSymbols.AddOrUpdate(element,
            _ => new HashSet<ISymbol>(SymbolEqualityComparer.Default) { symbol },
            (_, symbols) =>
            {
                symbols.Add(symbol);
                return symbols;
            });
        _symbolToElements.AddOrUpdate(symbol,
            _ => new HashSet<Element> { element },
            (_, elements) =>
            {
                elements.Add(element);
                return elements;
            });
    }

    public void Add(Relation relation) => _relations.TryAdd(relation, default);

    public void Add(Func<ElementsProvider, IEnumerable<Relation>> relationFactory) =>
        _relationFactories.TryAdd(relationFactory, default);
    
    public void Add(Trait trait) => _traits.TryAdd(trait, default);
    
    public void Add(Func<ElementsProvider, IEnumerable<Trait>> traitFactory) =>
        _traitFactories.TryAdd(traitFactory, default);

    public IEnumerable<Element> For(ISymbol symbol) => _symbolToElements.TryGetValue(symbol, out var elements) 
        ? elements 
        : Enumerable.Empty<Element>();

    public IEnumerable<Element> Where(Func<ISymbol, bool> predicate) => _symbolToElements
        .Where(pair => predicate(pair.Key))
        .SelectMany(pair => pair.Value);

    public Model Build()
    {
        foreach (var relationsFactory in _relationFactories.Keys)
        foreach (var relation in relationsFactory(this))
            Add(relation);

        return new Model(_elementToSymbols.Keys.ToImmutableArray(),
            _relations.Keys.ToImmutableArray());
    }
}