using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.Domain;

public static class DomainModelResolver
{
    private static readonly ConcurrentDictionary<INamespaceSymbol, bool?> Cache = new();

    public static bool BelongsToDomainModel(this ISymbol symbol)
    {
        if (symbol.TryGetAttribute(typeof(NotDomainModelAttribute), out _))
            return false;
        if (symbol.TryGetAttribute(typeof(DomainModelAttribute), out _))
            return true;
        var namespaceSymbol = symbol as INamespaceSymbol ?? symbol.ContainingNamespace;
        var isNamespaceDomainModel = IsNamespaceDomainModel(namespaceSymbol);
        if (isNamespaceDomainModel != null)
            return isNamespaceDomainModel.Value;
        return symbol.TryGetAssemblyAttribute(typeof(DomainModelAttribute), out _);
    }

    public static bool BelongsToDomainModel(this ISymbol symbol,
        Type attributeType,
        TypeAttributeSources sources,
        [NotNullWhen(true)] out AttributeData? attribute)
    {
        if (!typeof(DomainPerspectiveAttribute).IsAssignableFrom(attributeType))
        {
            Log.Warning("Not domain perspective attribute {attributeType} used to check domain model element",
                attributeType);
            attribute = null;
            return false;
        }
        if (symbol.IsAnnotatedDirectly(attributeType, out attribute))
            return true;
        if (symbol is not ITypeSymbol typeSymbol)
        {
            attribute = null;
            return false;
        }
        if (!typeSymbol.IsAnnotatedIndirectly(attributeType,
                sources,
                out attribute))
            return false;
        if (symbol.TryGetAttribute(typeof(NotDomainModelAttribute), out _))
            return false;
        if (symbol.TryGetAttribute(typeof(DomainModelAttribute), out _))
            return true;
        var namespaceSymbol = symbol as INamespaceSymbol ?? symbol.ContainingNamespace;
        var isNamespaceDomainModel = IsNamespaceDomainModel(namespaceSymbol);
        if (isNamespaceDomainModel != null)
            return isNamespaceDomainModel.Value;
        return symbol.TryGetAssemblyAttribute(typeof(DomainModelAttribute), out _);
    }

    private static bool IsAnnotatedDirectly(this ISymbol symbol, Type attributeType,
        [NotNullWhen(true)] out AttributeData? attribute) =>
        symbol.TryGetAttribute(attributeType, out attribute);

    private static bool IsAnnotatedIndirectly(this ITypeSymbol symbol, Type attributeType,
        TypeAttributeSources sources,
        [NotNullWhen(true)] out AttributeData? attribute) =>
        symbol.TryGetAttribute(attributeType, sources, out attribute, out _);

    private static bool? IsNamespaceDomainModel(INamespaceSymbol namespaceSymbol)
    {
        if (Cache.TryGetValue(namespaceSymbol, out var cachedResult))
            return cachedResult;
        while (namespaceSymbol is { IsGlobalNamespace: false })
        {
            if (namespaceSymbol.TryGetAttribute(typeof(NotDomainModelAttribute), out _))
            {
                Cache.TryAdd(namespaceSymbol, false);
                return false;
            }
            if (namespaceSymbol.TryGetAttribute(typeof(DomainModelAttribute), out _))
            {
                Cache.TryAdd(namespaceSymbol, true);
                return true;
            }
            namespaceSymbol = namespaceSymbol.ContainingNamespace;
        }
        Cache.TryAdd(namespaceSymbol, null);
        return null;
    }
}