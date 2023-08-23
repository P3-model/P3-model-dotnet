using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

[UsedImplicitly]
public class DomainBuildingBlocksDependenciesAnalyzer : SymbolAnalyzer<IFieldSymbol>,
    SymbolAnalyzer<IPropertySymbol>,
    SymbolAnalyzer<IParameterSymbol>,
    SymbolAnalyzer<ILocalSymbol>
{
    public void Analyze(IFieldSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = symbol.Type;
        modelBuilder.Add(elements => AddRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IPropertySymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = symbol.Type;
        modelBuilder.Add(elements => AddRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(IParameterSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = symbol.Type;
        modelBuilder.Add(elements => AddRelations(sourceSymbol, destinationSymbol, elements));
    }

    public void Analyze(ILocalSymbol symbol, ModelBuilder modelBuilder)
    {
        var sourceSymbol = symbol.ContainingType;
        var destinationSymbol = symbol.Type;
        modelBuilder.Add(elements => AddRelations(sourceSymbol, destinationSymbol, elements));
    }

    private static IEnumerable<Relation> AddRelations(ISymbol sourceSymbol, ISymbol destinationSymbol,
        ElementsProvider elements)
    {
        var sources = elements.For(sourceSymbol).OfType<DomainBuildingBlock>();
        var destinations = elements.For(destinationSymbol).OfType<DomainBuildingBlock>().ToList();
        foreach (var source in sources)
        foreach (var destination in destinations.Where(destination => !source.Equals(destination)))
            yield return CreateRelation(source, destination);
    }

    private static DomainBuildingBlock.DependsOnBuildingBlock CreateRelation(DomainBuildingBlock source,
        DomainBuildingBlock destination) => source switch
    {
        ProcessStep processStep => new ProcessStep.DependsOnBuildingBlock(processStep, destination),
        _ => new DomainBuildingBlock.DependsOnBuildingBlock(source, destination)
    };
}