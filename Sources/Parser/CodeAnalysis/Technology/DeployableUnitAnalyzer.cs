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
        ElementsProvider elements) => symbol
        .GetReferencedAssembliesFromSameRepository()
        .SelectRecursively(assemblySymbol => assemblySymbol.GetReferencedAssembliesFromSameRepository())
        .Distinct(SymbolEqualityComparer.Default)
        .SelectMany(assemblySymbol => elements
            .OfType<ElementInfo<ModelBoundary>>()
            .Where(elementInfo => AreRelated(elementInfo, assemblySymbol)))
        .Distinct()
        .Select(elementInfo => new ModelBoundary.IsDeployedInDeployableUnit(elementInfo.Element, deployableUnit));

    private static bool AreRelated(ElementInfo elementInfo, ISymbol symbol) =>
        elementInfo.Symbols.Contains(symbol) ||
        symbol.SourcesAreIn(elementInfo.Directories);
}