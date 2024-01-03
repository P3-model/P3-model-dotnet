using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;

namespace P3Model.Parser.Configuration.Analyzers;

public class NamespaceOptionsBuilder
{
    private readonly List<Func<INamespaceSymbol, bool>> _namespacePredicates = new()
    {
        symbol => !AnnotatedWith<NotDomainModuleAttribute>(symbol)
    };
    private readonly List<Func<string, string>> _namespaceFilters = new();

    [PublicAPI]
    public NamespaceOptionsBuilder OnlyFromAssembliesAnnotatedWith<TAttribute>() => Matching(
        symbol => symbol.ConstituentNamespaces
            .Any(ns => ns.ContainingAssembly.TryGetAttribute(typeof(TAttribute), out _)));

    [PublicAPI]
    public NamespaceOptionsBuilder Matching(string pattern) => Matching(
        symbol => Regex.IsMatch(symbol.ToDisplayString(), pattern));

    [PublicAPI]
    public NamespaceOptionsBuilder Exclude(string pattern) => Matching(
        symbol => !Regex.IsMatch(symbol.ToDisplayString(), pattern));

    [PublicAPI]
    public NamespaceOptionsBuilder ExcludeAnnotatedWith<TAttribute>(Func<AttributeData, bool>? predicate = null)
        where TAttribute : NamespaceApplicable =>
        Matching(symbol => !AnnotatedWith<TAttribute>(symbol, predicate));

    private static bool AnnotatedWith<TAttribute>(INamespaceSymbol symbol,
        Func<AttributeData, bool>? predicate = null)
        where TAttribute : NamespaceApplicable
    {
        while (symbol is { IsGlobalNamespace: false })
        {
            if (symbol
                .GetTypeMembers()
                .Any(typeSymbol => typeSymbol.TryGetAttribute(typeof(TAttribute), out var attribute) &&
                                   attribute.TryGetArgumentValue(nameof(NamespaceApplicable.ApplyOnNamespace),
                                       out bool applyOnNamespace) &&
                                   applyOnNamespace &&
                                   (predicate is null || predicate(attribute))))
                return true;
            symbol = symbol.ContainingNamespace;
        }
        return false;
    }

    [PublicAPI]
    public NamespaceOptionsBuilder Matching(Func<INamespaceSymbol, bool> predicate)
    {
        _namespacePredicates.Add(predicate);
        return this;
    }

    [PublicAPI]
    public NamespaceOptionsBuilder RemoveRootNamespace(string rootNamespace) => Filter(currentHierarchy =>
        currentHierarchy.Equals(rootNamespace)
            ? string.Empty
            : currentHierarchy.StartsWith(rootNamespace)
                ? currentHierarchy[(rootNamespace.Length + 1)..]
                : currentHierarchy);

    [PublicAPI]
    public NamespaceOptionsBuilder RemoveNamespacePart(params string[] layerNames) => Filter(initialHierarchy =>
        layerNames.Aggregate(initialHierarchy,
            (currentHierarchy, layerName) =>
            {
                if (currentHierarchy.Equals(layerName))
                    return string.Empty;
                var index = currentHierarchy.IndexOf(layerName, StringComparison.InvariantCulture);
                return index switch
                {
                    -1 => currentHierarchy,
                    0 => currentHierarchy[(layerName.Length + 1)..],
                    _ => currentHierarchy.Remove(index - 1, layerName.Length + 1)
                };
            }));

    private NamespaceOptionsBuilder Filter(Func<string, string> filter)
    {
        _namespaceFilters.Add(filter);
        return this;
    }

    public NamespaceOptions Build() => new(
        symbol => _namespacePredicates.All(predicate => predicate(symbol)),
        symbol => _namespaceFilters.Aggregate(
            symbol.ToDisplayString(),
            (currentHierarchy, filter) => filter(currentHierarchy)));
}