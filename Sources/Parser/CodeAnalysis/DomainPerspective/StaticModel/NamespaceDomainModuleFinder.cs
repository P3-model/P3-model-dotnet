using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

public class NamespaceDomainModuleFinder : DomainModuleFinder
{
    private readonly Func<INamespaceSymbol, bool> _isDomainModelNamespace;
    private readonly Func<INamespaceSymbol, string> _getModulesHierarchy;

    public NamespaceDomainModuleFinder(Func<INamespaceSymbol, bool> isDomainModelNamespace,
        Func<INamespaceSymbol, string> getModulesHierarchy)
    {
        _isDomainModelNamespace = isDomainModelNamespace;
        _getModulesHierarchy = getModulesHierarchy;
    }

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
        if (symbol.IsGlobalNamespace || !_isDomainModelNamespace(symbol))
        {
            module = null;
            return false;
        }
        var modulesHierarchy = _getModulesHierarchy(symbol);
        if (string.IsNullOrEmpty(modulesHierarchy))
        {
            module = null;
            return false;
        }
        module = new DomainModule(HierarchyId.FromValue(modulesHierarchy));
        return true;
    }
}