using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;

namespace P3Model.Parser.CodeAnalysis.Domain;

public static class DomainModelResolver
{
    private static readonly ConcurrentDictionary<INamespaceSymbol, Result> Cache =
        new(SymbolEqualityComparer.Default);

    [PublicAPI]
    public static bool IsExplicitlyIncludedInDomainModel(this ISymbol symbol) =>
        GetResult(symbol) == Result.IsIncluded;

    [PublicAPI]
    public static bool IsExplicitlyExcludedFromDomainModel(this ISymbol symbol) =>
        GetResult(symbol) == Result.IsExcluded;

    [PublicAPI]
    public static bool IsExplicitlyIncludedInDomainModel(this INamespaceSymbol namespaceSymbol) =>
        GetResult(namespaceSymbol) == Result.IsIncluded;

    [PublicAPI]
    public static bool IsExplicitlyExcludedFromDomainModel(this INamespaceSymbol namespaceSymbol) =>
        GetResult(namespaceSymbol) == Result.IsExcluded;

    private static Result GetResult(this ISymbol symbol)
    {
        if (symbol is INamespaceSymbol namespaceSymbol)
            return namespaceSymbol.GetResult();
        if (symbol.TryGetAttribute(typeof(NotDomainModelAttribute), out var attributeData) &&
            (!attributeData.TryGetNamedArgumentValue<bool>(
                    nameof(NotDomainModelAttribute.ApplyOnNamespace),
                    out var applyOnNamespace) ||
                !applyOnNamespace))
            return Result.IsExcluded;
        if (symbol.TryGetAttribute(typeof(DomainModelAttribute), out attributeData) &&
            (!attributeData.TryGetNamedArgumentValue(
                    nameof(NotDomainModelAttribute.ApplyOnNamespace),
                    out applyOnNamespace) ||
                !applyOnNamespace))
            return Result.IsIncluded;
        namespaceSymbol = symbol.ContainingNamespace;
        return namespaceSymbol.GetResult();
    }

    private static Result GetResult(this INamespaceSymbol namespaceSymbol)
    {
        if (Cache.TryGetValue(namespaceSymbol, out var cachedResult))
            return cachedResult;
        var currentSymbol = namespaceSymbol;
        while (currentSymbol is { IsGlobalNamespace: false })
        {
            if (currentSymbol.TryGetAttribute(typeof(NotDomainModelAttribute), out _))
            {
                Cache.TryAdd(namespaceSymbol, Result.IsExcluded);
                return Result.IsExcluded;
            }
            if (currentSymbol.TryGetAttribute(typeof(DomainModelAttribute), out _))
            {
                Cache.TryAdd(namespaceSymbol, Result.IsIncluded);
                return Result.IsIncluded;
            }
            currentSymbol = currentSymbol.ContainingNamespace;
        }
        if (namespaceSymbol.TryGetAssemblyAttribute(typeof(DomainModelAttribute), out _))
        {
            Cache.TryAdd(namespaceSymbol, Result.IsIncluded);
            return Result.IsIncluded;
        }
        Cache.TryAdd(namespaceSymbol, Result.Unspecified);
        return Result.Unspecified;
    }

    private enum Result
    {
        Unspecified,
        IsIncluded,
        IsExcluded
    }
}