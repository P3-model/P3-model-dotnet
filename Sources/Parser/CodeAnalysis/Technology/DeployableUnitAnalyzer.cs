using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

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
        modelBuilder.Add(elements => GetRelationsToDomainModules(symbol, deployableUnit, elements));
        modelBuilder.Add(elements => GetRelationsToCodeStructures(symbol, deployableUnit, elements));
        modelBuilder.Add(elements => GetRelationsToDatabases(symbol, deployableUnit, elements));
    }

    private static IEnumerable<Relation> GetRelationsToDomainModules(IAssemblySymbol symbol,
        DeployableUnit deployableUnit, ElementsProvider elements) => symbol
        .GetReferencedAssembliesFromSameRepository()
        .SelectRecursively(assemblySymbol => assemblySymbol.GetReferencedAssembliesFromSameRepository())
        .Distinct<IAssemblySymbol>(CompilationIndependentSymbolEqualityComparer.Default)
        .SelectMany(assemblySymbol => elements
            .OfType<ElementInfo<DomainModule>>()
            .Where(info => info.Symbols.Contains(assemblySymbol) ||
                           info.Symbols.Any(s => s.IsFrom(assemblySymbol)) ||
                           info.Directories.Any(d => d.ContainsSourcesOf(assemblySymbol)))
            .Select(info => new DomainModule.IsDeployedInDeployableUnit(info.Element, deployableUnit)));

    private static IEnumerable<Relation> GetRelationsToCodeStructures(ISymbol symbol,
        DeployableUnit deployableUnit, ElementsProvider elements) => elements
        .OfType<ElementInfo<CSharpProject>>()
        .Where(info => info.Symbols.Contains(symbol))
        .Select(elementInfo => new DeployableUnit.ContainsCSharpProject(deployableUnit, elementInfo.Element));

    private static IEnumerable<Relation> GetRelationsToDatabases(IAssemblySymbol symbol,
        DeployableUnit deployableUnit, ElementsProvider elements) => symbol
        .GetReferencedAssembliesFromSameRepository()
        .SelectRecursively(assemblySymbol => assemblySymbol.GetReferencedAssembliesFromSameRepository())
        .Distinct<IAssemblySymbol>(CompilationIndependentSymbolEqualityComparer.Default)
        .SelectMany(assemblySymbol => elements
            .OfType<ElementInfo<Database>>()
            .Where(info => info.Symbols.Any(s => s.IsFrom(assemblySymbol))))
        .Select(info => new DeployableUnit.UsesDatabase(deployableUnit, info.Element));
}