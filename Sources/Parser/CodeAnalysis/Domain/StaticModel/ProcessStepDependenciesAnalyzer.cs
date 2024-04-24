using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel;

[UsedImplicitly]
public class UseCaseDependenciesAnalyzer : SymbolAnalyzer<IFieldSymbol>,
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
        var sources = elements.For(sourceSymbol).OfType<UseCase>();
        var destinations = elements.For(destinationSymbol).OfType<DomainBuildingBlock>().ToList();
        foreach (var source in sources)
        foreach (var destination in destinations)
            yield return new UseCase.DependsOnBuildingBlock(source, destination);
    }
}