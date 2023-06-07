using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.People;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.People;

public class ActorAnalyzer : SymbolAnalyzer<INamedTypeSymbol>, SymbolAnalyzer<IMethodSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    public void Analyze(IMethodSymbol symbol, ModelBuilder modelBuilder) => Analyze((ISymbol)symbol, modelBuilder);

    private static void Analyze(ISymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ActorAttribute), out var actorAttribute))
            return;
        var name = actorAttribute.GetConstructorParameterValue<string>();
        var actor = new Actor(name);
        modelBuilder.Add(actor, symbol);
        modelBuilder.Add(elements => GetRelations(symbol, actor, elements));
    }
    
    private static IEnumerable<Relation> GetRelations(ISymbol symbol, Actor actor, ElementsProvider elements)
    {
        var product = elements.OfType<Product>().Single();
        yield return new Actor.UsesProduct(actor, product);
        foreach (var processStep in elements.For(symbol).OfType<ProcessStep>())
            yield return new Actor.UsesProcessStep(actor, processStep);
    }
}