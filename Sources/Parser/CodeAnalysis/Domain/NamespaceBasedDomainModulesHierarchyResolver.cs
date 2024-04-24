using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis.Domain;

public class NamespaceBasedDomainModulesHierarchyResolver(
    Func<INamespaceSymbol, string> getModulesHierarchy)
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
        if (symbol.IsGlobalNamespace)
        {
            hierarchyPath = null;
            return false;
        }
        var modulesHierarchy = getModulesHierarchy(symbol);
        if (string.IsNullOrEmpty(modulesHierarchy))
        {
            hierarchyPath = null;
            return false;
        }
        hierarchyPath = HierarchyPath.FromValue(modulesHierarchy);
        return true;
    }
}