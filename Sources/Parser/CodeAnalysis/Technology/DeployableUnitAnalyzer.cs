using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class DeployableUnitAnalyzer : SymbolAnalyzer<IAssemblySymbol>
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(DeployableUnitAttribute), out var deployableUnitAttribute))
            return;
        var name = deployableUnitAttribute.GetConstructorArgumentValue<string>(nameof(DeployableUnitAttribute.Name));
        var deployableUnit = new DeployableUnit(name);
        modelBuilder.Add(deployableUnit, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, deployableUnit, elements));
    }

    private static IEnumerable<Relation> GetRelations(IAssemblySymbol symbol, DeployableUnit deployableUnit,
        ElementsProvider elements)
    {
        var namespaceSymbols = symbol
            .Modules
            // TODO: better way to select assemblies from analyzed solution
            .SelectMany(m => m.ReferencedAssemblySymbols)
            .Where(a => a.Locations.Length > 0 && a.Locations[0].IsInSource)
            .Select(a => a.GlobalNamespace)
            .SelectRecursively(n => n.GetNamespaceMembers())
            // TODO: Check why INamespaceSymbol instances in elements and in query are different.
            .SelectMany(n => elements
                .Where(s => s is INamespaceSymbol ns && ns.Name.Equals(n.Name, StringComparison.InvariantCulture)))
            .OfType<DomainModule>()
            .Distinct()
            .Select(domainModule => new DomainModule.IsDeployedInDeployableUnit(domainModule, deployableUnit));
        return namespaceSymbols;
    }
}