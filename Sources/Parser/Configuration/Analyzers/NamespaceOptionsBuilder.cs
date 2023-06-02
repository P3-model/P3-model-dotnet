using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.Configuration.Analyzers;

public class NamespaceOptionsBuilder
{
    private readonly List<Func<INamespaceSymbol, bool>> _namespacePredicates = new();
    private readonly List<Func<INamespaceSymbol, string, string>> _namespaceFilters = new();

    // TODO: support for same namespace declared in multiple assemblies
    // [PublicAPI]
    // public NamespaceOptionsBuilder UseOnlyAssembliesAnnotatedWith(Type attributeType) => Matching(
    //     symbol => symbol.ContainingAssembly.TryGetAttribute(attributeType, out _));

    [PublicAPI]
    public NamespaceOptionsBuilder Matching(string pattern) => Matching(
        symbol => Regex.IsMatch(symbol.ToDisplayString(), pattern));

    [PublicAPI]
    public NamespaceOptionsBuilder Exclude(string pattern) => Matching(
        symbol => !Regex.IsMatch(symbol.ToDisplayString(), pattern));

    [PublicAPI]
    public NamespaceOptionsBuilder Matching(Func<INamespaceSymbol, bool> predicate)
    {
        _namespacePredicates.Add(predicate);
        return this;
    }

    [PublicAPI]
    public NamespaceOptionsBuilder RemoveRootNamespace(string rootNamespace) => Filter((_, currentHierarchy) =>
        currentHierarchy.Equals(rootNamespace)
            ? string.Empty
            : currentHierarchy.StartsWith(rootNamespace)
                ? currentHierarchy[(rootNamespace.Length + 1)..]
                : currentHierarchy);

    [PublicAPI]
    public NamespaceOptionsBuilder RemoveLayerName(params string[] layerNames) => Filter((_, initialHierarchy) =>
        layerNames.Aggregate(initialHierarchy,
            (currentHierarchy, layerName) =>
            {
                var index = currentHierarchy.IndexOf(layerName, StringComparison.InvariantCulture);
                return index switch
                {
                    -1 => currentHierarchy,
                    0 => currentHierarchy[(layerName.Length + 1)..],
                    _ => currentHierarchy.Remove(index - 1, currentHierarchy.Length + 1)
                };
            }));

    [PublicAPI]
    public NamespaceOptionsBuilder Filter(Func<INamespaceSymbol, string, string> filter)
    {
        _namespaceFilters.Add(filter);
        return this;
    }

    public NamespaceOptions Build() => new(
        symbol => _namespacePredicates.All(predicate => predicate(symbol)),
        symbol => _namespaceFilters.Aggregate(
            symbol.ToDisplayString(),
            (currentHierarchy, filter) => filter(symbol, currentHierarchy)));
}