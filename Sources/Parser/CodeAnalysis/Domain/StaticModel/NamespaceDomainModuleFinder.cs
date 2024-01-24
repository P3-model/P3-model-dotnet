using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

public class NamespaceDomainModuleFinder(
    Func<INamespaceSymbol, bool> isDomainModelNamespace,
    Func<INamespaceSymbol, string> getModulesHierarchy)
    : DomainModuleFinder
{
    public bool TryFind(ISymbol symbol, [NotNullWhen(true)] out DomainModule? module)
    {
        module = null;
        return symbol switch
        {
            INamespaceSymbol namespaceSymbol => TryFind(namespaceSymbol, out module),
            INamedTypeSymbol typeSymbol => TryFind(typeSymbol.ContainingNamespace, out module),
            IMethodSymbol methodSymbol => TryFind(methodSymbol.ContainingType.ContainingNamespace, out module),
            _ => false
        };
    }

    public bool TryFind(INamespaceSymbol symbol, [NotNullWhen(true)] out DomainModule? module)
    {
        if (symbol.IsGlobalNamespace || !isDomainModelNamespace(symbol))
        {
            module = null;
            return false;
        }
        var modulesHierarchy = getModulesHierarchy(symbol);
        if (string.IsNullOrEmpty(modulesHierarchy))
        {
            module = null;
            return false;
        }
        module = new DomainModule(HierarchyId.FromValue(modulesHierarchy));
        return true;
    }
}