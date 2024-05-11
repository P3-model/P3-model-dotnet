using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis.Domain;

public class NamespaceBasedDomainModulesHierarchyResolver(ImmutableArray<string> partsToSkip)
    : DomainModulesHierarchyResolver
{
    [PublicAPI]
    public bool TryFind(ISymbol symbol, [NotNullWhen(true)] out HierarchyPath? hierarchyPath)
    {
        hierarchyPath = null;
        return symbol switch
        {
            INamespaceSymbol namespaceSymbol => TryFind(namespaceSymbol, out hierarchyPath),
            INamedTypeSymbol typeSymbol => TryFind(typeSymbol.ContainingNamespace, out hierarchyPath),
            IMethodSymbol methodSymbol => TryFind(methodSymbol.ContainingType.ContainingNamespace, out hierarchyPath),
            _ => false
        };
    }

    [PublicAPI]
    public bool TryFind(INamespaceSymbol symbol, [NotNullWhen(true)] out HierarchyPath? hierarchyPath)
    {
        var parts = new List<string>();
        while (!symbol.IsGlobalNamespace)
        {
            if (!IsSkippedNamespace(symbol))
                parts.Add(symbol.Name);
            symbol = symbol.ContainingNamespace;
        }
        if (parts.Count == 0)
        {
            hierarchyPath = null;
            return false;
        }
        parts.Reverse();
        hierarchyPath = HierarchyPath.FromParts(parts);
        return true;
    }

    private bool IsSkippedNamespace(INamespaceSymbol symbol) => 
        symbol.TryGetAttribute(typeof(SkipNamespaceInDomainModulesHierarchyAttribute), out _) || 
        partsToSkip.Any(partToSkip => symbol.Name.Equals(partToSkip, StringComparison.OrdinalIgnoreCase));
}