using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

public class DomainModuleAnalyzer : SymbolAnalyzer<INamespaceSymbol>
{
    private readonly Func<INamespaceSymbol, bool> _isDomainModelNamespace;
    private readonly Func<INamespaceSymbol, string> _getModulesHierarchy;

    public DomainModuleAnalyzer(Func<INamespaceSymbol, bool> isDomainModelNamespace,
        Func<INamespaceSymbol, string> getModulesHierarchy)
    {
        _isDomainModelNamespace = isDomainModelNamespace;
        _getModulesHierarchy = getModulesHierarchy;
    }

    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!_isDomainModelNamespace(symbol) || symbol.IsGlobalNamespace)
            return;
        var modulesHierarchy = _getModulesHierarchy(symbol);
        if (string.IsNullOrEmpty(modulesHierarchy))
            return;
        var module = new DomainModule(new HierarchyId(modulesHierarchy));
        modelBuilder.Add(module, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, module, elements));
        var containingNamespace = symbol.ContainingNamespace;
        if (containingNamespace != null)
            Analyze(containingNamespace, modelBuilder);
    }

    private static IEnumerable<Relation> GetRelations(INamespaceSymbol symbol, DomainModule module,
        ElementsProvider elements) => symbol
        .GetNamespaceMembers()
        .Select(childSymbol => elements
            .For(childSymbol)
            .OfType<DomainModule>()
            .SingleOrDefault())
        .Where(childModule => childModule != null)
        .Select(childModule => new DomainModule.ContainsDomainModule(module, childModule!));
}