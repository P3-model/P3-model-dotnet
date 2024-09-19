using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;

namespace P3Model.Parser.CodeAnalysis.Domain;

public static class DomainModelResolver
{
    private static readonly ConcurrentDictionary<INamespaceSymbol, SymbolDomainModelStatus> Cache =
        new(SymbolEqualityComparer.Default);

    [PublicAPI]
    public static bool IsExplicitlyIncludedInDomainModel(this ISymbol symbol) =>
        CheckStatus(symbol) == SymbolDomainModelStatus.Included;

    [PublicAPI]
    public static bool IsExplicitlyExcludedFromDomainModel(this ISymbol symbol) =>
        CheckStatus(symbol) == SymbolDomainModelStatus.Excluded;

    [PublicAPI]
    public static bool IsExplicitlyIncludedInDomainModel(this INamespaceSymbol namespaceSymbol) =>
        CheckStatus(namespaceSymbol) == SymbolDomainModelStatus.Included;

    [PublicAPI]
    public static bool IsExplicitlyExcludedFromDomainModel(this INamespaceSymbol namespaceSymbol) =>
        CheckStatus(namespaceSymbol) == SymbolDomainModelStatus.Excluded;

    private static SymbolDomainModelStatus CheckStatus(this ISymbol symbol)
    {
        if (symbol is INamespaceSymbol namespaceSymbol)
            return namespaceSymbol.CheckStatus();
        if (symbol.TryGetAttribute(typeof(NotDomainModelAttribute), out var attributeData) &&
            (!attributeData.TryGetNamedArgumentValue<bool>(
                    nameof(NotDomainModelAttribute.ApplyOnNamespace),
                    out var applyOnNamespace) ||
                !applyOnNamespace))
            return SymbolDomainModelStatus.Excluded;
        if (symbol.TryGetAttribute(typeof(DomainModelAttribute), out attributeData) &&
            (!attributeData.TryGetNamedArgumentValue(
                    nameof(NotDomainModelAttribute.ApplyOnNamespace),
                    out applyOnNamespace) ||
                !applyOnNamespace))
            return SymbolDomainModelStatus.Included;
        namespaceSymbol = symbol.ContainingNamespace;
        return namespaceSymbol.CheckStatus();
    }

    private static SymbolDomainModelStatus CheckStatus(this INamespaceSymbol namespaceSymbol)
    {
        if (Cache.TryGetValue(namespaceSymbol, out var cachedResult))
            return cachedResult;
        var currentSymbol = namespaceSymbol;
        while (currentSymbol is { IsGlobalNamespace: false })
        {
            if (currentSymbol.TryGetAttribute(typeof(NotDomainModelAttribute), out _))
            {
                Cache.TryAdd(namespaceSymbol, SymbolDomainModelStatus.Excluded);
                return SymbolDomainModelStatus.Excluded;
            }
            if (currentSymbol.TryGetAttribute(typeof(DomainModelAttribute), out _))
            {
                Cache.TryAdd(namespaceSymbol, SymbolDomainModelStatus.Included);
                return SymbolDomainModelStatus.Included;
            }
            currentSymbol = currentSymbol.ContainingNamespace;
        }
        if (namespaceSymbol.TryGetAssemblyAttribute(typeof(DomainModelAttribute), out _))
        {
            Cache.TryAdd(namespaceSymbol, SymbolDomainModelStatus.Included);
            return SymbolDomainModelStatus.Included;
        }
        Cache.TryAdd(namespaceSymbol, SymbolDomainModelStatus.Unspecified);
        return SymbolDomainModelStatus.Unspecified;
    }

    private enum SymbolDomainModelStatus
    {
        Unspecified,
        Included,
        Excluded
    }
}