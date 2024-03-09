using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public class NamespaceBasedDomainModulesHierarchyResolver(
    Func<INamespaceSymbol, string> getModulesHierarchy)
    : DomainModulesHierarchyResolver
{
    [PublicAPI]
    public bool TryFind(ISymbol symbol, [NotNullWhen(true)] out HierarchyId? hierarchyId)
    {
        hierarchyId = null;
        return symbol switch
        {
            INamespaceSymbol namespaceSymbol => TryFind(namespaceSymbol, out hierarchyId),
            INamedTypeSymbol typeSymbol => TryFind(typeSymbol.ContainingNamespace, out hierarchyId),
            IMethodSymbol methodSymbol => TryFind(methodSymbol.ContainingType.ContainingNamespace, out hierarchyId),
            _ => false
        };
    }
    
    [PublicAPI]
    public bool TryFind(INamespaceSymbol symbol, [NotNullWhen(true)] out HierarchyId? hierarchyId)
    {
        if (symbol.IsGlobalNamespace)
        {
            hierarchyId = null;
            return false;
        }
        var modulesHierarchy = getModulesHierarchy(symbol);
        if (string.IsNullOrEmpty(modulesHierarchy))
        {
            hierarchyId = null;
            return false;
        }
        hierarchyId = HierarchyId.FromValue(modulesHierarchy);
        return true;
    }
}